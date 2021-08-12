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
    var result = await client.breakSentence([{ text: "How are you? I am fine. What did you do today?" }]);

	if (0 == result.length) {
		console.log ('No sentences found.');
	}
	else {
		var sentLen = result[0].sentLen;
		console.log ('Sentences found: ' + sentLen.length);
		for (var i = 0; i < sentLen.length; i++) {
			console.log ('Length of sentence ' + (i + 1) + ': ' + sentLen[i]);
		}
	}
}

try {
    quickstart();
}
catch (error) {
    console.log(error);
}
