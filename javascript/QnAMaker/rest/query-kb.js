'use strict';

// <dependencies>
/* To install dependencies, run:
 * npm install requestretry
 */
const request = require("requestretry");
// </dependencies>

/*
* Set the `subscriptionKey` and `authoringEndpoint` variables to your
* QnA Maker authoring subscription key and endpoint.
*
* These values can be found in the Azure portal (ms.portal.azure.com/).
* Look up your QnA Maker resource. Then, in the "Resource management"
* section, find the "Keys and Endpoint" page.
*
* The value of `authoringEndpoint` has the format https://YOUR-RESOURCE-NAME.cognitiveservices.azure.com.
*
* Set the `runtimeEndpoint` variable to your QnA Maker runtime endpoint.
* The value of `runtimeEndpoint` has the format https://YOUR-RESOURCE-NAME.azurewebsites.net.
*
* Set the `kbId` variable to the ID of a knowledge base you have
* previously created.
*/
// <constants>
const subscriptionKey = "PASTE_YOUR_QNA_MAKER_AUTHORING_SUBSCRIPTION_KEY_HERE";
const authoringEndpoint = "PASTE_YOUR_QNA_MAKER_AUTHORING_ENDPOINT_HERE";
const runtimeEndpoint = "PASTE_YOUR_QNA_MAKER_RUNTIME_ENDPOINT_HERE";
const kbId = "PASTE_YOUR_QNA_MAKER_KB_ID_HERE";

const getEndpointKeyMethod = "/qnamaker/v4.0/endpointkeys";
// </constants>

// <query>
const getEndpointKey = async () => {
	var request_params = {
		uri: authoringEndpoint + getEndpointKeyMethod,
		method: 'GET',
		headers: {
			'Ocp-Apim-Subscription-Key': subscriptionKey
		}
	};
	var response = await request(request_params);
	var result = JSON.parse (response.body);
	return result.primaryEndpointKey;
};

const getAnswerMethod = "/qnamaker/knowledgebases/" + kbId + "/generateAnswer";

// JSON format for passing question to service
const question = {'question': 'Is the QnA Maker Service free?','top': 3};

const getAnswer = async (endpointKey) => {
	var request_params = {
		uri: runtimeEndpoint + getAnswerMethod,
		method: 'POST',
		headers: {
			'Content-Type' : 'application/json',
			'Content-Length' : JSON.stringify(question).length,
// Note this differs from the "Ocp-Apim-Subscription-Key"/<subscription key> used by most Cognitive Services.
			'Authorization': "EndpointKey " + endpointKey
		},
		json: true,
		body: question
	};
	var response = await request(request_params);
	return response.body;
};

const main = async()=>{
	var endpointKey = await getEndpointKey();
	return await getAnswer(endpointKey);
}

main()
.then(answer => {
    console.log("Answer:")
	console.log(answer);
}).catch(err => {
    console.log(err);
})
// </query>
