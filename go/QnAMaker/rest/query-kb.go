package main

import (
    "bytes"
	"encoding/json"
    "fmt"
    "io/ioutil"
    "log"
    "net/http"
    "os"
    "strconv"
)

/*
* Configure the local environment:
* Set the QNA_MAKER_SUBSCRIPTION_KEY, QNA_MAKER_ENDPOINT,
* QNA_MAKER_RUNTIME_ENDPOINT, and QNA_MAKER_KB_ID
* environment variables on your local machine using
* the appropriate method for your preferred shell (Bash, PowerShell, Command
* Prompt, etc.). 
*
* If the environment variable is created after the application is launched in a
* console or with Visual Studio, the shell (or Visual Studio) needs to be closed
* and reloaded to take the environment variable into account.
*/

func main() {
    if "" == os.Getenv("QNA_MAKER_ENDPOINT") {
        log.Fatal("Please set/export the environment variable QNA_MAKER_ENDPOINT.")
    }
    var authoring_endpoint string = os.Getenv("QNA_MAKER_ENDPOINT")

    if "" == os.Getenv("QNA_MAKER_RUNTIME_ENDPOINT") {
        log.Fatal("Please set/export the environment variable QNA_MAKER_RUNTIME_ENDPOINT.")
    }
    var runtime_endpoint string = os.Getenv("QNA_MAKER_RUNTIME_ENDPOINT")

    if "" == os.Getenv("QNA_MAKER_SUBSCRIPTION_KEY") {
        log.Fatal("Please set/export the environment variable QNA_MAKER_SUBSCRIPTION_KEY.")
    }
    var subscription_key string = os.Getenv("QNA_MAKER_SUBSCRIPTION_KEY")

    if "" == os.Getenv("QNA_MAKER_KB_ID") {
        log.Fatal("Please set/export the environment variable QNA_MAKER_KB_ID.")
    }
    var knowledge_base_id string = os.Getenv("QNA_MAKER_KB_ID")

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
    var query_kb_uri string = runtime_endpoint + "/qnamaker/knowledgebases/" + knowledge_base_id + "/generateAnswer";
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