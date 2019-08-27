package main

/*  Import the required libraries. If this is your first time running a Go program,
 *  you will need to 'go get' the azure-sdk-for-go and go-autorest packages.
 */
import (
	"context"
	"encoding/json"
	"fmt"
	"github.com/Azure/azure-sdk-for-go/services/cognitiveservices/v2.0/computervision"
	"github.com/Azure/go-autorest/autorest"
	"io"
	"log"
	"os"
	"strings"
	"time"
)

/*  The examples in this quickstart are for the Computer Vision API for Microsoft
 *  Cognitive Services with the following tasks:
 *  - Describing images
 *  - Categorizing images
 *  - Tagging images
 *  - Detecting faces
 *  - Detecting adult or racy content
 *  - Detecting the color scheme
 *  - Detecting domain-specific content (celebrities/landmarks)
 *  - Detecting image types (clip art/line drawing)
 *  - Detecting objects
 *  - Detecting brands
 *  - Recognizing printed and handwritten text with the batch read API
 *	- Recognizing printed text with OCR
 */

// Declare global so don't have to pass it to all of the tasks.
var computerVisionContext context.Context

func main() {
	/*  Prerequisites for the Computer Vision client:
	 *  Set environment variables for COMPUTER_VISION_SUBSCRIPTION_KEY and COMPUTER_VISION_ENDPOINT,
	 *  then restart your command shell or your IDE for changes to take effect.
	 */
  	computerVisionKey := os.Getenv("COMPUTER_VISION_SUBSCRIPTION_KEY")
	if (computerVisionKey != "COMPUTER_VISION_SUBSCRIPTION_KEY") {
		log.Fatal("\n\nPlease set a COMPUTER_VISION_SUBSCRIPTION_KEY environment variable.\n" +
							  "**You may need to restart your shell or IDE after it's set.**\n")
	}

	endpointURL := os.Getenv("COMPUTER_VISION_ENDPOINT")
	if (endpointURL != "COMPUTER_VISION_ENDPOINT") {
		log.Fatal("\n\nPlease set a COMPUTER_VISION_ENDPOINT environment variable.\n" +
							  "**You may need to restart your shell or IDE after it's set.**")
	}

	computerVisionClient := computervision.New(endpointURL);
	computerVisionClient.Authorizer = autorest.NewCognitiveServicesAuthorizer(computerVisionKey)

	computerVisionContext = context.Background()
	// END - Configure the Computer Vision client


	// Analyze a local image
	localImagePath := "resources\\faces.jpg"
	workingDirectory, err := os.Getwd()
	if err != nil {
		log.Fatal(err)
	}
	fmt.Printf("\nLocal image path:\n%v\n", workingDirectory + "\\" + localImagePath)

	DescribeLocalImage(computerVisionClient, localImagePath)
	CategorizeLocalImage(computerVisionClient, localImagePath)
	TagLocalImage(computerVisionClient, localImagePath)
	DetectFacesLocalImage(computerVisionClient, localImagePath)
	DetectAdultOrRacyContentLocalImage(computerVisionClient, localImagePath)
	DetectColorSchemeLocalImage(computerVisionClient, localImagePath)
	DetectDomainSpecificContentLocalImage(computerVisionClient, localImagePath)
	DetectImageTypesLocalImage(computerVisionClient, localImagePath)
	DetectObjectsLocalImage(computerVisionClient, localImagePath)
	// END - Analyze a local iamge

	// Brand detection on a local image
	fmt.Println("\nGetting new local image for brand detection ... \n")
	localImagePath = "resources\\gray-shirt-logo.jpg"
	workingDirectory, err = os.Getwd()
	if err != nil {
		log.Fatal(err)
	}
	fmt.Printf("Local image path:\n%v\n", workingDirectory + "\\" + localImagePath)

	DetectBrandsLocalImage(computerVisionClient, localImagePath)
	// END - Brand detection


	// Text recognition on a local image with the Read API
	fmt.Println("\nGetting new local image for text recognition of handwriting with the Read API... \n")
	localImagePath = "resources\\handwritten_text.jpg"
	workingDirectory, err = os.Getwd()
	if err != nil {
		log.Fatal(err)
	}
	fmt.Printf("Local image path:\n%v\n", workingDirectory + "\\" + localImagePath)

	RecognizeTextReadAPILocalImage(computerVisionClient, localImagePath)
	// END - Text recognition on a local image with the Read API

	// Text recognition on a local image with OCR
	fmt.Println("\nGetting new local image for text recognition with OCR... \n")
	localImagePath = "resources\\printed_text.jpg"
	workingDirectory, err = os.Getwd()
	if err != nil {
		log.Fatal(err)
	}
	fmt.Printf("Local image path:\n%v\n", workingDirectory + "\\" + localImagePath)
	ExtractTextOCRLocalImage(computerVisionClient, localImagePath)
	// END - Text recognition on a local image with OCR

	// Analyze a remote image
	remoteImageURL := "https://github.com/Azure-Samples/cognitive-services-sample-data-files/raw/master/ComputerVision/Images/landmark.jpg"
	fmt.Printf("\nRemote image path: \n%v\n", remoteImageURL)

	DescribeRemoteImage(computerVisionClient, remoteImageURL)
	CategorizeRemoteImage(computerVisionClient, remoteImageURL)
	TagRemoteImage(computerVisionClient, remoteImageURL)
	DetectFacesRemoteImage(computerVisionClient, remoteImageURL)
	DetectAdultOrRacyContentRemoteImage(computerVisionClient, remoteImageURL)
	DetectColorSchemeRemoteImage(computerVisionClient, remoteImageURL)
	DetectDomainSpecificContentRemoteImage(computerVisionClient, remoteImageURL)
	DetectImageTypesRemoteImage(computerVisionClient, remoteImageURL)
	DetectObjectsRemoteImage(computerVisionClient, remoteImageURL)
	// END - Analyze a remote image

	// Brand detection on a remote image
	fmt.Println("\nGetting new remote image for brand recognition ... \n")
	remoteImageURL = "https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/images/gray-shirt-logo.jpg"
	fmt.Printf("Remote image path: \n%v\n", remoteImageURL)

	DetectBrandsRemoteImage(computerVisionClient, remoteImageURL)
	// END - Brand detection on a remote image


	// Text recognition on a remote image
	fmt.Println("\nGetting new remote image for text recognition of printed text with the Read API... \n")
	remoteImageURL = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/printed_text.jpg"
	fmt.Printf("Remote image path: \n%v\n", remoteImageURL)

	RecognizeTextReadAPIRemoteImage(computerVisionClient, remoteImageURL)
	// END - Text recognition on a remote image

	// Text recognition on a remote image with OCR
	remoteImageURL = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/printed_text.jpg"
	ExtractTextOCRRemoteImage(computerVisionClient, remoteImageURL)
	// END - Text recognition on a remote image with OCR
}

