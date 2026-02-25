// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// Voice Live with Foundry Agent Service v2 - Node.js Console Voice Assistant
// Uses @azure/ai-voicelive SDK with handler-based event subscription pattern.

import "dotenv/config";
import { VoiceLiveClient } from "@azure/ai-voicelive";
import { DefaultAzureCredential } from "@azure/identity";
import record from "node-record-lpcm16";
import Speaker from "speaker";
import { existsSync, mkdirSync, appendFileSync } from "node:fs";
import { join, dirname } from "node:path";
import { fileURLToPath } from "node:url";

const __dirname = dirname(fileURLToPath(import.meta.url));

// ---------------------------------------------------------------------------
// Logging and conversation log setup
// ---------------------------------------------------------------------------
const logsDir = join(__dirname, "logs");
if (!existsSync(logsDir)) mkdirSync(logsDir, { recursive: true });

const timestamp = new Date()
  .toISOString()
  .replace(/[:.]/g, "-")
  .replace("T", "_")
  .slice(0, 19);
const conversationLogFile = join(logsDir, `conversation_${timestamp}.log`);

function writeConversationLog(message) {
  appendFileSync(conversationLogFile, message + "\n", "utf-8");
}

// ---------------------------------------------------------------------------
// Audio helpers
// ---------------------------------------------------------------------------

/**
 * AudioProcessor manages microphone capture via node-record-lpcm16
 * and playback via the speaker npm package. Audio format: 24 kHz, 16-bit, mono.
 */
class AudioProcessor {
  constructor() {
    this._recorder = null;
    this._speaker = null;
    this._skipSeq = 0;
    this._nextSeq = 0;
  }

  /** Start capturing microphone audio and forward PCM chunks to the session. */
  startCapture(session) {
    if (this._recorder) return;

    this._recorder = record.record({
      sampleRate: 24000,
      channels: 1,
      audioType: "raw",
      recorder: "sox",
      encoding: "signed-integer",
      bitwidth: 16,
    });

    this._recorder.stream().on("data", (chunk) => {
      if (session.isConnected) {
        session.sendAudio(new Uint8Array(chunk)).catch(() => {
          /* ignore send errors after disconnect */
        });
      }
    });

    console.log("[audio] Microphone capture started");
  }

  /** Initialise the speaker for playback. */
  startPlayback() {
    if (this._speaker) return;
    this._resetSpeaker();
    console.log("[audio] Playback ready");
  }

  /** Queue a PCM16 buffer (base64 from service) for playback. */
  queueAudio(base64Delta) {
    const seq = this._nextSeq++;
    if (seq < this._skipSeq) return; // skip if barge-in happened
    const buf = Buffer.from(base64Delta, "base64");
    if (this._speaker && !this._speaker.destroyed) {
      this._speaker.write(buf);
    }
  }

  /** Discard queued audio (barge-in). */
  skipPendingAudio() {
    this._skipSeq = this._nextSeq++;
    // Reset speaker to flush its internal buffer
    this._resetSpeaker();
  }

  /** Shut down capture and playback. */
  shutdown() {
    if (this._recorder) {
      this._recorder.stop();
      this._recorder = null;
    }
    if (this._speaker) {
      this._speaker.end();
      this._speaker = null;
    }
    console.log("[audio] Audio processor shut down");
  }

  /** (Re-)create the Speaker instance. */
  _resetSpeaker() {
    if (this._speaker && !this._speaker.destroyed) {
      try {
        this._speaker.end();
      } catch {
        /* ignore */
      }
    }
    this._speaker = new Speaker({
      channels: 1,
      bitDepth: 16,
      sampleRate: 24000,
      signed: true,
    });
    // Swallow speaker errors (e.g. device busy after barge-in reset)
    this._speaker.on("error", () => {});
  }
}

