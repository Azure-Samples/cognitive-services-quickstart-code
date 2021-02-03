'use strict';

// <dependencies>
/* To install dependencies, run:
 * npm install requestretry
 */
const request = require("requestretry");
// </dependencies>

/*
* Configure the local environment:
* Set the QNA_MAKER_SUBSCRIPTION_KEY, QNA_MAKER_ENDPOINT,
* QNA_MAKER_RUNTIME_ENDPOINT, and QNA_MAKER_KB_ID
* environment variables on your local machine using
* the appropriate method for your preferred shell (Bash, PowerShell, Command
* Prompt, etc.). 
*
* If the environment variable is created after the application is launched in a
* console or with Visual Studio, the shell (or Visual Studio) needs to be closed
* and reloaded to take the environment variable into account.
*/
// <constants>
const subscriptionKey = process.env.QNA_MAKER_SUBSCRIPTION_KEY;
if (! process.env.QNA_MAKER_SUBSCRIPTION_KEY) {
	throw "Please set/export the environment variable QNA_MAKER_SUBSCRIPTION_KEY.";
}

const authoringEndpoint = process.env.QNA_MAKER_ENDPOINT;
if (! process.env.QNA_MAKER_ENDPOINT) {
	throw "Please set/export the environment variable QNA_MAKER_ENDPOINT.";
}

const runtimeEndpoint = process.env.QNA_MAKER_RUNTIME_ENDPOINT;
if (! process.env.QNA_MAKER_RUNTIME_ENDPOINT) {
	throw "Please set/export the environment variable QNA_MAKER_RUNTIME_ENDPOINT.";
}

const kbId = process.env.QNA_MAKER_KB_ID;
if (! process.env.QNA_MAKER_KB_ID) {
	throw "Please set/export the environment variable QNA_MAKER_KB_ID.";
}

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