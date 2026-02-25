import os
import json
from dotenv import load_dotenv
from azure.identity import DefaultAzureCredential
from azure.ai.projects import AIProjectClient
from azure.ai.projects.models import PromptAgentDefinition

load_dotenv()

# Helper functions for Voice Live configuration chunking (512-char metadata limit)
def chunk_config(config_json: str, limit: int = 512) -> dict:
    """Split config into chunked metadata entries."""
    metadata = {"microsoft.voice-live.configuration": config_json[:limit]}
    remaining = config_json[limit:]
    chunk_num = 1
    while remaining:
        metadata[f"microsoft.voice-live.configuration.{chunk_num}"] = remaining[:limit]
        remaining = remaining[limit:]
        chunk_num += 1
    return metadata

def reassemble_config(metadata: dict) -> str:
    """Reassemble chunked Voice Live configuration."""
    config = metadata.get("microsoft.voice-live.configuration", "")
    chunk_num = 1
    while f"microsoft.voice-live.configuration.{chunk_num}" in metadata:
        config += metadata[f"microsoft.voice-live.configuration.{chunk_num}"]
        chunk_num += 1
    return config

# Setup client
project_client = AIProjectClient(
    endpoint=os.environ["PROJECT_ENDPOINT"],
    credential=DefaultAzureCredential(),
)
agent_name = os.environ["AGENT_NAME"]

# Define Voice Live session settings
voice_live_config = {
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
        "input_audio_noise_reduction": {"type": "azure_deep_noise_suppression"},
        "input_audio_echo_cancellation": {"type": "server_echo_cancellation"}
    }
}

# Create agent with Voice Live configuration in metadata
agent = project_client.agents.create_version(
    agent_name=agent_name,
    definition=PromptAgentDefinition(
        model=os.environ["MODEL_DEPLOYMENT_NAME"],
        instructions="You are a helpful assistant that answers general questions",
    ),
    metadata=chunk_config(json.dumps(voice_live_config))
)
print(f"Agent created: {agent.name} (version {agent.version})")

# Verify Voice Live configuration was stored correctly
retrieved_agent = project_client.agents.get(agent_name=agent_name)
stored_metadata = (retrieved_agent.versions or {}).get("latest", {}).get("metadata", {})
stored_config = reassemble_config(stored_metadata)

if stored_config:
    print("\nVoice Live configuration:")
    print(json.dumps(json.loads(stored_config), indent=2))
else:
    print("\nVoice Live configuration not found in agent metadata.")