/*  Describe a local image by:
 *    1. Instantiating a ReadCloser, which is required by AnalyzeImageInStream.
 *    2. Opening the ReadCloser instance for reading.
 *    3. Calling the Computer Vision service's AnalyzeImageInStream with the:
 *       - context
 *       - image
 *	 - the number of descriptions to return
 *       - "" to specify the default language ("en") as the output language
 *    4. Displaying the image captions and their confidence values.
 */
func DescribeLocalImage(client computervision.BaseClient, localImagePath string) {
	var localImage io.ReadCloser
	localImage, err := os.Open(localImagePath)
	if err != nil {
		log.Fatal(err)
	}

	maxNumberDescriptionCandidates := new(int32)
	*maxNumberDescriptionCandidates = 1

	localImageDescription, err := client.DescribeImageInStream(
			computerVisionContext,
			localImage,
			maxNumberDescriptionCandidates,
			"")
		if err != nil {
			log.Fatal(err)
		}

		fmt.Println("\nCaptions from local image: ")
		if len(*localImageDescription.Captions) == 0 {
			fmt.Println("No captions detected.")
		} else {
			for _, caption := range *localImageDescription.Captions {
				fmt.Printf("'%v' with confidence %.2f%%\n", *caption.Text, *caption.Confidence * 100)
			}
		}
}
// END - Describe a local image

/*  Describe a remote image file by:
*    1. Saving the URL as an ImageURL type for passing to AnalyzeImage.
*    2. Defining what to extract from the image by initializing an array of VisualFeatureTypes.
*    3. Calling the Computer Vision service's AnalyzeImage with the:
*       - context
*       - image
*       - features to extract
*       - an empty slice for the Details enumeration
*       - "" to specify the default language ("en") as the output language
*    4. Displaying the image captions and their confidence values.
 */
func DescribeRemoteImage(client computervision.BaseClient, remoteImageURL string) {
	var remoteImage computervision.ImageURL
	remoteImage.URL = &remoteImageURL

	maxNumberDescriptionCandidates := new(int32)
	*maxNumberDescriptionCandidates = 1

	remoteImageDescription, err := client.DescribeImage(
			computerVisionContext,
			remoteImage,
			maxNumberDescriptionCandidates,
			"")
		if err != nil {
			log.Fatal(err)
		}

		fmt.Println("\nCaptions from remote image: ")
		if len(*remoteImageDescription.Captions) == 0 {
			fmt.Println("No captions detected.")
		} else {
			for _, caption := range *remoteImageDescription.Captions {
				fmt.Printf("'%v' with confidence %.2f%%\n", *caption.Text, *caption.Confidence * 100)
			}
		}
}
// END - Describe a remote image

/*  Categorize a local image by:
 *    1. Instantiating a ReadCloser, which is required by AnalyzeImageInStream.
 *    2. Opening the ReadCloser instance for reading.
 *    3. Defining what to extract from the image by initializing an array of VisualFeatureTypes.
 *    4. Calling the Computer Vision service's AnalyzeImageInStream with the:
 *       - context
 *       - image
 *       - features to extract
 *       - an empty slice for the Details enumeration
 *       - "" to specify the default language ("en") as the output language
 *    5. Displaying the image categories and their confidence values.
 */
func CategorizeLocalImage(client computervision.BaseClient, localImagePath string) {
	var localImage io.ReadCloser
	localImage, err := os.Open(localImagePath)
	if err != nil {
		log.Fatal(err)
	}

	features := []computervision.VisualFeatureTypes{computervision.VisualFeatureTypesCategories}
	imageAnalysis, err := client.AnalyzeImageInStream(
			computerVisionContext,
			localImage,
			features,
			[]computervision.Details{},
			"")
		if err != nil {
			log.Fatal(err)
		}

	fmt.Println("\nCategories from local image: ")
	if len(*imageAnalysis.Categories) == 0 {
		fmt.Println("No categories detected.")
	} else {
		for _, category := range *imageAnalysis.Categories {
			fmt.Printf("'%v' with confidence %.2f%%\n", *category.Name, *category.Score * 100)
		}
	}
}
// END - Categorize a local image


/*  Categorize a remote image by:
*    1. Saving the URL as an ImageURL type for passing to AnalyzeImage.
*    2. Defining what to extract from the image by initializing an array of VisualFeatureTypes.
*    3. Calling the Computer Vision service's AnalyzeImage with the:
*       - context
*       - image
*       - features to extract
*       - an empty slice for the Details enumeration
*       - "" to specify the default language ("en") as the output language
*    4. Displaying the image categories and their confidence values.
 */
