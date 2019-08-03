package main

import (

	"bufio"
	"context"
	"encoding/json"
	"fmt"
	"github.com/Azure/azure-sdk-for-go/services/cognitiveservices/v1.0/contentmoderator"
	"github.com/Azure/go-autorest/autorest"
	"github.com/satori/go.uuid"
	"io/ioutil"
	"log"
	"os"
	"path"
	"path/filepath"
	"time"
)

/*
Content Moderator Quickstart

This quickstart shows you how to:
	- moderate images using a human reviewer

Prerequisites:
	- Go 1.12+ 
	- Install the Content Moderator library in command line: 
		go get github.com/Azure/azure-sdk-for-go/services/cognitiveservices/v1.0/contentmoderator
	- For the Human Reviews example, you need an account and team name on the Content Moderator website.
	  Sign up or sign in here: https://contentmoderator.cognitive.microsoft.com/dashboard
	- Set your keys, endpoints, team name, callback endpoint in your environment variables, for the Authentication sections.
How to run:
	- In command line from root folder: go run ContentModeratorQuickstart.go
References:
	- Content Moderator website: https://contentmoderator.cognitive.microsoft.com/dashboard
	- Documentation: https://docs.microsoft.com/en-us/azure/cognitive-services/content-moderator/
	- SDK: https://godoc.org/github.com/Azure/azure-sdk-for-go/services/cognitiveservices/v1.0/contentmoderator
	- API: https://docs.microsoft.com/en-us/azure/cognitive-services/content-moderator/api-reference
*/

