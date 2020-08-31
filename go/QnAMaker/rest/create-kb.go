package main

// <dependencies>
import (
    "bytes"
    "encoding/json"
    "fmt"
    "io/ioutil"
	"log"
    "net/http"
	"os"
    "strconv"
    "time"
)
// </dependencies>

// <model>
var kb string = `{
	"name": "QnA Maker FAQ",
	"qnaList": [
	  {
		"id": 0,
		"answer": "You can use our REST APIs to manage your Knowledge Base. See here for details: https://westus.dev.cognitive.microsoft.com/docs/services/58994a073d9e04097c7ba6fe/operations/58994a073d9e041ad42d9baa",
		"source": "Custom Editorial",
		"questions": [
		  "How do I programmatically update my Knowledge Base?"
		],
		"metadata": [
		  {
			"name": "category",
			"value": "api"
		  }
		]
	  }
	],
	"urls": [
		"https://docs.microsoft.com/en-in/azure/cognitive-services/qnamaker/faqs"
	],
	"files": []
  }`;
// </model>

// <response>
type Response struct {
	Headers	map[string][]string
	Body	string
}
// </response>

// <post>
func post(uri string, content string, subscription_key string) Response {
	req, _ := http.NewRequest("POST", uri, bytes.NewBuffer([]byte(content)))
	req.Header.Add("Ocp-Apim-Subscription-Key", subscription_key)
	req.Header.Add("Content-Type", "application/json")
	req.Header.Add("Content-Length", strconv.Itoa(len(content)))
	client := &http.Client{}
	response, err := client.Do(req)
	if err != nil {
		panic(err)
	}

	defer response.Body.Close()
	body, _ := ioutil.ReadAll(response.Body)

	return Response {response.Header, string(body)}
}
// </post>

// <get>
func get(uri string, subscription_key string) Response {
	req, _ := http.NewRequest("GET", uri, nil)
	req.Header.Add("Ocp-Apim-Subscription-Key", subscription_key)
	client := &http.Client{}
	response, err := client.Do(req)
	if err != nil {
		panic(err)
	}

	defer response.Body.Close()
	body, _ := ioutil.ReadAll(response.Body)
	fmt.Println("get body -----------------------------------------------------")
	fmt.Println(string(body))
	fmt.Println(response.Header)
	return Response {response.Header, string(body)}
}
// </get>

// <create_kb>
func create_kb(uri string, req string, subscription_key string) (string, string) {
	fmt.Println("Calling " + uri + ".")
	result := post(uri, req, subscription_key)

	operationIds, exists := result.Headers["Location"]

	if(exists){
		return operationIds[0], result.Body
	} else {
		// error message is in result.Body
		return "", result.Body
	}
}
// </create_kb>

// <get_status>
func check_status(uri string, subscription_key string) (string) {
	fmt.Println("Calling " + uri + ".")
	result := get(uri, subscription_key)
	return result.Body
}
// </get_status>

// <main>
func main() {
/*
* Configure the local environment:
* Set the QNA_MAKER_SUBSCRIPTION_KEY and QNA_MAKER_ENDPOINT
* environment variables on your local machine using
* the appropriate method for your preferred shell (Bash, PowerShell, Command
* Prompt, etc.). 
*
* If the environment variable is created after the application is launched in a
* console or with Visual Studio, the shell (or Visual Studio) needs to be closed
* and reloaded to take the environment variable into account.
*/
	if "" == os.Getenv("QNA_MAKER_SUBSCRIPTION_KEY") {
		log.Fatal("Please set/export the environment variable QNA_MAKER_SUBSCRIPTION_KEY.")
	}
	var subscription_key string = os.Getenv("QNA_MAKER_SUBSCRIPTION_KEY")
	if "" == os.Getenv("QNA_MAKER_ENDPOINT") {
		log.Fatal("Please set/export the environment variable QNA_MAKER_ENDPOINT.")
	}
	var endpoint string = os.Getenv("QNA_MAKER_ENDPOINT")

	var service string = "/qnamaker/v4.0"
	var method string = "/knowledgebases/create"
	var uri = endpoint + service + method
	
	operation, body := create_kb(uri, kb, subscription_key)
	fmt.Printf(body + "\n")

	var done bool = false
	for done == false {
		uri := endpoint + service + operation
		status := check_status(uri, subscription_key)
		fmt.Println(status)

		var status_obj map[string]interface{}

		json.Unmarshal([]byte(status), &status_obj)

		state := status_obj["operationState"]

        // If the operation isn't finished, wait and query again.
		if state == "Running" || state == "NotStarted" {

			fmt.Printf ("Waiting 10 seconds...")
			time.Sleep (10 * time.Second)

		} else {
			done = true
		}
	}
}
// </main>