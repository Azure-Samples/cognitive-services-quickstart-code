'use strict';

const os = require("os");
const fs = require('fs');

const auth = require("@azure/ms-rest-js");
const ContentModerator = require("@azure/cognitiveservices-contentmoderator")

/**
 * Content Moderator quickstart
 * 
 * This quickstart contains these examples:
 *  - Image moderation
 *  - Text moderation
 * 
 * Prerequisites
 *  - A Content Moderator subscription at https://ms.portal.azure.com.
 *  - Install the content moderator and ms-rest-js libraries in the command line with:
 *      npm install @azure/ms-rest-js
 *      npm install @azure/cognitiveservices-contentmoderator
 *  - Set your environment variables (in Authenticate section) with your subscription keys and endpoint.
 *    The endpoint is in this format: https://{YOUR_REGION}.api.cognitive.microsoft.com/
 *  - Download and add the content_moderator_text_moderation.txt file in this quickstart's root folder to your local root folder.
 * 
 * How to run:
 *  - From the command line:
 *      node content_moderater_quickstart.js
 * 
 * References:
 *  - Content Moderator documentation: https://docs.microsoft.com/en-us/azure/cognitive-services/content-moderator/
 *  - Node SDK: https://docs.microsoft.com/en-us/javascript/api/azure-cognitiveservices-contentmoderator/?view=azure-node-latest
 *  - Content Moderator API: https://docs.microsoft.com/en-us/azure/cognitive-services/content-moderator/api-reference
 *  - npm library: https://www.npmjs.com/package/@azure/cognitiveservices-contentmoderator
 */

 // Image moderation variables
const IMAGE_MODERATION_URLS = ['https://moderatorsampleimages.blob.core.windows.net/samples/sample2.jpg', // text in image
'https://moderatorsampleimages.blob.core.windows.net/samples/sample5.png'] // faces image

// Text moderation variables
const TEXT_MODERATION_FILE = 'text_file.txt'
const outputFile = 'text_moderation_output.txt';

/**
 * AUTHENTICATE
 * Using your subscription key and endpoint, a client is created that is used call the API.
 */
// Set CONTENT_MODERATOR_SUBSCRIPTION_KEY and CONTENT_MODERATOR_ENDPOINT in your environment variables.
let credentials = new auth.ApiKeyCredentials({ inHeader: { 'Ocp-Apim-Subscription-Key': process.env['CONTENT_MODERATOR_SUBSCRIPTION_KEY'] } });
let client = new ContentModerator.ContentModeratorClient(process.env['CONTENT_MODERATOR_ENDPOINT'], credentials);
/**
 * END - Authenticate
 */

async function main() {

    console.log('\n##############################################################################\n')

    /**
     * IMAGE MODERATION
     * This example will moderate URL images.
     */
    console.log('IMAGE MODERATION')
    console.log()

    // Moderate racy content
    // Image moderation, using image at [0]
    console.log(`Evaluating the image ${IMAGE_MODERATION_URLS[0].substring(IMAGE_MODERATION_URLS[0].lastIndexOf('/')+1)} for adult and/or racy content...`)
    // Create the body model of your image URLs. Used for detecting all types (racy, text, and faces)
    let imageBodyModel = { dataRepresentation: "URL", value: IMAGE_MODERATION_URLS[0] }
    // Detect racy content from image
    let imageRacyResult = await client.imageModeration.evaluateUrlInput("application/json", imageBodyModel)

    // Print result
    console.log(JSON.stringify(imageRacyResult, null, 2));
    console.log();

    // Moderate text content
    console.log(`Evaluating the image ${IMAGE_MODERATION_URLS[0].substring(IMAGE_MODERATION_URLS[0].lastIndexOf('/')+1)} for text content...`)
    // Detect and extract text from image
    let imageTextResult = await client.imageModeration.oCRUrlInput('eng', 'application/json', imageBodyModel);
    console.log(JSON.stringify(imageTextResult, null, 2));
    console.log();

    // Moderate faces content
    console.log(`Evaluating the image ${IMAGE_MODERATION_URLS[1].substring(IMAGE_MODERATION_URLS[1].lastIndexOf('/')+1)} for faces content...`)
    let imageFacesResult = await client.imageModeration.findFacesUrlInput('application/json', imageBodyModel);
    console.log(JSON.stringify(imageFacesResult, null, 2));
    /**
     * END - Image Moderation
     */
    
    console.log('\n##############################################################################\n')

    /**
     * TEXT MODERATION
     * Moderates text from a local .txt file.
     */
    console.log('TEXT MODERATION')
    console.log()
    console.log("Evaluating a text file and screening for profanity...");
    console.log()
    let data = fs.readFileSync(TEXT_MODERATION_FILE , {encoding: "utf8"});
    data = data.replace(os.EOL, " ");
    let writeStream = fs.createWriteStream(outputFile);
    
    // Screen the input text: check for profanity, do autocorrect text, 
    // and check for personally identifying information (PII)
    writeStream.write("Normalize text and autocorrect typos.\n");
    let screenResult = await client.textModeration.screenText("text/plain", data, { autocorrect: true, pII: true } );
    writeStream.write(JSON.stringify(screenResult, null, 2));
    writeStream.close();

    console.log()
    console.log(`Check your output file ${outputFile} for results.`);
    console.log()
    /**
     * END - Text Moderation
     */

    console.log('\n##############################################################################\n')
}
main();
