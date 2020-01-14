# <dependencies>
import os
import time

# install with command: py -m pip install azure-cognitiveservices-knowledge-qnamaker
from azure.cognitiveservices.knowledge.qnamaker import QnAMakerClient
from azure.cognitiveservices.knowledge.qnamaker.models import QnADTO, MetadataDTO, CreateKbDTO, OperationStateType, UpdateKbOperationDTO, UpdateKbOperationDTOAdd, EndpointKeysDTO
from msrest.authentication import CognitiveServicesCredentials
# </dependencies>

# This sample does the following tasks.
# - Create a knowledge base.
# - Update a knowledge base.
# - Publish a knowledge base.
# - Download a knowledge base.
# - Delete a knowledge base.
# - Get published endpoint.

# <resourcekeys>
key_var_name = 'QNAMAKER_KEY'
if not key_var_name in os.environ:
	raise Exception('Please set/export the environment variable: {}'.format(key_var_name))
# example is 32 character string: xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
authoring_key = os.environ[key_var_name]

host_var_name = 'QNAMAKER_HOST'
if not host_var_name in os.environ:
	raise Exception('Please set/export the environment variable: {}'.format(host_var_name))

# example is: https://REPLACE-WITH-YOUR-RESOURCE-NAME.cognitiveservices.azure.com
authoring_host = os.environ[host_var_name]
# </resourcekeys>

# Helper functions
# <monitorOperation>
def _monitor_operation(client, operation):

    for i in range(20):
        if operation.operation_state in [OperationStateType.not_started, OperationStateType.running]:
            print("Waiting for operation: {} to complete.".format(operation.operation_id))
            time.sleep(5)
            operation = client.operations.get_details(operation_id=operation.operation_id)
        else:
            break
    if operation.operation_state != OperationStateType.succeeded:
        raise Exception("Operation {} failed to complete.".format(operation.operation_id))

    return operation
# </monitorOperation>

# <createkb>
def create_kb(client):

    qna = QnADTO(
        answer="You can use our REST APIs to manage your knowledge base.",
        questions=["How do I manage my knowledgebase?"],
        metadata=[MetadataDTO(name="Category", value="api")]
    )
    urls = ["https://docs.microsoft.com/en-in/azure/cognitive-services/qnamaker/faqs"]

    create_kb_dto = CreateKbDTO(
        name="QnA Maker FAQ from SDK quickstart",
        qna_list=[qna],
        urls=urls
    )
    create_op = client.knowledgebase.create(create_kb_payload=create_kb_dto)

    create_op = _monitor_operation(client=client, operation=create_op)

    # Get knowledge base ID from resourceLocation HTTP header
    knowledge_base_ID = create_op.resource_location.replace("/knowledgebases/", "")
    print("Created KB with ID: {}".format(knowledge_base_ID))

    return knowledge_base_ID
# </createkb>

# <updatekb>
def update_kb(client, kb_id):
    update_kb_operation_dto = UpdateKbOperationDTO(
        add=UpdateKbOperationDTOAdd(
            qna_list=[
                QnADTO(questions=["bye"], answer="goodbye")
            ]
        )
    )
    update_op = client.knowledgebase.update(kb_id=kb_id, update_kb=update_kb_operation_dto)
    _monitor_operation(client=client, operation=update_op)
    print("Updated.")

# </updatekb>

# <publishkb>
def publish_kb(client, kb_id):
	client.knowledgebase.publish(kb_id=kb_id)
	print("Published.")
# </publishkb>

# <getEndpointKeys>
def getEndpointKeys_kb(client):
	keys = client.endpoint_keys.get_keys()
	print("Query knowledge base with prediction runtime key {}.".format(keys.primary_endpoint_key))
# </getEndpointKeys>

# <downloadkb>
def download_kb(client, kb_id):
	kb_data = client.knowledgebase.download(kb_id=kb_id, environment="Prod")
	print("Downloaded. It has {} QnAs.".format(len(kb_data.qna_documents)))
# </downloadkb>

# <deletekb>
def delete_kb(client, kb_id):
	client.knowledgebase.delete(kb_id=kb_id)
	print("Deleted.")
# </deletekb>

# Main

# <authorization>
client = QnAMakerClient(endpoint=authoring_host, credentials=CognitiveServicesCredentials(authoring_key))
# </authorization>

# Create a KB
kb_id = create_kb(client=client)

# Update a KB
update_kb (client=client, kb_id=kb_id)

# Publish the KB
publish_kb (client=client, kb_id=kb_id)

# Get published endpoint in order to query knowledge base for answer
getEndpointKeys_kb(client=client)

# Download the KB
download_kb (client=client, kb_id=kb_id)

# Delete the KB
delete_kb (client=client, kb_id=kb_id)