// <snippet_single>
using System.Net.Http.Headers;
using System.Text;

using Azure;
using Azure.AI.Vision.Face;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FaceQuickstart
{
    class Program
    {
        static readonly string largePersonGroupId = Guid.NewGuid().ToString();

        // URL path for the images.
        const string IMAGE_BASE_URL = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/";

        // From your Face subscription in the Azure portal, get your subscription key and endpoint.
        static readonly string SUBSCRIPTION_KEY = Environment.GetEnvironmentVariable("FACE_APIKEY") ?? "<apikey>";
        static readonly string ENDPOINT = Environment.GetEnvironmentVariable("FACE_ENDPOINT") ?? "<endpoint>";

        static void Main(string[] args)
        {
            // Recognition model 4 was released in 2021 February.
            // It is recommended since its accuracy is improved
            // on faces wearing masks compared with model 3,
            // and its overall accuracy is improved compared
            // with models 1 and 2.
            FaceRecognitionModel RECOGNITION_MODEL4 = FaceRecognitionModel.Recognition04;

            // Authenticate.
            FaceClient client = Authenticate(ENDPOINT, SUBSCRIPTION_KEY);

            // Identify - recognize a face(s) in a large person group (a large person group is created in this example).
            IdentifyInLargePersonGroup(client, IMAGE_BASE_URL, RECOGNITION_MODEL4).Wait();

            Console.WriteLine("End of quickstart.");
        }

        /*
         *	AUTHENTICATE
         *	Uses subscription key and region to create a client.
         */
        public static FaceClient Authenticate(string endpoint, string key)
        {
            return new FaceClient(new Uri(endpoint), new AzureKeyCredential(key));
        }

        // Detect faces from image url for recognition purposes. This is a helper method for other functions in this quickstart.
        // Parameter `returnFaceId` of `DetectAsync` must be set to `true` (by default) for recognition purposes.
        // Parameter `returnFaceAttributes` is set to include the QualityForRecognition attribute. 
        // Recognition model must be set to recognition_03 or recognition_04 as a result.
        // Result faces with insufficient quality for recognition are filtered out. 
        // The field `faceId` in returned `DetectedFace`s will be used in Verify and Identify.
        // It will expire 24 hours after the detection call.
        private static async Task<List<FaceDetectionResult>> DetectFaceRecognize(FaceClient faceClient, string url, FaceRecognitionModel recognition_model)
        {
            // Detect faces from image URL.
            Response<IReadOnlyList<FaceDetectionResult>> response = await faceClient.DetectAsync(new Uri(url), FaceDetectionModel.Detection03, recognition_model, returnFaceId: true, [FaceAttributeType.QualityForRecognition]);
            IReadOnlyList<FaceDetectionResult> detectedFaces = response.Value;
            List<FaceDetectionResult> sufficientQualityFaces = new List<FaceDetectionResult>();
            foreach (FaceDetectionResult detectedFace in detectedFaces)
            {
                var faceQualityForRecognition = detectedFace.FaceAttributes.QualityForRecognition;
                if (faceQualityForRecognition.HasValue && (faceQualityForRecognition.Value != QualityForRecognition.Low))
                {
                    sufficientQualityFaces.Add(detectedFace);
                }
            }
            Console.WriteLine($"{detectedFaces.Count} face(s) with {sufficientQualityFaces.Count} having sufficient quality for recognition detected from image `{Path.GetFileName(url)}`");

            return sufficientQualityFaces;
        }

        /*
         * IDENTIFY FACES
         * To identify faces, you need to create and define a large person group.
         * The Identify operation takes one or several face IDs from DetectedFace or PersistedFace and a LargePersonGroup and returns 
         * a list of Person objects that each face might belong to. Returned Person objects are wrapped as Candidate objects, 
         * which have a prediction confidence value.
         */
        public static async Task IdentifyInLargePersonGroup(FaceClient client, string url, FaceRecognitionModel recognitionModel)
        {
            Console.WriteLine("========IDENTIFY FACES========");
            Console.WriteLine();

            // Create a dictionary for all your images, grouping similar ones under the same key.
            Dictionary<string, string[]> personDictionary =
                new Dictionary<string, string[]>
                    { { "Family1-Dad", new[] { "Family1-Dad1.jpg", "Family1-Dad2.jpg" } },
                      { "Family1-Mom", new[] { "Family1-Mom1.jpg", "Family1-Mom2.jpg" } },
                      { "Family1-Son", new[] { "Family1-Son1.jpg", "Family1-Son2.jpg" } }
                    };
            // A group photo that includes some of the persons you seek to identify from your dictionary.
            string sourceImageFileName = "identification1.jpg";

            // Create a large person group.
            Console.WriteLine($"Create a person group ({largePersonGroupId}).");
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SUBSCRIPTION_KEY);
            using (var content = new ByteArrayContent(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new Dictionary<string, object> { ["name"] = largePersonGroupId, ["recognitionModel"] = recognitionModel.ToString() }))))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                await httpClient.PutAsync($"{ENDPOINT}/face/v1.0/largepersongroups/{largePersonGroupId}", content);
            }
            // The similar faces will be grouped into a single large person group person.
            foreach (var groupedFace in personDictionary.Keys)
            {
                // Limit TPS
                await Task.Delay(250);
                string? personId = null;
                using (var content = new ByteArrayContent(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new Dictionary<string, object> { ["name"] = groupedFace }))))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    using (var response = await httpClient.PostAsync($"{ENDPOINT}/face/v1.0/largepersongroups/{largePersonGroupId}/persons", content))
                    {
                        string contentString = await response.Content.ReadAsStringAsync();
                        personId = (string?)(JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString)?["personId"]);
                    }
                }
                Console.WriteLine($"Create a person group person '{groupedFace}'.");

                // Add face to the large person group person.
                foreach (var similarImage in personDictionary[groupedFace])
                {
                    Console.WriteLine($"Check whether image is of sufficient quality for recognition");
                    Response<IReadOnlyList<FaceDetectionResult>> response = await client.DetectAsync(new Uri($"{url}{similarImage}"), FaceDetectionModel.Detection03, recognitionModel, returnFaceId: false, [FaceAttributeType.QualityForRecognition]);
                    IReadOnlyList<FaceDetectionResult> detectedFaces1 = response.Value;
                    bool sufficientQuality = true;
                    foreach (var face1 in detectedFaces1)
                    {
                        var faceQualityForRecognition = face1.FaceAttributes.QualityForRecognition;
                        //  Only "high" quality images are recommended for person enrollment
                        if (faceQualityForRecognition.HasValue && (faceQualityForRecognition.Value != QualityForRecognition.High))
                        {
                            sufficientQuality = false;
                            break;
                        }
                    }

                    if (!sufficientQuality)
                    {
                        continue;
                    }

                    if (detectedFaces1.Count != 1)
                    {
                        continue;
                    }

                    // add face to the large person group
                    Console.WriteLine($"Add face to the person group person({groupedFace}) from image `{similarImage}`");
                    using (var content = new ByteArrayContent(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new Dictionary<string, object> { ["url"] = $"{url}{similarImage}" }))))
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        await httpClient.PostAsync($"{ENDPOINT}/face/v1.0/largepersongroups/{largePersonGroupId}/persons/{personId}/persistedfaces?detectionModel=detection_03", content);
                    }
                }
            }

            // Start to train the large person group.
            Console.WriteLine();
            Console.WriteLine($"Train person group {largePersonGroupId}.");
            await httpClient.PostAsync($"{ENDPOINT}/face/v1.0/largepersongroups/{largePersonGroupId}/train", null);

            // Wait until the training is completed.
            while (true)
            {
                await Task.Delay(1000);
                string? trainingStatus = null;
                using (var response = await httpClient.GetAsync($"{ENDPOINT}/face/v1.0/largepersongroups/{largePersonGroupId}/training"))
                {
                    string contentString = await response.Content.ReadAsStringAsync();
                    trainingStatus = (string?)(JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString)?["status"]);
                }
                Console.WriteLine($"Training status: {trainingStatus}.");
                if ("succeeded".Equals(trainingStatus)) { break; }
            }
            Console.WriteLine();

            Console.WriteLine("Pausing for 60 seconds to avoid triggering rate limit on free account...");
            await Task.Delay(60000);

            List<Guid> sourceFaceIds = new List<Guid>();
            // Detect faces from source image url.
            List<FaceDetectionResult> detectedFaces = await DetectFaceRecognize(client, $"{url}{sourceImageFileName}", recognitionModel);

            // Add detected faceId to sourceFaceIds.
            foreach (var detectedFace in detectedFaces) { sourceFaceIds.Add(detectedFace.FaceId.Value); }

            // Identify the faces in a large person group.
            List<Dictionary<string, object>> identifyResults = new List<Dictionary<string, object>>();
            using (var content = new ByteArrayContent(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new Dictionary<string, object> { ["faceIds"] = sourceFaceIds, ["largePersonGroupId"] = largePersonGroupId }))))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                using (var response = await httpClient.PostAsync($"{ENDPOINT}/face/v1.0/identify", content))
                {
                    string contentString = await response.Content.ReadAsStringAsync();
                    identifyResults = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(contentString) ?? [];
                }
            }

            foreach (var identifyResult in identifyResults)
            {
                string faceId = (string)identifyResult["faceId"];
                List<Dictionary<string, object>> candidates = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(((JArray)identifyResult["candidates"]).ToString()) ?? [];
                if (candidates.Count == 0)
                {
                    Console.WriteLine($"No person is identified for the face in: {sourceImageFileName} - {faceId},");
                    continue;
                }

                string? personName = null;
                using (var response = await httpClient.GetAsync($"{ENDPOINT}/face/v1.0/largepersongroups/{largePersonGroupId}/persons/{candidates.First()["personId"]}"))
                {
                    string contentString = await response.Content.ReadAsStringAsync();
                    personName = (string?)(JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString)?["name"]);
                }
                Console.WriteLine($"Person '{personName}' is identified for the face in: {sourceImageFileName} - {faceId}," +
                    $" confidence: {candidates.First()["confidence"]}.");

                Dictionary<string, object> verifyResult = new Dictionary<string, object>();
                using (var content = new ByteArrayContent(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new Dictionary<string, object> { ["faceId"] = faceId, ["personId"] = candidates.First()["personId"], ["largePersonGroupId"] = largePersonGroupId }))))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    using (var response = await httpClient.PostAsync($"{ENDPOINT}/face/v1.0/verify", content))
                    {
                        string contentString = await response.Content.ReadAsStringAsync();
                        verifyResult = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString) ?? [];
                    }
                }
                Console.WriteLine($"Verification result: is a match? {verifyResult["isIdentical"]}. confidence: {verifyResult["confidence"]}");
            }
            Console.WriteLine();

            // Delete large person group.
            Console.WriteLine("========DELETE PERSON GROUP========");
            Console.WriteLine();
            await httpClient.DeleteAsync($"{ENDPOINT}/face/v1.0/largepersongroups/{largePersonGroupId}");
            Console.WriteLine($"Deleted the person group {largePersonGroupId}.");
            Console.WriteLine();
        }
    }
}
// </snippet_single>