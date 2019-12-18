"use strict";

var Autosuggest = require("@azure/cognitiveservices-autosuggest");
var msRest = require("@azure/ms-rest-js");

/* To run this sample, install the following modules.
 *     npm install @azure/cognitiveservices-autosuggest
 *     npm install @azure/ms-rest-js
 */

 // Add your AUTOSUGGEST_SUBSCRIPTION_KEY to environment variables
let subscription_key = process.env['AUTOSUGGEST_SUBSCRIPTION_KEY'];
if (!subscription_key) {
    throw new Error('please set/export the following environment variable: ' + subscription_key);
}

// Create a client
const creds = new msRest.ApiKeyCredentials({ inHeader: { 'Ocp-Apim-Subscription-Key': subscription_key } });
const client = new Autosuggest.AutoSuggestClient(creds);

async function quickstart() {
    // Returns a Suggestions interface
    let result = await client.autoSuggest("xb", null);

    // Returns a SuggestionsSuggestionGroup[] 
    let groups = result.suggestionGroups;
    if (groups.length > 0) {
        // Returns a SearchAction[] 
        let suggestions = groups[0].searchSuggestions;
        if (suggestions.length > 0) {
            // View the entire suggestions list
            console.log(suggestions)
            // View certain properties
            for (var item in suggestions) {
                console.log(suggestions[item].query);
                console.log(suggestions[item].displayText);
            }
        }
        else {
            console.log("No suggestions found in this group.");
        }
    }
    else {
        console.log("No suggestions found.");
    }
}

try {
    quickstart();
}
catch (error) {
    console.log(error);
}
