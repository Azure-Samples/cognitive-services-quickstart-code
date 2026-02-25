// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// Create a Foundry agent with Voice Live session configuration in metadata.
// Uses @azure/ai-agents SDK to create the agent and store chunked Voice Live
// session settings so the VoiceLive service can pick them up at connection time.

import "dotenv/config";
import { AgentsClient } from "@azure/ai-agents";
import { DefaultAzureCredential } from "@azure/identity";

// ---------------------------------------------------------------------------
// Voice Live configuration chunking helpers (512-char metadata value limit)
// ---------------------------------------------------------------------------

/**
 * Split a JSON config string into chunked metadata entries.
 * @param {string} configJson - Serialized JSON configuration.
 * @param {number} [limit=512] - Maximum characters per metadata value.
 * @returns {Record<string, string>} Metadata key/value pairs.
 */
function chunkConfig(configJson, limit = 512) {
  const metadata = {
    "microsoft.voice-live.configuration": configJson.slice(0, limit),
  };
  let remaining = configJson.slice(limit);
  let chunkNum = 1;
  while (remaining.length > 0) {
    metadata[`microsoft.voice-live.configuration.${chunkNum}`] =
      remaining.slice(0, limit);
    remaining = remaining.slice(limit);
    chunkNum++;
  }
  return metadata;
}

/**
 * Reassemble a chunked Voice Live configuration from metadata.
 * @param {Record<string, string>} metadata - Agent metadata.
 * @returns {string} The full JSON configuration string.
 */
function reassembleConfig(metadata) {
  let config = metadata["microsoft.voice-live.configuration"] ?? "";
  let chunkNum = 1;
  while (`microsoft.voice-live.configuration.${chunkNum}` in metadata) {
    config += metadata[`microsoft.voice-live.configuration.${chunkNum}`];
    chunkNum++;
  }
  return config;
}

// ---------------------------------------------------------------------------
// Main
// ---------------------------------------------------------------------------
async function main() {
  const endpoint = process.env.PROJECT_ENDPOINT;
  const agentName = process.env.AGENT_NAME;
  const modelDeployment = process.env.MODEL_DEPLOYMENT_NAME;

  if (!endpoint || !agentName || !modelDeployment) {
    console.error(
      "Set PROJECT_ENDPOINT, AGENT_NAME, and MODEL_DEPLOYMENT_NAME in your .env file.",
    );
    process.exit(1);
  }

  const credential = new DefaultAzureCredential();
  const client = new AgentsClient(endpoint, credential);

  // Define Voice Live session settings
  const voiceLiveConfig = {
    session: {
      voice: {
        name: "en-US-Ava:DragonHDLatestNeural",
        type: "azure-standard",
        temperature: 0.8,
      },
      input_audio_transcription: {
        model: "azure-speech",
      },
      turn_detection: {
        type: "azure_semantic_vad",
        end_of_utterance_detection: {
          model: "semantic_detection_v1_multilingual",
        },
      },
      input_audio_noise_reduction: { type: "azure_deep_noise_suppression" },
      input_audio_echo_cancellation: { type: "server_echo_cancellation" },
    },
  };

  // Create the agent with Voice Live configuration stored in metadata
  const configJson = JSON.stringify(voiceLiveConfig);
  const agent = await client.createAgent(modelDeployment, {
    name: agentName,
    instructions:
      "You are a helpful assistant that answers general questions",
    metadata: chunkConfig(configJson),
  });
  console.log(`Agent created: ${agent.name} (id: ${agent.id})`);

  // Verify the stored configuration
  const retrieved = await client.getAgent(agent.id);
  const storedConfig = reassembleConfig(retrieved.metadata ?? {});

  if (storedConfig) {
    console.log("\nVoice Live configuration:");
    console.log(JSON.stringify(JSON.parse(storedConfig), null, 2));
  } else {
    console.log("\nVoice Live configuration not found in agent metadata.");
  }
}

main().catch((err) => {
  console.error("Error:", err);
  process.exit(1);
});
