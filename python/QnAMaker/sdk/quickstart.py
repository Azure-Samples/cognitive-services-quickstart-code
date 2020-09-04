# install QnA Maker package on Windows with command:
# py -m pip install azure-cognitiveservices-knowledge-qnamaker

# install QnA Maker package on Mac/Linux with command:
# pip install azure-cognitiveservices-knowledge-qnamaker

# ==========================================
# Tasks Included
# ==========================================
# This sample does the following tasks.
# - Create a knowledge base.
# - Update a knowledge base.
# - Publish a knowledge base.
# - Download a knowledge base.
# - Get runtime endpoint key.
# - Query a knowledge base.
# - Delete a knowledge base.

# ==========================================
# Further reading
#
# General documentation: https://docs.microsoft.com/azure/cognitive-services/QnAMaker
# Reference documentation: https://docs.microsoft.com/en-us/python/api/overview/azure/cognitiveservices/qnamaker?view=azure-python
# ==========================================

# <Dependencies>
import os
import time

from azure.cognitiveservices.knowledge.qnamaker.authoring import QnAMakerClient
from azure.cognitiveservices.knowledge.qnamaker.runtime import QnAMakerRuntimeClient
from azure.cognitiveservices.knowledge.qnamaker.authoring.models import QnADTO, MetadataDTO, CreateKbDTO, OperationStateType, UpdateKbOperationDTO, UpdateKbOperationDTOAdd, EndpointKeysDTO, QnADTOContext, PromptDTO
from azure.cognitiveservices.knowledge.qnamaker.runtime.models import QueryDTO
from msrest.authentication import CognitiveServicesCredentials
# </Dependencies>

# <Resourcevariables>
key_var_name = 'QNA_MAKER_SUBSCRIPTION_KEY'
if not key_var_name in os.environ:
    raise Exception('Please set/export the environment variable: {}'.format(key_var_name))
subscription_key = os.environ[key_var_name]

endpoint_var_name = 'QNA_MAKER_ENDPOINT'
if not endpoint_var_name in os.environ:
    raise Exception('Please set/export the environment variable: {}'.format(endpoint_var_name))
endpoint = os.environ[endpoint_var_name]

runtime_endpoint_var_name = 'QNA_MAKER_RUNTIME_ENDPOINT'
if not runtime_endpoint_var_name in os.environ:
    raise Exception('Please set/export the environment variable: {}'.format(runtime_endpoint_var_name))
runtime_endpoint = os.environ[runtime_endpoint_var_name]
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
    print ("Creating knowledge base...")

    qna1 = QnADTO(
        answer="Yes, You can use our [REST APIs](https://docs.microsoft.com/rest/api/cognitiveservices/qnamaker/knowledgebase) to manage your knowledge base.",
        questions=["How do I manage my knowledgebase?"],
        metadata=[
            MetadataDTO(name="Category", value="api"),
            MetadataDTO(name="Language", value="REST"),
        ]
    )

    qna2 = QnADTO(
        answer="Yes, You can use our [Python SDK](https://pypi.org/project/azure-cognitiveservices-knowledge-qnamaker/) with the [Python Reference Docs](https://docs.microsoft.com/python/api/azure-cognitiveservices-knowledge-qnamaker/azure.cognitiveservices.knowledge.qnamaker?view=azure-python) to manage your knowledge base.",
        questions=["Can I program with Python?"],
        metadata=[
            MetadataDTO(name="Category", value="api"),
            MetadataDTO(name="Language", value="Python"),
        ]
    )

    urls = []
    files=[]

    create_kb_dto = CreateKbDTO(
        name="QnA Maker Python SDK Quickstart",
        qna_list=[
            qna1,
            qna2
        ],
        urls=urls,
        files=[],
        enable_hierarchical_extraction=True,
        default_answer_used_for_extraction="No answer found.",
        language="English"
    )
    create_op = client.knowledgebase.create(create_kb_payload=create_kb_dto)

    create_op_monitor = _monitor_operation(client=client, operation=create_op)

    # Get knowledge base ID from resourceLocation HTTP header
    knowledge_base_ID = create_op_monitor.resource_location.replace("/knowledgebases/", "")
    print("Created KB with ID: {}".format(knowledge_base_ID))

    return knowledge_base_ID
