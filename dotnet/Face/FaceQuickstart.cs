// <snippet_using>
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
// </snippet_using>

/**
 * FACE QUICKSTART
 * 
 * This quickstart includes the following examples for Face:
 *  - Detect Faces
 *  - Find Similar
 *  - Identify faces (and person group operations)
 *  - Large Person Group 
 *  - Group Faces
 *  - FaceList
 *  - Large FaceList
 *  - Snapshot Operations
 * 
 * Prerequisites:
 *  - Visual Studio 2019 (or 2017, but this is app uses .NETCore, not .NET Framework)
 *  - NuGet libraries:
 *    Microsoft.Azure.CognitiveServices.Vision.Face
 *    
 * The Snapshot sample needs a 2nd Face resource in Azure to execute. Create one with a different region than your original Face resource. 
 * For example, Face resource 1: created with the 'westus' region. Face resource 2: created with the 'eastus' region. 
 * For this sample, the 1st region is referred to as the source region, and the 2nd region is referred to as the target region.
 * In the Program.cs file at the top, set your environment variables with your keys, regions, and ID. 
 * Once set, close and reopen the solution file for them to take effect.
 *   
 * How to run:
 *  - Create a new C# Console app in Visual Studio 2019.
 *  - Copy/paste the Program.cs file in the Github quickstart into your own Program.cs file. 
 *  
 * Dependencies within the samples: 
 *  - Authenticate produces a client that's used by all samples.
 *  - Detect Faces is a helper function that is used by several other samples. 
 *  - Snapshot Operations need a person group ID to be executed, so it uses the one created from Identify Faces. 
 *  - The Delete Person Group uses a person group ID, so it uses the one used in the Snapshot example. 
 *    It will delete the person group from both of your Face resources in their respective regions.
 *   
 * References:
 *  - Face Documentation: https://docs.microsoft.com/en-us/azure/cognitive-services/face/
 *  - .NET SDK: https://docs.microsoft.com/en-us/dotnet/api/overview/azure/cognitiveservices/client/face?view=azure-dotnet
 *  - API Reference: https://docs.microsoft.com/en-us/azure/cognitive-services/face/apireference
 */

namespace FaceQuickstart
{
    class Program
    {
        // Used for the Identify, Snapshot, and Delete examples.
        // The same person group is used for both Person Group Operations and Snapshot Operations.
        // <snippet_persongroup_declare>
        static string sourcePersonGroup = null;
        // </snippet_persongroup_declare>
        // <snippet_snapshot_persongroup>
        static string targetPersonGroup = null;
        // </snippet_snapshot_persongroup>

        // <snippet_image_url>
        // Used for all examples.
        // URL for the images.
        const string IMAGE_BASE_URL = "https://csdx.blob.core.windows.net/resources/Face/Images/";
        // </snippet_image_url>

