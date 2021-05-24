'use strict';

// <dependencies>
/* To install dependencies, run:
 * npm install requestretry
 */
const request = require("requestretry");

// time delay between requests
const delayMS = 500;

// retry recount
const retry = 5;

// retry reqeust if error or 429 received
var retryStrategy = function (err, response, body) {
    let shouldRetry = err || (response.statusCode === 429);
    if (shouldRetry) console.log("retry");
    return shouldRetry;
}

const sleep = (waitTimeInMs) => new Promise(resolve => setTimeout(resolve, waitTimeInMs));
// </dependencies>  

/*
* Set the `resourceKey` and `resourceAuthoringEndpoint` variables to your
* QnA Maker authoring subscription key and endpoint.
*
* These values can be found in the Azure portal (ms.portal.azure.com/).
* Look up your QnA Maker resource. Then, in the "Resource management"
* section, find the "Keys and Endpoint" page.
*
* The value of `resourceAuthoringEndpoint` has the format https://YOUR-RESOURCE-NAME.cognitiveservices.azure.com.
*/
// <authorization>
const resourceKey = "PASTE_YOUR_QNA_MAKER_SUBSCRIPTION_KEY_HERE";
const resourceAuthoringEndpoint = "PASTE_YOUR_QNA_MAKER_AUTHORING_ENDPOINT_HERE" + "/qnamaker/v4.0";
// </authorization>

// <utility>
// Formats and indents JSON for display.
const pretty_print = (s) => {
    return JSON.stringify(s, null, 4);
}
// </utility>

// <createKb>
const createKb = async () => {

    try{
  
        // Dictionary that holds the knowledge base.
        // The data source includes a QnA pair with metadata, the URL for the
        // QnA Maker FAQ article, and the URL for the Azure Bot Service FAQ article.
        const kb_model = {
            "name": "QnA Maker FAQ",
            "qnaList": [
                {
                    "id": 0,
                    "answer": "You can use our REST APIs to manage your Knowledge Base. See here for details: https://westus.dev.cognitive.microsoft.com/docs/services/58994a073d9e04097c7ba6fe/operations/58994a073d9e041ad42d9baa",
                    "source": "Custom Editorial",
                    "questions": [
                        "How do I programmatically update my Knowledge Base?"
                    ],
                    "metadata": [
                        {
                            "name": "category",
                            "value": "api"
                        }
                    ]
                }
            ],
            "urls": [
                "https://docs.microsoft.com/en-in/azure/cognitive-services/qnamaker/faqs"
            ],
            "files": []
        };        

        const request_params = {
            method: 'POST',
            uri: resourceAuthoringEndpoint + "/knowledgebases/create",
            headers: {
                'Content-Type': 'application/json',
                'Content-Length': JSON.stringify(kb_model).length,
                'Ocp-Apim-Subscription-Key': resourceKey
            },
            body: kb_model,
            json: true
        };

        const response = await request(request_params);

        if(response.statusCode != "202") throw Error(response.statusCode);

        console.log(response.body.operationId);
        console.log(response.body.operationState); 
        console.log(pretty_print(response.body));

        return response.body;

    } catch(err) {
        console.log(err);
    }
}
// </createKb>

// <deleteKb>
const deleteKb = async (kbId) => {

    try{
        const request_params = {
            method: 'DELETE',
            uri: resourceAuthoringEndpoint + "/knowledgebases/" + kbId,
            headers: {
                'Content-Type': 'application/json',
                'Ocp-Apim-Subscription-Key': resourceKey,
            },
            resolveWithFullResponse: true
        };

        return await request(request_params);
        
    } catch (err){
        throw err;
    }
}
// </deleteKb>

// <downloadKb>
const downloadKb = async (kbId) => {

    try{

        // slots: 'test' or 'prod'
        const slot = "test";

        const request_params = {
            method: 'GET',
            uri: resourceAuthoringEndpoint + "/knowledgebases/" + kbId + "/" + slot + "/qna/" ,
            headers: {
                'Ocp-Apim-Subscription-Key': resourceKey,
            },
            resolveWithFullResponse: true
        };


        return await request(request_params);

    } catch (err){
        throw err;
    }
}
 // </downloadKb>

// <replaceKb>
const replaceKb = async (kbId) => {

    try{
        const newKbDefinition = {
            'qnaList': [
            {
                'id': 0,
                'answer': 'You can use our REST APIs to manage your Knowledge Base. See here for details: https://westus.dev.cognitive.microsoft.com/docs/services/5a93fcf85b4ccd136866eb37/operations/5ac266295b4ccd1554da7600',
                'source': 'Custom Editorial',
                'questions': [
                'How do I programmatically update my Knowledge Base?'
                ],
                'metadata': [
                {
                    'name': 'category',
                    'value': 'api'
                }
                ]
            }
            ]
        };

        const request_params = {
            method : 'PUT',
            uri: resourceAuthoringEndpoint + "/knowledgebases/" + kbId,
            headers : {
                'Content-Type' : 'application/json',
                'Content-Length' : JSON.stringify(newKbDefinition).length,
                'Ocp-Apim-Subscription-Key' : resourceKey,
            },
            body: JSON.stringify(newKbDefinition),
            resolveWithFullResponse: true
        };
    
        return await request(request_params);

    } catch (err){
        throw err;
    }    
}      
// </replaceKb>

// <publishKb>
var publishKb = async (kbId) => {

    try{

        var options = {
            uri: resourceAuthoringEndpoint + "/knowledgebases/" + kbId,
            method: 'POST',
            headers: {
                'Ocp-Apim-Subscription-Key': resourceKey
            },
            resolveWithFullResponse: true
        };

        return await request(options);

    } catch (err){
        throw err;
    }
};
// </publishKb>

// <operationDetails>
const getOperationStatus = async (result) => {

    try{
    
        let response;
        let state = result.operationState;

        while( state==="Running" || state==="NotStarted"){

            const options = {
                method: 'GET',
                uri: resourceAuthoringEndpoint + "/operations/" + result.operationId,
                headers: {
                    'Ocp-Apim-Subscription-Key': resourceKey
                },
                resolveWithFullResponse: true
            };

            let responseFull = await request(options);
            response = JSON.parse(responseFull.body);
            state = response.operationState;

			console.log ("Waiting 10 seconds...")
            // artificial retry
            await sleep(10000);
        }

        return response;

    } catch(err) {
        console.log(err);
    }
}
// </operationDetails>

// <main>
const main = async()=>{

    try{
        const createResult = await createKb();
        const createdOperation = await getOperationStatus(createResult);
        
        const knowledgeBaseId = createdOperation.resourceLocation.replace("/knowledgebases/","");
        console.log("Created knowledge base ID = " + knowledgeBaseId);

        const updateResult = await replaceKb(knowledgeBaseId);
        if(updateResult.statusCode != "204") throw updateResult;
        console.log("updated...");

        const downloadResult = await downloadKb(knowledgeBaseId);
        if(downloadResult.statusCode != "200") throw downloadResult;
        console.log("downloaded...\n\n KB = " + downloadResult.body + "\n\n");

        const publishResult = await publishKb(knowledgeBaseId);
        if(publishResult.statusCode != "204") throw publishResult;
        console.log("published");

        const deleteResult = await deleteKb(knowledgeBaseId);
        if(deleteResult.statusCode != "204") throw deleteResult;
        console.log("deleted");

        return knowledgeBaseId;

    } catch (err){
        throw err;
    }
}

main()
.then(kbID => {
    console.log("KBID = " + kbID);
}).catch(err => {
    console.log(JSON.stringify(err));
})
// </main>