func CategorizeRemoteImage(client computervision.BaseClient, remoteImageURL string) {
	var remoteImage computervision.ImageURL
	remoteImage.URL = &remoteImageURL

	features := []computervision.VisualFeatureTypes{computervision.VisualFeatureTypesCategories}
	imageAnalysis, err := client.AnalyzeImage(
			computerVisionContext,
			remoteImage,
			features,
			[]computervision.Details{},
			"")
		if err != nil {
			log.Fatal(err)
		}

	fmt.Println("\nCategories from remote image: ")
	if len(*imageAnalysis.Categories) == 0 {
		fmt.Println("No categories detected.")
	} else {
		for _, category := range *imageAnalysis.Categories {
			fmt.Printf("'%v' with confidence %.2f%%\n", *category.Name, *category.Score * 100)
		}
	}
}
// END - Categorize a remote image


/*  Tag a local image by:
 *    1. Instantiating a ReadCloser, which is required by TagImageInStream.
 *    2. Opening the ReadCloser instance for reading.
 *    3. Calling the Computer Vision service's AnalyzeImageInStream with the:
 *       - context
 *       - image
 *       - "" to specify the default language ("en") as the output language
 *    4. Displaying the image categories and their confidence values.
 */
func TagLocalImage(client computervision.BaseClient, localImagePath string) {
	var localImage io.ReadCloser
	localImage, err := os.Open(localImagePath)
	if err != nil {
		log.Fatal(err)
	}

	localImageTags, err := client.TagImageInStream(
			computerVisionContext,
			localImage,
			"")
		if err != nil {
			log.Fatal(err)
		}

		fmt.Println("\nTags in the local image: ")
		if len(*localImageTags.Tags) == 0 {
			fmt.Println("No tags detected.")
		} else {
			for _, tag := range *localImageTags.Tags {
				fmt.Printf("'%v' with confidence %.2f%%\n", *tag.Name, *tag.Confidence * 100)
			}
		}
	}
// END - Tag a local image


/*  Tag a remote image file by:
 *    1. Saving the URL as an ImageURL type for passing to AnalyzeImage.
 *    2. Calling the Computer Vision service's TagImage with the:
 *       - context
 *       - image
 *       - "" to specify the default language ("en") as the output language
 *    3. Displaying the image categories and their confidence values.
 */
func TagRemoteImage(client computervision.BaseClient, remoteImageURL string) {
	var remoteImage computervision.ImageURL
	remoteImage.URL = &remoteImageURL

	remoteImageTags, err := client.TagImage(
			computerVisionContext,
			remoteImage,
			"")
		if err != nil {
			log.Fatal(err)
		}

		fmt.Println("\nTags in the remote image: ")
		if len(*remoteImageTags.Tags) == 0 {
			fmt.Println("No tags detected.")
		} else {
			for _, tag := range *remoteImageTags.Tags {
				fmt.Printf("'%v' with confidence %.2f%%\n", *tag.Name, *tag.Confidence * 100)
			}
		}
}
// END - Tag a remote image


/*  Detect faces in a local image by:
 *    1. Instantiating a ReadCloser, which is required by AnalyzeImageInStream.
 *    2. Opening the ReadCloser instance for reading.
 *    3. Defining what to extract from the image by initializing an array of VisualFeatureTypes.
 *    4. Calling the Computer Vision service's AnalyzeImageInStream with the:
 *       - context
 *       - image
 *       - features to extract
 *       - an empty slice for the Details enumeration
 *       - "" to specify the default language ("en") as the output language
 *    5. Displaying the faces and their bounding boxes.
 */
func DetectFacesLocalImage(client computervision.BaseClient, localImagePath string) {
	var localImage io.ReadCloser
	localImage, err := os.Open(localImagePath)
	if err != nil {
		log.Fatal(err)
	}

	features := []computervision.VisualFeatureTypes{computervision.VisualFeatureTypesFaces}

	imageAnalysis, err := client.AnalyzeImageInStream(
			computerVisionContext,
			localImage,
			features,
			[]computervision.Details{},
			"")
		if err != nil {
			log.Fatal(err)
		}

		fmt.Println("\nDetecting faces in a local image ...")
		if len(*imageAnalysis.Faces) == 0 {
			fmt.Println("No faces detected.")
		} else {
			for _, face := range *imageAnalysis.Faces {
				fmt.Printf("'%v' of age %v at location (%v, %v), (%v, %v)\n",
					face.Gender, *face.Age,
					*face.FaceRectangle.Left, *face.FaceRectangle.Top,
					*face.FaceRectangle.Left + *face.FaceRectangle.Width,
					*face.FaceRectangle.Top + *face.FaceRectangle.Height)
			}
		}
}
// END - Detect faces in a local image


/*  Detect faces in a remote image file by:
*    1. Saving the URL as an ImageURL type for passing to AnalyzeImage.
*    2. Defining what to extract from the image by initializing an array of VisualFeatureTypes.
*    3. Calling the Computer Vision service's AnalyzeImage with the:
*       - context
*       - image
*       - features to extract
*       - an empty slice for the Details enumeration
*       - "" to specify the default language ("en") as the output language
*    4. Displaying the image categories and their confidence values.
 */
func DetectFacesRemoteImage(client computervision.BaseClient, remoteImageURL string) {
	var remoteImage computervision.ImageURL
	remoteImage.URL = &remoteImageURL

	features := []computervision.VisualFeatureTypes{computervision.VisualFeatureTypesFaces}
	imageAnalysis, err := client.AnalyzeImage(
			computerVisionContext,
			remoteImage,
			features,
			[]computervision.Details{},
			"")
		if err != nil {
			log.Fatal(err)
		}

	fmt.Println("\nDetecting faces in a remote image ...")
	if len(*imageAnalysis.Faces) == 0 {
		fmt.Println("No faces detected.")
	} else {
		for _, face := range *imageAnalysis.Faces {
			fmt.Printf("'%v' of age %v at location (%v, %v), (%v, %v)\n",
				face.Gender, *face.Age,
				*face.FaceRectangle.Left, *face.FaceRectangle.Top,
				*face.FaceRectangle.Left + *face.FaceRectangle.Width,
				*face.FaceRectangle.Top + *face.FaceRectangle.Height)
		}
	}
}
// END - Detect faces in a remote image


