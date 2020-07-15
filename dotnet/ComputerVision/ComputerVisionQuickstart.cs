// <snippet_using>
using System;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
// </snippet_using>

/*
 * Computer Vision SDK QuickStart
 *
 * Examples included:
 *  - Authenticate
 *  - Analyze Image with an image url
 *  - Analyze Image with a local file
 *  - Detect Objects with an image URL
 *  - Detect Objects with a local file
 *  - Generate Thumbnail from a URL and local image
 *  - Read Batch File recognizes both handwritten and printed text
 *  - Recognize Text from an image URL
 *  - Recognize Text from a a local image
 *  - Recognize Text OCR with an image URL
 *  - Recognize Text OCR with a local image
 *
 *  Prerequisites:
 *   - Visual Studio 2019 (or 2017, but note this is a .Net Core console app, not .Net Framework)
 *   - NuGet library: Microsoft.Azure.CognitiveServices.Vision.ComputerVision
 *   - Azure Computer Vision resource from https://ms.portal.azure.com
 *   - Create a .Net Core console app, then copy/paste this Program.cs file into it. Be sure to update the namespace if it's different.
 *   - At the top of your Program.cs, set your environment variables on your local machine with the suggested names.
 *   - Download local images (celebrities.jpg, objects.jpg, handwritten_text.jpg, and printed_text.jpg)
 *     from the link below then add to your bin/Debug/netcoreapp2.2 folder.
 *     https://github.com/Azure-Samples/cognitive-services-sample-data-files/tree/master/ComputerVision/Images
 *
 *   How to run:
 *    - Once your prerequisites are complete, press the Start button in Visual Studio.
 *    - Each example displays a printout of its results.
 *
 *   References:
 *    - .NET SDK: https://docs.microsoft.com/en-us/dotnet/api/overview/azure/cognitiveservices/client/computervision?view=azure-dotnet
 *    - API (testing console): https://westus.dev.cognitive.microsoft.com/docs/services/5adf991815e1060e6355ad44/operations/56f91f2e778daf14a499e1fa
 *    - Computer Vision documentation: https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/
 */

namespace ComputerVisionQuickstart
{
    class Program
    {
        // <snippet_vars>
        // Add your Computer Vision subscription key and endpoint to your environment variables. 
        // Close/reopen your project for them to take effect.
        static string subscriptionKey = Environment.GetEnvironmentVariable("COMPUTER_VISION_SUBSCRIPTION_KEY");
        static string endpoint = Environment.GetEnvironmentVariable("COMPUTER_VISION_ENDPOINT");
        // </snippet_vars>

        // Download these images (link in prerequisites), or you can use any appropriate image on your local machine.
        private const string ANALYZE_LOCAL_IMAGE = "celebrities.jpg";
        private const string DETECT_LOCAL_IMAGE = "objects.jpg";
        private const string DETECT_DOMAIN_SPECIFIC_LOCAL = "celebrities.jpg";
        private const string EXTRACT_TEXT_LOCAL_IMAGE = "handwritten_text.jpg";
        private const string OCR_LOCAL_IMAGE = "printed_text.jpg";

        // <snippet_analyze_url>
        // URL image used for analyzing an image (image of puppy)
        private const string ANALYZE_URL_IMAGE = "https://moderatorsampleimages.blob.core.windows.net/samples/sample16.png";
        // </snippet_analyze_url>
        // URL image for detecting objects (image of man on skateboard)
        private const string DETECT_URL_IMAGE = "https://moderatorsampleimages.blob.core.windows.net/samples/sample9.png";
        // URL image for detecting domain-specific content (image of ancient ruins)
        private const string DETECT_DOMAIN_SPECIFIC_URL = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/landmark.jpg";
        // URL image for extracting handwritten text.
        // <snippet_extracttext_url>
        private const string EXTRACT_TEXT_URL_HANDW = "https://raw.githubusercontent.com/MicrosoftDocs/azure-docs/master/articles/cognitive-services/Computer-vision/Images/readsample.jpg";
        // URL image for extracting printed text.
        private const string EXTRACT_TEXT_URL_PRINT = "https://intelligentkioskstore.blob.core.windows.net/visionapi/suggestedphotos/3.png";
        // </snippet_extracttext_url>
        // URL image for OCR (optical character recognition).
        private const string OCR_URL = "https://raw.githubusercontent.com/MicrosoftDocs/azure-docs/master/articles/cognitive-services/Computer-vision/Images/readsample.jpg";

