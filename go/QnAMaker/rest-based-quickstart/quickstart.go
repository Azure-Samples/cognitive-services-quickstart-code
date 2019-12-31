package main

// <snippet_dependencies>
import (
    "bytes"
    "encoding/json"
	"fmt"
	//"os"
    "io/ioutil"
    "net/http"
    "strconv"
	"time"
	"strings"
)
// </snippet_dependencies>

/* This sample for the Azure Cognitive Services QnA Maker API shows how to:
 * - Create an knowledge base.
 * - Get Operation status.
 * - Publish knowledge base - required to query knowledge base.
 * - Get endpoint key - required to query knowledge base.
 * - Query knowledge base.
 */

/*
 * Configure the local environment:
 *   Set the following environment variables on your local machine using the
 *   appropriate method for your preferred shell (Bash, PowerShell, Command Prompt, etc.).
 *
 *   QNAMAKER_AUTHORING_KEY
 *   QNAMAKER_AUTHORING_ENDPOINT
 *   QNAMAKER_RESOURCE_NAME
 *
 * If the environment variable is created after the application is launched in a console or with Visual
 * Studio, the shell (or Visual Studio) needs to be closed and reloaded for the environment variable to take effect.
 */

 // <snippet_response_structure>
 /// Represents the HTTP response returned by an HTTP request.
 type Response struct {
	Headers	map[string][]string
	Status  string
    Body	string
}
// </snippet_response_structure>

// <snippet_main>
func main() {

	var authoring_key string = os.Getenv("QNAMAKER_AUTHORING_KEY")

	// example - "MY-RESOURCE_NAME"
	var resource_name string =  os.Getenv("QNAMAKER_RESOURCE_NAME")

	// example: "https://MY-RESOURCE_NAME.cognitiveservices.azure.com/"
	var authoring_endpoint string = os.Getenv("QNAMAKER_AUTHORING_ENDPOINT")

	var service string = "qnamaker/v4.0"

	// 1) Create knowledge base - get knowledge base ID
	var knowledgeBaseID string = create_knowledgebase(service, authoring_key, authoring_endpoint)
	fmt.Println("knowledgeBaseID = " + knowledgeBaseID)

	// 2) Publish knowledge base
	publish(service, authoring_key, authoring_endpoint, knowledgeBaseID)

	// 3) Get published knowledge base's endpoint key
	var endpointKey string = getPublishedEndpoint(service, authoring_key, authoring_endpoint)
	fmt.Println("endpointKey = " + endpointKey)

	// 4) Query published knowledge base
	queryEndpoint(resource_name, endpointKey, knowledgeBaseID)
}
// </snippet_main>

// <snippet_get>
func get(key string, uri string) Response {

    req, _ := http.NewRequest("GET", uri, nil)
    req.Header.Add("Ocp-Apim-Subscription-Key", key)
    client := &http.Client{}
    response, err := client.Do(req)
    if err != nil {
        panic(err)
	}

    defer response.Body.Close()
    body, _ := ioutil.ReadAll(response.Body)
    return Response {response.Header, response.Status, string(body)}
}
// </snippet_get>

// <snippet_post>
// authoring param determines which authorization header to pass
func post(key string, uri string, content string, authoring bool) Response {

	var postContent = bytes.NewBuffer([]byte(content))
	var postContentLength = strconv.Itoa(len(content))
	var authorizationName string = ""
	var authorizationValue string =  ""

    req, _ := http.NewRequest("POST", uri, postContent)

	if authoring {
		authorizationName = "Ocp-Apim-Subscription-Key"
		authorizationValue = key
	} else {
		authorizationName = "Authorization"
		authorizationValue =  "EndpointKey " + key
	}

	req.Header.Add(authorizationName, authorizationValue)

	if(len(content)>0){
		req.Header.Add("Content-Type", "application/json")
		req.Header.Add("Content-Length", postContentLength)
	}

    client := &http.Client{}
    response, err := client.Do(req)
    if err != nil {
        panic(err)
	}

    defer response.Body.Close()
    body, _ := ioutil.ReadAll(response.Body)

    return Response {response.Header, response.Status, string(body)}
}
// </snippet_post>