/*  Detect adult or racy content in a local image by:
 *    1. Instantiating a ReadCloser, which is required by AnalyzeImageInStream.
 *    2. Opening the ReadCloser instance for reading.
 *    3. Defining what to extract from the image by initializing an array of VisualFeatureTypes.
 *    4. Calling the Computer Vision service's AnalyzeImageInStream with the:
 *       - context
 *       - image
 *       - features to extract
 *       - an empty slice for the Details enumeration
 *       - "" to specify the default language ("en") as the output language
 *    5. Displaying the faces and their bounding boxes.
 */
func DetectAdultOrRacyContentLocalImage(client computervision.BaseClient, localImagePath string) {
	var localImage io.ReadCloser
	localImage, err := os.Open(localImagePath)
	if err != nil {
		log.Fatal(err)
	}

	features := []computervision.VisualFeatureTypes{computervision.VisualFeatureTypesAdult}
	imageAnalysis, err := client.AnalyzeImageInStream(
			computerVisionContext,
			localImage,
			features,
			[]computervision.Details{},
			"")
		if err != nil {
			log.Fatal(err)
		}

		fmt.Println("\nAnalyzing local image for adult or racy content: ");
		fmt.Printf("Is adult content: %v with confidence %.2f%%\n", *imageAnalysis.Adult.IsAdultContent, *imageAnalysis.Adult.AdultScore * 100)
		fmt.Printf("Has racy content: %v with confidence %.2f%%\n", *imageAnalysis.Adult.IsRacyContent, *imageAnalysis.Adult.RacyScore * 100)
}
// END - Detect adult or racy content in a local image


/*  Detect adult or racy content in a remote image file by:
*    1. Saving the URL as an ImageURL type for passing to AnalyzeImage.
*    2. Defining what to extract from the image by initializing an array of VisualFeatureTypes.
*    3. Calling the Computer Vision service's AnalyzeImage with the:
*       - context
*       - image
*       - features to extract
*       - an empty slice for the Details enumeration
*       - "" to specify the default language ("en") as the output language
*    4. Displaying the image categories and their confidence values.
 */
func DetectAdultOrRacyContentRemoteImage(client computervision.BaseClient, remoteImageURL string) {
	var remoteImage computervision.ImageURL
	remoteImage.URL = &remoteImageURL

	features := []computervision.VisualFeatureTypes{computervision.VisualFeatureTypesAdult}
	imageAnalysis, err := client.AnalyzeImage(
			computerVisionContext,
			remoteImage,
			features,
			[]computervision.Details{},
			"")
		if err != nil {
			log.Fatal(err)
		}

		fmt.Println("\nAnalyzing remote image for adult or racy content: ");
		fmt.Printf("Is adult content: %v with confidence %.2f%%\n", *imageAnalysis.Adult.IsAdultContent, *imageAnalysis.Adult.AdultScore * 100)
		fmt.Printf("Has racy content: %v with confidence %.2f%%\n", *imageAnalysis.Adult.IsRacyContent, *imageAnalysis.Adult.RacyScore * 100)
}
// END - Detect adult or racy content in a remote image


/*  Detect the color scheme of a local image by:
 *    1. Instantiating a ReadCloser, which is required by AnalyzeImageInStream.
 *    2. Opening the ReadCloser instance for reading.
 *    3. Defining what to extract from the image by initializing an array of VisualFeatureTypes.
 *    4. Calling the Computer Vision service's AnalyzeImageInStream with the:
 *       - context
 *       - image
 *       - features to extract
 *       - an empty slice for the Details enumeration
 *       - "" to specify the default language ("en") as the output language
 *    5. Displaying the faces and their bounding boxes.
 */
func DetectColorSchemeLocalImage(client computervision.BaseClient, localImagePath string) {
	var localImage io.ReadCloser
	localImage, err := os.Open(localImagePath)
	if err != nil {
		log.Fatal(err)
	}

	features := []computervision.VisualFeatureTypes{computervision.VisualFeatureTypesColor}
	imageAnalysis, err := client.AnalyzeImageInStream(
			computerVisionContext,
			localImage,
			features,
			[]computervision.Details{},
			"")
		if err != nil {
			log.Fatal(err)
		}

	fmt.Println("\nColor scheme of the local image: ");
	fmt.Printf("Is black and white: %v\n", *imageAnalysis.Color.IsBWImg)
	fmt.Printf("Accent color: 0x%v\n", *imageAnalysis.Color.AccentColor)
	fmt.Printf("Dominant background color: %v\n", *imageAnalysis.Color.DominantColorBackground)
	fmt.Printf("Dominant foreground color: %v\n", *imageAnalysis.Color.DominantColorForeground)
	fmt.Printf("Dominant colors: %v\n", strings.Join(*imageAnalysis.Color.DominantColors, ", "))
}
// END - Detect the color scheme of a local image


/*  Detect the color scheme of a remote image file by:
*    1. Saving the URL as an ImageURL type for passing to AnalyzeImage.
*    2. Defining what to extract from the image by initializing an array of VisualFeatureTypes.
*    3. Calling the Computer Vision service's AnalyzeImage with the:
*       - context
*       - image
*       - features to extract
*       - an empty slice for the Details enumeration
*       - "" to specify the default language ("en") as the output language
*    4. Displaying the image categories and their confidence values.
 */
