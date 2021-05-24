/*  This Quickstart for the Azure Cognitive Services Computer Vision API shows how to analyze
 *  an image and recognize text for both a local and remote (URL) image.
 *
 *  Analyzing an image includes:
 *  - Displaying image captions and confidence values
 *  - Displaying image category names and confidence values
 *  - Displaying image tags and confidence values
 *  - Displaying any faces found in the image and their bounding boxes
 *  - Displaying whether any adult or racy content was detected and the confidence values
 *  - Displaying the image color scheme
 *  - Displaying any celebrities detected in the image and their bounding boxes
 *  - Displaying any landmarks detected in the image and their bounding boxes
 *  - Displaying whether an image is a clip art or line drawing type
 *  - Extract Text (OCR) with the new Read API to find text in an image or document.
 */

// <snippet_imports_and_vars>
// <snippet_imports>
import com.microsoft.azure.cognitiveservices.vision.computervision.*;
import com.microsoft.azure.cognitiveservices.vision.computervision.implementation.ComputerVisionImpl;
import com.microsoft.azure.cognitiveservices.vision.computervision.models.*;

import java.io.*;
import java.nio.file.Files;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;
// </snippet_imports>

// <snippet_classdef_1>
public class ComputerVisionQuickstart {
    // </snippet_classdef_1>

    // <snippet_creds>
    static String subscriptionKey = "PASTE_YOUR_COMPUTER_VISION_SUBSCRIPTION_KEY_HERE";
    static String endpoint = "PASTE_YOUR_COMPUTER_VISION_ENDPOINT_HERE";
    // </snippet_creds>
    // </snippet_imports_and_vars>

    // <snippet_beginmain>
    public static void main(String[] args) {
        
        System.out.println("\nAzure Cognitive Services Computer Vision - Java Quickstart Sample");
        // </snippet_beginmain>

        // <snippet_authinmain>
        // Create an authenticated Computer Vision client.
        ComputerVisionClient compVisClient = Authenticate(subscriptionKey, endpoint); 
        // </snippet_authinmain>

        // <snippet_analyzeinmain>
        // Analyze local and remote images
        AnalyzeLocalImage(compVisClient);
        // </snippet_analyzeinmain>

        // <snippet_readinmain>
        // Read from local file
        ReadFromFile(compVisClient);
        // </snippet_readinmain>
    // <snippet_endmain>
    }
    // </snippet_endmain>

    // <snippet_auth>
    public static ComputerVisionClient Authenticate(String subscriptionKey, String endpoint){
        return ComputerVisionManager.authenticate(subscriptionKey).withEndpoint(endpoint);
    }
    // </snippet_auth>