        static void Main(string[] args)
        {
            // Used in Authenticate and Snapshot examples. The client these help create is used by all examples.
            // <snippet_mainvars>
            // From your Face subscription in the Azure portal, get your subscription key and endpoint.
            // Set your environment variables using the names below. Close and reopen your project for changes to take effect.
            string SUBSCRIPTION_KEY = Environment.GetEnvironmentVariable("FACE_SUBSCRIPTION_KEY");
            string ENDPOINT = Environment.GetEnvironmentVariable("FACE_ENDPOINT");
            // </snippet_mainvars>

            // <snippet_snapshot_vars>
            // The Snapshot example needs its own 2nd client, since it uses two different regions.
            string TARGET_SUBSCRIPTION_KEY = Environment.GetEnvironmentVariable("FACE_SUBSCRIPTION_KEY2");
            string TARGET_ENDPOINT = Environment.GetEnvironmentVariable("FACE_ENDPOINT2");
            // Grab your subscription ID, from any resource in Azure, from the Overview page (all resources have the same subscription ID). 
            Guid AZURE_SUBSCRIPTION_ID = new Guid(Environment.GetEnvironmentVariable("AZURE_SUBSCRIPTION_ID"));
            // Target subscription ID. It will be the same as the source ID if created Face resources from the same
            // subscription (but moving from region to region). If they are different subscriptions, add the other
            // target ID here.
            Guid TARGET_AZURE_SUBSCRIPTION_ID = new Guid(Environment.GetEnvironmentVariable("AZURE_SUBSCRIPTION_ID"));
            // </snippet_snapshot_vars>

            // <snippet_detect_models>
            // Used in the Detect Faces and Verify examples.
            // Recognition model 3 is used for feature extraction, use 1 to simply recognize/detect a face. 
            // However, the API calls to Detection that are used with Verify, Find Similar, or Identify must share the same recognition model.
            const string RECOGNITION_MODEL3 = RecognitionModel.Recognition03;
            // </snippet_detect_models>

            // Large FaceList variables
            const string LargeFaceListId = "mylargefacelistid_001"; // must be lowercase, 0-9, "_" or "-" characters
            const string LargeFaceListName = "MyLargeFaceListName";

            // <snippet_client>
            // Authenticate.
            IFaceClient client = Authenticate(ENDPOINT, SUBSCRIPTION_KEY);
            // </snippet_client>
            // <snippet_snapshot_client>
            // Authenticate for another region or subscription (used in Snapshot only).
            IFaceClient clientTarget = Authenticate(TARGET_ENDPOINT, TARGET_SUBSCRIPTION_KEY);
            // </snippet_snapshot_client>

            // <snippet_detect_call>
            // Detect - get features from faces.
            DetectFaceExtract(client, IMAGE_BASE_URL, RECOGNITION_MODEL3).Wait();
            // </snippet_detect_call>
            // Find Similar - find a similar face from a list of faces.
            FindSimilar(client, IMAGE_BASE_URL, RECOGNITION_MODEL3).Wait();
            // Verify - compare two images if the same person or not.
            Verify(client, IMAGE_BASE_URL, RECOGNITION_MODEL3).Wait();

            // Identify - recognize a face(s) in a person group (a person group is created in this example).
            IdentifyInPersonGroup(client, IMAGE_BASE_URL, RECOGNITION_MODEL3).Wait();
            // LargePersonGroup - create, then get data.
            LargePersonGroup(client, IMAGE_BASE_URL, RECOGNITION_MODEL3).Wait();
            // Group faces - automatically group similar faces.
            Group(client, IMAGE_BASE_URL, RECOGNITION_MODEL3).Wait();
            // FaceList - create a face list, then get data
            FaceListOperations(client, IMAGE_BASE_URL).Wait();
            // Large FaceList - create a large face list, then get data
            LargeFaceListOperations(client, IMAGE_BASE_URL).Wait();
            // Take a snapshot of a person group in one region, move it to the next region.
            // Can also be used for moving a person group from one Azure subscription to the next.
            // 20200923 Temporarily commenting this out due to an issue with taking snapshots.
            //Snapshot(client, clientTarget, sourcePersonGroup, AZURE_SUBSCRIPTION_ID, TARGET_AZURE_SUBSCRIPTION_ID).Wait();

            // <snippet_persongroup_delete>
            // At end, delete person groups in both regions (since testing only)
            Console.WriteLine("========DELETE PERSON GROUP========");
            Console.WriteLine();
            DeletePersonGroup(client, sourcePersonGroup).Wait();
            // </snippet_persongroup_delete>
            // <snippet_target_persongroup_delete>
            // 20200923 This target person group is not created unless we take the snapshot.
            //DeletePersonGroup(clientTarget, targetPersonGroup).Wait();
            Console.WriteLine();
            // </snippet_target_persongroup_delete>

            Console.WriteLine("End of quickstart.");
        }

        // <snippet_auth>
        /*
		 *	AUTHENTICATE
		 *	Uses subscription key and region to create a client.
		 */
        public static IFaceClient Authenticate(string endpoint, string key)
        {
            return new FaceClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };
        }
        // </snippet_auth>
        /*
		 * END - Authenticate
		 */

