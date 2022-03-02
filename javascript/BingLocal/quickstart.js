"use strict";

/* To run this sample, install the following modules.
 * npm install @azure/cognitiveservices-localsearch
 * npm install @azure/ms-rest-js
 */
var LocalSearch = require ("@azure/cognitiveservices-localsearch");
var msRest = require ("@azure/ms-rest-js");

const subscription_key = 'PASTE_YOUR_BING_SEARCH_SUBSCRIPTION_KEY_HERE';
const endpoint = 'PASTE_YOUR_BING_SEARCH_ENDPOINT';

const creds = new msRest.ApiKeyCredentials({ inHeader: { 'Ocp-Apim-Subscription-Key': subscription_key } });
const client = new LocalSearch.LocalSearchClient(creds, { baseUri: endpoint });

async function quickstart() {
    var result = await client.local.search("restaurant", { location: 'lat:47.608013;long:-122.335167;re:100m'});

	var places = result.places.value;
	if (places.length > 0) {
		console.log("Results:\n");
		places.forEach((item) => {
			console.log("Name: " + item.name);
			console.log("URL: " + item.url);
			console.log();
		});
	}
	else {
		console.log("No places found for this query.");
	}
}

try {
    quickstart();
}
catch (error) {
    console.log(error);
}
