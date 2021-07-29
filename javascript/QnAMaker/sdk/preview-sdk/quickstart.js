"use strict";

/*
 * ==========================================
   Install QnA Maker package with commands
 * ==========================================
    npm install @azure/cognitiveservices-qnamaker

 * ==========================================
   Tasks Included
 * ==========================================
 * - Create a knowledge base
 * - Update a knowledge base
 * - Publish a knowledge base
 * - Download a knowledgebase
 * - Query a knowledgebase
 * - Delete a knowledge base

 * ==========================================
 * IMPORTANT NOTES
   This quickstart shows how to query a knowledgebase using the V2 API,
   which does not require a separate runtime endpoint.
   Make sure you have package @azure/cognitiveservices-qnamaker/v/3.2.0 or later installed.
   The QnA Maker subscription key and endpoint must be for a QnA Maker Managed resource.
   When you create your QnA Maker resource in the MS Azure portal, select the "Managed" checkbox.
 * ==========================================

 * ==========================================
   Further reading
 * General documentation: https://docs.microsoft.com/azure/cognitive-services/QnAMaker
 * Reference documentation: https://docs.microsoft.com/dotnet/api/microsoft.azure.cognitiveservices.knowledge.qnamaker?view=azure-dotnet
 * ==========================================

 */

// <Dependencies>
const msRest = require("@azure/ms-rest-js");
const qnamaker = require("@azure/cognitiveservices-qnamaker");
// </Dependencies>

/*
* Set the `subscription_key` and `endpoint` variables to your
* QnA Maker authoring subscription key and endpoint.
*
* These values can be found in the Azure portal (ms.portal.azure.com/).
* Look up your QnA Maker resource. Then, in the "Resource management"
* section, find the "Keys and Endpoint" page.
*
* The value of `endpoint` has the format https://YOUR-RESOURCE-NAME.cognitiveservices.azure.com.
*/
// <Resourcevariables>
const subscription_key = "PASTE_YOUR_QNA_MAKER_MANAGED_SUBSCRIPTION_KEY_HERE";
const endpoint = "PASTE_YOUR_QNA_MAKER_MANAGED_ENDPOINT_HERE";
// </Resourcevariables>

// <ListKnowledgeBases>
const listKnowledgeBasesInResource = async (KBclient) => {

    const result = await KBclient.listAll()

    for (const item of result.knowledgebases) {
        console.log(`${item.name} stored in ${item.language} language with ${item.sources.length} sources, last updated ${item.lastChangedTimestamp}`);
    }
    return result.knowledgebases;
}
// </ListKnowledgeBases>

// <GenerateAnswer>
const generateAnswer = async (KBclient, kb_id) => {

	console.log(`Querying knowledge base...`)

    const requestQuery = await KBclient.generateAnswer(
        kb_id,
        {
            question: "How do I manage my knowledgebase?",
            top: 1,
            strictFilters: [
                { name: "Category", value: "api" }
            ]
        }
    );
    console.log(JSON.stringify(requestQuery));

}
// </GenerateAnswer>

// <DownloadKB>
const downloadKnowledgeBase = async (KBclient, kb_id) => {

	console.log(`Downloading knowledge base...`)

    var kbData = await KBclient.download(kb_id, "Prod");
    console.log(`Knowledge base downloaded. It has ${kbData.qnaDocuments.length} QnAs.`);

    // Do something meaningful with data
}
// </DownloadKB>

// <DeleteKB>
const deleteKnowledgeBase = async (KBclient, kb_id) => {

	console.log(`Deleting knowledge base...`)

    const results = await KBclient.deleteMethod(kb_id)

    if ( ! results._response.status.toString().startsWith("2")) {
        console.log(`Delete operation state failed - HTTP status ${results._response.status}`)
        return false
    }

    console.log(`Delete operation state succeeded - HTTP status ${results._response.status}`)
    return true
}
// </DeleteKB>


// <MonitorOperation>
const wait_for_operation = async (qnaClient, operation_id) => {

    let state = "NotStarted"
    let operationResult = undefined

    while ("Running" === state || "NotStarted" === state) {

        operationResult = await qnaClient.operations.getDetails(operation_id)
        state = operationResult.operationState;

        console.log(`Operation state - ${state}`)

        await delayTimer(1000);
    }

    return operationResult;
}
const delayTimer = async (timeInMs) => {
    return await new Promise((resolve) => {
        setTimeout(resolve, timeInMs);
    });
}
// </MonitorOperation>


