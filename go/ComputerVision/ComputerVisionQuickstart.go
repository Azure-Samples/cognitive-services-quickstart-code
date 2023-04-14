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

// <snippet_imports_and_vars>
// <snippet_imports>
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
// </snippet_imports>

// <snippet_context>
// Declare global so don't have to pass it to all of the tasks.
var computerVisionContext context.Context
// </snippet_context>

func main() {
	// <snippet_vars>
	computerVisionKey := "PASTE_YOUR_COMPUTER_VISION_KEY_HERE"
	endpointURL := "PASTE_YOUR_COMPUTER_VISION_ENDPOINT_HERE"
	// </snippet_vars>
	// </snippet_imports_and_vars>
	
	/*
	 * Local image file I/O
	 * Store these in your root directory in a "resources" folder.
	 */ 
	handwritingImagePath := "resources\\handwritten_text.jpg"
	printedImagePath := "resources\\printed_text.jpg"
	/*
	 * END - Local image file I/O
	 */

	/*
	 * URL images
	 */ 
	printedImageURL := "https://raw.githubusercontent.com/MicrosoftDocs/azure-docs/master/articles/cognitive-services/Computer-vision/Images/readsample.jpg"
	/*
	 * END - URL images
	 */

	// <snippet_client>
	/*  
	 * Configure the Computer Vision client
	 */
	computerVisionClient := computervision.New(endpointURL);
	computerVisionClient.Authorizer = autorest.NewCognitiveServicesAuthorizer(computerVisionKey)

	computerVisionContext = context.Background()
	/*
	 * END - Configure the Computer Vision client
	 */
	// </snippet_client>


	// Analyze text in an image, local
	BatchReadFileLocalImage(computerVisionClient, handwritingImagePath)
	// <snippet_readinmain>
	// Analyze text in an image, remote
	BatchReadFileRemoteImage(computerVisionClient, printedImageURL)
	// </snippet_readinmain>

	// Analyze printed text in an image, local and remote
	RecognizePrintedOCRLocalImage(computerVisionClient, printedImagePath)
	RecognizePrintedOCRRemoteImage(computerVisionClient, printedImageURL)

	fmt.Println("-----------------------------------------")
	fmt.Println("End of quickstart.")
}

/*  
 * Batch Read File - local
 * A new way to recognize text, with more accurate results than the RecognizeText API call.
 */
func BatchReadFileLocalImage(client computervision.BaseClient, localImagePath string) {
	fmt.Println("-----------------------------------------")
	fmt.Println("BATCH READ FILE - local")
	fmt.Println()
	var localImage io.ReadCloser
	localImage, err := os.Open(localImagePath)
	if err != nil { log.Fatal(err) }

	//	When you use the Read Document interface, the response contains a field
	//	called "Operation-Location", which contains the URL to use for your
	//	GetReadOperationResult to access OCR results.
	textHeaders, err := client.BatchReadFileInStream(computerVisionContext, localImage)
	if err != nil { log.Fatal(err) }

	//	Use ExtractHeader from the autorest library to get the Operation-Location URL
	operationLocation := autorest.ExtractHeaderValue("Operation-Location", textHeaders.Response)

	numberOfCharsInOperationId := 36
	operationId := string(operationLocation[len(operationLocation)-numberOfCharsInOperationId : len(operationLocation)])

	readOperationResult, err := client.GetReadOperationResult(computerVisionContext, operationId)
	if err != nil { log.Fatal(err) }

	// Wait for the operation to complete.
	i := 0
	maxRetries := 10

	fmt.Println("Recognizing text in a local image with the batch Read API ...")
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
/*
 * END - Recognize text with the Read API in a local image
 */ 


/*  
 * Batch Read File - remote
 * A new way to recognize text, with more accurate results than the RecognizeText API call.
 */
// <snippet_read_call>
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
	// </snippet_read_call>

	// <snippet_read_response>
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
	// </snippet_read_response>

	// <snippet_read_display>
	// Display the results.
	fmt.Println()
	for _, recResult := range *(readOperationResult.RecognitionResults) {
		for _, line := range *recResult.Lines {
			fmt.Println(*line.Text)
		}
	}
	// </snippet_read_display>
	fmt.Println()
}

/*  
 * Recognize Printed Text with OCR - local
 */
func RecognizePrintedOCRLocalImage(client computervision.BaseClient, localImagePath string) {
	fmt.Println("-----------------------------------------")
	fmt.Println("RECOGNIZE PRINTED TEXT - local")
	fmt.Println()
	var localImage io.ReadCloser
	localImage, err := os.Open(localImagePath)
	if err != nil { log.Fatal(err) }

	fmt.Println("Recognizing text in a local image with OCR ...")
	ocrResult, err := client.RecognizePrintedTextInStream(computerVisionContext, true, localImage, computervision.En)
	if err != nil { log.Fatal(err) }

	// Get orientation of text.
	fmt.Printf("Text angle: %.4f\n", *ocrResult.TextAngle)

	// Get bounding boxes for each line of text and print text.
	for _, region := range *ocrResult.Regions {
		for _, line := range *region.Lines {
			fmt.Printf("\nBounding box: %v\n", *line.BoundingBox)
			s := ""
			for _, word := range *line.Words {
				s += *word.Text + " "
			}
			fmt.Printf("Text: %v", s)
		}
	}
	fmt.Println()
	fmt.Println()
}

/*  
 *  Recognize Printed Text with OCR - remote
 */
func RecognizePrintedOCRRemoteImage(client computervision.BaseClient, remoteImageURL string) {
	fmt.Println("-----------------------------------------")
	fmt.Println("RECOGNIZE PRINTED TEXT - remote")
	fmt.Println()
	var remoteImage computervision.ImageURL
	remoteImage.URL = &remoteImageURL

	fmt.Println("Recognizing text in a remote image with OCR ...")
	ocrResult, err := client.RecognizePrintedText(computerVisionContext, true, remoteImage, computervision.En)
	if err != nil { log.Fatal(err) }

	// Get orientation of text.
	fmt.Printf("Text angle: %.4f\n", *ocrResult.TextAngle)

	// Get bounding boxes for each line of text and print text.
	for _, region := range *ocrResult.Regions {
		for _, line := range *region.Lines {
			fmt.Printf("\nBounding box: %v\n", *line.BoundingBox)
			s := ""
			for _, word := range *line.Words {
				s += *word.Text + " "
			}
			fmt.Printf("Text: %v", s)
		}
	}
	fmt.Println()
	fmt.Println()
}
/*
 * END - Recognize Printed Text with OCR 
 */
