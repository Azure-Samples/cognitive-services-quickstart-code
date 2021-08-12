package main

import (
	"context"
	"fmt"
	"github.com/Azure/azure-sdk-for-go/services/cognitiveservices/v1.0/autosuggest"
	"github.com/Azure/go-autorest/autorest"
	"log"
	"os"
)

func main() {
	var subscription_key string = "PASTE_YOUR_AUTO_SUGGEST_SUBSCRIPTION_KEY_HERE"
	var endpoint string = "PASTE_YOUR_AUTO_SUGGEST_ENDPOINT_HERE"

	// Get the context, which is required by the SDK methods.
	ctx := context.Background()

	client := autosuggest.New()
	// Set the subscription key on the client.
	client.Authorizer = autorest.NewCognitiveServicesAuthorizer(subscription_key)
	client.Endpoint = endpoint

	// This should return the query suggestion "xbox."
	result, err := client.AutoSuggest (ctx, "xb", "", "", "", "", "", "", "", "", "", "", []autosuggest.ResponseFormat{"Json"})
	if nil != err {
		log.Fatal(err)
	}

	groups := *result.SuggestionGroups
	if len(groups) > 0 {
		group, _ := groups[0].AsSuggestionsSuggestionGroup()
		fmt.Printf ("First suggestion group: %s\n", (*group).Name)
		suggestions := *(*group).SearchSuggestions
		if len(suggestions) > 0 {
			fmt.Println("First suggestion:")
			fmt.Printf("Query: %s\n", *suggestions[0].Query)
			fmt.Printf("Display text: %s\n", *suggestions[0].DisplayText)
		} else {
			fmt.Println("No suggestions found in this group.")
		}
	} else {
		fmt.Println("No suggestions found.")
	}
}
