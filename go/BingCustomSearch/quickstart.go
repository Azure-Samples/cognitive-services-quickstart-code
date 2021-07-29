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
	var subscription_key string = "PASTE_YOUR_CUSTOM_SEARCH_SUBSCRIPTION_KEY_HERE"
	var search_instance_id string = "PASTE_YOUR_CUSTOM_SEARCH_INSTANCE_ID_HERE"

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
		fmt.Println("First 10 results follow.\n")
		for i := 0; i < 10 && i < len(results); i++ {
		fmt.Println("Title: " + *results[i].Name)
		fmt.Println("URL: " + *results[i].URL)
		fmt.Println()
		}
	}
}
