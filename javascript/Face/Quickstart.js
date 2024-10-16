// <snippet_single>
const { randomUUID } = require("crypto");

const { AzureKeyCredential } = require("@azure/core-auth");

const createFaceClient = require("@azure-rest/ai-vision-face").default,
  { getLongRunningPoller } = require("@azure-rest/ai-vision-face");

const sleep = (ms) => new Promise((resolve) => setTimeout(resolve, ms));

const main = async () => {
  const endpoint = process.env["FACE_ENDPOINT"] ?? "<endpoint>";
  const apikey = process.env["FACE_APIKEY"] ?? "<apikey>";
  const credential = new AzureKeyCredential(apikey);
  const client = createFaceClient(endpoint, credential);

  const imageBaseUrl =
    "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/";
  const largePersonGroupId = randomUUID();

  console.log("========IDENTIFY FACES========");
  console.log();

  // Create a dictionary for all your images, grouping similar ones under the same key.
  const personDictionary = {
    "Family1-Dad": ["Family1-Dad1.jpg", "Family1-Dad2.jpg"],
    "Family1-Mom": ["Family1-Mom1.jpg", "Family1-Mom2.jpg"],
    "Family1-Son": ["Family1-Son1.jpg", "Family1-Son2.jpg"],
  };

  // A group photo that includes some of the persons you seek to identify from your dictionary.
  const sourceImageFileName = "identification1.jpg";

  // Create a large person group.
  console.log(`Creating a person group with ID: ${largePersonGroupId}`);
  await client.path("/largepersongroups/{largePersonGroupId}", largePersonGroupId).put({
    body: {
      name: largePersonGroupId,
      recognitionModel: "recognition_04",
    },
  });

  // The similar faces will be grouped into a single large person group person.
  console.log("Adding faces to person group...");
  await Promise.all(
    Object.keys(personDictionary).map(async (name) => {
      console.log(`Create a persongroup person: ${name}`);
      const createLargePersonGroupPersonResponse = await client
        .path("/largepersongroups/{largePersonGroupId}/persons", largePersonGroupId)
        .post({
          body: { name },
        });

      const { personId } = createLargePersonGroupPersonResponse.body;

      await Promise.all(
        personDictionary[name].map(async (similarImage) => {
          // Check if the image is of sufficent quality for recognition.
          const detectResponse = await client.path("/detect").post({
            contentType: "application/json",
            queryParameters: {
              detectionModel: "detection_03",
              recognitionModel: "recognition_04",
              returnFaceId: false,
              returnFaceAttributes: ["qualityForRecognition"],
            },
            body: { url: `${imageBaseUrl}${similarImage}` },
          });

          const sufficientQuality = detectResponse.body.every(
            (face) => face.faceAttributes?.qualityForRecognition === "high",
          );
          if (!sufficientQuality) {
            return;
          }

          if (detectResponse.body.length != 1) {
            return;
          }

          // Quality is sufficent, add to group.
          console.log(
            `Add face to the person group person: (${name}) from image: (${similarImage})`,
          );
          await client
            .path(
              "/largepersongroups/{largePersonGroupId}/persons/{personId}/persistedfaces",
              largePersonGroupId,
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

  // Start to train the large person group.
  console.log();
  console.log(`Training person group: ${largePersonGroupId}`);
  const trainResponse = await client
    .path("/largepersongroups/{largePersonGroupId}/train", largePersonGroupId)
    .post();
  const poller = await getLongRunningPoller(client, trainResponse);
  await poller.pollUntilDone();
  console.log(`Training status: ${poller.getOperationState().status}`);
  if (poller.getOperationState().status !== "succeeded") {
    return;
  }

  console.log("Pausing for 60 seconds to avoid triggering rate limit on free account...");
  await sleep(60000);

  // Detect faces from source image url and only take those with sufficient quality for recognition.
  const detectResponse = await client.path("/detect").post({
    contentType: "application/json",
    queryParameters: {
      detectionModel: "detection_03",
      recognitionModel: "recognition_04",
      returnFaceId: true,
      returnFaceAttributes: ["qualityForRecognition"],
    },
    body: { url: `${imageBaseUrl}${sourceImageFileName}` },
  });
  const faceIds = detectResponse.body.filter((face) => face.faceAttributes?.qualityForRecognition !== "low").map((face) => face.faceId);

  // Identify the faces in a large person group.
  const identifyResponse = await client.path("/identify").post({
    body: { faceIds, largePersonGroupId: largePersonGroupId },
  });
  await Promise.all(
    identifyResponse.body.map(async (result) => {
      try {
        const getLargePersonGroupPersonResponse = await client
          .path(
            "/largepersongroups/{largePersonGroupId}/persons/{personId}",
            largePersonGroupId,
            result.candidates[0].personId,
          )
          .get();
        const person = getLargePersonGroupPersonResponse.body;
        console.log(
          `Person: ${person.name} is identified for face in: ${sourceImageFileName} with ID: ${result.faceId}. Confidence: ${result.candidates[0].confidence}`,
        );

        // Verification:
        const verifyResponse = await client.path("/verify").post({
          body: {
            faceId: result.faceId,
            largePersonGroupId: largePersonGroupId,
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

  // Delete large person group.
  console.log(`Deleting person group: ${largePersonGroupId}`);
  await client.path("/largepersongroups/{largePersonGroupId}", largePersonGroupId).delete();
  console.log();

  console.log("Done.");
};

main().catch(console.error);
// </snippet_single>