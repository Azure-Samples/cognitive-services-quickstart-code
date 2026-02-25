from __future__ import annotations
import os
import sys
import asyncio
import base64
from datetime import datetime
import logging
import queue
import signal
from typing import Any, Union, Optional, TYPE_CHECKING, cast

from azure.core.credentials import AzureKeyCredential
from azure.core.credentials_async import AsyncTokenCredential
from azure.identity.aio import AzureCliCredential

from azure.ai.voicelive.aio import connect, AgentSessionConfig
from azure.ai.voicelive.models import (
    InputAudioFormat,
    Modality,
    OutputAudioFormat,
    RequestSession,
    ServerEventType,
    MessageItem,
    InputTextContentPart,
    LlmInterimResponseConfig,
    InterimResponseTrigger,
    AzureStandardVoice,
    AudioNoiseReduction,
    AudioEchoCancellation,
    AzureSemanticVadMultilingual
)
from dotenv import load_dotenv
import pyaudio

if TYPE_CHECKING:
    # Only needed for type checking; avoids runtime import issues
    from azure.ai.voicelive.aio import VoiceLiveConnection

# Environment variable loading
_script_dir = os.path.dirname(os.path.abspath(__file__))
load_dotenv(os.path.join(_script_dir, './.env'), override=True)

# Set up logging
## Add folder for logging
os.makedirs(os.path.join(_script_dir, 'logs'), exist_ok=True)

## Add timestamp for logfiles
timestamp = datetime.now().strftime("%Y-%m-%d_%H-%M-%S")

## Create conversation log filename
logfilename = f"{timestamp}_conversation.log"

## Set up logging
logging.basicConfig(
    filename=os.path.join(_script_dir, 'logs', f'{timestamp}_voicelive.log'),
    filemode="w",
    format='%(asctime)s:%(name)s:%(levelname)s:%(message)s',
    level=logging.INFO
)
logger = logging.getLogger(__name__)

