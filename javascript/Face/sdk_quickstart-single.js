// <snippet_single>
const { randomUUID } = require("crypto");

const { AzureKeyCredential } = require("@azure/core-auth");

const createFaceClient = require("@azure-rest/ai-vision-face").default,
  { FaceAttributeTypeRecognition04, getLongRunningPoller } = require("@azure-rest/ai-vision-face");

/**
 * NOTE This sample might not work with the free tier of the Face service because it might exceed the rate limits.
 * If that happens, try inserting calls to sleep() between calls to the Face service.
 */
const sleep = (ms) => new Promise((resolve) => setTimeout(resolve, ms));

const main = async () => {
  const endpoint = process.env["FACE_ENDPOINT"] ?? "<endpoint>";
  const apikey = process.env["FACE_APIKEY"] ?? "<apikey>";
  const credential = new AzureKeyCredential(apikey);
  const client = createFaceClient(endpoint, credential);

  const imageBaseUrl =
    "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/";
  const personGroupId = randomUUID();

  console.log("========IDENTIFY FACES========");
  console.log();

  // Create a dictionary for all your images, grouping similar ones under the same key.
  const personDictionary = {
    "Family1-Dad": ["Family1-Dad1.jpg", "Family1-Dad2.jpg"],
    "Family1-Mom": ["Family1-Mom1.jpg", "Family1-Mom2.jpg"],
    "Family1-Son": ["Family1-Son1.jpg", "Family1-Son2.jpg"],
    "Family1-Daughter": ["Family1-Daughter1.jpg", "Family1-Daughter2.jpg"],
    "Family2-Lady": ["Family2-Lady1.jpg", "Family2-Lady2.jpg"],
    "Family2-Man": ["Family2-Man1.jpg", "Family2-Man2.jpg"],
  };

  // A group photo that includes some of the persons you seek to identify from your dictionary.
  const sourceImageFileName = "identification1.jpg";

  // Create a person group.
  console.log(`Creating a person group with ID: ${personGroupId}`);
  await client.path("/persongroups/{personGroupId}", personGroupId).put({
    body: {
      name: personGroupId,
      recognitionModel: "recognition_04",
    },
  });

  // The similar faces will be grouped into a single person group person.
  console.log("Adding faces to person group...");
  await Promise.all(
    Object.keys(personDictionary).map(async (name) => {
      console.log(`Create a persongroup person: ${name}`);
      const createPersonGroupPersonResponse = await client
        .path("/persongroups/{personGroupId}/persons", personGroupId)
        .post({
          body: { name },
        });

      const { personId } = createPersonGroupPersonResponse.body;

      await Promise.all(
        personDictionary[name].map(async (similarImage) => {
          // Check if the image is of sufficent quality for recognition.
          const detectResponse = await client.path("/detect").post({
            contentType: "application/json",
            queryParameters: {
              detectionModel: "detection_03",
              recognitionModel: "recognition_04",
              returnFaceId: false,
              returnFaceAttributes: [FaceAttributeTypeRecognition04.QUALITY_FOR_RECOGNITION],
            },
            body: { url: `${imageBaseUrl}${similarImage}` },
          });

          const sufficientQuality = detectResponse.body.every(
            (face) => face.faceAttributes?.qualityForRecognition === "high",
          );
          if (!sufficientQuality) {
            return;
          }

          // Quality is sufficent, add to group.
          console.log(
            `Add face to the person group person: (${name}) from image: (${similarImage})`,
          );
          await client
            .path(
              "/persongroups/{personGroupId}/persons/{personId}/persistedfaces",
              personGroupId,
              personId,
            )
            .post({
              queryParameters: { detectionModel: "detection_03" },
              body: { url: `${imageBaseUrl}${similarImage}` },
            });
        }),
      );
    }),
  );
  console.log("Done adding faces to person group.");

  // Start to train the person group.
  console.log();
  console.log(`Training person group: ${personGroupId}`);
  const trainResponse = await client
    .path("/persongroups/{personGroupId}/train", personGroupId)
    .post();
  const poller = await getLongRunningPoller(client, trainResponse);
  await poller.pollUntilDone();
  console.log(`Training status: ${poller.getOperationState().status}`);
  if (poller.getOperationState().status !== "succeeded") {
    return;
  }

  // Detect faces from source image url and only take those with sufficient quality for recognition.
  const detectResponse = await client.path("/detect").post({
    contentType: "application/json",
    queryParameters: {
      detectionModel: "detection_03",
      recognitionModel: "recognition_04",
      returnFaceId: true,
    },
    body: { url: `${imageBaseUrl}${sourceImageFileName}` },
  });
  const faceIds = detectResponse.body.map((face) => face.faceId);

  // Identify the faces in a person group.
  const identifyResponse = await client.path("/identify").post({
    body: { faceIds, personGroupId },
  });
  await Promise.all(
    identifyResponse.body.map(async (result) => {
      try {
        const getPersonGroupPersonResponse = await client
          .path(
            "/persongroups/{personGroupId}/persons/{personId}",
            personGroupId,
            result.candidates[0].personId,
          )
          .get();
        const person = getPersonGroupPersonResponse.body;
        console.log(
          `Person: ${person.name} is identified for face in: ${sourceImageFileName} with ID: ${result.faceId}. Confidence: ${result.candidates[0].confidence}`,
        );

        // Verification:
        const verifyResponse = await client.path("/verify").post({
          body: {
            faceId: result.faceId,
            personGroupId,
            personId: person.personId,
          },
        });
        console.log(
          `Verification result between face ${result.faceId} and person ${person.personId}: ${verifyResponse.body.isIdentical} with confidence: ${verifyResponse.body.confidence}`,
        );
      } catch (error) {
        console.log(`No persons identified for face with ID ${result.faceId}`);
      }
    }),
  );
  console.log();

  // Delete person group.
  console.log(`Deleting person group: ${personGroupId}`);
  await client.path("/persongroups/{personGroupId}", personGroupId).delete();
  console.log();

  console.log("Done.");
};

main().catch(console.error);
// </snippet_single>