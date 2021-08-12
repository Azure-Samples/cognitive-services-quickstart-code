//
// This quickstart shows how to add utterances to a LUIS model using the REST APIs.
//

var request = require('request-promise');

//////////
// Values to modify.

// YOUR-APP-ID: The App ID GUID found on the www.luis.ai Application Settings page.
const LUIS_appId = "PASTE_YOUR_LUIS_APP_ID_HERE";

// YOUR-AUTHORING-KEY: Your LUIS authoring key, 32 character value.
const LUIS_authoringKey = "PASTE_YOUR_LUIS_AUTHORING_SUBSCRIPTION_KEY_HERE";

// YOUR-AUTHORING-ENDPOINT: Replace this with your authoring key endpoint.
// For example, "https://your-resource-name.cognitiveservices.azure.com/"
const LUIS_endpoint = "PASTE_YOUR_LUIS_AUTHORING_ENDPOINT_HERE";

// NOTE: Replace this your version number. The Pizza app uses a version number of "0.1".
const LUIS_versionId = "0.1";
//////////

const addUtterancesURI = `${LUIS_endpoint}luis/authoring/v3.0-preview/apps/${LUIS_appId}/versions/${LUIS_versionId}/examples`;
const addTrainURI = `${LUIS_endpoint}luis/authoring/v3.0-preview/apps/${LUIS_appId}/versions/${LUIS_versionId}/train`;

const utterances = [
    {
        'text': 'order a pizza',
        'intentName': 'ModifyOrder',
        'entityLabels': [
            {
                'entityName': 'Order',
                'startCharIndex': 6,
                'endCharIndex': 12
            }
        ]
    },
    {
        'text': 'order a large pepperoni pizza',
        'intentName': 'ModifyOrder',
        'entityLabels': [
            {
                'entityName': 'Order',
                'startCharIndex': 6,
                'endCharIndex': 28
            },
            {
                'entityName': 'FullPizzaWithModifiers',
                'startCharIndex': 6,
                'endCharIndex': 28
            },
            {
                'entityName': 'PizzaType',
                'startCharIndex': 14,
                'endCharIndex': 28
            },
            {
                'entityName': 'Size',
                'startCharIndex': 8,
                'endCharIndex': 12
            }
        ]
    },
    {
        'text': 'I want two large pepperoni pizzas on thin crust',
        'intentName': 'ModifyOrder',
        'entityLabels': [
            {
                'entityName': 'Order',
                'startCharIndex': 7,
                'endCharIndex': 46
            },
            {
                'entityName': 'FullPizzaWithModifiers',
                'startCharIndex': 7,
                'endCharIndex': 46
            },
            {
                'entityName': 'PizzaType',
                'startCharIndex': 17,
                'endCharIndex': 32
            },
            {
                'entityName': 'Size',
                'startCharIndex': 11,
                'endCharIndex': 15
            },
            {
                'entityName': 'Quantity',
                'startCharIndex': 7,
                'endCharIndex': 9
            },
            {
                'entityName': 'Crust',
                'startCharIndex': 37,
                'endCharIndex': 46
            }
        ]
    }
];

// Main function.
const main = async() =>{

    await addUtterances(utterances);
    await train("POST");
    await train("GET");

}

// Adds the utterances to the model.
const addUtterances = async (utterances) => {

    const options = {
        uri: addUtterancesURI,
        method: 'POST',
        headers: {
            'Ocp-Apim-Subscription-Key': LUIS_authoringKey
        },
        json: true,
        body: utterances
    };

    const response = await request(options)
    console.log("addUtterance:\n" + JSON.stringify(response, null, 2));
}

// With verb === "POST", sends a training request.
// With verb === "GET", obtains the training status.
const train = async (verb) => {

    const options = {
        uri: addTrainURI,
        method: verb,
        headers: {
            'Ocp-Apim-Subscription-Key': LUIS_authoringKey
        },
        json: true,
        body: null // The body can be empty for a training request
    };

    const response = await request(options)
    console.log("train " + verb + ":\n" + JSON.stringify(response, null, 2));
}

// MAIN
main().then(() => console.log("done")).catch((err)=> console.log(err));
