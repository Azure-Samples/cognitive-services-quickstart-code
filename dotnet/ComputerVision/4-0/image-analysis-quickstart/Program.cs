// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using NUnit.Framework;


public class Program
{

    static void AnalyzeImage(){
    string endpoint = Environment.GetEnvironmentVariable("VISION_ENDPOINT");
    string key = Environment.GetEnvironmentVariable("VISION_KEY");

    // Create an Image Analysis client.
    ImageAnalysisClient client = new ImageAnalysisClient(new Uri(endpoint), new AzureKeyCredential(key));

    List visualFeatures = [VisualFeatures.Caption, VisualFeatures.Read];
    Uri imageURL = new Uri("https://aka.ms/azsdk/image-analysis/sample.jpg");

    ImageAnalysisResult result = client.Analyze(
        imageURL,
        visualFeatures,
        new ImageAnalysisOptions { GenderNeutralCaption = true });

    Console.WriteLine($"Image analysis results:");
    Console.WriteLine($" Caption:");
    Console.WriteLine($"   '{result.Caption.Text}', Confidence {result.Caption.Confidence:F4}");

    // Print text (OCR) analysis results to the console
    Console.WriteLine("Image analysis results:");
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
