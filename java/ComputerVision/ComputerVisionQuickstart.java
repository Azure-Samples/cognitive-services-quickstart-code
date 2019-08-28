// <snippet_imports>
import com.microsoft.azure.cognitiveservices.vision.computervision.*;
import com.microsoft.azure.cognitiveservices.vision.computervision.models.*;

import java.io.File;
import java.io.FileInputStream;
import java.nio.file.Files;

import java.util.ArrayList;
import java.util.List;
// </snippet_imports>

public class ComputerVisionQuickstarts
{
    
    // <snippet_mainvars>
    public static void main(String[] args)
    {
        /*  Configure the local environment:
         *
         *  Set the COMPUTER_VISION_SUBSCRIPTION_KEY environment variable on your
         *  local machine using the appropriate method for your preferred command shell.
         *
         *  Note that environment variables cannot contain quotation marks, so the quotation marks
         *  are included in the code below to stringify them.
         *
         *  Note that after setting these environment variables in your preferred command shell,
         *  you will need to close and then re-open your command shell.
         */
        String azureComputerVisionApiKey = System.getenv("COMPUTER_VISION_SUBSCRIPTION_KEY");
        //  END - Configure the local environment.

        /*  Create an authenticated Computer Vision client:
         *
         *  Enter your Computer Vision endpoint URL.
         *  It will have the format: https://westus.api.cognitive.microsoft.com 
         *  You may need to change the first part ("westus") to match your subscription.
         *  Then, create an authenticated client with the API key and the endpoint URL.
         */
        String endpointUrl = ("<your endpoint here>");
// </snippet_mainvars>

        // <snippet_client>
        ComputerVisionClient compVisClient = ComputerVisionManager.authenticate(azureComputerVisionApiKey).withEndpoint(endpointUrl);
        //  END - Create an authenticated Computer Vision client.
        
        System.out.println("\nAzure Cognitive Services Computer Vision - Java Quickstart Sample");
        AnalyzeLocalImage(compVisClient);
    }
    // </snippet_client>


