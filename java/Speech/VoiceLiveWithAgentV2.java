// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import com.azure.ai.voicelive.VoiceLiveAsyncClient;
import com.azure.ai.voicelive.VoiceLiveClientBuilder;
import com.azure.ai.voicelive.VoiceLiveSessionAsyncClient;
import com.azure.ai.voicelive.models.AgentSessionConfig;
import com.azure.ai.voicelive.models.ClientEventConversationItemCreate;
import com.azure.ai.voicelive.models.ClientEventResponseCancel;
import com.azure.ai.voicelive.models.ClientEventResponseCreate;
import com.azure.ai.voicelive.models.ClientEventSessionUpdate;
import com.azure.ai.voicelive.models.ConversationRequestItem;
import com.azure.ai.voicelive.models.InputAudioFormat;
import com.azure.ai.voicelive.models.InputTextContentPart;
import com.azure.ai.voicelive.models.InteractionModality;
import com.azure.ai.voicelive.models.InterimResponseTrigger;
import com.azure.ai.voicelive.models.LlmInterimResponseConfig;
import com.azure.ai.voicelive.models.MessageContentPart;
import com.azure.ai.voicelive.models.OutputAudioFormat;
import com.azure.ai.voicelive.models.ServerEventType;
import com.azure.ai.voicelive.models.SessionUpdate;
import com.azure.ai.voicelive.models.SessionUpdateError;
import com.azure.ai.voicelive.models.SessionUpdateResponseAudioDelta;
import com.azure.ai.voicelive.models.SystemMessageItem;
import com.azure.ai.voicelive.models.VoiceLiveSessionOptions;
import com.azure.core.util.BinaryData;
import com.azure.identity.AzureCliCredentialBuilder;

import javax.sound.sampled.AudioFormat;
import javax.sound.sampled.AudioSystem;
import javax.sound.sampled.DataLine;
import javax.sound.sampled.LineUnavailableException;
import javax.sound.sampled.SourceDataLine;
import javax.sound.sampled.TargetDataLine;
import java.io.FileWriter;
import java.io.IOException;
import java.io.PrintWriter;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.Arrays;
import java.util.concurrent.BlockingQueue;
import java.util.concurrent.CountDownLatch;
import java.util.concurrent.LinkedBlockingQueue;
import java.util.concurrent.atomic.AtomicBoolean;
import java.util.concurrent.atomic.AtomicInteger;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 * Voice assistant using Azure AI Voice Live SDK with Foundry Agent support.
 *
 * <p>This sample demonstrates:</p>
 * <ul>
 *   <li>Connecting to Voice Live with AgentSessionConfig</li>
 *   <li>Configuring interim responses to bridge latency gaps</li>
 *   <li>Proactive greeting message on session start</li>
 *   <li>Real-time audio capture and playback with barge-in support</li>
 *   <li>Conversation logging to a file</li>
 * </ul>
 *
 * <p>Required environment variables:</p>
 * <ul>
 *   <li>VOICELIVE_ENDPOINT - Voice Live service endpoint</li>
 *   <li>AGENT_NAME - Name of the Foundry agent</li>
 *   <li>PROJECT_NAME - Foundry project name (e.g., myproject)</li>
 * </ul>
 *
 * <p>Optional environment variables:</p>
 * <ul>
 *   <li>VOICE_NAME - Voice name (default: en-US-Ava:DragonHDLatestNeural)</li>
 *   <li>AGENT_VERSION - Specific agent version</li>
 *   <li>CONVERSATION_ID - Resume a previous conversation</li>
 *   <li>FOUNDRY_RESOURCE_OVERRIDE - Cross-resource Foundry endpoint</li>
 *   <li>AGENT_AUTHENTICATION_IDENTITY_CLIENT_ID - Managed identity client ID for cross-resource auth</li>
 * </ul>
 */
// <all>
public class VoiceLiveWithAgentV2 {

    private static final Logger logger = Logger.getLogger(VoiceLiveWithAgentV2.class.getName());

    // Audio configuration: 24 kHz, 16-bit, mono PCM
    private static final float SAMPLE_RATE = 24000;
    private static final int SAMPLE_SIZE_BITS = 16;
    private static final int CHANNELS = 1;
    private static final int FRAME_SIZE = 2; // 16-bit mono = 2 bytes per frame
    private static final int BUFFER_SIZE = 4800; // 100ms at 24kHz

