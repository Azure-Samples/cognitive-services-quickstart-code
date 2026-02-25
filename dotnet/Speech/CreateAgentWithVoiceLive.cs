// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text;
using Azure.AI.Projects;
using Azure.Identity;

/// <summary>
/// Creates an Azure AI Foundry agent configured for Voice Live sessions.
///
/// Voice Live session settings (voice, VAD, noise reduction, etc.) are stored
/// in the agent's metadata using a chunking strategy because each metadata value
/// is limited to 512 characters.
///
/// Required environment variables:
///   PROJECT_CONNECTION_STRING - Azure AI Foundry project connection string
///   AGENT_NAME               - Name for the agent
///   MODEL_DEPLOYMENT_NAME    - Model deployment name (e.g., gpt-4o-mini)
/// </summary>

// <create_agent>
var connectionString = Environment.GetEnvironmentVariable("PROJECT_CONNECTION_STRING");
var agentName = Environment.GetEnvironmentVariable("AGENT_NAME");
var model = Environment.GetEnvironmentVariable("MODEL_DEPLOYMENT_NAME");

if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(agentName)
    || string.IsNullOrEmpty(model))
{
    Console.Error.WriteLine("Set PROJECT_CONNECTION_STRING, AGENT_NAME, and MODEL_DEPLOYMENT_NAME environment variables.");
    return;
}

// Create the Agents client with Entra ID authentication
var projectClient = new AIProjectClient(connectionString, new DefaultAzureCredential());
var agentsClient = projectClient.GetAgentsClient();

// Define Voice Live session settings
var voiceLiveConfig = """
{
  "session": {
    "voice": {
      "name": "en-US-Ava:DragonHDLatestNeural",
      "type": "azure-standard",
      "temperature": 0.8
    },
    "input_audio_transcription": {
      "model": "azure-speech"
    },
    "turn_detection": {
      "type": "azure_semantic_vad",
      "end_of_utterance_detection": {
        "model": "semantic_detection_v1_multilingual"
      }
    },
    "input_audio_noise_reduction": { "type": "azure_deep_noise_suppression" },
    "input_audio_echo_cancellation": { "type": "server_echo_cancellation" }
  }
}
""";

// Chunk the config into metadata entries (512-char limit per value)
var metadata = ChunkConfig(voiceLiveConfig.Trim());

// Create the agent with Voice Live configuration in metadata
var agent = await agentsClient.CreateAgentAsync(
    model: model,
    name: agentName,
    instructions: "You are a helpful assistant that answers general questions",
    metadata: metadata);

Console.WriteLine($"Agent created: {agent.Value.Name} (id: {agent.Value.Id})");

// Verify Voice Live configuration was stored correctly
var retrieved = await agentsClient.GetAgentAsync(agent.Value.Id);
var storedConfig = ReassembleConfig(retrieved.Value.Metadata);

if (!string.IsNullOrEmpty(storedConfig))
{
    Console.WriteLine("\nVoice Live configuration:");
    Console.WriteLine(storedConfig);
}
else
{
    Console.WriteLine("\nVoice Live configuration not found in agent metadata.");
}
// </create_agent>

// <chunk_config>
/// <summary>
/// Splits a configuration JSON string into chunked metadata entries.
/// Each metadata value is limited to 512 characters.
/// </summary>
static Dictionary<string, string> ChunkConfig(string configJson)
{
    const int limit = 512;
    var metadata = new Dictionary<string, string>
    {
        ["microsoft.voice-live.configuration"] = configJson[..Math.Min(configJson.Length, limit)]
    };

    var remaining = configJson.Length > limit ? configJson[limit..] : "";
    var chunkNum = 1;
    while (remaining.Length > 0)
    {
        var chunk = remaining[..Math.Min(remaining.Length, limit)];
        metadata[$"microsoft.voice-live.configuration.{chunkNum}"] = chunk;
        remaining = remaining.Length > limit ? remaining[limit..] : "";
        chunkNum++;
    }
    return metadata;
}
// </chunk_config>

// <reassemble_config>
/// <summary>
/// Reassembles chunked Voice Live configuration from agent metadata.
/// </summary>
static string ReassembleConfig(IReadOnlyDictionary<string, string>? metadata)
{
    if (metadata == null) return "";

    var config = new StringBuilder();
    if (metadata.TryGetValue("microsoft.voice-live.configuration", out var baseValue))
    {
        config.Append(baseValue);
    }
    var chunkNum = 1;
    while (metadata.TryGetValue($"microsoft.voice-live.configuration.{chunkNum}", out var chunk))
    {
        config.Append(chunk);
        chunkNum++;
    }
    return config.ToString();
}
// </reassemble_config>
