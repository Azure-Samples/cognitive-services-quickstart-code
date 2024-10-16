'use strict';

var msRest = require("@azure/ms-rest-js");
var Face = require("@azure/cognitiveservices-face");

const FileSet = require('file-set');
const createReadStream = require('fs').createReadStream
const uuidV4 = require('uuid/v4');

/** 
 * Install the following modules: 
 * npm install @azure/ms-rest-js
 * npm install @azure/cognitiveservices-face
 * 
 * Collect local images:
 * This quickstart uses a variety of both URL images and local ones. Download all the local ones from here:
 * https://github.com/Azure-Samples/cognitive-services-sample-data-files/tree/master/Face/images
 * Put these images in your root folder.
 */

/**
* This sample includes:
*   - Detect Faces: using a single-faced image and a group image, gets the IDs of all eligible faces.
*   - Find Similar: using the single-faced image as a query, it searches the group image for a similar face.
*   - Verify: compares two images to see if they are of the same person. Detects their faces first.
*   - Person Group: creates a person group and uses it to identify faces in other images. 
*   - Identify: identify a face in a photo from a select person group.
*   - Snapshot: move a person group or facelist from one region (or Azure subscription) to another.
*/

/**
 * Shared variables
 * These variables are shared by more than one example below.
 */
// An image with only one face
let singleFaceImageUrl = 'https://www.biography.com/.image/t_share/MTQ1MzAyNzYzOTgxNTE0NTEz/john-f-kennedy---mini-biography.jpg';
// An image with several faces
let groupImageUrl = 'http://www.historyplace.com/kennedy/president-family-portrait-closeup.jpg';
const IMAGE_BASE_URL = 'https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/'
// Person Group ID must be lower case, alphanumeric, with '-' and/or '_'.
const PERSON_GROUP_ID = 'my-unique-person-group'

/**
 * AUTHENTICATE
 * Used for all examples.
 */
let key = 'PASTE_YOUR_FACE_SUBSCRIPTION_KEY_HERE';
let endpoint = 'PASTE_YOUR_FACE_ENDPOINT_HERE';

