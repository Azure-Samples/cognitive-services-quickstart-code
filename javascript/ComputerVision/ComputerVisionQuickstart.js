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
 *  - Replace the values of `key` and `endpoint` with your Computer Vision key and endpoint.
 *  - This quickstart can be run all at once (node ComputerVisionQuickstart.js from the command line) or used to copy/paste sections as needed. 
 *    If sections are extracted, make sure to copy/paste the authenticate section too, as each example relies on it.
 *
 * Resources:
 *  - Node SDK: https://docs.microsoft.com/en-us/javascript/api/azure-cognitiveservices-computervision/?view=azure-node-latest
 *  - Documentation: https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/
 *  - API v3.2: https://westus.dev.cognitive.microsoft.com/docs/services/computer-vision-v3-2/operations/5d986960601faab4bf452005
 * 
 * Examples included in this quickstart:
 * Extract text using Read API.
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
const key = 'PASTE_YOUR_COMPUTER_VISION_KEY_HERE';
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
        * OCR: READ API
        *
        * This example recognizes both handwritten and printed text, and can handle image files (.jpg/.png/.bmp) and multi-page files (.pdf and .tiff)
        * Please see REST API reference for more information:
        * Read: https://westus.dev.cognitive.microsoft.com/docs/services/computer-vision-v3-2/operations/5d986960601faab4bf452005
        * Get Read Result: https://westus.dev.cognitive.microsoft.com/docs/services/computer-vision-v3-2/operations/5d9869604be85dee480c8750
        * 
        */

      // <snippet_statuses>
      // Status strings returned from Read API. NOTE: CASING IS SIGNIFICANT.
      // Before Read 3.0, these are "Succeeded" and "Failed"
      const STATUS_SUCCEEDED = "succeeded";
      const STATUS_FAILED = "failed"
      // </snippet_statuses>

      console.log('-------------------------------------------------');
      console.log('READ');
      console.log();
      const printedTextURL = 'https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/printed_text.jpg';
      const handwrittenTextURL = 'https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/handwritten_text.jpg';

      const handwrittenImagePath = __dirname + '\\handwritten_text.jpg';
      try {
        await downloadFilesToLocal(handwrittenTextURL, handwrittenImagePath);
      } catch {
        console.log('>>> Download sample file failed. Sample cannot continue');
        process.exit(1);
      }

      console.log('\nReading URL image for text in ...', printedTextURL.split('/').pop());
      // API call returns a ReadResponse, grab the operation location (ID) from the response.
      const operationLocationUrl = await computerVisionClient.read(printedTextURL)
        .then((response) => {
          return response.operationLocation;
        });

      console.log();
      // From the operation location URL, grab the last element, the operation ID.
      const operationIdUrl = operationLocationUrl.substring(operationLocationUrl.lastIndexOf('/') + 1);

      // Wait for the read operation to finish, use the operationId to get the result.
      while (true) {
        const readOpResult = await computerVisionClient.getReadResult(operationIdUrl)
          .then((result) => {
            return result;
          })
        console.log('Read status: ' + readOpResult.status)
        if (readOpResult.status === STATUS_FAILED) {
          console.log('The Read File operation has failed.')
          break;
        }
        if (readOpResult.status === STATUS_SUCCEEDED) {
          console.log('The Read File operation was a success.');
          console.log();
          console.log('Read File URL image result:');
          // Print the text captured

          // Looping through: TextRecognitionResult[], then Line[]
          for (const textRecResult of readOpResult.analyzeResult.readResults) {
            for (const line of textRecResult.lines) {
              console.log(line.text)
            }
          }
          break;
        }
        await sleep(1000);
      }
      console.log();

      // With a local image, get the text.
      console.log('\Reading local image for text in ...', path.basename(handwrittenImagePath));

      // Call API, returns a Promise<Models.readInStreamResponse>
      const streamResponse = await computerVisionClient.readInStream(() => createReadStream(handwrittenImagePath))
        .then((response) => {
          return response;
        });

      console.log();
      // Get operation location from response, so you can get the operation ID.
      const operationLocationLocal = streamResponse.operationLocation
      // Get the operation ID at the end of the URL
      const operationIdLocal = operationLocationLocal.substring(operationLocationLocal.lastIndexOf('/') + 1);

      // Wait for the read operation to finish, use the operationId to get the result.
      while (true) {
        const readOpResult = await computerVisionClient.getReadResult(operationIdLocal)
          .then((result) => {
            return result;
          })
        console.log('Read status: ' + readOpResult.status)
        if (readOpResult.status === STATUS_FAILED) {
          console.log('The Read File operation has failed.')
          break;
        }
        if (readOpResult.status === STATUS_SUCCEEDED) {
          console.log('The Read File operation was a success.');
          console.log();
          console.log('Read File local image result:');
          // Print the text captured

          // Looping through: pages of result from readResults[], then Line[]
          for (const textRecResult of readOpResult.analyzeResult.readResults) {
            for (const line of textRecResult.lines) {
              console.log(line.text)
            }
          }
          break;
        }
        await sleep(1000);
      }
      console.log();
      /**
      * END - READ API
      */

      /**
       * OCR: READ PRINTED & HANDWRITTEN TEXT WITH THE READ API
       * Extracts text from images using OCR (optical character recognition).
       */
      console.log('-------------------------------------------------');
      console.log('READ PRINTED, HANDWRITTEN TEXT AND PDF');
      console.log();

      // <snippet_read_images>
      // URL images containing printed and/or handwritten text. 
      // The URL can point to image files (.jpg/.png/.bmp) or multi-page files (.pdf, .tiff).
      const printedTextSampleURL = 'https://github.com/Azure-Samples/cognitive-services-sample-data-files/blob/master/ComputerVision/Images/printed_text.jpg?raw=true';
      const multiLingualTextURL = 'https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/MultiLingual.png';
      const mixedMultiPagePDFURL = 'https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/MultiPageHandwrittenForm.pdf';
      // </snippet_read_images>
      const handwrittenImageLocalPath = __dirname + '\\handwritten_text.jpg';


      // <snippet_read_call>
      // Recognize text in printed image from a URL
      console.log('Read printed text from URL...', printedTextSampleURL.split('/').pop());
      const printedResult = await readTextFromURL(computerVisionClient, printedTextSampleURL);
      printRecText(printedResult);

      // Recognize multi-lingual text in a PNG from a URL
      console.log('\nRead printed multi-lingual text in a PNG from URL...', multiLingualTextURL.split('/').pop());
      const multiLingualResult = await readTextFromURL(computerVisionClient, multiLingualTextURL);
      printRecText(multiLingualResult);

      // Recognize printed text and handwritten text in a PDF from a URL
      console.log('\nRead printed and handwritten text from a PDF from URL...', mixedMultiPagePDFURL.split('/').pop());
      const mixedPdfResult = await readTextFromURL(computerVisionClient, mixedMultiPagePDFURL);
      printRecText(mixedPdfResult);
      // </snippet_read_call>

      // Recognize text in handwritten image from a local file
      console.log('\nRead handwritten text from local file...', handwrittenImageLocalPath);
      const handwritingResult = await readTextFromFile(computerVisionClient, handwrittenImageLocalPath);
      printRecText(handwritingResult);

      // <snippet_read_helper>
      // Perform read and await the result from URL
      async function readTextFromURL(client, url) {
        // To recognize text in a local image, replace client.read() with readTextInStream() as shown:
        let result = await client.read(url);
        // Operation ID is last path segment of operationLocation (a URL)
        let operation = result.operationLocation.split('/').slice(-1)[0];

        // Wait for read recognition to complete
        // result.status is initially undefined, since it's the result of read
        while (result.status !== STATUS_SUCCEEDED) { await sleep(1000); result = await client.getReadResult(operation); }
        return result.analyzeResult.readResults; // Return the first page of result. Replace [0] with the desired page if this is a multi-page file such as .pdf or .tiff.
      }
      // </snippet_read_helper>

      // Perform read and await the result from local file
      async function readTextFromFile(client, localImagePath) {
        // To recognize text in a local image, replace client.read() with readTextInStream() as shown:
        let result = await client.readInStream(() => createReadStream(localImagePath));
        // Operation ID is last path segment of operationLocation (a URL)
        let operation = result.operationLocation.split('/').slice(-1)[0];

        // Wait for read recognition to complete
        // result.status is initially undefined, since it's the result of read
        while (result.status !== STATUS_SUCCEEDED) { await sleep(1000); result = await client.getReadResult(operation); }
        return result.analyzeResult.readResults; // Return the first page of result. Replace [0] with the desired page if this is a multi-page file such as .pdf or .tiff.
      }

      // <snippet_read_print>
      // Prints all text from Read result
      function printRecText(readResults) {
        console.log('Recognized text:');
        for (const page in readResults) {
          if (readResults.length > 1) {
            console.log(`==== Page: ${page}`);
          }
          const result = readResults[page];
          if (result.lines.length) {
            for (const line of result.lines) {
              console.log(line.words.map(w => w.text).join(' '));
            }
          }
          else { console.log('No recognized text.'); }
        }
      }
      // </snippet_read_print>

      // <snippet_read_download>
      /**
       * 
       * Download the specified file in the URL to the current local folder
       * 
       */
      function downloadFilesToLocal(url, localFileName) {
        return new Promise((resolve, reject) => {
          console.log('--- Downloading file to local directory from: ' + url);
          const request = https.request(url, (res) => {
            if (res.statusCode !== 200) {
              console.log(`Download sample file failed. Status code: ${res.statusCode}, Message: ${res.statusMessage}`);
              reject();
            }
            var data = [];
            res.on('data', (chunk) => {
              data.push(chunk);
            });
            res.on('end', () => {
              console.log('   ... Downloaded successfully');
              fs.writeFileSync(localFileName, Buffer.concat(data));
              resolve();
            });
          });
          request.on('error', function (e) {
            console.log(e.message);
            reject();
          });
          request.end();
        });
      }
      // </snippet_read_download>

      /**
       * END - Recognize Printed & Handwritten Text
       */

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
