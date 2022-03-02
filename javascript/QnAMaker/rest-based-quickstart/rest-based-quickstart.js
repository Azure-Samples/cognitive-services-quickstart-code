'use strict';

// <snippet_dependencies>
const request = require('requestretry');
// </snippet_dependencies>

// <snippet_variables>
// 36 character key from Azure portal for QnA Maker resource
const authoringKey = 'PASTE_YOUR_QNA_MAKER_AUTHORING_SUBSCRIPTION_KEY_HERE';

// example://https://YOUR-RESOURCE_NAME.cognitiveservices.azure.com with NO trailing forward slash
const authoringEndpoint = 'PASTE_YOUR_QNA_MAKER_AUTHORING_ENDPOINT_HERE';

const runtimeEndpoint = 'PASTE_YOUR_QNA_MAKER_RUNTIME_ENDPOINT_HERE';

const service = "/qnamaker/v4.0";
// </snippet_variables>

// <snippet_knowledge_base_json>
const kb = {
  'name': 'QnA Maker FAQ',
  'qnaList': [
    {
      'id': 0,
      'answer': 'You can use our REST APIs to manage your knowledge base. See here for details: https://westus.dev.cognitive.microsoft.com/docs/services/58994a073d9e04097c7ba6fe/operations/58994a073d9e041ad42d9baa',
      'source': 'Custom Editorial',
      'questions': [
        'How do I programmatically update my knowledge base?'
      ],
      'metadata': [
        {
          'name': 'category',
          'value': 'api'
        }
      ]
    }
  ],
  'urls': [
    'https://docs.microsoft.com/en-in/azure/cognitive-services/qnamaker/faqs'
  ],
  'files': []
};
// </snippet_knowledge_base_json>

const myRetryStrategy = (err, response) => {

    // retry the request if we had an error or if the response was a 'Bad Gateway'
    return !!err || response.statusCode === "403"|| response.statusCode === "429";
  }

// <snippet_get>
const requestUri = async (uri, key, method, body, authoringAuthorizationHeader=true) => {

    let headers = {
        'Ocp-Apim-Subscription-Key': key
    };

    if(!authoringAuthorizationHeader){
      headers = {'Authorization':`EndpointKey ${key}`};
  }

    const options = {
        method: method,
        url: uri,
        maxAttempts: 5,   // (default) try 5 times
        retryDelay: 5000,
        headers: headers,
        fullResponse: true,
        retryStrategy: myRetryStrategy
    };

    if(method==="POST"){
      options.json=true;
      options.body=body;
    }

    // Pass the callback function to the response handler.
    return await request(options);
};
// </snippet_get>

// <snippet_create_kb>
// returns operation ID
const create_kb = async () => {

    const uri = `${authoringEndpoint}${service}/knowledgebases/create`;

    // Starts the QnA Maker operation to create the knowledge base.
    const response = await requestUri(uri, authoringKey, "POST", kb, true);

    return response.headers["location"];
};
// </snippet_create_kb>

// <snippet_get_status>
const getStatus = async (operation)=> {

    // Iteratively gets the state of the operation creating the
    // knowledge base. Once the operation state is set to something other
    // than "Running" or "NotStarted", the loop ends.
    let knowledgeBaseID = "";
    const uri = `${authoringEndpoint}${service}${operation}`;

    var done = false;
    while (true != done)
    {
        var response = await requestUri(uri, authoringKey, "GET", null, true);

        // Gets and checks the state of the operation.
        const state = JSON.parse(response.body)["operationState"];
        if (state==="Running" || state==="NotStarted")
        {
            // wait 5 seconds
            await new Promise(resolve => setTimeout(resolve, 5000));
        }
        else
        {
            // QnA Maker has completed creating the knowledge base.
            done = true;
            knowledgeBaseID = JSON.parse(response.body)["resourceLocation"].replace("/knowledgebases/", "");
        }
    }

    return knowledgeBaseID;
}
// </snippet_get_status>

// <snippet_publish_kb>
const publish = async(knowledgeBaseID)=> {

    const uri = `${authoringEndpoint}${service}/knowledgebases/${knowledgeBaseID}`;

    var response = await requestUri(uri, authoringKey, "POST", null, true);

    console.log(`KB published successfully? ${(response.statusCode===204 ? "Yes" : "No")}`);

    return response;
}
// </snippet_publish_kb>

// <snippet_get_endpoint_key>
const get_endpoint_key = async() => {

    const uri =  `${authoringEndpoint}${service}/endpointkeys`;

    var response = await requestUri(uri, authoringKey, "GET", null, true);

    return JSON.parse(response.body)["primaryEndpointKey"];
}
// </snippet_get_endpoint_key>

// <snippet_query>
const query = async(endpointKey, knowledgeBaseID)=>{

  const uri = runtimeEndpoint + '/qnamaker/knowledgebases/${knowledgeBaseID}/generateAnswer';

  const question = {'question': 'Is the QnA Maker Service free?','top': 3};

  var response = await requestUri(uri, endpointKey, "POST", question, false);

  return response.body;

}
// </snippet_query>

// <snippet_main>
const main = async() => {

    // Create knowledge base - get ID
    const operation = await create_kb();
    console.log(`operationID is ${operation}`);

    const knowledgeBaseID = await getStatus(operation);
    console.log(`knowledgeBaseID is ${knowledgeBaseID}`);

    // publish
    const result = await publish(knowledgeBaseID);

    // get endpoint key after publish
    const endpointKey = await get_endpoint_key();
    console.log(`endpointKey = ${endpointKey}`);

    // query endpoint
    const queryResponse = await query(endpointKey, knowledgeBaseID);

    // print results
    console.log(queryResponse);
}
// </snippet_main>


main()
.then(() =>  console.log("done"))
.catch((ex)=> console.log(ex));




