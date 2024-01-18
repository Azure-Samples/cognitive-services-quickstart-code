// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// DESCRIPTION:
//     This sample demonstrates how to generate a human-readable sentence that describes the content
//     of a publicly accessible image URL, using a synchronous client.
//
//     By default the caption may contain gender terms such as "man", "woman", or "boy", "girl".
//     You have the option to request gender-neutral terms such as "person" or "child" by setting
//     `genderNeutralCaption` to `true` when calling `analyze`, as shown in this example.
//
//     The synchronous (blocking) `analyze` method call returns an `ImageAnalysisResult` object.
//     A call to `getCaption()` on this result will return a `CaptionResult` object. It contains:
//     - The text of the caption. Captions are only supported in English at the moment. 
//     - A confidence score in the range [0, 1], with higher values indicating greater confidences in
//       the caption.
//
// USAGE:
//     Compile the sample:
//         mvn clean dependency:copy-dependencies
//         javac SampleCaptionImageUrl.java -cp target\dependency\*
//     Run the sample:
//         java -cp ".;target\dependency\*" SampleCaptionImageUrl
//
//     Set these two environment variables before running the sample:
//     1) VISION_ENDPOINT - Your endpoint URL, in the form https://your-resource-name.cognitiveservices.azure.com
//                          where `your-resource-name` is your unique Azure Computer Vision resource name.
//     2) VISION_KEY - Your Computer Vision key (a 32-character Hexadecimal number)


// <snippet_single>
import com.azure.ai.vision.imageanalysis.ImageAnalysisClient;
import com.azure.ai.vision.imageanalysis.ImageAnalysisClientBuilder;
import com.azure.core.credential.KeyCredential;

import com.azure.ai.vision.imageanalysis.models.ImageAnalysisOptions;
import com.azure.ai.vision.imageanalysis.models.ImageAnalysisResult;
import com.azure.ai.vision.imageanalysis.models.VisualFeatures;
import com.azure.ai.vision.imageanalysis.models.DetectedTextLine;
import com.azure.ai.vision.imageanalysis.models.DetectedTextWord;

import java.net.URL;
import java.util.Arrays;

public class ImageAnalysis {

    public static void main(String[] args) {

        String endpoint = System.getenv("VISION_ENDPOINT");
        String key = System.getenv("VISION_KEY");

        if (endpoint == null || key == null) {
            System.out.println("Missing environment variable 'VISION_ENDPOINT' or 'VISION_KEY'.");
            System.out.println("Set them before running this sample.");
            System.exit(1);
        }

        // Create a synchronous Image Analysis client.
        ImageAnalysisClient client = new ImageAnalysisClientBuilder()
            .endpoint(endpoint)
            .credential(new KeyCredential(key))
            .buildClient();

        // This is a synchronous (blocking) call.
        ImageAnalysisResult result = client.analyze(
            new URL("https://learn.microsoft.com/azure/ai-services/computer-vision/media/quickstarts/presentation.png"), // imageUrl: the URL of the image to analyze
            Arrays.asList(VisualFeatures.CAPTION, VisualFeatures.READ), // visualFeatures
            new ImageAnalysisOptions().setGenderNeutralCaption(true)); // options:  Set to 'true' or 'false' (relevant for CAPTION or DENSE_CAPTIONS visual features)

        // Print analysis results to the console
        System.out.println("Image analysis results:");
        System.out.println(" Caption:");
        System.out.println("   \"" + result.getCaption().getText() + "\", Confidence "
            + String.format("%.4f", result.getCaption().getConfidence()));
        System.out.println(" Read:");
        for (DetectedTextLine line : result.getRead().getBlocks().get(0).getLines()) {
            System.out.println("   Line: '" + line.getText()
                + "', Bounding polygon " + line.getBoundingPolygon());
            for (DetectedTextWord word : line.getWords()) {
                System.out.println("     Word: '" + word.getText()
                    + "', Bounding polygon " + word.getBoundingPolygon()
                    + ", Confidence " + String.format("%.4f", word.getConfidence()));
            }
        }
    }
}
// </snippet_single>
