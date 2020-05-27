//
// This quickstart shows how to predict the intent of an utterance by using the LUIS REST APIs.
//

package main

// Import dependencies.
import (
    "fmt"
    "net/http"
    "net/url"
    "io/ioutil"
    "log"
)

func main() {

    //////////
    // Values to modify.

	// YOUR-APP-ID: The App ID GUID found on the www.luis.ai Application Settings page.
    var appID = "YOUR-APP-ID"

	// YOUR-PREDICTION-KEY: Your LUIS authoring key, 32 character value.
	var predictionKey = "YOUR-PREDICTION-KEY"

    // YOUR-PREDICTION-ENDPOINT: Replace with your authoring key endpoint.
    // For example, "https://westus.api.cognitive.microsoft.com/"
    var predictionEndpoint = "https://YOUR-PREDICTION-ENDPOINT/"

	// utterance for public app
    var utterance = "I want two large pepperoni pizzas on thin crust please"
    //////////

    // Call the prediction endpoint.
	endpointPrediction(appID, predictionKey, predictionEndpoint, utterance)
}

// Calls the prediction endpoint and displays the prediction results on the console.
func endpointPrediction(appID string, predictionKey string, predictionEndpoint string, utterance string) {

    var endpointUrl = fmt.Sprintf("%sluis/prediction/v3.0/apps/%s/slots/production/predict?subscription-key=%s&verbose=true&show-all-intents=true&query=%s", predictionEndpoint, appID, predictionKey, url.QueryEscape(utterance))

    response, err := http.Get(endpointUrl)

    if err != nil {
        // handle error
        fmt.Println("error from Get")
        log.Fatal(err)
    }

    response2, err2 := ioutil.ReadAll(response.Body)

    if err2 != nil {
        // handle error
        fmt.Println("error from ReadAll")
        log.Fatal(err2)
    }

    fmt.Println("response")
    fmt.Println(string(response2))
}