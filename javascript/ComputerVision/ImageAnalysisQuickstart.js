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
       * DESCRIBE IMAGE
       * Describes what the main objects or themes are in an image.
       * Describes both a URL and a local image.
       */
      console.log('-------------------------------------------------');
      console.log('DESCRIBE IMAGE');
      console.log();

      // <snippet_describe_image>
      const describeURL = 'https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/celebrities.jpg';
      // </snippet_describe_image>

      // Image of a dog
      const objectURL = 'https://raw.githubusercontent.com/Azure-Samples/cognitive-services-node-sdk-samples/master/Data/image.jpg';

      // Image of different kind of dog.
      const tagsURL = 'https://moderatorsampleimages.blob.core.windows.net/samples/sample16.png';

      const typeURLImage = 'https://raw.githubusercontent.com/Azure-Samples/cognitive-services-python-sdk-samples/master/samples/vision/images/make_things_happen.jpg';

      const categoryURLImage = 'https://moderatorsampleimages.blob.core.windows.net/samples/sample16.png';

      const brandURLImage = 'https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/images/red-shirt-logo.jpg';

      const colorURLImage = 'https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/celebrities.jpg';
 
      const domainURLImage = 'https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/landmark.jpg';
      
      const adultURLImage = 'https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/celebrities.jpg';


      // local image path
      const describeImagePath = __dirname + '\\celebrities.jpg';
      try {
        await downloadFilesToLocal(describeURL, describeImagePath);
      } catch {
        console.log('>>> Download sample file failed. Sample cannot continue');
        process.exit(1);
      }

    

      /**
       * ANALYZE ALL FEATURES
       * This example includes all the features available from the Analyze Image API call.
       */
      console.log('-------------------------------------------------');
      console.log('Analyze Image');
      console.log();

      const analyzeImageURL = 'https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/celebrities.jpg';

      // <snippet_features_remote>
      // Get the visual feature for analysis
      const features = ['Categories','Brands','Adult','Color','Description','Faces','Image_type','Objects','Tags'];
      const domainDetails = ['Celebrities','Landmarks'];
      // </snippet_features_remote>

      // <snippet_analyze>
      const result = (await computerVisionClient.analyzeImage(facesImageURL,{visualFeatures: features},{details: domainDetails}));

      // Detect faces
      // Print the bounding box, gender, and age from the faces.
      const faces = result.faces
      if (faces.length) {
        console.log(`${faces.length} face${faces.length == 1 ? '' : 's'} found:`);
        for (const face of faces) {
          console.log(`    Gender: ${face.gender}`.padEnd(20)
            + ` Age: ${face.age}`.padEnd(10) + `at ${formatRectFaces(face.faceRectangle)}`);
        }
      } else { console.log('No faces found.'); }

      // Formats the bounding box
      function formatRectFaces(rect) {
        return `top=${rect.top}`.padEnd(10) + `left=${rect.left}`.padEnd(10) + `bottom=${rect.top + rect.height}`.padEnd(12)
          + `right=${rect.left + rect.width}`.padEnd(10) + `(${rect.width}x${rect.height})`;
      }

      // Detect Objects
      const objects = result.objects;
      console.log();
      // Print objects bounding box and confidence
      if (objects.length) {
        console.log(`${objects.length} object${objects.length == 1 ? '' : 's'} found:`);
        for (const obj of objects) { console.log(`    ${obj.object} (${obj.confidence.toFixed(2)}) at ${formatRectObjects(obj.rectangle)}`); }
      } else { console.log('No objects found.'); }

      // Formats the bounding box
      function formatRectObjects(rect) {
        return `top=${rect.y}`.padEnd(10) + `left=${rect.x}`.padEnd(10) + `bottom=${rect.y + rect.h}`.padEnd(12)
          + `right=${rect.x + rect.w}`.padEnd(10) + `(${rect.w}x${rect.h})`;
      }
      console.log();

      // Detect tags
      const tags = result.tags;
      console.log(`Tags: ${formatTags(tags)}`);

      // Format tags for display
      function formatTags(tags) {
        return tags.map(tag => (`${tag.name} (${tag.confidence.toFixed(2)})`)).join(', ');
      }
      console.log();

      // Detect image type
      const types = result.imageType;
      console.log(`Image appears to be ${describeType(types)}`);

      function describeType(imageType) {
        if (imageType.clipArtType && imageType.clipArtType > imageType.lineDrawingType) return 'clip art';
        if (imageType.lineDrawingType && imageType.clipArtType < imageType.lineDrawingType) return 'a line drawing';
        return 'a photograph';
      }
      console.log();

      // Detect Category
      const categories = result.categories;
      console.log(`Categories: ${formatCategories(categories)}`);

      // Formats the image categories
      function formatCategories(categories) {
        categories.sort((a, b) => b.score - a.score);
        return categories.map(cat => `${cat.name} (${cat.score.toFixed(2)})`).join(', ');
      }
      console.log();

      // Detect Brands
      const brands = result.brands;

      // Print the brands found
      if (brands.length) {
        console.log(`${brands.length} brand${brands.length != 1 ? 's' : ''} found:`);
        for (const brand of brands) {
          console.log(`    ${brand.name} (${brand.confidence.toFixed(2)} confidence)`);
        }
      } else { console.log(`No brands found.`); }
      console.log();

      // Detect Colors
      const color = result.color;
      printColorScheme(color);

      // Print a detected color scheme
      function printColorScheme(colors) {
        console.log(`Image is in ${colors.isBwImg ? 'black and white' : 'color'}`);
        console.log(`Dominant colors: ${colors.dominantColors.join(', ')}`);
        console.log(`Dominant foreground color: ${colors.dominantColorForeground}`);
        console.log(`Dominant background color: ${colors.dominantColorBackground}`);
        console.log(`Suggested accent color: #${colors.accentColor}`);
      }
      console.log();

      // <snippet_landmarks>
      // Detect landmarks
      const domain = result.landmarks;

      // Prints domain-specific, recognized objects
      if (domain.length) {
        console.log(`${domain.length} ${domain.length == 1 ? 'landmark' : 'landmarks'} found:`);
        for (const obj of domain) {
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

      // <snippet_adult>
      // Detect Adult content
      // Function to confirm racy or not
      const isIt = flag => flag ? 'is' : "isn't";

      const adult = result.adult;
      console.log(`This probably ${isIt(adult.isAdultContent)} adult content (${adult.adultScore.toFixed(4)} score)`);
      console.log(`This probably ${isIt(adult.isRacyContent)} racy content (${adult.racyScore.toFixed(4)} score)`);
      // </snippet_adult>
      console.log();
      // </snippet_analyze>



      /**
       * Describe Image
       */
      // Describe URL image
      console.log('Analyzing URL image to describe...', describeURL.split('/').pop());
      const caption = (await computerVisionClient.describeImage(describeURL)).captions[0];
      console.log(`This may be ${caption.text} (${caption.confidence.toFixed(2)} confidence)`);

      // Describe local image
      console.log('\nAnalyzing local image to describe...', path.basename(describeImagePath));
      // DescribeImageInStream takes a function that returns a ReadableStream, NOT just a ReadableStream instance.
      const captionLocal = (await computerVisionClient.describeImageInStream(
        () => createReadStream(describeImagePath))).captions[0];
      console.log(`This may be ${caption.text} (${captionLocal.confidence.toFixed(2)} confidence)`);
      /**
       * END - Describe Image
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
      await computerVisionClient.generateThumbnail(100, 100, dogURL, { smartCropping: true })
        .then((thumbResponse) => {
          const destination = fs.createWriteStream("thumb.png")
          thumbResponse.readableStreamBody.pipe(destination)
          console.log('Thumbnail saved.') // Saves into root folder
        })
      console.log();
      /**
       * END - Generate Thumbnail
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