// <snippet_check_status>
func check_status(authoring_key string, uri string) (string, string) {

	result := get(authoring_key, uri)

    if retry, success := result.Headers["Retry-After"]; success {
        return retry[0], result.Body
    } else {
        // If the response headers did not include a Retry-After value, default to 30 seconds.
        return "30", result.Body
    }
}
// </snippet_check_status>

// <snippet_create_knowledgebase>
func create_knowledgebase(service string, authoring_key string, authoring_endpoint string)(string){

	var authoring = true
	var uri string = authoring_endpoint + service + "/knowledgebases/create"

	var knowledgeBaseID string = ""

	var knowledgeBaseDef string = `{
		'name': 'QnA Maker FAQ',
		'qnaList': [
		  {
			'id': 0,
			'answer': 'You can use our REST APIs to manage your knowledge base. See here for details: https://westus.dev.cognitive.microsoft.com/docs/services/58994a073d9e04097c7ba6fe/operations/58994a073d9e041ad42d9baa',
			'source': 'Custom Editorial',
			'questions': [
			  'How do I programmatically update my knowledge base?'
			],
			'metadata': [
			  {
				'name': 'category',
				'value': 'api'
			  }
			]
		  }
		],
		'urls': [
		  'https://docs.microsoft.com/en-in/azure/cognitive-services/qnamaker/faqs'
		],
		'files': []
	  }`;

	result := post(authoring_key, uri, knowledgeBaseDef, authoring)

	operationIds := result.Headers["Location"]

	var done bool = false

	// wait for creation to complete
	for done == false {

        uri_checkstatus := authoring_endpoint + service + operationIds[0]
		wait, httpResponseBody := check_status(authoring_key, uri_checkstatus)

		// get property value from returned body's JSON
        var status_obj map[string]interface{}
        json.Unmarshal([]byte(httpResponseBody), &status_obj)
        state := status_obj["operationState"]

        // If the operation isn't finished, wait and query again.
        if state == "Running" || state == "NotStarted" {

            fmt.Printf ("Waiting " + wait + " seconds...")
            sec, _ := strconv.Atoi(wait)
            time.Sleep (time.Duration(sec) * time.Second)

        } else {
			done = true
			var knowledgeBaseResourceLocation = status_obj["resourceLocation"].(string)
			knowledgeBaseID = strings.ReplaceAll(knowledgeBaseResourceLocation, "/knowledgebases/", "")
        }
	}

	return knowledgeBaseID

}
// </snippet_create_knowledgebase>

// <snippet_publish>
func publish(service string, authoring_key string, authoring_endpoint string, knowledgeBaseId string){

	var authoring = true
	var uri string = authoring_endpoint + service + "/knowledgebases/" + knowledgeBaseId

	result := post(authoring_key, uri, "", authoring)

	// no data returned so check HTTP status 204
	fmt.Println("publish status code should be 204  = " + result.Status)
}
// </snippet_publish>

// <snippet_get_published_endpoint_key>
func getPublishedEndpoint(service string, authoring_key string, authoring_endpoint string)(string){

	var uri string = authoring_endpoint + service + "/endpointkeys"

	result := get(authoring_key, uri)

	// get property value from returned body's JSON
	var status_obj map[string]interface{}
	json.Unmarshal([]byte(result.Body), &status_obj)
	var endpointKey = status_obj["primaryEndpointKey"].(string)

	return endpointKey
}
// </snippet_get_published_endpoint_key>

// <snippet_query>
// query runtime instead of authoring
func queryEndpoint(resourceName string, endpoint_key string, knowledgeBaseId string){

	var authoring = false
	var uri string = "https://" + resourceName + ".azurewebsites.net/qnamaker/knowledgebases/" + knowledgeBaseId + "/generateAnswer";

	var question string = `{'question': 'Is the QnA Maker Service free?','top': 3}`;

	result := post(endpoint_key, uri, question, authoring)

	fmt.Println("result body = " + result.Body)
}
// </snippet_query>