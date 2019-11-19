// <snippet_imports>
package main

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
// </snippet_imports>

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
 *  - Generate Thumbnail
 *  - Recognizing printed and handwritten text with the Batch Read API
 *	- Recognizing printed text with OCR
 *
 *  Prerequisites:
 *    Import the required libraries. From the command line, you will need to 'go get' 
 *    the azure-sdk-for-go and go-autorest packages from Github.
 *    For example:
 *	  go get github.com/Azure/azure-sdk-for-go/services/cognitiveservices/v2.0/computervision
 * 
 *    Download images faces.jpg, handwritten_text.jpg, objects.jpg, cheese_clipart.png,
 *    printed_text.jpg, and gray-shirt-logo.jpg, then add to your root folder from here:
 *    https://github.com/Azure-Samples/cognitive-services-sample-data-files/tree/master/ComputerVision/Images
 *   
 *    Add your Azure Computer Vision subscription key and endpoint to your environment variables with names:
 *    COMPUTER_VISION_SUBSCRIPTION_KEY and COMPUTER_VISION_ENDPOINT
 *
 *  How to run:
 *	  From command line: go run ComptuerVisionQuickstart.go
 * 
 *  References:
 *    - SDK reference: 
 *      https://godoc.org/github.com/Azure/azure-sdk-for-go/services/cognitiveservices/v2.0/computervision
 *    - Computer Vision documentation:
 * 		https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/index
 *    - Computer Vision API: 
 *      https://westus.dev.cognitive.microsoft.com/docs/services/5cd27ec07268f6c679a3e641/operations/56f91f2e778daf14a499f21b
 */

 // <snippet_context>
// Declare global so don't have to pass it to all of the tasks.
var computerVisionContext context.Context
// </snippet_context>

func main() {

	/*
	 * Local image file I/O
	 * Store these in your root directory in a "resources" folder.
	 */ 
	facesImagePath := "resources\\faces.jpg"
	brandsImagePath := "resources\\gray-shirt-logo.jpg"
	objectsImagePath := "resources\\objects.jpg"
	clipartImagePath := "resources\\cheese_clipart.png"
	handwritingImagePath := "resources\\handwritten_text.jpg"
	printedImagePath := "resources\\printed_text.jpg"
	/*
	 * END - Local image file I/O
	 */

	/*
	 * URL images
	 */ 
	facesImageURL := "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/faces.jpg"
	// <snippet_analyze_url>
	landmarkImageURL := "https://github.com/Azure-Samples/cognitive-services-sample-data-files/raw/master/ComputerVision/Images/landmark.jpg"
	// </snippet_analyze_url>
	objectsImageURL := "https://moderatorsampleimages.blob.core.windows.net/samples/sample6.png"
	// <snippet_brand_url>
	brandsImageURL := "https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/images/gray-shirt-logo.jpg"
	// </snippet_brand_url>
	adultRacyImageURL := "https://moderatorsampleimages.blob.core.windows.net/samples/sample3.png"
	detectTypeImageURL := "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/lion_drawing.png"
	printedImageURL := "https://moderatorsampleimages.blob.core.windows.net/samples/sample2.jpg"
	/*
	 * END - URL images
	 */

	// <snippet_client>
	/*  
	 * Configure the Computer Vision client
	 * Set environment variables for COMPUTER_VISION_SUBSCRIPTION_KEY and COMPUTER_VISION_ENDPOINT,
	 * then restart your command shell or your IDE for changes to take effect.
	 */
	  computerVisionKey := os.Getenv("COMPUTER_VISION_SUBSCRIPTION_KEY")
	
	if (computerVisionKey == "") {
		log.Fatal("\n\nPlease set a COMPUTER_VISION_SUBSCRIPTION_KEY environment variable.\n" +
							  "**You may need to restart your shell or IDE after it's set.**\n")
	}

	endpointURL := os.Getenv("COMPUTER_VISION_ENDPOINT")
	if (endpointURL == "") {
		log.Fatal("\n\nPlease set a COMPUTER_VISION_ENDPOINT environment variable.\n" +
							  "**You may need to restart your shell or IDE after it's set.**")
	}

	computerVisionClient := computervision.New(endpointURL);
	computerVisionClient.Authorizer = autorest.NewCognitiveServicesAuthorizer(computerVisionKey)

	computerVisionContext = context.Background()
	/*
	 * END - Configure the Computer Vision client
	 */
	// </snippet_client>

	// Analyze features of an image, local
	DescribeLocalImage(computerVisionClient, facesImagePath)
	CategorizeLocalImage(computerVisionClient, facesImagePath)
	TagLocalImage(computerVisionClient, facesImagePath)
	DetectFacesLocalImage(computerVisionClient, facesImagePath)
	DetectObjectsLocalImage(computerVisionClient, objectsImagePath)
	DetectBrandsLocalImage(computerVisionClient, brandsImagePath)
	DetectAdultOrRacyContentLocalImage(computerVisionClient, facesImagePath)
	DetectColorSchemeLocalImage(computerVisionClient, objectsImagePath)
	DetectDomainSpecificContentLocalImage(computerVisionClient, facesImagePath)
	DetectImageTypesLocalImage(computerVisionClient, clipartImagePath)
	GenerateThumbnailLocalImage(computerVisionClient, objectsImagePath)

	// <snippet_analyze>
	// Analyze features of an image, remote
	DescribeRemoteImage(computerVisionClient, landmarkImageURL)
	CategorizeRemoteImage(computerVisionClient, landmarkImageURL)
	TagRemoteImage(computerVisionClient, landmarkImageURL)
	DetectFacesRemoteImage(computerVisionClient, facesImageURL)
	DetectObjectsRemoteImage(computerVisionClient, objectsImageURL)
	DetectBrandsRemoteImage(computerVisionClient, brandsImageURL)
	DetectAdultOrRacyContentRemoteImage(computerVisionClient, adultRacyImageURL)
	DetectColorSchemeRemoteImage(computerVisionClient, brandsImageURL)
	DetectDomainSpecificContentRemoteImage(computerVisionClient, landmarkImageURL)
	DetectImageTypesRemoteImage(computerVisionClient, detectTypeImageURL)
	GenerateThumbnailRemoteImage(computerVisionClient, adultRacyImageURL)
	// </snippet_analyze>

	// Analyze text in an image, local
	BatchReadFileLocalImage(computerVisionClient, handwritingImagePath)
	// <snippet_readinmain>
	// Analyze text in an image, remote
	BatchReadFileRemoteImage(computerVisionClient, printedImageURL)
	// </snippet_readinmain>

	// Analyze printed text in an image, local and remote
	RecognizePrintedOCRLocalImage(computerVisionClient, printedImagePath)
	RecognizePrintedOCRRemoteImage(computerVisionClient, printedImageURL)

	fmt.Println("-----------------------------------------")
	fmt.Println("End of quickstart.")
}