        // <snippet_detect>
        /* 
		 * DETECT FACES
		 * Detects features from faces and IDs them.
		 */
        public static async Task DetectFaceExtract(IFaceClient client, string url, string recognitionModel)
        {
            Console.WriteLine("========DETECT FACES========");
            Console.WriteLine();

            // Create a list of images
            List<string> imageFileNames = new List<string>
                            {
                                "detection1.jpg",    // single female with glasses
								// "detection2.jpg", // (optional: single man)
								// "detection3.jpg", // (optional: single male construction worker)
								// "detection4.jpg", // (optional: 3 people at cafe, 1 is blurred)
								"detection5.jpg",    // family, woman child man
								"detection6.jpg"     // elderly couple, male female
							};

            foreach (var imageFileName in imageFileNames)
            {
                IList<DetectedFace> detectedFaces;

                // Detect faces with all attributes from image url.
                detectedFaces = await client.Face.DetectWithUrlAsync($"{url}{imageFileName}",
                        returnFaceAttributes: new List<FaceAttributeType?> { FaceAttributeType.Accessories, FaceAttributeType.Age,
                        FaceAttributeType.Blur, FaceAttributeType.Emotion, FaceAttributeType.Exposure, FaceAttributeType.FacialHair,
                        FaceAttributeType.Gender, FaceAttributeType.Glasses, FaceAttributeType.Hair, FaceAttributeType.HeadPose,
                        FaceAttributeType.Makeup, FaceAttributeType.Noise, FaceAttributeType.Occlusion, FaceAttributeType.Smile },
                        // We specify detection model 1 because we are retrieving attributes.
                        detectionModel: DetectionModel.Detection01,
                        recognitionModel: recognitionModel);

                Console.WriteLine($"{detectedFaces.Count} face(s) detected from image `{imageFileName}`.");
                // </snippet_detect>
                // <snippet_detect_parse>
                // Parse and print all attributes of each detected face.
                foreach (var face in detectedFaces)
                {
                    Console.WriteLine($"Face attributes for {imageFileName}:");

                    // Get bounding box of the faces
                    Console.WriteLine($"Rectangle(Left/Top/Width/Height) : {face.FaceRectangle.Left} {face.FaceRectangle.Top} {face.FaceRectangle.Width} {face.FaceRectangle.Height}");

                    // Get accessories of the faces
                    List<Accessory> accessoriesList = (List<Accessory>)face.FaceAttributes.Accessories;
                    int count = face.FaceAttributes.Accessories.Count;
                    string accessory; string[] accessoryArray = new string[count];
                    if (count == 0) { accessory = "NoAccessories"; }
                    else
                    {
                        for (int i = 0; i < count; ++i) { accessoryArray[i] = accessoriesList[i].Type.ToString(); }
                        accessory = string.Join(",", accessoryArray);
                    }
                    Console.WriteLine($"Accessories : {accessory}");

                    // Get face other attributes
                    Console.WriteLine($"Age : {face.FaceAttributes.Age}");
                    Console.WriteLine($"Blur : {face.FaceAttributes.Blur.BlurLevel}");

                    // Get emotion on the face
                    string emotionType = string.Empty;
                    double emotionValue = 0.0;
                    Emotion emotion = face.FaceAttributes.Emotion;
                    if (emotion.Anger > emotionValue) { emotionValue = emotion.Anger; emotionType = "Anger"; }
                    if (emotion.Contempt > emotionValue) { emotionValue = emotion.Contempt; emotionType = "Contempt"; }
                    if (emotion.Disgust > emotionValue) { emotionValue = emotion.Disgust; emotionType = "Disgust"; }
                    if (emotion.Fear > emotionValue) { emotionValue = emotion.Fear; emotionType = "Fear"; }
                    if (emotion.Happiness > emotionValue) { emotionValue = emotion.Happiness; emotionType = "Happiness"; }
                    if (emotion.Neutral > emotionValue) { emotionValue = emotion.Neutral; emotionType = "Neutral"; }
                    if (emotion.Sadness > emotionValue) { emotionValue = emotion.Sadness; emotionType = "Sadness"; }
                    if (emotion.Surprise > emotionValue) { emotionType = "Surprise"; }
                    Console.WriteLine($"Emotion : {emotionType}");

                    // Get more face attributes
                    Console.WriteLine($"Exposure : {face.FaceAttributes.Exposure.ExposureLevel}");
                    Console.WriteLine($"FacialHair : {string.Format("{0}", face.FaceAttributes.FacialHair.Moustache + face.FaceAttributes.FacialHair.Beard + face.FaceAttributes.FacialHair.Sideburns > 0 ? "Yes" : "No")}");
                    Console.WriteLine($"Gender : {face.FaceAttributes.Gender}");
                    Console.WriteLine($"Glasses : {face.FaceAttributes.Glasses}");

                    // Get hair color
                    Hair hair = face.FaceAttributes.Hair;
                    string color = null;
                    if (hair.HairColor.Count == 0) { if (hair.Invisible) { color = "Invisible"; } else { color = "Bald"; } }
                    HairColorType returnColor = HairColorType.Unknown;
                    double maxConfidence = 0.0f;
                    foreach (HairColor hairColor in hair.HairColor)
                    {
                        if (hairColor.Confidence <= maxConfidence) { continue; }
                        maxConfidence = hairColor.Confidence; returnColor = hairColor.Color; color = returnColor.ToString();
                    }
                    Console.WriteLine($"Hair : {color}");

                    // Get more attributes
                    Console.WriteLine($"HeadPose : {string.Format("Pitch: {0}, Roll: {1}, Yaw: {2}", Math.Round(face.FaceAttributes.HeadPose.Pitch, 2), Math.Round(face.FaceAttributes.HeadPose.Roll, 2), Math.Round(face.FaceAttributes.HeadPose.Yaw, 2))}");
                    Console.WriteLine($"Makeup : {string.Format("{0}", (face.FaceAttributes.Makeup.EyeMakeup || face.FaceAttributes.Makeup.LipMakeup) ? "Yes" : "No")}");
                    Console.WriteLine($"Noise : {face.FaceAttributes.Noise.NoiseLevel}");
                    Console.WriteLine($"Occlusion : {string.Format("EyeOccluded: {0}", face.FaceAttributes.Occlusion.EyeOccluded ? "Yes" : "No")} " +
                        $" {string.Format("ForeheadOccluded: {0}", face.FaceAttributes.Occlusion.ForeheadOccluded ? "Yes" : "No")}   {string.Format("MouthOccluded: {0}", face.FaceAttributes.Occlusion.MouthOccluded ? "Yes" : "No")}");
                    Console.WriteLine($"Smile : {face.FaceAttributes.Smile}");
                    Console.WriteLine();
                }
            }
        }
        // </snippet_detect_parse>

        // Detect faces from image url for recognition purpose. This is a helper method for other functions in this quickstart.
        // Parameter `returnFaceId` of `DetectWithUrlAsync` must be set to `true` (by default) for recognition purpose.
        // The field `faceId` in returned `DetectedFace`s will be used in Face - Find Similar, Face - Verify. and Face - Identify.
        // It will expire 24 hours after the detection call.
        // <snippet_face_detect_recognize>
        private static async Task<List<DetectedFace>> DetectFaceRecognize(IFaceClient faceClient, string url, string recognition_model)
        {
            // Detect faces from image URL. Since only recognizing, use the recognition model 1.
            // We use detection model 2 because we are not retrieving attributes.
            IList<DetectedFace> detectedFaces = await faceClient.Face.DetectWithUrlAsync(url, recognitionModel: recognition_model, detectionModel: DetectionModel.Detection02);
            Console.WriteLine($"{detectedFaces.Count} face(s) detected from image `{Path.GetFileName(url)}`");
            return detectedFaces.ToList();
        }
        // </snippet_face_detect_recognize>
        /*
		 * END - DETECT FACES 
		 */

