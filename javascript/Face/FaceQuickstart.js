var msRest = require("@azure/ms-rest-js");
var Face = require("@azure/cognitiveservices-face");

/* Install the following modules: 
 * npm install @azure/ms-rest-js
 * npm install @azure/cognitiveservices-face
 */

 /**
 * This sample includes:
 *   - Detect Faces: using a single-faced image and a group image, gets the IDs of all eligible faces.
 *   - Find Similar: using the single-faced image as a query, it searches the group image for a similar face.
 *                   It will display the confidence number (0.0 - 1.0) of any faces found.
 */

/**
* Authenticate
*/
// Set the FACE_SUBSCRIPTION_KEY in your environment variables with your subscription key as a value.
let credentials = new msRest.ApiKeyCredentials({ inHeader: { 'Ocp-Apim-Subscription-Key': process.env['FACE_SUBSCRIPTION_KEY'] } });
// Set FACE_REGION in your environment variables with its endpoint region (such as 'westus') as a value.
let client = new Face.FaceClient(credentials, process.env['FACE_ENDPOINT']);

async function main() {
    /**
     * DETECT FACES 
     * Detects the faces in the source image, then in the target image.
     */
    console.log("---------------------------------")
    console.log("DETECT FACES")
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
            console.log('Face IDs found in group image:')
            // Initialize empty array of strings
            var faceIds = new Array(faces.length)
            for (let face of faces) {
                faceIds.unshift(face.faceId)
                console.log(face.faceId)
            }
            var filteredIds = faceIds.filter((id)=> {
                return id != null;
            })

            return filteredIds;
        }).catch((err) => {
            console.log(`No faces detected in group image: ${groupImageUrl}.`);
            throw err;
        });
        console.log()
    /**
     * END - DETECT FACES
     */

    /**
     * FIND SIMILAR
     * Find the similar face in the target image, then display face attributes.
     * Uses the group of detected faces array that was detected in Detect Faces.
     */
    console.log("---------------------------------")
    console.log("FIND SIMILAR")
    // The options parameter in findSimilar(faceIds, options) 
    // is a Models.FaceFindSimilarOptionalParams parameter
    // API call returns a Promise<SimilarFace[]>.
    client.face.findSimilar(faceId=singleDetectedFace, options={ faceIds : groupDetectedFaces })
        .then((similars) => {
            console.log('Similar faces found in group image:');
            for (let similar of similars) {
                // Search target image list for source face
                console.log(`Face ID: ${similar.faceId}.`);
                // The group image in this example contains a similar face that is turned, 
                // so confidence will be lower than a face looking straight ahead.
                console.log(`Confidence: ${Number(similar.confidence).toFixed(2)}.`);
            }
            console.log()
    }).catch((err) => {
        console.log(`No similar faces found in group image: ${groupImageUrl}.`);
    // END - FIND SIMILAR
}
main();
