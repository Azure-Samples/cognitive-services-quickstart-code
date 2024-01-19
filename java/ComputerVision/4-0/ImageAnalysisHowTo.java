// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// USAGE:
//     Compile the sample:
//         mvn clean dependency:copy-dependencies
//         javac ImageAnalysisHowTo.java -cp target\dependency\*
//     Run the sample:
//         java -cp ".;target\dependency\*" ImageAnalysisHowTo
//
//     Set these two environment variables before running the sample:
//     1) VISION_ENDPOINT - Your endpoint URL, in the form https://your-resource-name.cognitiveservices.azure.com
//                          where `your-resource-name` is your unique Azure Computer Vision resource name.
//     2) VISION_KEY - Your Computer Vision key (a 32-character Hexadecimal number)

import com.azure.ai.vision.imageanalysis.*;
import com.azure.ai.vision.imageanalysis.models.*;
import com.azure.core.credential.KeyCredential;
import com.azure.core.exception.HttpResponseException;
import com.azure.core.util.BinaryData;
import java.io.File;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.Arrays;
import java.util.List;

public class ImageAnalysisHowTo {

    public static void main(String[] args) throws MalformedURLException {

        // <snippet_client>
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
        // </snippet_client>

        // <snippet_file>
        BinaryData imageData = BinaryData.fromFile(new File("sample.png").toPath());
        // </snippet_file>
        // <snippet_url>
        URL imageURL = new URL("https://learn.microsoft.com/azure/ai-services/computer-vision/media/quickstarts/presentation.png");
        // </snippet_url>

        // <snippet_options>
        // Specify analysis options (or set `options` to null for defaults)
        ImageAnalysisOptions options = new ImageAnalysisOptions()
            .setLanguage("en")
            .setGenderNeutralCaption(true)
            .setSmartCropsAspectRatios(Arrays.asList(0.9, 1.33, 1.78));
        // </snippet_options>

        // <snippet_features>
        // visualFeatures: Select one or more visual features to analyze.
        List<VisualFeatures> visualFeatures = Arrays.asList(
                    VisualFeatures.SMART_CROPS,
                    VisualFeatures.CAPTION,
                    VisualFeatures.DENSE_CAPTIONS,
                    VisualFeatures.OBJECTS,
                    VisualFeatures.PEOPLE,
                    VisualFeatures.READ,
                    VisualFeatures.TAGS);
        // </snippet_features>

        // <snippet_call>
        try {
            // Analyze all visual features from an image URL. This is a synchronous (blocking) call.
            ImageAnalysisResult result = client.analyze(
                imageURL,
                visualFeatures,
                options);

            printAnalysisResults(result);

        } catch (HttpResponseException e) {
            System.out.println("Exception: " + e.getClass().getSimpleName());
            System.out.println("Status code: " + e.getResponse().getStatusCode());
            System.out.println("Message: " + e.getMessage());
        } catch (Exception e) {
            System.out.println("Message: " + e.getMessage());
        }
        // </snippet_call>
    }

    // <snippet_results>
    // Print all analysis results to the console
    public static void printAnalysisResults(ImageAnalysisResult result) {

        System.out.println("Image analysis results:");

        if (result.getCaption() != null) {
            System.out.println(" Caption:");
            System.out.println("   \"" + result.getCaption().getText() + "\", Confidence "
                + String.format("%.4f", result.getCaption().getConfidence()));
        }

        if (result.getDenseCaptions() != null) {
            System.out.println(" Dense Captions:");
            for (DenseCaption denseCaption : result.getDenseCaptions().getValues()) {
                System.out.println("   \"" + denseCaption.getText() + "\", Bounding box "
                    + denseCaption.getBoundingBox() + ", Confidence " + String.format("%.4f", denseCaption.getConfidence()));
            }
        }

        if (result.getRead() != null) {
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

        if (result.getTags() != null) {
            System.out.println(" Tags:");
            for (DetectedTag tag : result.getTags().getValues()) {
                System.out.println("   \"" + tag.getName() + "\", Confidence " + String.format("%.4f", tag.getConfidence()));
            }
        }

        if (result.getObjects() != null) {
            System.out.println(" Objects:");
            for (DetectedObject detectedObject : result.getObjects().getValues()) {
                System.out.println("   \"" + detectedObject.getTags().get(0).getName() + "\", Bounding box "
                    + detectedObject.getBoundingBox() + ", Confidence " + String.format("%.4f", detectedObject.getTags().get(0).getConfidence()));
            }
        }

        if (result.getPeople() != null) {
            System.out.println(" People:");
            for (DetectedPerson person : result.getPeople().getValues()) {
                System.out.println("   Bounding box "
                    + person.getBoundingBox() + ", Confidence " + String.format("%.4f", person.getConfidence()));
            }
        }

        if (result.getSmartCrops() != null) {
            System.out.println(" Crop Suggestions:");
            for (CropRegion cropRegion : result.getSmartCrops().getValues()) {
                System.out.println("   Aspect ratio "
                    + cropRegion.getAspectRatio() + ": Bounding box " + cropRegion.getBoundingBox());
            }
        }

        System.out.println(" Image height = " + result.getMetadata().getHeight());
        System.out.println(" Image width = " + result.getMetadata().getWidth());
        System.out.println(" Model version = " + result.getModelVersion());
    }
    // </snippet_results>
}