/*  
 * Describe Image - local
 */
func DescribeLocalImage(client computervision.BaseClient, localImagePath string) {
	fmt.Println("-----------------------------------------")
	fmt.Println("DESCRIBE IMAGE - local")
	fmt.Println()
	var localImage io.ReadCloser
	localImage, err := os.Open(localImagePath)
	if err != nil { log.Fatal(err) }

	maxNumberDescriptionCandidates := new(int32)
	*maxNumberDescriptionCandidates = 1

	localImageDescription, err := client.DescribeImageInStream(
			computerVisionContext,
			localImage,
			maxNumberDescriptionCandidates,
			"") // language
	if err != nil { log.Fatal(err) }

	fmt.Println("Captions from local image: ")
	if len(*localImageDescription.Captions) == 0 {
		fmt.Println("No captions detected.")
	} else {
		for _, caption := range *localImageDescription.Captions {
			fmt.Printf("'%v' with confidence %.2f%%\n", *caption.Text, *caption.Confidence * 100)
		}
	}
	
	fmt.Println()
}
/*
 * END - Describe Image - local
 */

/*  
 * Describe Image - remote
 */
// <snippet_analyze_describe>
func DescribeRemoteImage(client computervision.BaseClient, remoteImageURL string) {
	fmt.Println("-----------------------------------------")
	fmt.Println("DESCRIBE IMAGE - remote")
	fmt.Println()
	var remoteImage computervision.ImageURL
	remoteImage.URL = &remoteImageURL

	maxNumberDescriptionCandidates := new(int32)
	*maxNumberDescriptionCandidates = 1

	remoteImageDescription, err := client.DescribeImage(
			computerVisionContext,
			remoteImage,
			maxNumberDescriptionCandidates,
			"") // language
		if err != nil { log.Fatal(err) }

	fmt.Println("Captions from remote image: ")
	if len(*remoteImageDescription.Captions) == 0 {
		fmt.Println("No captions detected.")
	} else {
		for _, caption := range *remoteImageDescription.Captions {
			fmt.Printf("'%v' with confidence %.2f%%\n", *caption.Text, *caption.Confidence * 100)
		}
	}
	fmt.Println()
}
// </snippet_analyze_describe>
/*
 * END - Describe Image - remote
 */

/*  
 * Categorize Image - local
 */
func CategorizeLocalImage(client computervision.BaseClient, localImagePath string) {
	fmt.Println("-----------------------------------------")
	fmt.Println("CATEGORIZE IMAGE - local")
	fmt.Println()
	var localImage io.ReadCloser
	localImage, err := os.Open(localImagePath)
	if err != nil { log.Fatal(err) }

	features := []computervision.VisualFeatureTypes{computervision.VisualFeatureTypesCategories}
	imageAnalysis, err := client.AnalyzeImageInStream(
			computerVisionContext,
			localImage,
			features,
			[]computervision.Details{},
			"") // language
	if err != nil { log.Fatal(err) }

	fmt.Println("Categories from local image: ")
	if len(*imageAnalysis.Categories) == 0 {
		fmt.Println("No categories detected.")
	} else {
		for _, category := range *imageAnalysis.Categories {
			fmt.Printf("'%v' with confidence %.2f%%\n", *category.Name, *category.Score * 100)
		}
	}
	fmt.Println()
}
/*
 * END - Categorize Image - local
 */

/*  
 * Categorize Image - remote
 */