    // <snippet_analyzelocal_refs>
    public static void AnalyzeLocalImage(ComputerVisionClient compVisClient) {
        /*
         * Analyze a local image:
         *
         * Set a string variable equal to the path of a local image. The image path
         * below is a relative path.
         */
        String pathToLocalImage = "src\\main\\resources\\myImage.png";
        // </snippet_analyzelocal_refs>

        // <snippet_analyzelocal_features>
        // This list defines the features to be extracted from the image.
        List<VisualFeatureTypes> featuresToExtractFromLocalImage = new ArrayList<>();
        featuresToExtractFromLocalImage.add(VisualFeatureTypes.DESCRIPTION);
        featuresToExtractFromLocalImage.add(VisualFeatureTypes.CATEGORIES);
        featuresToExtractFromLocalImage.add(VisualFeatureTypes.TAGS);
        featuresToExtractFromLocalImage.add(VisualFeatureTypes.FACES);
        featuresToExtractFromLocalImage.add(VisualFeatureTypes.OBJECTS);
        featuresToExtractFromLocalImage.add(VisualFeatureTypes.BRANDS);
        featuresToExtractFromLocalImage.add(VisualFeatureTypes.ADULT);
        featuresToExtractFromLocalImage.add(VisualFeatureTypes.COLOR);
        featuresToExtractFromLocalImage.add(VisualFeatureTypes.IMAGE_TYPE);
        // </snippet_analyzelocal_features>

        System.out.println("\nAnalyzing local image ...");
        // <snippet_analyzelocal_analyze>
        try {
            // Need a byte array for analyzing a local image.
            File rawImage = new File(pathToLocalImage);
            byte[] imageByteArray = Files.readAllBytes(rawImage.toPath());

            // Call the Computer Vision service and tell it to analyze the loaded image.
            ImageAnalysis analysis = compVisClient.computerVision().analyzeImageInStream().withImage(imageByteArray)
                    .withVisualFeatures(featuresToExtractFromLocalImage).execute();

            // </snippet_analyzelocal_analyze>

            // <snippet_analyzelocal_captions>
            // Display image captions and confidence values.
            System.out.println("\nCaptions: ");
            for (ImageCaption caption : analysis.description().captions()) {
                System.out.printf("\'%s\' with confidence %f\n", caption.text(), caption.confidence());
            }
            // </snippet_analyzelocal_captions>

            // <snippet_analyzelocal_category>
            // Display image category names and confidence values.
            System.out.println("\nCategories: ");
            for (Category category : analysis.categories()) {
                System.out.printf("\'%s\' with confidence %f\n", category.name(), category.score());
            }
            // </snippet_analyzelocal_category>

            // <snippet_analyzelocal_tags>
            // Display image tags and confidence values.
            System.out.println("\nTags: ");
            for (ImageTag tag : analysis.tags()) {
                System.out.printf("\'%s\' with confidence %f\n", tag.name(), tag.confidence());
            }
            // </snippet_analyzelocal_tags>

            // <snippet_analyzelocal_faces>
            // Display any faces found in the image and their location.
            System.out.println("\nFaces: ");
            for (FaceDescription face : analysis.faces()) {
                System.out.printf("\'%s\' of age %d at location (%d, %d), (%d, %d)\n", face.gender(), face.age(),
                        face.faceRectangle().left(), face.faceRectangle().top(),
                        face.faceRectangle().left() + face.faceRectangle().width(),
                        face.faceRectangle().top() + face.faceRectangle().height());
            }
            // </snippet_analyzelocal_faces>

            // <snippet_analyzelocal_objects>
            // Display any objects found in the image.
            System.out.println("\nObjects: ");
            for ( DetectedObject object : analysis.objects()) {
                System.out.printf("Object \'%s\' detected at location (%d, %d)\n", object.objectProperty(),
                        object.rectangle().x(), object.rectangle().y());
            }
            // </snippet_analyzelocal_objects>

            // <snippet_analyzelocal_brands>
            // Display any brands found in the image.
            System.out.println("\nBrands: ");
            for ( DetectedBrand brand : analysis.brands()) {
                System.out.printf("Brand \'%s\' detected at location (%d, %d)\n", brand.name(),
                        brand.rectangle().x(), brand.rectangle().y());
            }
            // </snippet_analyzelocal_brands>

            // <snippet_analyzelocal_adult>
            // Display whether any adult/racy/gory content was detected and the confidence
            // values.
            System.out.println("\nAdult: ");
            System.out.printf("Is adult content: %b with confidence %f\n", analysis.adult().isAdultContent(),
                    analysis.adult().adultScore());
            System.out.printf("Has racy content: %b with confidence %f\n", analysis.adult().isRacyContent(),
                    analysis.adult().racyScore());
            System.out.printf("Has gory content: %b with confidence %f\n", analysis.adult().isGoryContent(),
                    analysis.adult().goreScore());
            // </snippet_analyzelocal_adult>

            // <snippet_analyzelocal_colors>
            // Display the image color scheme.
            System.out.println("\nColor scheme: ");
            System.out.println("Is black and white: " + analysis.color().isBWImg());
            System.out.println("Accent color: " + analysis.color().accentColor());
            System.out.println("Dominant background color: " + analysis.color().dominantColorBackground());
            System.out.println("Dominant foreground color: " + analysis.color().dominantColorForeground());
            System.out.println("Dominant colors: " + String.join(", ", analysis.color().dominantColors()));
            // </snippet_analyzelocal_colors>

            // <snippet_analyzelocal_celebrities>
            // Display any celebrities detected in the image and their locations.
            System.out.println("\nCelebrities: ");
            for (Category category : analysis.categories()) {
                if (category.detail() != null && category.detail().celebrities() != null) {
                    for (CelebritiesModel celeb : category.detail().celebrities()) {
                        System.out.printf("\'%s\' with confidence %f at location (%d, %d), (%d, %d)\n", celeb.name(),
                                celeb.confidence(), celeb.faceRectangle().left(), celeb.faceRectangle().top(),
                                celeb.faceRectangle().left() + celeb.faceRectangle().width(),
                                celeb.faceRectangle().top() + celeb.faceRectangle().height());
                    }
                }
            }
            // </snippet_analyzelocal_celebrities>

            // <snippet_analyzelocal_landmarks>
            // Display any landmarks detected in the image and their locations.
            System.out.println("\nLandmarks: ");
            for (Category category : analysis.categories()) {
                if (category.detail() != null && category.detail().landmarks() != null) {
                    for (LandmarksModel landmark : category.detail().landmarks()) {
                        System.out.printf("\'%s\' with confidence %f\n", landmark.name(), landmark.confidence());
                    }
                }
            }
            // </snippet_analyzelocal_landmarks>

            // <snippet_imagetype>
            // Display what type of clip art or line drawing the image is.
            System.out.println("\nImage type:");
            System.out.println("Clip art type: " + analysis.imageType().clipArtType());
            System.out.println("Line drawing type: " + analysis.imageType().lineDrawingType());
            // </snippet_imagetype>
            // <snippet_analyze_catch>
        }

        catch (Exception e) {
            System.out.println(e.getMessage());
            e.printStackTrace();
        }
    }
    // </snippet_analyze_catch>

