// <snippet_imports>
package main

import (
	"encoding/json"
	"container/list"
	"context"
	"fmt"
	"github.com/Azure/azure-sdk-for-go/services/cognitiveservices/v1.0/face"
	"github.com/Azure/go-autorest/autorest"
	"github.com/satori/go.uuid"
	"io"
	"io/ioutil"
	"log"
	"os"
	"path"
	"strconv"
	"strings"
	"time"
)
// </snippet_imports>

/*
Face Quickstart

- Detect Faces 
- Find Similar
- Verify
- Identify
- Large Face List
- Person Group Operations
- Large Person Group Operations
- Snapshot Operations

Prerequisites:
    - Go 1.12+
    - Install Face SDK in console: go get github.com/Azure/azure-sdk-for-go/services/cognitiveservices/v1.0/face
    - Create an images folder in your root folder and copy images from here:
	  https://github.com/Azure-Samples/cognitive-services-sample-data-files/tree/master/Face/images
	- Set your keys and endpoints in the variables below, in the Authenticate section.
How to run:
	- 'go run GoQuickstart.go' from the command line or run from an IDE

References: 
    - Documentation: https://docs.microsoft.com/en-us/azure/cognitive-services/face/
	- SDK: https://godoc.org/github.com/Azure/azure-sdk-for-go/services/cognitiveservices/v1.0/face
	- API: https://docs.microsoft.com/en-us/azure/cognitive-services/face/apireference
*/

