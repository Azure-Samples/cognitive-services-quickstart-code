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
 *  - Set your subscription key and endpoint into your environment variables COMPUTER_VISION_ENDPOINT and COMPUTER_VISION_SUBSCRIPTION_KEY.
 *       An example of COMPUTER_VISION_ENDPOINT will look like:         https://westus2.api.cognitive.microsoft.com
 *       An example of COMPUTER_VISION_SUBSCRIPTION_KEY will look like: 0123456789abcdef0123456789abcdef
 *  - The DESCRIBE IMAGE example uses a local image celebrities.jpg, which will be downloaded on demand.
 *  - The READ (the API for performing Optical Character Recognition or doing text retrieval from PDF) example uses local images and a PDF files, which will be downloaded on demand.
 * 
 * How to run:
 *  - This quickstart can be run all at once (node ComputerVisionQuickstart.js from the command line) or used to copy/paste sections as needed. 
 *    If sections are extracted, make sure to copy/paste the authenticate section too, as each example relies on it.
 *
 * Resources:
 *  - Node SDK: https://docs.microsoft.com/en-us/javascript/api/azure-cognitiveservices-computervision/?view=azure-node-latest
 *  - Documentation: https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/
 *  - API v3.0: https://westcentralus.dev.cognitive.microsoft.com/docs/services/computer-vision-v3-ga/operations/5d986960601faab4bf452005
 * 
 * Examples included in this quickstart:
 * Authenticate, Describe Image, Detect Faces, Detect Objects, Detect Tags, Detect Type, 
 * Detect Category, Detect Brand, Detect Color Scheme, Detect Domain-specific Content, Detect Adult Content
 * Generate Thumbnail, Recognize Printed & Handwritten Text using Read API.
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

      const describeImagePath = __dirname + '\\celebrities.jpg';
      try {
        await downloadFilesToLocal(describeURL, describeImagePath);
      } catch {
        console.log('>>> Download sample file failed. Sample cannot continue');
        process.exit(1);
      }

      // <snippet_describe>
      // Analyze URL image
      console.log('Analyzing URL image to describe...', describeURL.split('/').pop());
      const caption = (await computerVisionClient.describeImage(describeURL)).captions[0];
      console.log(`This may be ${caption.text} (${caption.confidence.toFixed(2)} confidence)`);
      // </snippet_describe>

      // Analyze local image
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
      const faces = (await computerVisionClient.analyzeImage(facesImageURL, { visualFeatures: ['Faces'] })).faces;

      // Print the bounding box, gender, and age from the faces.
      if (faces.length) {
        console.log(`${faces.length} face${faces.length == 1 ? '' : 's'} found:`);
        for (const face of faces) {
          console.log(`    Gender: ${face.gender}`.padEnd(20)
            + ` Age: ${face.age}`.padEnd(10) + `at ${formatRectFaces(face.faceRectangle)}`);
        }
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
      const objects = (await computerVisionClient.analyzeImage(objectURL, { visualFeatures: ['Objects'] })).objects;
      console.log();

      // Print objects bounding box and confidence
      if (objects.length) {
        console.log(`${objects.length} object${objects.length == 1 ? '' : 's'} found:`);
        for (const obj of objects) { console.log(`    ${obj.object} (${obj.confidence.toFixed(2)}) at ${formatRectObjects(obj.rectangle)}`); }
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
      const tags = (await computerVisionClient.analyzeImage(tagsURL, { visualFeatures: ['Tags'] })).tags;
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
      const types = (await computerVisionClient.analyzeImage(typeURLImage, { visualFeatures: ['ImageType'] })).imageType;
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
      const categories = (await computerVisionClient.analyzeImage(categoryURLImage)).categories;
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
      const brands = (await computerVisionClient.analyzeImage(brandURLImage, { visualFeatures: ['Brands'] })).brands;

      // Print the brands found
      if (brands.length) {
        console.log(`${brands.length} brand${brands.length != 1 ? 's' : ''} found:`);
        for (const brand of brands) {
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
      const color = (await computerVisionClient.analyzeImage(colorURLImage, { visualFeatures: ['Color'] })).color;
      printColorScheme(color);
      // </snippet_colors>

      // <snippet_colors_print>
      // Print a detected color scheme
      function printColorScheme(colors) {
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
      const domain = (await computerVisionClient.analyzeImageByDomain('landmarks', domainURLImage)).result.landmarks;

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
      const adult = (await computerVisionClient.analyzeImage(adultURLImage, {
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
        *READ API
        *
        * This example recognizes both handwritten and printed text, and can handle image files (.jpg/.png/.bmp) and multi-page files (.pdf and .tiff)
        * Please see REST API reference for more information:
        * Read: https://westcentralus.dev.cognitive.microsoft.com/docs/services/computer-vision-v3-ga/operations/5d986960601faab4bf452005
        * Get Result Result: https://westcentralus.dev.cognitive.microsoft.com/docs/services/computer-vision-v3-ga/operations/5d9869604be85dee480c8750
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
       * READ PRINTED & HANDWRITTEN TEXT
       * Recognizes text from images using OCR (optical character recognition).
       * Recognition is shown for both printed and handwritten text.
       * Read 3.0 supports the following language: en (English), de (German), es (Spanish), fr (French), it (Italian), nl (Dutch) and pt (Portuguese).
       */
      console.log('-------------------------------------------------');
      console.log('READ PRINTED, HANDWRITTEN TEXT AND PDF');
      console.log();

      // <snippet_read_images>
      // URL images containing printed and/or handwritten text. 
      // The URL can point to image files (.jpg/.png/.bmp) or multi-page files (.pdf, .tiff).
      const printedTextSampleURL = 'https://moderatorsampleimages.blob.core.windows.net/samples/sample2.jpg';
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