class AudioProcessor:
    """
    Handles real-time audio capture and playback for the voice assistant.

    Threading Architecture:
    - Main thread: Event loop and UI
    - Capture thread: PyAudio input stream reading
    - Send thread: Async audio data transmission to VoiceLive
    - Playback thread: PyAudio output stream writing
    """
    
    loop: asyncio.AbstractEventLoop
    
    class AudioPlaybackPacket:
        """Represents a packet that can be sent to the audio playback queue."""
        def __init__(self, seq_num: int, data: Optional[bytes]):
            self.seq_num = seq_num
            self.data = data

    def __init__(self, connection: VoiceLiveConnection) -> None:
        self.connection = connection
        self.audio = pyaudio.PyAudio()

        # Audio configuration - PCM16, 24kHz, mono as specified
        self.format = pyaudio.paInt16
        self.channels = 1
        self.rate = 24000
        self.chunk_size = 1200 # 50ms

        # Capture and playback state
        self.input_stream = None

        self.playback_queue: queue.Queue[AudioProcessor.AudioPlaybackPacket] = queue.Queue()
        self.playback_base = 0
        self.next_seq_num = 0
        self.output_stream: Optional[pyaudio.Stream] = None

        logger.info("AudioProcessor initialized with 24kHz PCM16 mono audio")

    def start_capture(self) -> None:
        """Start capturing audio from microphone."""
        def _capture_callback(
            in_data,      # data
            _frame_count,  # number of frames
            _time_info,    # dictionary
            _status_flags):
            """Audio capture thread - runs in background."""
            audio_base64 = base64.b64encode(in_data).decode("utf-8")
            asyncio.run_coroutine_threadsafe(
                self.connection.input_audio_buffer.append(audio=audio_base64), self.loop
            )
            return (None, pyaudio.paContinue)

        if self.input_stream:
            return

        # Store the current event loop for use in threads
        self.loop = asyncio.get_event_loop()

        try:
            self.input_stream = self.audio.open(
                format=self.format,
                channels=self.channels,
                rate=self.rate,
                input=True,
                frames_per_buffer=self.chunk_size,
                stream_callback=_capture_callback,
            )
            logger.info("Started audio capture")

        except Exception:
            logger.exception("Failed to start audio capture")
            raise

    def start_playback(self) -> None:
        """Initialize audio playback system."""
        if self.output_stream:
            return

        remaining = bytes()
        def _playback_callback(
            _in_data,
            frame_count,  # number of frames
            _time_info,
            _status_flags):

            nonlocal remaining
            frame_count *= pyaudio.get_sample_size(pyaudio.paInt16)

            out = remaining[:frame_count]
            remaining = remaining[frame_count:]

            while len(out) < frame_count:
                try:
                    packet = self.playback_queue.get_nowait()
                except queue.Empty:
                    out = out + bytes(frame_count - len(out))
                    continue
                except Exception:
                    logger.exception("Error in audio playback")
                    raise

                if not packet or not packet.data:
                    # None packet indicates end of stream
                    logger.info("End of playback queue.")
                    break

                if packet.seq_num < self.playback_base:
                    # skip requested
                    # ignore skipped packet and clear remaining
                    if len(remaining) > 0:
                        remaining = bytes()
                    continue

                num_to_take = frame_count - len(out)
                out = out + packet.data[:num_to_take]
                remaining = packet.data[num_to_take:]

            if len(out) >= frame_count:
                return (out, pyaudio.paContinue)
            else:
                return (out, pyaudio.paComplete)

        try:
            self.output_stream = self.audio.open(
                format=self.format,
                channels=self.channels,
                rate=self.rate,
                output=True,
                frames_per_buffer=self.chunk_size,
                stream_callback=_playback_callback
            )
            logger.info("Audio playback system ready")
        except Exception:
            logger.exception("Failed to initialize audio playback")
            raise

    def _get_and_increase_seq_num(self) -> int:
        seq = self.next_seq_num
        self.next_seq_num += 1
        return seq

    def queue_audio(self, audio_data: Optional[bytes]) -> None:
        """Queue audio data for playback."""
        self.playback_queue.put(
            AudioProcessor.AudioPlaybackPacket(
                seq_num=self._get_and_increase_seq_num(),
                data=audio_data))

    def skip_pending_audio(self) -> None:
        """Skip current audio in playback queue."""
        self.playback_base = self._get_and_increase_seq_num()

    def shutdown(self) -> None:
        """Clean up audio resources."""
        if self.input_stream:
            self.input_stream.stop_stream()
            self.input_stream.close()
            self.input_stream = None

        logger.info("Stopped audio capture")

        # Inform thread to complete
        if self.output_stream:
            self.skip_pending_audio()
            self.queue_audio(None)
            self.output_stream.stop_stream()
            self.output_stream.close()
            self.output_stream = None

        logger.info("Stopped audio playback")

        if self.audio:
            self.audio.terminate()

        logger.info("Audio processor cleaned up")

