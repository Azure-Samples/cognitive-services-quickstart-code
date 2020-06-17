# install with command:
# py -m pip install azure-cognitiveservices-knowledge-qnamaker==0.2.0

# <Dependencies>
import os
import time

from azure.cognitiveservices.knowledge.qnamaker.authoring import QnAMakerClient
from azure.cognitiveservices.knowledge.qnamaker.runtime import QnAMakerRuntimeClient
from azure.cognitiveservices.knowledge.qnamaker.authoring.models import QnADTO, MetadataDTO, CreateKbDTO, OperationStateType, UpdateKbOperationDTO, UpdateKbOperationDTOAdd, EndpointKeysDTO
from azure.cognitiveservices.knowledge.qnamaker.runtime.models import QueryDTO
from msrest.authentication import CognitiveServicesCredentials

# </Dependencies>

# This sample does the following tasks.
# - Create a knowledge base.
# - Update a knowledge base.
# - Publish a knowledge base.
# - Download a knowledge base.
# - Delete a knowledge base.
# - Get published endpoint.

# <Resourcevariables>
authoring_key = 'REPLACE-WITH-YOUR-QNA-MAKER-KEY'
resource_name = "REPLACE-WITH-YOUR-RESOURCE-NAME"

authoringURL = f"https://{resource_name}.cognitiveservices.azure.com"
queryingURL = f"https://{resource_name}.azurewebsites.net"
# </Resourcevariables>

# <MonitorOperation>
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
# </MonitorOperation>

# <CreateKBMethod>
def create_kb(client):

    qna = QnADTO(
        answer="You can use our REST APIs to manage your knowledge base.",
        questions=["How do I manage my knowledgebase?"],
        metadata=[MetadataDTO(name="Category", value="api")]
    )
    urls = ["https://docs.microsoft.com/en-in/azure/cognitive-services/qnamaker/faqs"]

    create_kb_dto = CreateKbDTO(
        name="QnA Maker Python SDK Quickstart",
        qna_list=[qna],
        urls=urls
    )
    create_op = client.knowledgebase.create(create_kb_payload=create_kb_dto)

    create_op = _monitor_operation(client=client, operation=create_op)

    # Get knowledge base ID from resourceLocation HTTP header
    knowledge_base_ID = create_op.resource_location.replace("/knowledgebases/", "")
    print("Created KB with ID: {}".format(knowledge_base_ID))

    return knowledge_base_ID
# </CreateKBMethod>

# <UpdateKBMethod>
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

# </UpdateKBMethod>

# <PublishKB>
def publish_kb(client, kb_id):
	client.knowledgebase.publish(kb_id=kb_id)
	print("Published.")
# </PublishKB>

# <DownloadKB>
def download_kb(client, kb_id):
	kb_data = client.knowledgebase.download(kb_id=kb_id, environment="Prod")
	print("Downloaded. It has {} QnAs.".format(len(kb_data.qna_documents)))
# </DownloadKB>

# <DeleteKB>
def delete_kb(client, kb_id):
	client.knowledgebase.delete(kb_id=kb_id)
	print("Deleted.")
# </DeleteKB>

# <GetQueryEndpointKey>
def getEndpointKeys_kb(client):
	keys = client.endpoint_keys.get_keys()
	print("Query knowledge base with prediction runtime key {}.".format(keys.primary_endpoint_key))

	return keys.primary_endpoint_key

# </GetQueryEndpointKey>

# <GenerateAnswer>
def generate_answer(client, kb_id, runtimeKey):

    authHeaderValue = "EndpointKey " + runtimeKey

    answer = client.runtime.generate_answer(kb_id, QueryDTO(question = "How do I manage my knowledgebase?"), dict(Authorization=authHeaderValue))
    print(f"{answer}.")
# </GenerateAnswer>

# <Main>

# <AuthorizationAuthor>
client = QnAMakerClient(endpoint=authoringURL, credentials=CognitiveServicesCredentials(authoring_key))
# </AuthorizationAuthor>

kb_id = create_kb(client=client)
update_kb (client=client, kb_id=kb_id)
publish_kb (client=client, kb_id=kb_id)
download_kb (client=client, kb_id=kb_id)

queryRuntimeKey = getEndpointKeys_kb(client=client)

runtimeClient = QnAMakerRuntimeClient(runtime_endpoint=queryingURL, credentials=CognitiveServicesCredentials(queryRuntimeKey))
generate_answer(client=runtimeClient,kb_id=kb_id,runtimeKey=queryRuntimeKey)

delete_kb (client=client, kb_id=kb_id)

# </Main>
