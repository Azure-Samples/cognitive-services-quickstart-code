package main

import (

	"context"
	"encoding/json"
	"fmt"
	"github.com/Azure/azure-sdk-for-go/services/cognitiveservices/v1.0/contentmoderator"
	"github.com/Azure/go-autorest/autorest"
	"github.com/satori/go.uuid"
	"log"
	"os"
	"path"
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
	- Set your keys, endpoints, team name, callback endpoint in your environment variables, in the Authentication section.
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
	contentmoderatorContext := context.Background()

	// Get your Content Moderator subscription key from Azure, set into your environment variables
	subscriptionKey := os.Getenv("CONTENT_MODERATOR_SUBSCRIPTION_KEY")
	// Add your Content Moderator endpoint here
	endpoint := os.Getenv("CONTENT_MODERATOR_ENDPOINT")

	// HUMAN REVIEWS - IMAGE
	// Add your team name (created when you started your Content Moderator website account) 
	// Find from Content Moderator website on Credentials page (it's the Id)
	teamName := os.Getenv("CONTENT_MODERATOR_TEAM_NAME")
	// Using a group of people photo to moderate
	imageURL := "https://moderatorsampleimages.blob.core.windows.net/samples/sample5.png"

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
	END - Authenticate
	*/

	/*
	HUMAN REVIEWS - IMAGE
	Moderate an image from a URL with a human reviewer
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
	reviewList, riErr := clientReviews.CreateReviews(contentmoderatorContext, "Image", teamName, rbArray, "")
	if riErr != nil { log.Fatal(riErr) }
	
	// Get the review details before it gets human reviewed, to compare with the details after the review.
	// Get the review ID from the ListString struct
	id := *reviewList.Value
	// Get the review, which reurns a Review type struct
	reviewBefore, rbErr := clientReviews.GetReview(contentmoderatorContext, teamName, id[0])
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
	reviewAfter, raErr := clientReviews.GetReview(contentmoderatorContext, teamName, id[0])
	if raErr != nil { log.Fatal(raErr) }

	// Print result, notice the ReviewerResultTags has the result of the review.
	responseB, _ := json.MarshalIndent(reviewAfter, "", "\t")
	fmt.Println(string(responseB))
	/*
	END - HUMAN REVIEWS - IMAGE
	*/

}


