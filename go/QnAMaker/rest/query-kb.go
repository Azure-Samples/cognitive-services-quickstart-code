package main

// <dependencies>
import (
    "bytes"
    "encoding/json"
    "fmt"
    "io/ioutil"
    "net/http"
    "strconv"
)
// </dependencies>

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
*
* Set the `kb_id` variable to the ID of a knowledge base you have
* previously created.
*/

// <main>
func main() {
	var subscription_key string = "PASTE_YOUR_QNA_MAKER_SUBSCRIPTION_KEY_HERE"
	var authoring_endpoint string = "PASTE_YOUR_QNA_MAKER_AUTHORING_ENDPOINT_HERE"
	var runtime_endpoint string = "PASTE_YOUR_QNA_MAKER_RUNTIME_ENDPOINT_HERE"
    var kb_id string = "PASTE_YOUR_QNA_MAKER_KB_ID_HERE"

	// Get the primary endpoint key for this subscription.
	var get_runtime_key_uri = authoring_endpoint + "/qnamaker/v4.0/endpointkeys"
	req, _ := http.NewRequest("GET", get_runtime_key_uri, nil)
	req.Header.Add("Ocp-Apim-Subscription-Key", subscription_key)
	client := &http.Client{}
	response, err := client.Do(req)
	if err != nil {
		panic(err)
	}
	defer response.Body.Close()
    body, _ := ioutil.ReadAll(response.Body)
	var response_body map[string]interface{}
	json.Unmarshal([]byte(body), &response_body)
	endpoint_key := response_body["primaryEndpointKey"].(string)

    // JSON format for passing question to service
    var question string = "{'question': 'Is the QnA Maker Service free?','top': 3}"

	// Send the query.
    var query_kb_uri string = runtime_endpoint + "/qnamaker/knowledgebases/" + kb_id + "/generateAnswer";
    req, _ = http.NewRequest("POST", query_kb_uri, bytes.NewBuffer([]byte(question)))
	// Note this differs from the "Ocp-Apim-Subscription-Key"/<subscription key> used by most Cognitive Services.
    req.Header.Add("Authorization", "EndpointKey " + endpoint_key)
    req.Header.Add("Content-Type", "application/json")
    req.Header.Add("Content-Length", strconv.Itoa(len(question)))
    client = &http.Client{}
    response, err = client.Do(req)
    if err != nil {
        panic(err)
    }
    defer response.Body.Close()
    body, _ = ioutil.ReadAll(response.Body)

    fmt.Printf(string(body) + "\n")
}
// </main>
