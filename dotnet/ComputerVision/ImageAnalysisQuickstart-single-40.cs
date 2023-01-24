//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
//
// Azure AI Vision SDK -- C# Image Analysis Quickstart
//

// <snippet_single>
using Azure.AI.Vision.Core.Input;
using Azure.AI.Vision.Core.Options;
using Azure.AI.Vision.ImageAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    public static async Task Main()
    {
        // Replace the string "PASTE_YOUR_COMPUTER_VISION_ENDPOINT_HERE" with your Computer Vision endpoint, found in the Azure portal.
        // The endpoint has the form "https://<your-computer-vision-resource-name>.cognitiveservices.azure.com".
        // Replace the string "PASTE_YOUR_COMPUTER_VISION_SUBSCRIPTION_KEY_HERE" with your Computer Vision key. The key is a 32-character
        // HEX number (no dashes), found in the Azure portal. Similar to "d0dbd4c2a93346f18c785a426da83e15".
        var serviceOptions = new VisionServiceOptions("PASTE_YOUR_COMPUTER_VISION_ENDPOINT_HERE", "PASTE_YOUR_COMPUTER_VISION_SUBSCRIPTION_KEY_HERE");

        // Specify the URL of the image to analyze
        var imageSource = VisionSource.FromUrl(new Uri("https://moderatorsampleimages.blob.core.windows.net/samples/sample16.png"));

        // Specify the options that will control the ImageAnalyzer
        var analysisOptions = new ImageAnalysisOptions()
        {
            // You must define one or more features to extract during image analysis
            Features = ImageAnalysisFeature.Tags
        };

        using var analyzer = new ImageAnalyzer(serviceOptions, imageSource, analysisOptions);

        Console.WriteLine("Please wait for image analysis results...");
        var result = await analyzer.AnalyzeAsync();

        if (result.Reason == ImageAnalysisResultReason.Analyzed)
        {
            Console.WriteLine($" Tags:");
            foreach (var tag in e.Result.Tags)
            {
                Console.WriteLine($"   \"{tag.Name}\", Confidence {tag.Confidence:0.0000}");
            }

        }
        else if (result.Reason == ImageAnalysisResultReason.Error)
        {
            Console.WriteLine(" Analysis failed.");

                var errorDetails = ImageAnalysisErrorDetails.FromResult(e.Result);
                Console.WriteLine($"   Error reason : {errorDetails.Reason}");
                Console.WriteLine($"   Error code : {errorDetails.ErrorCode}");
                Console.WriteLine($"   Error message: {errorDetails.Message}");
                Console.WriteLine(" Did you set the computer vision endpoint and key?");
        }
    }
}
// </snippet_single>