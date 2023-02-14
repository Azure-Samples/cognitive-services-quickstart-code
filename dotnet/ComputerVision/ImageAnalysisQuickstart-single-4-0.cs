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
        var serviceOptions = new VisionServiceOptions("PASTE_YOUR_COMPUTER_VISION_ENDPOINT_HERE", "PASTE_YOUR_COMPUTER_VISION_SUBSCRIPTION_KEY_HERE");

        
        // Specify the URL of the image to analyze
        var imageSource = VisionSource.FromUrl(new Uri("https://learn.microsoft.com/azure/cognitive-services/computer-vision/media/quickstarts/presentation.png"));

        // Specify the options that will control the ImageAnalyzer
        var analysisOptions = new ImageAnalysisOptions()
        {
            // You must define one or more features to extract during image analysis
            Features = ImageAnalysisFeature.Caption
            | ImageAnalysisFeature.Text
        };

        var analyzer = new ImageAnalyzer(serviceOptions, imageSource, analysisOptions);

        Console.WriteLine("Please wait for image analysis results...");
        var result = await analyzer.AnalyzeAsync();

        if (result.Reason == ImageAnalysisResultReason.Analyzed)
        {
            if (result.Caption != null)
            {
                Console.WriteLine(" Caption:");
                Console.WriteLine($"   \"{result.Caption.Content}\", Confidence {result.Caption.Confidence:0.0000}");
            }
        }
        if (result.Text != null)
        {
            Console.WriteLine($" Text:");
            foreach (var line in result.Text.Lines)
            {
                string pointsToString = "{" + string.Join(',', line.BoundingPolygon.Select(pointsToString => pointsToString.ToString())) + "}";
                Console.WriteLine($"   Line: '{line.Content}', Bounding polygon {pointsToString}");

                foreach (var word in line.Words)
                {
                    pointsToString = "{" + string.Join(',', word.BoundingPolygon.Select(pointsToString => pointsToString.ToString())) + "}";
                    Console.WriteLine($"     Word: '{word.Content}', Bounding polygon {pointsToString}, Confidence {word.Confidence:0.0000}");
                }
            }
        }
        else if (result.Reason == ImageAnalysisResultReason.Error)
        {
            var errorDetails = ImageAnalysisErrorDetails.FromResult(result);
            Console.WriteLine(" Analysis failed.");
            Console.WriteLine($"   Error reason : {errorDetails.Reason}");
            Console.WriteLine($"   Error code : {errorDetails.ErrorCode}");
            Console.WriteLine($"   Error message: {errorDetails.Message}");
            Console.WriteLine(" Did you set the computer vision endpoint and key?");
        }
    }
}
// </snippet_single>