    // Conversation log
    private static final String LOG_FILENAME = "conversation_"
            + LocalDateTime.now().format(DateTimeFormatter.ofPattern("yyyyMMdd_HHmmss")) + ".log";

    // <audio_processor>
    /**
     * Manages real-time audio capture from the microphone and playback to the speakers.
     * Uses a blocking queue for audio buffering and supports barge-in (skip pending audio).
     */
    static class AudioProcessor {
        private final AudioFormat format;
        private TargetDataLine captureLine;
        private SourceDataLine playbackLine;
        private final BlockingQueue<byte[]> playbackQueue = new LinkedBlockingQueue<>();
        private final AtomicBoolean capturing = new AtomicBoolean(false);
        private final AtomicBoolean playing = new AtomicBoolean(false);
        private final AtomicInteger nextSeqNum = new AtomicInteger(0);
        private volatile int playbackBase = 0;
        private Thread captureThread;
        private Thread playbackThread;
        private final VoiceLiveSessionAsyncClient session;

        AudioProcessor(VoiceLiveSessionAsyncClient session) {
            this.session = session;
            this.format = new AudioFormat(SAMPLE_RATE, SAMPLE_SIZE_BITS, CHANNELS, true, false);
        }

        void startCapture() throws LineUnavailableException {
            if (capturing.get()) {
                return;
            }

            DataLine.Info info = new DataLine.Info(TargetDataLine.class, format);
            if (!AudioSystem.isLineSupported(info)) {
                throw new LineUnavailableException("Microphone not available");
            }

            captureLine = (TargetDataLine) AudioSystem.getLine(info);
            captureLine.open(format, BUFFER_SIZE * FRAME_SIZE);
            captureLine.start();
            capturing.set(true);

            captureThread = new Thread(() -> {
                byte[] buffer = new byte[BUFFER_SIZE * FRAME_SIZE];
                while (capturing.get()) {
                    int bytesRead = captureLine.read(buffer, 0, buffer.length);
                    if (bytesRead > 0) {
                        byte[] audioChunk = Arrays.copyOf(buffer, bytesRead);
                        try {
                            session.sendInputAudio(BinaryData.fromBytes(audioChunk)).block();
                        } catch (Exception e) {
                            if (capturing.get()) {
                                logger.warning("Audio send failed: " + e.getMessage());
                            }
                        }
                    }
                }
            }, "audio-capture");
            captureThread.setDaemon(true);
            captureThread.start();
            logger.info("Started audio capture");
        }

        void startPlayback() throws LineUnavailableException {
            if (playing.get()) {
                return;
            }

            DataLine.Info info = new DataLine.Info(SourceDataLine.class, format);
            if (!AudioSystem.isLineSupported(info)) {
                throw new LineUnavailableException("Speakers not available");
            }

            playbackLine = (SourceDataLine) AudioSystem.getLine(info);
            playbackLine.open(format, BUFFER_SIZE * FRAME_SIZE);
            playbackLine.start();
            playing.set(true);

            playbackThread = new Thread(() -> {
                while (playing.get()) {
                    try {
                        byte[] data = playbackQueue.take();
                        if (data.length == 0) {
                            // Poison pill to stop playback
                            break;
                        }
                        playbackLine.write(data, 0, data.length);
                    } catch (InterruptedException e) {
                        Thread.currentThread().interrupt();
                        break;
                    }
                }
            }, "audio-playback");
            playbackThread.setDaemon(true);
            playbackThread.start();
            logger.info("Audio playback system ready");
        }

        void queueAudio(byte[] audioData) {
            int seqNum = nextSeqNum.getAndIncrement();
            if (seqNum >= playbackBase) {
                playbackQueue.offer(audioData);
            }
        }

        void skipPendingAudio() {
            playbackBase = nextSeqNum.getAndIncrement();
            playbackQueue.clear();
            if (playbackLine != null) {
                playbackLine.flush();
            }
        }

        void shutdown() {
            capturing.set(false);
            playing.set(false);

            if (captureLine != null) {
                captureLine.stop();
                captureLine.close();
                logger.info("Stopped audio capture");
            }

            skipPendingAudio();
            playbackQueue.offer(new byte[0]); // poison pill
            if (playbackLine != null) {
                playbackLine.drain();
                playbackLine.stop();
                playbackLine.close();
                logger.info("Stopped audio playback");
            }
            logger.info("Audio processor cleaned up");
        }
    }
    // </audio_processor>

