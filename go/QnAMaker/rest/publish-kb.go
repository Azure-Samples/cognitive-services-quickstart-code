package main

import (
    "bytes"
    "fmt"
    "log"
    "net/http"
    "os"
)

func main() {
/*
* Configure the local environment:
* Set the QNA_MAKER_SUBSCRIPTION_KEY, QNA_MAKER_ENDPOINT, and QNA_MAKER_KB_ID
* environment variables on your local machine using
* the appropriate method for your preferred shell (Bash, PowerShell, Command
* Prompt, etc.). 
*
* If the environment variable is created after the application is launched in a
* console or with Visual Studio, the shell (or Visual Studio) needs to be closed
* and reloaded to take the environment variable into account.
*/
    if "" == os.Getenv("QNA_MAKER_ENDPOINT") {
        log.Fatal("Please set/export the environment variable QNA_MAKER_ENDPOINT.")
    }
    var endpoint string = os.Getenv("QNA_MAKER_ENDPOINT")

    if "" == os.Getenv("QNA_MAKER_SUBSCRIPTION_KEY") {
        log.Fatal("Please set/export the environment variable QNA_MAKER_SUBSCRIPTION_KEY.")
    }
    var subscription_key string = os.Getenv("QNA_MAKER_SUBSCRIPTION_KEY")

    if "" == os.Getenv("QNA_MAKER_KB_ID") {
        log.Fatal("Please set/export the environment variable QNA_MAKER_KB_ID.")
    }
    var kb_id string = os.Getenv("QNA_MAKER_KB_ID")

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