// ---------------------------------------------------------------------------
// BasicVoiceAssistant
// ---------------------------------------------------------------------------
class BasicVoiceAssistant {
  /**
   * @param {object} opts
   * @param {string} opts.endpoint
   * @param {import("@azure/identity").TokenCredential} opts.credential
   * @param {string} opts.agentName
   * @param {string} opts.projectName
   * @param {string} [opts.agentVersion]
   * @param {string} [opts.conversationId]
   * @param {string} [opts.foundryResourceOverride]
   * @param {string} [opts.authenticationIdentityClientId]
   */
  constructor(opts) {
    this.endpoint = opts.endpoint;
    this.credential = opts.credential;
    this.agentConfig = {
      agentName: opts.agentName,
      projectName: opts.projectName,
      ...(opts.agentVersion && { agentVersion: opts.agentVersion }),
      ...(opts.conversationId && { conversationId: opts.conversationId }),
      ...(opts.foundryResourceOverride && {
        foundryResourceOverride: opts.foundryResourceOverride,
      }),
      ...(opts.foundryResourceOverride &&
        opts.authenticationIdentityClientId && {
          authenticationIdentityClientId: opts.authenticationIdentityClientId,
        }),
    };

    this._session = null;
    this._audio = new AudioProcessor();
    this._greetingSent = false;
    this._activeResponse = false;
    this._responseApiDone = false;
  }

  /** Connect, subscribe to events, and run until interrupted. */
  async start() {
    const client = new VoiceLiveClient(this.endpoint, this.credential);
    const session = client.createSession({ agent: this.agentConfig });
    this._session = session;

    console.log(
      `[init] Connecting to VoiceLive with agent "${this.agentConfig.agentName}" ` +
        `for project "${this.agentConfig.projectName}" ...`,
    );

    await session.connect();

    // Subscribe to VoiceLive events using the handler pattern
    const subscription = session.subscribe({
      onSessionUpdated: async (event, context) => {
        const s = event.session;
        const agent = s?.agent;
        const voice = s?.voice;
        console.log(`[session] Session ready: ${context.sessionId}`);
        writeConversationLog(
          [
            `SessionID: ${context.sessionId}`,
            `Agent Name: ${agent?.name ?? ""}`,
            `Agent Description: ${agent?.description ?? ""}`,
            `Agent ID: ${agent?.agentId ?? ""}`,
            `Voice Name: ${voice?.name ?? ""}`,
            `Voice Type: ${voice?.type ?? ""}`,
            "",
          ].join("\n"),
        );

        // Configure session for audio conversation
        await this._setupSession();

        // Proactive greeting
        if (!this._greetingSent) {
          this._greetingSent = true;
          console.log("[session] Sending proactive greeting ...");
          try {
            await session.addConversationItem({
              type: "message",
              role: "system",
              content: [
                { type: "input_text", text: "Say something to welcome the user." },
              ],
            });
            await session.sendEvent({ type: "response.create" });
          } catch (err) {
            console.error("[session] Failed to send greeting:", err.message);
          }
        }

        // Start microphone capture once session is configured
        this._audio.startPlayback();
        this._audio.startCapture(session);

        console.log("\n" + "=".repeat(65));
        console.log("🎤 VOICE ASSISTANT READY");
        console.log("Start speaking to begin conversation");
        console.log("Press Ctrl+C to exit");
        console.log("=".repeat(65) + "\n");
      },

      onConversationItemInputAudioTranscriptionCompleted: async (event) => {
        const transcript = event.transcript ?? "";
        console.log(`👤 You said:\t${transcript}`);
        writeConversationLog(`User Input:\t${transcript}`);
      },

      onResponseTextDone: async (event) => {
        const text = event.text ?? "";
        console.log(`🤖 Agent responded with text:\t${text}`);
        writeConversationLog(`Agent Text Response:\t${text}`);
      },

      onResponseAudioTranscriptDone: async (event) => {
        const transcript = event.transcript ?? "";
        console.log(`🤖 Agent responded with audio transcript:\t${transcript}`);
        writeConversationLog(`Agent Audio Response:\t${transcript}`);
      },

      onInputAudioBufferSpeechStarted: async () => {
        console.log("🎤 Listening...");
        this._audio.skipPendingAudio();

        // Cancel in-progress response (barge-in)
        if (this._activeResponse && !this._responseApiDone) {
          try {
            await session.sendEvent({ type: "response.cancel" });
          } catch (err) {
            const msg = err?.message ?? "";
            if (!msg.toLowerCase().includes("no active response")) {
              console.warn("[barge-in] Cancel failed:", msg);
            }
          }
        }
      },

      onInputAudioBufferSpeechStopped: async () => {
        console.log("🤔 Processing...");
      },

      onResponseCreated: async () => {
        this._activeResponse = true;
        this._responseApiDone = false;
      },

      onResponseAudioDelta: async (event) => {
        if (event.delta) {
          this._audio.queueAudio(event.delta);
        }
      },

      onResponseAudioDone: async () => {
        console.log("🎤 Ready for next input...");
      },

      onResponseDone: async () => {
        console.log("✅ Response complete");
        this._activeResponse = false;
        this._responseApiDone = true;
      },

      onServerError: async (event) => {
        const msg = event.error?.message ?? "";
        if (msg.includes("Cancellation failed: no active response")) {
          // Benign – ignore
          return;
        }
        console.error(`❌ VoiceLive error: ${msg}`);
      },

      onConversationItemCreated: async (event) => {
        console.log(`[event] Conversation item created: ${event.item?.id ?? ""}`);
      },
    });

    // Keep the process alive until disconnect or Ctrl+C
    await new Promise((resolve) => {
      const onSigint = () => {
        resolve();
      };
      process.once("SIGINT", onSigint);
      process.once("SIGTERM", onSigint);

      // Also resolve if subscription closes (e.g. server-side disconnect)
      const poll = setInterval(() => {
        if (!session.isConnected) {
          clearInterval(poll);
          resolve();
        }
      }, 500);
    });

    // Cleanup
    await subscription.close();
    await session.disconnect();
    this._audio.shutdown();
    await session.dispose();
  }

