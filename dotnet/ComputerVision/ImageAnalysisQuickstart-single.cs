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
 *    - API (testing console): https://westus.dev.cognitive.microsoft.com/docs/services/computer-vision-v3-2/operations/5d986960601faab4bf452005
 *    - Computer Vision documentation: https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/
 */

// <snippet_single>
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

namespace ComputerVisionQuickstart
{
    class Program
    {
        // Add your Computer Vision key and endpoint
        static string key = Environment.GetEnvironmentVariable("VISION_KEY");
        static string endpoint = Environment.GetEnvironmentVariable("VISION_ENDPOINT");

        // URL image used for analyzing an image (image of puppy)
        private const string ANALYZE_URL_IMAGE = "https://moderatorsampleimages.blob.core.windows.net/samples/sample16.png";

        static void Main(string[] args)
        {
            Console.WriteLine("Azure Cognitive Services Computer Vision - .NET quickstart example");
            Console.WriteLine();

            // Create a client
            ComputerVisionClient client = Authenticate(endpoint, key);

            // Analyze an image to get features and other properties.
            AnalyzeImageUrl(client, ANALYZE_URL_IMAGE).Wait();
        }

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
       
        public static async Task AnalyzeImageUrl(ComputerVisionClient client, string imageUrl)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("ANALYZE IMAGE - URL");
            Console.WriteLine();

            // Creating a list that defines the features to be extracted from the image. 

            List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Tags
            };

            Console.WriteLine($"Analyzing the image {Path.GetFileName(imageUrl)}...");
            Console.WriteLine();
            // Analyze the URL image 
            ImageAnalysis results = await client.AnalyzeImageAsync(imageUrl, visualFeatures: features);

            // Image tags and their confidence score
            Console.WriteLine("Tags:");
            foreach (var tag in results.Tags)
            {
                Console.WriteLine($"{tag.Name} {tag.Confidence}");
            }
            Console.WriteLine();
        }
    }
}
// </snippet_single>