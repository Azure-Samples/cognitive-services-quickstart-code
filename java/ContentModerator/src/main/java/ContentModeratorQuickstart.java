// <snippet_imports>
import com.google.gson.*;

import com.microsoft.azure.cognitiveservices.vision.contentmoderator.*;
import com.microsoft.azure.cognitiveservices.vision.contentmoderator.models.*;

import java.io.*;
import java.util.*;
import java.util.concurrent.*;
// </snippet_imports>

/** \
 * This example reads two URL images from file, then moderates them. A successful response shows
 * a JSON representation of the moderation results.
 * 
 * Prerequisites:
 * 1. Obtain an Azure Content Moderator resource 
 * 2. Clone or download this repo: https://github.com/Azure-Samples/cognitive-services-java-sdk-samples.git
 * 3. Use Java 8 or later
 * 4. Set and add your Content Moderator subscription key and endpoint environment variables in main() below.
 * 
 * How to run:
 *   Run in your favorite IDE or...
 *   To use the command line:
 *      - Make sure Maven is installed: https://maven.apache.org/install.html 
 *      - Run from the ContentModerator folder: mvn compile exec:java -Dexec.cleanupDaemonThreads=false
 *      - All dependencies will be automatically included. To list them (if desired): mvn dependency:list 
 * 
 * Resources:
 *   - Content Moderator documentation: https://docs.microsoft.com/en-us/azure/cognitive-services/content-moderator/
 *   - Content Moderator Java SDK: https://docs.microsoft.com/en-us/java/api/overview/azure/cognitiveservices/client/contentmoderator?view=azure-java-stable
 *   - Content Moderator API: https://docs.microsoft.com/en-us/azure/cognitive-services/content-moderator/api-reference
 */ 

public class ContentModeratorQuickstart {

    // MODERATE IMAGES variable
    private static File imageListFile = new File ("src\\main\\resources\\ImageFiles.txt");

    // TEXT MODERATION variable
    private static File textFile = new File("src\\main\\resources\\TextModeration.txt");

    // <snippet_evaluationdata>
    // Contains the image moderation results for an image, including text and face detection from the image.
    public static class EvaluationData {
        // The URL of the evaluated image.
        public String ImageUrl;
        // The image moderation results.
        public Evaluate ImageModeration;
        // The text detection results.
        public OCR TextDetection;
        // The face detection results;

        
                // Create an object in which to store the image moderation results.
        List<EvaluationData> evaluationData = new ArrayList<EvaluationData>();public FoundFaces FaceDetection;
    }
    // </snippet_evaluationdata>

    public static void main(String[] args) {  

        // <snippet_client>
        /**
         * Authenticate
         */
        // Set CONTENT_MODERATOR_SUBSCRIPTION_KEY in your environment settings, with your key as its value.
        // Set COMPUTER_MODERATOR_ENDPOINT in your environment variables, 
        // replace the first part (for example, "westus") with your own, if needed.
        ContentModeratorClient client = 
            ContentModeratorManager.authenticate(AzureRegionBaseUrl
                .fromString(System.getenv("CONTENT_MODERATOR_ENDPOINT")), 
                System.getenv("CONTENT_MODERATOR_SUBSCRIPTION_KEY"));
        // </snippet_client>
        
        // Create an object in which to store the image moderation results.
        List<EvaluationData> evaluationData = new ArrayList<EvaluationData>();

        // Moderate URL images
        moderateImages(client, evaluationData);
        // Moderate text from file
        moderateText(client);
    } 
    // End main()

