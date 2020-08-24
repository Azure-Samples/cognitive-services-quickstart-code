package main

import (
    "bytes"
    "encoding/json"
    "fmt"
    "io/ioutil"
    "net/http"
    "strconv"
    "time"
)

var host string = "https://{your-resource-name}.api.cognitive.microsoft.com"
var service string = "/qnamaker/v4.0"
var method string = "/knowledgebases/create"

var uri = host + service + method

// Replace this with a valid subscription key.
var subscriptionKey string = "<your-qna-maker-subscription-key>"

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

  type Response struct {
	Headers	map[string][]string
	Body	string
}

func post(uri string, content string) Response {
	req, _ := http.NewRequest("POST", uri, bytes.NewBuffer([]byte(content)))
	req.Header.Add("Ocp-Apim-Subscription-Key", subscriptionKey)
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

func get(uri string) Response {
	req, _ := http.NewRequest("GET", uri, nil)
	req.Header.Add("Ocp-Apim-Subscription-Key", subscriptionKey)
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

func create_kb(uri string, req string) (string, string) {
	fmt.Println("Calling " + uri + ".")
	result := post(uri, req)

	operationIds, exists := result.Headers["Location"]

	if(exists){
		return operationIds[0], result.Body
	} else {
		// error message is in result.Body
		return "", result.Body
	}
}

func check_status(uri string) (string, string) {
	fmt.Println("Calling " + uri + ".")
	result := get(uri)
	if retry, success := result.Headers["Retry-After"]; success {
		return retry[0], result.Body
	} else {
// If the response headers did not include a Retry-After value, default to 30 seconds.
		return "30", result.Body
	}
}

func main() {
	
	operation, body := create_kb(uri, kb)
	fmt.Printf(body + "\n")

	var done bool = false

	for done == false {

		uri := host + service + operation
		wait, status := check_status(uri)
		fmt.Println(status)

		var status_obj map[string]interface{}

		json.Unmarshal([]byte(status), &status_obj)

		state := status_obj["operationState"]

        // If the operation isn't finished, wait and query again.
		if state == "Running" || state == "NotStarted" {

			fmt.Printf ("Waiting " + wait + " seconds...")
			sec, _ := strconv.Atoi(wait)
			time.Sleep (time.Duration(sec) * time.Second)

		} else {
			done = true
		}
	}
}