  /** Configure session modalities, audio format, and interim response. */
  async _setupSession() {
    console.log("[session] Configuring session ...");
    await this._session.updateSession({
      modalities: ["text", "audio"],
      inputAudioFormat: "pcm16",
      outputAudioFormat: "pcm16",
      interimResponse: {
        type: "llm_interim_response",
        triggers: ["tool", "latency"],
        latencyThresholdInMs: 100,
        instructions:
          "Create friendly interim responses indicating wait time due to ongoing processing, if any. " +
          "Do not include in all responses! Do not say you don't have real-time access to information when calling tools!",
      },
    });
    console.log("[session] Session configuration sent");
  }
}

// ---------------------------------------------------------------------------
// Main
// ---------------------------------------------------------------------------
async function main() {
  const endpoint = process.env.VOICELIVE_ENDPOINT ?? "";
  const agentName = process.env.AGENT_NAME ?? "";
  const projectName = process.env.PROJECT_NAME ?? "";
  const agentVersion = process.env.AGENT_VERSION;
  const conversationId = process.env.CONVERSATION_ID;
  const foundryResourceOverride = process.env.FOUNDRY_RESOURCE_OVERRIDE;
  const authenticationIdentityClientId =
    process.env.AGENT_AUTHENTICATION_IDENTITY_CLIENT_ID;

  console.log("Environment variables:");
  console.log(`  VOICELIVE_ENDPOINT: ${endpoint}`);
  console.log(`  AGENT_NAME: ${agentName}`);
  console.log(`  PROJECT_NAME: ${projectName}`);
  console.log(`  AGENT_VERSION: ${agentVersion ?? "(not set)"}`);
  console.log(`  CONVERSATION_ID: ${conversationId ?? "(not set)"}`);
  console.log(
    `  FOUNDRY_RESOURCE_OVERRIDE: ${foundryResourceOverride ?? "(not set)"}`,
  );
  console.log(
    `  AGENT_AUTHENTICATION_IDENTITY_CLIENT_ID: ${authenticationIdentityClientId ?? "(not set)"}`,
  );

  if (!endpoint || !agentName || !projectName) {
    console.error(
      "Set VOICELIVE_ENDPOINT, AGENT_NAME, and PROJECT_NAME in your .env file.",
    );
    process.exit(1);
  }

  const credential = new DefaultAzureCredential();

  const assistant = new BasicVoiceAssistant({
    endpoint,
    credential,
    agentName,
    projectName,
    agentVersion,
    conversationId,
    foundryResourceOverride,
    authenticationIdentityClientId,
  });

  try {
    await assistant.start();
  } catch (err) {
    if (err?.code === "ERR_USE_AFTER_CLOSE") return; // normal on Ctrl+C
    console.error("Fatal error:", err);
    process.exit(1);
  }
}

console.log("🎙️  Basic Foundry Voice Agent with Azure VoiceLive SDK (Agent Mode)");
console.log("=".repeat(65));
main().then(
  () => console.log("\n👋 Voice assistant shut down. Goodbye!"),
  (err) => {
    console.error("Unhandled error:", err);
    process.exit(1);
  },
);
