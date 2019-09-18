"use strict";

/* This sample for the Azure Cognitive Services LUIS API shows how to:
 * - Create an application.
 * - Add intents to an application.
 * - Add entities to an application.
 * - Add utterances to an application.
 * - Train an application.
 * - Publish an application.
 * - Delete an application.
 * - List all applications.
 */

/* To run this sample, install the following modules.
 * npm install @azure/ms-rest-js
 * npm install @azure/cognitiveservices-luis-authoring
 */
const msRest = require ("@azure/ms-rest-js");
const LUIS = require ("@azure/cognitiveservices-luis-authoring");

/*  Configure the local environment:
 * Set the LUIS_SUBSCRIPTION_KEY and LUIS_ENDPOINT environment variables 
 * on your local machine using the appropriate method for your preferred shell 
 * (Bash, PowerShell, Command Prompt, etc.). 
 *
 * If the environment variable is created after the application is launched in a console or with Visual
 * Studio, the shell (or Visual Studio) needs to be closed and reloaded for changes to take effect.
 */
const key = process.env['LUIS_SUBSCRIPTION_KEY'];
if (!key) {
    throw new Error('Set/export your LUIS subscription key as an environment variable.');
}

const endpoint = process.env['LUIS_ENDPOINT'];
if (!endpoint) {
    throw new Error('Set/export your LUIS endpoint as an environment variable.');
}

const creds = new msRest.ApiKeyCredentials({ inHeader: { 'Ocp-Apim-Subscription-Key': key } });
const client = new LUIS.LUISAuthoringClient(creds, endpoint);

function create_app() {
    var d = new Date(Date.now());
    var name = "Contoso " + (d.getMonth() + 1) + "-" + d.getDate() + "-" + d.getFullYear() + " " +
        d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds();
    var description = "Flight booking app built with Azure SDK for Java.";
    var version = "0.1";
    var locale = "en-us";
    var create_app_payload = {
        name: name,
        description: description,
        initialVersionId: version,
        culture: locale
    };
    return client.apps.add(create_app_payload).then((result) => {
        console.log("Created LUIS app with ID " + result.body);
        return { id: result.body, version: version };
    }).catch(error => {
        throw error;
    });
}

function add_entities(app_info) {
    var entity_name = "Destination";
    return client.model.addEntity(app_info.id, app_info.version, { name: entity_name }).then((result) => {
        var h_entity_name = "Class";
        var h_entity_children = ["First", "Business", "Economy"];
        return client.model.addHierarchicalEntity(app_info.id, app_info.version, { name: h_entity_name, children: h_entity_children }).then((result) => {
            var c_entity_name = "Flight"
            var c_entity_children = ["Class", "Destination"];
            return client.model.addCompositeEntity(app_info.id, app_info.version, { name: c_entity_name, children: c_entity_children }).then((result) => {
                console.log("Entities Destination, Class, Flight created.");
            });
        });
    }).catch(error => {
        throw error;
    });
}

function add_intents(app_info) {
    var name = "FindFlights";
    return client.model.addIntent(app_info.id, app_info.version, { name: name }).then((result) => {
        console.log("Intent FindFlights added.");
    }).catch(error => {
        throw error;
    });
}

function create_utterance(intent, text, labels) {
    text = text.toLowerCase();
    var entityLabels = new Array();

    labels.forEach(function (value, key, map) {
        value = value.toLowerCase();
        var start_index = text.indexOf(value);
        var end_index = start_index + value.length;
        if (start_index > -1) {
            entityLabels.push({ entityName: key, startCharIndex: start_index, endCharIndex: end_index });
        }
    });

    console.log("Created " + entityLabels.length + " entity labels.");

    return { text: text, entityLabels: entityLabels, intentName: intent };
}

function add_utterances(app_info) {
    var utterance_1 = create_utterance("FindFlights", "find flights in economy to Madrid", new Map([["Flight", "economy to Madrid"], ["Destination", "Madrid"], ["Class", "economy"]]));
    var utterance_2 = create_utterance("FindFlights", "find flights to London in first class", new Map([["Flight", "London in first class"], ["Destination", "London"], ["Class", "first"]]));

    return client.examples.batch(app_info.id, app_info.version, [utterance_1, utterance_2]).then((result) => {
        console.log("Example utterances added.");
    }).catch(error => {
        throw error;
    });
}

function wait_for_operation(app_info) {
    return client.train.getStatus(app_info.id, app_info.version).then(function (result) {
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
    return client.train.trainVersion(app_info.id, app_info.version).then((result) => {
        // Wait for the train operation to finish.
        console.log("Waiting for train operation to finish...");
        return wait_for_operation(app_info);
    }).catch(error => {
        throw error;
    });
}

function publish_app(app_info) {
    return client.apps.publish(app_info.id, { versionId: app_info.version, isStaging: true }).then((result) => {
        console.log("Application published. Endpoint URL: " + result.endpointUrl);    
    }).catch(error => {
        throw error;
    });
}

function delete_app(app_info) {
    return client.apps.deleteMethod(app_info.id).then((result) => {
        console.log("Application with ID " + app_info.id + " deleted. Operation result: " + result.message);
    }).catch(error => {
        throw error;
    })
}

function list_apps() {
    client.apps.list().then((result) => {
        for (let x of result) {
            console.log(x.id);
        }
    }).catch(error => {
        throw error;
    });
}

async function quickstart() {
//    list_apps();
    var app_info = await create_app();
    await add_entities(app_info);
    await add_intents(app_info);
    await add_utterances(app_info);
    await train_app(app_info);
    await publish_app(app_info);
    await delete_app(app_info);
}

try {
    quickstart();
}
catch (error) {
    console.log(error);
}