// <snippet_analyze_categorize>
func CategorizeRemoteImage(client computervision.BaseClient, remoteImageURL string) {
	fmt.Println("-----------------------------------------")
	fmt.Println("CATEGORIZE IMAGE - remote")
	fmt.Println()
	var remoteImage computervision.ImageURL
	remoteImage.URL = &remoteImageURL

	features := []computervision.VisualFeatureTypes{computervision.VisualFeatureTypesCategories}
	imageAnalysis, err := client.AnalyzeImage(
			computerVisionContext,
			remoteImage,
			features,
			[]computervision.Details{},
			"")
	if err != nil { log.Fatal(err) }

	fmt.Println("Categories from remote image: ")
	if len(*imageAnalysis.Categories) == 0 {
		fmt.Println("No categories detected.")
	} else {
		for _, category := range *imageAnalysis.Categories {
			fmt.Printf("'%v' with confidence %.2f%%\n", *category.Name, *category.Score * 100)
		}
	}
	fmt.Println()
}
// </snippet_analyze_categorize>
/*
 * END - Categorize Image - remote
 */

/*  
 * Tag Image - local
 */
func TagLocalImage(client computervision.BaseClient, localImagePath string) {
	fmt.Println("-----------------------------------------")
	fmt.Println("TAG IMAGE - local")
	fmt.Println()
	var localImage io.ReadCloser
	localImage, err := os.Open(localImagePath)
	if err != nil { log.Fatal(err) }

	localImageTags, err := client.TagImageInStream(
			computerVisionContext,
			localImage,
			"")
	if err != nil { log.Fatal(err) }

	fmt.Println("Tags in the local image: ")
	if len(*localImageTags.Tags) == 0 {
		fmt.Println("No tags detected.")
	} else {
		for _, tag := range *localImageTags.Tags {
			fmt.Printf("'%v' with confidence %.2f%%\n", *tag.Name, *tag.Confidence * 100)
		}
	}
	fmt.Println()
}
/*
 * END - Tag Image - local
 */

/*  
 * Tag Image - remote
 */
// <snippet_tags>
func TagRemoteImage(client computervision.BaseClient, remoteImageURL string) {
	fmt.Println("-----------------------------------------")
	fmt.Println("TAG IMAGE - remote")
	fmt.Println()
	var remoteImage computervision.ImageURL
	remoteImage.URL = &remoteImageURL

	remoteImageTags, err := client.TagImage(
			computerVisionContext,
			remoteImage,
			"")
	if err != nil { log.Fatal(err) }

	fmt.Println("Tags in the remote image: ")
	if len(*remoteImageTags.Tags) == 0 {
		fmt.Println("No tags detected.")
	} else {
		for _, tag := range *remoteImageTags.Tags {
			fmt.Printf("'%v' with confidence %.2f%%\n", *tag.Name, *tag.Confidence * 100)
		}
	}
	fmt.Println()
}
// </snippet_tags>
/*
 * END - Tag Image - remote
 */

/*  
 * Detect Faces - local
 */
func DetectFacesLocalImage(client computervision.BaseClient, localImagePath string) {
	fmt.Println("-----------------------------------------")
	fmt.Println("DETECT FACES - local")
	fmt.Println()
	var localImage io.ReadCloser
	localImage, err := os.Open(localImagePath)
	if err != nil { log.Fatal(err) }

	// Define the features you want returned with the API call.
	features := []computervision.VisualFeatureTypes{computervision.VisualFeatureTypesFaces}

	imageAnalysis, err := client.AnalyzeImageInStream(
			computerVisionContext,
			localImage,
			features,
			[]computervision.Details{},
		"")
	if err != nil { log.Fatal(err) }

	fmt.Println("Detecting faces in a local image ...")
	if len(*imageAnalysis.Faces) == 0 {
		fmt.Println("No faces detected.")
	} else {
		// Print the bounding box locations of the found faces.
		for _, face := range *imageAnalysis.Faces {
			fmt.Printf("'%v' of age %v at location (%v, %v), (%v, %v)\n",
				face.Gender, *face.Age,
				*face.FaceRectangle.Left, *face.FaceRectangle.Top,
				*face.FaceRectangle.Left + *face.FaceRectangle.Width,
				*face.FaceRectangle.Top + *face.FaceRectangle.Height)
		}
	}
	fmt.Println()
}
/*
 * END - Detect Faces - local
 */
 
/*  
 * Detect Faces - remote
 */
// <snippet_faces>
func DetectFacesRemoteImage(client computervision.BaseClient, remoteImageURL string) {
	fmt.Println("-----------------------------------------")
	fmt.Println("DETECT FACES - remote")
	fmt.Println()
	var remoteImage computervision.ImageURL
	remoteImage.URL = &remoteImageURL

	// Define the features you want returned with the API call.
	features := []computervision.VisualFeatureTypes{computervision.VisualFeatureTypesFaces}
	imageAnalysis, err := client.AnalyzeImage(
			computerVisionContext,
			remoteImage,
			features,
			[]computervision.Details{},
			"")
		if err != nil { log.Fatal(err) }

	fmt.Println("Detecting faces in a remote image ...")
	if len(*imageAnalysis.Faces) == 0 {
		fmt.Println("No faces detected.")
	} else {
		// Print the bounding box locations of the found faces.
		for _, face := range *imageAnalysis.Faces {
			fmt.Printf("'%v' of age %v at location (%v, %v), (%v, %v)\n",
				face.Gender, *face.Age,
				*face.FaceRectangle.Left, *face.FaceRectangle.Top,
				*face.FaceRectangle.Left + *face.FaceRectangle.Width,
				*face.FaceRectangle.Top + *face.FaceRectangle.Height)
		}
	}
	fmt.Println()
}
// </snippet_faces>
/*
 * END - Detect Faces - remote
 */
