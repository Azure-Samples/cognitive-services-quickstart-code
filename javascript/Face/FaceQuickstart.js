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
 *   - Verify: compares two images to see if they are of the same person. Detects their faces first.
 */

 /**
  * Shared variables
  * These variables are shared by more than one example below.
  */
 const IMAGE_BASE_URL = 'https://csdx.blob.core.windows.net/resources/Face/Images/'

/**
 * AUTHENTICATE
 */
// Set the FACE_SUBSCRIPTION_KEY in your environment variables with your subscription key as a value.
let credentials = new msRest.ApiKeyCredentials({ inHeader: { 'Ocp-Apim-Subscription-Key': process.env['FACE_SUBSCRIPTION_KEY'] } });
// Set FACE_REGION in your environment variables with its endpoint region (such as 'westus') as a value.
let client = new Face.FaceClient(credentials, process.env['FACE_ENDPOINT']);
/**
 * END - AUTHENTICATE
 */

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
    await client.face.findSimilar(faceId=singleDetectedFace, options={ faceIds : groupDetectedFaces })
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
            throw err;
        });
    /**
     * END - FIND SIMILAR
     */ 

    /**
     * VERIFY
     * Verify-Face-To-Face: Verify whether two faces belong to a same person or 
     * whether one face belongs to a person. 
     */
    console.log("---------------------------------")
    console.log("VERIFY")

    // Create an array to hold the target photos
    let targetImageFileNames = ['Family1-Dad1.jpg', 'Family1-Dad2.jpg']
    // Then declare your source photos, they'll be used to query the target images
    let sourceImageFileNames = ['Family1-Dad3.jpg', 'Family1-Son1.jpg']

    console.log('Detect in source images:')
    // Detect faces in the source image array, then get their IDs
    let sourceFaces = await Promise.all(sourceImageFileNames.map(async (imageName) => {
       // Returns a Promise<DetectedFace[]>
        return client.face.detectWithUrl(IMAGE_BASE_URL + imageName)
            .then((faces) => {
                console.log(`${faces.length} face detected from image ${imageName} with ID ${faces[0].faceId}`)
                let id = faces[0].faceId
                return { id } 
            }).catch((err) => {
                console.log(`No face detected in: ${sourceImageFileNames[0]}.`)
                throw err;
            })
    }))
    console.log()

    // Get objects out of sourceFaces array, 
    // then get IDs and put in new array for the API call params
    var sourceFaceIds = []
    for (var dict of sourceFaces) {
        sourceFaceIds.push(dict['id'])
    }

    console.log('Detect in target images:')
    // Detect faces in the target image array, then get their IDs
    let targetFaces = await Promise.all(targetImageFileNames.map(async (imageName) => {
        // Returns a Promise<DetectedFace[]>
            return client.face.detectWithUrl(IMAGE_BASE_URL + imageName)
                .then((faces) => {
                    console.log(`${faces.length} face detected from image ${imageName} with ID ${faces[0].faceId}`)
                    let id = faces[0].faceId
                    return { id } 
                }).catch((err) => {
                    console.log(`No face detected in: ${targetImageFileNames[0]}.`)
                    throw err;
                })
        }))

    // Get objects out of targetFaces array, 
    // then get IDs and put in new array for the API call params
    var targetFaceIds = []
    for (var dict of targetFaces) {
        targetFaceIds.push(dict['id'])
    }
    console.log()

    // Compare all source images with a target image
    // (target imnages are of same person, so we compare only 1 to both source images)
    await Promise.all(sourceFaceIds.map(async (sourceId) => {
        // Returns a Promise<Verify Result> 
        return client.face.verifyFaceToFace(faceId1=sourceId, faceId2=targetFaceIds[0])
            .then((result) => {
                console.log(`Are image IDs ${sourceId} and ${targetFaceIds[0]} identical? ${result.isIdentical}`)
                // Confidence score 0.0-1.0: 
                // lower means less of a match, higher is more of a match
                console.log(`with confidence: ${result.confidence}`)
            }).catch((err) => {
                throw err;
            })
    }))
    /**
     * END - VERIFY
     */ 
}
main();