        // <snippet_find_similar>
        /*
		 * FIND SIMILAR
		 * This example will take an image and find a similar one to it in another image.
		 */
        public static async Task FindSimilar(IFaceClient client, string url, string recognition_model)
        {
            Console.WriteLine("========FIND SIMILAR========");
            Console.WriteLine();

            List<string> targetImageFileNames = new List<string>
                                {
                                    "Family1-Dad1.jpg",
                                    "Family1-Daughter1.jpg",
                                    "Family1-Mom1.jpg",
                                    "Family1-Son1.jpg",
                                    "Family2-Lady1.jpg",
                                    "Family2-Man1.jpg",
                                    "Family3-Lady1.jpg",
                                    "Family3-Man1.jpg"
                                };

            string sourceImageFileName = "findsimilar.jpg";
            IList<Guid?> targetFaceIds = new List<Guid?>();
            foreach (var targetImageFileName in targetImageFileNames)
            {
                // Detect faces from target image url.
                var faces = await DetectFaceRecognize(client, $"{url}{targetImageFileName}", recognition_model);
                // Add detected faceId to list of GUIDs.
                targetFaceIds.Add(faces[0].FaceId.Value);
            }

            // Detect faces from source image url.
            IList<DetectedFace> detectedFaces = await DetectFaceRecognize(client, $"{url}{sourceImageFileName}", recognition_model);
            Console.WriteLine();

            // Find a similar face(s) in the list of IDs. Comapring only the first in list for testing purposes.
            IList<SimilarFace> similarResults = await client.Face.FindSimilarAsync(detectedFaces[0].FaceId.Value, null, null, targetFaceIds);
            // </snippet_find_similar>
            // <snippet_find_similar_print>
            foreach (var similarResult in similarResults)
            {
                Console.WriteLine($"Faces from {sourceImageFileName} & ID:{similarResult.FaceId} are similar with confidence: {similarResult.Confidence}.");
            }
            Console.WriteLine();
            // </snippet_find_similar_print>
        }
        /*
		 * END - FIND SIMILAR 
		 */

        /*
		 * VERIFY
		 * The Verify operation takes a face ID from DetectedFace or PersistedFace and either another face ID 
		 * or a Person object and determines whether they belong to the same person. If you pass in a Person object, 
		 * you can optionally pass in a PersonGroup to which that Person belongs to improve performance.
		 */
        public static async Task Verify(IFaceClient client, string url, string recognitionModel03)
        {
            Console.WriteLine("========VERIFY========");
            Console.WriteLine();

            List<string> targetImageFileNames = new List<string> { "Family1-Dad1.jpg", "Family1-Dad2.jpg" };
            string sourceImageFileName1 = "Family1-Dad3.jpg";
            string sourceImageFileName2 = "Family1-Son1.jpg";


            List<Guid> targetFaceIds = new List<Guid>();
            foreach (var imageFileName in targetImageFileNames)
            {
                // Detect faces from target image url.
                List<DetectedFace> detectedFaces = await DetectFaceRecognize(client, $"{url}{imageFileName} ", recognitionModel03);
                targetFaceIds.Add(detectedFaces[0].FaceId.Value);
                Console.WriteLine($"{detectedFaces.Count} faces detected from image `{imageFileName}`.");
            }

            // Detect faces from source image file 1.
            List<DetectedFace> detectedFaces1 = await DetectFaceRecognize(client, $"{url}{sourceImageFileName1} ", recognitionModel03);
            Console.WriteLine($"{detectedFaces1.Count} faces detected from image `{sourceImageFileName1}`.");
            Guid sourceFaceId1 = detectedFaces1[0].FaceId.Value;

            // Detect faces from source image file 2.
            List<DetectedFace> detectedFaces2 = await DetectFaceRecognize(client, $"{url}{sourceImageFileName2} ", recognitionModel03);
            Console.WriteLine($"{detectedFaces2.Count} faces detected from image `{sourceImageFileName2}`.");
            Guid sourceFaceId2 = detectedFaces2[0].FaceId.Value;

            // Verification example for faces of the same person.
            VerifyResult verifyResult1 = await client.Face.VerifyFaceToFaceAsync(sourceFaceId1, targetFaceIds[0]);
            Console.WriteLine(
                verifyResult1.IsIdentical
                    ? $"Faces from {sourceImageFileName1} & {targetImageFileNames[0]} are of the same (Positive) person, similarity confidence: {verifyResult1.Confidence}."
                    : $"Faces from {sourceImageFileName1} & {targetImageFileNames[0]} are of different (Negative) persons, similarity confidence: {verifyResult1.Confidence}.");

            // Verification example for faces of different persons.
            VerifyResult verifyResult2 = await client.Face.VerifyFaceToFaceAsync(sourceFaceId2, targetFaceIds[0]);
            Console.WriteLine(
                verifyResult2.IsIdentical
                    ? $"Faces from {sourceImageFileName2} & {targetImageFileNames[0]} are of the same (Negative) person, similarity confidence: {verifyResult2.Confidence}."
                    : $"Faces from {sourceImageFileName2} & {targetImageFileNames[0]} are of different (Positive) persons, similarity confidence: {verifyResult2.Confidence}.");

            Console.WriteLine();
        }
        /*
		 * END - VERIFY 
		 */

