// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <snippet_single>
using Azure;
using Azure.AI.Vision.ImageAnalysis;
using System;

public class Program
{
    static void AnalyzeImage()
    {
        string endpoint = Environment.GetEnvironmentVariable("VISION_ENDPOINT");
        string key = Environment.GetEnvironmentVariable("VISION_KEY");

        ImageAnalysisClient client = new ImageAnalysisClient(
            new Uri(endpoint),
            new AzureKeyCredential(key));

        ImageAnalysisResult result = client.Analyze(
            new Uri("https://learn.microsoft.com/azure/ai-services/computer-vision/media/quickstarts/presentation.png"),
            VisualFeatures.Caption | VisualFeatures.Read,
            new ImageAnalysisOptions { GenderNeutralCaption = true });

        Console.WriteLine("Image analysis results:");
        Console.WriteLine(" Caption:");
        Console.WriteLine($"   '{result.Caption.Text}', Confidence {result.Caption.Confidence:F4}");

        Console.WriteLine(" Read:");
        foreach (DetectedTextBlock block in result.Read.Blocks)
            foreach (DetectedTextLine line in block.Lines)
            {
                Console.WriteLine($"   Line: '{line.Text}', Bounding Polygon: [{string.Join(" ", line.BoundingPolygon)}]");
                foreach (DetectedTextWord word in line.Words)
                {
                    Console.WriteLine($"     Word: '{word.Text}', Confidence {word.Confidence.ToString("#.####")}, Bounding Polygon: [{string.Join(" ", word.BoundingPolygon)}]");
                }
            }
    }

    static void Main()
    {
        try
        {
            AnalyzeImage();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
// </snippet_single>
