package main

import (
	"context"
	"fmt"
	"github.com/Azure/azure-sdk-for-go/services/cognitiveservices/v1.0/customsearch"
	"github.com/Azure/go-autorest/autorest"
	"log"
	"os"
)

func main() {
	if "" == os.Getenv("CUSTOM_SEARCH_SUBSCRIPTION_KEY") {
		log.Fatal("Please set/export the environment variable CUSTOM_SEARCH_SUBSCRIPTION_KEY.")
	}
	var subscription_key string = os.Getenv("CUSTOM_SEARCH_SUBSCRIPTION_KEY")
	if "" == os.Getenv("CUSTOM_SEARCH_INSTANCE_ID") {
		log.Fatal("Please set/export the environment variable CUSTOM_SEARCH_INSTANCE_ID.")
	}
	var search_instance_id string = os.Getenv("CUSTOM_SEARCH_INSTANCE_ID")

	// Get the context, which is required by the SDK methods.
	ctx := context.Background()

	client := customsearch.NewCustomInstanceClient()
	// Set the subscription key on the client.
	client.Authorizer = autorest.NewCognitiveServicesAuthorizer(subscription_key)

	result, err := client.Search (ctx, search_instance_id, "xbox", "", "", "", "", "", "", nil, "", nil, "", "", nil, "")
	if nil != err {
		log.Fatal(err)
	}

	var web_pages = *result.WebPages
	fmt.Printf("Estimated total results: %d.\n", *web_pages.TotalEstimatedMatches)
	var results = *web_pages.Value
	if len(results) > 0 {
		fmt.Println("First result:")
		fmt.Println("Title: " + *results[0].Name)
		fmt.Println("URL: " + *results[0].URL)
	}
}
