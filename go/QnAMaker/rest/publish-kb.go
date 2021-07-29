package main

// <dependencies>
import (
    "bytes"
    "fmt"
    "net/http"
)
// </dependencies>

// <main>
func main() {
/*
* Set the `subscription_key` and `endpoint` variables to your
* QnA Maker authoring subscription key and endpoint.
*
* These values can be found in the Azure portal (ms.portal.azure.com/).
* Look up your QnA Maker resource. Then, in the "Resource management"
* section, find the "Keys and Endpoint" page.
*
* The value of `endpoint` has the format https://YOUR-RESOURCE-NAME.cognitiveservices.azure.com.
*
* Set the `kb_id` variable to the ID of a knowledge base you have
* previously created.
*/
	var subscription_key string = "PASTE_YOUR_QNA_MAKER_AUTHORING_SUBSCRIPTION_KEY_HERE"
	var endpoint string = "PASTE_YOUR_QNA_MAKER_AUTHORING_ENDPOINT_HERE"
    var kb_id string = "PASTE_YOUR_QNA_MAKER_KB_ID_HERE"

    var service string = "/qnamaker/v4.0"
    var method string = "/knowledgebases/"
    var uri = endpoint + service + method + kb_id

    var content = bytes.NewBuffer([]byte(nil));

    req, _ := http.NewRequest("POST", uri, content)

    req.Header.Add("Ocp-Apim-Subscription-Key", subscription_key)

    client := &http.Client{}
    response, err := client.Do(req)
    if err != nil {
        panic(err)
    }
    // print 204 - success code
    fmt.Println(response.StatusCode)
}
// </main>
