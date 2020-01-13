"use strict";

/* This sample does the following tasks.
 * - Create a knowledge base
 * - Update a knowledge base
 * - Publish a knowledge base
 * - Get published endpoint key
 * - Delete a knowledge base
 */

exports.__esModule = true;

/* To run this sample, install the following modules.
 * npm install @azure/ms-rest-js
 * npm install @azure/cognitiveservices-qnamaker
 */
// <dependencies>
const msRest = require("@azure/ms-rest-js");
const qnamaker = require("@azure/cognitiveservices-qnamaker");
require('dotenv').config();
// </dependencies>


// <resourceSettings>
// Get environment values key and endpoint for your QnA Maker resource - found in the Azure portal for that resource
if (!process.env.QNAMAKER_AUTHORING_KEY) {
    throw new Error('please set/export the following environment variable: QNAMAKER_AUTHORING_KEY');
}
const authoringKey = process.env.QNAMAKER_AUTHORING_KEY;

if (!process.env.QNAMAKER_ENDPOINT) {
    throw new Error('please set/export the following environment variable: QNAMAKER_ENDPOINT');
}
const endpoint = process.env.QNAMAKER_ENDPOINT;
// </resourceSettings>

// Create client.
/*
 * Use key and endpoint to create client.
 * https://docs.microsoft.com/javascript/api/@azure/cognitiveservices-qnamaker/qnamakerclient?view=azure-node-latest
 */

 // <authorization>
const creds = new msRest.ApiKeyCredentials({ inHeader: { 'Ocp-Apim-Subscription-Key': authoringKey } });
const qnaMakerClient = new qnamaker.QnAMakerClient(creds, endpoint);
const knowledgeBaseClient = new qnamaker.Knowledgebase(qnaMakerClient);
// </authorization>

// <utilForQuickstartOnlyToSimulateAsyncOperation >
const delayTimer = async (timeInMs)=>{
    return await new Promise((resolve) => {
        setTimeout(resolve, timeInMs);
    });
}
// </utilForQuickstartOnlyToSimulateAsyncOperation >


/*
 * list all knowledge bases
 * https://docs.microsoft.com/javascript/api/@azure/cognitiveservices-qnamaker/knowledgebase?view=azure-node-latest#listall-servicecallback-knowledgebasesdto--
 */
// <listKnowledgeBases>
const listKnowledgeBases = async() => {

    const result = await knowledgeBaseClient.listAll()

    for (const item of result.knowledgebases){
        console.log(`${item.name} stored in ${item.language} language with ${item.sources.length} sources, last updated ${item.lastChangedTimestamp}`);
    }
    return result.knowledgebases;
}
// </listKnowledgeBases>


/*
 * delete knowledge base
 * https://github.com/Azure/azure-sdk-for-js/blob/master/sdk/cognitiveservices/cognitiveservices-qnamaker/src/operations/knowledgebase.ts#L81
 */
// <deleteKnowledgeBase>
const deleteKnowledgeBase = async (kb_id) => {

    const results = await knowledgeBaseClient.deleteMethod(kb_id)

    if(results._response.status.toString().indexOf("2",0)==-1) {
        console.log(`Delete operation state failed - HTTP status ${results._response.status}`)
        return false
    }

    console.log(`Delete operation state succeeded - HTTP status ${results._response.status}`)
    return true
}
// </deleteKnowledgeBase>

/*
 * Monitor long-running operation
 * https://docs.microsoft.com/javascript/api/@azure/cognitiveservices-qnamaker/operations?view=azure-node-latest#getdetails-string--servicecallback-operation--
 */
// <monitorOperation>
const wait_for_operation = async (operation_id) => {

    let state = "NotStarted"
    let operationResult = undefined

    while("Running" === state || "NotStarted" === state){

        operationResult = await qnaMakerClient.operations.getDetails(operation_id)
        state = operationResult.operationState;

        console.log(`Operation state - ${state}`)

        await delayTimer(1000);
    }

    return operationResult;
}
// </monitorOperation>


// Main functions.


/*
* Create knowledge base from JSON object
* https://docs.microsoft.com/javascript/api/@azure/cognitiveservices-qnamaker/knowledgebase?view=azure-node-latest#create-createkbdto--requestoptionsbase--servicecallback-operation--
*/
// <createKnowledgeBase>
const createKnowledgeBase = async () => {

    const answer = "You can use our REST APIs to manage your Knowledge Base. See here for details: https://westus.dev.cognitive.microsoft.com/docs/services/58994a073d9e04097c7ba6fe/operations/58994a073d9e041ad42d9baa";
    const source = "Custom Editorial";
    const questions = ["How do I programmatically update my Knowledge Base?"];
    const metadata = [{ Name: "category", Value: "api" }];
    const qna_list = [{ id: 0, answer: answer, Source: source, questions: questions, Metadata: metadata }];
    const create_kb_payload = {
        name: 'QnA Maker FAQ',
        qnaList: qna_list,
        urls: [],
        files: []
    };

    const results = await knowledgeBaseClient.create(create_kb_payload)

    if(results._response.status.toString().indexOf("2",0)==-1) {
        console.log(`Create request failed - HTTP status ${results._response.status}`)
        return
    }

    const operationResult = await wait_for_operation( results.operationId)

    if(!operationResult || !operationResult.operationState ||  !(operationResult.operationState= "Succeeded") || !operationResult.resourceLocation) {
        console.log(`Create operation state failed - HTTP status ${operationResult._response.status}`)
        return
    }

    // parse resourceLocation for KB ID
   const kbID = operationResult.resourceLocation.replace("/knowledgebases/","");

   console.log(`Create operation ${operationResult._response.status}, KB ID ${kbID}`)
   return kbID;
}
// </createKnowledgeBase>