/*  
 * Detect Adult or Racy Content - local
 */
func DetectAdultOrRacyContentLocalImage(client computervision.BaseClient, localImagePath string) {
	fmt.Println("-----------------------------------------")
	fmt.Println("DETECT ADULT OR RACY CONTENT - local")
	fmt.Println()
	var localImage io.ReadCloser
	localImage, err := os.Open(localImagePath)
	if err != nil { log.Fatal(err) }

	// Define the features you want returned with the API call.
	features := []computervision.VisualFeatureTypes{computervision.VisualFeatureTypesAdult}
	imageAnalysis, err := client.AnalyzeImageInStream(
			computerVisionContext,
			localImage,
			features,
			[]computervision.Details{},
			"")
	if err != nil { log.Fatal(err) }

	// Print whether or not there is questionable content.
	// Confidence levels: low means content is OK, high means it's not.
	fmt.Println("Analyzing local image for adult or racy content: ");
	fmt.Printf("Is adult content: %v with confidence %.2f%%\n", *imageAnalysis.Adult.IsAdultContent, *imageAnalysis.Adult.AdultScore * 100)
	fmt.Printf("Has racy content: %v with confidence %.2f%%\n", *imageAnalysis.Adult.IsRacyContent, *imageAnalysis.Adult.RacyScore * 100)
	fmt.Println()
}
/* 
 * END - Detect Adult or Racy Content - local
 */

/*  
 * Detect Adult or Racy Content - remote
 */
// <snippet_adult>
func DetectAdultOrRacyContentRemoteImage(client computervision.BaseClient, remoteImageURL string) {
	fmt.Println("-----------------------------------------")
	fmt.Println("DETECT ADULT OR RACY CONTENT - remote")
	fmt.Println()
	var remoteImage computervision.ImageURL
	remoteImage.URL = &remoteImageURL

	// Define the features you want returned from the API call.
	features := []computervision.VisualFeatureTypes{computervision.VisualFeatureTypesAdult}
	imageAnalysis, err := client.AnalyzeImage(
			computerVisionContext,
			remoteImage,
			features,
			[]computervision.Details{},
			"") // language, English is default
	if err != nil { log.Fatal(err) }

	// Print whether or not there is questionable content.
	// Confidence levels: low means content is OK, high means it's not.
	fmt.Println("Analyzing remote image for adult or racy content: ");
	fmt.Printf("Is adult content: %v with confidence %.2f%%\n", *imageAnalysis.Adult.IsAdultContent, *imageAnalysis.Adult.AdultScore * 100)
	fmt.Printf("Has racy content: %v with confidence %.2f%%\n", *imageAnalysis.Adult.IsRacyContent, *imageAnalysis.Adult.RacyScore * 100)
	fmt.Println()
}
// </snippet_adult>
/* 
 * END - Detect Adult or Racy Content - remote
 */ 


/*  
 * Detect Color Scheme - local
 */
func DetectColorSchemeLocalImage(client computervision.BaseClient, localImagePath string) {
	fmt.Println("-----------------------------------------")
	fmt.Println("DETECT COLOR SCHEME - local")
	fmt.Println()
	var localImage io.ReadCloser
	localImage, err := os.Open(localImagePath)
	if err != nil { log.Fatal(err) }

	// Define the features you want returned with the API call.
	features := []computervision.VisualFeatureTypes{computervision.VisualFeatureTypesColor}
	imageAnalysis, err := client.AnalyzeImageInStream(
			computerVisionContext,
			localImage,
			features,
			[]computervision.Details{},
			"") // language, English is default
	if err != nil { log.Fatal(err) }

	fmt.Println("Color scheme of the local image: ");
	fmt.Printf("Is black and white: %v\n", *imageAnalysis.Color.IsBWImg)
	fmt.Printf("Accent color: 0x%v\n", *imageAnalysis.Color.AccentColor)
	fmt.Printf("Dominant background color: %v\n", *imageAnalysis.Color.DominantColorBackground)
	fmt.Printf("Dominant foreground color: %v\n", *imageAnalysis.Color.DominantColorForeground)
	fmt.Printf("Dominant colors: %v\n", strings.Join(*imageAnalysis.Color.DominantColors, ", "))
	fmt.Println()
}
/*
 * END - Detect Color Scheme - local
 */

/*  
 * Detect Color Scheme - remote
 */