        static void Main(string[] args)
        {
            Console.WriteLine("Azure Cognitive Services Computer Vision - .NET quickstart example");
            Console.WriteLine();

            // <snippet_client>
            // Create a client
            ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);
            // </snippet_client>

            // <snippet_analyzeinmain>
            // Analyze an image to get features and other properties.
            AnalyzeImageUrl(client, ANALYZE_URL_IMAGE).Wait();
            // </snippet_analyzeinmain>
            AnalyzeImageLocal(client, ANALYZE_LOCAL_IMAGE).Wait();

            // Detect objects in an image.
            DetectObjectsUrl(client, DETECT_URL_IMAGE).Wait();
            DetectObjectsLocal(client, DETECT_LOCAL_IMAGE).Wait();

            // Detect domain-specific content in both a URL image and a local image.
            DetectDomainSpecific(client, DETECT_DOMAIN_SPECIFIC_URL, DETECT_DOMAIN_SPECIFIC_LOCAL).Wait();

            // Generate a thumbnail image from a URL and local image
            GenerateThumbnail(client, ANALYZE_URL_IMAGE, DETECT_LOCAL_IMAGE).Wait();

            // <snippet_extracttextinmain>
            // Read the batch text from an image (handwriting and/or printed).
            BatchReadFileUrl(client, EXTRACT_TEXT_URL_HANDW).Wait();
            // </snippet_extracttextinmain>

            BatchReadFileLocal(client, EXTRACT_TEXT_LOCAL_IMAGE).Wait();

            // Recognize text using the OCR (optical character recognition) method.
            RecognizePrintedTextUrl(client, OCR_URL).Wait();
            RecognizePrintedTextLocal(client, OCR_LOCAL_IMAGE).Wait();

            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("Computer Vision quickstart is complete.");
            Console.WriteLine();
            Console.WriteLine("Press enter to exit...");
            Console.WriteLine();
        }