func DetectColorSchemeRemoteImage(client computervision.BaseClient, remoteImageURL string) {
	var remoteImage computervision.ImageURL
	remoteImage.URL = &remoteImageURL

	features := []computervision.VisualFeatureTypes{computervision.VisualFeatureTypesColor}
	imageAnalysis, err := client.AnalyzeImage(
			computerVisionContext,
			remoteImage,
			features,
			[]computervision.Details{},
			"")
		if err != nil {
			log.Fatal(err)
		}

		fmt.Println("\nColor scheme of the remote image: ");
		fmt.Printf("Is black and white: %v\n", *imageAnalysis.Color.IsBWImg)
		fmt.Printf("Accent color: 0x%v\n", *imageAnalysis.Color.AccentColor)
		fmt.Printf("Dominant background color: %v\n", *imageAnalysis.Color.DominantColorBackground)
		fmt.Printf("Dominant foreground color: %v\n", *imageAnalysis.Color.DominantColorForeground)
		fmt.Printf("Dominant colors: %v\n", strings.Join(*imageAnalysis.Color.DominantColors, ", "))
}
// END - Detect the color scheme of a remote image


/*  Detect domain-specific content (celebrities, landmarks) in a local image by:
 *    1. Instantiating a ReadCloser, which is required by AnalyzeImageInStream.
 *    2. Opening the ReadCloser instance for reading.
 *    3. Calling the Computer Vision service's AnalyzeImageByDomainInStream with the:
 *       - context
 *		   - domain-specific content to extract
 *       - image
 *       - "" to specify the default language ("en") as the output language
 *    4. Decoding the data returned from AnalyzeImageByDomainInStream.
 *    5. Displaying the celebrities/landmarks and their bounding boxes.
 */
func DetectDomainSpecificContentLocalImage(client computervision.BaseClient, localImagePath string) {
	var localImage io.ReadCloser
	localImage, err := os.Open(localImagePath)
	if err != nil {
		log.Fatal(err)
	}

	fmt.Println("\nDetecting domain-specific content in the local image ...")

	celebrities, err := client.AnalyzeImageByDomainInStream(
			computerVisionContext,
			"celebrities",
			localImage,
			"")
		if err != nil {
			log.Fatal(err)
		}

	fmt.Println("\nCelebrities: ")

	// Marshal the output from AnalyzeImageByDomainInStream into JSON.
	data, err := json.MarshalIndent(celebrities.Result, "", "\t")

	// Define structs for which to unmarshal the JSON.
	type Celebrities struct {
		Name string `json:"name"`
	}

	type CelebrityResult struct {
		Celebrities	[]Celebrities `json:"celebrities"`
	}

	var celebrityResult CelebrityResult

	// Unmarshal the data.
	err = json.Unmarshal(data, &celebrityResult)
	if err != nil {
		log.Fatal(err)
	}

	//	Check if any celebrities detected
	if len(celebrityResult.Celebrities) == 0 {
		fmt.Println("No celebrities detected.")
	}	else {
		for _, celebrity := range celebrityResult.Celebrities {
			fmt.Printf("name: %v\n", celebrity.Name)
		}
	}

	fmt.Println("\nLandmarks: ")

	localImage, err = os.Open(localImagePath)
	if err != nil {
		log.Fatal(err)
	}

	landmarks, err := client.AnalyzeImageByDomainInStream(
			computerVisionContext,
			"landmarks",
			localImage,
			"")
		if err != nil {
			log.Fatal(err)
		}

	// Marshal the output from AnalyzeImageByDomainInStream into JSON.
	data, err = json.MarshalIndent(landmarks.Result, "", "\t")

	// Define structs for which to unmarshal the JSON.
	type Landmarks struct {
		Name string `json:"name"`
	}

	type LandmarkResult struct {
		Landmarks	[]Landmarks `json:"landmarks"`
	}

	var landmarkResult LandmarkResult

	// Unmarshal the data.
	err = json.Unmarshal(data, &landmarkResult)
	if err != nil {
		log.Fatal(err)
	}

	//	Check if any landmarks detected
	if len(landmarkResult.Landmarks) == 0 {
		fmt.Println("No landmarks detected.")
	}	else {
		for _, landmark := range landmarkResult.Landmarks {
			fmt.Printf("name: %v\n", landmark.Name)
		}
	}
}
// END - Detect domain-specific content in a local image


/*  Detect domain-specific content (celebrities, landmarks) in remote image file by:
*    1. Saving the URL as an ImageURL type for passing to AnalyzeImage.
*    2. Calling the Computer Vision service's AnalyzeImageByDomain with the:
*       - context
*	- domain-specific content to extract
*       - image
*       - "" to specify the default language ("en") as the output language
*    3. Decoding the data returned from AnalyzeImageByDomain.
*    4. Displaying the celebrities/landmarks and their bounding boxes.
*/
func DetectDomainSpecificContentRemoteImage(client computervision.BaseClient, remoteImageURL string) {
	var remoteImage computervision.ImageURL
	remoteImage.URL = &remoteImageURL

	fmt.Println("\nDetecting domain-specific content in the local image ...")

	celebrities, err := client.AnalyzeImageByDomain(
			computerVisionContext,
			"celebrities",
			remoteImage,
			"")
		if err != nil {
			log.Fatal(err)
		}

	fmt.Println("\nCelebrities: ")

	// Marshal the output from AnalyzeImageByDomain into JSON.
	data, err := json.MarshalIndent(celebrities.Result, "", "\t")

	// Define structs for which to unmarshal the JSON.
	type Celebrities struct {
		Name string `json:"name"`
	}

	type CelebrityResult struct {
		Celebrities	[]Celebrities `json:"celebrities"`
	}

	var celebrityResult CelebrityResult

	// Unmarshal the data.
	err = json.Unmarshal(data, &celebrityResult)
	if err != nil {
		log.Fatal(err)
	}

	//	Check if any celebrities detected
	if len(celebrityResult.Celebrities) == 0 {
		fmt.Println("No celebrities detected.")
	}	else {
		for _, celebrity := range celebrityResult.Celebrities {
			fmt.Printf("name: %v\n", celebrity.Name)
		}
	}

	fmt.Println("\nLandmarks: ")

	landmarks, err := client.AnalyzeImageByDomain(
			computerVisionContext,
			"landmarks",
			remoteImage,
			"")
		if err != nil {
			log.Fatal(err)
		}

	// Marshal the output from AnalyzeImageByDomain into JSON.
	data, err = json.MarshalIndent(landmarks.Result, "", "\t")

	// Define structs for which to unmarshal the JSON.
	type Landmarks struct {
		Name string `json:"name"`
	}

	type LandmarkResult struct {
		Landmarks	[]Landmarks `json:"landmarks"`
	}

	var landmarkResult LandmarkResult

	// Unmarshal the data.
	err = json.Unmarshal(data, &landmarkResult)
	if err != nil {
		log.Fatal(err)
	}

	// Check if any celebrities detected
	if len(landmarkResult.Landmarks) == 0 {
		fmt.Println("No landmarks detected.")
	}	else {
		for _, landmark := range landmarkResult.Landmarks {
			fmt.Printf("name: %v\n", landmark.Name)
		}
	}
}
// END - Detect domain-specific content in a remote image


