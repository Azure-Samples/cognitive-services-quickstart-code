"use strict";

/* This Bing Spell Check quickstart takes in some misspelled words and suggests corrections.
 *
 * Prerequisites:
 * - Add your Bing Spell Check subscription key and endpoint to your environment variables, using
 *   BING_SPELL_CHECK_SUBSCRIPTION_KEY and BING_SPELL_CHECK_ENDPOINT as variable names.
 * - Install the following modules:
 *     npm install @azure/cognitiveservices-spellcheck
 *     npm install @azure/ms-rest-js
 * 
 * Node SDK: https://docs.microsoft.com/en-us/javascript/api/@azure/cognitiveservices-spellcheck/?view=azure-node-latest
 */
var SpellCheck = require("@azure/cognitiveservices-spellcheck");
var msRest = require("@azure/ms-rest-js");

const key = process.env['BING_SPELL_CHECK_SUBSCRIPTION_KEY'];
if (!key) {
    throw new Error('Set/export a BING_SPELL_CHECK_SUBSCRIPTION_KEY environment variable.');
}

const endpoint = process.env['BING_SPELL_CHECK_ENDPOINT'];
if (!endpoint) {
    throw new Error('Set/export a BING_SPELL_CHECK_ENDPOINT environment variable.');
}

const creds = new msRest.ApiKeyCredentials({ inHeader: { 'Ocp-Apim-Subscription-Key': key } });
const client = new SpellCheck.SpellCheckClient(creds, { endpoint: endpoint });

async function quickstart() {
    let query = 'bill gtaes was eher';
    let misspelledWords = [];
    let suggestedWords = [];
    await client.spellChecker(query)
        .then((response) => {
            console.log();
            for (var i = 0; i < response._response.parsedBody.flaggedTokens.length; i++) {
                var spellingFlaggedToken = response._response.parsedBody.flaggedTokens[i];
                misspelledWords.push(spellingFlaggedToken.token);
                var correction = spellingFlaggedToken.suggestions[0].suggestion; // gets each word
                suggestedWords.push(correction);
            }
            console.log('Original query: ' + query);
            console.log();
            console.log('Misspelled words: ');
            console.log(misspelledWords);
            console.log();
            console.log('Suggested correction(s): ');
            console.log(suggestedWords);
        }).catch((err) => {
            throw err;
        })
}

try {
    quickstart();
}
catch (error) {
    console.log(error);
}