    // <voice_assistant>
    /**
     * Voice assistant that connects to a Foundry Agent via the Voice Live service.
     * Handles session lifecycle, event processing, and audio I/O.
     */
    static class BasicVoiceAssistant {
        private final String endpoint;
        private final AgentSessionConfig agentConfig;
        private VoiceLiveSessionAsyncClient session;
        private AudioProcessor audioProcessor;
        private boolean sessionReady = false;
        private boolean greetingSent = false;
        private boolean activeResponse = false;
        private boolean responseApiDone = false;

        // <agent_config>
        BasicVoiceAssistant(String endpoint, String agentName, String projectName,
                            String agentVersion, String conversationId,
                            String foundryResourceOverride, String authIdentityClientId) {
            this.endpoint = endpoint;

            // Build the agent session configuration
            AgentSessionConfig config = new AgentSessionConfig(agentName, projectName);
            if (agentVersion != null && !agentVersion.isEmpty()) {
                config.setAgentVersion(agentVersion);
            }
            if (conversationId != null && !conversationId.isEmpty()) {
                config.setConversationId(conversationId);
            }
            if (foundryResourceOverride != null && !foundryResourceOverride.isEmpty()) {
                config.setFoundryResourceOverride(foundryResourceOverride);
                if (authIdentityClientId != null && !authIdentityClientId.isEmpty()) {
                    config.setAuthenticationIdentityClientId(authIdentityClientId);
                }
            }
            this.agentConfig = config;
        }
        // </agent_config>

        // <start_session>
        void start() throws Exception {
            logger.info("Connecting to VoiceLive API with agent config...");

            // Create the Voice Live async client with Entra ID authentication
            VoiceLiveAsyncClient client = new VoiceLiveClientBuilder()
                    .endpoint(endpoint)
                    .credential(new AzureCliCredentialBuilder().build())
                    .buildAsyncClient();

            // Connect using AgentSessionConfig
            session = client.startSession(agentConfig).block();
            if (session == null) {
                throw new RuntimeException("Failed to start Voice Live session");
            }

            try {
                audioProcessor = new AudioProcessor(session);
                setupSession();
                audioProcessor.startPlayback();

                logger.info("Voice assistant ready! Start speaking...");
                System.out.println();
                System.out.println("=".repeat(65));
                System.out.println("🎤 VOICE ASSISTANT READY");
                System.out.println("Start speaking to begin conversation");
                System.out.println("Press Ctrl+C to exit");
                System.out.println("=".repeat(65));
                System.out.println();

                // Process events (blocking)
                processEvents();
            } finally {
                if (audioProcessor != null) {
                    audioProcessor.shutdown();
                }
                if (session != null) {
                    session.closeAsync().block();
                }
            }
        }
        // </start_session>

        // <setup_session>
        private void setupSession() {
            logger.info("Setting up voice conversation session...");

            // Configure interim responses to bridge latency gaps during processing
            LlmInterimResponseConfig interimResponseConfig = new LlmInterimResponseConfig()
                    .setTriggers(Arrays.asList(
                            InterimResponseTrigger.TOOL,
                            InterimResponseTrigger.LATENCY))
                    .setLatencyThresholdMs(100)
                    .setInstructions("Create friendly interim responses indicating wait time due to "
                            + "ongoing processing, if any. Do not include in all responses! Do not "
                            + "say you don't have real-time access to information when calling tools!");

            // Create session configuration
            VoiceLiveSessionOptions sessionOptions = new VoiceLiveSessionOptions()
                    .setModalities(Arrays.asList(InteractionModality.TEXT, InteractionModality.AUDIO))
                    .setInputAudioFormat(InputAudioFormat.PCM16)
                    .setOutputAudioFormat(OutputAudioFormat.PCM16)
                    .setInterimResponse(BinaryData.fromObject(interimResponseConfig));

            // Send session update
            session.sendEvent(new ClientEventSessionUpdate(sessionOptions)).block();
            logger.info("Session configuration sent");
        }
        // </setup_session>