let credentials = new msRest.ApiKeyCredentials({ inHeader: { 'Ocp-Apim-Subscription-Key': key } });
let client = new Face.FaceClient(credentials, endpoint);
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

    // Detect a single face in an image. Returns a Promise<DetectedFace[]>.
    // NOTE: FaceDetectWithUrlOptionalParams.returnFaceId default value is true. See:
    // https://github.com/Azure/azure-sdk-for-js/blob/master/sdk/cognitiveservices/cognitiveservices-face/src/models/index.ts#L891
    let singleDetectedFace = await client.face.detectWithUrl(singleFaceImageUrl)
        .then((faces) => {
            console.log(`Face ID found in single face image: \n${faces[0].faceId}`)
            return faces[0].faceId;
        }).catch((err) => {
            console.log('No faces detected in single face image:' + singleFaceImageUrl)
            throw err
        })
    console.log()

    // Detect the faces in a group image. API call returns a Promise<DetectedFace[]>.
    let groupDetectedFaces = await client.face.detectWithUrl(groupImageUrl)
        .then((faces) => {
            console.log('Face IDs found in group image:')
            // Initialize empty array of strings
            var faceIds = new Array(faces.length)
            for (let face of faces) {
                faceIds.unshift(face.faceId)
                console.log(face.faceId)
            }
            var filteredIds = faceIds.filter((id) => {
                return id != null
            })
            return filteredIds
        }).catch((err) => {
            console.log(`No faces detected in group image: ${groupImageUrl}.`)
            throw err
        })
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
    // is a Models.FaceFindSimilarOptionalParams parameter (in index.ts)
    // API call returns a Promise<SimilarFace[]>.
    await client.face.findSimilar(singleDetectedFace, { faceIds: groupDetectedFaces })
        .then((similars) => {
            console.log('Similar faces found in group image:')
            for (let similar of similars) {
                // Search target image list for source face
                console.log(`Face ID: ${similar.faceId}.`)
                // The group image in this example contains a similar face that is turned, 
                // so confidence will be lower than a face looking straight ahead.
                console.log(`Confidence: ${Number(similar.confidence).toFixed(2) * 100}%`)
            }
        }).catch((err) => {
            console.log(`No similar faces found in group image: ${groupImageUrl}.`)
            throw err
        });
    console.log()
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

    console.log('Detect faces in source images:')
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

    console.log('Detect faces in target images:')
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
        return client.face.verifyFaceToFace(sourceId, targetFaceIds[0])
            .then((result) => {
                console.log(`Are image IDs \n${sourceId} and \n${targetFaceIds[0]} identical? ${result.isIdentical}`)
                // Confidence score 0.0-1.0: 
                // lower means less of a match, higher is more of a match
                console.log(`with ${Math.round(result.confidence * 100)}% confidence.`)
                console.log()
            }).catch((err) => {
                throw err;
            })
    }))
    /**
     * END - VERIFY
     */

    /**
     * PERSON GROUP
     * This example creates a person group from several images of different people.
     * The group can then be used in other Face API calls to identify, verify, or do snapshot operations.
     */
    console.log("---------------------------------")
    console.log("PERSON GROUP")
    // Create empty Person Group. Returns a Promise<RestResponse>. 
    await client.personGroup.create(PERSON_GROUP_ID, { 'name': PERSON_GROUP_ID })
        .then(() => {
        }).catch((err) => {
            console.log(err)
            throw err
        })
    console.log('Person group ID: ' + PERSON_GROUP_ID + ' was created.')

    // Define woman friend, add them to person group, returns a Promise<Person>
    let woman = await client.personGroupPerson.create(PERSON_GROUP_ID, { "name": "Woman" })
        .then((wFace) => {
            console.log('Person ' + wFace.personId + ' was created.')
            return wFace
        }).catch((err) => {
            throw err
        })
    // Define man friend, add them to person group, returns a Promise<Person>
    let man = await client.personGroupPerson.create(PERSON_GROUP_ID, { "name": "Man" })
        .then((mFace) => {
            console.log('Person ' + mFace.personId + ' was created.')
            return mFace
        }).catch((err) => {
            throw err
        })
    // Define child friend, add them to person group, returns a Promise<Person>
    let child = await client.personGroupPerson.create(PERSON_GROUP_ID, { "name": "Child" })
        .then((chFace) => {
            console.log('Person ' + chFace.personId + ' was created.')
            return chFace
        }).catch((err) => {
            throw err
        })
    console.log()

    // Detect faces and register to correct person
    // Make array of jpeg images of friends in working directory
    let womanImages = new FileSet("w*.jpg").files
    let manImages = new FileSet("m*.jpg").files
    let childImages = new FileSet("c*.jpg").files

    // Add images to the person group person "woman"
    for (let wImage of womanImages) {
        // Returns a Promise<PersistedFace>
        await client.personGroupPerson.addFaceFromStream(PERSON_GROUP_ID,
            woman.personId, () => createReadStream(wImage))
            .then((face) => {
                console.log('ID ' + face.persistedFaceId + ' was added to a person group person called Woman.')
            })
    }

    // Add images to the person group person "man"
    for (let mImage of manImages) {
        // Returns a Promise<PersistedFace>
        await client.personGroupPerson.addFaceFromStream(PERSON_GROUP_ID,
            man.personId, () => createReadStream(mImage))
            .then((face) => {
                console.log('ID ' + face.persistedFaceId + ' was added to a person group person called Man.')
            })
    }

    // Add images to the person group person "child"
    for (let chImage of childImages) {
        // Returns a Promise<PersistedFace>
        await client.personGroupPerson.addFaceFromStream(PERSON_GROUP_ID,
            child.personId, () => createReadStream(chImage))
            .then((face) => {
                console.log('ID ' + face.persistedFaceId + ' was added to a person group person called Child.')
            })
    }
    console.log()

    // After all Persons added to person group, train the person group.
    console.log('Training the person group...')
    await client.personGroup.train(PERSON_GROUP_ID)

    while (true) {
        let status = await client.personGroup.getTrainingStatus(PERSON_GROUP_ID)
            .then((trainingStatusResponse) => {
                return trainingStatusResponse
            })
        console.log('Training status: ' + status.status)
        if (status.status == 'failed') {
            console.log('Training the person group has failed.')
            break;
        }
        if (status.status == 'succeeded') {
            console.log('Training the person group was a success.')
            break;
        }

        await sleep(1000)
    }
    console.log()
    /**
     * END - PERSON GROUP
     */

    /**
     * IDENTIFY
     * Identifies a face from a defined person group.
     * It uses the detected faces API call from the DETECT FACES example.
     */
    console.log("---------------------------------")
    console.log("IDENTIFY")
    // Detect the faces from the group photo
    let groupPhoto = 'test-image-person-group.jpg'
    // API call returns a Promise<DetectedFace[]>, then add their IDs to an array.
    var faceIds = []
    let groupDetectedFacesIdentify =
        await client.face.detectWithStream(() => createReadStream(groupPhoto))
            .then((faces) => {
                console.log('Face IDs found in target image:')
                // Initialize empty array of strings
                faceIds = new Array(faces.length)
                for (let face of faces) {
                    faceIds.unshift(face.faceId)
                    console.log(face.faceId)
                }
                var filteredIds = faceIds.filter((id) => {
                    return id != null
                })
                return filteredIds
            }).catch((err) => {
                console.log(`No faces detected in target image: ${groupImageUrl}.`)
                throw err
            })
    console.log()

    // Identify the faces, API call returns a Promise<IdentifyResult[]>. 
    // The optional parameter is from Models.FaceIdentifyOptionalParams
    // Matches will be drawn from the Person objects created in the Person Group example.
    await client.face.identify(groupDetectedFacesIdentify, { "personGroupId": PERSON_GROUP_ID })
        // API call returns a Promise<IdentifyResult[]>
        .then((identifyResults) => {
            for (let result of identifyResults) {
                // result.candidates is an IdentifyCandidate[]
                for (let candidate of result.candidates) {
                    console.log('Person ' + candidate.personId
                        + ' is identified in the photo with '
                        + Math.round(candidate.confidence * 100) + '% confidence.')
                }
            }
        }).catch((err) => {
            console.log(err)
            throw err
        })
    console.log()
    /**
    * END - IDENTIFY
    */

    // Helper method to sleep/wait
    function sleep(ms) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }

    /**
     * SNAPSHOT
     * This example transfers a person group from one region to another region.
     * You can also transfer facelists and large person groups or large facelists.
     * You can also transfer it to another subscription(change the target subscription key).
     * It uses the same client as the above examples for its source client.
     */
    console.log("---------------------------------")
    console.log("SNAPSHOT")
    // Authenticate 2, need an additional (target) client for this example.
    let sourceClient = client // renaming to be relevent to this example

    // Your general Azure subscription ID is needed for this example only.
    // Since we are moving a person group from one region to another in the same
    // subscription, our source ID and target ID are the same.
    // Add your subscription ID (not to be confused with a subscription key) to your
    // environment variables. Find in the Azure portal on your Face overview page.
    let subscriptionID = process.env['AZURE_SUBSCRIPTION_ID']

    // Add a 2nd Face subscription key to your environment variables. Make sure the
    // 2nd set of key and endpoint is in the region or subscription you want to
    // transfer to.
    let credentials2 = new msRest.ApiKeyCredentials({
        inHeader: { 'Ocp-Apim-Subscription-Key': process.env['FACE_SUBSCRIPTION_KEY2'] }
    });
    let targetClient = new Face.FaceClient(credentials2, process.env['FACE_ENDPOINT2']);

    // Take the snapshot from your source region or Face subscription
    console.log('Taking the snapshot.')
    let takeOperationId = null
    await sourceClient.snapshot.take('PersonGroup', PERSON_GROUP_ID, [subscriptionID])
        .then((takeResponse) => {
            // Get the operation ID at the end of the response URL operation location
            takeOperationId = takeResponse.operationLocation.substring(takeResponse.operationLocation.lastIndexOf('/') + 1)
            console.log('Operation ID (take): ' + takeOperationId)
        }).catch((err) => {
            console.log(err)
            throw err
        })

    // With the operation ID, wait for the 'take' operation to complete
    let snapshotID = null
    // Create an ID for the target region or subscription (can be any alphanumeric, '-', or '_' character)
    let personGroupIdTarget = uuidV4() // generates a random uuid

    console.log()
    console.log('Wait for the snapshot (take) to complete...')
    while (true) {
        // Get the status of the 'take' operation, the OpeationStatus interface is returned
        let status = await sourceClient.snapshot.getOperationStatus(takeOperationId)
            .then((takeOperationStatus) => {
                return takeOperationStatus
            })
        console.log('Operation status (take): ' + status.status)
        if (status.status == "failed") {
            console.log('The taking of the snapshot has failed.')
            break;
        }
        if (status.status == "succeeded") {
            // From successful result of the take operation status, get the snapshot ID
            snapshotID = status.resourceLocation.substring(status.resourceLocation.lastIndexOf('/') + 1)
            console.log('Snapshot ID: ' + snapshotID)
            break;
        }
        // Wait one second
        await sleep(1000)
    }
    console.log()

    let applyOperationId = null
    // With the snapshot ID, call the 'apply' function on your target client
    console.log('Applying the snapshot.')
    await targetClient.snapshot.apply(snapshotID, personGroupIdTarget)
        .then((applyResponse) => {
            // Get the operation ID from the 'apply' response
            applyOperationId = applyResponse.operationLocation.substring(applyResponse.operationLocation.lastIndexOf('/') + 1)
            console.log('Operation ID (apply): ' + applyOperationId)
        }).catch((err) => {
            console.log(err)
            throw err
        })

    // Wait for the 'apply' function to complete
    console.log()
    console.log('Wait for the snapshot (apply) to complete...')
    while (true) {
        // Get the status of the 'apply' operation, using its operation ID
        let status = await sourceClient.snapshot.getOperationStatus(applyOperationId)
            .then((applyOperationStatus) => {
                return applyOperationStatus
            })
        console.log('Operation status (apply): ' + status.status)
        if (status.status == 'failed') {
            console.log('The applying of the snapshot has failed.')
            break;
        }
        if (status.status == 'succeeded') {
            break;
        }
        // Wait one second
        await sleep(1000)
    }
    console.log()
    console.log('Snapshot transfer is complete.')
    console.log()

    console.log("---------------------------------")
    console.log("DELETE")
    // After the quickstart, delete the person group and facelists from Azure account(s)
    client.personGroup.deleteMethod(PERSON_GROUP_ID)
    console.log('The source region/subscription person group has been deleted.')
    targetClient.personGroup.deleteMethod(personGroupIdTarget)
    console.log('The target region/subscription person group has been deleted.')
    console.log()
    console.log("---------------------------------")
    console.log("End of quickstart.")
}
main();