# </CreateKBMethod>

# <UpdateKBMethod>
def update_kb(client, kb_id):
    print ("Updating knowledge base...")

    qna3 = QnADTO(
        answer="goodbye",
        questions=[
            "bye",
            "end",
            "stop",
            "quit",
            "done"
            ],
        metadata=[
            MetadataDTO(name="Category", value="Chitchat"),
            MetadataDTO(name="Chitchat", value="end"),
        ]
    )

    qna4 = QnADTO(
        answer="Hello, please select from the list of questions or enter a new question to continue.",
        questions=[
            "hello",
            "hi",
            "start"
        ],
        metadata=[
            MetadataDTO(name="Category", value="Chitchat"),
            MetadataDTO(name="Chitchat", value="begin"),
        ],
        context = QnADTOContext(

            is_context_only = False,
            prompts = [

                PromptDTO(
                    display_order =1,
                    display_text= "Use REST",
                    qna_id=1

                ),
                PromptDTO(
                    display_order =2,
                    display_text= "Use .NET NuGet package",
                    qna_id=2
                ),
            ]
        )

    )

    urls = [
        "https://docs.microsoft.com/azure/cognitive-services/QnAMaker/troubleshooting"
    ]



    update_kb_operation_dto = UpdateKbOperationDTO(
        add=UpdateKbOperationDTOAdd(
            qna_list=[
                qna3,
                qna4
            ],
            urls = urls,
            files=[]
        ),
        delete=None,
        update=None
    )
    update_op = client.knowledgebase.update(kb_id=kb_id, update_kb=update_kb_operation_dto)
    _monitor_operation(client=client, operation=update_op)
    print("Updated knowledge base.")

# </UpdateKBMethod>

# <PublishKB>
def publish_kb(client, kb_id):
    print("Publishing knowledge base...")
    client.knowledgebase.publish(kb_id=kb_id)
    print("Published knowledge base.")
# </PublishKB>

# <DownloadKB>
def download_kb(client, kb_id):
    print("Downloading knowledge base...")
    kb_data = client.knowledgebase.download(kb_id=kb_id, environment="Prod")
    print("Downloaded knowledge base. It has {} QnAs.".format(len(kb_data.qna_documents)))
# </DownloadKB>

# <DeleteKB>
def delete_kb(client, kb_id):
    print("Deleting knowledge base...")
    client.knowledgebase.delete(kb_id=kb_id)
    print("Deleted knowledge base.")
# </DeleteKB>

# <GetQueryEndpointKey>
def getEndpointKeys_kb(client):
    print("Getting runtime endpoint keys...")
    keys = client.endpoint_keys.get_keys()
    print("Primary runtime endpoint key: {}.".format(keys.primary_endpoint_key))

    return keys.primary_endpoint_key

# </GetQueryEndpointKey>

# <GenerateAnswer>
def generate_answer(client, kb_id, runtimeKey):
    print ("Querying knowledge base...")

    authHeaderValue = "EndpointKey " + runtimeKey

    listSearchResults = client.runtime.generate_answer(kb_id, QueryDTO(question = "How do I manage my knowledgebase?"), dict(Authorization=authHeaderValue))

    for i in listSearchResults.answers:
        print(f"Answer ID: {i.id}.")
        print(f"Answer: {i.answer}.")
        print(f"Answer score: {i.score}.")
# </GenerateAnswer>

# <Main>

# <AuthorizationAuthor>
client = QnAMakerClient(endpoint=endpoint, credentials=CognitiveServicesCredentials(subscription_key))
# </AuthorizationAuthor>

kb_id = create_kb(client=client)
update_kb (client=client, kb_id=kb_id)
publish_kb (client=client, kb_id=kb_id)
download_kb (client=client, kb_id=kb_id)

queryRuntimeKey = getEndpointKeys_kb(client=client)

# <AuthorizationQuery>
runtimeClient = QnAMakerRuntimeClient(runtime_endpoint=runtime_endpoint, credentials=CognitiveServicesCredentials(queryRuntimeKey))
# </AuthorizationQuery>

generate_answer(client=runtimeClient,kb_id=kb_id,runtimeKey=queryRuntimeKey)
delete_kb (client=client, kb_id=kb_id)

# </Main>