        // <snippet_auth>
        /*
         * AUTHENTICATE
         * Creates a Computer Vision client used by each example.
         */
        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
            return client;
        }
        // </snippet_auth>
        /*
         * END - Authenticate
         */

        // <snippet_visualfeatures>
        /* 
         * ANALYZE IMAGE - URL IMAGE
         * Analyze URL image. Extracts captions, categories, tags, objects, faces, racy/adult content,
         * brands, celebrities, landmarks, color scheme, and image types.
         */
        public static async Task AnalyzeImageUrl(ComputerVisionClient client, string imageUrl)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("ANALYZE IMAGE - URL");
            Console.WriteLine();

            // Creating a list that defines the features to be extracted from the image. 
            List<VisualFeatureTypes> features = new List<VisualFeatureTypes>()
        {
          VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
          VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
          VisualFeatureTypes.Tags, VisualFeatureTypes.Adult,
          VisualFeatureTypes.Color, VisualFeatureTypes.Brands,
          VisualFeatureTypes.Objects
        };
            // </snippet_visualfeatures>

            // <snippet_analyze_call>
            Console.WriteLine($"Analyzing the image {Path.GetFileName(imageUrl)}...");
            Console.WriteLine();
            // Analyze the URL image 
            ImageAnalysis results = await client.AnalyzeImageAsync(imageUrl, features);
            // </snippet_analyze_call>

            // <snippet_describe>
            // Sunmarizes the image content.
            Console.WriteLine("Summary:");
            foreach (var caption in results.Description.Captions)
            {
                Console.WriteLine($"{caption.Text} with confidence {caption.Confidence}");
            }
            Console.WriteLine();
            // </snippet_describe>

            // <snippet_categorize>
            // Display categories the image is divided into.
            Console.WriteLine("Categories:");
            foreach (var category in results.Categories)
            {
                Console.WriteLine($"{category.Name} with confidence {category.Score}");
            }
            Console.WriteLine();
            // </snippet_categorize>

            // <snippet_tags>
            // Image tags and their confidence score
            Console.WriteLine("Tags:");
            foreach (var tag in results.Tags)
            {
                Console.WriteLine($"{tag.Name} {tag.Confidence}");
            }
            Console.WriteLine();
            // </snippet_tags>

            // <snippet_objects>
            // Objects
            Console.WriteLine("Objects:");
            foreach (var obj in results.Objects)
            {
                Console.WriteLine($"{obj.ObjectProperty} with confidence {obj.Confidence} at location {obj.Rectangle.X}, " +
                  $"{obj.Rectangle.X + obj.Rectangle.W}, {obj.Rectangle.Y}, {obj.Rectangle.Y + obj.Rectangle.H}");
            }
            Console.WriteLine();
            // </snippet_objects>

            // <snippet_faces>
            // Faces
            Console.WriteLine("Faces:");
            foreach (var face in results.Faces)
            {
                Console.WriteLine($"A {face.Gender} of age {face.Age} at location {face.FaceRectangle.Left}, " +
                  $"{face.FaceRectangle.Left}, {face.FaceRectangle.Top + face.FaceRectangle.Width}, " +
                  $"{face.FaceRectangle.Top + face.FaceRectangle.Height}");
            }
            Console.WriteLine();
            // </snippet_faces>

            // <snippet_adult>
            // Adult or racy content, if any.
            Console.WriteLine("Adult:");
            Console.WriteLine($"Has adult content: {results.Adult.IsAdultContent} with confidence {results.Adult.AdultScore}");
            Console.WriteLine($"Has racy content: {results.Adult.IsRacyContent} with confidence {results.Adult.RacyScore}");
            Console.WriteLine();
            // </snippet_adult>

            // <snippet_brands>
            // Well-known (or custom, if set) brands.
            Console.WriteLine("Brands:");
            foreach (var brand in results.Brands)
            {
                Console.WriteLine($"Logo of {brand.Name} with confidence {brand.Confidence} at location {brand.Rectangle.X}, " +
                  $"{brand.Rectangle.X + brand.Rectangle.W}, {brand.Rectangle.Y}, {brand.Rectangle.Y + brand.Rectangle.H}");
            }
            Console.WriteLine();
            // </snippet_brands>

            // <snippet_celebs>
            // Celebrities in image, if any.
            Console.WriteLine("Celebrities:");
            foreach (var category in results.Categories)
            {
                if (category.Detail?.Celebrities != null)
                {
                    foreach (var celeb in category.Detail.Celebrities)
                    {
                        Console.WriteLine($"{celeb.Name} with confidence {celeb.Confidence} at location {celeb.FaceRectangle.Left}, " +
                          $"{celeb.FaceRectangle.Top}, {celeb.FaceRectangle.Height}, {celeb.FaceRectangle.Width}");
                    }
                }
            }
            Console.WriteLine();
            // </snippet_celebs>


            // <snippet_landmarks>
            // Popular landmarks in image, if any.
            Console.WriteLine("Landmarks:");
            foreach (var category in results.Categories)
            {
                if (category.Detail?.Landmarks != null)
                {
                    foreach (var landmark in category.Detail.Landmarks)
                    {
                        Console.WriteLine($"{landmark.Name} with confidence {landmark.Confidence}");
                    }
                }
            }
            Console.WriteLine();
            // </snippet_landmarks>

            // <snippet_color>
            // Identifies the color scheme.
            Console.WriteLine("Color Scheme:");
            Console.WriteLine("Is black and white?: " + results.Color.IsBWImg);
            Console.WriteLine("Accent color: " + results.Color.AccentColor);
            Console.WriteLine("Dominant background color: " + results.Color.DominantColorBackground);
            Console.WriteLine("Dominant foreground color: " + results.Color.DominantColorForeground);
            Console.WriteLine("Dominant colors: " + string.Join(",", results.Color.DominantColors));
            Console.WriteLine();
            // </snippet_color>

            // <snippet_type>
            // Detects the image types.
            Console.WriteLine("Image Type:");
            Console.WriteLine("Clip Art Type: " + results.ImageType.ClipArtType);
            Console.WriteLine("Line Drawing Type: " + results.ImageType.LineDrawingType);
            Console.WriteLine();
            // </snippet_type>
        }
        /*
         * END - ANALYZE IMAGE - URL IMAGE
         */

        /*
       * ANALYZE IMAGE - LOCAL IMAGE
	     * Analyze local image. Extracts captions, categories, tags, objects, faces, racy/adult content,
	     * brands, celebrities, landmarks, color scheme, and image types.
       */
        public static async Task AnalyzeImageLocal(ComputerVisionClient client, string localImage)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("ANALYZE IMAGE - LOCAL IMAGE");
            Console.WriteLine();

            // Creating a list that defines the features to be extracted from the image. 
            List<VisualFeatureTypes> features = new List<VisualFeatureTypes>()
        {
          VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
          VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
          VisualFeatureTypes.Tags, VisualFeatureTypes.Adult,
          VisualFeatureTypes.Color, VisualFeatureTypes.Brands,
          VisualFeatureTypes.Objects
        };

            Console.WriteLine($"Analyzing the local image {Path.GetFileName(localImage)}...");
            Console.WriteLine();

            using (Stream analyzeImageStream = File.OpenRead(localImage))
            {
                // Analyze the local image.
                ImageAnalysis results = await client.AnalyzeImageInStreamAsync(analyzeImageStream, features);

                // Sunmarizes the image content.
                Console.WriteLine("Summary:");
                foreach (var caption in results.Description.Captions)
                {
                    Console.WriteLine($"{caption.Text} with confidence {caption.Confidence}");
                }
                Console.WriteLine();

                // Display categories the image is divided into.
                Console.WriteLine("Categories:");
                foreach (var category in results.Categories)
                {
                    Console.WriteLine($"{category.Name} with confidence {category.Score}");
                }
                Console.WriteLine();

                // Image tags and their confidence score
                Console.WriteLine("Tags:");
                foreach (var tag in results.Tags)
                {
                    Console.WriteLine($"{tag.Name} {tag.Confidence}");
                }
                Console.WriteLine();

                // Objects
                Console.WriteLine("Objects:");
                foreach (var obj in results.Objects)
                {
                    Console.WriteLine($"{obj.ObjectProperty} with confidence {obj.Confidence} at location {obj.Rectangle.X}, " +
                      $"{obj.Rectangle.X + obj.Rectangle.W}, {obj.Rectangle.Y}, {obj.Rectangle.Y + obj.Rectangle.H}");
                }
                Console.WriteLine();

                // Detected faces, if any.
                Console.WriteLine("Faces:");
                foreach (var face in results.Faces)
                {
                    Console.WriteLine($"A {face.Gender} of age {face.Age} at location {face.FaceRectangle.Left}, {face.FaceRectangle.Top}, " +
                      $"{face.FaceRectangle.Left + face.FaceRectangle.Width}, {face.FaceRectangle.Top + face.FaceRectangle.Height}");
                }
                Console.WriteLine();

                // Adult or racy content, if any.
                Console.WriteLine("Adult:");
                Console.WriteLine($"Has adult content: {results.Adult.IsAdultContent} with confidence {results.Adult.AdultScore}");
                Console.WriteLine($"Has racy content: {results.Adult.IsRacyContent} with confidence {results.Adult.RacyScore}");
                Console.WriteLine();

                // Well-known brands, if any.
                Console.WriteLine("Brands:");
                foreach (var brand in results.Brands)
                {
                    Console.WriteLine($"Logo of {brand.Name} with confidence {brand.Confidence} at location {brand.Rectangle.X}, " +
                      $"{brand.Rectangle.X + brand.Rectangle.W}, {brand.Rectangle.Y}, {brand.Rectangle.Y + brand.Rectangle.H}");
                }
                Console.WriteLine();

                // Celebrities in image, if any.
                Console.WriteLine("Celebrities:");
                foreach (var category in results.Categories)
                {
                    if (category.Detail?.Celebrities != null)
                    {
                        foreach (var celeb in category.Detail.Celebrities)
                        {
                            Console.WriteLine($"{celeb.Name} with confidence {celeb.Confidence} at location {celeb.FaceRectangle.Left}, " +
                              $"{celeb.FaceRectangle.Top},{celeb.FaceRectangle.Height},{celeb.FaceRectangle.Width}");
                        }
                    }
                }
                Console.WriteLine();

                // Popular landmarks in image, if any.
                Console.WriteLine("Landmarks:");
                foreach (var category in results.Categories)
                {
                    if (category.Detail?.Landmarks != null)
                    {
                        foreach (var landmark in category.Detail.Landmarks)
                        {
                            Console.WriteLine($"{landmark.Name} with confidence {landmark.Confidence}");
                        }
                    }
                }
                Console.WriteLine();

                // Identifies the color scheme.
                Console.WriteLine("Color Scheme:");
                Console.WriteLine("Is black and white?: " + results.Color.IsBWImg);
                Console.WriteLine("Accent color: " + results.Color.AccentColor);
                Console.WriteLine("Dominant background color: " + results.Color.DominantColorBackground);
                Console.WriteLine("Dominant foreground color: " + results.Color.DominantColorForeground);
                Console.WriteLine("Dominant colors: " + string.Join(",", results.Color.DominantColors));
                Console.WriteLine();

                // Detects the image types.
                Console.WriteLine("Image Type:");
                Console.WriteLine("Clip Art Type: " + results.ImageType.ClipArtType);
                Console.WriteLine("Line Drawing Type: " + results.ImageType.LineDrawingType);
                Console.WriteLine();
            }
        }
        /*
         * END - ANALYZE IMAGE - LOCAL IMAGE
         */

        /* 
       * DETECT OBJECTS - URL IMAGE
       */
        public static async Task DetectObjectsUrl(ComputerVisionClient client, string urlImage)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("DETECT OBJECTS - URL IMAGE");
            Console.WriteLine();

            Console.WriteLine($"Detecting objects in URL image {Path.GetFileName(urlImage)}...");
            Console.WriteLine();
            // Detect the objects
            DetectResult detectObjectAnalysis = await client.DetectObjectsAsync(urlImage);

            // For each detected object in the picture, print out the bounding object detected, confidence of that detection and bounding box within the image
            Console.WriteLine("Detected objects:");
            foreach (var obj in detectObjectAnalysis.Objects)
            {
                Console.WriteLine($"{obj.ObjectProperty} with confidence {obj.Confidence} at location {obj.Rectangle.X}, " +
                  $"{obj.Rectangle.X + obj.Rectangle.W}, {obj.Rectangle.Y}, {obj.Rectangle.Y + obj.Rectangle.H}");
            }
            Console.WriteLine();
        }
        /*
         * END - DETECT OBJECTS - URL IMAGE
         */

        /*
         * DETECT OBJECTS - LOCAL IMAGE
         * This is an alternative way to detect objects, instead of doing so through AnalyzeImage.
         */
        public static async Task DetectObjectsLocal(ComputerVisionClient client, string localImage)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("DETECT OBJECTS - LOCAL IMAGE");
            Console.WriteLine();

            using (Stream stream = File.OpenRead(localImage))
            {
                // Make a call to the Computer Vision service using the local file
                DetectResult results = await client.DetectObjectsInStreamAsync(stream);

                Console.WriteLine($"Detecting objects in local image {Path.GetFileName(localImage)}...");
                Console.WriteLine();

                // For each detected object in the picture, print out the bounding object detected, confidence of that detection and bounding box within the image
                Console.WriteLine("Detected objects:");
                foreach (var obj in results.Objects)
                {
                    Console.WriteLine($"{obj.ObjectProperty} with confidence {obj.Confidence} at location {obj.Rectangle.X}, " +
                      $"{obj.Rectangle.X + obj.Rectangle.W}, {obj.Rectangle.Y}, {obj.Rectangle.Y + obj.Rectangle.H}");
                }
                Console.WriteLine();
            }
        }
        /*
         * END - DETECT OBJECTS - LOCAL IMAGE
         */

        /*
         * DETECT DOMAIN-SPECIFIC CONTENT
         * Recognizes landmarks or celebrities in an image.
         */
        public static async Task DetectDomainSpecific(ComputerVisionClient client, string urlImage, string localImage)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("DETECT DOMAIN-SPECIFIC CONTENT - URL & LOCAL IMAGE");
            Console.WriteLine();

            // Detect the domain-specific content in a URL image.
            DomainModelResults resultsUrl = await client.AnalyzeImageByDomainAsync("landmarks", urlImage);
            // Display results.
            Console.WriteLine($"Detecting landmarks in the URL image {Path.GetFileName(urlImage)}...");

            var jsonUrl = JsonConvert.SerializeObject(resultsUrl.Result);
            JObject resultJsonUrl = JObject.Parse(jsonUrl);
            Console.WriteLine($"Landmark detected: {resultJsonUrl["landmarks"][0]["name"]} " +
              $"with confidence {resultJsonUrl["landmarks"][0]["confidence"]}.");
            Console.WriteLine();

            // Detect the domain-specific content in a local image.
            using (Stream imageStream = File.OpenRead(localImage))
            {
                // Change "celebrities" to "landmarks" if that is the domain you are interested in.
                DomainModelResults resultsLocal = await client.AnalyzeImageByDomainInStreamAsync("celebrities", imageStream);
                Console.WriteLine($"Detecting celebrities in the local image {Path.GetFileName(localImage)}...");
                // Display results.
                var jsonLocal = JsonConvert.SerializeObject(resultsLocal.Result);
                JObject resultJsonLocal = JObject.Parse(jsonLocal);
                Console.WriteLine($"Celebrity detected: {resultJsonLocal["celebrities"][2]["name"]} " +
                  $"with confidence {resultJsonLocal["celebrities"][2]["confidence"]}");

                Console.WriteLine(resultJsonLocal);
            }
            Console.WriteLine();
        }
        /*
         * END - DETECT DOMAIN-SPECIFIC CONTENT
         */

        /*
         * GENERATE THUMBNAIL
         * Taking in a URL and local image, this example will generate a thumbnail image with specified width/height (pixels).
         * The thumbnail will be saved locally.
         */
        public static async Task GenerateThumbnail(ComputerVisionClient client, string urlImage, string localImage)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("GENERATE THUMBNAIL - URL & LOCAL IMAGE");
            Console.WriteLine();

            // Thumbnails will be saved locally in your bin\Debug\netcoreappx.x\ folder of this project.
            string localSavePath = @".";

            // URL
            Console.WriteLine("Generating thumbnail with URL image...");
            // Setting smartCropping to true enables the image to adjust its aspect ratio 
            // to center on the area of interest in the image. Change the width/height, if desired.
            Stream thumbnailUrl = await client.GenerateThumbnailAsync(60, 60, urlImage, true);

            string imageNameUrl = Path.GetFileName(urlImage);
            string thumbnailFilePathUrl = Path.Combine(localSavePath, imageNameUrl.Insert(imageNameUrl.Length - 4, "_thumb"));

            Console.WriteLine("Saving thumbnail from URL image to " + thumbnailFilePathUrl);
            using (Stream file = File.Create(thumbnailFilePathUrl)) { thumbnailUrl.CopyTo(file); }

            Console.WriteLine();

            // LOCAL
            Console.WriteLine("Generating thumbnail with local image...");

            using (Stream imageStream = File.OpenRead(localImage))
            {
                Stream thumbnailLocal = await client.GenerateThumbnailInStreamAsync(100, 100, imageStream, smartCropping: true);

                string imageNameLocal = Path.GetFileName(localImage);
                string thumbnailFilePathLocal = Path.Combine(localSavePath,
                        imageNameLocal.Insert(imageNameLocal.Length - 4, "_thumb"));
                // Save to file
                Console.WriteLine("Saving thumbnail from local image to " + thumbnailFilePathLocal);
                using (Stream file = File.Create(thumbnailFilePathLocal)) { thumbnailLocal.CopyTo(file); }
            }
            Console.WriteLine();
        }
        /*
         * END - GENERATE THUMBNAIL
         */

        // <snippet_extract_call>
        /*
         * BATCH READ FILE - URL IMAGE
         * Recognizes handwritten text. 
         * This API call offers an improvement of results over the Recognize Text calls.
         */
        public static async Task BatchReadFileUrl(ComputerVisionClient client, string urlImage)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("BATCH READ FILE - URL IMAGE");
            Console.WriteLine();

            // Read text from URL
            var textHeaders = await client.ReadAsync(urlImage, language: "en");
            // After the request, get the operation location (operation ID)
            string operationLocation = textHeaders.OperationLocation;
            // </snippet_extract_call>

            // <snippet_extract_response>
            // Retrieve the URI where the recognized text will be stored from the Operation-Location header.
            // We only need the ID and not the full URL
            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            // Extract the text
            // Delay is between iterations and tries a maximum of 10 times.
            int i = 0;
            int maxRetries = 10;
            ReadOperationResult results;
            Console.WriteLine($"Extracting text from URL image {Path.GetFileName(urlImage)}...");
            Console.WriteLine();
            do
            {
                results = await client.GetReadResultAsync(operationId);
                Console.WriteLine("Server status: {0}, waiting {1} seconds...", results.Status, i);
                await Task.Delay(1000);
                if (i == 9) 
                { 
                    Console.WriteLine("Server timed out."); 
                }
            }
            while ((results.Status == TextOperationStatusCodes.Running ||
                results.Status == TextOperationStatusCodes.NotStarted) && i++ < maxRetries);
            // </snippet_extract_response>

            // <snippet_extract_display>
            // Display the found text.
            Console.WriteLine();
            var textRecognitionLocalFileResults = results.RecognitionResults;
            foreach (TextRecognitionResult recResult in textRecognitionLocalFileResults)
            {
                foreach (Line line in recResult.Lines)
                {
                    Console.WriteLine(line.Text);
                }
            }
            Console.WriteLine();
        }
        // </snippet_extract_display>
        /*
         * END - BATCH READ FILE - URL IMAGE
         */

        /*
         * BATCH READ FILE - LOCAL IMAGE
         * This API call offers an improvement of results over the Recognize Text calls.
         */
        public static async Task BatchReadFileLocal(ComputerVisionClient client, string localImage)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("BATCH READ FILE - LOCAL IMAGE");
            Console.WriteLine();

            // Helps calucalte starting index to retrieve operation ID
            const int numberOfCharsInOperationId = 36;

            Console.WriteLine($"Extracting text from local image {Path.GetFileName(localImage)}...");
            Console.WriteLine();
            using (Stream imageStream = File.OpenRead(localImage))
            {
                // Read the text from the local image
                BatchReadFileInStreamHeaders localFileTextHeaders = await client.BatchReadFileInStreamAsync(imageStream);
                // Get the operation location (operation ID)
                string operationLocation = localFileTextHeaders.OperationLocation;

                // Retrieve the URI where the recognized text will be stored from the Operation-Location header.
                string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

                // Extract text, wait for it to complete.
                int i = 0;
                int maxRetries = 10;
                ReadOperationResult results;
                do
                {
                    results = await client.GetReadOperationResultAsync(operationId);
                    Console.WriteLine("Server status: {0}, waiting {1} seconds...", results.Status, i);
                    await Task.Delay(1000);
                    if (i == 9)
                    {
                        Console.WriteLine("Server timed out.");
                    }
                }
                while ((results.Status == TextOperationStatusCodes.Running ||
                    results.Status == TextOperationStatusCodes.NotStarted) && i++ < maxRetries);

                // Display the found text.
                Console.WriteLine();
                var textRecognitionLocalFileResults = results.RecognitionResults;
                foreach (TextRecognitionResult recResult in textRecognitionLocalFileResults)
                {
                    foreach (Line line in recResult.Lines)
                    {
                        Console.WriteLine(line.Text);
                    }
                }
                Console.WriteLine();
            }
        }
        /*
         * END - BATCH READ FILE - LOCAL IMAGE
         */

        /*
         * RECOGNIZE PRINTED TEXT - URL IMAGE
         */
        public static async Task RecognizePrintedTextUrl(ComputerVisionClient client, string imageUrl)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("RECOGNIZE PRINTED TEXT - URL IMAGE");
            Console.WriteLine();

            Console.WriteLine($"Performing OCR on URL image {Path.GetFileName(imageUrl)}...");
            Console.WriteLine();

            // Perform OCR on image
            OcrResult remoteOcrResult = await client.RecognizePrintedTextAsync(true, imageUrl);

            // Print the recognized text
            Console.WriteLine("Text:");
            Console.WriteLine("Language: " + remoteOcrResult.Language);
            Console.WriteLine("Text Angle: " + remoteOcrResult.TextAngle);
            Console.WriteLine("Orientation: " + remoteOcrResult.Orientation);
            Console.WriteLine();
            Console.WriteLine("Text regions: ");
            foreach (var remoteRegion in remoteOcrResult.Regions)
            {
                Console.WriteLine("Region bounding box: " + remoteRegion.BoundingBox);
                foreach (var line in remoteRegion.Lines)
                {
                    Console.WriteLine("Line bounding box: " + line.BoundingBox);

                    foreach (var word in line.Words)
                    {
                        Console.WriteLine("Word bounding box: " + word.BoundingBox);
                        Console.WriteLine("Text: " + word.Text);
                    }
                    Console.WriteLine();
                }
            }
        }
        /*
         * END - RECOGNIZE PRINTED TEXT - URL IMAGE
         */

        /*
         * RECOGNIZE PRINTED TEXT - LOCAL IMAGE
         */
        public static async Task RecognizePrintedTextLocal(ComputerVisionClient client, string localImage)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("RECOGNIZE PRINTED TEXT - LOCAL IMAGE");
            Console.WriteLine();

            using (Stream stream = File.OpenRead(localImage))
            {
                Console.WriteLine($"Performing OCR on local image {Path.GetFileName(localImage)}...");
                Console.WriteLine();
                // Get the recognized text
                OcrResult localFileOcrResult = await client.RecognizePrintedTextInStreamAsync(true, stream);

                // Display text, language, angle, orientation, and regions of text from the results.
                Console.WriteLine("Text:");
                Console.WriteLine("Language: " + localFileOcrResult.Language);
                Console.WriteLine("Text Angle: " + localFileOcrResult.TextAngle);
                Console.WriteLine("Orientation: " + localFileOcrResult.Orientation);
                Console.WriteLine();
                Console.WriteLine("Text regions: ");

                // Getting only one line of text for testing purposes. To see full demonstration, remove the counter & conditional.
                int counter = 0;
                foreach (var localRegion in localFileOcrResult.Regions)
                {
                    Console.WriteLine("Region bounding box: " + localRegion.BoundingBox);
                    foreach (var line in localRegion.Lines)
                    {
                        Console.WriteLine("Line bounding box: " + line.BoundingBox);
                        if (counter == 1)
                        {
                            Console.WriteLine();
                            return;
                        }
                        counter++;
                        foreach (var word in line.Words)
                        {
                            Console.WriteLine("Word bounding box: " + word.BoundingBox);
                            Console.WriteLine("Text: " + word.Text);
                        }
                    }
                }
            }
        }
        /*
         * END - RECOGNIZE PRINTED TEXT - LOCAL IMAGE
         */
    }
}
