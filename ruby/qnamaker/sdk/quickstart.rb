# install QnA Maker package with command:
# gem install azure_cognitiveservices_qnamaker
# gem install azure_cognitiveservices_qnamakerruntime

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

# <Dependencies>
require 'azure_cognitiveservices_qnamaker'
require 'azure_cognitiveservices_qnamakerruntime'
include Azure::CognitiveServices::Qnamaker::V4_0::Models
include Azure::CognitiveServices::QnamakerRuntime::V4_0::Models
# </Dependencies>

# <Resourcevariables>
subscription_key = 'PASTE_YOUR_QNA_MAKER_AUTHORING_SUBSCRIPTION_KEY_HERE'
endpoint = 'PASTE_YOUR_QNA_MAKER_AUTHORING_ENDPOINT_HERE'
runtime_endpoint = 'PASTE_YOUR_QNA_MAKER_RUNTIME_ENDPOINT_HERE'
# </Resourcevariables>

# <MonitorOperation>
def _monitor_operation(client, operation)

    for i in 1..20
        if ["NotStarted", "Running"].include?(operation.operation_state)
            print("Waiting for operation: #{operation.operation_id} to complete.\n")
            sleep(10)
            operation = client.operations.get_details(operation.operation_id)
        else
            break
		end
	end
    if operation.operation_state != "Succeeded"
        raise Exception("Operation #{operation.operation_id} failed to complete.")
	end

    return operation
end
# </MonitorOperation>

# <CreateKBMethod>
def create_kb(client)
    print("Creating knowledge base...\n")

	metadata_1 = MetadataDTO.new()
	metadata_1.name = "Category"
	metadata_1.value = "api"
	metadata_2 = MetadataDTO.new()
	metadata_2.name = "Language"
	metadata_2.value = "REST"
    qna1 = QnADTO.new()
	qna1.answer = "Yes, You can use our [REST APIs](https://docs.microsoft.com/rest/api/cognitiveservices/qnamaker/knowledgebase) to manage your knowledge base."
	qna1.questions = ["How do I manage my knowledgebase?"]
	qna1.metadata = [metadata_1, metadata_2]

	metadata_2 = MetadataDTO.new()
	metadata_2.name = "Language"
	metadata_2.value = "Ruby"
    qna2 = QnADTO.new()
	qna2.answer = "Yes, You can use our [Ruby SDK](https://rubygems.org/gems/azure_cognitiveservices_qnamaker) to manage your knowledge base."
	qna2.questions = ["Can I program with Ruby?"]
    qna2.metadata = [metadata_1, metadata_2]

    create_kb_dto = CreateKbDTO.new()
	create_kb_dto.name = "QnA Maker Ruby SDK Quickstart"
	create_kb_dto.qna_list = [qna1, qna2]

    operation = client.knowledgebase.create(create_kb_payload=create_kb_dto)
	result = _monitor_operation(client, operation)
    knowledge_base_ID = result.resource_location.sub! "/knowledgebases/", ""

    print("Created KB with ID: #{knowledge_base_ID}\n")

    return knowledge_base_ID
end
# </CreateKBMethod>

# <UpdateKBMethod>
def update_kb(client, kb_id)
    print("Updating knowledge base...\n")

	metadata_1 = MetadataDTO.new()
	metadata_1.name = "Category"
	metadata_1.value = "Chitchat"
	metadata_2 = MetadataDTO.new()
	metadata_2.name = "Chitchat"
	metadata_2.value = "end"
    qna3 = QnADTO.new()
	qna3.answer = "goodbye"
	qna3.questions = ["bye", "end", "stop", "quit", "done"]
    qna3.metadata=[metadata_1, metadata_2]

	metadata_2 = MetadataDTO.new()
	metadata_2.name = "Chitchat"
	metadata_2.value = "begin"
	prompt_1 = PromptDTO.new()
	prompt_1.display_order = 1
	prompt_1.display_text = "Use REST"
	prompt_1.qna_id = 1
	prompt_2 = PromptDTO.new()
	prompt_2.display_order = 2
	prompt_2.display_text = "Use .NET NuGet package"
	prompt_2.qna_id = 2
	context = QnADTOContext.new()
	context.is_context_only = false
	context.prompts = [prompt_1, prompt_2]
	qna4 = QnADTO.new()
	qna4.answer = "Hello, please select from the list of questions or enter a new question to continue."
	qna4.questions = ["hello", "hi", "start"]
	qna4.metadata=[metadata_1, metadata_2]
	qna4.context = context

	add = UpdateKbOperationDTOAdd.new()
	add.qna_list = [qna3, qna4]
	add.urls = ["https://docs.microsoft.com/azure/cognitive-services/QnAMaker/troubleshooting"]
    update_kb_operation_dto = UpdateKbOperationDTO.new()
	update_kb_operation_dto.add = add

    operation = client.knowledgebase.update(kb_id, update_kb_operation_dto)
	_monitor_operation(client, operation)
    print("Updated knowledge base.\n")
end
# </UpdateKBMethod>

# <PublishKB>
def publish_kb(client, kb_id)
    print("Publishing knowledge base...\n")
    client.knowledgebase.publish(kb_id)
    print("Published knowledge base.\n")
end
# </PublishKB>

# <DownloadKB>
def download_kb(client, kb_id)
    print("Downloading knowledge base...\n")
    kb_data = client.knowledgebase.download(kb_id, "Prod")
    print("Downloaded knowledge base. It has #{kb_data.qna_documents.length()} QnAs.\n")
end
# </DownloadKB>

# <DeleteKB>
def delete_kb(client, kb_id)
    print("Deleting knowledge base...\n")
    client.knowledgebase.delete(kb_id)
    print("Deleted knowledge base.\n")
end
# </DeleteKB>

# <GetQueryEndpointKey>
def getEndpointKeys_kb(client)
    print("Getting runtime endpoint keys...\n")
    keys = client.endpoint_keys.get_keys()
    print("Primary runtime endpoint key: #{keys.primary_endpoint_key}.\n")

    return keys.primary_endpoint_key
end
# </GetQueryEndpointKey>

# <GenerateAnswer>
def generate_answer(client, kb_id, endpoint_key)
    print("Querying knowledge base...\n")

	query = QueryDTO.new()
	query.question = "How do I manage my knowledgebase?"
	headers = Hash.new
	headers["Authorization"] = "EndpointKey #{endpoint_key}"
    listSearchResults = client.runtime.generate_answer(kb_id, query, custom_headers:headers)

    for i in listSearchResults.answers
        print("Answer ID: #{i.id}.\n")
        print("Answer: #{i.answer}.\n")
        print("Answer score: #{i.score}.\n")
	end
end
# </GenerateAnswer>

# <Main>

# <AuthorizationAuthor>
credentials = MsRestAzure::CognitiveServicesCredentials.new(subscription_key)
client = Azure::CognitiveServices::Qnamaker::V4_0::QnamakerClient.new(credentials)
client.endpoint = endpoint
# </AuthorizationAuthor>

kb_id = create_kb(client)
update_kb(client, kb_id)
publish_kb(client, kb_id)
download_kb(client, kb_id)

runtime_endpoint_key = getEndpointKeys_kb(client)

# <AuthorizationQuery>
credentials = MsRestAzure::CognitiveServicesCredentials.new(subscription_key)
runtime_client = Azure::CognitiveServices::QnamakerRuntime::V4_0::QnamakerRuntimeClient.new(credentials)
runtime_client.runtime_endpoint = runtime_endpoint
# </AuthorizationQuery>

generate_answer(runtime_client, kb_id, runtime_endpoint_key)

delete_kb(client, kb_id)

# </Main>
