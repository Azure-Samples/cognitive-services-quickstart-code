// <snippet_imports>
import com.google.gson.*;

import com.microsoft.azure.cognitiveservices.vision.contentmoderator.*;
import com.microsoft.azure.cognitiveservices.vision.contentmoderator.models.*;

import java.io.*;
import java.util.*;
import java.util.concurrent.*;
// </snippet_imports>

/** \
 * This quickstart:
 *  - Moderates a URL image
 *  - Moderates a text file
 *  - Creates a human review for an image that posts to the Content Moderator website
 * 
 * A successful response shows a JSON representation of the moderation results.
 * 
 * Prerequisites:
 * 1. Obtain an Azure Content Moderator resource 
 * 2. Clone or download this repo: https://github.com/Azure-Samples/cognitive-services-java-sdk-samples.git
 * 3. Use Java 8 or later
 * 4. Set and add your Content Moderator subscription key and endpoint environment variables in main() below.
 *    Aquire the endpoint from your Content Moderator account... 
 *    use only the base endpoint, for example: https://westus.api.cognitive.microsoft.com
 * 
 * How to run:
 *   Run in your favorite IDE or...
 *   To use the command line:
 *      - Make sure Maven is installed: https://maven.apache.org/install.html 
 *      - Run from the root folder (has pom.xml in it): mvn compile exec:java -Dexec.cleanupDaemonThreads=false
 *      - All dependencies will be automatically included. To list them (if desired): mvn dependency:list 
 *  NOTE: You may see a "WARNING: An illegal reflective access operation has occurred..." within the results.
 *  This does not impact a successful result, so it can be ignored.
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
    
    // HUMAN REVIEWS - IMAGES variables
    private static final String REVIEW_URL = "https://moderatorsampleimages.blob.core.windows.net/samples/sample5.png";
    private static final long LATENCY_DELAY = 30; // milliseconds of delay
    // Add your Content Moderator team name and callback endpoint to your environment variables.
    private static final String TEAM_NAME = System.getenv("CONTENT_MODERATOR_TEAM_NAME");
    private static final String CALLBACK_ENDPOINT = System.getenv("CONTENT_MODERATOR_REVIEWS_ENDPOINT");

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
        public FoundFaces FaceDetection;
    }
    // </snippet_evaluationdata>

    public static void main(String[] args) {  

        // <snippet_client>
        /**
         * Authenticate
         */
        // Set CONTENT_MODERATOR_SUBSCRIPTION_KEY in your environment settings, with your key as its value.
        // Set COMPUTER_MODERATOR_ENDPOINT in your environment variables with your Azure endpoint.
        ContentModeratorClient client = 
            ContentModeratorManager.authenticate(AzureRegionBaseUrl
                                   .fromString(System.getenv("CONTENT_MODERATOR_ENDPOINT")), 
                                        System.getenv("CONTENT_MODERATOR_SUBSCRIPTION_KEY"));
        // </snippet_client>
        
        // Create a List in which to store the image moderation results.
        List<EvaluationData> evaluationData = new ArrayList<EvaluationData>();

        // Moderate URL images
        moderateImages(client, evaluationData);
        // Moderate text from file
        moderateText(client);
        // Create a human review
        humanReviews(client);
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
    
    /**
     * HUMAN REVIEWS - IMAGES
     * This example will create a review for humans to moderate on the Content Moderator website at:
     * https://{LOCATION OR CUSTOM NAME}.contentmoderator.cognitive.microsoft.com
     * Requires you to moderate an image on website (when prompted), 
     * then return to the app (IDE or console) to initiate printing of the results of the review.
     */
    public static void humanReviews(ContentModeratorClient client) {
        System.out.println("---------------------------------------");
        System.out.println("HUMAN REVIEWS - IMAGES");
        System.out.println();
    
        System.out.println("Creating a review for the image: " + REVIEW_URL.substring(REVIEW_URL.lastIndexOf('/')+1, REVIEW_URL.length()));
        System.out.println();
    
        // Create List for the review body items
        List<CreateReviewBodyItem> body = new ArrayList<>();
        List<CreateReviewBodyItemMetadataItem> metadata = new ArrayList<>();
        // This establishes the adult or racy score (sc) that will be returned (true) with the results.
        metadata.add(new CreateReviewBodyItemMetadataItem().withKey("sc").withValue("true"));
        body.add(new CreateReviewBodyItem().withCallbackEndpoint(CALLBACK_ENDPOINT)
                                           .withContent(REVIEW_URL)
                                           .withMetadata(metadata)
                                           .withType("Image"));
    
        // Creates a review by POST to callback URL. Returns a list of string review IDs.
        List<String> reviews = client.reviews().createReviews(TEAM_NAME, "Image", body, null);
        System.out.println();
        System.out.println("Getting review details before human review..."); // for comparison's sake
        System.out.println();
    
        // Returns a Review object, notice the "reviewerResultTags" is empty.
        Review reviewDetailsBefore = client.reviews().getReview(TEAM_NAME, reviews.get(0));
        Gson gsonBefore = new GsonBuilder().setPrettyPrinting().create();
        System.out.println(gsonBefore.toJson(reviewDetailsBefore).toString());
        System.out.println();
    
        // Go to website to perform reviews
        System.out.println("Perform manual reviews on the Content Moderator review site, press ENTER when done...");
        try { System.in.read(); } catch (Exception e) { e.printStackTrace(); }
    
        // Wait for server to process review
        System.out.println("Wait " + LATENCY_DELAY + " seconds for the server to process the request...");
        System.out.println();
        try { TimeUnit.SECONDS.sleep(LATENCY_DELAY); } catch (Exception e) { e.printStackTrace(); }
        
    
        // Get review details again to view changes since the human review. The "reviewerResultTags" is now full.
        System.out.println("Getting review details after human review...");
        System.out.println();
        Review reviewDetailsAfter = client.reviews().getReview(TEAM_NAME, reviews.get(0));
        Gson gsonAfter = new GsonBuilder().setPrettyPrinting().create();
        System.out.println(gsonAfter.toJson(reviewDetailsAfter).toString());
        System.out.println();
        }
    /**
     * END - HUMAN REVIEWS - IMAGES
     */
} 


        
