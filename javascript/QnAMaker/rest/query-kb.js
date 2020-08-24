'use strict';

var request = require('request');
var request_as_promised = require('request-promise');

// Represents the various elements used to create HTTP request URIs
// for QnA Maker operations.
// From Publish Page: HOST
// Example: https://YOUR-RESOURCE-NAME.azurewebsites.net/qnamaker
var host = "https://YOUR-RESOURCE-NAME.azurewebsites.net/qnamaker";

// Authorization endpoint key
// From Publish Page
var endpoint_key = "YOUR-ENDPOINT-KEY";

// Management APIs postpend the version to the route
// From Publish Page, value after POST
// Example: /knowledgebases/ZZZ15f8c-d01b-4698-a2de-85b0dbf3358c/generateAnswer
var route = "/knowledgebases/YOUR-KNOWLEDGE-BASE-ID/generateAnswer";

// JSON format for passing question to service
var question = {'question': 'Is the QnA Maker Service free?','top': 3};

var getanswer = async () => {

    try{
        // Add an utterance
        var options = {
            uri: host + route,
            method: 'POST',
            headers: {
                'Authorization': "EndpointKey " + endpoint_key
            },
            json: true,
            body: question
        };

        var response = await request_as_promised.post(options);

        console.log(response);

    } catch (err){
        console.log(err.statusCode);
        console.log(err.message);
        console.log(err.error);
    }
};

getanswer();
