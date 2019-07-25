import com.microsoft.azure.cognitiveservices.vision.computervision.*;
import com.microsoft.azure.cognitiveservices.vision.computervision.models.*;

import java.io.File;
import java.io.FileInputStream;
import java.nio.file.Files;

import java.util.ArrayList;
import java.util.List;

public class ComputerVisionQuickstarts
{
    public static void main(String[] args)
    {
        System.out.println("\nAzure Cognitive Services Computer Vision - Java Quickstart Sample");
        RunQuickstarts();
    }

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


    public static void RunQuickstarts()
    {
        /*  Configure the local environment:
         *
         *  Set the AZURE_COMPUTERVISION_API_KEY and AZURE_REGION environment variables on your
         *  local machine using the appropriate method for your preferred command shell.
         *
         *  For AZURE_REGION, use the same region you used to get your subscription keys.
         ***Can we link (or just put the URL here) to the docs for finding your region?
         *
         *  Note that environment variables cannot contain quotation marks, so the quotation marks
         *  are included in the code below to stringify them.
         *
         *  Note that after setting these environment variables in your preferred command shell,
         *  you will need to close and then re-open your command shell.
         */

        String azureComputerVisionApiKey = System.getenv("AZURE_COMPUTERVISION_API_KEY");
        String azureRegion = System.getenv("AZURE_REGION");
        //  END - Configure the local environment.


        /*  Create an authenticated Computer Vision client:
         *
         *  Concatenate the Azure region with the Azure base URL to create the endpoint URL, and
         *  then create an authenticated client with the API key and the endpoint URL.
         */

        String endpointUrl = ("https://").concat(azureRegion).concat(".api.cognitive.microsoft.com");
        ComputerVisionClient compVisClient = ComputerVisionManager.authenticate(azureComputerVisionApiKey).withEndpoint(endpointUrl);
        //  END - Create an authenticated Computer Vision client.


        /*  Analyze a local image:
         *
         *  Set a string variable equal to the path of a local image. The image path below is a relative path.
         */
        String pathToLocalImage = "src\\main\\resources\\landmark.jpg";

        //  This list defines the features to be extracted from the image.
        List<VisualFeatureTypes> featuresToExtractFromLocalImage = new ArrayList<>();
        featuresToExtractFromLocalImage.add(VisualFeatureTypes.DESCRIPTION);
        featuresToExtractFromLocalImage.add(VisualFeatureTypes.CATEGORIES);
        featuresToExtractFromLocalImage.add(VisualFeatureTypes.TAGS);
        featuresToExtractFromLocalImage.add(VisualFeatureTypes.FACES);
        featuresToExtractFromLocalImage.add(VisualFeatureTypes.ADULT);
        featuresToExtractFromLocalImage.add(VisualFeatureTypes.COLOR);
        featuresToExtractFromLocalImage.add(VisualFeatureTypes.IMAGE_TYPE);

        System.out.println("\nAnalyzing local image ...");

        try
        {
            //  Need a byte array for analyzing a local image.
            File rawImage = new File(pathToLocalImage);
            byte[] imageByteArray = Files.readAllBytes(rawImage.toPath());

            //  Call the Computer Vision service and tell it to analyze the loaded image.
            ImageAnalysis analysis = compVisClient.computerVision().analyzeImageInStream()
                .withImage(imageByteArray)
                .withVisualFeatures(featuresToExtractFromLocalImage)
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
        //  END - Analyze a local image.


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
            //  END - Analyze an image from a URL.
        }

        catch (Exception e)
        {
            System.out.println(e.getMessage());
            e.printStackTrace();
        }
    }
}