// <snippet_main_client>
func main() {

	// A global context for use in all samples
	faceContext := context.Background()

	// Base url for the Verify and Large Face List examples
	const imageBaseURL = "https://csdx.blob.core.windows.net/resources/Face/Images/"

	/*
	Authenticate
	*/
	// Add FACE_SUBSCRIPTION_KEY, FACE_ENDPOINT, and AZURE_SUBSCRIPTION_ID to your environment variables.
	subscriptionKey := os.Getenv("FACE_SUBSCRIPTION_KEY")
	
	// This is also known as the 'source' endpoint for the Snapshot example
	endpoint := os.Getenv("FACE_ENDPOINT")

	// Client used for Detect Faces, Find Similar, and Verify examples.
	client := face.NewClient(endpoint)
	client.Authorizer = autorest.NewCognitiveServicesAuthorizer(subscriptionKey)
	/*
	END - Authenticate
	*/
	// </snippet_main_client>


	/*
	Authenticate target client
	*/
	// <snippet_target_client>
	// This key should be from another Face resource with a different region. 
	// Used for the Snapshot example only.
	targetSubscriptionKey := os.Getenv("FACE_SUBSCRIPTION_KEY2")

	// This should have a different region than your source endpoint. used only in Snapshot.
	targetEndpoint := os.Getenv("FACE_ENDPOINT2")

	// Get your subscription ID (different than the key) from any Face resource in Azure.
	azureSubscriptionID, uuidErr := uuid.FromString(os.Getenv("AZURE_SUBSCRIPTION_ID"))
	// </snippet_target_client>
	if uuidErr != nil { log.Fatal(uuidErr) }
	/*
	END - Authenticate target client
	*/

	/*
	DETECT FACES
	This example detects all faces in the images, then extracts some features from them.
	*/
	fmt.Println()
	fmt.Println("------------------------------")
	fmt.Println("DETECT FACES")
	// <snippet_detect>
	// Detect a face in an image that contains a single face
	singleFaceImageURL := "https://www.biography.com/.image/t_share/MTQ1MzAyNzYzOTgxNTE0NTEz/john-f-kennedy---mini-biography.jpg" 
	singleImageURL := face.ImageURL { URL: &singleFaceImageURL } 
	singleImageName := path.Base(singleFaceImageURL)
	// Use recognition model 2 for feature extraction. Recognition model 1 is used to simply recogize faces.
	recognitionModel02 := face.Recognition02
	// Array types chosen for the attributes of Face
	attributes := []face.AttributeType {"age", "emotion", "gender"}
	returnFaceID := true
	returnRecognitionModel := false
	returnFaceLandmarks := false

	// API call to detect faces in single-faced image, using recognition model 2
	detectSingleFaces, dErr := client.DetectWithURL(faceContext, singleImageURL, &returnFaceID, &returnFaceLandmarks, attributes, recognitionModel02, &returnRecognitionModel)
	if dErr != nil { log.Fatal(dErr) }

	// Dereference *[]DetectedFace, in order to loop through it.
	dFaces := *detectSingleFaces.Value
	// </snippet_detect>

	// Display the detected face ID in the first single-face image.
	// Face IDs are used for comparison to faces (their IDs) detected in other images.
	// <snippet_detect_display>
	fmt.Println("Detected face in (" + singleImageName + ") with ID(s): ")
	fmt.Println(dFaces[0].FaceID)
	fmt.Println()
	// Find/display the age and gender attributes
	for _, dFace := range dFaces { 
		fmt.Println("Face attributes:")
		fmt.Printf("  Age: %.0f", *dFace.FaceAttributes.Age) 
		fmt.Println("\n  Gender: " + dFace.FaceAttributes.Gender) 
	} 
	// Get/display the emotion attribute
	emotionStruct := *dFaces[0].FaceAttributes.Emotion
	// Convert struct to a map
	var emotionMap map[string]float64
	result, _ := json.Marshal(emotionStruct)
	json.Unmarshal(result, &emotionMap)
	// Find the emotion with the highest score (confidence level). Range is 0.0 - 1.0.
	var highest float64 
	emotion := ""
	dScore := -1.0
	for name, value := range emotionMap{
		if (value > highest) {
			emotion, dScore = name, value
			highest = value
		}
	}
	fmt.Println("  Emotion: " + emotion + " (score: " + strconv.FormatFloat(dScore, 'f', 3, 64) + ")")
	// </snippet_detect_display>

	// <snippet_similar_single_ref>
	// Select an ID in single-faced image for comparison to faces detected in group image. Used in Find Similar.
	firstImageFaceID := dFaces[0].FaceID
	// </snippet_similar_single_ref>

	// <snippet_similar_multiple_ref>
	// Detect the faces in an image that contains multiple faces
	groupImageURL := "http://www.historyplace.com/kennedy/president-family-portrait-closeup.jpg"
	groupImageName := path.Base(groupImageURL)
	groupImage := face.ImageURL { URL: &groupImageURL } 

	// API call to detect faces in group image, using recognition model 2. This returns a ListDetectedFace struct.
	detectedGroupFaces, dgErr := client.DetectWithURL(faceContext, groupImage, &returnFaceID, &returnFaceLandmarks, nil, recognitionModel02, &returnRecognitionModel)
	if dgErr != nil { log.Fatal(dgErr) }
	fmt.Println()

	// Detect faces in the group image.
	// Dereference *[]DetectedFace, in order to loop through it.
	dFaces2 := *detectedGroupFaces.Value
	// Make slice list of UUIDs
	faceIDs := make([]uuid.UUID, len(dFaces2))
	fmt.Print("Detected faces in (" + groupImageName + ") with ID(s):\n")
	for i, face := range dFaces2 {
		faceIDs[i] = *face.FaceID // Dereference DetectedFace.FaceID
		fmt.Println(*face.FaceID)
	}
	// </snippet_similar_multiple_ref>
	/*
	END - Detect faces
	*/
	
	/*
	FIND SIMILAR
	Finds a list of detected faces in group image, using the single-faced image as a query.
	*/
	fmt.Println()
	fmt.Println("------------------------------")
	fmt.Println("FIND SIMILAR")

	// <snippet_similar>
	// Add single-faced image ID to struct
	findSimilarBody := face.FindSimilarRequest { FaceID: firstImageFaceID, FaceIds: &faceIDs }
	// Get the list of similar faces found in the group image of previously detected faces
	listSimilarFaces, sErr := client.FindSimilar(faceContext, findSimilarBody)
	if sErr != nil { log.Fatal(sErr) }

	// The *[]SimilarFace 
	simFaces := *listSimilarFaces.Value
	// </snippet_similar>

	// <snippet_similar_print>
	// Print the details of the similar faces detected 
	fmt.Print("Similar faces found in (" + groupImageName + ") with ID(s):\n")
	var sScore float64
	for _, face := range simFaces {
		fmt.Println(face.FaceID)
		// Confidence of the found face with range 0.0 to 1.0.
		sScore = *face.Confidence
		fmt.Println("The similarity confidence: ", strconv.FormatFloat(sScore, 'f', 3, 64))
	}
	// </snippet_similar_print>
	// NOTE: The similar face IDs of the single face image and the similar one found in the group image do not need to match, 
	// they are only used for identification purposes in each image. 
	// The similarity of the faces are matched using the Cognitive Services algorithm in FindSimilar().
	/*
	END - Find Similar
	*/

	/*
	VERIFY
	Compares one face with another to check if they are from the same person.
	*/
	fmt.Println()
	fmt.Println("-----------------------------")
	fmt.Println("VERIFY")
	// <snippet_ver_images>
	// Create a slice list to hold the target photos of the same person
	targetImageFileNames :=  make([]string, 2)
	targetImageFileNames[0] = "Family1-Dad1.jpg"
	targetImageFileNames[1] = "Family1-Dad2.jpg"
	
	// The source photos contain this person, maybe
	sourceImageFileName1 := "Family1-Dad3.jpg"
	sourceImageFileName2 := "Family1-Son1.jpg"
	// </snippet_ver_images>

	// <snippet_ver_detect_source>
	// DetectWithURL parameters
	urlSource1 := imageBaseURL + sourceImageFileName1
	urlSource2 := imageBaseURL + sourceImageFileName2
	url1 :=  face.ImageURL { URL: &urlSource1 }
	url2 := face.ImageURL { URL: &urlSource2 }
	returnFaceIDVerify := true
	returnFaceLandmarksVerify := false
	returnRecognitionModelVerify := false
	// Recognition model 1 is used to rcognise a face, not extract features from it.
	recognitionModel01 := face.Recognition01

	// Detect face(s) from source image 1, returns a ListDetectedFace struct
	detectedVerifyFaces1, dErrV1 := client.DetectWithURL(faceContext, url1 , &returnFaceIDVerify, &returnFaceLandmarksVerify, nil, recognitionModel01, &returnRecognitionModelVerify)
	if dErrV1 != nil { log.Fatal(dErrV1) }
	// Dereference the result, before getting the ID
	dVFaceIds1 := *detectedVerifyFaces1.Value 
	// Get ID of the detected face
	imageSource1Id := dVFaceIds1[0].FaceID
	fmt.Println(fmt.Sprintf("%v face(s) detected from image: %v", len(dVFaceIds1), sourceImageFileName1))

	// Detect face(s) from source image 2, returns a ListDetectedFace struct
	detectedVerifyFaces2, dErrV2 := client.DetectWithURL(faceContext, url2 , &returnFaceIDVerify, &returnFaceLandmarksVerify, nil, recognitionModel01, &returnRecognitionModelVerify)
	if dErrV2 != nil { log.Fatal(dErrV2) }
	// Dereference the result, before getting the ID
	dVFaceIds2 := *detectedVerifyFaces2.Value 
	// Get ID of the detected face
	imageSource2Id := dVFaceIds2[0].FaceID
	fmt.Println(fmt.Sprintf("%v face(s) detected from image: %v", len(dVFaceIds2), sourceImageFileName2))
	// </snippet_ver_detect_source>

	// <snippet_ver_detect_target>
	// Detect faces from each target image url in list. DetectWithURL returns a VerifyResult with Value of list[DetectedFaces]
	// Empty slice list for the target face IDs (UUIDs)
	var detectedVerifyFacesIds [2]uuid.UUID
	for i, imageFileName := range targetImageFileNames {
		urlSource := imageBaseURL + imageFileName 
		url :=  face.ImageURL { URL: &urlSource}
		detectedVerifyFaces, dErrV := client.DetectWithURL(faceContext, url, &returnFaceIDVerify, &returnFaceLandmarksVerify, nil, recognitionModel01, &returnRecognitionModelVerify)
		if dErrV != nil { log.Fatal(dErrV) }
		// Dereference *[]DetectedFace from Value in order to loop through it.
		dVFaces := *detectedVerifyFaces.Value
		// Add the returned face's face ID
		detectedVerifyFacesIds[i] = *dVFaces[0].FaceID
		fmt.Println(fmt.Sprintf("%v face(s) detected from image: %v", len(dVFaces), imageFileName))
	}
	// </snippet_ver_detect_target>

	// <snippet_ver>
	// Verification example for faces of the same person. The higher the confidence, the more identical the faces in the images are.
    // Since target faces are the same person, in this example, we can use the 1st ID in the detectedVerifyFacesIds list to compare.
	verifyRequestBody1 := face.VerifyFaceToFaceRequest{ FaceID1: imageSource1Id, FaceID2: &detectedVerifyFacesIds[0] }
	verifyResultSame, vErrSame := client.VerifyFaceToFace(faceContext, verifyRequestBody1)
	if vErrSame != nil { log.Fatal(vErrSame) }

	fmt.Println()

	// Check if the faces are from the same person.
	if (*verifyResultSame.IsIdentical) {
		fmt.Println(fmt.Sprintf("Faces from %v & %v are of the same person, with confidence %v", 
		sourceImageFileName1, targetImageFileNames[0], strconv.FormatFloat(*verifyResultSame.Confidence, 'f', 3, 64)))
	} else {
		// Low confidence means they are more differant than same.
		fmt.Println(fmt.Sprintf("Faces from %v & %v are of a different person, with confidence %v", 
		sourceImageFileName1, targetImageFileNames[0], strconv.FormatFloat(*verifyResultSame.Confidence, 'f', 3, 64)))
	}

	// Verification example for faces of different persons. 
	// Since target faces are same person, in this example, we can use the 1st ID in the detectedVerifyFacesIds list to compare.
	verifyRequestBody2 := face.VerifyFaceToFaceRequest{ FaceID1: imageSource2Id, FaceID2: &detectedVerifyFacesIds[0] }
	verifyResultDiff, vErrDiff := client.VerifyFaceToFace(faceContext, verifyRequestBody2)
	if vErrDiff != nil { log.Fatal(vErrDiff) }
	// Check if the faces are from the same person.
	if (*verifyResultDiff.IsIdentical) {
		fmt.Println(fmt.Sprintf("Faces from %v & %v are of the same person, with confidence %v", 
		sourceImageFileName2, targetImageFileNames[0], strconv.FormatFloat(*verifyResultDiff.Confidence, 'f', 3, 64)))
	} else {
		// Low confidence means they are more differant than same.
		fmt.Println(fmt.Sprintf("Faces from %v & %v are of a different person, with confidence %v", 
		sourceImageFileName2, targetImageFileNames[0], strconv.FormatFloat(*verifyResultDiff.Confidence, 'f', 3, 64)))
	}
	// </snippet_ver>
	/*
	END - Verify
	*/

	/*
	LARGE FACE LIST
	This example creates a large face list from single-faced images
	*/
		fmt.Println()
		fmt.Println("-----------------------------")
		fmt.Println("LARGE FACE LIST")

		// Create a slice for the images you want to add
		imagesForList :=  make([]string, 9)
		imagesForList[0] = "Family1-Dad1.jpg"
		imagesForList[1] = "Family1-Dad2.jpg"
		imagesForList[2] = "Family1-Daughter1.jpg"
		imagesForList[3] = "Family1-Mom1.jpg"
		imagesForList[4] = "Family1-Son1.jpg"
		imagesForList[5] = "Family2-Lady1.jpg"
		imagesForList[6] = "Family2-Man1.jpg"
		imagesForList[7] = "Family3-Lady1.jpg"
		imagesForList[8] = "Family3-Man1.jpg"

		// Create a special face list client
		faceListClient := face.NewLargeFaceListClient(endpoint)
		faceListClient.Authorizer = autorest.NewCognitiveServicesAuthorizer(subscriptionKey)

		// Create an ID for our list
		faceListID := "my-face-list"

		fmt.Printf("Creating large face list: %v...", faceListID)
		fmt.Println()

		// Create the metadata for the body of the requset
		listMetadata := face.MetaDataContract { RecognitionModel: recognitionModel01, Name: &faceListID }
		// Create the large face list, empty for now
		faceListClient.Create(faceContext, faceListID, listMetadata)

		// Prepare to add each face
		// Define a targetFace=left,top,width,height
		//targetFaceDimensions := []int32 {10, 10, 100, 100}
		listUserData := ""

		// First face added to list
		var firstFace uuid.UUID
		// Add each face in image array
		for i, listFace := range imagesForList {
			// Add base URL with the specific image to a struct
			listImage := imageBaseURL + listFace
			listImageURL:= face.ImageURL { URL: &listImage }
			// Add the slice of faces to our face list
			oneFace, pFaceErr := faceListClient.AddFaceFromURL(faceContext, faceListID, listImageURL, listUserData, nil)
			if pFaceErr != nil { log.Fatal(pFaceErr) }
				if (i == 0) {
					firstFace = *oneFace.PersistedFaceID
				}
		}

		// Get persisted faces from the face list.
		fmt.Println("Persisted faces in large face list:")
		fmt.Println()

		stringFirstFace := firstFace.String()
		numberInList := int32(len(imagesForList))

		// Returns a ListPersistedFace struct
		persistedFaces, pListErr := faceListClient.ListFaces(faceContext, faceListID, stringFirstFace, &numberInList)
		if pListErr != nil { log.Fatal(pListErr) }

		// The persistedFaces.Value returns a *[]PersistedFace
		for _, persistedFace := range *persistedFaces.Value {
			fmt.Println(persistedFace.PersistedFaceID)
		 }

		// Delete the large face list, so we can retest (recreate) the list again and again.
		// If this example fails before deleting the list (and after it was created), delete the list from
		// the API console, so you can test it again:
		// https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/5a1580d5d2de3616c086f2cd
		faceListClient.Delete(faceContext, faceListID)
		fmt.Println()
		fmt.Println("Deleted the large face list:" + faceListID)
	/*
	END - Large Face List
	*/

	/*
	PERSON GROUP OPERATIONS
	This example creates a Person Group from local, single-faced images, then trains it. 
	It can then be used to detect and identify faces in a group image.
	A Person Group is made up of several Person Group Persons
	The Person Groups Persons are Person objects, each of which contain Persisted Face objects
	for each similar image of that person.
	*/
	fmt.Println()
	fmt.Println("------------------------------")
	fmt.Println("PERSON GROUP OPERATIONS")

	// <snippet_pg_setup>
	// Get working directory
	root, rootErr := os.Getwd()
	if rootErr != nil { log.Fatal(rootErr) }

	// Full path to images folder
	imagePathRoot := path.Join(root+"\\images\\")

	// Authenticate - Need a special person group client for your person group
	personGroupClient := face.NewPersonGroupClient(endpoint)
	personGroupClient.Authorizer = autorest.NewCognitiveServicesAuthorizer(subscriptionKey)

	// Create the Person Group
	// Create an empty Person Group. Person Group ID must be lower case, alphanumeric, and/or with '-', '_'.
	personGroupID := "unique-person-group"
	fmt.Println("Person group ID: " + personGroupID)
	metadata := face.MetaDataContract { Name: &personGroupID }

	// Create the person group
	personGroupClient.Create(faceContext, personGroupID, metadata)
	// </snippet_pg_setup>
	
	// <snippet_pgp_setup>
	// Authenticate - Need a special person group person client for your person group person
	personGroupPersonClient := face.NewPersonGroupPersonClient(endpoint)
	personGroupPersonClient.Authorizer = autorest.NewCognitiveServicesAuthorizer(subscriptionKey)

	// Create each person group person for each group of images (woman, man, child)
	// Define woman friend
	w := "Woman"
	nameWoman := face.NameAndUserDataContract { Name: &w }
	// Returns a Person type
	womanPerson, wErr := personGroupPersonClient.Create(faceContext, personGroupID, nameWoman)
	if wErr != nil { log.Fatal(wErr) }
	fmt.Print("Woman person ID: ")
	fmt.Println(womanPerson.PersonID)
	// Define man friend
	m := "Man"
	nameMan := face.NameAndUserDataContract { Name: &m }
	// Returns a Person type
	manPerson, wErr := personGroupPersonClient.Create(faceContext, personGroupID, nameMan)
	if wErr != nil { log.Fatal(wErr) }
	fmt.Print("Man person ID: ")
	fmt.Println(manPerson.PersonID)
	// Define child friend
	ch := "Child"
	nameChild := face.NameAndUserDataContract { Name: &ch }
	// Returns a Person type
	childPerson, wErr := personGroupPersonClient.Create(faceContext, personGroupID, nameChild)
	if wErr != nil { log.Fatal(wErr) }
	fmt.Print("Child person ID: ")
	fmt.Println(childPerson.PersonID)
	// </snippet_pgp_setup>

	// <snippet_pgp_assign>
	// Detect faces and register to correct person
	// Lists to hold all their person images
	womanImages := list.New()
	manImages := list.New()
	childImages := list.New()
	
	// Collect the local images for each person, add them to their own person group person
	images, fErr := ioutil.ReadDir(imagePathRoot)
	if fErr != nil { log.Fatal(fErr)}
    for _, f := range images {
		path:= (imagePathRoot+f.Name())
        if strings.HasPrefix(f.Name(), "w") {
			var wfile io.ReadCloser
			wfile, err:= os.Open(path)
			if err != nil { log.Fatal(err) }
			womanImages.PushBack(wfile)
			personGroupPersonClient.AddFaceFromStream(faceContext, personGroupID, *womanPerson.PersonID, wfile, "", nil)
		}
		if strings.HasPrefix(f.Name(), "m") {
			var mfile io.ReadCloser
			mfile, err:= os.Open(path)
			if err != nil { log.Fatal(err) }
			manImages.PushBack(mfile)
			personGroupPersonClient.AddFaceFromStream(faceContext, personGroupID, *manPerson.PersonID, mfile, "", nil)
		}
		if strings.HasPrefix(f.Name(), "ch") {
			var chfile io.ReadCloser
			chfile, err:= os.Open(path)
			if err != nil { log.Fatal(err) }
			childImages.PushBack(chfile)
			personGroupPersonClient.AddFaceFromStream(faceContext, personGroupID, *childPerson.PersonID, chfile, "", nil)
		}
	}
	// </snippet_pgp_assign>
	
	// <snippet_pg_train>
	// Train the person group
	personGroupClient.Train(faceContext, personGroupID)

	// Wait for it to succeed in training
	for {
		trainingStatus, tErr := personGroupClient.GetTrainingStatus(faceContext, personGroupID)
		if tErr != nil { log.Fatal(tErr) }
		
		if trainingStatus.Status == "succeeded" {
			fmt.Println("Training status:", trainingStatus.Status)
			break
		}
		time.Sleep(2)
	}
	// </snippet_pg_train>
	/*
	END - Person Group operations
	*/

	/*
	LARGE PERSON GROUP OPERATIONS
	The same in structure as the regular-sized person group but with different API calls,
	able to handle scale. To distingish from the person group example, "L" (large) is appended to
	similarly named variables. Two clients are created in this example.
	Once a large person group is created and trained, it can be used to detect or 
	identify faces in other images.
	*/
	fmt.Println()
	fmt.Println("------------------------------")
	fmt.Println("LARGE PERSON GROUP OPERATIONS")

	// Get working directory
	rootL, rootErrL := os.Getwd()
	if rootErrL != nil { log.Fatal(rootErrL) }

	// Full path to images folder
	imagePathRootL := path.Join(rootL+"\\images\\")

	// Authenticate - Need a special person group client for your person group
	personGroupClientL := face.NewLargePersonGroupClient(endpoint)
	personGroupClientL.Authorizer = autorest.NewCognitiveServicesAuthorizer(subscriptionKey)

	// Create the large Person Group
	// Create an empty large Person Group. 
	// Large Person Group ID must be lower case, alphanumeric, and/or with '-', '_'.
	largePersonGroupID := "unique-large-person-group"
	fmt.Println("Large person group ID: " + largePersonGroupID)

	// Prepare metadata for large person group creation
	metadataL := face.MetaDataContract { Name: &largePersonGroupID }
	// Create the large person group
	personGroupClientL.Create(faceContext, largePersonGroupID, metadataL)
	
	// Authenticate - Need a special person group person client for your person group person
	personGroupPersonClientL := face.NewLargePersonGroupPersonClient(endpoint)
	personGroupPersonClientL.Authorizer = autorest.NewCognitiveServicesAuthorizer(subscriptionKey)

	// Create each person group person for each group of images (woman, man, child)
	// Define woman friend
	wL := "Woman"
	nameWomanL := face.NameAndUserDataContract { Name: &wL }
	// Returns a Person type
	womanPersonL, wErrL := personGroupPersonClientL.Create(faceContext, largePersonGroupID, nameWomanL)
	if wErrL != nil { log.Fatal(wErrL) }
	fmt.Print("Woman person ID: ")
	fmt.Println(womanPersonL.PersonID)
	// Define man friend
	mL := "Man"
	nameManL := face.NameAndUserDataContract { Name: &mL }
	// Returns a Person type
	manPersonL, mErrL := personGroupPersonClientL.Create(faceContext, largePersonGroupID, nameManL)
	if mErrL != nil { log.Fatal(mErrL) }
	fmt.Print("Man person ID: ")
	fmt.Println(manPersonL.PersonID)
	// Define child friend
	chL := "Child"
	nameChildL := face.NameAndUserDataContract { Name: &chL }
	// Returns a Person type
	childPersonL, chErrL := personGroupPersonClientL.Create(faceContext, largePersonGroupID, nameChildL)
	if chErrL != nil { log.Fatal(chErrL) }
	fmt.Print("Child person ID: ")
	fmt.Println(childPersonL.PersonID)

	// Detect faces and register to correct person
	// Lists to hold all their person images
	womanImagesL := list.New()
	manImagesL := list.New()
	childImagesL := list.New()
	
	// Collect the local images for each person, add them to their own person group person
	imagesL, fErrL := ioutil.ReadDir(imagePathRootL)
	if fErrL != nil { log.Fatal(fErrL)}
    for _, f := range imagesL {
		path:= (imagePathRootL+f.Name())
        if strings.HasPrefix(f.Name(), "w") {
			var wfileL io.ReadCloser
			wfileL, errL:= os.Open(path)
			if errL != nil { log.Fatal(errL) }
			womanImagesL.PushBack(wfileL)
			personGroupPersonClientL.AddFaceFromStream(faceContext, largePersonGroupID, *womanPersonL.PersonID, wfileL, "", nil)
		}
		if strings.HasPrefix(f.Name(), "m") {
			var mfileL io.ReadCloser
			mfileL, errL:= os.Open(path)
			if errL != nil { log.Fatal(errL) }
			manImagesL.PushBack(mfileL)
			personGroupPersonClientL.AddFaceFromStream(faceContext, largePersonGroupID, *manPersonL.PersonID, mfileL, "", nil)
		}
		if strings.HasPrefix(f.Name(), "ch") {
			var chfileL io.ReadCloser
			chfileL, errL:= os.Open(path)
			if errL != nil { log.Fatal(errL) }
			childImagesL.PushBack(chfileL)
			personGroupPersonClientL.AddFaceFromStream(faceContext, largePersonGroupID, *childPersonL.PersonID, chfileL, "", nil)
		}
	}
	
	// Train the person group
	personGroupClientL.Train(faceContext, largePersonGroupID)

	// Wait for it to succeed in training
	for {
		trainingStatusL, tErrL := personGroupClientL.GetTrainingStatus(faceContext, largePersonGroupID)
		if tErrL != nil { log.Fatal(tErrL) }
		
		if trainingStatusL.Status == "succeeded" {
			fmt.Println("Training status:", trainingStatusL.Status)
			break
		}
		time.Sleep(2)
	}

	// Since testing, delete the large person group so you can run multiple times.
	// A large person group of the same exact name is not allowed to be created.
	personGroupClientL.Delete(faceContext, largePersonGroupID)
	fmt.Println()
	fmt.Println("Deleted large person group : " + largePersonGroupID)
	/*
	END - LARGE PERSON GROUP OPERATIONS
	*/

	/*
	IDENTIFY
	Uses an existing person group to identify a face in an image
	*/
	fmt.Println()
	fmt.Println("------------------------------")
	fmt.Println("IDENTIFY FACES...")
	// <snippet_id_source_get>
	personGroupTestImageName := "test-image-person-group.jpg"
	// Use image path root from the one created in person group
	personGroupTestImagePath := imagePathRoot
	var personGroupTestImage io.ReadCloser
	// Returns a ReaderCloser
	personGroupTestImage, identErr:= os.Open(personGroupTestImagePath+personGroupTestImageName)
	if identErr != nil { log.Fatal(identErr) }
	// </snippet_id_source_get>
	
	// <snippet_id_source_detect>
	// Detect faces in group test image, using recognition model 1 (default)
	returnIdentifyFaceID := true
	// Returns a ListDetectedFaces
	detectedTestImageFaces, dErr := client.DetectWithStream(faceContext, personGroupTestImage, &returnIdentifyFaceID, nil, nil, face.Recognition01, nil)
	if dErr != nil { log.Fatal(dErr) }

	// Make list of face IDs from the detection. 
	length := len(*detectedTestImageFaces.Value)
	testImageFaceIDs := make([]uuid.UUID, length)
	// ListDetectedFace is a struct with a Value property that returns a *[]DetectedFace
	for i, f := range *detectedTestImageFaces.Value {
		testImageFaceIDs[i] = *f.FaceID
	}
	// </snippet_id_source_detect>

	// <snippet_id>
	// Identify the faces in the test image with everyone in the person group as a query
	identifyRequestBody := face.IdentifyRequest { FaceIds: &testImageFaceIDs, PersonGroupID: &personGroupID }
	identifiedFaces, err := client.Identify(faceContext, identifyRequestBody)
	if err != nil { log.Fatal(err) }
	// </snippet_id>

	// <snippet_id_print>
	// Get the result which person(s) were identified
	iFaces := *identifiedFaces.Value
	for _, person := range iFaces {
		fmt.Println("Person for face ID: " )
		fmt.Print(person.FaceID)
		fmt.Println(" is identified in " + personGroupTestImageName + ".")
	}
	// </snippet_id_print>
	/*
	END - Identify
	*/

	/*
	SNAPSHOT OPERATIONS
	This example moves a person group from one region to another. The person group created in Person Group Operations will be used.
	You must have 2 Face resources created in Azure with 2 different regions, for example 'westus' and 'eastus'. Any region will work.
	Snapshot requires its own special authenticated client.
	*/
	fmt.Println()
	fmt.Println("------------------------------")
	fmt.Println("SNAPSHOT OPERATIONS")

	// <snippet_snap_target_id>
	// Add your Azure subscription ID(s) to a UUID array.
	numberOfSubKeys := 1 
	targetUUIDArray := make([]uuid.UUID, numberOfSubKeys)
	for i := range targetUUIDArray {
		targetUUIDArray[i] = azureSubscriptionID
	}
	// </snippet_snap_target_id>

	// <snippet_snap_target_auth>
	// Create a client from your source region, where your person group exists. Use for taking the snapshot.
	snapshotSourceClient := face.NewSnapshotClient(endpoint)
	snapshotSourceClient.Authorizer = autorest.NewCognitiveServicesAuthorizer(subscriptionKey)
	// Create a client for your target region. Use for applying the snapshot.
	snapshotTargetClient := face.NewSnapshotClient(targetEndpoint)
	snapshotTargetClient.Authorizer = autorest.NewCognitiveServicesAuthorizer(targetSubscriptionKey)
	// </snippet_snap_target_auth>



	// <snippet_snap_take>
	// Take snapshot
	takeBody := face.TakeSnapshotRequest { Type: face.SnapshotObjectTypePersonGroup, ObjectID: &personGroupID, ApplyScope: &targetUUIDArray }
	takeSnapshotResult, takeErr := snapshotSourceClient.Take(faceContext, takeBody)
	if takeErr != nil { log.Fatal(takeErr) }
	// Get the operations ID
	strTakeOperation := strings.ReplaceAll(takeSnapshotResult.Header.Get("Operation-Location"), "/operations/", "")
	fmt.Println("Taking snapshot (operations ID: " + strTakeOperation + ")... started")
	// Convert string operation ID to UUID
	takeOperationID, uuidErr := uuid.FromString(strTakeOperation)
	if uuidErr != nil { log.Fatal(uuidErr) }
	// </snippet_snap_take>

	// <snippet_snap_query>
	// Wait for the snapshot taking to finish
	var strSnapshotID string
	for {
		takeSnapshotStatus, tErr := snapshotSourceClient.GetOperationStatus(faceContext, takeOperationID)
		if tErr != nil { log.Fatal(tErr) }
		
		if takeSnapshotStatus.Status == "succeeded" {
			fmt.Println("Taking snapshot operation status: ", takeSnapshotStatus.Status)
			strSnapshotID = strings.ReplaceAll(*takeSnapshotStatus.ResourceLocation, "/snapshots/", "")
			break
		}
		time.Sleep(2)
	}

	// Convert string snapshot to UUID
	snapshotID, uuidErr := uuid.FromString(strSnapshotID)
	if uuidErr != nil { log.Fatal(uuidErr) }
	// </snippet_snap_query>
	
	// <snippet_snap_apply>
	// Creates a new snapshot instance in your target region. 
	// Make sure not to create a new snapshot in your target region with the same name as another one.
	applyBody := face.ApplySnapshotRequest { ObjectID: &personGroupID }
	applySnapshotResult, applyErr := snapshotTargetClient.Apply(faceContext, snapshotID, applyBody)
	if applyErr != nil { log.Fatal(applyErr) }
	
	// Get operation ID from response to track the progress of applying a snapshot.
	strApplyOperation := strings.ReplaceAll(applySnapshotResult.Header.Get("Operation-Location"), "/operations/", "")
	fmt.Println("Applying snapshot (operations ID: " + strApplyOperation + ")... started")
	// Convert operation ID to GUID
	applyOperationID, guidErr := uuid.FromString(strApplyOperation)
	if guidErr != nil { log.Fatal(guidErr) }
	// </snippet_snap_apply>
	
	// <snippet_snap_apply_query>
	// Wait for the snapshot applying to finish
	for {
		applySnapshotStatus, aErr := snapshotTargetClient.GetOperationStatus(faceContext, applyOperationID)
		if aErr != nil { log.Fatal(aErr) }
		
		if applySnapshotStatus.Status == "succeeded" {
			fmt.Println("Taking snapshot operation status: ", applySnapshotStatus.Status)
			break
		}
		time.Sleep(2)
	}
	// </snippet_snap_apply_query>

	fmt.Println("Applying snapshot... Done")
	/*
	END - Snapshot
	*/

	/*
	DELETE PERSON GROUP
	Delete once example is complete. This deletes a person group, since we are only testing. 
	If not deleted, rerunning this sample will recreate a Person Group with the same name, which will cause an error.
	*/	
	fmt.Println()
	fmt.Println("------------------------------")
	fmt.Println("DELETED PERSON GROUP (source region): " + personGroupID)
	personGroupClient.Delete(faceContext, personGroupID)

	// Delete Person Group in new region, too. First create a Person Group client for target region.
	personGroupTargetClient := face.NewPersonGroupClient(targetEndpoint)
	personGroupTargetClient.Authorizer = autorest.NewCognitiveServicesAuthorizer(targetSubscriptionKey)

	fmt.Println("DELETED PERSON GROUP (target region): " + personGroupID)
	personGroupTargetClient.Delete(faceContext, personGroupID)
	/*
	END - Delete Person Group
	*/

	fmt.Println()
	fmt.Println("End sample.")
}	
