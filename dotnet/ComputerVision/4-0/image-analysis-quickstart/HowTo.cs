// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using NUnit.Framework;
using Azure.AI.Vision.ImageAnalysis;

public class HowTo
{
    static void AnalyzeImage() {

        // <snippet_client>
        string endpoint = Environment.GetEnvironmentVariable("VISION_ENDPOINT");
        string key = Environment.GetEnvironmentVariable("VISION_KEY");

        // Create an Image Analysis client.
        ImageAnalysisClient client = new ImageAnalysisClient(new Uri(endpoint), new AzureKeyCredential(key));
        // </snippet_client>

        // <snippet_url>
        Uri imageURL = new Uri("https://aka.ms/azai/vision/image-analysis-sample.jpg");
        // </snippet_url>

        // <snippet_file>
        using FileStream stream = new FileStream("image-analysis-sample.jpg", FileMode.Open);
        BinaryData imageStream = BinaryData.FromStream(stream)
        // </snippet_file>

        // <snippet_features>
        List visualFeatures = [
            VisualFeatures.Caption, 
            VisualFeatures.DenseCaptions,
            VisualFeatures.Objects,
            VisualFeatures.Read,
            VisualFeatures.Tags,
            VisualFeatures.People,
            VisualFeatures.SmartCrops];
        // </snippet_features>

        // <snippet_options>
        ImageAnalysisOptions options = new ImageAnalysisOptions { 
            GenderNeutralCaption = true, 
            language="en",
            SmartCropsAspectRatios = new float[] { 0.9F, 1.33F }};
        // </snippet_options>

        // <snippet_call>
        ImageAnalysisResult result = client.Analyze(
            imageURL,
            visualFeatures,
            options);
        // </snippet_call>

        // <snippet_results>
        Console.WriteLine($"Image analysis results:");
        Console.WriteLine($" Caption:");
        Console.WriteLine($"   '{result.Caption.Text}', Confidence {result.Caption.Confidence:F4}");

        // Print dense caption results to the console
        Console.WriteLine($"Image analysis results:");
        Console.WriteLine($" Dense Captions:");
        foreach (DenseCaption denseCaption in result.DenseCaptions.Values)
        {
            Console.WriteLine($"   Region: '{denseCaption.Text}', Confidence {denseCaption.Confidence:F4}, Bounding box {denseCaption.BoundingBox}");
        }

        // Print object detection results to the console
        Console.WriteLine($"Image analysis results:");
        Console.WriteLine($" Objects:");
        foreach (DetectedObject detectedObject in result.Objects.Values)
        {
            Console.WriteLine($"   Object: '{detectedObject.Tags.First().Name}', Bounding box {detectedObject.BoundingBox.ToString()}");
        }

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

        // Print tags results to the console
        Console.WriteLine($"Image analysis results:");
        Console.WriteLine($" Tags:");
        foreach (DetectedTag tag in result.Tags.Values)
        {
            Console.WriteLine($"   '{tag.Name}', Confidence {tag.Confidence:F4}");
        }

        // Print people detection results to the console
        Console.WriteLine($"Image analysis results:");
        Console.WriteLine($" People:");
        foreach (DetectedPerson person in result.People.Values)
        {
            Console.WriteLine($"   Person: Bounding box {person.BoundingBox.ToString()}, Confidence {person.Confidence:F4}");
        }

        // Print smart-crops analysis results to the console
        Console.WriteLine($"Image analysis results:");
        Console.WriteLine($" SmartCrops:");
        foreach (CropRegion cropRegion in result.SmartCrops.Values)
        {
            Console.WriteLine($"   Aspect ratio: {cropRegion.AspectRatio}, Bounding box: {cropRegion.BoundingBox}");
        }
        // </snippet_results>
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