/*
 * Update knowledge base - can be used to completely replace knowledge base question and answer sets
 * https://docs.microsoft.com/javascript/api/@azure/cognitiveservices-qnamaker/knowledgebase?view=azure-node-latest#update-string--updatekboperationdto--servicecallback-operation--
 */
// <updateKnowledgeBase>
const updateKnowledgeBase = async(kb_id) => {

    // Add new Q&A lists, URLs, and files to the KB.
    const answer = "You can change the default message if you use the QnAMakerDialog. See this for details: https://docs.botframework.com/en-us/azure-bot-service/templates/qnamaker/#navtitle";
    const source = "Custom Editorial";
    const questions = ["How can I change the default message from QnA Maker?"];
    const metadata = [{ Name: "category", Value: "api" }];
    const qna_list = [{ id: 1, answer: answer, Source: source, questions: questions, Metadata: metadata }];
    const update_kb_add_payload = { qnaList: qna_list, urls: [], files: [] };


    // Update the KB name.
    const name = "New KB name";
    const update_kb_update_payload = { name: name };

    // Delete the QnaList with ID 0.
    const ids = [0];
    const update_kb_delete_payload = { ids: ids };

    // Bundle the add, update, and delete requests.
    const update_kb_payload = {
        add: update_kb_add_payload,
        update: update_kb_update_payload,
        deleteProperty:
        update_kb_delete_payload
    };

    const results = await knowledgeBaseClient.update(kb_id, update_kb_payload)

    if(!results._response.status.toString().indexOf("2",0)==-1) {
        console.log(`Update request failed - HTTP status ${results._response.status}`)
        return false
    }

    const operationResult = await wait_for_operation( results.operationId)

    if(operationResult.operationState != "Succeeded") {
        console.log(`Update operation state failed - HTTP status ${operationResult._response.status}`)
        return false
    }

    console.log(`Update operation state ${operationResult._response.status} - HTTP status ${operationResult._response.status}`)
    return true
}
// </updateKnowledgeBase>

/*
 * Publish knowledge base to use query prediction runtime
 * https://docs.microsoft.com/javascript/api/@azure/cognitiveservices-qnamaker/knowledgebase?view=azure-node-latest#publish-string--requestoptionsbase--servicecallback-void--
 *  */
// <publishKnowledgeBase>
const publishKnowledgeBase = async(kb_id) => {

    const results = await knowledgeBaseClient.publish(kb_id)

    if(!results._response.status.toString().indexOf("2",0)==-1) {
        console.log(`Publish request failed - HTTP status ${results._response.status}`)
        return false
    }

    console.log(`Publish request succeeded - HTTP status ${results._response.status}`)

    return true
}
// </publishKnowledgeBase>

/*
 * Get Endpoint Keys - use this key to get answer from knowledge base using query prediction runtime
 * https://docs.microsoft.com/javascript/api/@azure/cognitiveservices-qnamaker/endpointkeys?view=azure-node-latest
 */
// <getEndpointKeys>
const getEndpointKeys = async () => {

    const endpointKeysClient = new qnamaker.EndpointKeys(qnaMakerClient);

    const results = await endpointKeysClient.getKeys();

    if(!results._response.status.toString().indexOf("2",0)==-1) {
        console.log(`GetEndpointKeys request failed - HTTP status ${results._response.status}`)
        return null
    }

    console.log(`GetEndpointKeys request succeeded - HTTP status ${results._response.status} - primary key ${results.primaryEndpointKey}`)

    return results.primaryEndpointKey
}
// </getEndpointKeys>

// <main>
const quickstart = async () => {

    const knowledgeBaseID = await createKnowledgeBase();

    await updateKnowledgeBase(knowledgeBaseID);

    await publishKnowledgeBase(knowledgeBaseID);

    const primaryEndpointKey = await getEndpointKeys();

    await listKnowledgeBases()

    await deleteKnowledgeBase(knowledgeBaseID)

}
// <main>

// <mainCall>
quickstart()
.then(()=> console.log("done"))
.catch(error => console.log(error));
// </mainCall>