/*  Detect the image type (clip art, line drawing) of a local image by:
 *    1. Instantiating a ReadCloser, which is required by AnalyzeImageInStream.
 *    2. Opening the ReadCloser instance for reading.
 *    3. Defining what to extract from the image by initializing an array of VisualFeatureTypes.
 *    4. Calling the Computer Vision service's AnalyzeImageInStream with the:
 *       - context
 *       - image
 *       - features to extract
 *       - an empty slice for the Details enumeration
 *       - "" to specify the default language ("en") as the output language
 *    5. Displaying the faces and their bounding boxes.
 */
func DetectImageTypesLocalImage(client computervision.BaseClient, localImagePath string) {
	var localImage io.ReadCloser
	localImage, err := os.Open(localImagePath)
	if err != nil {
		log.Fatal(err)
	}

	features := []computervision.VisualFeatureTypes{computervision.VisualFeatureTypesImageType}

	imageAnalysis, err := client.AnalyzeImageInStream(
			computerVisionContext,
			localImage,
			features,
			[]computervision.Details{},
			"")
		if err != nil {
			log.Fatal(err)
		}

		fmt.Println("\nImage type of local image:")

		fmt.Println("\nClip art type: ")
		switch *imageAnalysis.ImageType.ClipArtType {
		case 0:
			fmt.Println("Image is not clip art.")
		case 1:
			fmt.Println("Image is ambiguously clip art.")
		case 2:
			fmt.Println("Image is normal clip art.")
		case 3:
			fmt.Println("Image is good clip art.")
		}

		fmt.Println("\nLine drawing type: ")
		if *imageAnalysis.ImageType.LineDrawingType == 1 {
			fmt.Println("Image is a line drawing.")
		}	else {
			fmt.Println("Image is not a line drawing.")
		}
}
// END - Detect image type of a local image


/*  Detect the image type (clip art, line drawing) of a remote image by:
*    1. Saving the URL as an ImageURL type for passing to AnalyzeImage.
*    2. Defining what to extract from the image by initializing an array of VisualFeatureTypes.
*    3. Calling the Computer Vision service's AnalyzeImage with the:
*       - context
*       - image
*       - features to extract
*       - an enumeration specifying the domain-specific details to return
*       - "" to specify the default language ("en") as the output language
*    4. Displaying the image categories and their confidence values.
 */
func DetectImageTypesRemoteImage(client computervision.BaseClient, remoteImageURL string) {
	var remoteImage computervision.ImageURL
	remoteImage.URL = &remoteImageURL

	features := []computervision.VisualFeatureTypes{computervision.VisualFeatureTypesImageType}

	imageAnalysis, err := client.AnalyzeImage(
			computerVisionContext,
			remoteImage,
			features,
			[]computervision.Details{},
			"")
		if err != nil {
			log.Fatal(err)
		}

		fmt.Println("\nImage type of remote image:")

		fmt.Println("\nClip art type: ")
		switch *imageAnalysis.ImageType.ClipArtType {
		case 0:
			fmt.Println("Image is not clip art.")
		case 1:
			fmt.Println("Image is ambiguously clip art.")
		case 2:
			fmt.Println("Image is normal clip art.")
		case 3:
			fmt.Println("Image is good clip art.")
		}

		fmt.Println("\nLine drawing type: ")
		if *imageAnalysis.ImageType.LineDrawingType == 1 {
			fmt.Println("Image is a line drawing.")
		}	else {
			fmt.Println("Image is not a line drawing.")
		}
	}
// END - Detect image type of a remote image


/*  Detect objects in a local image by:
 *    1. Instantiating a ReadCloser, which is required by DetectObjectsInStream.
 *    2. Opening the ReadCloser instance for reading.
 *    3. Calling the Computer Vision service's DetectObjectsInStream with the:
 *       - context
 *       - image
 *    4. Displaying the objects and their bounding boxes.
 */
func DetectObjectsLocalImage(client computervision.BaseClient, localImagePath string) {
	var localImage io.ReadCloser
	localImage, err := os.Open(localImagePath)
	if err != nil {
		log.Fatal(err)
	}

	imageAnalysis, err := client.DetectObjectsInStream(
			computerVisionContext,
			localImage,
			)
		if err != nil {
			log.Fatal(err)
		}

		fmt.Println("\nDetecting objects in local image: ")
		if len(*imageAnalysis.Objects) == 0 {
			fmt.Println("No objects detected.")
		} else {
			for _, object := range *imageAnalysis.Objects {
				fmt.Printf("'%v' with confidence %.2f%% at location (%v, %v), (%v, %v)\n",
					*object.Object, *object.Confidence * 100,
					*object.Rectangle.X, *object.Rectangle.X + *object.Rectangle.W,
					*object.Rectangle.Y, *object.Rectangle.Y + *object.Rectangle.H)
			}
		}
}
// END - Detect objects in local image


/*  Detect objects in a remote image by:
*    1. Saving the URL as an ImageURL type for passing to DetectObjects.
*    2. Calling the Computer Vision service's DetectObjects with the:
*       - context
*       - image
*    3. Displaying the objects and their bounding boxes.
 */
