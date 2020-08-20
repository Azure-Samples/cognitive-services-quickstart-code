'use strict';

let fs = require ('fs');
let https = require ('https');

// **********************************************
// *** Update or verify the following values. ***
// **********************************************

// Represents the various elements used to create HTTP request URIs
// for QnA Maker operations.
let host = 'westus.api.cognitive.microsoft.com';
let service = '/qnamaker/v4.0';
let method = '/knowledgebases/';

// NOTE: Replace this with a valid subscription key.
let subscriptionKey = '<qna-maker-subscription-key>';

// NOTE: Replace this with a valid knowledge base ID.
let kb = '<qna-maker-knowledge-base-id>';

// Build your path URL.
let path = service + method + kb;

// Formats and indents JSON for display.
let pretty_print = function (s) {
    return JSON.stringify(JSON.parse(s), null, 4);
}

// Call 'callback' after we have the entire response.
let response_handler = function (callback, response) {
    let body = '';
    response.on ('data', function (d) {
        body += d;
    });
    response.on ('end', function () {
        // Calls 'callback' with the status code, headers, and body of the response.
        callback ({ status : response.statusCode, headers : response.headers, body : body });
    });
    response.on ('error', function (e) {
        console.log ('Error: ' + e.message);
    });
};

// Get an HTTP response handler that calls 'callback' when we have the entire response.
let get_response_handler = function (callback) {
    // Return a function that takes an HTTP response and is closed over the callback.
    // Function signature is required by https.request, hence the need for the closure.
    return function (response) {
        response_handler (callback, response);
    }
}

// Calls 'callback' when we have the entire response from the POST request.
let post = function (path, content, callback) {
    let request_params = {
        method : 'POST',
        hostname : host,
        path : path,
        headers : {
            'Content-Type' : 'application/json',
            'Content-Length' : content.length,
            'Ocp-Apim-Subscription-Key' : subscriptionKey,
        }
    };

    // Pass the callback function to the response handler.
    let req = https.request (request_params, get_response_handler (callback));
    req.write (content);
    req.end ();
}

// Calls 'callback' when we have the response from the /knowledgebases POST method.
let publish_kb = function (path, req, callback) {

    console.log ('Calling ' + host + path + '.');

    // Send the POST request.
    post (path, req, function (response) {

        // Extract data from the POST response and pass to 'callback'.
        if (response.status == '204') {

            let result = {'result':'Success'};
            callback (JSON.stringify(result));
        }
        else {
            callback (response.body);
        }
    });
}


// Sends the request to publish the knowledge base.
publish_kb (path, '', function (result) {
    console.log (pretty_print(result));
});
