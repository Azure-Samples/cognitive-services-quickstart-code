"use strict";

/* This sample for the Azure Cognitive Services LUIS API shows how to:
 * - Query by slot name
 */

/* To run this sample, install the following modules.
 * npm install @azure/ms-rest-js
 * npm install @azure/cognitiveservices-luis-runtime
 */

// For more information about LUIS, see
//
// LUIS portal - https://www.luis.ai/welcome
// LUIS conceptual docs - https://docs.microsoft.com/en-us/azure/cognitive-services/luis
// LUIS JS SDK docs for prediction - https://docs.microsoft.com/en-us/javascript/api/@azure/cognitiveservices-luis-runtime/predictionoperations?view=azure-node-latest
// LUIS Runtime NPM - https://www.npmjs.com/package/@azure/cognitiveservices-luis-runtime


// <Dependencies>
const msRest = require("@azure/ms-rest-js");
const LUIS = require("@azure/cognitiveservices-luis-runtime");
// </Dependencies>

// <Variables>
const key = "REPLACE-WITH-YOUR-ASSIGNED-PREDICTION-KEY"

const endpoint = "https://REPLACE-WITH-YOUR-RESOURCE-NAME.cognitiveservices.azure.com"
// </Variables>

// <AuthoringCreateClient>
const luisRuntimeClient = new LUIS.LUISRuntimeClient(
    new msRest.ApiKeyCredentials({
        inHeader: { "Ocp-Apim-Subscription-Key": key }
    }),
    endpoint
);
// </AuthoringCreateClient>

// <OtherVariables>
// Use public app ID or replace with your own trained and published app's ID
// to query your own app
// public appID = `df67dcdb-c37d-46af-88e1-8b97951ca1c2`
// with slot of `production`
const luisAppID = "REPLACE-WITH-YOUR-LUIS_APP_ID"

// `production` or `staging`
const luisSlotName = "production"
// </OtherVariables>

// <predict>
const predict = async (app_id, slot_name) => {

    const predictionRequest = {
        query: "turn on all lights",
        options: {
            datetimeReference: new Date(),
            preferExternalEntities: false
        },
        externalEntities: [],
        dynamicLists: []
    };
    const verbose = true;
    const showAllIntents = true;

    // Note be sure to specify, using the slot_name parameter, whether your application is in staging or production.
    const predictionResult = await luisRuntimeClient.prediction
        .getSlotPrediction(luisAppID, luisSlotName, predictionRequest, { verbose, showAllIntents });

    console.log(JSON.stringify(predictionResult));
}
// </predict>

// <Main>
const quickstart = async () => {
    await predict();
}

try {
    quickstart();
} catch (error) {
    console.log(error);
}
// </Main>