        /*
		 * IDENTIFY FACES
		 * To identify faces, you need to create and define a person group.
		 * The Identify operation takes one or several face IDs from DetectedFace or PersistedFace and a PersonGroup and returns 
		 * a list of Person objects that each face might belong to. Returned Person objects are wrapped as Candidate objects, 
		 * which have a prediction confidence value.
		 */
        public static async Task IdentifyInPersonGroup(IFaceClient client, string url, string recognitionModel)
        {
            Console.WriteLine("========IDENTIFY FACES========");
            Console.WriteLine();

            // <snippet_persongroup_files>
            // Create a dictionary for all your images, grouping similar ones under the same key.
            Dictionary<string, string[]> personDictionary =
                new Dictionary<string, string[]>
                    { { "Family1-Dad", new[] { "Family1-Dad1.jpg", "Family1-Dad2.jpg" } },
                      { "Family1-Mom", new[] { "Family1-Mom1.jpg", "Family1-Mom2.jpg" } },
                      { "Family1-Son", new[] { "Family1-Son1.jpg", "Family1-Son2.jpg" } },
                      { "Family1-Daughter", new[] { "Family1-Daughter1.jpg", "Family1-Daughter2.jpg" } },
                      { "Family2-Lady", new[] { "Family2-Lady1.jpg", "Family2-Lady2.jpg" } },
                      { "Family2-Man", new[] { "Family2-Man1.jpg", "Family2-Man2.jpg" } }
                    };
            // A group photo that includes some of the persons you seek to identify from your dictionary.
            string sourceImageFileName = "identification1.jpg";
            // </snippet_persongroup_files>

            // <snippet_persongroup_create>
            // Create a person group. 
            string personGroupId = Guid.NewGuid().ToString();
            sourcePersonGroup = personGroupId; // This is solely for the snapshot operations example
            Console.WriteLine($"Create a person group ({personGroupId}).");
            await client.PersonGroup.CreateAsync(personGroupId, personGroupId, recognitionModel: recognitionModel);
            // The similar faces will be grouped into a single person group person.
            foreach (var groupedFace in personDictionary.Keys)
            {
                // Limit TPS
                await Task.Delay(250);
                Person person = await client.PersonGroupPerson.CreateAsync(personGroupId: personGroupId, name: groupedFace);
                Console.WriteLine($"Create a person group person '{groupedFace}'.");

                // Add face to the person group person.
                foreach (var similarImage in personDictionary[groupedFace])
                {
                    Console.WriteLine($"Add face to the person group person({groupedFace}) from image `{similarImage}`");
                    PersistedFace face = await client.PersonGroupPerson.AddFaceFromUrlAsync(personGroupId, person.PersonId,
                        $"{url}{similarImage}", similarImage);
                }
            }
            // </snippet_persongroup_create>

            // <snippet_persongroup_train>
            // Start to train the person group.
            Console.WriteLine();
            Console.WriteLine($"Train person group {personGroupId}.");
            await client.PersonGroup.TrainAsync(personGroupId);

            // Wait until the training is completed.
            while (true)
            {
                await Task.Delay(1000);
                var trainingStatus = await client.PersonGroup.GetTrainingStatusAsync(personGroupId);
                Console.WriteLine($"Training status: {trainingStatus.Status}.");
                if (trainingStatus.Status == TrainingStatusType.Succeeded) { break; }
            }
            // </snippet_persongroup_train>
            Console.WriteLine();
            // <snippet_identify_sources>
            List<Guid?> sourceFaceIds = new List<Guid?>();
            // Detect faces from source image url.
            List<DetectedFace> detectedFaces = await DetectFaceRecognize(client, $"{url}{sourceImageFileName}", recognitionModel);

            // Add detected faceId to sourceFaceIds.
            foreach (var detectedFace in detectedFaces) { sourceFaceIds.Add(detectedFace.FaceId.Value); }
            // </snippet_identify_sources>
            // <snippet_identify>
            // Identify the faces in a person group. 
            var identifyResults = await client.Face.IdentifyAsync(sourceFaceIds, personGroupId);

            foreach (var identifyResult in identifyResults)
            {
                Person person = await client.PersonGroupPerson.GetAsync(personGroupId, identifyResult.Candidates[0].PersonId);
                Console.WriteLine($"Person '{person.Name}' is identified for face in: {sourceImageFileName} - {identifyResult.FaceId}," +
                    $" confidence: {identifyResult.Candidates[0].Confidence}.");
            }
            Console.WriteLine();
            // </snippet_identify>
        }
        /*
		 * END - IDENTIFY FACES
		 */

