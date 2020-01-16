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
// <Dependencies>
const msRest = require("@azure/ms-rest-js");
const LUIS = require("@azure/cognitiveservices-luis-authoring");
require("dotenv").config();
// </Dependencies>

/*  Configure the local environment:
 * Set the LUIS_AUTHORING_KEY and LUIS_AUTHORING_ENDPOINT environment variables
 * on your local machine using the appropriate method for your preferred shell
 * (Bash, PowerShell, Command Prompt, etc.).
 *
 * If the environment variable is created after the application is launched in a console or with Visual
 * Studio, the shell (or Visual Studio) needs to be closed and reloaded for changes to take effect.
 */
// <Variables>
const key = process.env["LUIS_AUTHORING_KEY"];
if (!key) {
  throw new Error(
    "Set/export your LUIS subscription key as an environment variable."
  );
}

const endpoint = process.env["LUIS_AUTHORING_ENDPOINT"];
if (!endpoint) {
  throw new Error("Set/export your LUIS endpoint as an environment variable.");
}
// </Variables>

// <AuthoringCreateClient>
const luisAuthoringCredentials = new msRest.ApiKeyCredentials({
  inHeader: { "Ocp-Apim-Subscription-Key": key }
});
const luisAuthoringClient = new LUIS.LUISAuthoringClient(
  luisAuthoringCredentials,
  endpoint
);
// </AuthoringCreateClient>

// <utilForQuickstartOnlyToSimulateAsyncOperation >
const delayTimer = async timeInMs => {
  return await new Promise(resolve => {
    setTimeout(resolve, timeInMs);
  });
};
// </utilForQuickstartOnlyToSimulateAsyncOperation >

// <AuthoringCreateApplication>
const create_app = async () => {
  const create_app_payload = {
    name: "Contoso",
    description: "Flight booking app built with Azure SDK for Java.",
    initialVersionId: "0.1",
    culture: "en-us"
  };

  const createAppResult = await luisAuthoringClient.apps.add(
    create_app_payload
  );

  console.log(`Created LUIS app with ID ${createAppResult.body}`);

  return {
    id: createAppResult.body,
    version: create_app_payload.initialVersionId
  };
};
// </AuthoringCreateApplication>

// <AuthoringAddEntities>
const add_entities = async app_info => {
  const addEntityDestinationResult = await luisAuthoringClient.model.addEntity(
    app_info.id,
    app_info.version,
    { name: "Destination" }
  );
  console.log("Entity Destination created.");

  const addEntityClassResult = await luisAuthoringClient.model.addEntity(
    app_info.id,
    app_info.version,
    { name: "Class" }
  );
  console.log("Entity Class created.");

  const addEntityFlightResult = await luisAuthoringClient.model.addEntity(
    app_info.id,
    app_info.version,
    { name: "Flight" }
  );
  console.log("Entity Flight created.");
};
// </AuthoringAddEntities>

// <AuthoringAddIntents>
const add_intents = async app_info => {
  const addIntentFindFlightsResult = await luisAuthoringClient.model.addIntent(
    app_info.id,
    app_info.version,
    { name: "FindFlights" }
  );
  console.log("Intent FindFlights added.");
};
// </AuthoringAddIntents>

// <AuthoringBatchAddUtterancesForIntent>
const create_utterance = (intent, text, labels) => {
  var entityLabels = new Array();

  labels.forEach((value, key, map) => {
    const start_index = text.toLowerCase().indexOf(value.toLowerCase());
    const end_index = start_index + value.length;

    if (start_index > -1) {
      entityLabels.push({
        entityName: key,
        startCharIndex: start_index,
        endCharIndex: end_index
      });
    }
  });

  console.log(`Created ${entityLabels.length} entity labels.`);

  return { text: text, entityLabels: entityLabels, intentName: intent };
};

const add_utterances = async app_info => {
  const utterance_1 = create_utterance(
    "FindFlights",
    "find flights in economy to Madrid",
    new Map([
      ["Flight", "economy to Madrid"],
      ["Destination", "Madrid"],
      ["Class", "economy"]
    ])
  );
  const utterance_2 = create_utterance(
    "FindFlights",
    "find flights to London in first class",
    new Map([
      ["Flight", "London in first class"],
      ["Destination", "London"],
      ["Class", "first"]
    ])
  );
  const utterance_3 = create_utterance(
    "FindFlights",
    "find flights from seattle to London in first class",
    new Map([
      ["Flight", "flights from seattle to London in first class"],
      ["Location", "London"],
      ["Location", "Seattle"],
      ["Class", "first"]
    ])
  );

  const luisExamplesBatchResult = await luisAuthoringClient.examples.batch(
    app_info.id,
    app_info.version,
    [utterance_1, utterance_2, utterance_3]
  );
  console.log("Example utterances added.");
};
// </AuthoringBatchAddUtterancesForIntent>

const wait_for_operation = async app_info => {
  let operationResult = null;
  let modelUniqueStatus = ["InProgress"];

  while (
    modelUniqueStatus.includes("InProgress") ||
    modelUniqueStatus.includes("Queued")
  ) {
    await delayTimer(1000);

    operationResult = await luisAuthoringClient.train.getStatus(
      app_info.id,
      app_info.version
    );

    modelUniqueStatus = [
      ...new Set(operationResult.map(op => op.details.status))
    ];

    console.log(`Current model status: ${JSON.stringify(modelUniqueStatus)}`);
  }

  return operationResult;
};

// <AuthoringTrainVersion>
const train_app = async app_info => {
  const trainResult = await luisAuthoringClient.train.trainVersion(
    app_info.id,
    app_info.version
  );

  console.log("Waiting for train operation to finish...");

  const operationResult = await wait_for_operation(app_info);
};
// </AuthoringTrainVersion>

// <AuthoringPublishVersion>
const publish_app = async app_info => {
  const publishResult = await luisAuthoringClient.apps.publish(app_info.id, {
    versionId: app_info.version,
    isStaging: true
  });

  console.log(
    `Application published. Endpoint URL: ${publishResult.endpointUrl}`
  );
};
// </AuthoringPublishVersion>

// <AuthoringDeleteApp>
const delete_app = async app_info => {
  const deleteResult = await luisAuthoringClient.apps.deleteMethod(app_info.id);

  console.log(
    `Application with ID ${app_info.id} deleted. Operation result: ${deleteResult.message}`
  );
};
// </AuthoringDeleteApp>

// <AuthoringListApps>
const list_apps = async () => {
  const apps = await luisAuthoringClient.apps.list;

  for (let app of apps) {
    console.log(`ID ${app.id}, NAME ${app.name}`);
  }
};
// </AuthoringListApps>

// <Main>
const quickstart = async () => {
  //    list_apps();
  var app_info = await create_app();
  await add_entities(app_info);
  await add_intents(app_info);
  await add_utterances(app_info);
  await train_app(app_info);
  await publish_app(app_info);
  await delete_app(app_info);
};

try {
  quickstart();
} catch (error) {
  console.log(error);
}
// </Main>