    /**
     * MODERATE IMAGES
     * Read image URLs from the input file and evaluate/moderate each one.
     */
    public static void moderateImages(ContentModeratorClient client, List<EvaluationData> resultsList) {
        System.out.println();
        System.out.println("---------------------------------------");
        System.out.println("MODERATE IMAGES");
        System.out.println();
        
        // <snippet_imagemod_iterate>        
        // ImageFiles.txt is the file that contains the image URLs to evaluate.
        // Relative paths are relative to the execution directory.
        try (BufferedReader inputStream = new BufferedReader(new FileReader(imageListFile) )){
            String line;
            while ((line = inputStream.readLine()) != null) {
                if (line.length() > 0) {
                    // Evalutate each line of text
                    BodyModelModel url = new BodyModelModel();
                    url.withDataRepresentation("URL");
                    url.withValue(line);
                    // Save to EvaluationData class for later
                    EvaluationData imageData = new EvaluationData(); 
                    imageData.ImageUrl = url.value();
                    // </snippet_imagemod_iterate>

                    // <snippet_imagemod_ar>
                    // Evaluate for adult and racy content.
                    imageData.ImageModeration = client.imageModerations().evaluateUrlInput("application/json", url, new EvaluateUrlInputOptionalParameter().withCacheImage(true));
                    Thread.sleep(1000);
                    // </snippet_imagemod_ar>

                    // <snippet_imagemod_text>
                    // Detect and extract text from image.
                    imageData.TextDetection = client.imageModerations().oCRUrlInput("eng", "application/json", url, new OCRUrlInputOptionalParameter().withCacheImage(true));
                    Thread.sleep(1000);
                    // </snippet_imagemod_text>

                    // <snippet_imagemod_faces>
                    // Detect faces.
                    imageData.FaceDetection = client.imageModerations().findFacesUrlInput("application/json", url, new FindFacesUrlInputOptionalParameter().withCacheImage(true));
                    Thread.sleep(1000);
                    // </snippet_imagemod_faces>

                    // <snippet_imagemod_storedata>
                    resultsList.add(imageData);
                    // </snippet_imagemod_storedata>

                   System.out.println("Image moderation status: " + imageData.ImageModeration.status().description());
                }
            }
            System.out.println();

            // <snippet_imagemod_printdata>
            // Save the moderation results to a file.
            // ModerationOutput.json contains the output from the evaluation.
            // Relative paths are relative to the execution directory (where pom.xml is located).
            BufferedWriter writer = new BufferedWriter(new FileWriter(new File("src\\main\\resources\\ImageModerationOutput.json")));
            // For formatting the printed results
            Gson gson = new GsonBuilder().setPrettyPrinting().create();  

            writer.write(gson.toJson(resultsList).toString());
            System.out.println("Check ImageModerationOutput.json to see printed results.");
            writer.close();
            // </snippet_imagemod_printdata>

        // <snippet_imagemod_catch>
        }   catch (Exception e) {
            System.out.println(e.getMessage());
            e.printStackTrace();
        }
        // </snippet_imagemod_catch>
        System.out.println();
    }
    /**
     * END - MODERATE IMAGES
     */

    /**
     * MODERATE TEXT
     * Read text as a string from the input file and evaluate/moderate it.
     * Check for profanity, autocorrect the text, check for personally identifying information (PII), and classify the text.
     */
    public static void moderateText(ContentModeratorClient client) {
        System.out.println("---------------------------------------");
        System.out.println("MODERATE TEXT");
        System.out.println();

        try (BufferedReader inputStream = new BufferedReader(new FileReader(textFile))) {
            String line;
            Screen textResults = null;
            // For formatting the printed results
            Gson gson = new GsonBuilder().setPrettyPrinting().create(); 

            while ((line = inputStream.readLine()) != null) {
                if (line.length() > 0) {
                    textResults = client.textModerations().screenText("text/plain", line.getBytes(), null);
                    // Uncomment below line to print in console
                    // System.out.println(gson.toJson(textResults).toString());
                }
            }

            System.out.println("Text moderation status: " + textResults.status().description());
            System.out.println();

            // Create output results file to TextModerationOutput.json
            BufferedWriter writer = new BufferedWriter(new FileWriter(new File("src\\main\\resources\\TextModerationOutput.json")));
            writer.write(gson.toJson(textResults).toString());
            System.out.println("Check TextModerationOutput.json to see printed results.");
            System.out.println();
            writer.close();
        }   catch (Exception e) {
            System.out.println(e.getMessage());
            e.printStackTrace();
        }
    }
    /**
     * END - MODERATE TEXT
     */
} 


        