// <CreateKBMethod>
const createKnowledgeBase = async (qnaClient, kbclient) => {

	console.log(`Creating knowledge base...`)

    const qna1 = {
        answer: "Yes, You can use our [REST APIs](https://docs.microsoft.com/rest/api/cognitiveservices/qnamaker/knowledgebase) to manage your knowledge base.",
        questions: ["How do I manage my knowledgebase?"],
        metadata: [
            { name: "Category", value: "api" },
            { name: "Language", value: "REST" }
        ]
    };

    const qna2 = {
        answer: "Yes, You can use our JS SDK on NPM for [authoring](https://www.npmjs.com/package/@azure/cognitiveservices-qnamaker), [query runtime](https://www.npmjs.com/package/@azure/cognitiveservices-qnamaker-runtime), and [the reference docs](https://docs.microsoft.com/en-us/javascript/api/@azure/cognitiveservices-qnamaker/?view=azure-node-latest) to manage your knowledge base.",
        questions: ["How do I manage my knowledgebase?"],
        metadata: [
            { name: "Category", value: "api" },
            { name: "Language", value: "JavaScript" }
        ]
    };

    const create_kb_payload = {
        name: 'QnA Maker JavaScript SDK Quickstart',
        qnaList: [
            qna1,
            qna2
        ],
        urls: [],
        files: [
            /*{
                fileName: "myfile.md",
                fileUri: "https://mydomain/myfile.md"
            }*/
        ],
        defaultAnswerUsedForExtraction: "No answer found.",
        enableHierarchicalExtraction: true,
        language: "English"
    };

    const results = await kbclient.create(create_kb_payload)

    if ( ! results._response.status.toString().startsWith("2")) {
        console.log(`Create request failed - HTTP status ${results._response.status}`)
        return
    }

    const operationResult = await wait_for_operation(qnaClient, results.operationId)

    if (!operationResult || !operationResult.operationState || !(operationResult.operationState = "Succeeded") || !operationResult.resourceLocation) {
        console.log(`Create operation state failed - HTTP status ${operationResult._response.status}`)
        return
    }

    // parse resourceLocation for KB ID
    const kbID = operationResult.resourceLocation.replace("/knowledgebases/", "");

    return kbID;
}
// </CreateKBMethod>

// <UpdateKBMethod>
const updateKnowledgeBase = async (qnaClient, kbclient, kb_id) => {

	console.log(`Updating knowledge base...`)

    const urls = [
        "https://docs.microsoft.com/azure/cognitive-services/QnAMaker/troubleshooting"
    ]

    const qna3 = {
        answer: "goodbye",
        questions: [
            "bye",
            "end",
            "stop",
            "quit",
            "done"
        ],
        metadata: [
            { name: "Category", value: "Chitchat" },
            { name: "Chitchat", value: "end" }
        ]
    };

    const qna4 = {
        answer: "Hello, please select from the list of questions or enter a new question to continue.",
        questions: [
            "hello",
            "hi",
            "start"
        ],
        metadata: [
            { name: "Category", value: "Chitchat" },
            { name: "Chitchat", value: "begin" }
        ],
        context: {
            isContextOnly: false,
            prompts: [
                {
                    displayOrder: 1,
                    displayText: "Use REST",
                    qna: null,
                    qnaId: 1
                },
                {
                    displayOrder: 2,
                    displayText: "Use JS NPM package",
                    qna: null,
                    qnaId: 2
                },
            ]
        }
    };

    console.log(JSON.stringify(qna4))

    // Add new Q&A lists, URLs, and files to the KB.
    const kb_add_payload = {
        qnaList: [
            qna3,
            qna4
        ],
        urls: urls,
        files: []
    };

    // Bundle the add, update, and delete requests.
    const update_kb_payload = {
        add: kb_add_payload,
        update: null,
        delete: null,
        defaultAnswerUsedForExtraction: "No answer found. Please rephrase your question."
    };

    console.log(JSON.stringify(update_kb_payload))

    const results = await kbclient.update(kb_id, update_kb_payload)

    if ( ! results._response.status.toString().startsWith("2")) {
        console.log(`Update request failed - HTTP status ${results._response.status}`)
        return false
    }

    const operationResult = await wait_for_operation(qnaClient, results.operationId)

    if (operationResult.operationState != "Succeeded") {
        console.log(`Update operation state failed - HTTP status ${operationResult._response.status}`)
        return false
    }

    console.log(`Update operation state ${operationResult._response.status} - HTTP status ${operationResult._response.status}`)
    return true
}
// </UpdateKBMethod>

// <PublishKB>
const publishKnowledgeBase = async (kbclient, kb_id) => {

	console.log(`Publishing knowledge base...`)

    const results = await kbclient.publish(kb_id)

    if ( ! results._response.status.toString().startsWith("2")) {
        console.log(`Publish request failed - HTTP status ${results._response.status}`)
        return false
    }

    console.log(`Publish request succeeded - HTTP status ${results._response.status}`)

    return true
}
// </PublishKB>

// <Main>
const main = async () => {

    // <AuthorizationAuthor>
    const creds = new msRest.ApiKeyCredentials({ inHeader: { 'Ocp-Apim-Subscription-Key': subscription_key } });
    const qnaMakerClient = new qnamaker.QnAMakerClient(creds, endpoint);
    const knowledgeBaseClient = new qnamaker.Knowledgebase(qnaMakerClient);
    // </AuthorizationAuthor>

    const knowledgeBaseID = await createKnowledgeBase(qnaMakerClient, knowledgeBaseClient);
    await updateKnowledgeBase(qnaMakerClient, knowledgeBaseClient, knowledgeBaseID);
    await publishKnowledgeBase(knowledgeBaseClient, knowledgeBaseID);
    await downloadKnowledgeBase(knowledgeBaseClient, knowledgeBaseID)
    await listKnowledgeBasesInResource(knowledgeBaseClient)
	await generateAnswer(knowledgeBaseClient, knowledgeBaseID);
    await deleteKnowledgeBase(knowledgeBaseClient, knowledgeBaseID)
}
// </Main>

// <mainCall>
main()
    .then(() => console.log("done"))
    .catch(error => console.log(error));
// </mainCall>
