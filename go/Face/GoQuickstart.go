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

/*
Face Quickstart

Using URL images, these samples detect faces, find similar faces, and identify faces.
The Person Group Operations sample uses local image files to create a person group with serveral person group persons and uses the group ID to identify faces in another local image. 
Finally, we take a snapshot of the person group and copy it to a target region.
All person groups (source and target region) created are deleted at the end of the sample.

Prerequisites:
    - Go 1.12+
    - Install Face SDK in console: go get github.com/Azure/azure-sdk-for-go/services/cognitiveservices/v1.0/face
    - Create an images folder in your root folder and copy images from here:
	  https://github.com/Azure-Samples/cognitive-services-sample-data-files/tree/master/Face/images
	- Set your keys and endpoints in the variables below, in the Authenticate section.

References: 
    - Documentation: https://docs.microsoft.com/en-us/azure/cognitive-services/face/
    - SDK: https://godoc.org/github.com/Azure/azure-sdk-for-go/services/cognitiveservices/v1.0/face
    - API: https://docs.microsoft.com/en-us/azure/cognitive-services/face/apireference
*/

func main() {

	// A global context for use in all samples
	faceContext := context.Background()

/*
	Authenticate
	*/
	// Add the "FACE_SUBSCRIPTION_KEY"s, "FACE_REGION"s, and the "AZURE_SUBSCRIPTION_ID" to your environment variables.
	// Use the same region as you used for your Face subscription key resource in Azure, for example 'westus'.
	sourceSubscriptionKey := os.Getenv("FACE_SUBSCRIPTION_KEY")
	targetSubscriptionKey := os.Getenv("FACE_SUBSCRIPTION_KEY2")
	sourceRegion := os.Getenv("FACE_REGION")
	// Use this region for targetEndpoint.
	targetRegion := os.Getenv("FACE_REGION2")
	// Replace this with the endpoint for your subscription key.
	sourceEndpoint := "https://"+sourceRegion+".api.cognitive.microsoft.com"
	// Use this endpoint for the snapshot sample only.
	targetEndpoint := "https://"+targetRegion+".api.cognitive.microsoft.com"
	// Get your subscription ID from any Face resource in Azure on the Overview page.
	azureSubscriptionID, uuidErr := uuid.FromString(os.Getenv("AZURE_SUBSCRIPTION_ID"))
	if uuidErr != nil { log.Fatal(uuidErr) }

	// Client used for Detect Faces and Find Similar samples.
	client := face.NewClient(sourceEndpoint)
	client.Authorizer = autorest.NewCognitiveServicesAuthorizer(sourceSubscriptionKey)

	/*
	Detect faces in two images
	This sample detects all faces in the images, then extracts some features from them.
	*/
	fmt.Println()
	fmt.Println("------------------------------")
	fmt.Println("DETECTING FACES ... ")
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

	// Display the detected face ID in the first single-face image.
	// Face IDs are used for comparison to faces (their IDs) detected in other images.
	fmt.Println("Detected face in (" + singleImageName + ") with ID: ")
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
	
	// Select an ID in single-faced image for comparison to faces detected in group image. Used in Find Similar.
	firstImageFaceID := dFaces[0].FaceID

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
	// Make list of UUIDs
	faceIDs := make([]uuid.UUID, len(dFaces2))
	fmt.Print("Detected faces in (" + groupImageName + ") with IDs:\n")
	for i, face := range dFaces2 {
		faceIDs[i] = *face.FaceID // Dereference DetectedFace.FaceID
		fmt.Println(*face.FaceID)
	}
	/*
	END - Detect faces
	*/
	
	/*
	Find a similar face
	Finds a list of detected faces in group image, using the single-faced image as a query.
	*/
	fmt.Println()
	fmt.Println("------------------------------")
	fmt.Println("FINDING SIMILAR FACE ... ")

	// Add single-faced image ID to struct
	findSimilarBody := face.FindSimilarRequest { FaceID: firstImageFaceID, FaceIds: &faceIDs }
	// Get the list of similar faces found in the group image of previously detected faces
	listSimilarFaces, sErr := client.FindSimilar(faceContext, findSimilarBody)
	if sErr != nil { log.Fatal(sErr) }

	// The *[]SimilarFace 
	simFaces := *listSimilarFaces.Value

	// Print the details of the similar faces detected 
	fmt.Print("Similar faces found in (" + groupImageName + "):\n")
	var sScore float64
	for _, face := range simFaces {
		fmt.Println(face.FaceID)
		// Confidence of the found face with range 0.0 to 1.0.
		sScore = *face.Confidence
		fmt.Println("The similarity confidence: ", strconv.FormatFloat(sScore, 'f', 3, 64))
	}

	// NOTE: The similar face IDs of the single face image and the similar one found in the group image do not need to match, 
	// they are only used for identification purposes in each image. 
	// The similarity of the faces are matched using the Cognitive Services algorithm in FindSimilar().
	/*
	END - Find similar
	*/

	/*
	Person Group operations
	This sample creates a Person Group from local, single-faced images, then trains it. 
	It can then be used to detect and identify faces in a group image.
	*/
	fmt.Println()
	fmt.Println("------------------------------")
	fmt.Println("PERSON GROUP OPERATIONS...")

	// Get working directory
	root, rootErr := os.Getwd()
	if rootErr != nil { log.Fatal(rootErr) }

	// Full path to images folder
	imagePathRoot := path.Join(root+"\\images\\")

	// Authenticate - Need a special person group client for your person group
	personGroupClient := face.NewPersonGroupClient(sourceEndpoint)
	personGroupClient.Authorizer = autorest.NewCognitiveServicesAuthorizer(sourceSubscriptionKey)

	// Create the Person Group
	// Create an empty Person Group. Person Group ID must be lower case, alphanumeric, and/or with '-', '_'.
	personGroupID := "unique-person-group"
	fmt.Println("Person group ID: " + personGroupID)
	metadata := face.MetaDataContract { Name: &personGroupID }

	// Create the person group
	personGroupClient.Create(faceContext, personGroupID, metadata)
	
	// Authenticate - Need a special person group person client for your person group person
	personGroupPersonClient := face.NewPersonGroupPersonClient(sourceEndpoint)
	personGroupPersonClient.Authorizer = autorest.NewCognitiveServicesAuthorizer(sourceSubscriptionKey)

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
	/*
	END - Person Group operations
	*/

	/*
	Identify a face
	Uses an existing person group to identify a face in an image
	*/
	fmt.Println()
	fmt.Println("------------------------------")
	fmt.Println("IDENTIFY FACES...")
	personGroupTestImageName := "test-image-person-group.jpg"
	// Use image path root from the one created in person group
	personGroupTestImagePath := imagePathRoot
	var personGroupTestImage io.ReadCloser
	// Returns a ReaderCloser
	personGroupTestImage, identErr:= os.Open(personGroupTestImagePath+personGroupTestImageName)
	if identErr != nil { log.Fatal(identErr) }
	
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

	// Identify the faces in the test image with everyone in the person group as a query
	identifyRequestBody := face.IdentifyRequest { FaceIds: &testImageFaceIDs, PersonGroupID: &personGroupID }
	identifiedFaces, err := client.Identify(faceContext, identifyRequestBody)
	if err != nil { log.Fatal(err) }

	// Get the result which person(s) were identified
	iFaces := *identifiedFaces.Value
	for _, person := range iFaces {
		fmt.Println("Person for face ID: " )
		fmt.Print(person.FaceID)
		fmt.Println(" is identified in " + personGroupTestImageName + ".")
	}
	/*
	END - Identify a face
	*/

	/*
	Take a snapshot
	This sample moves a person group from one region to another. The person group created in Person Group Operations will be used.
	You must have 2 Face resources created in Azure with 2 different regions, for example 'westus' and 'eastus'. Any region will work.
	Snapshot requires its own special authenticated client.
	*/
	fmt.Println()
	fmt.Println("------------------------------")
	fmt.Println("TAKING A SNAPSHOT...")

	// Create a client from your source region, where your person group exists. Use for taking the snapshot.
	snapshotSourceClient := face.NewSnapshotClient(sourceEndpoint)
	snapshotSourceClient.Authorizer = autorest.NewCognitiveServicesAuthorizer(sourceSubscriptionKey)
	// Create a client for your target region. Use for applying the snapshot.
	snapshotTargetClient := face.NewSnapshotClient(targetEndpoint)
	snapshotTargetClient.Authorizer = autorest.NewCognitiveServicesAuthorizer(targetSubscriptionKey)

	// Add your Azure subscription ID(s) to a UUID array.
	numberOfSubKeys := 1 
	targetUUIDArray := make([]uuid.UUID, numberOfSubKeys)
	for i := range targetUUIDArray {
		targetUUIDArray[i] = azureSubscriptionID
	}
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

	fmt.Println("Applying snapshot... Done")
	/*
	END - Take a snapshot
	*/

	/*
	Delete Person Group
	Delete once sample is complete. This deletes a person group, since we are only testing. 
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