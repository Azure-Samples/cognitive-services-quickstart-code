/*  The examples in this quickstart are for the Computer Vision API for Microsoft
 *  Cognitive Services with the following tasks:
 *  - Recognizing printed and handwritten text with the Batch Read API
 *	- Recognizing printed text with OCR
 *
 *  Prerequisites:
 *    Import the required libraries. From the command line, you will need to 'go get' 
 *    the azure-sdk-for-go and go-autorest packages from Github.
 *    For example:
 *	  go get github.com/Azure/azure-sdk-for-go/services/cognitiveservices/v2.0/computervision
 * 
 *    Download images faces.jpg, handwritten_text.jpg, objects.jpg, cheese_clipart.png,
 *    printed_text.jpg, and gray-shirt-logo.jpg, then add to your root folder from here:
 *    https://github.com/Azure-Samples/cognitive-services-sample-data-files/tree/master/ComputerVision/Images
 *   
 *  How to run:
 *	  From command line: go run ComptuerVisionQuickstart.go
 * 
 *  References:
 *    - SDK reference: 
 *      https://godoc.org/github.com/Azure/azure-sdk-for-go/services/cognitiveservices/v2.0/computervision
 *    - Computer Vision documentation:
 * 		https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/index
 *    - Computer Vision API: 
 *      https://westus.dev.cognitive.microsoft.com/docs/services/5cd27ec07268f6c679a3e641/operations/56f91f2e778daf14a499f21b
 */

// <snippet_single>
package main

import (
	"context"
	"encoding/json"
	"fmt"
	"github.com/Azure/azure-sdk-for-go/services/cognitiveservices/v2.0/computervision"
	"github.com/Azure/go-autorest/autorest"
	"io"
	"log"
	"os"
	"strings"
	"time"
)
// Declare global so don't have to pass it to all of the tasks.
var computerVisionContext context.Context

func main() {
	computerVisionKey := "PASTE_YOUR_COMPUTER_VISION_SUBSCRIPTION_KEY_HERE"
	endpointURL := "PASTE_YOUR_COMPUTER_VISION_ENDPOINT_HERE"

	/*
	 * URL images
	 */ 
	printedImageURL := "https://raw.githubusercontent.com/MicrosoftDocs/azure-docs/master/articles/cognitive-services/Computer-vision/Images/readsample.jpg"
	/*
	 * END - URL images
	 */

	/*  
	 * Configure the Computer Vision client
	 */
	computerVisionClient := computervision.New(endpointURL);
	computerVisionClient.Authorizer = autorest.NewCognitiveServicesAuthorizer(computerVisionKey)

	computerVisionContext = context.Background()
	/*
	 * END - Configure the Computer Vision client
	 */

	// <snippet_readinmain>
	// Analyze text in an image, remote
	BatchReadFileRemoteImage(computerVisionClient, printedImageURL)
	
}

/*  
 * Batch Read File - remote
 * A new way to recognize text, with more accurate results than the RecognizeText API call.
 */

 func BatchReadFileRemoteImage(client computervision.BaseClient, remoteImageURL string) {
	fmt.Println("-----------------------------------------")
	fmt.Println("BATCH READ FILE - remote")
	fmt.Println()
	var remoteImage computervision.ImageURL
	remoteImage.URL = &remoteImageURL

	// The response contains a field called "Operation-Location", 
	// which is a URL with an ID that you'll use for GetReadOperationResult to access OCR results.
	textHeaders, err := client.BatchReadFile(computerVisionContext, remoteImage)
	if err != nil { log.Fatal(err) }

	// Use ExtractHeader from the autorest library to get the Operation-Location URL
	operationLocation := autorest.ExtractHeaderValue("Operation-Location", textHeaders.Response)

	numberOfCharsInOperationId := 36
	operationId := string(operationLocation[len(operationLocation)-numberOfCharsInOperationId : len(operationLocation)])

	readOperationResult, err := client.GetReadOperationResult(computerVisionContext, operationId)
	if err != nil { log.Fatal(err) }

	// Wait for the operation to complete.
	i := 0
	maxRetries := 10

	fmt.Println("Recognizing text in a remote image with the batch Read API ...")
	for readOperationResult.Status != computervision.Failed &&
			readOperationResult.Status != computervision.Succeeded {
		if i >= maxRetries {
			break
		}
		i++

		fmt.Printf("Server status: %v, waiting %v seconds...\n", readOperationResult.Status, i)
		time.Sleep(1 * time.Second)

		readOperationResult, err = client.GetReadOperationResult(computerVisionContext, operationId)
		if err != nil { log.Fatal(err) }
	}

	// Display the results.
	fmt.Println()
	for _, recResult := range *(readOperationResult.RecognitionResults) {
		for _, line := range *recResult.Lines {
			fmt.Println(*line.Text)
		}
	}
	fmt.Println()
}
// </snippet_single>