        /*
		 * LARGE PERSON GROUP
		 * The example will create a large person group, retrieve information from it, 
		 * list the Person IDs it contains, and finally delete a large person group.
		 * For simplicity, the same images are used for the regular-sized person group in IDENTIFY FACES of this quickstart.
		 * A large person group is made up of person group persons. 
		 * One person group person is made up of many similar images of that person, which are each PersistedFace objects.
		 */
        public static async Task LargePersonGroup(IFaceClient client, string url, string recognitionModel)
        {
            Console.WriteLine("========LARGE PERSON GROUP========");
            Console.WriteLine();

            // Create a dictionary for all your images, grouping similar ones under the same key.
            Dictionary<string, string[]> personDictionary =
            new Dictionary<string, string[]>
                { { "Family1-Dad", new[] { "Family1-Dad1.jpg", "Family1-Dad2.jpg" } },
                      { "Family1-Mom", new[] { "Family1-Mom1.jpg", "Family1-Mom2.jpg" } },
                      { "Family1-Son", new[] { "Family1-Son1.jpg", "Family1-Son2.jpg" } },
                      { "Family1-Daughter", new[] { "Family1-Daughter1.jpg", "Family1-Daughter2.jpg" } },
                      { "Family2-Lady", new[] { "Family2-Lady1.jpg", "Family2-Lady2.jpg" } },
                      { "Family2-Man", new[] { "Family2-Man1.jpg", "Family2-Man2.jpg" } }
                };

            // Create a large person group ID. 
            string largePersonGroupId = Guid.NewGuid().ToString();
            Console.WriteLine($"Create a large person group ({largePersonGroupId}).");

            // Create the large person group
            await client.LargePersonGroup.CreateAsync(largePersonGroupId: largePersonGroupId, name: largePersonGroupId, recognitionModel);

            // Create Person objects from images in our dictionary
            // We'll store their IDs in the process
            List<Guid> personIds = new List<Guid>();
            foreach (var groupedFace in personDictionary.Keys)
            {
                // Limit TPS
                await Task.Delay(250);

                Person personLarge = await client.LargePersonGroupPerson.CreateAsync(largePersonGroupId, groupedFace);
                Console.WriteLine();
                Console.WriteLine($"Create a large person group person '{groupedFace}' ({personLarge.PersonId}).");

                // Store these IDs for later retrieval
                personIds.Add(personLarge.PersonId);

                // Add face to the large person group person.
                foreach (var image in personDictionary[groupedFace])
                {
                    Console.WriteLine($"Add face to the person group person '{groupedFace}' from image `{image}`");
                    PersistedFace face = await client.LargePersonGroupPerson.AddFaceFromUrlAsync(largePersonGroupId, personLarge.PersonId,
                        $"{url}{image}", image);
                }
            }

            // Start to train the large person group.
            Console.WriteLine();
            Console.WriteLine($"Train large person group {largePersonGroupId}.");
            await client.LargePersonGroup.TrainAsync(largePersonGroupId);

            // Wait until the training is completed.
            while (true)
            {
                await Task.Delay(1000);
                var trainingStatus = await client.LargePersonGroup.GetTrainingStatusAsync(largePersonGroupId);
                Console.WriteLine($"Training status: {trainingStatus.Status}.");
                if (trainingStatus.Status == TrainingStatusType.Succeeded) { break; }
            }
            Console.WriteLine();

            // Now that we have created and trained a large person group, we can retrieve data from it.
            // Get list of persons and retrieve data, starting at the first Person ID in previously saved list.
            IList<Person> persons = await client.LargePersonGroupPerson.ListAsync(largePersonGroupId, start: "");

            Console.WriteLine($"Persisted Face IDs (from {persons.Count} large person group persons): ");
            foreach (Person person in persons)
            {
                foreach (Guid pFaceId in person.PersistedFaceIds)
                {
                    Console.WriteLine($"The person '{person.Name}' has an image with ID: {pFaceId}");
                }
            }
            Console.WriteLine();

            // After testing, delete the large person group, PersonGroupPersons also get deleted.
            await client.LargePersonGroup.DeleteAsync(largePersonGroupId);
            Console.WriteLine($"Deleted the large person group {largePersonGroupId}.");
            Console.WriteLine();
        }
        /*
		 * END - LARGE PERSON GROUP
		 */

        /*
		 * GROUP FACES
		 * This method of grouping is useful if you don't need to create a person group. It will automatically group similar
		 * images, whereas the person group method allows you to define the grouping.
		 * A single "messyGroup" array contains face IDs for which no similarities were found.
		 */
        public static async Task Group(IFaceClient client, string url, string recognition_model)
        {
            Console.WriteLine("========GROUP FACES========");
            Console.WriteLine();

            // Create list of image names
            List<string> imageFileNames = new List<string>
                              {
                                  "Family1-Dad1.jpg",
                                  "Family1-Dad2.jpg",
                                  "Family3-Lady1.jpg",
                                  "Family1-Daughter1.jpg",
                                  "Family1-Daughter2.jpg",
                                  "Family1-Daughter3.jpg"
                              };
            // Create empty dictionary to store the groups
            Dictionary<string, string> faces = new Dictionary<string, string>();
            List<Guid?> faceIds = new List<Guid?>();

            // First, detect the faces in your images
            foreach (var imageFileName in imageFileNames)
            {
                // Detect faces from image url.
                IList<DetectedFace> detectedFaces = await DetectFaceRecognize(client, $"{url}{imageFileName}", recognition_model);
                // Add detected faceId to faceIds and faces.
                faceIds.Add(detectedFaces[0].FaceId.Value);
                faces.Add(detectedFaces[0].FaceId.ToString(), imageFileName);
            }
            Console.WriteLine();
            // Group the faces. Grouping result is a group collection, each group contains similar faces.
            var groupResult = await client.Face.GroupAsync(faceIds);

            // Face groups contain faces that are similar to all members of its group.
            for (int i = 0; i < groupResult.Groups.Count; i++)
            {
                Console.Write($"Found face group {i + 1}: ");
                foreach (var faceId in groupResult.Groups[i]) { Console.Write($"{faces[faceId.ToString()]} "); }
                Console.WriteLine(".");
            }

            // MessyGroup contains all faces which are not similar to any other faces. The faces that cannot be grouped.
            if (groupResult.MessyGroup.Count > 0)
            {
                Console.Write("Found messy face group: ");
                foreach (var faceId in groupResult.MessyGroup) { Console.Write($"{faces[faceId.ToString()]} "); }
                Console.WriteLine(".");
            }
            Console.WriteLine();
        }
        /*
		 * END - GROUP FACES
		 */