func DetectObjectsRemoteImage(client computervision.BaseClient, remoteImageURL string) {
	var remoteImage computervision.ImageURL
	remoteImage.URL = &remoteImageURL

	imageAnalysis, err := client.DetectObjects(
			computerVisionContext,
			remoteImage,
	)
	if err != nil {
		log.Fatal(err)
	}

	fmt.Println("\nDetecting objects in remote image: ")
	if len(*imageAnalysis.Objects) == 0 {
		fmt.Println("No objects detected.")
	} else {
		for _, object := range *imageAnalysis.Objects {
			fmt.Printf("'%v' with confidence %.2f%% at location (%v, %v), (%v, %v)\n",
				*object.Object, *object.Confidence * 100,
				*object.Rectangle.X, *object.Rectangle.X + *object.Rectangle.W,
				*object.Rectangle.Y, *object.Rectangle.Y + *object.Rectangle.H)
		}
	}
}
// END - Detect objects in remote image


/*  Detect brands in a local image by:
 *    1. Instantiating a ReadCloser, which is required by AnalyzeImageInStream.
 *    2. Opening the ReadCloser instance for reading.
 *    3. Defining what to extract from the image by initializing an array of VisualFeatureTypes.
 *    4. Calling the Computer Vision service's AnalyzeImageInStream with the:
 *       - context
 *       - image
 *       - features to extract
 *       - an empty slice for the Details enumeration
 *       - "" to specify the default language ("en") as the output language
 *    5. Displaying the brands, confidence values, and their bounding boxes.
 */
func DetectBrandsLocalImage(client computervision.BaseClient, localImagePath string) {
	var localImage io.ReadCloser
	localImage, err := os.Open(localImagePath)
	if err != nil {
		log.Fatal(err)
	}

	features := []computervision.VisualFeatureTypes{computervision.VisualFeatureTypesBrands}

	imageAnalysis, err := client.AnalyzeImageInStream(
			computerVisionContext,
			localImage,
			features,
			[]computervision.Details{},
			"en")
		if err != nil {
			log.Fatal(err)
		}

		fmt.Println("\nDetecting brands in local image: ")
		if len(*imageAnalysis.Brands) == 0 {
			fmt.Println("No brands detected.")
		} else {
			for _, brand := range *imageAnalysis.Brands {
				fmt.Printf("'%v' with confidence %.2f%% at location (%v, %v), (%v, %v)\n",
					*brand.Name, *brand.Confidence * 100,
		      *brand.Rectangle.X, *brand.Rectangle.X + *brand.Rectangle.W,
		      *brand.Rectangle.Y, *brand.Rectangle.Y + *brand.Rectangle.H)
			}
		}
}
// END - Detect brands in local image


/*  Detect brands in a remote image by:
*    1. Saving the URL as an ImageURL type for passing to AnalyzeImage.
*    2. Defining what to extract from the image by initializing an array of VisualFeatureTypes.
*    3. Calling the Computer Vision service's AnalyzeImage with the:
*       - context
*       - image
*       - features to extract
*       - an enumeration specifying the domain-specific details to return
*       - "" to specify the default language ("en") as the output language
*    5. Displaying the brands, confidence values, and their bounding boxes.
 */
func DetectBrandsRemoteImage(client computervision.BaseClient, remoteImageURL string) {
	var remoteImage computervision.ImageURL
	remoteImage.URL = &remoteImageURL

	features := []computervision.VisualFeatureTypes{computervision.VisualFeatureTypesBrands}

	imageAnalysis, err := client.AnalyzeImage(
		computerVisionContext,
		remoteImage,
		features,
		[]computervision.Details{},
		"en")
	if err != nil {
	 	log.Fatal(err)
	}

	fmt.Println("\nDetecting brands in remote image: ")
	if len(*imageAnalysis.Brands) == 0 {
		fmt.Println("No brands detected.")
	} else {
		for _, brand := range *imageAnalysis.Brands {
			fmt.Printf("'%v' with confidence %.2f%% at location (%v, %v), (%v, %v)\n",
				*brand.Name, *brand.Confidence * 100,
				*brand.Rectangle.X, *brand.Rectangle.X + *brand.Rectangle.W,
				*brand.Rectangle.Y, *brand.Rectangle.Y + *brand.Rectangle.H)
		}
	}
}
// END - Detect brands in remote image


/*  Recognize text with the Read API in a local image by:
 *    1. Instantiating a ReadCloser, which is required by BatchReadFileInStream.
 *    2. Opening the ReadCloser instance for reading.
 *    3. Specifying whether the text to recognize is handwritten or printed.
 *    4. Calling the Computer Vision service's BatchReadFileInStream with the:
 *       - context
 *       - image
 *       - text recognition mode
 *    5. Extracting the Operation-Location URL value from the BatchReadFileInStream
 *       response
 *    6. Waiting for the operation to complete.
 *    7. Displaying the results.
 */
