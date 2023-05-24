// <snippet_imports>
const util = require('util');
const fs = require('fs');
const TrainingApi = require("@azure/cognitiveservices-customvision-training");
const PredictionApi = require("@azure/cognitiveservices-customvision-prediction");
const msRest = require("@azure/ms-rest-js");
// </snippet_imports>

/*
Prerequisites:

1. Install the Custom Vision SDK. Run:
npm install @azure/cognitiveservices-customvision-training
npm install @azure/cognitiveservices-customvision-prediction

2. Create an "Images" folder in your working directory.

3. Download the images used by this sample from:
https://github.com/Azure-Samples/cognitive-services-sample-data-files/tree/master/CustomVision/ImageClassification/Images

This sample looks for images in the following paths:
<your working directory>/Images/Hemlock
<your working directory>/Images/Japanese_Cherry
<your working directory>/Images/Test
*/

// <snippet_creds>
// retrieve environment variables
const trainingKey = process.env["VISION_TRAINING_KEY"];
const trainingEndpoint = process.env["VISION_TRAINING_ENDPOINT"];

const predictionKey = process.env["VISION_PREDICTION_KEY"];
const predictionResourceId = process.env["VISION_PREDICTION_RESOURCE_ID"];
const predictionEndpoint = process.env["VISION_PREDICTION_ENDPOINT"];
// </snippet_creds>

// <snippet_vars>
const publishIterationName = "classifyModel";
const setTimeoutPromise = util.promisify(setTimeout);
// </snippet_vars>

// <snippet_auth>
const credentials = new msRest.ApiKeyCredentials({ inHeader: { "Training-key": trainingKey } });
const trainer = new TrainingApi.TrainingAPIClient(credentials, trainingEndpoint);
const predictor_credentials = new msRest.ApiKeyCredentials({ inHeader: { "Prediction-key": predictionKey } });
const predictor = new PredictionApi.PredictionAPIClient(predictor_credentials, predictionEndpoint);
// </snippet_auth>

// <snippet_create>
(async () => {
    console.log("Creating project...");
    const sampleProject = await trainer.createProject("Sample Project");
    // </snippet_create>

    // <snippet_tags>
    const hemlockTag = await trainer.createTag(sampleProject.id, "Hemlock");
    const cherryTag = await trainer.createTag(sampleProject.id, "Japanese Cherry");
    // </snippet_tags>

    // <snippet_upload>
    const sampleDataRoot = "Images";

    console.log("Adding images...");
    let fileUploadPromises = [];

    const hemlockDir = `${sampleDataRoot}/Hemlock`;
    const hemlockFiles = fs.readdirSync(hemlockDir);
    hemlockFiles.forEach(file => {
        fileUploadPromises.push(trainer.createImagesFromData(sampleProject.id, fs.readFileSync(`${hemlockDir}/${file}`), { tagIds: [hemlockTag.id] }));
    });

    const cherryDir = `${sampleDataRoot}/Japanese_Cherry`;
    const japaneseCherryFiles = fs.readdirSync(cherryDir);
    japaneseCherryFiles.forEach(file => {
        fileUploadPromises.push(trainer.createImagesFromData(sampleProject.id, fs.readFileSync(`${cherryDir}/${file}`), { tagIds: [cherryTag.id] }));
    });

    await Promise.all(fileUploadPromises);
    // </snippet_upload>

    // <snippet_train>
    console.log("Training...");
    let trainingIteration = await trainer.trainProject(sampleProject.id);

    // Wait for training to complete
    console.log("Training started...");
    while (trainingIteration.status == "Training") {
        console.log("Training status: " + trainingIteration.status);
        await setTimeoutPromise(1000, null);
        trainingIteration = await trainer.getIteration(sampleProject.id, trainingIteration.id)
    }
    console.log("Training status: " + trainingIteration.status);
    // </snippet_train>

    // <snippet_publish>
    // Publish the iteration to the end point
    await trainer.publishIteration(sampleProject.id, trainingIteration.id, publishIterationName, predictionResourceId);
    // </snippet_publish>

    // <snippet_test>
    const testFile = fs.readFileSync(`${sampleDataRoot}/Test/test_image.jpg`);

    const results = await predictor.classifyImage(sampleProject.id, publishIterationName, testFile);

    // Show results
    console.log("Results:");
    results.predictions.forEach(predictedResult => {
        console.log(`\t ${predictedResult.tagName}: ${(predictedResult.probability * 100.0).toFixed(2)}%`);
    });
    // </snippet_test>

// <snippet_function_close>
})()
// </snippet_function_close>