class BasicVoiceAssistant:
    """
    Basic voice assistant implementing the VoiceLive SDK patterns with Foundry Agent.
    
    Uses the new AgentSessionConfig for strongly-typed agent configuration at connection time.
    This sample also demonstrates how to collect a conversation log of user and agent interactions.
    """

    def __init__(
        self,
        endpoint: str,
        credential: Union[AzureKeyCredential, AsyncTokenCredential],
        voice: str,
        agent_name: str,
        project_name: str,
        agent_version: Optional[str] = None,
        conversation_id: Optional[str] = None,
        foundry_resource_override: Optional[str] = None,
        agent_authentication_identity_client_id: Optional[str] = None,
    ):
        self.endpoint = endpoint
        self.credential = credential
        self.voice = voice
        # Build AgentSessionConfig internally
        self.agent_config: AgentSessionConfig = {
            "agent_name": agent_name,
            "agent_version": agent_version if agent_version else None,
            "project_name": project_name,
            "conversation_id": conversation_id if conversation_id else None,
            "foundry_resource_override": foundry_resource_override if foundry_resource_override else None, 
            "authentication_identity_client_id": agent_authentication_identity_client_id if agent_authentication_identity_client_id and foundry_resource_override else None,                
        }        

        self.connection: Optional["VoiceLiveConnection"] = None
        self.audio_processor: Optional[AudioProcessor] = None
        self.session_ready = False
        self.greeting_sent = False
        self._active_response = False
        self._response_api_done = False

    async def start(self) -> None:
        """Start the voice assistant session."""
        try:
            logger.info(
                "Connecting to VoiceLive API with agent %s for project %s (version=%s, conversation_id=%s, foundry_override=%s, auth_identity=%s)",
                self.agent_config.get("agent_name"),
                self.agent_config.get("project_name"),
                self.agent_config.get("agent_version"),
                self.agent_config.get("conversation_id"),
                self.agent_config.get("foundry_resource_override"),
                self.agent_config.get("agent-authentication-identity-client-id")
            )

            # Connect using AgentSessionConfig (new SDK pattern)
            async with connect(
                endpoint=self.endpoint,
                credential=self.credential,
                api_version="2026-01-01-preview",
                agent_config=self.agent_config,
            ) as connection:
                conn = connection
                self.connection = conn

                # Initialize audio processor
                ap = AudioProcessor(conn)
                self.audio_processor = ap

                # Configure session for voice conversation
                await self._setup_session()

                # Start audio systems
                ap.start_playback()

                logger.info("Voice assistant ready! Start speaking...")
                print("\n" + "=" * 65)
                print("🎤 VOICE ASSISTANT READY")
                print("Start speaking to begin conversation")
                print("Press Ctrl+C to exit")
                print("=" * 65 + "\n")

                # Process events
                await self._process_events()
        finally:
            if self.audio_processor:
                self.audio_processor.shutdown()

    async def _setup_session(self) -> None:
        """Configure the VoiceLive session for audio conversation."""
        logger.info("Setting up voice conversation session...")

        # Set up interim response configuration to bridge latency gaps during processing
        interim_response_config = LlmInterimResponseConfig(
            triggers=[InterimResponseTrigger.TOOL, InterimResponseTrigger.LATENCY],
            latency_threshold_ms=100,
            instructions="""Create friendly interim responses indicating wait time due to ongoing processing, if any. Do not include
                            in all responses! Do not say you don't have real-time access to information when calling tools!"""
        )

        # Create session configuration
        session_config = RequestSession(
            modalities=[Modality.TEXT, Modality.AUDIO],
            input_audio_format=InputAudioFormat.PCM16,
            output_audio_format=OutputAudioFormat.PCM16,
            interim_response=interim_response_config,
            # Uncomment the following, if not stored with agent configuration on the service side
            # voice=AzureStandardVoice(name=self.voice),
            # turn_detection=AzureSemanticVadMultilingual(),
            # input_audio_echo_cancellation=AudioEchoCancellation(),
            # input_audio_noise_reduction=AudioNoiseReduction(type="azure_deep_noise_suppression")
        )

        conn = self.connection
        if conn is None:
            raise RuntimeError("Connection must be established before setting up session")
        await conn.session.update(session=session_config)

        logger.info("Session configuration sent")

    async def _process_events(self) -> None:
        """Process events from the VoiceLive connection."""
        try:
            conn = self.connection
            if conn is None:
                raise RuntimeError("Connection must be established before processing events")
            async for event in conn:
                await self._handle_event(event)
        except Exception:
            logger.exception("Error processing events")
            raise

    async def _handle_event(self, event: Any) -> None:
        """Handle different types of events from VoiceLive."""
        logger.debug("Received event: %s", event.type)
        ap = self.audio_processor
        conn = self.connection
        if ap is None or conn is None:
            raise RuntimeError("AudioProcessor and Connection must be initialized")

        if event.type == ServerEventType.SESSION_UPDATED:
            logger.info("Session ready: %s", event.session.id)
            s, a, v = event.session, event.session.agent, event.session.voice
            await write_conversation_log("\n".join([
                f"SessionID: {s.id}", f"Agent Name: {a.name}",
                f"Agent Description: {a.description}", f"Agent ID: {a.agent_id}",
                f"Voice Name: {v['name']}", f"Voice Type: {v['type']}",
                f"Voice Temperature: {v['temperature']}", ""
            ]))
            self.session_ready = True

            # Invoke Proactive greeting
            if not self.greeting_sent:
                self.greeting_sent = True
                logger.info("Sending proactive greeting request")
                try:
                    await conn.conversation.item.create(
                        item=MessageItem(
                            role="system",
                            content=[
                                InputTextContentPart(
                                    text="Say something to welcome the user."
                                )
                            ]
                        )
                    )
                    await conn.response.create()
                except Exception:
                    logger.exception("Failed to send proactive greeting request")

            # Start audio capture once session is ready
            ap.start_capture()

        elif event.type == ServerEventType.CONVERSATION_ITEM_INPUT_AUDIO_TRANSCRIPTION_COMPLETED:
            print(f'👤 You said:\t{event.get("transcript", "")}')
            await write_conversation_log(f'User Input:\t{event.get("transcript", "")}')

        elif event.type == ServerEventType.RESPONSE_TEXT_DONE:
            print(f'🤖 Agent responded with text:\t{event.get("text", "")}')
            await write_conversation_log(f'Agent Text Response:\t{event.get("text", "")}')

        elif event.type == ServerEventType.RESPONSE_AUDIO_TRANSCRIPT_DONE:
            print(f'🤖 Agent responded with audio transcript:\t{event.get("transcript", "")}')
            await write_conversation_log(f'Agent Audio Response:\t{event.get("transcript", "")}')

        elif event.type == ServerEventType.INPUT_AUDIO_BUFFER_SPEECH_STARTED:
            logger.info("User started speaking - stopping playback")
            print("🎤 Listening...")

            ap.skip_pending_audio()

            # Only cancel if response is active and not already done
            if self._active_response and not self._response_api_done:
                try:
                    await conn.response.cancel()
                    logger.debug("Cancelled in-progress response due to barge-in")
                except Exception as e:
                    if "no active response" in str(e).lower():
                        logger.debug("Cancel ignored - response already completed")
                    else:
                        logger.warning("Cancel failed: %s", e)

        elif event.type == ServerEventType.INPUT_AUDIO_BUFFER_SPEECH_STOPPED:
            logger.info("🎤 User stopped speaking")
            print("🤔 Processing...")

        elif event.type == ServerEventType.RESPONSE_CREATED:
            logger.info("🤖 Assistant response created")
            self._active_response = True
            self._response_api_done = False

        elif event.type == ServerEventType.RESPONSE_AUDIO_DELTA:
            logger.debug("Received audio delta")
            ap.queue_audio(event.delta)

        elif event.type == ServerEventType.RESPONSE_AUDIO_DONE:
            logger.info("🤖 Assistant finished speaking")
            print("🎤 Ready for next input...")

        elif event.type == ServerEventType.RESPONSE_DONE:
            logger.info("✅ Response complete")
            self._active_response = False
            self._response_api_done = True

        elif event.type == ServerEventType.ERROR:
            msg = event.error.message
            if "Cancellation failed: no active response" in msg:
                logger.debug("Benign cancellation error: %s", msg)
            else:
                logger.error("❌ VoiceLive error: %s", msg)
                print(f"Error: {msg}")

        elif event.type == ServerEventType.CONVERSATION_ITEM_CREATED:
            logger.debug("Conversation item created: %s", event.item.id)

        else:
            logger.debug("Unhandled event type: %s", event.type)

