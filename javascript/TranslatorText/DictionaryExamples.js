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
    var result = await client.dictionaryExamples('en', 'fr', [{ text: "great", translation: "formidable" }]);

	if (0 == result.length) {
		console.log ('No examples found.');
	}
	else {
		for (var i = 0; i < result[0].examples.length; i++) {
			console.log ('Example ' + (i + 1) + ':');
			console.log ('Source prefix: ' + result[0].examples[i].sourcePrefix);
			console.log ('Source term: ' + result[0].examples[i].sourceTerm);
			console.log ('Source suffix: ' + result[0].examples[i].sourceSuffix);
			console.log ('Target prefix: ' + result[0].examples[i].targetPrefix);
			console.log ('Target term: ' + result[0].examples[i].targetTerm);
			console.log ('Target suffix: ' + result[0].examples[i].targetSuffix);
			console.log ();
		}
	}
}

try {
    quickstart();
}
catch (error) {
    console.log(error);
}
