/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License. 
 */

/**
 * Computer Vision example
 * 
 * Prerequisites: 
 *  - Node.js 8.0+
 *  - Install the Computer Vision SDK: @azure/cognitiveservices-computervision (See https://www.npmjs.com/package/@azure/cognitiveservices-computervision) by running
 *    the following command in this directory:
 *       npm install
 *  - The DESCRIBE IMAGE example uses a local image celebrities.jpg, which will be downloaded on demand.
 *  - The READ (the API for performing Optical Character Recognition or doing text retrieval from PDF) example uses local images and a PDF files, which will be downloaded on demand.
 * 
 * How to run:
 *  - Replace the values of `key` and `endpoint` with your Computer Vision subscription key and endpoint.
 *  - This quickstart can be run all at once (node ComputerVisionQuickstart.js from the command line) or used to copy/paste sections as needed. 
 *    If sections are extracted, make sure to copy/paste the authenticate section too, as each example relies on it.
 *
 * Resources:
 *  - Node SDK: https://docs.microsoft.com/en-us/javascript/api/azure-cognitiveservices-computervision/?view=azure-node-latest
 *  - Documentation: https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/
 *  - API v3.2: https://westus.dev.cognitive.microsoft.com/docs/services/computer-vision-v3-2/operations/5d986960601faab4bf452005
 * 
 * Examples included in this quickstart:
 * Authenticate, Describe Image, Detect Faces, Detect Objects, Detect Tags, Detect Type, 
 * Detect Category, Detect Brand, Detect Color Scheme, Detect Domain-specific Content, Detect Adult Content
 * Generate Thumbnail
 */

// <snippet_imports_and_vars>
// <snippet_imports>
'use strict';

const async = require('async');
const fs = require('fs');
const https = require('https');
const path = require("path");
const createReadStream = require('fs').createReadStream
const sleep = require('util').promisify(setTimeout);
const ComputerVisionClient = require('@azure/cognitiveservices-computervision').ComputerVisionClient;
const ApiKeyCredentials = require('@azure/ms-rest-js').ApiKeyCredentials;
// </snippet_imports>

// <snippet_vars>
/**
 * AUTHENTICATE
 * This single client is used for all examples.
 */
 const key = 'PASTE_YOUR_COMPUTER_VISION_SUBSCRIPTION_KEY_HERE';
 const endpoint = 'PASTE_YOUR_COMPUTER_VISION_ENDPOINT_HERE';
// </snippet_vars>
// </snippet_imports_and_vars>

// <snippet_client>
const computerVisionClient = new ComputerVisionClient(
  new ApiKeyCredentials({ inHeader: { 'Ocp-Apim-Subscription-Key': key } }), endpoint);
// </snippet_client>
/**
 * END - Authenticate
 */

// <snippet_functiondef_begin>
function computerVision() {
  async.series([
    async function () {
      // </snippet_functiondef_begin>

      /**
       * Generate description 
       * Generate description of an image, which returns:
       *     all possible description in image and confidence score.
       */
       console.log('-------------------------------------------------');
       console.log('GENERATE DESCRIPTION');
       console.log();
 

      // <snippet_describe_image>
      const describeURL = 'https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/celebrities.jpg';
      // </snippet_describe_image>

      // <snippet_features_remote>
      // Get the visual feature for analysis
      const features = ['Description','Tags'];
      // </snippet_features_remote>

      // <snippet_analyze>
      const result = (await computerVisionClient.analyzeImage(describeURL,{visualFeatures: features}));

      // Detect tags
      const tags = result.tags;
      console.log(`Tags: ${formatTags(tags)}`);

      // Format tags for display
      function formatTags(tags) {
        return tags.map(tag => (`${tag.name} (${tag.confidence.toFixed(2)})`)).join(', ');
      }
      console.log();

      console.log(`Description: ${formatDescription(result.description.captions)}`);

      // Format tags for display
      function formatDescription(captions) {
        return captions.map(caption => (`${caption.text} (${caption.confidence.toFixed(2)})`)).join(', ');
      }
      // </snippet_analyze>

      console.log();
      console.log('-------------------------------------------------');
      console.log('End of quickstart.');
      // <snippet_functiondef_end>

    },
    function () {
      return new Promise((resolve) => {
        resolve();
      })
    }
  ], (err) => {
    throw (err);
  });
}

computerVision();
// </snippet_functiondef_end>
