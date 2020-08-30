'use strict';

/* To install dependencies, run:
 * npm install requestretry
 */
const request = require("requestretry");

/*
* Configure the local environment:
* Set the QNA_MAKER_SUBSCRIPTION_KEY, QNA_MAKER_ENDPOINT, and QNA_MAKER_KB_ID
* environment variables on your local machine using
* the appropriate method for your preferred shell (Bash, PowerShell, Command
* Prompt, etc.). 
*
* If the environment variable is created after the application is launched in a
* console or with Visual Studio, the shell (or Visual Studio) needs to be closed
* and reloaded to take the environment variable into account.
*/
//<authorization>
const subscriptionKey = process.env.QNA_MAKER_SUBSCRIPTION_KEY;
if (! process.env.QNA_MAKER_SUBSCRIPTION_KEY) {
	throw "Please set/export the environment variable QNA_MAKER_SUBSCRIPTION_KEY.";
}

const endpoint = process.env.QNA_MAKER_ENDPOINT;
if (! process.env.QNA_MAKER_ENDPOINT) {
	throw "Please set/export the environment variable QNA_MAKER_ENDPOINT.";
}

const kbId = process.env.QNA_MAKER_KB_ID;
if (! process.env.QNA_MAKER_KB_ID) {
	throw "Please set/export the environment variable QNA_MAKER_KB_ID.";
}

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