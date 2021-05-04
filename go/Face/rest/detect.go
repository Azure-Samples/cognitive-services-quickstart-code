package main

import (
	"crypto/tls"
    "encoding/json"
    "fmt"
    "io/ioutil"
    "net/http"
	"os"
    "strings"
    "time"
)

func main() {
	// Add FACE_SUBSCRIPTION_KEY and FACE_ENDPOINT to your environment variables.
	subscriptionKey := os.Getenv("FACE_SUBSCRIPTION_KEY")
	endpoint := os.Getenv("FACE_ENDPOINT")

    const imageUrl =
      "https://upload.wikimedia.org/wikipedia/commons/c/c3/RH_Louise_Lillian_Gish.jpg"

    const params = "?detectionModel=detection_03"
    uri := endpoint + "/face/v1.0/detect" + params
    const imageUrlEnc = "{\"url\":\"" + imageUrl + "\"}"

    reader := strings.NewReader(imageUrlEnc)

    //Configure TLS, etc.
    tr := &http.Transport{
        TLSClientConfig: &tls.Config{
            InsecureSkipVerify: true,
        },
    }
    
    // Create the Http client
    client := &http.Client{
        Transport: tr,
        Timeout: time.Second * 2,
    }

    // Create the Post request, passing the image URL in the request body
    req, err := http.NewRequest("POST", uri, reader)
    if err != nil {
        panic(err)
    }

    // Add headers
    req.Header.Add("Content-Type", "application/json")
    req.Header.Add("Ocp-Apim-Subscription-Key", subscriptionKey)

    // Send the request and retrieve the response
    resp, err := client.Do(req)
    if err != nil {
        panic(err)
    }

    defer resp.Body.Close()

    // Read the response body.
    // Note, data is a byte array
    data, err := ioutil.ReadAll(resp.Body)
    if err != nil {
        panic(err)
    }

    // Parse the Json data
    var f interface{}
    json.Unmarshal(data, &f)

    // Format and display the Json result
    jsonFormatted, _ := json.MarshalIndent(f, "", "  ")
    fmt.Println(string(jsonFormatted))
}