    // END - Analyze a local image.

    // <snippet_analyzeurl>
    public static void AnalyzeRemoteImage(ComputerVisionClient compVisClient) {
        /*
         * Analyze an image from a URL:
         *
         * Set a string variable equal to the path of a remote image.
         */
        String pathToRemoteImage = "https://github.com/Azure-Samples/cognitive-services-sample-data-files/raw/master/ComputerVision/Images/faces.jpg";

        // This list defines the features to be extracted from the image.
        List<VisualFeatureTypes> featuresToExtractFromRemoteImage = new ArrayList<>();
        featuresToExtractFromRemoteImage.add(VisualFeatureTypes.DESCRIPTION);
        featuresToExtractFromRemoteImage.add(VisualFeatureTypes.CATEGORIES);
        featuresToExtractFromRemoteImage.add(VisualFeatureTypes.TAGS);
        featuresToExtractFromRemoteImage.add(VisualFeatureTypes.FACES);
        featuresToExtractFromRemoteImage.add(VisualFeatureTypes.ADULT);
        featuresToExtractFromRemoteImage.add(VisualFeatureTypes.COLOR);
        featuresToExtractFromRemoteImage.add(VisualFeatureTypes.IMAGE_TYPE);

        System.out.println("\n\nAnalyzing an image from a URL ...");

        try {
            // Call the Computer Vision service and tell it to analyze the loaded image.
            ImageAnalysis analysis = compVisClient.computerVision().analyzeImage().withUrl(pathToRemoteImage)
                    .withVisualFeatures(featuresToExtractFromRemoteImage).execute();

            // Display image captions and confidence values.
            System.out.println("\nCaptions: ");
            for (ImageCaption caption : analysis.description().captions()) {
                System.out.printf("\'%s\' with confidence %f\n", caption.text(), caption.confidence());
            }

            // Display image category names and confidence values.
            System.out.println("\nCategories: ");
            for (Category category : analysis.categories()) {
                System.out.printf("\'%s\' with confidence %f\n", category.name(), category.score());
            }

            // Display image tags and confidence values.
            System.out.println("\nTags: ");
            for (ImageTag tag : analysis.tags()) {
                System.out.printf("\'%s\' with confidence %f\n", tag.name(), tag.confidence());
            }

            // Display any faces found in the image and their location.
            System.out.println("\nFaces: ");
            for (FaceDescription face : analysis.faces()) {
                System.out.printf("\'%s\' of age %d at location (%d, %d), (%d, %d)\n", face.gender(), face.age(),
                        face.faceRectangle().left(), face.faceRectangle().top(),
                        face.faceRectangle().left() + face.faceRectangle().width(),
                        face.faceRectangle().top() + face.faceRectangle().height());
            }

            // Display whether any adult or racy content was detected and the confidence
            // values.
            System.out.println("\nAdult: ");
            System.out.printf("Is adult content: %b with confidence %f\n", analysis.adult().isAdultContent(),
                    analysis.adult().adultScore());
            System.out.printf("Has racy content: %b with confidence %f\n", analysis.adult().isRacyContent(),
                    analysis.adult().racyScore());

            // Display the image color scheme.
            System.out.println("\nColor scheme: ");
            System.out.println("Is black and white: " + analysis.color().isBWImg());
            System.out.println("Accent color: " + analysis.color().accentColor());
            System.out.println("Dominant background color: " + analysis.color().dominantColorBackground());
            System.out.println("Dominant foreground color: " + analysis.color().dominantColorForeground());
            System.out.println("Dominant colors: " + String.join(", ", analysis.color().dominantColors()));

            // Display any celebrities detected in the image and their locations.
            System.out.println("\nCelebrities: ");
            for (Category category : analysis.categories()) {
                if (category.detail() != null && category.detail().celebrities() != null) {
                    for (CelebritiesModel celeb : category.detail().celebrities()) {
                        System.out.printf("\'%s\' with confidence %f at location (%d, %d), (%d, %d)\n", celeb.name(),
                                celeb.confidence(), celeb.faceRectangle().left(), celeb.faceRectangle().top(),
                                celeb.faceRectangle().left() + celeb.faceRectangle().width(),
                                celeb.faceRectangle().top() + celeb.faceRectangle().height());
                    }
                }
            }

            // Display any landmarks detected in the image and their locations.
            System.out.println("\nLandmarks: ");
            for (Category category : analysis.categories()) {
                if (category.detail() != null && category.detail().landmarks() != null) {
                    for (LandmarksModel landmark : category.detail().landmarks()) {
                        System.out.printf("\'%s\' with confidence %f\n", landmark.name(), landmark.confidence());
                    }
                }
            }

            // Display what type of clip art or line drawing the image is.
            System.out.println("\nImage type:");
            System.out.println("Clip art type: " + analysis.imageType().clipArtType());
            System.out.println("Line drawing type: " + analysis.imageType().lineDrawingType());
        }

        catch (Exception e) {
            System.out.println(e.getMessage());
            e.printStackTrace();
        }
    }
    // END - Analyze an image from a URL.
    // </snippet_analyzeurl>

    
    /**
     * OCR with READ : Performs a Read Operation on a remote image
     * @param client instantiated vision client
     * @param remoteTextImageURL public url from which to perform the read operation against
     */
    