        // <process_events>
        private void processEvents() throws InterruptedException {
            CountDownLatch latch = new CountDownLatch(1);

            session.receiveEvents().subscribe(
                    event -> handleEvent(event),
                    error -> {
                        logger.log(Level.SEVERE, "Error processing events", error);
                        latch.countDown();
                    },
                    () -> {
                        logger.info("Event stream completed");
                        latch.countDown();
                    }
            );

            latch.await();
        }
        // </process_events>

        // <handle_events>
        private void handleEvent(SessionUpdate event) {
            ServerEventType type = event.getType();
            logger.fine("Received event: " + type);

            if (type == ServerEventType.SESSION_UPDATED) {
                logger.info("Session updated and ready");
                sessionReady = true;
                String sessionId = extractField(event, "id");
                writeLog(String.format("SessionID: %s\n", sessionId));

                // Send a proactive greeting
                if (!greetingSent) {
                    greetingSent = true;
                    sendProactiveGreeting();
                }

                // Start audio capture once session is ready
                try {
                    audioProcessor.startCapture();
                } catch (LineUnavailableException e) {
                    logger.log(Level.SEVERE, "Failed to start audio capture", e);
                }

            } else if (type == ServerEventType.CONVERSATION_ITEM_INPUT_AUDIO_TRANSCRIPTION_COMPLETED) {
                String transcript = extractField(event, "transcript");
                System.out.println("👤 You said:\t" + transcript);
                writeLog("User Input:\t" + transcript);

            } else if (type == ServerEventType.RESPONSE_AUDIO_TRANSCRIPT_DONE) {
                String transcript = extractField(event, "transcript");
                System.out.println("🤖 Agent responded:\t" + transcript);
                writeLog("Agent Audio Response:\t" + transcript);

            } else if (type == ServerEventType.INPUT_AUDIO_BUFFER_SPEECH_STARTED) {
                logger.info("User started speaking - stopping playback");
                System.out.println("🎤 Listening...");
                audioProcessor.skipPendingAudio();

                // Cancel in-progress response for barge-in
                if (activeResponse && !responseApiDone) {
                    try {
                        session.sendEvent(new ClientEventResponseCancel()).block();
                        logger.fine("Cancelled in-progress response due to barge-in");
                    } catch (Exception e) {
                        if (e.getMessage() != null && e.getMessage().toLowerCase().contains("no active response")) {
                            logger.fine("Cancel ignored - response already completed");
                        } else {
                            logger.warning("Cancel failed: " + e.getMessage());
                        }
                    }
                }

            } else if (type == ServerEventType.INPUT_AUDIO_BUFFER_SPEECH_STOPPED) {
                logger.info("User stopped speaking");
                System.out.println("🤔 Processing...");

            } else if (type == ServerEventType.RESPONSE_CREATED) {
                logger.info("Assistant response created");
                activeResponse = true;
                responseApiDone = false;

            } else if (type == ServerEventType.RESPONSE_AUDIO_DELTA) {
                logger.fine("Received audio delta");
                SessionUpdateResponseAudioDelta audioDelta = (SessionUpdateResponseAudioDelta) event;
                byte[] audioData = audioDelta.getDelta();
                if (audioData != null && audioData.length > 0) {
                    audioProcessor.queueAudio(audioData);
                }

            } else if (type == ServerEventType.RESPONSE_AUDIO_DONE) {
                logger.info("Assistant finished speaking");
                System.out.println("🎤 Ready for next input...");

            } else if (type == ServerEventType.RESPONSE_DONE) {
                logger.info("Response complete");
                activeResponse = false;
                responseApiDone = true;

            } else if (type == ServerEventType.ERROR) {
                SessionUpdateError errorEvent = (SessionUpdateError) event;
                String msg = errorEvent.getError().getMessage();
                if (msg != null && msg.contains("Cancellation failed: no active response")) {
                    logger.fine("Benign cancellation error: " + msg);
                } else {
                    logger.severe("VoiceLive error: " + msg);
                    System.out.println("Error: " + msg);
                }

            } else {
                logger.fine("Unhandled event type: " + type);
            }
        }
        // </handle_events>

        // <proactive_greeting>
        private void sendProactiveGreeting() {
            logger.info("Sending proactive greeting request");
            try {
                // Create a system message to trigger greeting
                SystemMessageItem greetingMessage = new SystemMessageItem(
                        Arrays.asList(new InputTextContentPart("Say something to welcome the user.")));
                ClientEventConversationItemCreate createEvent = new ClientEventConversationItemCreate()
                        .setItem(greetingMessage);
                session.sendEvent(createEvent).block();

                // Request a response
                session.sendEvent(new ClientEventResponseCreate()).block();
            } catch (Exception e) {
                logger.log(Level.WARNING, "Failed to send proactive greeting", e);
            }
        }
        // </proactive_greeting>