// <snippet_color>
func DetectColorSchemeRemoteImage(client computervision.BaseClient, remoteImageURL string) {
	fmt.Println("-----------------------------------------")
	fmt.Println("DETECT COLOR SCHEME - remote")
	fmt.Println()
	var remoteImage computervision.ImageURL
	remoteImage.URL = &remoteImageURL

	// Define the features you'd like returned with the result.
	features := []computervision.VisualFeatureTypes{computervision.VisualFeatureTypesColor}
	imageAnalysis, err := client.AnalyzeImage(
			computerVisionContext,
			remoteImage,
			features,
			[]computervision.Details{},
			"") // language, English is default
	if err != nil { log.Fatal(err) }

	fmt.Println("Color scheme of the remote image: ");
	fmt.Printf("Is black and white: %v\n", *imageAnalysis.Color.IsBWImg)
	fmt.Printf("Accent color: 0x%v\n", *imageAnalysis.Color.AccentColor)
	fmt.Printf("Dominant background color: %v\n", *imageAnalysis.Color.DominantColorBackground)
	fmt.Printf("Dominant foreground color: %v\n", *imageAnalysis.Color.DominantColorForeground)
	fmt.Printf("Dominant colors: %v\n", strings.Join(*imageAnalysis.Color.DominantColors, ", "))
	fmt.Println()
}
// </snippet_color>
/* 
 * END - Detect Color Scheme - remote
 */

/*  
 * Detect Domain-specific Content - local
 * Detect domain-specific content (celebrities, landmarks) in a local image.
 */
func DetectDomainSpecificContentLocalImage(client computervision.BaseClient, localImagePath string) {
	fmt.Println("-----------------------------------------")
	fmt.Println("DETECT DOMAIN-SPECIFIC CONTENT - local")
	fmt.Println()
	var localImage io.ReadCloser
	localImage, err := os.Open(localImagePath)
	if err != nil { log.Fatal(err) }

	fmt.Println("Detecting domain-specific content in the local image ...")

	celebrities, err := client.AnalyzeImageByDomainInStream(
			computerVisionContext,
			"celebrities",
			localImage,
			"") // language, default is English
		if err != nil { log.Fatal(err) }

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
	if err != nil { log.Fatal(err) }

	//	Check if any celebrities detected.
	if len(celebrityResult.Celebrities) == 0 {
		fmt.Println("No celebrities detected.")
	}	else {
		for _, celebrity := range celebrityResult.Celebrities {
			fmt.Printf("name: %v\n", celebrity.Name)
		}
	}

	fmt.Println("\nLandmarks: ")

	localImage, err = os.Open(localImagePath)
	if err != nil { log.Fatal(err) }

	landmarks, err := client.AnalyzeImageByDomainInStream(
			computerVisionContext,
			"landmarks",
			localImage,
			"")
	if err != nil { log.Fatal(err) }

	// Marshal the output from AnalyzeImageByDomainInStream into JSON.
	data, err = json.MarshalIndent(landmarks.Result, "", "\t")

	// Define structs for which to unmarshal the JSON.
	type Landmarks struct {
		Name string `json:"name"`
	}

	type LandmarkResult struct {
		Landmarks []Landmarks `json:"landmarks"`
	}

	var landmarkResult LandmarkResult

	// Unmarshal the data.
	err = json.Unmarshal(data, &landmarkResult)
	if err != nil { log.Fatal(err) }

	//	Check if any landmarks detected
	if len(landmarkResult.Landmarks) == 0 {
		fmt.Println("No landmarks detected.")
	}	else {
		for _, landmark := range landmarkResult.Landmarks {
			fmt.Printf("name: %v\n", landmark.Name)
		}
	}
	fmt.Println()
}
/*
 * END - Detect Domain-specific Content - local
 */

/*  
 * Detect Domain-specific Content - remote
 */
// <snippet_celebs>
func DetectDomainSpecificContentRemoteImage(client computervision.BaseClient, remoteImageURL string) {
	fmt.Println("-----------------------------------------")
	fmt.Println("DETECT DOMAIN-SPECIFIC CONTENT - remote")
	fmt.Println()
	var remoteImage computervision.ImageURL
	remoteImage.URL = &remoteImageURL

	fmt.Println("Detecting domain-specific content in the local image ...")

	// Check if there are any celebrities in the image.
	celebrities, err := client.AnalyzeImageByDomain(
			computerVisionContext,
			"celebrities",
			remoteImage,
			"") // language, English is default
	if err != nil { log.Fatal(err) }

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
	if err != nil { log.Fatal(err) }

	//	Check if any celebrities detected.
	if len(celebrityResult.Celebrities) == 0 {
		fmt.Println("No celebrities detected.")
	}	else {
		for _, celebrity := range celebrityResult.Celebrities {
			fmt.Printf("name: %v\n", celebrity.Name)
		}
	}
	// </snippet_celebs>

	// <snippet_landmarks>
	fmt.Println("\nLandmarks: ")

	// Check if there are any landmarks in the image.
	landmarks, err := client.AnalyzeImageByDomain(
			computerVisionContext,
			"landmarks",
			remoteImage,
			"")
	if err != nil { log.Fatal(err) }

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
	if err != nil { log.Fatal(err) }

	// Check if any celebrities detected.
	if len(landmarkResult.Landmarks) == 0 {
		fmt.Println("No landmarks detected.")
	}	else {
		for _, landmark := range landmarkResult.Landmarks {
			fmt.Printf("name: %v\n", landmark.Name)
		}
	}
	fmt.Println()
}
// </snippet_landmarks>
/* 
 * END - Detect Domain-specific Content - remote


/*  
 * Detect Image Type - local
 */