        /*
		 * FACELIST OPERATIONS
		 * Create a face list and add single-faced images to it, then retrieve data from the faces.
		 * Images are used from URLs.
		 */
        public static async Task FaceListOperations(IFaceClient client, string baseUrl)
        {
            Console.WriteLine("========FACELIST OPERATIONS========");
            Console.WriteLine();

            const string FaceListId = "myfacelistid_001";
            const string FaceListName = "MyFaceListName";

            // Create an empty FaceList with user-defined specifications, it gets stored in the client.
            await client.FaceList.CreateAsync(faceListId: FaceListId, name: FaceListName);

            // Create a list of single-faced images to append to base URL. Images with mulitple faces are not accepted.
            List<string> imageFileNames = new List<string>
                            {
                                "detection1.jpg",    // single female with glasses
								"detection2.jpg",    // single male
							    "detection3.jpg",    // single male construction worker
							};

            // Add Faces to the FaceList.
            foreach (string image in imageFileNames)
            {
                string urlFull = baseUrl + image;
                // Returns a Task<PersistedFace> which contains a GUID, and is stored in the client.
                await client.FaceList.AddFaceFromUrlAsync(faceListId: FaceListId, url: urlFull);
            }

            // Print the face list
            Console.WriteLine("Face IDs from the face list: ");
            Console.WriteLine();

            // List the IDs of each stored image
            FaceList faceList = await client.FaceList.GetAsync(FaceListId);

            foreach (PersistedFace face in faceList.PersistedFaces)
            {
                Console.WriteLine(face.PersistedFaceId);
            }

            // Delete the face list, for repetitive testing purposes (cannot recreate list with same name).
            await client.FaceList.DeleteAsync(FaceListId);
            Console.WriteLine();
            Console.WriteLine("Deleted the face list.");
            Console.WriteLine();
        }
        /*
		 * END - FACELIST OPERATIONS
		 */

        /*
		* LARGE FACELIST OPERATIONS
		* Create a large face list and adds single-faced images to it, then retrieve data from the faces.
		* Images are used from URLs. Large face lists are preferred for scale, up to 1 million images.
		*/
        public static async Task LargeFaceListOperations(IFaceClient client, string baseUrl)
        {
            Console.WriteLine("======== LARGE FACELIST OPERATIONS========");
            Console.WriteLine();

            const string LargeFaceListId = "mylargefacelistid_001"; // must be lowercase, 0-9, or "_"
            const string LargeFaceListName = "MyLargeFaceListName";
            const int timeIntervalInMilliseconds = 1000; // waiting time in training

            List<string> singleImages = new List<string>
                                {
                                    "Family1-Dad1.jpg",
                                    "Family1-Daughter1.jpg",
                                    "Family1-Mom1.jpg",
                                    "Family1-Son1.jpg",
                                    "Family2-Lady1.jpg",
                                    "Family2-Man1.jpg",
                                    "Family3-Lady1.jpg",
                                    "Family3-Man1.jpg"
                                };

            // Create a large face list
            Console.WriteLine("Creating a large face list...");
            await client.LargeFaceList.CreateAsync(largeFaceListId: LargeFaceListId, name: LargeFaceListName);

            // Add Faces to the LargeFaceList.
            Console.WriteLine("Adding faces to a large face list...");
            foreach (string image in singleImages)
            {
                // Returns a PersistedFace which contains a GUID.
                await client.LargeFaceList.AddFaceFromUrlAsync(largeFaceListId: LargeFaceListId, url: $"{baseUrl}{image}");
            }

            // Training a LargeFaceList is what sets it apart from a regular FaceList.
            // You must train before using the large face list, for example to use the Find Similar operations.
            Console.WriteLine("Training a large face list...");
            await client.LargeFaceList.TrainAsync(LargeFaceListId);

            // Wait for training finish.
            while (true)
            {
                Task.Delay(timeIntervalInMilliseconds).Wait();
                var status = await client.LargeFaceList.GetTrainingStatusAsync(LargeFaceListId);

                if (status.Status == TrainingStatusType.Running)
                {
                    Console.WriteLine($"Training status: {status.Status}");
                    continue;
                }
                else if (status.Status == TrainingStatusType.Succeeded)
                {
                    Console.WriteLine($"Training status: {status.Status}");
                    break;
                }
                else
                {
                    throw new Exception("The train operation has failed!");
                }
            }

            // Print the large face list
            Console.WriteLine();
            Console.WriteLine("Face IDs from the large face list: ");
            Console.WriteLine();
            Parallel.ForEach(
                    await client.LargeFaceList.ListFacesAsync(LargeFaceListId),
                    faceId =>
                    {
                        Console.WriteLine(faceId.PersistedFaceId);
                    }
                );

            // Delete the large face list, for repetitive testing purposes (cannot recreate list with same name).
            await client.LargeFaceList.DeleteAsync(LargeFaceListId);
            Console.WriteLine();
            Console.WriteLine("Deleted the large face list.");
            Console.WriteLine();
        }
        /*
		* END - LARGE FACELIST OPERATIONS
		*/

