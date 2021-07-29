"use strict";

/* To run this sample, install the following modules.
 * npm install @azure/cognitiveservices-autosuggest
 * npm install @azure/ms-rest-js
 */
var TranslatorText = require("@azure/cognitiveservices-translatortext");
var msRest = require("@azure/ms-rest-js");

const subscription_key = 'PASTE_YOUR_TRANSLATOR_SUBSCRIPTION_KEY_HERE';
const endpoint = 'PASTE_YOUR_TRANSLATOR_ENDPOINT_HERE';

const creds = new msRest.ApiKeyCredentials({ inHeader: { 'Ocp-Apim-Subscription-Key': subscription_key } });
const client = new TranslatorText.TranslatorTextClient(creds, endpoint).translator;

async function quickstart() {
    var result = await client.detect([{ text: "Salve, mondo!" }]);

	if (0 == result.length) {
		console.log ('No results found.');
	}
	else {
		console.log ('Language detected: ' + result[0].language);
		console.log ('Score: ' + result[0].score);
	}
}

try {
    quickstart();
}
catch (error) {
    console.log(error);
}