func DetectImageTypesLocalImage(client computervision.BaseClient, localImagePath string) {
	fmt.Println("-----------------------------------------")
	fmt.Println("DETECT IMAGE TYPES - local")
	fmt.Println()
	var localImage io.ReadCloser
	localImage, err := os.Open(localImagePath)
	if err != nil { log.Fatal(err) }

	// Select the features you want returned in the response.
	features := []computervision.VisualFeatureTypes{computervision.VisualFeatureTypesImageType}

	imageAnalysis, err := client.AnalyzeImageInStream(
			computerVisionContext,
			localImage,
			features,
			[]computervision.Details{},
			"") // language, default is English
	if err != nil { log.Fatal(err) }

	fmt.Println("Image type of local image:")

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
	fmt.Println()
}
/*
 * END - Detect Image Type - local
 */

/*  
 * Detect Image Type - remote
 * Detect the image type (clip art, line drawing) of a remote image.  
 */
// <snippet_type>
func DetectImageTypesRemoteImage(client computervision.BaseClient, remoteImageURL string) {
	fmt.Println("-----------------------------------------")
	fmt.Println("DETECT IMAGE TYPES - remote")
	fmt.Println()
	var remoteImage computervision.ImageURL
	remoteImage.URL = &remoteImageURL

	features := []computervision.VisualFeatureTypes{computervision.VisualFeatureTypesImageType}

	imageAnalysis, err := client.AnalyzeImage(
			computerVisionContext,
			remoteImage,
			features,
			[]computervision.Details{},
			"")
	if err != nil { log.Fatal(err) }

	fmt.Println("Image type of remote image:")

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
	fmt.Println()
}
// </snippet_type>
/* 
 * END - Detect Image Type - remote
 */

/*  
 * Detect Objects - local
 */
func DetectObjectsLocalImage(client computervision.BaseClient, localImagePath string) {
	fmt.Println("-----------------------------------------")
	fmt.Println("DETECT OBJECTS - local")
	fmt.Println()
	var localImage io.ReadCloser
	localImage, err := os.Open(localImagePath)
	if err != nil { log.Fatal(err) }

	imageAnalysis, err := client.DetectObjectsInStream(
			computerVisionContext,
			localImage,
			)
	if err != nil { log.Fatal(err) }

	fmt.Println("Detecting objects in local image: ")
	if len(*imageAnalysis.Objects) == 0 {
		fmt.Println("No objects detected.")
	} else {
		// Print object names detected and bounding boxes with confidence levels.
		for _, object := range *imageAnalysis.Objects {
			fmt.Printf("'%v' with confidence %.2f%% at location (%v, %v), (%v, %v)\n",
				*object.Object, *object.Confidence * 100,
				*object.Rectangle.X, *object.Rectangle.X + *object.Rectangle.W,
				*object.Rectangle.Y, *object.Rectangle.Y + *object.Rectangle.H)
		}
	}
	fmt.Println()
}
/* 
 * END - Detect Objects - local
 */

/*  
 * Detect Objects - remote
 */
// <snippet_objects>
func DetectObjectsRemoteImage(client computervision.BaseClient, remoteImageURL string) {
	fmt.Println("-----------------------------------------")
	fmt.Println("DETECT OBJECTS - remote")
	fmt.Println()
	var remoteImage computervision.ImageURL
	remoteImage.URL = &remoteImageURL

	imageAnalysis, err := client.DetectObjects(
			computerVisionContext,
			remoteImage,
	)
	if err != nil { log.Fatal(err) }

	fmt.Println("Detecting objects in remote image: ")
	if len(*imageAnalysis.Objects) == 0 {
		fmt.Println("No objects detected.")
	} else {
		// Print the objects found with confidence level and bounding box locations.
		for _, object := range *imageAnalysis.Objects {
			fmt.Printf("'%v' with confidence %.2f%% at location (%v, %v), (%v, %v)\n",
				*object.Object, *object.Confidence * 100,
				*object.Rectangle.X, *object.Rectangle.X + *object.Rectangle.W,
				*object.Rectangle.Y, *object.Rectangle.Y + *object.Rectangle.H)
		}
	}
	fmt.Println()
}
// </snippet_objects>
/*
 * END - Detect Objects - remote
 */

/*  
 * Detect Brands - local
 */
func DetectBrandsLocalImage(client computervision.BaseClient, localImagePath string) {
	fmt.Println("-----------------------------------------")
	fmt.Println("DETECT BRANDS - local")
	fmt.Println()
	var localImage io.ReadCloser
	localImage, err := os.Open(localImagePath)
	if err != nil { log.Fatal(err) }

	// Define the features you want returned in the response.
	features := []computervision.VisualFeatureTypes{computervision.VisualFeatureTypesBrands}

	imageAnalysis, err := client.AnalyzeImageInStream(
			computerVisionContext,
			localImage,
			features,
			[]computervision.Details{},
			"en") // language
	if err != nil { log.Fatal(err) }

	fmt.Println("Detecting brands in local image: ")
	if len(*imageAnalysis.Brands) == 0 {
		fmt.Println("No brands detected.")
	} else {
		// Get the bounding box for each brand and confidence level that it's identified correctly.
		for _, brand := range *imageAnalysis.Brands {
			fmt.Printf("'%v' with confidence %.2f%% at location (%v, %v), (%v, %v)\n",
				*brand.Name, *brand.Confidence * 100,
			*brand.Rectangle.X, *brand.Rectangle.X + *brand.Rectangle.W,
			*brand.Rectangle.Y, *brand.Rectangle.Y + *brand.Rectangle.H)
		}
	}
	fmt.Println()
}
/*
 * END - Detect Brands - local
 */

