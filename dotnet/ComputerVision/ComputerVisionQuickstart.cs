// <snippet_using>
using System;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Linq;
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
        // Add your Computer Vision subscription key and endpoint
        static string subscriptionKey = "COMPUTER_VISION_SUBSCRIPTION_KEY";
        static string endpoint = "COMPUTER_VISION_ENDPOINT";
        // </snippet_vars>

        // Download these images (link in prerequisites), or you can use any appropriate image on your local machine.
        private const string ANALYZE_LOCAL_IMAGE = "celebrities.jpg";
        private const string DETECT_LOCAL_IMAGE = "objects.jpg";
        private const string DETECT_DOMAIN_SPECIFIC_LOCAL = "celebrities.jpg";
        private const string READ_TEXT_LOCAL_IMAGE = "print_text.png";

        // <snippet_analyze_url>
        // URL image used for analyzing an image (image of puppy)
        private const string ANALYZE_URL_IMAGE = "https://moderatorsampleimages.blob.core.windows.net/samples/sample16.png";
        // </snippet_analyze_url>
        // URL image for detecting objects (image of man on skateboard)
        private const string DETECT_URL_IMAGE = "https://moderatorsampleimages.blob.core.windows.net/samples/sample9.png";
        // URL image for detecting domain-specific content (image of ancient ruins)
        private const string DETECT_DOMAIN_SPECIFIC_URL = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/landmark.jpg";
        // <snippet_readtext_url>
        private const string READ_TEXT_URL_IMAGE = "https://intelligentkioskstore.blob.core.windows.net/visionapi/suggestedphotos/3.png";
        // </snippet_readtext_url>
       
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
            // Extract text (OCR) from a URL image using the Read API
            ReadFileUrl(client, READ_TEXT_URL_IMAGE).Wait();
            // Extract text (OCR) from a local image using the Read API
            ReadFileLocal(client, READ_TEXT_LOCAL_IMAGE).Wait();
            // </snippet_extracttextinmain>

            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("Computer Vision quickstart is complete.");
            Console.WriteLine();
            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
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

            List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
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
                ImageAnalysis results = await client.AnalyzeImageInStreamAsync(analyzeImageStream);

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

        // <snippet_read_url>
        /*
         * READ FILE - URL 
         * Extracts text. 
         */
        public static async Task ReadFileUrl(ComputerVisionClient client, string urlFile)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("READ FILE FROM URL");
            Console.WriteLine();

            // Read text from URL
            var textHeaders = await client.ReadAsync(urlFile, language: "en");
            // After the request, get the operation location (operation ID)
            string operationLocation = textHeaders.OperationLocation;
            Thread.Sleep(2000);
            // </snippet_extract_call>

            // <snippet_read_response>
            // Retrieve the URI where the extracted text will be stored from the Operation-Location header.
            // We only need the ID and not the full URL
            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            // Extract the text
            ReadOperationResult results;
            Console.WriteLine($"Extracting text from URL file {Path.GetFileName(urlFile)}...");
            Console.WriteLine();
            do
            {
                results = await client.GetReadResultAsync(Guid.Parse(operationId));
            }
            while ((results.Status == OperationStatusCodes.Running ||
                results.Status == OperationStatusCodes.NotStarted));
            // </snippet_read_response>

            // <snippet_read_display>
            // Display the found text.
            Console.WriteLine();
            var textUrlFileResults = results.AnalyzeResult.ReadResults;
            foreach (ReadResult page in textUrlFileResults)
            {
                foreach (Line line in page.Lines)
                {
                    Console.WriteLine(line.Text);
                }
            }
	    // </snippet_read_display>
            Console.WriteLine();
        }


        // </snippet_read_url>
        /*
         * END - READ FILE - URL
         */


        // <snippet_read_local>
        /*
         * READ FILE - LOCAL
         */

        public static async Task ReadFileLocal(ComputerVisionClient client, string localFile)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("READ FILE FROM LOCAL");
            Console.WriteLine();

            // Read text from URL
            var textHeaders = await client.ReadInStreamAsync(File.OpenRead(localFile), language: "en");
            // After the request, get the operation location (operation ID)
            string operationLocation = textHeaders.OperationLocation;
            Thread.Sleep(2000);
            // </snippet_extract_call>

            // <snippet_extract_response>
            // Retrieve the URI where the recognized text will be stored from the Operation-Location header.
            // We only need the ID and not the full URL
            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            // Extract the text
            ReadOperationResult results;
            Console.WriteLine($"Reading text from local file {Path.GetFileName(localFile)}...");
            Console.WriteLine();
            do
            {
                results = await client.GetReadResultAsync(Guid.Parse(operationId));
            }
            while ((results.Status == OperationStatusCodes.Running ||
                results.Status == OperationStatusCodes.NotStarted));
            // </snippet_extract_response>

            // <snippet_extract_display>
            // Display the found text.
            Console.WriteLine();
            var textUrlFileResults = results.AnalyzeResult.ReadResults;
            foreach (ReadResult page in textUrlFileResults)
            {
                foreach (Line line in page.Lines)
                {
                        Console.WriteLine(line.Text);
                }
            }
            Console.WriteLine();
        }
        /*
         * END - READ FILE - LOCAL
         */


    }
}
