'use strict';

/* 
 * This sample does the following tasks: Performs a custom search query.
 */

/* To run this sample, please install the required packages by running the following commands at an Administrator command prompt.
 * npm install azure-cognitiveservices-customsearch
 * npm install ms-rest-azure
 * 
 * For more information about how to use the Azure Custom Search Node.js SDK, see:
 * https://github.com/Azure/azure-sdk-for-node/blob/master/lib/services/cognitiveServicesCustomSearch/README.md
 */

const search = require('azure-cognitiveservices-customsearch');
const auth = require('ms-rest-azure');

if (!process.env.CUSTOM_SEARCH_SUBSCRIPTION_KEY) {
    throw 'Please set/export the following environment variable: CUSTOM_SEARCH_SUBSCRIPTION_KEY';
}
let subscriptionKey = process.env.CUSTOM_SEARCH_SUBSCRIPTION_KEY;

/* For more information, see:
 * https://docs.microsoft.com/en-us/azure/cognitive-services/bing-custom-search/quick-start#create-a-custom-search-instance
 */
if (!process.env.CUSTOM_SEARCH_INSTANCE_ID) {
    throw 'Please set/export the following environment variable: CUSTOM_SEARCH_INSTANCE_ID';
}
let instance_ID = process.env.CUSTOM_SEARCH_INSTANCE_ID;

let query = 'xbox';

let credentials = new auth.CognitiveServicesCredentials(subscriptionKey);
let client = new search.CustomSearchClient(credentials);

/* For more information about CustomSearchClient and CustomInstance, see:
 * https://docs.microsoft.com/en-us/javascript/api/azure-cognitiveservices-customsearch/customsearchclient?view=azure-node-latest
 * https://docs.microsoft.com/en-us/javascript/api/azure-cognitiveservices-customsearch/custominstance?view=azure-node-latest
 */
client.customInstance.search(instance_ID, query, function (err, result, request, response) {
    if (err) {
        console.log(err);
    }
    else {
        console.log(result.queryContext.originalQuery);
        console.log(result.webPages.value);
    }
});
