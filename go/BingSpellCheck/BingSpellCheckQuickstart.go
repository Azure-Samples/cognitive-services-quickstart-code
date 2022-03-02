package main

import (
    "context"
    "fmt"
    "github.com/Azure/azure-sdk-for-go/services/cognitiveservices/v1.0/spellcheck"
    "github.com/Azure/go-autorest/autorest"
    "log"
    "os"
)

/*
Bing Spell Check Quickstart
Checks the spelling of each word in a query, then suggests correction(s).

Prerequisites:
  - Install the Bing Spell Check SDK and Autorest libraries:
	  go get github.com/Azure/azure-sdk-for-go/services/cognitiveservices/v1.0/spellcheck
	  go get github.com/Azure/go-autorest/autorest
  
Run:
  - go run BingSpellCheckQuickstart.go

Resources:
  - Bing Spell Check SDK: https://godoc.org/github.com/Azure/azure-sdk-for-go/services/cognitiveservices/v1.0/spellcheck
  - Bing Spell Check documentation: https://docs.microsoft.com/en-us/azure/cognitive-services/bing-spell-check/index
  - Bing Spell Check API: https://docs.microsoft.com/en-us/rest/api/cognitiveservices-bingsearch/bing-spell-check-api-v7-reference
*/

func main() {
    // Authenticate
    subscriptionKey := "PASTE_YOUR_SPELL_CHECK_SUBSCRIPTION_KEY_HERE"
    spellCheckClient := spellcheck.New() //BaseClient class
    csAuthorizer := autorest.NewCognitiveServicesAuthorizer(subscriptionKey)
    spellCheckClient.Authorizer = csAuthorizer

    // Check the spelling of a query
    query := "bill Gatas was ehre"
    // Returns a SpellCheck struct
    spellCheckResult, err := spellCheckClient.SpellCheckerMethod(
	context.Background(), // context
	query,                // text to check
	"",                   // Accept-Language header
	"",                   // Pragma header
	"",                   // User-Agent header
	"",                   // X-MSEdge-ClientID header
	"",                   // X-MSEdge-ClientIP header
	"",                   // X-Search-Location header
	spellcheck.ActionType(""), // action type
	"",      // app name
	"",      // country code
	"",      // client machine name
	"",      // doc ID
	"",      // market
	"",      // session ID
	"",      // set lang
	"proof", // user ID
	"",      // mode
	"",      // pre context text
	"",      // post context text
    )
    if err != nil { log.Fatal(err) }

    fmt.Println()
    fmt.Println("Original query: \n" + query)
    fmt.Println()

    // The misspelled words are referred to as tokens 
    fmt.Println("Misspelled words: ")
    // 'eachWord' is a SpellingFlagToken struct
    for _, eachWord := range *spellCheckResult.FlaggedTokens {
    	fmt.Println(*eachWord.Token) // the misspelled words
    }

    fmt.Println()
    fmt.Println("Suggested corrections: ")
    for _, eachWord := range *spellCheckResult.FlaggedTokens {
	    for _, suggestedWord := range *eachWord.Suggestions { // Returns a *[]SpellingTokenSuggestion
	    	fmt.Println(*suggestedWord.Suggestion)
	    } 
    }
}