    private static void ReadFromUrl(ComputerVisionClient client) {
        System.out.println("-----------------------------------------------");
        String remoteTextImageURL = "https://intelligentkioskstore.blob.core.windows.net/visionapi/suggestedphotos/3.png";
        System.out.println("Read with URL: " + remoteTextImageURL);
        try {
            // Cast Computer Vision to its implementation to expose the required methods
            ComputerVisionImpl vision = (ComputerVisionImpl) client.computerVision();

            // Read in remote image and response header
            ReadHeaders responseHeader = vision.readWithServiceResponseAsync(remoteTextImageURL, null, null,null)
                    .toBlocking()
                    .single()
                    .headers();

            // Extract the operation Id from the operationLocation header
            String operationLocation = responseHeader.operationLocation();
            System.out.println("Operation Location:" + operationLocation);

            getAndPrintReadResult(vision, operationLocation);

        } catch (Exception e) {
            System.out.println(e.getMessage());
            e.printStackTrace();
        }
    }

    // <snippet_read_setup>
    /**
     * OCR with READ : Performs a Read Operation on a local image
     * @param client instantiated vision client
     * @param localFilePath local file path from which to perform the read operation against
     */
    private static void ReadFromFile(ComputerVisionClient client) {
        System.out.println("-----------------------------------------------");
        
        String localFilePath = "src\\main\\resources\\myImage.png";
        System.out.println("Read with local file: " + localFilePath);
        // </snippet_read_setup>
        // <snippet_read_call>

        try {
            File rawImage = new File(localFilePath);
            byte[] localImageBytes = Files.readAllBytes(rawImage.toPath());

            // Cast Computer Vision to its implementation to expose the required methods
            ComputerVisionImpl vision = (ComputerVisionImpl) client.computerVision();

            // Read in remote image and response header
            ReadInStreamHeaders responseHeader =
                    vision.readInStreamWithServiceResponseAsync(localImageBytes, null, null)
                        .toBlocking()
                        .single()
                        .headers();
            // </snippet_read_call>
    // <snippet_read_response>
            // Extract the operationLocation from the response header
            String operationLocation = responseHeader.operationLocation();
            System.out.println("Operation Location:" + operationLocation);

            getAndPrintReadResult(vision, operationLocation);
    // </snippet_read_response>
            // <snippet_read_catch>

        } catch (Exception e) {
            System.out.println(e.getMessage());
            e.printStackTrace();
        }
    }
    // </snippet_read_catch>