/*  
 * Detect Brands - remote
 */
// <snippet_brands>
func DetectBrandsRemoteImage(client computervision.BaseClient, remoteImageURL string) {
	fmt.Println("-----------------------------------------")
	fmt.Println("DETECT BRANDS - remote")
	fmt.Println()
	var remoteImage computervision.ImageURL
	remoteImage.URL = &remoteImageURL

	// Define the kinds of features you want returned.
	features := []computervision.VisualFeatureTypes{computervision.VisualFeatureTypesBrands}

	imageAnalysis, err := client.AnalyzeImage(
		computerVisionContext,
		remoteImage,
		features,
		[]computervision.Details{},
		"en")
	if err != nil { log.Fatal(err) }

	fmt.Println("Detecting brands in remote image: ")
	if len(*imageAnalysis.Brands) == 0 {
		fmt.Println("No brands detected.")
	} else {
		// Get bounding box around the brand and confidence level it's correctly identified.
		for _, brand := range *imageAnalysis.Brands {
			fmt.Printf("'%v' with confidence %.2f%% at location (%v, %v), (%v, %v)\n",
				*brand.Name, *brand.Confidence * 100,
				*brand.Rectangle.X, *brand.Rectangle.X + *brand.Rectangle.W,
				*brand.Rectangle.Y, *brand.Rectangle.Y + *brand.Rectangle.H)
		}
	}
	fmt.Println()
}
// </snippet_brands>
/*
 * END - Detect brands in remote image
 */

/*
 * Generate Thumbnail - local
 */
func GenerateThumbnailLocalImage(client computervision.BaseClient, localImagePath string) {
	fmt.Println("-----------------------------------------")
	fmt.Println("GENERATE THUMBNAIL - local")
	fmt.Println()

	var localImage io.ReadCloser
	localImage, err := os.Open(localImagePath)
	if err != nil { log.Fatal(err) }

	// Call API, adjust the thumbnail width/height (pixels) if desired.
	// SmartCropping is set to true, which means the aspect ratio is allowed to change in order
	// to frame the thumbnail better.
	smartCropping := true
	thumbLocal, err := client.GenerateThumbnailInStream(computerVisionContext, 100, 100, localImage, &smartCropping)
	if err != nil { log.Fatal(err) }

	// Write the image binary to file
	file, err := os.Create("resources\\thumb_local.png")
	if err != nil { log.Fatal(err) }
	defer file.Close()
	_, err = io.Copy(file, thumbLocal.Body)
	if err != nil { log.Fatal(err) }

	fmt.Println("The thunbnail from local has been saved to file.")
	fmt.Println()
}
/*
 * END - Generate Thumbnail -local
 */

/*
 * Generate Thumbnail - remote
 */
func GenerateThumbnailRemoteImage(client computervision.BaseClient, remoteImageURL string) {
	fmt.Println("-----------------------------------------")
	fmt.Println("GENERATE THUMBNAIL - remote")
	fmt.Println()

	// Call API, adjust the thumbnail width/height (pixels) if desired.
	// SmartCropping is set to true, which means the aspect ratio is allowed to change in order
	// to frame the thumbnail better.
	smartCropping := true
	var remoteImage computervision.ImageURL
	remoteImage.URL = &remoteImageURL
	thumbLocal, err := client.GenerateThumbnail(computerVisionContext, 100, 100, remoteImage, &smartCropping)
	if err != nil { log.Fatal(err) }

	// Write the image binary to file
	file, err := os.Create("resources\\thumb_remote.png")
	if err != nil { log.Fatal(err) }
	defer file.Close()
	_, err = io.Copy(file, thumbLocal.Body)
	if err != nil { log.Fatal(err) }

	fmt.Println("The thunbnail from remote has been saved to file.")
	fmt.Println()
}
/*
 * END - Generate Thumbnail -local
 */

/*  
 * Batch Read File - local
 * A new way to recognize text, with more accurate results than the RecognizeText API call.
 */
func BatchReadFileLocalImage(client computervision.BaseClient, localImagePath string) {
	fmt.Println("-----------------------------------------")
	fmt.Println("BATCH READ FILE - local")
	fmt.Println()
	var localImage io.ReadCloser
	localImage, err := os.Open(localImagePath)
	if err != nil { log.Fatal(err) }

	//	When you use the Read Document interface, the response contains a field
	//	called "Operation-Location", which contains the URL to use for your
	//	GetReadOperationResult to access OCR results.
	textHeaders, err := client.BatchReadFileInStream(computerVisionContext, localImage)
	if err != nil { log.Fatal(err) }

	//	Use ExtractHeader from the autorest library to get the Operation-Location URL
	operationLocation := autorest.ExtractHeaderValue("Operation-Location", textHeaders.Response)

	numberOfCharsInOperationId := 36
	operationId := string(operationLocation[len(operationLocation)-numberOfCharsInOperationId : len(operationLocation)])

	readOperationResult, err := client.GetReadOperationResult(computerVisionContext, operationId)
	if err != nil { log.Fatal(err) }

	// Wait for the operation to complete.
	i := 0
	maxRetries := 10

	fmt.Println("Recognizing text in a local image with the batch Read API ...")
	for readOperationResult.Status != computervision.Failed &&
			readOperationResult.Status != computervision.Succeeded {
		if i >= maxRetries {
			break
		}
		i++

		fmt.Printf("Server status: %v, waiting %v seconds...\n", readOperationResult.Status, i)
		time.Sleep(1 * time.Second)

		readOperationResult, err = client.GetReadOperationResult(computerVisionContext, operationId)
		if err != nil { log.Fatal(err) }
	}

	// Display the results.
	fmt.Println()
	for _, recResult := range *(readOperationResult.RecognitionResults) {
		for _, line := range *recResult.Lines {
			fmt.Println(*line.Text)
		}
	}
	fmt.Println()
}
/*
 * END - Recognize text with the Read API in a local image
 */ 


