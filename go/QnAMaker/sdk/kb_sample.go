package main

// <dependencies>
import (
	"context"
	"fmt"
	"github.com/Azure/azure-sdk-for-go/services/cognitiveservices/v4.0/qnamaker"
	"github.com/Azure/azure-sdk-for-go/services/cognitiveservices/v4.0/qnamakerruntime"
	"github.com/Azure/go-autorest/autorest"
    "log"
	"strings"
	"time"
)
// </dependencies>

/* This sample for the Azure Cognitive Services QnA Maker API shows how to:
 * - Create a knowledge base
 * - List all knowledge bases
 * - Update a knowledge base
 * - Publish a knowledge base
 * - Query a knowledge base
 * - Delete a knowledge base
 */

// <error_handling>
// Helper function to handle errors.
func print_inner_error (error qnamaker.InnerErrorModel) {
	if error.Code != nil {
		fmt.Println (*error.Code)
	}
	if error.InnerError != nil {
		print_inner_error (*error.InnerError)
	}
}

// Helper function to handle errors.
func print_error_details (errors []qnamaker.Error) {
	for _, err := range errors {
		if err.Message != nil {
			fmt.Println (*err.Message)
		}
		if err.Details != nil {
			print_error_details (*err.Details)
		}
		if err.InnerError != nil {
			print_inner_error (*err.InnerError)
		}
	}
}

// Helper function to handle errors.
func handle_error (result qnamaker.Operation) {
	if result.ErrorResponse != nil {
		response := *result.ErrorResponse
		if response.Error != nil {
			err := *response.Error
			if err.Message != nil {
				fmt.Println (*err.Message)
			}
			if err.Details != nil {
				print_error_details (*err.Details)
			}
			if err.InnerError != nil {
				print_inner_error (*err.InnerError)
			}
		}
	}
}
// </error_handling>

/*
* Set the `subscription_key` and `authoring_endpoint` variables to your
* QnA Maker authoring subscription key and endpoint.
*
* These values can be found in the Azure portal (ms.portal.azure.com/).
* Look up your QnA Maker resource. Then, in the "Resource management"
* section, find the "Keys and Endpoint" page.
*
* The value of `authoring_endpoint` has the format https://YOUR-RESOURCE-NAME.cognitiveservices.azure.com.
*
* Set the `runtime_endpoint` variable to your QnA Maker runtime endpoint.
* The value of `runtime_endpoint` has the format https://YOUR-RESOURCE-NAME.azurewebsites.net.
*/
// <environment>
var subscription_key string = "PASTE_YOUR_QNA_MAKER_AUTHORING_SUBSCRIPTION_KEY_HERE"
var authoring_endpoint string = "PASTE_YOUR_QNA_MAKER_AUTHORING_ENDPOINT_HERE"
var runtime_endpoint string = "PASTE_YOUR_QNA_MAKER_RUNTIME_ENDPOINT_HERE"
// </environment>

// <get_runtime_endpoint_key>
// Get runtime endpoint key.
func get_runtime_endpoint_key () string {
	// Get the context, which is required by the SDK methods.
	ctx := context.Background()

	client := qnamaker.NewEndpointKeysClient(authoring_endpoint)
	// Set the subscription key on the client.
	client.Authorizer = autorest.NewCognitiveServicesAuthorizer(subscription_key)

	result, err := client.GetKeys (ctx)
	if err != nil {
		log.Fatal(err)
	}

	return *result.PrimaryEndpointKey
}
// </get_runtime_endpoint_key>

// <wait_for_operation>
func wait_for_operation (operation_id string) string {
	// Get the context, which is required by the SDK methods.
	ctx := context.Background()

	ops_client := qnamaker.NewOperationsClient(authoring_endpoint)
	// Set the subscription key on the client.
	ops_client.Authorizer = autorest.NewCognitiveServicesAuthorizer(subscription_key)

	result := ""
	done := false
	for done == false {
		op_result, op_err := ops_client.GetDetails (ctx, operation_id)
		if op_err != nil {
			log.Fatal(op_err)
		}
		// If the operation isn't finished, wait and query again.
		if op_result.OperationState == "Running" || op_result.OperationState == "NotStarted" {
			fmt.Println ("Operation is not finished. Waiting 10 seconds...")
			time.Sleep (time.Duration(10) * time.Second)
		} else {
			done = true
			fmt.Print ("Operation result: " + op_result.OperationState)
			fmt.Println ()
			if op_result.OperationState == "Failed" {
				handle_error (op_result)
				log.Fatal()
			} else {
				result = strings.ReplaceAll(*op_result.ResourceLocation, "/knowledgebases/", "")
			}
		}
	}
	return result
}
// </wait_for_operation>

