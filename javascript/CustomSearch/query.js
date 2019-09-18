'use strict';

/* 
 * This quickstart performs a Bing custom search query, using the search term "xbox".
 *
 * Prequisites:
 * - Get your Bing Custom Search subscription key from the Azure portal,
 *   then add it to your environment variables as BING_CUSTOM_SEARCH_SUBSCRIPTION_KEY
 * - Install these npm packages from the command line:
 *     npm install azure-cognitiveservices-customsearch
 *     npm install ms-rest-azure
 * - Create a Bing Custom Search instance: https://docs.microsoft.com/en-us/azure/cognitive-services/bing-custom-search/quick-start#create-a-custom-search-instance
 *   In creating the instance, add some search URLs, such as: https://twitter.com/xbox,
 *   https://www.facebook.com/xbox, etc. (or your own preferred search URLs).
 * - Add BING_CUSTOM_SEARCH_INSTANCE_ID to your environment variables with the value for your 
 *   Custom Configuration ID from customsearch.ai (Production tab--> Endpoints--> Custom Configuration ID).
 * - Close, reopen project for environment variable changes to take effect.
 * 
 * How to run, from the command line:
 *   node bing_custom_search_quickstart.js
 * 
 * Azure Bing Custom Search Node.js SDK:
 * https://github.com/Azure/azure-sdk-for-node/blob/master/lib/services/cognitiveServicesCustomSearch/README.md
 */

const search = require('azure-cognitiveservices-customsearch');
const auth = require('ms-rest-azure');

let subscriptionKey = process.env['BING_CUSTOM_SEARCH_SUBSCRIPTION_KEY'];
if (!subscriptionKey) {
    throw 'Set/export an environment variable for BING_CUSTOM_SEARCH_SUBSCRIPTION_KEY';
}

let instance_ID = process.env['BING_CUSTOM_SEARCH_INSTANCE_ID'];
if (!instance_ID) {
    throw 'Set/export an environment variable for BING_CUSTOM_SEARCH_INSTANCE_ID';
}

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
