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
    var result = await client.dictionaryLookup('en', 'fr', [{ text: "great" }]);

	if (0 == result.length) {
		console.log ('No translations found.');
	}
	else {
		console.log ('Translations:');
		for (var i = 0; i < result[0].translations.length; i++) {
			console.log (result[0].translations[i].displayTarget);
		}
	}
}

try {
    quickstart();
}
catch (error) {
    console.log(error);
}