        private void writeLog(String message) {
            try {
                Path logDir = Paths.get("logs");
                Files.createDirectories(logDir);
                try (PrintWriter writer = new PrintWriter(
                        new FileWriter(logDir.resolve(LOG_FILENAME).toString(), true))) {
                    writer.println(message);
                }
            } catch (IOException e) {
                logger.warning("Failed to write conversation log: " + e.getMessage());
            }
        }

        /**
         * Extracts a string field value from a SessionUpdate event's JSON representation.
         */
        private String extractField(SessionUpdate event, String fieldName) {
            try {
                String json = event.toJsonString();
                // Simple extraction: find "fieldName":"value"
                String key = "\"" + fieldName + "\":\"";
                int start = json.indexOf(key);
                if (start >= 0) {
                    start += key.length();
                    int end = json.indexOf("\"", start);
                    if (end >= 0) {
                        return json.substring(start, end);
                    }
                }
            } catch (IOException ignored) { }
            return "";
        }
    }
    // </voice_assistant>

    // <main>
    public static void main(String[] args) {
        String endpoint = System.getenv("VOICELIVE_ENDPOINT");
        String agentName = System.getenv("AGENT_NAME");
        String projectName = System.getenv("PROJECT_NAME");
        String agentVersion = System.getenv("AGENT_VERSION");
        String conversationId = System.getenv("CONVERSATION_ID");
        String foundryResourceOverride = System.getenv("FOUNDRY_RESOURCE_OVERRIDE");
        String authIdentityClientId = System.getenv("AGENT_AUTHENTICATION_IDENTITY_CLIENT_ID");

        System.out.println("Environment variables:");
        System.out.println("VOICELIVE_ENDPOINT: " + endpoint);
        System.out.println("AGENT_NAME: " + agentName);
        System.out.println("PROJECT_NAME: " + projectName);
        System.out.println("AGENT_VERSION: " + agentVersion);
        System.out.println("CONVERSATION_ID: " + conversationId);
        System.out.println("FOUNDRY_RESOURCE_OVERRIDE: " + foundryResourceOverride);

        if (endpoint == null || endpoint.isEmpty()
                || agentName == null || agentName.isEmpty()
                || projectName == null || projectName.isEmpty()) {
            System.err.println("Set VOICELIVE_ENDPOINT, AGENT_NAME, and PROJECT_NAME environment variables.");
            System.exit(1);
        }

        // Verify audio devices
        checkAudioDevices();

        System.out.println("🎙️ Basic Foundry Voice Agent with Azure VoiceLive SDK (Agent Mode)");
        System.out.println("=".repeat(65));

        BasicVoiceAssistant assistant = new BasicVoiceAssistant(
                endpoint, agentName, projectName,
                agentVersion, conversationId,
                foundryResourceOverride, authIdentityClientId);

        // Handle graceful shutdown
        Runtime.getRuntime().addShutdownHook(new Thread(() -> {
            System.out.println("\n👋 Voice assistant shut down. Goodbye!");
        }));

        try {
            assistant.start();
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
            System.out.println("\n👋 Voice assistant shut down. Goodbye!");
        } catch (Exception e) {
            System.err.println("Fatal Error: " + e.getMessage());
            e.printStackTrace();
        }
    }
    // </main>

    // <check_audio>
    private static void checkAudioDevices() {
        AudioFormat format = new AudioFormat(SAMPLE_RATE, SAMPLE_SIZE_BITS, CHANNELS, true, false);
        DataLine.Info captureInfo = new DataLine.Info(TargetDataLine.class, format);
        DataLine.Info playbackInfo = new DataLine.Info(SourceDataLine.class, format);

        if (!AudioSystem.isLineSupported(captureInfo)) {
            System.err.println("❌ No audio input devices found. Please check your microphone.");
            System.exit(1);
        }
        if (!AudioSystem.isLineSupported(playbackInfo)) {
            System.err.println("❌ No audio output devices found. Please check your speakers.");
            System.exit(1);
        }
    }
    // </check_audio>
}
// </all>