func RecognizeTextReadAPILocalImage(client computervision.BaseClient, localImagePath string) {
	var localImage io.ReadCloser
	localImage, err := os.Open(localImagePath)
	if err != nil {
		log.Fatal(err)
	}

	textRecognitionMode := computervision.Handwritten

	//	When you use the Read Document interface, the response contains a field
	//	called "Operation-Location", which contains the URL to use for your
	//	GetReadOperationResult to access OCR results.
	textHeaders, err := client.BatchReadFileInStream(
		computerVisionContext,
		localImage,
		textRecognitionMode)
	if err != nil {
		log.Fatal(err)
	}

	//	Use ExtractHeader from the autorest library to get the Operation-Location URL
	operationLocation := autorest.ExtractHeaderValue("Operation-Location", textHeaders.Response)

	numberOfCharsInOperationId := 36
	operationId := string(operationLocation[len(operationLocation)-numberOfCharsInOperationId : len(operationLocation)])

	readOperationResult, err := client.GetReadOperationResult(computerVisionContext, operationId)
	if err != nil {
		log.Fatal(err)
	}

	// Wait for the operation to complete.
	i := 0
	maxRetries := 10

	fmt.Println("\nRecognizing text in a local image with the batch Read API ... \n")
	for readOperationResult.Status != computervision.Failed &&
			readOperationResult.Status != computervision.Succeeded {
		if i >= maxRetries {
			break
		}
		i++

		fmt.Printf("Server status: %v, waiting %v seconds...\n", readOperationResult.Status, i)
		time.Sleep(1 * time.Second)

		readOperationResult, err = client.GetReadOperationResult(computerVisionContext, operationId)
		if err != nil {
			log.Fatal(err)
		}
	}

	// Display the results.
	fmt.Println()
	for _, recResult := range *(readOperationResult.RecognitionResults) {
		for _, line := range *recResult.Lines {
			fmt.Println(*line.Text)
		}
	}
}
// END - Recognize text with the Read API in a local image


/*  Recognize text with the Read API in a remote image by:
 *    1. Saving the URL as an ImageURL type for passing to BatchReadFile.
 *    2. Specifying whether the text to recognize is handwritten or printed.
 *    3. Calling the Computer Vision service's BatchReadFile with the:
 *       - context
 *       - image
 *       - text recognition mode
 *    4. Extracting the Operation-Location URL value from the BatchReadFile
 *       response
 *    5. Waiting for the operation to complete.
 *    6. Displaying the results.
 */
func RecognizeTextReadAPIRemoteImage(client computervision.BaseClient, remoteImageURL string) {
	var remoteImage computervision.ImageURL
	remoteImage.URL = &remoteImageURL

	textRecognitionMode := computervision.Printed

	// When you use the Read Document interface, the response contains a field
	// called "Operation-Location", which contains the URL to use for your
	// GetReadOperationResult to access OCR results.
	textHeaders, err := client.BatchReadFile(
		computerVisionContext,
		remoteImage,
		textRecognitionMode)
	if err != nil {
		log.Fatal(err)
	}

	// Use ExtractHeader from the autorest library to get the Operation-Location URL
	operationLocation := autorest.ExtractHeaderValue("Operation-Location", textHeaders.Response)

	numberOfCharsInOperationId := 36
	operationId := string(operationLocation[len(operationLocation)-numberOfCharsInOperationId : len(operationLocation)])

	readOperationResult, err := client.GetReadOperationResult(computerVisionContext, operationId)
	if err != nil {
		log.Fatal(err)
	}

	// Wait for the operation to complete.
	i := 0
	maxRetries := 10

	fmt.Println("\nRecognizing text in a remote image with the batch Read API ... \n")
	for readOperationResult.Status != computervision.Failed &&
			readOperationResult.Status != computervision.Succeeded {
		if i >= maxRetries {
			break
		}
		i++

		fmt.Printf("Server status: %v, waiting %v seconds...\n", readOperationResult.Status, i)
		time.Sleep(1 * time.Second)

		readOperationResult, err = client.GetReadOperationResult(computerVisionContext, operationId)
		if err != nil {
			log.Fatal(err)
		}
	}

	// Display the results.
	fmt.Println()
	for _, recResult := range *(readOperationResult.RecognitionResults) {
		for _, line := range *recResult.Lines {
			fmt.Println(*line.Text)
		}
	}
}


/*  Extract text with OCR from a local image by:
 *    1. Instantiating a ReadCloser, which is required by AnalyzeImageInStream.
 *    2. Opening the ReadCloser instance for reading.
 *    3. Calling the Computer Vision service's RecognizePrintedTextInStream with the:
 *       - context
 *	 - whether to detect the text orientation
 *       - image
 *       - language
 *    4. Displaying the brands, confidence values, and their bounding boxes.
 */
func ExtractTextOCRLocalImage(client computervision.BaseClient, localImagePath string) {
	var localImage io.ReadCloser
	localImage, err := os.Open(localImagePath)
	if err != nil {
		log.Fatal(err)
	}

	fmt.Println("\nRecognizing text in a local image with OCR ... \n")
	ocrResult, err := client.RecognizePrintedTextInStream(computerVisionContext, true, localImage, computervision.En)
	if err != nil {
		log.Fatal(err)
	}

	fmt.Printf("Text angle: %.4f\n", *ocrResult.TextAngle)

	for _, region := range *ocrResult.Regions {
		for _, line := range *region.Lines {
			fmt.Printf("\nBounding box: %v\n", *line.BoundingBox)
			s := ""
			for _, word := range *line.Words {
				s += *word.Text + " "
			}
			fmt.Printf("Text: %v\n", s)
		}
	}
}


/*  Extract text with OCR from a local image by:
 *    1. Saving the URL as an ImageURL type for passing to AnalyzeImage.
 *    2. Calling the Computer Vision service's RecognizePrintedTextInStream with the:
 *       - context
 *	 - whether to detect the text orientation
 *       - image
 *       - language
 *    3. Displaying the brands, confidence values, and their bounding boxes.
 */
func ExtractTextOCRRemoteImage(client computervision.BaseClient, remoteImageURL string) {
	var remoteImage computervision.ImageURL
	remoteImage.URL = &remoteImageURL

	fmt.Println("\nRecognizing text in a remote image with OCR ... \n")
	ocrResult, err := client.RecognizePrintedText(computerVisionContext, true, remoteImage, computervision.En)
	if err != nil {
		log.Fatal(err)
	}

	fmt.Printf("Text angle: %.4f\n", *ocrResult.TextAngle)

	for _, region := range *ocrResult.Regions {
		for _, line := range *region.Lines {
			fmt.Printf("\nBounding box: %v\n", *line.BoundingBox)
			s := ""
			for _, word := range *line.Words {
				s += *word.Text + " "
			}
			fmt.Printf("Text: %v\n", s)
		}
	}
}