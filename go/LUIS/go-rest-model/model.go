//
// This quickstart shows how to add utterances to a LUIS model using the REST APIs.
//

// dependencies
package main
import (
	"fmt"
	"net/http"
	"io/ioutil"
	"log"
	"strings"
)

// main function
func main() {

	//////////
    // Values to modify.

	// YOUR-APP-ID: The App ID GUID found on the www.luis.ai Application Settings page.
	var appID = "PASTE_YOUR_LUIS_APP_ID_HERE"

	// YOUR-AUTHORING-KEY: Your LUIS authoring key, 32 character value.
	var authoringKey = "PASTE_YOUR_LUIS_AUTHORING_SUBSCRIPTION_KEY_HERE"

	//  YOUR-AUTHORING-ENDPOINT: Replace this with your authoring key endpoint.
    // For example, "https://your-resource-name.cognitiveservices.azure.com/"
	var endpoint = "PASTE_YOUR_LUIS_AUTHORING_ENDPOINT_HERE"

	// NOTE: Replace this your version number. The Pizza app uses a version number of "0.1".
	var version = "0.1"
	//////////

	var exampleUtterances = "[{'text': 'order a pizza', 'intentName': 'ModifyOrder', 'entityLabels': [{'entityName': 'Order', 'startCharIndex': 6, 'endCharIndex': 12}]}, {'text': 'order a large pepperoni pizza', 'intentName': 'ModifyOrder', 'entityLabels': [{'entityName': 'Order', 'startCharIndex': 6, 'endCharIndex': 28}, {'entityName': 'FullPizzaWithModifiers', 'startCharIndex': 6, 'endCharIndex': 28}, {'entityName': 'PizzaType', 'startCharIndex': 14, 'endCharIndex': 28}, {'entityName': 'Size', 'startCharIndex': 8, 'endCharIndex': 12}]}, {'text': 'I want two large pepperoni pizzas on thin crust', 'intentName': 'ModifyOrder', 'entityLabels': [{'entityName': 'Order', 'startCharIndex': 7, 'endCharIndex': 46}, {'entityName': 'FullPizzaWithModifiers', 'startCharIndex': 7, 'endCharIndex': 46}, {'entityName': 'PizzaType', 'startCharIndex': 17, 'endCharIndex': 32}, {'entityName': 'Size', 'startCharIndex': 11, 'endCharIndex': 15}, {'entityName': 'Quantity', 'startCharIndex': 7, 'endCharIndex': 9}, {'entityName': 'Crust', 'startCharIndex': 37, 'endCharIndex': 46}]}]"

	fmt.Println("add example utterances requested")
	addUtterance(authoringKey, appID, version, exampleUtterances, endpoint)

	fmt.Println("training selected")
	requestTraining(authoringKey, appID, version, endpoint)

	fmt.Println("training status selected")
	getTrainingStatus(authoringKey, appID, version, endpoint)
}

// Send the list of utterances to the model.
func addUtterance(authoringKey string, appID string,  version string, labeledExampleUtterances string, endpoint string){

	var authoringUrl = fmt.Sprintf("%sluis/authoring/v3.0-preview/apps/%s/versions/%s/examples", endpoint, appID, version)

	httpRequest("POST", authoringUrl, authoringKey, labeledExampleUtterances)
}

// Request training.
func requestTraining(authoringKey string, appID string,  version string, endpoint string){

	trainApp("POST", authoringKey, appID, version, endpoint)
}


func trainApp(httpVerb string, authoringKey string, appID string,  version string, endpoint string){

	var authoringUrl = fmt.Sprintf("%sluis/authoring/v3.0-preview/apps/%s/versions/%s/train", endpoint, appID, version)

	httpRequest(httpVerb,authoringUrl, authoringKey, "")
}


func getTrainingStatus(authoringKey string, appID string, version string, endpoint string){

	trainApp("GET", authoringKey, appID, version, endpoint)
}

// generic HTTP request
// includes setting header with authoring key
func httpRequest(httpVerb string, url string, authoringKey string, body string){

	client := &http.Client{}

	request, err := http.NewRequest(httpVerb, url, strings.NewReader(body))
	request.Header.Add("Ocp-Apim-Subscription-Key", authoringKey)

	fmt.Println("body")
	fmt.Println(body)

	response, err := client.Do(request)

	if err != nil {
		log.Fatal(err)
	} else {
		defer response.Body.Close()

		contents, err := ioutil.ReadAll(response.Body)

		if err != nil {
			log.Fatal(err)
		}

		fmt.Println("   ", response.StatusCode)
		fmt.Println(string(contents))
	}
}