    // <snippet_opid_extract>
    /**
     * Extracts the OperationId from a Operation-Location returned by the POST Read operation
     * @param operationLocation
     * @return operationId
     */
    private static String extractOperationIdFromOpLocation(String operationLocation) {
        if (operationLocation != null && !operationLocation.isEmpty()) {
            String[] splits = operationLocation.split("/");

            if (splits != null && splits.length > 0) {
                return splits[splits.length - 1];
            }
        }
        throw new IllegalStateException("Something went wrong: Couldn't extract the operation id from the operation location");
    }
    // </snippet_opid_extract>

    // <snippet_read_result_helper_call>
    /**
     * Polls for Read result and prints results to console
     * @param vision Computer Vision instance
     * @return operationLocation returned in the POST Read response header
     */
    private static void getAndPrintReadResult(ComputerVision vision, String operationLocation) throws InterruptedException {
        System.out.println("Polling for Read results ...");

        // Extract OperationId from Operation Location
        String operationId = extractOperationIdFromOpLocation(operationLocation);

        boolean pollForResult = true;
        ReadOperationResult readResults = null;

        while (pollForResult) {
            // Poll for result every second
            Thread.sleep(1000);
            readResults = vision.getReadResult(UUID.fromString(operationId));

            // The results will no longer be null when the service has finished processing the request.
            if (readResults != null) {
                // Get request status
                OperationStatusCodes status = readResults.status();

                if (status == OperationStatusCodes.FAILED || status == OperationStatusCodes.SUCCEEDED) {
                    pollForResult = false;
                }
            }
        }
        // </snippet_read_result_helper_call>
        
        // <snippet_read_result_helper_print>
        // Print read results, page per page
        for (ReadResult pageResult : readResults.analyzeResult().readResults()) {
            System.out.println("");
            System.out.println("Printing Read results for page " + pageResult.page());
            StringBuilder builder = new StringBuilder();

            for (Line line : pageResult.lines()) {
                builder.append(line.text());
                builder.append("\n");
            }

            System.out.println(builder.toString());
        }
    }
    // </snippet_read_result_helper_print>
    // <snippet_classdef_2>
}
// </snippet_classdef_2>