    /*  This Quickstart for the Azure Cognitive Services Computer Vision API shows how to analyze
     *  an image both locally and from a URL.
     *  Analyzing an image includes:
     *  - Displaying image captions and confidence values
     *  - Displaying image category names and confidence values
     *  - Displaying image tags and confidence values
     *  - Displaying any faces found in the image and their bounding boxes
     *  - Displaying whether any adult or racy content was detected and the confidence values
     *  - Displaying the image color scheme
     *  - Displaying any celebrities detected in the image and their bounding boxes
     *  - Displaying any landmarks detected in the image and their bounding boxes
     *  - Displaying what type of clip art or line drawing the image is
     *
     */
    // <snippet_analyzelocal_refs>
    public static void AnalyzeLocalImage(ComputerVisionClient compVisClient)
    {
        /*  Analyze a local image:
         *
         *  Set a string variable equal to the path of a local image. The image path below is a relative path.
         */
        String pathToLocalImage = "src\\main\\resources\\myImage.jpg";
        // </snippet_analyzelocal_refs>

        // <snippet_analyzelocal_features>
        //  This list defines the features to be extracted from the image.
        List<VisualFeatureTypes> featuresToExtractFromLocalImage = new ArrayList<>();
        featuresToExtractFromLocalImage.add(VisualFeatureTypes.DESCRIPTION);
        featuresToExtractFromLocalImage.add(VisualFeatureTypes.CATEGORIES);
        featuresToExtractFromLocalImage.add(VisualFeatureTypes.TAGS);
        featuresToExtractFromLocalImage.add(VisualFeatureTypes.FACES);
        featuresToExtractFromLocalImage.add(VisualFeatureTypes.ADULT);
        featuresToExtractFromLocalImage.add(VisualFeatureTypes.COLOR);
        featuresToExtractFromLocalImage.add(VisualFeatureTypes.IMAGE_TYPE);
        // </snippet_analyzelocal_features>

        System.out.println("\nAnalyzing local image ...");

        try
        {
            // <snippet_analyzelocal_analyze>
            //  Need a byte array for analyzing a local image.
            File rawImage = new File(pathToLocalImage);
            byte[] imageByteArray = Files.readAllBytes(rawImage.toPath());

            //  Call the Computer Vision service and tell it to analyze the loaded image.
            ImageAnalysis analysis = compVisClient.computerVision().analyzeImageInStream()
                .withImage(imageByteArray)
                .withVisualFeatures(featuresToExtractFromLocalImage)
                .execute();

            // </snippet_analyzelocal_analyze>

            // <snippet_analyzelocal_captions>
            //  Display image captions and confidence values.
            System.out.println("\nCaptions: ");
            for (ImageCaption caption : analysis.description().captions()) {
                System.out.printf("\'%s\' with confidence %f\n", caption.text(), caption.confidence());
            }
            // </snippet_analyzelocal_captions>

            // <snippet_analyzelocal_category>
            //  Display image category names and confidence values.
            System.out.println("\nCategories: ");
            for (Category category : analysis.categories()) {
                System.out.printf("\'%s\' with confidence %f\n", category.name(), category.score());
            }
            // </snippet_analyzelocal_category>


            // <snippet_analyzelocal_tags>
            //  Display image tags and confidence values.
            System.out.println("\nTags: ");
            for (ImageTag tag : analysis.tags()) {
                System.out.printf("\'%s\' with confidence %f\n", tag.name(), tag.confidence());
            }
            // </snippet_analyzelocal_tags>

            // <snippet_analyzelocal_faces>
            //  Display any faces found in the image and their location.
            System.out.println("\nFaces: ");
            for (FaceDescription face : analysis.faces()) {
                System.out.printf("\'%s\' of age %d at location (%d, %d), (%d, %d)\n", face.gender(), face.age(),
                    face.faceRectangle().left(), face.faceRectangle().top(),
                    face.faceRectangle().left() + face.faceRectangle().width(),
                    face.faceRectangle().top() + face.faceRectangle().height());
            }
            // </snippet_analyzelocal_faces>

            // <snippet_analyzelocal_adult>
            //  Display whether any adult or racy content was detected and the confidence values.
            System.out.println("\nAdult: ");
            System.out.printf("Is adult content: %b with confidence %f\n", analysis.adult().isAdultContent(), analysis.adult().adultScore());
            System.out.printf("Has racy content: %b with confidence %f\n", analysis.adult().isRacyContent(), analysis.adult().racyScore());
            // </snippet_analyzelocal_adult>

            // <snippet_analyzelocal_colors>
            //  Display the image color scheme.
            System.out.println("\nColor scheme: ");
            System.out.println("Is black and white: " + analysis.color().isBWImg());
            System.out.println("Accent color: " + analysis.color().accentColor());
            System.out.println("Dominant background color: " + analysis.color().dominantColorBackground());
            System.out.println("Dominant foreground color: " + analysis.color().dominantColorForeground());
            System.out.println("Dominant colors: " + String.join(", ", analysis.color().dominantColors()));
            // </snippet_analyzelocal_colors>

            // <snippet_analyzelocal_celebrities>
            //  Display any celebrities detected in the image and their locations.
            System.out.println("\nCelebrities: ");
            for (Category category : analysis.categories())
            {
                if (category.detail() != null && category.detail().celebrities() != null)
                {
                    for (CelebritiesModel celeb : category.detail().celebrities())
                    {
                        System.out.printf("\'%s\' with confidence %f at location (%d, %d), (%d, %d)\n", celeb.name(), celeb.confidence(),
                            celeb.faceRectangle().left(), celeb.faceRectangle().top(),
                            celeb.faceRectangle().left() + celeb.faceRectangle().width(),
                            celeb.faceRectangle().top() + celeb.faceRectangle().height());
                    }
                }
            }
            // </snippet_analyzelocal_celebrities>

            // <snippet_analyzelocal_landmarks>
            //  Display any landmarks detected in the image and their locations.
            System.out.println("\nLandmarks: ");
            for (Category category : analysis.categories())
            {
                if (category.detail() != null && category.detail().landmarks() != null)
                {
                    for (LandmarksModel landmark : category.detail().landmarks())
                    {
                        System.out.printf("\'%s\' with confidence %f\n", landmark.name(), landmark.confidence());
                    }
                }
            }
            // </snippet_analyzelocal_landmarks>

            //  Display what type of clip art or line drawing the image is.
            System.out.println("\nImage type:");
            System.out.println("Clip art type: " + analysis.imageType().clipArtType());
            System.out.println("Line drawing type: " + analysis.imageType().lineDrawingType());
        }

        catch (Exception e)
        {
            System.out.println(e.getMessage());
            e.printStackTrace();
        }
    }
    //  END - Analyze a local image.

