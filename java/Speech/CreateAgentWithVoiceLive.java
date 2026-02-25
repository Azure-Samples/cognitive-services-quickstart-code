// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import com.azure.ai.agents.AgentsClient;
import com.azure.ai.agents.AgentsClientBuilder;
import com.azure.ai.agents.models.AgentDetails;
import com.azure.ai.agents.models.AgentVersionDetails;
import com.azure.ai.agents.models.PromptAgentDefinition;
import com.azure.identity.DefaultAzureCredentialBuilder;

import java.util.LinkedHashMap;
import java.util.Map;

/**
 * Creates an Azure AI Foundry agent configured for Voice Live sessions.
 *
 * <p>Voice Live session settings (voice, VAD, noise reduction, etc.) are stored
 * in the agent's metadata using a chunking strategy because each metadata value
 * is limited to 512 characters.</p>
 *
 * <p>Required environment variables:</p>
 * <ul>
 *   <li>PROJECT_ENDPOINT - Azure AI Foundry project endpoint</li>
 *   <li>AGENT_NAME - Name for the agent</li>
 *   <li>MODEL_DEPLOYMENT_NAME - Model deployment name (e.g., gpt-4o-mini)</li>
 * </ul>
 */
public class CreateAgentWithVoiceLive {

    private static final int METADATA_VALUE_LIMIT = 512;

    // <create_agent>
    public static void main(String[] args) {
        String endpoint = System.getenv("PROJECT_ENDPOINT");
        String agentName = System.getenv("AGENT_NAME");
        String model = System.getenv("MODEL_DEPLOYMENT_NAME");

        if (endpoint == null || agentName == null || model == null) {
            System.err.println("Set PROJECT_ENDPOINT, AGENT_NAME, and MODEL_DEPLOYMENT_NAME environment variables.");
            System.exit(1);
        }

        // Create the Agents client with Entra ID authentication
        AgentsClient agentsClient = new AgentsClientBuilder()
                .credential(new DefaultAzureCredentialBuilder().build())
                .endpoint(endpoint)
                .buildAgentsClient();

        // Define Voice Live session settings
        String voiceLiveConfig = "{"
                + "\"session\": {"
                + "\"voice\": {"
                + "\"name\": \"en-US-Ava:DragonHDLatestNeural\","
                + "\"type\": \"azure-standard\","
                + "\"temperature\": 0.8"
                + "},"
                + "\"input_audio_transcription\": {"
                + "\"model\": \"azure-speech\""
                + "},"
                + "\"turn_detection\": {"
                + "\"type\": \"azure_semantic_vad\","
                + "\"end_of_utterance_detection\": {"
                + "\"model\": \"semantic_detection_v1_multilingual\""
                + "}"
                + "},"
                + "\"input_audio_noise_reduction\": {\"type\": \"azure_deep_noise_suppression\"},"
                + "\"input_audio_echo_cancellation\": {\"type\": \"server_echo_cancellation\"}"
                + "}"
                + "}";

        // Chunk the config into metadata entries (512-char limit per value)
        Map<String, String> metadata = chunkConfig(voiceLiveConfig);

        // Create the agent with Voice Live configuration in metadata
        PromptAgentDefinition definition = new PromptAgentDefinition(model)
                .setInstructions("You are a helpful assistant that answers general questions");

        AgentVersionDetails agent = agentsClient.createAgentVersion(agentName, definition, metadata, null);
        System.out.println("Agent created: " + agent.getName() + " (version " + agent.getVersion() + ")");

        // Verify Voice Live configuration was stored correctly
        AgentDetails retrieved = agentsClient.getAgent(agentName);
        Map<String, String> storedMetadata = retrieved.getVersions().getLatest().getMetadata();
        String storedConfig = reassembleConfig(storedMetadata);

        if (storedConfig != null && !storedConfig.isEmpty()) {
            System.out.println("\nVoice Live configuration:");
            System.out.println(storedConfig);
        } else {
            System.out.println("\nVoice Live configuration not found in agent metadata.");
        }
    }
    // </create_agent>

    // <chunk_config>
    /**
     * Splits a configuration JSON string into chunked metadata entries.
     * Each metadata value is limited to 512 characters.
     */
    static Map<String, String> chunkConfig(String configJson) {
        Map<String, String> metadata = new LinkedHashMap<>();
        metadata.put("microsoft.voice-live.configuration",
                configJson.substring(0, Math.min(configJson.length(), METADATA_VALUE_LIMIT)));

        String remaining = configJson.length() > METADATA_VALUE_LIMIT
                ? configJson.substring(METADATA_VALUE_LIMIT) : "";
        int chunkNum = 1;
        while (!remaining.isEmpty()) {
            String chunk = remaining.substring(0, Math.min(remaining.length(), METADATA_VALUE_LIMIT));
            metadata.put("microsoft.voice-live.configuration." + chunkNum, chunk);
            remaining = remaining.length() > METADATA_VALUE_LIMIT
                    ? remaining.substring(METADATA_VALUE_LIMIT) : "";
            chunkNum++;
        }
        return metadata;
    }
    // </chunk_config>

    // <reassemble_config>
    /**
     * Reassembles chunked Voice Live configuration from agent metadata.
     */
    static String reassembleConfig(Map<String, String> metadata) {
        if (metadata == null) {
            return "";
        }
        StringBuilder config = new StringBuilder();
        String base = metadata.get("microsoft.voice-live.configuration");
        if (base != null) {
            config.append(base);
        }
        int chunkNum = 1;
        while (metadata.containsKey("microsoft.voice-live.configuration." + chunkNum)) {
            config.append(metadata.get("microsoft.voice-live.configuration." + chunkNum));
            chunkNum++;
        }
        return config.toString();
    }
    // </reassemble_config>
}