func main() {

	// A global context for all examples
	context := context.Background()

	// Credentials used for all examples
	// Get your Content Moderator subscription key from Azure, set into your environment variables
	subscriptionKey := os.Getenv("CONTENT_MODERATOR_SUBSCRIPTION_KEY")
	// Add your Content Moderator endpoint here
	endpoint := os.Getenv("CONTENT_MODERATOR_ENDPOINT")

	// IMAGE MODERATION variables
	var imageModerationList = []string {"https://moderatorsampleimages.blob.core.windows.net/samples/sample2.jpg",
										"https://moderatorsampleimages.blob.core.windows.net/samples/sample5.png"}

	// TEXT MODERATION variable
	textFileName := "content_moderator_text_moderation.txt"

	// HUMAN REVIEWS variables
	// Add your team name (created when you started your Content Moderator website account) 
	// Find from Content Moderator website on Credentials page (it's the Id)
	teamName := os.Getenv("CONTENT_MODERATOR_TEAM_NAME")
	// Using a group of people photo to moderate
	imageURL := "https://moderatorsampleimages.blob.core.windows.net/samples/sample5.png"

	/*
	AUTHENTICATE - IMAGE MODERATION
	Create a special client for image moderation
	*/
	clientImage := contentmoderator.NewImageModerationClient(endpoint)
	clientImage.Authorizer = autorest.NewCognitiveServicesAuthorizer(subscriptionKey)
	/*
	END - Authenticate for image moderation
	*/

	/*
	AUTHENTICATE - TEXT MODERATION
	Create a special client for text moderation
	*/
	clientText := contentmoderator.NewTextModerationClient(endpoint)
	clientText.Authorizer = autorest.NewCognitiveServicesAuthorizer(subscriptionKey)
	/*
	END - Authenticate for text moderation
	*/

	/*
	AUTHENTICATE - HUMAN REVIEWS
	Create a special client for human reviews
	*/
	// Get your callback endpoint from https://{YOUR REGION}.contentmoderator.cognitive.microsoft.com
	// Find it in the Admin --> Credentials page
	callbackEndpoint := "https://westus.api.cognitive.microsoft.com/contentmoderator/review/v1.0"

	clientReviews := contentmoderator.NewReviewsClient(endpoint)
	clientReviews.Authorizer = autorest.NewCognitiveServicesAuthorizer(subscriptionKey)
	/*
	END - Authenticate for human reviews
	*/

	/*
	IMAGE MODERATION
	Moderate URL images for adult or racy content.
	*/
	fmt.Println()
	fmt.Println("------------------------------")
	fmt.Println("IMAGE MODERATION")
	fmt.Println("Evaluating the image " + path.Base(imageModerationList[0]) + " for adult and racy content ...")

	// Moderate the first image in the list. Returns an Evaluate struct.
	dataRepImage := "URL"
	imageData := contentmoderator.ImageURL { DataRepresentation: &dataRepImage, Value: &imageModerationList[0] }
	cacheImage := true
	moderatedImage, miErr := clientImage.EvaluateURLInput(context, "application/json", imageData, &cacheImage)
	if miErr != nil { log.Fatal(miErr) }
	// Print moderated image results
	printImage, _ := json.MarshalIndent(moderatedImage, "", "\t") // Converts struct into JSON
	fmt.Println("Moderated image:")
	fmt.Println(string(printImage))
	fmt.Println();

	// Moderate the first image's text content. Returns an OCR struct.
	enhanced := true // returns additional properties
	moderatedImageText, mitErr := clientImage.OCRURLInput(context, "eng", "application/json", imageData, &cacheImage, &enhanced)
	if mitErr != nil { log.Fatal(mitErr) }
	// Print moderated image text results
	printImageText, _ := json.MarshalIndent(moderatedImageText, "", "\t") // Converts struct into JSON
	fmt.Println("Moderated text in an image:");
	fmt.Println(string(printImageText))
	fmt.Println();

	// Moderate the second image's faces. Returns a FoundFaces struct.
	imageDataFaces := contentmoderator.ImageURL { DataRepresentation: &dataRepImage, Value: &imageModerationList[1] }
	moderatedImageFaces, mifErr := clientImage.FindFacesURLInput(context, "application/json", imageDataFaces, &cacheImage)
	if mifErr != nil { log.Fatal(mifErr) }
	// Print moderated image faces results
	printFaces, _ := json.MarshalIndent(moderatedImageFaces, "", "\t") // Converts struct into JSON
	fmt.Println("Moderated faces in an image:");
	fmt.Println(string(printFaces))
	/*
	END - Image Moderation
	*/

	/*
	TEXT MODERATION
	Moderates text from a file for adult/racy content, PII (personally idenitfiable information), and foul language.
	*/
	fmt.Println()
	fmt.Println("------------------------------")
	fmt.Println("TEXT MODERATION")
	fmt.Println("Evaluating the text file " + textFileName + " ...")

	// Read text file
	workingDirectory, _ := os.Getwd()
	textFilePath := filepath.Join(workingDirectory + "\\" + textFileName)
	file, fErr := os.Open(textFilePath)
	if fErr != nil { log.Fatal(fErr) }
	// Returns io.ReadCloser for ScreenText()
	readFile := ioutil.NopCloser(bufio.NewReader(file))

	// Some optional properties for the ScreenText()
	autocorrect := true
	pii := true
	classify := true
	moderatedText, mtErr := clientText.ScreenText(context, "text/plain", readFile, "eng", 
								&autocorrect, &pii, "", &classify)
	if mtErr != nil { log.Fatal(mtErr) }

	// Format and print results
	printText, _ := json.MarshalIndent(moderatedText, "", "\t") // Converts struct into JSON
	fmt.Println("Moderated text from file:");
	fmt.Println(string(printText))
	/*
	END - Text Moderation
	*/

	/*
	HUMAN REVIEWS - IMAGE
	Moderate an image from a URL with a human reviewer.
	Steps: 
		1. Run the quickstart
		2. Go to the Content Moderator website to review image
		3. Return to console and press ENTER (wait for results)
	*/
	fmt.Println()
	fmt.Println("------------------------------")
	fmt.Println("HUMAN REVIEWS - IMAGE")

	fmt.Println("Creating a review for the image " + path.Base(imageURL) + " ...")

	// Create the metadata for CreateReviewBodyItem.Metadata.
	key := "sc"
	value := "true"
	metadataArray := []contentmoderator.CreateReviewBodyItemMetadataItem { {Key: &key, Value: &value} }

	// Get the array of all possible types for the CreateReviewBodyItem.Type.
	bodyItemType := contentmoderator.PossibleTypeValues()
	// Generate random UUID for CreateReviewBodyItem.ContentID.
	u := uuid.Must(uuid.NewV4()).String()

	// Create the request body for the review. 
	reviewsBody := contentmoderator.CreateReviewBodyItem {
					Type: bodyItemType[0], // uses "Image" as Type
					Content: &imageURL,
					ContentID: &u,
					CallbackEndpoint: &callbackEndpoint,
					Metadata: &metadataArray,
	}

	// Add the reques body into an array
	rbArray := []contentmoderator.CreateReviewBodyItem { reviewsBody }

	// Create the review. Returns a ListString struct.
	// It gets sent as a POST to the callback endpoint.
	reviewList, riErr := clientReviews.CreateReviews(context, "Image", teamName, rbArray, "")
	if riErr != nil { log.Fatal(riErr) }
	
	// Get the review details before it gets human reviewed, to compare with the details after the review.
	// Get the review ID from the ListString struct
	id := *reviewList.Value
	// Get the review, which reurns a Review type struct
	reviewBefore, rbErr := clientReviews.GetReview(context, teamName, id[0])
	if rbErr != nil { log.Fatal(rbErr) }

	// Save review ID from response.
	reviewID := *reviewBefore.ReviewID
	fmt.Println("Review ID: " + reviewID)
	fmt.Println()

	// Print result of getting the review details, before the review.
	// Notice the ReviewerResultTags are currently empty.
	response, _ := json.MarshalIndent(reviewBefore, "", "\t") // Converts struct into JSON
	fmt.Println(string(response))

	// Go to the Content Moderator website to perform the review.
	// Go to Review --> Image and select 'a' (adult) or 'r' (racy) for testing purposes. 
	// Then select Next and go back to the console and press ENTER.
	fmt.Println("Perform manual reviews on the Content Moderator Review Site, then press ENTER here.")
	var input string
    fmt.Scanln(&input)

	// Wait for processing of review
	fmt.Println("Wait 30 seconds for the server to update the review")
	time.Sleep(30 * time.Second)

	// Get review details again, after the human review.
	reviewAfter, raErr := clientReviews.GetReview(context, teamName, id[0])
	if raErr != nil { log.Fatal(raErr) }

	// Print result, notice the ReviewerResultTags has the result of the review.
	responseB, _ := json.MarshalIndent(reviewAfter, "", "\t")
	fmt.Println(string(responseB))
	/*
	END - Human reviews for images
	*/
}