async def write_conversation_log(message: str) -> None:
    """Write a message to the conversation log."""
    log_path = os.path.join(_script_dir, 'logs', logfilename)
    await asyncio.to_thread(
        lambda: open(log_path, 'a', encoding='utf-8').write(message + "\n")
    )

def main() -> None:
    """Main function."""
    endpoint = os.environ.get("VOICELIVE_ENDPOINT", "")
    voice_name = os.environ.get("VOICE_NAME", "en-US-Ava:DragonHDLatestNeural")
    agent_name = os.environ.get("AGENT_NAME", "")
    agent_version = os.environ.get("AGENT_VERSION")
    project_name = os.environ.get("PROJECT_NAME", "")
    conversation_id = os.environ.get("CONVERSATION_ID")
    foundry_resource_override = os.environ.get("FOUNDRY_RESOURCE_OVERRIDE")
    agent_authentication_identity_client_id = os.environ.get("AGENT_AUTHENTICATION_IDENTITY_CLIENT_ID")

    print("Environment variables:")
    print(f"VOICELIVE_ENDPOINT: {endpoint}")
    print(f"VOICE_NAME: {voice_name}")
    print(f"AGENT_NAME: {agent_name}")
    print(f"AGENT_VERSION: {agent_version}")
    print(f"PROJECT_NAME: {project_name}")
    print(f"CONVERSATION_ID: {conversation_id}")
    print(f"FOUNDRY_RESOURCE_OVERRIDE: {foundry_resource_override}")
    print(f"AGENT_AUTHENTICATION_IDENTITY_CLIENT_ID: {agent_authentication_identity_client_id}")

    if not endpoint or not agent_name or not project_name:
        sys.exit("Set VOICELIVE_ENDPOINT, AGENT_NAME, and PROJECT_NAME in your .env file.")

    # Create client with appropriate credential (Entra ID required for Agent mode)
    credential = AzureCliCredential()
    logger.info("Using Azure token credential")

    # Create and start voice assistant
    assistant = BasicVoiceAssistant(
        endpoint=endpoint,
        credential=credential,
        voice=voice_name,
        agent_name=agent_name,
        agent_version=agent_version,
        project_name=project_name,
        conversation_id=conversation_id,
        foundry_resource_override=foundry_resource_override,
        agent_authentication_identity_client_id=agent_authentication_identity_client_id,
    )

    # Handle SIGTERM for graceful shutdown (SIGINT already raises KeyboardInterrupt)
    signal.signal(signal.SIGTERM, lambda *_: (_ for _ in ()).throw(KeyboardInterrupt()))

    # Start the assistant
    try:
        asyncio.run(assistant.start())
    except KeyboardInterrupt:
        print("\n👋 Voice assistant shut down. Goodbye!")
    except Exception as e:
        print("Fatal Error: ", e)

def _check_audio_devices() -> None:
    """Verify audio input/output devices are available."""
    p = pyaudio.PyAudio()
    try:
        def _has_channels(key):
            return any(
                cast(Union[int, float], p.get_device_info_by_index(i).get(key, 0) or 0) > 0
                for i in range(p.get_device_count())
            )
        if not _has_channels("maxInputChannels"):
            sys.exit("❌ No audio input devices found. Please check your microphone.")
        if not _has_channels("maxOutputChannels"):
            sys.exit("❌ No audio output devices found. Please check your speakers.")
    finally:
        p.terminate()

if __name__ == "__main__":
    try:
        _check_audio_devices()
    except SystemExit:
        raise
    except Exception as e:
        sys.exit(f"❌ Audio system check failed: {e}")

    print("🎙️ Basic Foundry Voice Agent with Azure VoiceLive SDK (Agent Mode)")
    print("=" * 65)
    main()