// <create_kb>
// Create a knowledge base.
func create_kb () string {
	// Get the context, which is required by the SDK methods.
	ctx := context.Background()

	client := qnamaker.NewKnowledgebaseClient(authoring_endpoint)
	// Set the subscription key on the client.
	client.Authorizer = autorest.NewCognitiveServicesAuthorizer(subscription_key)

	name := "QnA Maker FAQ"

	/*
	The fields of QnADTO are pointers, and we cannot get the addresses of literal values,
	so we declare helper variables.
	*/
	id := int32(0)
	answer := "You can use our REST APIs to manage your Knowledge Base. See here for details: https://westus.dev.cognitive.microsoft.com/docs/services/58994a073d9e04097c7ba6fe/operations/58994a073d9e041ad42d9baa"
	source := "Custom Editorial"
	questions := []string{ "How do I programmatically update my Knowledge Base?" }

	// The fields of MetadataDTO are also pointers.
	metadata_name_1 := "category"
	metadata_value_1 := "api"
	metadata := []qnamaker.MetadataDTO{ qnamaker.MetadataDTO{ Name: &metadata_name_1, Value: &metadata_value_1 } }
	qna_list := []qnamaker.QnADTO{ qnamaker.QnADTO{
		ID: &id,
		Answer: &answer,
		Source: &source,
		Questions: &questions,
		Metadata: &metadata,
	} }

	urls := []string{}
	files := []qnamaker.FileDTO{}

	// The fields of CreateKbDTO are all pointers, so we get the addresses of our variables.
	createKbPayload := qnamaker.CreateKbDTO{ Name: &name, QnaList: &qna_list, Urls: &urls, Files: &files }

	// Create the KB.
	kb_result, kb_err := client.Create (ctx, createKbPayload)
	if kb_err != nil {
		log.Fatal(kb_err)
	}

	// Wait for the KB create operation to finish.
	fmt.Println ("Waiting for KB create operation to finish...")
	// Operation.OperationID is a pointer, so we need to dereference it.
	return wait_for_operation (*kb_result.OperationID)
}
// </create_kb>

// <list_kbs>
// List all knowledge bases.
func list_kbs () {
	// Get the context, which is required by the SDK methods.
	ctx := context.Background()

	client := qnamaker.NewKnowledgebaseClient(authoring_endpoint)
	// Set the subscription key on the client.
	client.Authorizer = autorest.NewCognitiveServicesAuthorizer(subscription_key)

	result, err := client.ListAll (ctx)
	if err != nil {
		log.Fatal(err)
	}
	fmt.Println ("Existing knowledge bases:\n")
	// KnowledgebasesDTO.Knowledgebases is a pointer, so we need to dereference it.
	for _, item := range (*result.Knowledgebases) {
		// Most fields of KnowledgebaseDTO are pointers, so we need to dereference them.
		fmt.Println ("ID: " + *item.ID)
		fmt.Println ("Name: " + *item.Name)
		fmt.Println ()
	}
}
// </list_kbs>

// <update_kb>
// Update a knowledge base.
func update_kb (kb_id string) {
	// Get the context, which is required by the SDK methods.
	ctx := context.Background()

	client := qnamaker.NewKnowledgebaseClient(authoring_endpoint)
	// Set the subscription key on the client.
	client.Authorizer = autorest.NewCognitiveServicesAuthorizer(subscription_key)

	// Add new Q&A lists, URLs, and files to the KB.
	/*
	The fields of QnADTO are pointers, and we cannot get the addresses of literal values,
	so we declare helper variables.
	*/
	id := int32(1)
	answer := "You can change the default message if you use the QnAMakerDialog. See this for details: https://docs.botframework.com/en-us/azure-bot-service/templates/qnamaker/#navtitle"
	source := "Custom Editorial"
	questions := []string{ "How can I change the default message from QnA Maker?" }

	// The fields of MetadataDTO are also pointers.
	metadata_name_1 := "category"
	metadata_value_1 := "api"
	metadata := []qnamaker.MetadataDTO{ qnamaker.MetadataDTO{ Name: &metadata_name_1, Value: &metadata_value_1 } }
	qna_list := []qnamaker.QnADTO{ qnamaker.QnADTO{
		ID: &id,
		Answer: &answer,
		Source: &source,
		Questions: &questions,
		Metadata: &metadata,
	} }

	urls := []string{}
	files := []qnamaker.FileDTO{}

	/*
	The fields of UpdateKbOperationDTOAdd, updateKBUpdatePayload, updateKBDeletePayload,
	and UpdateKbOperationDTO are all pointers, so we get the addresses of our variables.
	*/
	updateKBAddPayload := qnamaker.UpdateKbOperationDTOAdd{ QnaList: &qna_list, Urls: &urls, Files: &files }

	// Update the KB name.
	name := "New KB name"
	updateKBUpdatePayload := qnamaker.UpdateKbOperationDTOUpdate { Name: &name }

	// Delete the QnaList with ID 0.
	ids := []int32{ 0 }
	updateKBDeletePayload := qnamaker.UpdateKbOperationDTODelete { Ids: &ids }

	// Bundle the add, update, and delete requests.
	updateKbPayload := qnamaker.UpdateKbOperationDTO{ Add: &updateKBAddPayload, Update: &updateKBUpdatePayload, Delete: &updateKBDeletePayload }

	// Update the KB.
	kb_result, kb_err := client.Update (ctx, kb_id, updateKbPayload)
	if kb_err != nil {
		log.Fatal(kb_err)
	}

	// Wait for the KB update operation to finish.
	fmt.Println ("Waiting for KB update operation to finish...")
	// Operation.OperationID is a pointer, so we need to dereference it.
	wait_for_operation (*kb_result.OperationID)
}
// </update_kb>

