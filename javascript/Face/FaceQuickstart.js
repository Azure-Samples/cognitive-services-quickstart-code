var msRest = require("@azure/ms-rest-js");
var Face = require("@azure/cognitiveservices-face");

/* Install the following modules: 
 * npm install @azure/ms-rest-js
 * npm install @azure/cognitiveservices-face
 */

 /**
 * Find Similar - Face API
 * This sample will find a similar face from one image URL in another image URL.
 * The steps in this sample include: 
 *     - authenticate
 *     - detect a face in each image
 *     - find a similar face in the group image, using the single face image as a query
 *     - display the confidence percent (0.0 - 1.0) that the query face is found in the group image
 */

/**
* Authenticate
*/
// Set the FACE_SUBSCRIPTION_KEY in your environment variables with your subscription key as a value.
let credentials = new msRest.ApiKeyCredentials({ inHeader: { 'Ocp-Apim-Subscription-Key': process.env['FACE_SUBSCRIPTION_KEY'] } });
// Set FACE_REGION in your environment variables with its endpoint region (such as 'westus') as a value.
let client = new Face.FaceClient(credentials, `https://${process.env['FACE_REGION']}.api.cognitive.microsoft.com/`);

async function main() {
    /**
    * Detect face in the source image, then in the target image
    */
    // An image with only one face
    let singleFaceImageUrl = 'https://www.biography.com/.image/t_share/MTQ1MzAyNzYzOTgxNTE0NTEz/john-f-kennedy---mini-biography.jpg';
    // Detect a single face in an image. Returns a Promise<DetectedFace[]>.
    // NOTE: FaceDetectWithUrlOptionalParams.returnFaceId default value is true. See:
    // https://github.com/Azure/azure-sdk-for-js/blob/master/sdk/cognitiveservices/cognitiveservices-face/src/models/index.ts#L891
    let singleDetectedFace = await client.face.detectWithUrl(singleFaceImageUrl)
        .then((faces) => {
            console.log(`Face ID found in single face image: ${faces[0].faceId}.`);
            return faces[0].faceId;
        }).catch((err) => {
            console.log('No faces detected in single face image:' + singleFaceImageUrl);
            throw err;
        });
    
    // An image with several faces
    let groupImageUrl = 'http://www.historyplace.com/kennedy/president-family-portrait-closeup.jpg';
    // Detect the faces in a group image. Returns a Promise<DetectedFace[]>.
    let groupDetectedFaces = await client.face.detectWithUrl(groupImageUrl)
        .then((faces) => {
            console.log(`Face ID found in group image: ${faces[0].faceId}`);
            return faces[0].faceId;
        }).catch((err) => {
            console.log(`No faces detected in group image: ${groupImageUrl}.`);
            throw err;
        });

    /**
    * Find the similar face in the target image, then display face attributes
    */
    // Returns a Promise<SimilarFace[]>.
    client.face.findSimilar(singleDetectedFace, { faceIds: [groupDetectedFaces] }).then((similars) => {
        console.log('Similar faces found in group image:');
        for (let similar of similars) {
            // Search target image list for source face
            console.log(`Face ID: ${similar.faceId}.`);
            console.log(`Confidence: ${similar.confidence}.`);
        }
    }).catch((err) => {
        console.log(`No similar faces found in group image: ${groupImageUrl}.`);
        throw err;
    });
}
main();
// END - Find a similar face in an image URL