        // <snippet_snapshot_take>
        /*
		 * SNAPSHOT OPERATIONS
		 * Copies a person group from one Azure region (or subscription) to another. For example: from the EastUS region to the WestUS.
		 * The same process can be used for face lists. 
		 * NOTE: the person group in the target region has a new person group ID, so it no longer associates with the source person group.
		 */
        public static async Task Snapshot(IFaceClient clientSource, IFaceClient clientTarget, string personGroupId, Guid azureId, Guid targetAzureId)
        {
            Console.WriteLine("========SNAPSHOT OPERATIONS========");
            Console.WriteLine();

            // Take a snapshot for the person group that was previously created in your source region.
            var takeSnapshotResult = await clientSource.Snapshot.TakeAsync(SnapshotObjectType.PersonGroup, personGroupId, new Guid?[] { azureId }); // add targetAzureId to this array if your target ID is different from your source ID.

            // Get operation id from response for tracking the progress of snapshot taking.
            var operationId = Guid.Parse(takeSnapshotResult.OperationLocation.Split('/')[2]);
            Console.WriteLine($"Taking snapshot(operation ID: {operationId})... Started");
            // </snippet_snapshot_take>
            // <snippet_snapshot_take_wait>
            // Wait for taking the snapshot to complete.
            OperationStatus operationStatus = null;
            do
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(1000));
                // Get the status of the operation.
                operationStatus = await clientSource.Snapshot.GetOperationStatusAsync(operationId);
                Console.WriteLine($"Operation Status: {operationStatus.Status}");
            }
            while (operationStatus.Status != OperationStatusType.Succeeded && operationStatus.Status != OperationStatusType.Failed);
            // Confirm the location of the resource where the snapshot is taken and its snapshot ID
            var snapshotId = Guid.Parse(operationStatus.ResourceLocation.Split('/')[2]);
            Console.WriteLine($"Source region snapshot ID: {snapshotId}");
            Console.WriteLine($"Taking snapshot of person group: {personGroupId}... Done\n");
            // </snippet_snapshot_take_wait>

            // <snippet_snapshot_apply>
            // Apply the snapshot in target region, with a new ID.
            var newPersonGroupId = Guid.NewGuid().ToString();
            targetPersonGroup = newPersonGroupId;

            try
            {
                var applySnapshotResult = await clientTarget.Snapshot.ApplyAsync(snapshotId, newPersonGroupId);

                // Get operation id from response for tracking the progress of snapshot applying.
                var applyOperationId = Guid.Parse(applySnapshotResult.OperationLocation.Split('/')[2]);
                Console.WriteLine($"Applying snapshot(operation ID: {applyOperationId})... Started");
                // </snippet_snapshot_apply>
                // <snippet_snapshot_apply_wait>
                // Wait for applying operation to complete
                do
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(1000));
                    // Get the status of the operation.
                    operationStatus = await clientSource.Snapshot.GetOperationStatusAsync(applyOperationId);
                    Console.WriteLine($"Operation Status: {operationStatus.Status}");
                }
                while (operationStatus.Status != OperationStatusType.Succeeded && operationStatus.Status != OperationStatusType.Failed);
                // Confirm location of the target resource location, with its ID.
                Console.WriteLine($"Person group in new region: {newPersonGroupId}");
                Console.WriteLine("Applying snapshot... Done\n");
            }
            // </snippet_snapshot_apply_wait>
            // <snippet_snapshot_trycatch>
            catch (Exception e)
            {
                throw new ApplicationException("Do you have a second Face resource in Azure? " +
                    "It's needed to transfer the person group to it for the Snapshot example.", e);
            }
        }
        // </snippet_snapshot_trycatch>
        /*
		 * END - SNAPSHOT OPERATIONS 
		 */

        // <snippet_deletepersongroup>
        /*
		 * DELETE PERSON GROUP
		 * After this entire example is executed, delete the person group in your Azure account,
		 * otherwise you cannot recreate one with the same name (if running example repeatedly).
		 */
        public static async Task DeletePersonGroup(IFaceClient client, String personGroupId)
        {
            await client.PersonGroup.DeleteAsync(personGroupId);
            Console.WriteLine($"Deleted the person group {personGroupId}.");
        }
        // </snippet_deletepersongroup>
        /*
		 * END - DELETE PERSON GROUP
		 */
    }
}
