'use strict';

/* To install dependencies, run:
 * npm install requestretry
 */
const request = require("requestretry");

/*
* Set the `subscriptionKey` and `endpoint` variables to your
* QnA Maker authoring subscription key and endpoint.
*
* These values can be found in the Azure portal (ms.portal.azure.com/).
* Look up your QnA Maker resource. Then, in the "Resource management"
* section, find the "Keys and Endpoint" page.
*
* The value of `endpoint` has the format https://YOUR-RESOURCE-NAME.cognitiveservices.azure.com.
*
* Set the `kbId` variable to the ID of a knowledge base you have
* previously created.
*/
const subscriptionKey = "PASTE_YOUR_QNA_MAKER_AUTHORING_SUBSCRIPTION_KEY_HERE";
const endpoint = "PASTE_YOUR_QNA_MAKER_AUTHORING_ENDPOINT_HERE";
const kbId = "PASTE_YOUR_QNA_MAKER_KB_ID_HERE";

const publishKbMethod = "/qnamaker/v4.0/knowledgebases/" + kbId

const publishKb = async () => {
	var request_params = {
		uri: endpoint + publishKbMethod,
		method: 'POST',
		headers: {
			'Ocp-Apim-Subscription-Key': subscriptionKey
		}
	};
	var response = await request(request_params);
	return response;
};

publishKb()
.then(response => {
// Note status code 204 is success.
    console.log("Result: " + response.statusCode);
}).catch(err => {
    console.log(err);
})
