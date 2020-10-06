// <snippet_imports>
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
// </snippet_imports>

namespace ImageClassification
{
    class Program
    {
        // <snippet_creds>
        private const string endpoint = "<your API endpoint here>";
        // Add your training & prediction key from the settings page of the portal
        private const string trainingKey = "<your training key here>";
        private const string predictionKey = "<your prediction key here>";
        private static List<string> hemlockImages;
        private static List<string> japaneseCherryImages;
        private static MemoryStream testImage;
        // </snippet_creds>

        static void Main(string[] args)
        {
            // <snippet_maincalls>
            CustomVisionTrainingClient TrainingApi = AuthenticateTraining(endpoint, trainingKey);
            CustomVisionPredictionClient predictionApi = AuthenticateTraining(endpoint, predictionKey);

            CreateProject(trainingApi);
            AddTags(trainingApi);
            UploadImages(trainingApi);
            TrainProject(trainingApi);
            PublishIteration(trainingApi);
            TestIteration(predictionApi);
            // </snippet_maincalls>
        }

        // <snippet_auth>
        private CustomVisionTrainingClient AuthenticateTraining(string endpoint, string trainingKey, string predictionKey)
        {
            // Create the Api, passing in the training key
            CustomVisionTrainingClient trainingApi = new CustomVisionTrainingClient(new Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.ApiKeyServiceClientCredentials(trainingKey))
            {
                Endpoint = endpoint
            };
            return trainingApi;
        }
        private CustomVisionPredictionClient AuthenticatePrediction(string endpoint, string predictionKey)
        {
            // Create a prediction endpoint, passing in the obtained prediction key
            CustomVisionPredictionClient predictionApi = new CustomVisionPredictionClient(new Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.ApiKeyServiceClientCredentials(predictionKey))
            {
                Endpoint = endpoint
            };
            return predictionApi;
        }
        // </snippet_auth>

        // <snippet_create>
        private void CreateProject(CustomVisionTrainingClient trainingApi)
        {
            // Create a new project
            Console.WriteLine("Creating new project:");
            var project = trainingApi.CreateProject("My New Project");
        }
        // </snippet_create>
        // <snippet_addtags>
        private void AddTags(CustomVisionTrainingClient trainingApi)
        {

            // Make two tags in the new project
            var hemlockTag = trainingApi.CreateTag(project.Id, "Hemlock");
            var japaneseCherryTag = trainingApi.CreateTag(project.Id, "Japanese Cherry");
        }
        // </snippet_addtags>

        // <snippet_upload>
        private void UploadImages(CustomVisionTrainingClient trainingApi)
        {
            // Add some images to the tags
            Console.WriteLine("\tUploading images");
            LoadImagesFromDisk();

            // Images can be uploaded one at a time
            foreach (var image in hemlockImages)
            {
                using (var stream = new MemoryStream(File.ReadAllBytes(image)))
                {
                    trainingApi.CreateImagesFromData(project.Id, stream, new List<Guid>() { hemlockTag.Id });
                }
            }

            // Or uploaded in a single batch 
            var imageFiles = japaneseCherryImages.Select(img => new ImageFileCreateEntry(Path.GetFileName(img), File.ReadAllBytes(img))).ToList();
            trainingApi.CreateImagesFromFiles(project.Id, new ImageFileCreateBatch(imageFiles, new List<Guid>() { japaneseCherryTag.Id }));

        }
        // </snippet_upload>

        // <snippet_train>
        private void TrainProject(CustomVisionTrainingClient trainingApi)
        {
            // Now there are images with tags start training the project
            Console.WriteLine("\tTraining");
            var iteration = trainingApi.TrainProject(project.Id);

            // The returned iteration will be in progress, and can be queried periodically to see when it has completed
            while (iteration.Status == "Training")
            {
                Thread.Sleep(1000);

                // Re-query the iteration to get it's updated status
                iteration = trainingApi.GetIteration(project.Id, iteration.Id);
            }
        }
        // </snippet_train>

        // <snippet_publish>
        private void PublishIteration(CustomVisionTrainingClient trainingApi)
        {

            // The iteration is now trained. Publish it to the prediction end point.
            var publishedModelName = "treeClassModel";
            var predictionResourceId = "<target prediction resource ID>";
            trainingApi.PublishIteration(project.Id, iteration.Id, publishedModelName, predictionResourceId);
            Console.WriteLine("Done!\n");

            // Now there is a trained endpoint, it can be used to make a prediction
        }
        // </snippet_publish>

        // <snippet_test>
        private void TestIteration(CustomVisionPredictionClient predictionApi)
        {

            // Make a prediction against the new project
            Console.WriteLine("Making a prediction:");
            var result = predictionApi.ClassifyImage(project.Id, publishedModelName, testImage);

            // Loop over each prediction and write out the results
            foreach (var c in result.Predictions)
            {
                Console.WriteLine($"\t{c.TagName}: {c.Probability:P1}");
            }
            Console.ReadKey();
        }
        // </snippet_test>

        // <snippet_loadimages>
        private static void LoadImagesFromDisk()
        {
            // this loads the images to be uploaded from disk into memory
            hemlockImages = Directory.GetFiles(Path.Combine("Images", "Hemlock")).ToList();
            japaneseCherryImages = Directory.GetFiles(Path.Combine("Images", "Japanese Cherry")).ToList();
            testImage = new MemoryStream(File.ReadAllBytes(Path.Combine("Images", "Test\\test_image.jpg")));
        }
        // </snippet_loadimages>
    }
}