    // <snippet_analyzeurl>
    public static void AnalyzeRemoteImage(ComputerVisionClient compVisClient)
    {
        /*  Analyze an image from a URL:
         *
         *  Set a string variable equal to the path of a remote image.
         */
        String pathToRemoteImage = "https://github.com/Azure-Samples/cognitive-services-sample-data-files/raw/master/ComputerVision/Images/faces.jpg";

        //  This list defines the features to be extracted from the image.
        List<VisualFeatureTypes> featuresToExtractFromRemoteImage = new ArrayList<>();
        featuresToExtractFromRemoteImage.add(VisualFeatureTypes.DESCRIPTION);
        featuresToExtractFromRemoteImage.add(VisualFeatureTypes.CATEGORIES);
        featuresToExtractFromRemoteImage.add(VisualFeatureTypes.TAGS);
        featuresToExtractFromRemoteImage.add(VisualFeatureTypes.FACES);
        featuresToExtractFromRemoteImage.add(VisualFeatureTypes.ADULT);
        featuresToExtractFromRemoteImage.add(VisualFeatureTypes.COLOR);
        featuresToExtractFromRemoteImage.add(VisualFeatureTypes.IMAGE_TYPE);

        System.out.println("\n\nAnalyzing an image from a URL ...");

        try
        {
            //  Call the Computer Vision service and tell it to analyze the loaded image.
            ImageAnalysis analysis = compVisClient.computerVision().analyzeImage()
                .withUrl(pathToRemoteImage)
                .withVisualFeatures(featuresToExtractFromRemoteImage)
                .execute();

            //  Display image captions and confidence values.
            System.out.println("\nCaptions: ");
            for (ImageCaption caption : analysis.description().captions()) {
                System.out.printf("\'%s\' with confidence %f\n", caption.text(), caption.confidence());
            }

            //  Display image category names and confidence values.
            System.out.println("\nCategories: ");
            for (Category category : analysis.categories()) {
                System.out.printf("\'%s\' with confidence %f\n", category.name(), category.score());
            }

            //  Display image tags and confidence values.
            System.out.println("\nTags: ");
            for (ImageTag tag : analysis.tags()) {
                System.out.printf("\'%s\' with confidence %f\n", tag.name(), tag.confidence());
            }

            //  Display any faces found in the image and their location.
            System.out.println("\nFaces: ");
            for (FaceDescription face : analysis.faces()) {
                System.out.printf("\'%s\' of age %d at location (%d, %d), (%d, %d)\n", face.gender(), face.age(),
                    face.faceRectangle().left(), face.faceRectangle().top(),
                    face.faceRectangle().left() + face.faceRectangle().width(),
                    face.faceRectangle().top() + face.faceRectangle().height());
            }

            //  Display whether any adult or racy content was detected and the confidence values.
            System.out.println("\nAdult: ");
            System.out.printf("Is adult content: %b with confidence %f\n", analysis.adult().isAdultContent(), analysis.adult().adultScore());
            System.out.printf("Has racy content: %b with confidence %f\n", analysis.adult().isRacyContent(), analysis.adult().racyScore());

            //  Display the image color scheme.
            System.out.println("\nColor scheme: ");
            System.out.println("Is black and white: " + analysis.color().isBWImg());
            System.out.println("Accent color: " + analysis.color().accentColor());
            System.out.println("Dominant background color: " + analysis.color().dominantColorBackground());
            System.out.println("Dominant foreground color: " + analysis.color().dominantColorForeground());
            System.out.println("Dominant colors: " + String.join(", ", analysis.color().dominantColors()));

            //  Display any celebrities detected in the image and their locations.
            System.out.println("\nCelebrities: ");
            for (Category category : analysis.categories())
            {
                if (category.detail() != null && category.detail().celebrities() != null)
                {
                    for (CelebritiesModel celeb : category.detail().celebrities())
                    {
                        System.out.printf("\'%s\' with confidence %f at location (%d, %d), (%d, %d)\n", celeb.name(), celeb.confidence(),
                            celeb.faceRectangle().left(), celeb.faceRectangle().top(),
                            celeb.faceRectangle().left() + celeb.faceRectangle().width(),
                            celeb.faceRectangle().top() + celeb.faceRectangle().height());
                    }
                }
            }

            //  Display any landmarks detected in the image and their locations.
            System.out.println("\nLandmarks: ");
            for (Category category : analysis.categories())
            {
                if (category.detail() != null && category.detail().landmarks() != null)
                {
                    for (LandmarksModel landmark : category.detail().landmarks())
                    {
                        System.out.printf("\'%s\' with confidence %f\n", landmark.name(), landmark.confidence());
                    }
                }
            }

            //  Display what type of clip art or line drawing the image is.
            System.out.println("\nImage type:");
            System.out.println("Clip art type: " + analysis.imageType().clipArtType());
            System.out.println("Line drawing type: " + analysis.imageType().lineDrawingType());
        }

        catch (Exception e)
        {
            System.out.println(e.getMessage());
            e.printStackTrace();
        }
    }
    //  END - Analyze an image from a URL.
    // </snippet_analyzeurl>
}