// <publish_kb>
// Publish a knowledge base.
func publish_kb (kb_id string) {
	// Get the context, which is required by the SDK methods.
	ctx := context.Background()

	client := qnamaker.NewKnowledgebaseClient(authoring_endpoint)
	// Set the subscription key on the client.
	client.Authorizer = autorest.NewCognitiveServicesAuthorizer(subscription_key)

	_, kb_err := client.Publish (ctx, kb_id)
	if kb_err != nil {
		log.Fatal(kb_err)
	}
	fmt.Println ("KB " + kb_id + " published.")
}
// </publish_kb>

// <download_kb>
// Download a knowledge base.
func download_kb (kb_id string) {
	// Get the context, which is required by the SDK methods.
	ctx := context.Background()

	client := qnamaker.NewKnowledgebaseClient(authoring_endpoint)
	// Set the subscription key on the client.
	client.Authorizer = autorest.NewCognitiveServicesAuthorizer(subscription_key)

	result, kb_err := client.Download (ctx, kb_id, "Prod")
	if kb_err != nil {
		log.Fatal(kb_err)
	}
	fmt.Println ("KB " + kb_id + " downloaded. It contains " + string (len(*result.QnaDocuments)) + " question/answer sets.")
}
// </download_kb>

// <query_kb>
// Send a query to a knowledge base.
func query_kb (kb_id string) {
	// Get the context, which is required by the SDK methods.
	ctx := context.Background()

	client := qnamakerruntime.NewRuntimeClient(runtime_endpoint)

	runtime_key := get_runtime_endpoint_key()
	// Set the runtime key on the client.
	headers := make(map[string]interface{})
	headers["Authorization"] = "EndpointKey " + runtime_key
	client.Authorizer = autorest.NewAPIKeyAuthorizerWithHeaders(headers)

	/*
	The fields of QueryDTO are pointers, and we cannot get the addresses of literal values,
	so we declare helper variables.
	*/
	question := "Is the QnA Maker service free?"
	var answers int32
	answers = 3

	query := qnamakerruntime.QueryDTO {
		Question: &question,
		Top: &answers,
	}

	result, kb_err := client.GenerateAnswer (ctx, kb_id, query)
	if kb_err != nil {
		log.Fatal(kb_err)
	}
	fmt.Println ("Top answers:\n")
	for _, answer := range *result.Answers {
		fmt.Printf ("Answer: %s", *answer.Answer)
		fmt.Printf ("Score: %f\n", *answer.Score)
	}
}
// </query_kb>

// <delete_kb>
// Delete a knowledge base.
func delete_kb (kb_id string) {
	// Get the context, which is required by the SDK methods.
	ctx := context.Background()

	client := qnamaker.NewKnowledgebaseClient(authoring_endpoint)
	// Set the subscription key on the client.
	client.Authorizer = autorest.NewCognitiveServicesAuthorizer(subscription_key)

	_, kb_err := client.Delete (ctx, kb_id)
	if kb_err != nil {
		log.Fatal(kb_err)
	}
	fmt.Println ("KB " + kb_id + " delete.")
}
// </delete_kb>

// <main>
func main() {
	fmt.Println ("Creating KB...")
	kb_id := create_kb()
	fmt.Println ()

	list_kbs()
	fmt.Println ()

	fmt.Println ("Updating KB...")
	update_kb (kb_id)
	fmt.Println ()

	fmt.Println ("Publishing KB...")
	publish_kb (kb_id)
	fmt.Println ()

	fmt.Println ("Downloading KB...")
	download_kb (kb_id)
	fmt.Println ()

	fmt.Println ("Querying KB...")
	query_kb (kb_id)
	fmt.Println()

	fmt.Println ("Deleting KB...")
	delete_kb (kb_id)
	fmt.Println ()
}
// </main>
