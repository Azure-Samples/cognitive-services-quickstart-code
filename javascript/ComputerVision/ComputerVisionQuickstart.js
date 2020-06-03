/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License. 
 */
// <snippet_imports>
'use strict';

const async = require('async');
const fs = require('fs');
const path = require("path");
const createReadStream = require('fs').createReadStream
const sleep = require('util').promisify(setTimeout);
const ComputerVisionClient = require('@azure/cognitiveservices-computervision').ComputerVisionClient;
const ApiKeyCredentials = require('@azure/ms-rest-js').ApiKeyCredentials;
// </snippet_imports>

/**
 * Computer Vision example
 * 
 * Prerequisites: 
 *  - Node.js 8.0+
 *  - Install the Computer Vision SDK: npm i @azure/cognitiveservices-computervision
 *       NPM package: https://www.npmjs.com/package/@azure/cognitiveservices-computervision
 *  - Install the 'ms-rest-js' package: npm i @azure/ms-rest-js
 *  - Install the 'async' package: npm i async
 *  - Set your subscription key and endpoint into your environment variables
 *  - The DESCRIBE IMAGE example uses a local image, download and place in your working folder: 
 *    https://moderatorsampleimages.blob.core.windows.net/samples/sample1.png 
 *  - The BATCH READ FILE example uses a local image, download and place in your working folder:
 *    https: //github.com/Azure-Samples/cognitive-services-sample-data-files/blob/master/ComputerVision/Images/handwritten_text.jpg
 * 
 * How to run:
 *  - This quickstart can be run all at once (node ComputerVisionQuickstart.js from the command line) or used to copy/paste sections as needed. 
 *    If sections are extracted, make sure to copy/paste the authenticate section too, as each example relies on it.
 *
 * Resources:
 *  - Node SDK: https://docs.microsoft.com/en-us/javascript/api/azure-cognitiveservices-computervision/?view=azure-node-latest
 *  - Documentation: https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/
 *  - API v2.0: https://westus.dev.cognitive.microsoft.com/docs/services/5adf991815e1060e6355ad44/operations/56f91f2e778daf14a499e1fa
 * 
 * Examples included in this quickstart:
 * Authenticate, Describe Image, Detect Faces, Detect Objects, Detect Tags, Detect Type, 
 * Detect Category, Detect Brand, Detect Color Scheme, Detect Domain-specific Content, Detect Adult Content
 * Generate Thumbnail, Batch Read File, Recognize Text (OCR), Recognize Printed & Handwritten Text.
 */

// <snippet_vars>
/**
 * AUTHENTICATE
 * This single client is used for all examples.
 */
let key = process.env['COMPUTER_VISION_SUBSCRIPTION_KEY'];
let endpoint = process.env['COMPUTER_VISION_ENDPOINT']
if (!key) { throw new Error('Set your environment variables for your subscription key and endpoint.'); }
// </snippet_vars>

