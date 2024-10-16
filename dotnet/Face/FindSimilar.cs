// <snippet_using>
using Azure;
using Azure.AI.Vision.Face;
// </snippet_using>

namespace FaceQuickstart
{
    class Program
    {
        // <snippet_image_url>
        const string IMAGE_BASE_URL = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/";
        // </snippet_image_url>

        // <snippet_creds>
        static readonly string SUBSCRIPTION_KEY = Environment.GetEnvironmentVariable("FACE_APIKEY") ?? "<apikey>";
        static readonly string ENDPOINT = Environment.GetEnvironmentVariable("FACE_ENDPOINT") ?? "<endpoint>";
        // </snippet_creds>

        static void Main(string[] args)
        {

            // <snippet_detect_models>
            FaceRecognitionModel RECOGNITION_MODEL4 = FaceRecognitionModel.Recognition04;
            // </snippet_detect_models>

            // <snippet_maincalls>
            FaceClient client = Authenticate(ENDPOINT, SUBSCRIPTION_KEY);
            FindSimilar(client, IMAGE_BASE_URL, RECOGNITION_MODEL4).Wait();
            // </snippet_maincalls>
        }

        // <snippet_auth>
        public static FaceClient Authenticate(string endpoint, string key)
        {
            return new FaceClient(new Uri(endpoint), new AzureKeyCredential(key));
        }
        // </snippet_auth>

        // <snippet_face_detect_recognize>
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
        // </snippet_face_detect_recognize>

        public static async Task FindSimilar(FaceClient client, string base_url, FaceRecognitionModel recognition_model)
        {
            // <snippet_loadfaces>
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
            IList<Guid> targetFaceIds = new List<Guid>();
            foreach (var targetImageFileName in targetImageFileNames)
            {
                // Detect faces from target image url.
                var faces = await DetectFaceRecognize(client, $"{base_url}{targetImageFileName}", recognition_model);
                // Add detected faceId to list of GUIDs.
                targetFaceIds.Add(faces[0].FaceId.Value);
            }

            // Detect faces from source image url.
            IList<FaceDetectionResult> detectedFaces = await DetectFaceRecognize(client, $"{base_url}{sourceImageFileName}", recognition_model);
            Console.WriteLine();
            // </snippet_loadfaces>

            // <snippet_find_similar>
            // Find a similar face(s) in the list of IDs. Comapring only the first in list for testing purposes.
            Response<IReadOnlyList<FaceFindSimilarResult>> response = await client.FindSimilarAsync(detectedFaces[0].FaceId.Value, targetFaceIds);
            IList<FaceFindSimilarResult> similarResults = response.Value.ToList();
            // </snippet_find_similar>
            // <snippet_find_similar_print>
            foreach (var similarResult in similarResults)
            {
                Console.WriteLine($"Faces from {sourceImageFileName} & ID:{similarResult.FaceId} are similar with confidence: {similarResult.Confidence}.");
            }
            Console.WriteLine();
            // </snippet_find_similar_print>
        }
    }
}