/*  
 * Batch Read File - remote
 * A new way to recognize text, with more accurate results than the RecognizeText API call.
 */
// <snippet_read_call>
func BatchReadFileRemoteImage(client computervision.BaseClient, remoteImageURL string) {
	fmt.Println("-----------------------------------------")
	fmt.Println("BATCH READ FILE - remote")
	fmt.Println()
	var remoteImage computervision.ImageURL
	remoteImage.URL = &remoteImageURL

	// The response contains a field called "Operation-Location", 
	// which is a URL with an ID that you'll use for GetReadOperationResult to access OCR results.
	textHeaders, err := client.BatchReadFile(computerVisionContext, remoteImage)
	if err != nil { log.Fatal(err) }

	// Use ExtractHeader from the autorest library to get the Operation-Location URL
	operationLocation := autorest.ExtractHeaderValue("Operation-Location", textHeaders.Response)

	numberOfCharsInOperationId := 36
	operationId := string(operationLocation[len(operationLocation)-numberOfCharsInOperationId : len(operationLocation)])
	// </snippet_read_call>

	// <snippet_read_response>
	readOperationResult, err := client.GetReadOperationResult(computerVisionContext, operationId)
	if err != nil { log.Fatal(err) }

	// Wait for the operation to complete.
	i := 0
	maxRetries := 10

	fmt.Println("Recognizing text in a remote image with the batch Read API ...")
	for readOperationResult.Status != computervision.Failed &&
			readOperationResult.Status != computervision.Succeeded {
		if i >= maxRetries {
			break
		}
		i++

		fmt.Printf("Server status: %v, waiting %v seconds...\n", readOperationResult.Status, i)
		time.Sleep(1 * time.Second)

		readOperationResult, err = client.GetReadOperationResult(computerVisionContext, operationId)
		if err != nil { log.Fatal(err) }
	}
	// </snippet_read_response>

	// <snippet_read_display>
	// Display the results.
	fmt.Println()
	for _, recResult := range *(readOperationResult.RecognitionResults) {
		for _, line := range *recResult.Lines {
			fmt.Println(*line.Text)
		}
	}
	// </snippet_read_display>
	fmt.Println()
}

/*  
 * Recognize Printed Text with OCR - local
 */
func RecognizePrintedOCRLocalImage(client computervision.BaseClient, localImagePath string) {
	fmt.Println("-----------------------------------------")
	fmt.Println("RECOGNIZE PRINTED TEXT - local")
	fmt.Println()
	var localImage io.ReadCloser
	localImage, err := os.Open(localImagePath)
	if err != nil { log.Fatal(err) }

	fmt.Println("Recognizing text in a local image with OCR ...")
	ocrResult, err := client.RecognizePrintedTextInStream(computerVisionContext, true, localImage, computervision.En)
	if err != nil { log.Fatal(err) }

	// Get orientation of text.
	fmt.Printf("Text angle: %.4f\n", *ocrResult.TextAngle)

	// Get bounding boxes for each line of text and print text.
	for _, region := range *ocrResult.Regions {
		for _, line := range *region.Lines {
			fmt.Printf("\nBounding box: %v\n", *line.BoundingBox)
			s := ""
			for _, word := range *line.Words {
				s += *word.Text + " "
			}
			fmt.Printf("Text: %v", s)
		}
	}
	fmt.Println()
	fmt.Println()
}

/*  
 *  Recognize Printed Text with OCR - remote
 */
func RecognizePrintedOCRRemoteImage(client computervision.BaseClient, remoteImageURL string) {
	fmt.Println("-----------------------------------------")
	fmt.Println("RECOGNIZE PRINTED TEXT - remote")
	fmt.Println()
	var remoteImage computervision.ImageURL
	remoteImage.URL = &remoteImageURL

	fmt.Println("Recognizing text in a remote image with OCR ...")
	ocrResult, err := client.RecognizePrintedText(computerVisionContext, true, remoteImage, computervision.En)
	if err != nil { log.Fatal(err) }

	// Get orientation of text.
	fmt.Printf("Text angle: %.4f\n", *ocrResult.TextAngle)

	// Get bounding boxes for each line of text and print text.
	for _, region := range *ocrResult.Regions {
		for _, line := range *region.Lines {
			fmt.Printf("\nBounding box: %v\n", *line.BoundingBox)
			s := ""
			for _, word := range *line.Words {
				s += *word.Text + " "
			}
			fmt.Printf("Text: %v", s)
		}
	}
	fmt.Println()
	fmt.Println()
}
/*
 * END - Recognize Printed Text with OCR 
 */
