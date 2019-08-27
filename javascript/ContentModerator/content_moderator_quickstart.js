'use strict';

const os = require("os");
const fs = require('fs');
const uuidv4 = require('uuid/v4');

const auth = require("@azure/ms-rest-js");
const ContentModerator = require("@azure/cognitiveservices-contentmoderator")

/**
 * Content Moderator quickstart
 * 
 * This quickstart contains these examples:
 *  - Image moderation
 *  - Text moderation
 *  - Human reviews
 * 
 * Prerequisites
 *  - A Content Moderator subscription at https://ms.portal.azure.com.
 *  - An account at the Content Moderator website (for Human Reviews example only): 
 *    https://contentmoderator.cognitive.microsoft.com
 *  - Install the Content Moderator library in the command line:
 *      npm install @azure/cognitiveservices-contentmoderator
 *  - Set your environment variables (in Authenticate section) with your subscription keys and endpoint.
 *    The endpoint is in this format: https://{REGION OR CUSTOM NAME}.api.cognitive.microsoft.com/
 *  - Download/add the text_file.txt file in this quickstart's root folder to your local root folder.
 *  - Install the content moderator library from the command line with:
 *      npm install @azure/cognitiveservices-contentmoderator
 *  - Set your environment variables (in Authenticate section) with your subscription keys and endpoint.
 *    The endpoint is in this format: https://{REGION OR CUSTOM NAME}.api.cognitive.microsoft.com/
 *  - Download and add (or create a local one) the text_file.txt file in this quickstart's root folder to your local root folder.
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

 // IMAGE MODERATION variables
const IMAGE_MODERATION_URLS = ['https://moderatorsampleimages.blob.core.windows.net/samples/sample2.jpg', // text in image
'https://moderatorsampleimages.blob.core.windows.net/samples/sample5.png'] // faces image

// TEXT MODERATION variables
const TEXT_MODERATION_FILE = 'text_file.txt'
const outputFile = 'text_moderation_output.txt'; // gets created at runtime

// Human Reviews - Images variables
// Set your callback endpoint in your environment variables with your own region. 
// For example: 'https://westus.api.cognitive.microsoft.com/contentmoderator/review/v1.0'.
let callbackEndpoint = process.env['CONTENT_MODERATOR_REVIEWS_ENDPOINT']
// The team name you used when you created an account at the Content Moderator website
const TEAM_NAME = process.env['CONTENT_MODERATOR_TEAM_NAME']
// Time to wait for processing the review (in milliseconds)
const TIME_TO_SLEEP = 45000;

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

/**
* AUTHENTICATE - HUMAN REVIEWS
* To create a human review, a special client is needed due to a unique subscription key.
* Get the key from the Content Moderator website: go to the gear symbol (settings) --> Credentials --> Ocp-Apim-Subscription-Key
*/
// Set CONTENT_MODERATOR_SUBSCRIPTION_KEY and CONTENT_MODERATOR_ENDPOINT in your environment variables.
let credentialsReviews = new auth.ApiKeyCredentials({ inHeader: { 'Ocp-Apim-Subscription-Key': process.env['CONTENT_MODERATOR_REVIEWS_KEY'] } });
let clientReviews = new ContentModerator.ContentModeratorClient(process.env['CONTENT_MODERATOR_ENDPOINT'], credentialsReviews);
/**
* END - Authenticate Human Reviews
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
    let imageBodyModel = { dataRepresentation: 'URL', value: IMAGE_MODERATION_URLS[0]} 
    let imageRacyResult = await client.imageModeration.evaluateUrlInput('application/json', imageBodyModel)
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

     /**
     * HUMAN REVIEW - IMAGES
     * This example will programmatically create an image review that gets sent to the Content Moderator website.
     * A human manually reviews it there, then the result is sent back to the code.
     */
    console.log('HUMAN REVIEW')
    console.log()
    // Create review
    console.log(`Creating the review for image ${IMAGE_MODERATION_URLS[1].substring(IMAGE_MODERATION_URLS[1].lastIndexOf('/')+1)}...`)
    console.log();

    // Create the body of the request
    let reviewBody = [ {
        callbackEndpoint: callbackEndpoint, 
        content: IMAGE_MODERATION_URLS[1], 
        contentId: String(uuidv4()),
        // 'sc' is for score (0.0 to 1.0), which returns values for 'a' (adult) or 'r' (racy). 
        // Setting value: 'true' means if the human reviewer selects 'a' or 'r', it comes back as "true" in the response.
        // If they are not selected, "false" is returned for 'a' or 'r' in the response.
        metadata: [ { key: "sc", value: "true"} ], 
        type: "Image" // "Text" is also possible, if reviewing text content.
    } ]

    // The API call to create a human review. A POST request to the callback endpoint.
    // createReviews() returns a Promise<Models.ReviewsCreateReviewsResponse> (can contain many reviews). 
    let reviewId;
    try {
        await clientReviews.reviews.createReviews("Image", TEAM_NAME, reviewBody)
            .then((result) => {
                //console.log(result.json())
                reviewId = result[0]; // ID of first review
                // Get review details before human review, to compare with details after review.
                console.log("Getting review details before the human review...");
                // Get a specific review's details
                // Returns a Promise<Models.ReviewsGetReviewResponse> 
                return clientReviews.reviews.getReview(TEAM_NAME, reviewId);
            })
            .then((beforeDetails) => { 
                // Print properties of the details you want. From Review class.
                console.log("Content: " + beforeDetails.content);
                console.log("Content ID: " + beforeDetails.contentId); 

                // Get the review details, before the human review
                beforeDetails.metadata.forEach((pair) => {
                    console.log("Metadata: " + JSON.stringify(pair));
                })
                beforeDetails.reviewerResultTags.forEach((pair) => {
                    if (typeof pair == 'undefined') {
                        console.log("Reviewer Results: none");
                    } else {
                        console.log("Reviewer Results: " + JSON.stringify(pair));
                    }
                }) 
            })
        }
        catch(err) {
            console.log(err);
        };
        console.log();

    // User should go review the item on the website, then press enter when finished reviewing.
    console.log('Perform manual reviews on the Content Moderator review site, then return to this console.');
    console.log();

    // When finished doing the human review, wait for server to process it before getting it again.
    console.log("Waiting " + TIME_TO_SLEEP / 1000 + " seconds for reviewer to complete review...");
    console.log();

    setTimeout(() => {
        console.log("Getting review details after human review...");
        try {
            clientReviews.reviews.getReview(TEAM_NAME, reviewId)
                .then ((afterDetails) => {
                    console.log("Content: " + afterDetails.content);
                    console.log("Content ID: " + afterDetails.contentId);

                    // Get the review details after the human review
                    afterDetails.metadata.forEach((pair) => {
                        console.log("Metadata: " + JSON.stringify(pair));
                    })   
                    afterDetails.reviewerResultTags.forEach((pair) => {
                        if (typeof pair == 'undefined') {
                            console.log("Reviewer Results: none\nWas the human review completed?");
                        } else {
                            console.log("Reviewer Results: " + JSON.stringify(pair));
                        }
                    }) 
                })
        } catch (err) {
            console.log(err.message);
        }
    }, TIME_TO_SLEEP);
    /**
     * END - Human Review - Images
     */
}
main();
