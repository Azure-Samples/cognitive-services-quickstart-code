// USAGE:
//     Compile the sample:
//         mvn clean dependency:copy-dependencies
//         javac ImageAnalysisQuickStart.java -cp target\dependency\*
//     Run the sample:
//         java -cp ".;target\dependency\*" ImageAnalysisQuickStart
//
//     Set these two environment variables before running the sample:
//     1) VISION_ENDPOINT - Your endpoint URL, in the form https://your-resource-name.cognitiveservices.azure.com
//                          where `your-resource-name` is your unique Azure Computer Vision resource name.
//     2) VISION_KEY - Your Computer Vision key (a 32-character Hexadecimal number)

// <snippet_single>
import com.azure.ai.vision.imageanalysis.*;
import com.azure.ai.vision.imageanalysis.models.*;
import com.azure.core.credential.KeyCredential;
import java.util.Arrays;

public class ImageAnalysisQuickStart {

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
        ImageAnalysisResult result = client.analyzeFromUrl(
            "https://learn.microsoft.com/azure/ai-services/computer-vision/media/quickstarts/presentation.png",
            Arrays.asList(VisualFeatures.CAPTION, VisualFeatures.READ),
            new ImageAnalysisOptions().setGenderNeutralCaption(true));

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