// <snippet_client>
let computerVisionClient = new ComputerVisionClient(
    new ApiKeyCredentials({inHeader: {'Ocp-Apim-Subscription-Key': key}}), endpoint);
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
       * DESCRIBE IMAGE
       * Describes what the main objects or themes are in an image.
       * Describes both a URL and a local image.
       */
      console.log('-------------------------------------------------');
      console.log('DESCRIBE IMAGE');
      console.log();

      // <snippet_describe_image>
      var describeURL = 'https://moderatorsampleimages.blob.core.windows.net/samples/sample1.jpg';
      // </snippet_describe_image>
      var describeImagePath = __dirname + '\\sample1.png';

      // <snippet_describe>
      // Analyze URL image
      console.log('Analyzing URL image to describe...', describeURL.split('/').pop());
      var caption = (await computerVisionClient.describeImage(describeURL)).captions[0];
      console.log(`This may be ${caption.text} (${caption.confidence.toFixed(2)} confidence)`);
      // </snippet_describe>

      // Analyze local image
      console.log('\nAnalyzing local image to describe...', path.basename(describeImagePath));
      // DescribeImageInStream takes a function that returns a ReadableStream, NOT just a ReadableStream instance.
      var caption = (await computerVisionClient.describeImageInStream(
          () => createReadStream(describeImagePath))).captions[0];
      console.log(`This may be ${caption.text} (${caption.confidence.toFixed(2)} confidence)`);
      /**
       * END - Describe Image
       */
      console.log();

      /**
       * DETECT FACES
       * This example detects faces and returns its:
       *     gender, age, location of face (bounding box), confidence score, and size of face.
       */
      console.log('-------------------------------------------------');
      console.log('DETECT FACES');
      console.log();

      // <snippet_faces>
      const facesImageURL = 'https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/faces.jpg';

      // Analyze URL image.
      console.log('Analyzing faces in image...', facesImageURL.split('/').pop());
      // Get the visual feature for 'Faces' only.
      let faces = (await computerVisionClient.analyzeImage(facesImageURL, {visualFeatures: ['Faces']})).faces;

      // Print the bounding box, gender, and age from the faces.
      if (faces.length) {
        console.log(`${faces.length} face${faces.length == 1 ? '' : 's'} found:`);
        for (let face of faces) { console.log(`    Gender: ${face.gender}`.padEnd(20) 
          + ` Age: ${face.age}`.padEnd(10) + `at ${formatRectFaces(face.faceRectangle)}`); }
      } else { console.log('No faces found.'); }
      // </snippet_faces>

      // <snippet_formatfaces>
      // Formats the bounding box
      function formatRectFaces(rect) {
        return `top=${rect.top}`.padEnd(10) + `left=${rect.left}`.padEnd(10) + `bottom=${rect.top + rect.height}`.padEnd(12) 
          + `right=${rect.left + rect.width}`.padEnd(10) + `(${rect.width}x${rect.height})`;
      }
      // </snippet_formatfaces>
     
      /**
       * END - Detect Faces
       */
      console.log();

      /**
       * DETECT OBJECTS
       * Detects objects in URL image:
       *     gives confidence score, shows location of object in image (bounding box), and object size. 
       */
      console.log('-------------------------------------------------');
      console.log('DETECT OBJECTS');
      console.log();
      
      // <snippet_objects>
      // Image of a dog
      const objectURL = 'https://raw.githubusercontent.com/Azure-Samples/cognitive-services-node-sdk-samples/master/Data/image.jpg';

      // Analyze a URL image
      console.log('Analyzing objects in image...', objectURL.split('/').pop());
      let objects = (await computerVisionClient.analyzeImage(objectURL, {visualFeatures: ['Objects']})).objects;
      console.log();

      // Print objects bounding box and confidence
      if (objects.length) {
          console.log(`${objects.length} object${objects.length == 1 ? '' : 's'} found:`);
          for (let obj of objects) { console.log(`    ${obj.object} (${obj.confidence.toFixed(2)}) at ${formatRectObjects(obj.rectangle)}`); }
      } else { console.log('No objects found.'); }
      // </snippet_objects>

      // <snippet_objectformat>
      // Formats the bounding box
      function formatRectObjects(rect) {
        return `top=${rect.y}`.padEnd(10) + `left=${rect.x}`.padEnd(10) + `bottom=${rect.y + rect.h}`.padEnd(12) 
        + `right=${rect.x + rect.w}`.padEnd(10) + `(${rect.w}x${rect.h})`;
      }
      // </snippet_objectformat>
      /**
       * END - Detect Objects
       */
      console.log();

      /**
       * DETECT TAGS  
       * Detects tags for an image, which returns:
       *     all objects in image and confidence score.
       */
      // <snippet_tags>
      console.log('-------------------------------------------------');
      console.log('DETECT TAGS');
      console.log();

      // Image of different kind of dog.
      const tagsURL = 'https://moderatorsampleimages.blob.core.windows.net/samples/sample16.png';

      // Analyze URL image
      console.log('Analyzing tags in image...', tagsURL.split('/').pop());
      let tags = (await computerVisionClient.analyzeImage(tagsURL, {visualFeatures: ['Tags']})).tags;
      console.log(`Tags: ${formatTags(tags)}`);
      // </snippet_tags>

      // <snippet_tagsformat>
      // Format tags for display
      function formatTags(tags) {
        return tags.map(tag => (`${tag.name} (${tag.confidence.toFixed(2)})`)).join(', ');
      }
      // </snippet_tagsformat>
      /**
       * END - Detect Tags
       */
      console.log();

      /**
       * DETECT TYPE
       * Detects the type of image, says whether it is clip art, a line drawing, or photograph).
       */
      console.log('-------------------------------------------------');
      console.log('DETECT TYPE');
      console.log();

      // <snippet_imagetype>
      const typeURLImage = 'https://raw.githubusercontent.com/Azure-Samples/cognitive-services-python-sdk-samples/master/samples/vision/images/make_things_happen.jpg';

       // Analyze URL image
      console.log('Analyzing type in image...', typeURLImage.split('/').pop());
      let types = (await computerVisionClient.analyzeImage(typeURLImage, {visualFeatures: ['ImageType']})).imageType;
      console.log(`Image appears to be ${describeType(types)}`);
      // </snippet_imagetype>

      // <snippet_imagetype_describe>
      function describeType(imageType) {
        if (imageType.clipArtType && imageType.clipArtType > imageType.lineDrawingType) return 'clip art';
        if (imageType.lineDrawingType && imageType.clipArtType < imageType.lineDrawingType) return 'a line drawing';
        return 'a photograph';
      }
      // </snippet_imagetype_describe>
      /**
       * END - Detect Type
       */
      console.log();

      /**
       * DETECT CATEGORY
       * Detects the categories of an image. Two different images are used to show the scope of the features.
       */
      console.log('-------------------------------------------------');
      console.log('DETECT CATEGORY');
      console.log();

      // <snippet_categories>
      const categoryURLImage = 'https://moderatorsampleimages.blob.core.windows.net/samples/sample16.png';

      // Analyze URL image
      console.log('Analyzing category in image...', categoryURLImage.split('/').pop());
      let categories = (await computerVisionClient.analyzeImage(categoryURLImage)).categories;
      console.log(`Categories: ${formatCategories(categories)}`);
      // </snippet_categories>

      // <snippet_categories_format>
      // Formats the image categories
      function formatCategories(categories) {
        categories.sort((a, b) => b.score - a.score);
        return categories.map(cat => `${cat.name} (${cat.score.toFixed(2)})`).join(', ');
      }
      // </snippet_categories_format>
      /**
       * END - Detect Categories
       */
      console.log();

      /**
       * DETECT BRAND
       * Detects brands and logos that appear in an image.
       */
      console.log('-------------------------------------------------');
      console.log('DETECT BRAND');
      console.log();

      // <snippet_brands>
      const brandURLImage = 'https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/images/red-shirt-logo.jpg';

      // Analyze URL image
      console.log('Analyzing brands in image...', brandURLImage.split('/').pop());
      let brands = (await computerVisionClient.analyzeImage(brandURLImage, {visualFeatures: ['Brands']})).brands;

      // Print the brands found
      if (brands.length) {
          console.log(`${brands.length} brand${brands.length != 1 ? 's' : ''} found:`);
          for (let brand of brands) {
              console.log(`    ${brand.name} (${brand.confidence.toFixed(2)} confidence)`);
          }
      } else { console.log(`No brands found.`); }
      // </snippet_brands>
      console.log();

      /**
       * DETECT COLOR SCHEME
       * Detects the color scheme of an image, including foreground, background, dominant, and accent colors.  
       */
      console.log('-------------------------------------------------');
      console.log('DETECT COLOR SCHEME');
      console.log();

      // <snippet_colors>
      const colorURLImage = 'https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/celebrities.jpg';

      // Analyze URL image
      console.log('Analyzing image for color scheme...', colorURLImage.split('/').pop());
      console.log();
      let color = (await computerVisionClient.analyzeImage(colorURLImage, {visualFeatures: ['Color']})).color;
      printColorScheme(color);
      // </snippet_colors>

      // <snippet_colors_print>
      // Print a detected color scheme
      function printColorScheme(colors){
        console.log(`Image is in ${colors.isBwImg ? 'black and white' : 'color'}`);
        console.log(`Dominant colors: ${colors.dominantColors.join(', ')}`);
        console.log(`Dominant foreground color: ${colors.dominantColorForeground}`);
        console.log(`Dominant background color: ${colors.dominantColorBackground}`);
        console.log(`Suggested accent color: #${colors.accentColor}`);
      }
      // </snippet_colors_print>
      /**
       * END - Detect Color Scheme
       */
      console.log();

      /**
       * GENERATE THUMBNAIL
       * This example generates a thumbnail image of a specified size, from a URL and a local image.
       */
      console.log('-------------------------------------------------');
      console.log('GENERATE THUMBNAIL');
      console.log();
      // Image of a dog.
      const dogURL = 'https://moderatorsampleimages.blob.core.windows.net/samples/sample16.png';
      console.log('Generating thumbnail...')
      await computerVisionClient.generateThumbnail(100, 100, dogURL, { smartCropping: true } )
          .then((thumbResponse) => {
            const destination = fs.createWriteStream("thumb.png")
            thumbResponse.readableStreamBody.pipe(destination)
            console.log('Thumbnail saved.') // Saves into root folder
          })
      console.log();
      /**
       * END - Generate Thumbnail
       */

      /**
      * DETECT DOMAIN-SPECIFIC CONTENT
      * Detects landmarks or celebrities.
      */
      console.log('-------------------------------------------------');
      console.log('DETECT DOMAIN-SPECIFIC CONTENT');
      console.log();

      // <snippet_domain_image>
      const domainURLImage = 'https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/landmark.jpg';
      // </snippet_domain_image>

      // <snippet_landmarks>
      // Analyze URL image
      console.log('Analyzing image for landmarks...', domainURLImage.split('/').pop());
      let domain = (await computerVisionClient.analyzeImageByDomain('landmarks', domainURLImage)).result.landmarks;

      // Prints domain-specific, recognized objects
      if (domain.length) {
        console.log(`${domain.length} ${domain.length == 1 ? 'landmark' : 'landmarks'} found:`);
        for (let obj of domain) {
          console.log(`    ${obj.name}`.padEnd(20) + `(${obj.confidence.toFixed(2)} confidence)`.padEnd(20) + `${formatRectDomain(obj.faceRectangle)}`);
        }
      } else {
        console.log('No landmarks found.');
      }
      // </snippet_landmarks>

      // <snippet_landmarks_rect>
      // Formats bounding box
      function formatRectDomain(rect) {
        if (!rect) return '';
        return `top=${rect.top}`.padEnd(10) + `left=${rect.left}`.padEnd(10) + `bottom=${rect.top + rect.height}`.padEnd(12) +
          `right=${rect.left + rect.width}`.padEnd(10) + `(${rect.width}x${rect.height})`;
      }
      // </snippet_landmarks_rect>

      console.log();

      /**
      * DETECT ADULT CONTENT
      * Detects "adult" or "racy" content that may be found in images. 
      * The score closer to 1.0 indicates racy/adult content.
      * Detection for both local and URL images.
      */
      console.log('-------------------------------------------------');
      console.log('DETECT ADULT CONTENT');
      console.log();

      // <snippet_adult_image>
      // The URL image and local images are not racy/adult. 
      // Try your own racy/adult images for a more effective result.
      const adultURLImage = 'https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/celebrities.jpg';
      // </snippet_adult_image>

      // <snippet_adult>
      // Function to confirm racy or not
      const isIt = flag => flag ? 'is' : "isn't";

      // Analyze URL image
      console.log('Analyzing image for racy/adult content...', adultURLImage.split('/').pop());
      var adult = (await computerVisionClient.analyzeImage(adultURLImage, {
        visualFeatures: ['Adult']
      })).adult;
      console.log(`This probably ${isIt(adult.isAdultContent)} adult content (${adult.adultScore.toFixed(4)} score)`);
      console.log(`This probably ${isIt(adult.isRacyContent)} racy content (${adult.racyScore.toFixed(4)} score)`);
      // </snippet_adult>
      console.log();
      /**
      * END - Detect Adult Content
      */

     /**
       * BATCH READ FILE
       * This example recognizes both handwritten and printed text with more accurate results than
       * the RecognizeText API calls.
       */
      console.log('-------------------------------------------------');
      console.log('BATCH READ FILE');
      console.log();
      let printedTextBatchUrl = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/printed_text.jpg"
      let handwrittenBatchImagePath = __dirname + '\\handwritten_text.jpg';

      console.log('\nBatch reading URL image for text in ...', printedTextBatchUrl.split('/').pop());
      // API call returns a BatchReadFileResponse, grab the operation location (ID) from the response.
      let operationLocationUrl = await computerVisionClient.batchReadFile(printedTextBatchUrl)
          .then((response) => {
            return response.operationLocation;
          });

      console.log();
      // From the operation location URL, grab the last element, the operation ID.
      let operationIdUrl = operationLocationUrl.substring(operationLocationUrl.lastIndexOf('/') + 1);

      // Wait for the read operation to finish, use the operationId to get the result.
      while(true) {
        let readOpResult = await computerVisionClient.getReadOperationResult(operationIdUrl)
            .then((result) => {
              return result;
            })
        console.log('Batch Read status: ' + readOpResult.status)
        if (readOpResult.status == "Failed") {
          console.log('The Batch Read File operation has failed.')
          break;
        }
        if (readOpResult.status == "Succeeded") {
          console.log('The Batch Read File operation was a success.');
          console.log();
          console.log('Batch Read File URL image result:');
          // Print the text captured

          // Looping through: TextRecognitionResult[], then Line[]
          for (let textRecResult of readOpResult.recognitionResults) {
            for (let line of textRecResult.lines) {
                console.log(line.text)
              }
          }
          break;
        }
        await sleep(1000);
      }
      console.log();

      // With a local image, get the text.
      console.log('\nBatch reading local image for text in ...', path.basename(handwrittenBatchImagePath));

      // Call API, returns a Promise<Models.BatchReadFileInStreamResponse>
      let batchStreamResponse = await computerVisionClient.batchReadFileInStream(() => createReadStream(handwrittenBatchImagePath))
        .then((response) => {
            return response;
        });

      console.log();
      // Get operation location from response, so you can get the operation ID.
      let operationLocationLocal = batchStreamResponse.operationLocation
      // Get the operation ID at the end of the URL
      let operationIdLocal = operationLocationLocal.substring(operationLocationLocal.lastIndexOf('/') + 1);

      // Wait for the read operation to finish, use the operationId to get the result.
      while (true) {
        let readOpResult = await computerVisionClient.getReadOperationResult(operationIdLocal)
          .then((result) => {
            return result;
          })
        console.log('Batch Read status: ' + readOpResult.status)
        if (readOpResult.status == "Failed") {
          console.log('The Batch Read File operation has failed.')
          break;
        }
        if (readOpResult.status == "Succeeded") {
          console.log('The Batch Read File operation was a success.');
          console.log();
          console.log('Batch Read File local image result:');
          // Print the text captured

          // Looping through: TextRecognitionResult[], then Line[]
          for (let textRecResult of readOpResult.recognitionResults) {
            for (let line of textRecResult.lines) {
              console.log(line.text)
            }
          }
          break;
        }
        await sleep(1000);
      }
      console.log();
      /**
      * END - BATCH READ FILE
      */

      /**
       * RECOGNIZE TEXT (OCR) 
       * Recognizes text from images using OCR (optical character recognition). 
       */
      console.log('-------------------------------------------------');
      console.log('RECOGNIZE TEXT');
      console.log();

      const textURLImage = 'https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/printed_text.jpg';

      // Recognize text in URL image
      console.log('Recognizing text from image...', textURLImage.split('/').pop());
      let ocr = await computerVisionClient.recognizePrintedText(true, textURLImage);
      console.log(`Language: ${ocr.language}; Orientation: ${ocr.orientation}`);
      printText(ocr);
      
      function printText(ocr) {
        if (ocr.regions.length) {
            console.log('Recognized text:');
            for (let region of ocr.regions) {
                for (let line of region.lines) {
                    console.log(line.words.map(w => w.text).join(' '));
                }
            }
        }
        else { console.log('No recognized text.'); }
      }
      /**
       * END - Recognize Text
       */
     console.log();

     /**
      * RECOGNIZE PRINTED & HANDWRITTEN TEXT
      * Recognizes text from images using OCR (optical character recognition).
      * Recognition is shown for both printed and handwritten text.
      */
     console.log('-------------------------------------------------');
     console.log('RECOGNIZE PRINTED & HANDWRITTEN TEXT');
     console.log();

     // <snippet_read_images>
     // URL images containing printed and handwritten text
      const printedText     = 'https://moderatorsampleimages.blob.core.windows.net/samples/sample2.jpg';
      const handwrittenText = 'https://raw.githubusercontent.com/MicrosoftDocs/azure-docs/master/articles/cognitive-services/Computer-vision/Images/readsample.jpg';
      // </snippet_read_images>
      
      // <snippet_read_call>
      // Recognize text in printed image
      console.log('Recognizing printed text...', printedText.split('/').pop());
      var printed = await recognizeText(computerVisionClient, 'Printed', printedText);
      printRecText(printed);

      // Recognize text in handwritten image
      console.log('\nRecognizing handwritten text...', handwrittenText.split('/').pop());
      var handwriting = await recognizeText(computerVisionClient, 'Handwritten', handwrittenText);
      printRecText(handwriting);
      // </snippet_read_call>

      // <snippet_read_helper>
      // Perform text recognition and await the result
      async function recognizeText(client, mode, url) {
        // To recognize text in a local image, replace client.recognizeText() with recognizeTextInStream() as shown:
        // result = await client.recognizeTextInStream(mode, () => createReadStream(localImagePath));
        let result = await client.recognizeText(mode, url);
        // Operation ID is last path segment of operationLocation (a URL)
        let operation = result.operationLocation.split('/').slice(-1)[0];

        // Wait for text recognition to complete
        // result.status is initially undefined, since it's the result of recognizeText
        while (result.status !== 'Succeeded') { await sleep(1000); result = await client.getTextOperationResult(operation); }
        return result.recognitionResult;
      }
      // </snippet_read_helper>

      // <snippet_read_print>
      // Prints all text from OCR result
      function printRecText(ocr) {
        if (ocr.lines.length) {
            console.log('Recognized text:');
            for (let line of ocr.lines) {
                console.log(line.words.map(w => w.text).join(' '));
            }
        }
        else { console.log('No recognized text.'); }
      }
      // </snippet_read_print>
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
