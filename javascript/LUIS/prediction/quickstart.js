"use strict";

/* To run this sample, install the following modules.
 * npm install @azure/cognitiveservices-luis-authoring
 * npm install @azure/cognitiveservices-luis-runtime
 * npm install @azure/ms-rest-js
 */
var authoring = require("@azure/cognitiveservices-luis-authoring");
var runtime = require("@azure/cognitiveservices-luis-runtime");
var msRest = require("@azure/ms-rest-js");

/* Configure the local environment:
* Set the following environment variables on your local machine using the
* appropriate method for your preferred shell (Bash, PowerShell, Command
* Prompt, etc.).
*
* LUIS_AUTHORING_KEY
* LUIS_AUTHORING_ENDPOINT
* LUIS_RUNTIME_KEY
* LUIS_RUNTIME_ENDPOINT
*
* If the environment variable is created after the application is launched in a console or with Visual
* Studio, the shell (or Visual Studio) needs to be closed and reloaded to take the environment variable into account.
*/
const authoring_key_var = 'LUIS_AUTHORING_KEY';
if (!process.env[authoring_key_var]) {
    throw new Error('please set/export the following environment variable: ' + authoring_key_var);
}
const authoring_key = process.env[authoring_key_var];

const authoring_endpoint_var = 'LUIS_AUTHORING_ENDPOINT';
if (!process.env[authoring_endpoint_var]) {
    throw new Error('please set/export the following environment variable: ' + authoring_endpoint_var);
}
const authoring_endpoint = process.env[authoring_endpoint_var];

const runtime_key_var = 'LUIS_RUNTIME_KEY';
if (!process.env[runtime_key_var]) {
    throw new Error('please set/export the following environment variable: ' + runtime_key_var);
}
const runtime_key = process.env[runtime_key_var];

const runtime_endpoint_var = 'LUIS_RUNTIME_ENDPOINT';
if (!process.env[runtime_endpoint_var]) {
    throw new Error('please set/export the following environment variable: ' + runtime_endpoint_var);
}
const runtime_endpoint = process.env[runtime_endpoint_var];

const authoring_creds = new msRest.ApiKeyCredentials({ inHeader: { 'Ocp-Apim-Subscription-Key': authoring_key } });
const authoring_client = new authoring.LUISAuthoringClient(authoring_creds, authoring_endpoint);

const runtime_creds = new msRest.ApiKeyCredentials({ inHeader: { 'Ocp-Apim-Subscription-Key': runtime_key } });
const runtime_client = new runtime.LUISRuntimeClient(runtime_creds, runtime_endpoint);

function create_app() {
    var create_app_payload = {
        domainName: "HomeAutomation",
        culture: "en-us"
    };
    return authoring_client.apps.addCustomPrebuiltDomain(create_app_payload).then((result) => {
        console.log("Created LUIS app with ID " + result.body);
        return new AppInfo(result.body, "0.1");
    }).catch(error => {
        throw error;
    });
}

function wait_for_operation(app_info) {
    return authoring_client.train.getStatus(app_info.id, app_info.version).then(function (result) {
        // GetStatus returns a list of training statuses, one for each model.
        // Loop through them and make sure all are done.
        if (result.some(function (info) {
            var status = info.details.status;
            if ("Queued" === status || "InProgress" === status) {
                return true;
            }
            else if ("Fail" === status) {
                console.log("Training operation failed. Reason: " + info.details.failureReason);
                return false;
            }
            else {
                return false;
            }
        })) {
            console.log("Waiting 10 seconds for training to complete...");
            return new Promise(function (resolve, reject) {
                setTimeout(function () {
                    resolve(wait_for_operation(app_info));
                }, 10000);
            });
        }
    }).catch(error => {
        throw error;
    });
}

function train_app(app_info) {
    return authoring_client.train.trainVersion(app_info.id, app_info.version).then((result) => {
        // Wait for the train operation to finish.
        console.log("Waiting for train operation to finish...");
        return wait_for_operation(app_info);
    }).catch(error => {
        throw error;
    });
}

function publish_app(app_info) {
    return authoring_client.apps.publish(app_info.id, { versionId: app_info.version, isStaging: true }).then((result) => {
        console.log("Application published. Endpoint URL: " + result.endpointUrl);    
    }).catch(error => {
        throw error;
    });
}

function predict(app_info) {
    var ops = new runtime.PredictionOperations(runtime_client);
// Note be sure to specify, using the slotName parameter, whether your application is in staging or production.
// By default, applications are created in staging.
	var slotName = "staging";
    var request = { query: "turn on the bedroom light" };
    return ops.getSlotPrediction(app_info.id, slotName, request).then((result) => {
        console.log(result);
    }).catch(error => {
        throw error;
    })
}

function delete_app(app_info) {
    return authoring_client.apps.deleteMethod(app_info.id).then((result) => {
        console.log("Application with ID " + app_info.id + " deleted. Operation result: " + result.message);
    }).catch(error => {
        throw error;
    })
}

async function quickstart() {
    var app_info = await create_app();
    await train_app(app_info);
    await publish_app(app_info);
	await predict(app_info);
    await delete_app(app_info);
}

try {
    quickstart();
}
catch (error) {
    console.log(error);
}
