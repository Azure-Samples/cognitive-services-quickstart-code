/* To run this sample, install the following modules.
 * npm install @azure/ms-rest-js @azure/cognitiveservices-luis-authoring @azure/cognitiveservices-luis-runtime
 */
// <Dependencies>
const msRest = require("@azure/ms-rest-js");
const LUIS_Authoring = require("@azure/cognitiveservices-luis-authoring");
const LUIS_Prediction = require("@azure/cognitiveservices-luis-runtime");
// </Dependencies>

// <Main>
const quickstart = async () => {

    // <VariablesYouChange>
    const authoringKey = 'PASTE_YOUR_LUIS_AUTHORING_SUBSCRIPTION_KEY_HERE';

    const authoringEndpoint = "PASTE_YOUR_LUIS_AUTHORING_ENDPOINT_HERE";
    const predictionEndpoint = "PASTE_YOUR_LUIS_PREDICTION_ENDPOINT_HERE";
    // </VariablesYouChange>

    // <VariablesYouDontNeedToChangeChange>
    const appName = "Contoso Pizza Company";
    const versionId = "0.1";
    const intentName = "OrderPizzaIntent";
    // </VariablesYouDontNeedToChangeChange>

    // <AuthoringCreateClient>
    const luisAuthoringCredentials = new msRest.ApiKeyCredentials({
        inHeader: { "Ocp-Apim-Subscription-Key": authoringKey }
    });
    const client = new LUIS_Authoring.LUISAuthoringClient(
        luisAuthoringCredentials,
        authoringEndpoint
    );
    // </AuthoringCreateClient>

    // Create app
    const appId = await createApp(client, appName, versionId);

    // <AddIntent>
    await client.model.addIntent(
        appId,
        versionId,
        { name: intentName }
    );
    // </AddIntent>

    // Add Entities
    await addEntities(client, appId, versionId);
    
    // Add Labeled example utterance
    await addLabeledExample(client, appId, versionId, intentName);

    // <TrainAppVersion>
    await client.train.trainVersion(appId, versionId);
    while (true) {
        const status = await client.train.getStatus(appId, versionId);
        if (status.every(m => m.details.status == "Success")) {
            // Assumes that we never fail, and that eventually we'll always succeed.
            break;
        }
    }
    // </TrainAppVersion>

    // <PublishVersion>
    await client.apps.publish(appId, { versionId: versionId, isStaging: false });
    // </PublishVersion>

    // <PredictionCreateClient>
    const luisPredictionClient = new LUIS_Prediction.LUISRuntimeClient(
        luisAuthoringCredentials,
        predictionEndpoint
    );
    // </PredictionCreateClient>

    // <QueryPredictionEndpoint>
    // Production == slot name
    const request = { query: "I want two small pepperoni pizzas with more salsa" };
    const response = await luisPredictionClient.prediction.getSlotPrediction(appId, "Production", request);
    console.log(JSON.stringify(response.prediction, null, 4 ));
    // </QueryPredictionEndpoint>

}


const createApp = async (client, appName, versionId) => {

    // <AuthoringCreateApplication>
    const create_app_payload = {
        name: appName,
        initialVersionId: versionId,
        culture: "en-us"
    };

    const createAppResult = await client.apps.add(
        create_app_payload
    );

    const appId = createAppResult.body
    // </AuthoringCreateApplication>

    console.log(`Created LUIS app with ID ${appId}`);

    return appId;
}



const addEntities = async (client, appId, versionId) => {

    // <AuthoringAddEntities>
    // Add Prebuilt entity
    await client.model.addPrebuilt(appId, versionId, ["number"]);

    // Define ml entity with children and grandchildren
    const mlEntityDefinition = {
        name: "Pizza order",
        children: [
            {
                name: "Pizza",
                children: [
                    { name: "Quantity" },
                    { name: "Type" },
                    { name: "Size" }
                ]
            },
            {
                name: "Toppings",
                children: [
                    { name: "Type" },
                    { name: "Quantity" }
                ]
            }
        ]
    };

    // Add ML entity 
    const response = await client.model.addEntity(appId, versionId, mlEntityDefinition);
    const mlEntityId = response.body;

    // Add phraselist feature
    const phraselistResponse = await client.features.addPhraseList(appId, versionId, {
        enabledForAllModels: false,
        isExchangeable: true,
        name: "QuantityPhraselist",
        phrases: "few,more,extra"
    });
    const phraseListId = phraselistResponse.body;

    // Get entity and subentities
    const model = await client.model.getEntity(appId, versionId, mlEntityId);
    const toppingQuantityId = getModelGrandchild(model, "Toppings", "Quantity");
    const pizzaQuantityId = getModelGrandchild(model, "Pizza", "Quantity");

    // add model as feature to subentity model
    await client.features.addEntityFeature(appId, versionId, pizzaQuantityId, { modelName: "number", isRequired: true });
    await client.features.addEntityFeature(appId, versionId, toppingQuantityId, { modelName: "number" });
    // <AuthoringAddModelAsFeature>

    // add phrase list as feature to subentity model
    await client.features.addEntityFeature(appId, versionId, toppingQuantityId, { featureName: "QuantityPhraselist" });
    // </AuthoringAddEntities>
}


const addLabeledExample = async (client, appId, versionId, intentName) => {

    // <AuthoringAddLabeledExamples>
    // Define labeled example
    const labeledExampleUtteranceWithMLEntity =
    {
        text: "I want two small seafood pizzas with extra cheese.",
        intentName: intentName,
        entityLabels: [
            {
                startCharIndex: 7,
                endCharIndex: 48,
                entityName: "Pizza order",
                children: [
                    {
                        startCharIndex: 7,
                        endCharIndex: 30,
                        entityName: "Pizza",
                        children: [
                            {
                                startCharIndex: 7,
                                endCharIndex: 9,
                                entityName: "Quantity"
                            },
                            {
                                startCharIndex: 11,
                                endCharIndex: 15,
                                entityName: "Size"
                            },
                            {
                                startCharIndex: 17,
                                endCharIndex: 23,
                                entityName: "Type"
                            }]
                    },
                    {
                        startCharIndex: 37,
                        endCharIndex: 48,
                        entityName: "Toppings",
                        children: [
                            {
                                startCharIndex: 37,
                                endCharIndex: 41,
                                entityName: "Quantity"
                            },
                            {
                                startCharIndex: 43,
                                endCharIndex: 48,
                                entityName: "Type"
                            }]
                    }
                ]
            }
        ]
    };

    console.log("Labeled Example Utterance:", JSON.stringify(labeledExampleUtteranceWithMLEntity, null, 4 ));

    // Add an example for the entity.
    // Enable nested children to allow using multiple models with the same name.
    // The quantity subentity and the phraselist could have the same exact name if this is set to True
    await client.examples.add(appId, versionId, labeledExampleUtteranceWithMLEntity, { enableNestedChildren: true });
    // </AuthoringAddLabeledExamples>
}


// <AuthoringSortModelObject>
const getModelGrandchild = (model, childName, grandchildName) => {

    return model.children.find(c => c.name == childName).children.find(c => c.name == grandchildName).id

}
// </AuthoringSortModelObject>


quickstart()
    .then(result => console.log("Done"))
    .catch(err => {
        console.log(`Error: ${err}`)
    })
