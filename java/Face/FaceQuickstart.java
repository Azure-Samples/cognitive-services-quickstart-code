import com.microsoft.azure.cognitiveservices.vision.faceapi.*;
import com.microsoft.azure.cognitiveservices.vision.faceapi.models.*;

import java.util.*;
import java.util.concurrent.TimeUnit;

/**
 * This quickstart contains:
 *  - Detect Faces: detect a face or faces in an image and URL
 *  - Find Similar: find face similar to the single-faced image in the group image
 *  - Verify: with 2 images, check if they are the same person or different people
 *  - Identify: with grouped images of the same person, use group to find similar faces in another image
 *  - Group Faces: groups images into sub-groups based on same person images
 *  - Face Lists: adds images to a face list, then retrieves them
 *  - Delete: deletes the person group and face list to enable repeated testing
 * 
 * Prerequisites:
 * - Create a Face subscription in the Azure portal.
 * - Create a lib folder in the root directory of your project, then add the jars from dependencies.txt
 * - Download the FaceAPI library (ms-azure-cs-faceapi.jar) from this repo and add to your lib folder.
 * - Replace the "REGION" variable in the authenticate section with your region. 
 *   The "westus" region is used, otherwise, as the default. 
 *   NOTE: this quickstart does not need your Face endpoint.
 * 
 * To compile and run, enter the following at a command prompt:
 *   javac FaceQuickstart.java -cp .;lib\*
 *   java -cp .;lib\* FaceQuickstart
 *
 * Note If you run this sample with JRE 9+, you may encounter the following issue: 
 * https://github.com/Azure/autorest-clientruntime-for-java/issues/569 which results in the following output:
 * WARNING: An illegal reflective access operation has occurred ... (plus several more warnings)
 *
 * This should not prevent the sample from running correctly, so this can be ignored.
 * 
 * References:
 *  - Face Documentation: https://docs.microsoft.com/en-us/azure/cognitive-services/face/
 *  - Face Java SDK: https://docs.microsoft.com/en-us/java/api/overview/azure/cognitiveservices/client/faceapi?view=azure-java-stable
 *  - API Reference: https://docs.microsoft.com/en-us/azure/cognitive-services/face/apireference
 */

public class FaceQuickstart {

    public static void main(String[] args) {

        // For Detect Faces and Find Similar Faces examples
        // This image should have a single face.
        final String SINGLE_FACE_URL = "https://www.biography.com/.image/t_share/MTQ1MzAyNzYzOTgxNTE0NTEz/john-f-kennedy---mini-biography.jpg";
        final String SINGLE_IMAGE_NAME = 
                SINGLE_FACE_URL.substring(SINGLE_FACE_URL.lastIndexOf('/')+1, SINGLE_FACE_URL.length());
        // This image should have several faces. At least one should be similar to the face in singleFaceImage.
        final String  GROUP_FACES_URL = "http://www.historyplace.com/kennedy/president-family-portrait-closeup.jpg";
        final String GROUP_IMAGE_NAME = 
                GROUP_FACES_URL.substring(GROUP_FACES_URL.lastIndexOf('/')+1, GROUP_FACES_URL.length());

        // For Identify, Verify, Group Faces, and Face Lists examples
        final String IMAGE_BASE_URL = "https://csdx.blob.core.windows.net/resources/Face/Images/";

        // Used for the Identify example and Delete examples
        final String PERSON_GROUP_ID = "my-families"; // can be any lowercase, 0-9, "-", or "_" character.
        // Used for the Face List and Delete examples
        final String FACE_LIST_ID = "my-families-list";

        /**
         * Authenticate
         */
        // Add FACE_SUBSCRIPTION_KEY to your environment variables with your key as the value.
        final String key = System.getenv("FACE_SUBSCRIPTION_KEY");

        // Add your region of your Face subscription, for example 'westus', 'eastus', etc.
        // List of Azure Regions: https://docs.microsoft.com/en-us/java/api/com.microsoft.azure.cognitiveservices.vision.faceapi.models.azureregions?view=azure-java-stable
        final AzureRegions myRegion = AzureRegions.WESTUS;

        // Create Face client
        FaceAPI client = FaceAPIManager.authenticate(REGION, KEY);
        /**
         * END - Authenticate
         */

        System.out.println("============== Detect Face ==============");
        // Detect the face in a single-faced image. Returns a list of UUIDs and prints them.
        List<UUID> singleFaceIDs = detectFaces(client, SINGLE_FACE_URL, SINGLE_IMAGE_NAME);
        // Detect the faces in a group image. Returns a list of UUIDs and prints them.
        List<UUID> groupFaceIDs = detectFaces(client, GROUP_FACES_URL, GROUP_IMAGE_NAME);

        System.out.println("============== Find Similar ==============");
        // Finds a similar face in group image. Returns a list of UUIDs and prints them.
        findSimilar(client, singleFaceIDs, groupFaceIDs, GROUP_IMAGE_NAME);
        
        System.out.println("============== Verify ==============");
        // Checks if 2 photos are of the same or different person.
        verify(client, IMAGE_BASE_URL);

        System.out.println("============== Identify ==============");
        // Groups similar photos of a person, then uses that group 
        // to recognize the person in another photo.
        identifyFaces(client, IMAGE_BASE_URL, PERSON_GROUP_ID);
        
        System.out.println("============== Group Faces ==============");
        // Groups all faces in list into sub-groups based on similar faces.
        groupFaces(client, IMAGE_BASE_URL);
        
        System.out.println("============== Face Lists ==============");
        faceLists(client, IMAGE_BASE_URL, FACE_LIST_ID);

        System.out.println("============== Delete ==============");
        delete(client, PERSON_GROUP_ID, FACE_LIST_ID);
    }
    /**
     * END - Main
     */

    /**
     * Detect Face
     * Detects the face(s) in an image URL.
     */
    public static List<UUID> detectFaces(FaceAPI client, String imageURL, String imageName) {
        // Create face IDs list
        List<DetectedFace> facesList = client.faces().detectWithUrl(imageURL, new DetectWithUrlOptionalParameter().withReturnFaceId(true));
        System.out.println("Detected face ID(s) from URL image: " + imageName  + " :");
        // Get face(s) UUID(s)
        List<UUID> faceUuids = new ArrayList<>();
        for (DetectedFace face : facesList) {
            faceUuids.add(face.faceId());
            System.out.println(face.faceId()); 
        }
        System.out.println();

        return faceUuids;
    }
    /**
     * END - Detect Face
     */

    /**
     * Find Similar
     * Finds a similar face in another image with 2 lists of face IDs. 
     * Returns the IDs of those that are similar.
     */
    public static List<UUID> findSimilar(FaceAPI client, List<UUID> singleFaceList, List<UUID> groupFacesList, String groupImageName) {
        // With our list of the single-faced image ID and the list of group IDs, check if any similar faces.
        List<SimilarFace> listSimilars = client.faces().findSimilar(singleFaceList.get(0),
                                             new FindSimilarOptionalParameter().withFaceIds(groupFacesList));
        // Display the similar faces found
        System.out.println();
        System.out.println("Similar faces found in group photo " + groupImageName + " are:");
        // Create a list of UUIDs to hold the similar faces found
        List<UUID> similarUuids = new ArrayList<>();
        for (SimilarFace face : listSimilars) {
            similarUuids.add(face.faceId());
            System.out.println("Face ID: " + face.faceId());
            // Get and print the level of certainty that there is a match
            // Confidence range is 0.0 to 1.0. Closer to 1.0 is more confident
            System.out.println("Confidence: " + face.confidence());
        }
        System.out.println();

        return similarUuids;
    }
    /**
     * END - Find Similar
     */
    
    /**
     * Verify
     * With 2 photos, compare them to check if they are the same or different person.
     */
    public static void verify(FaceAPI client, String imageBaseURL) {

        // Source images to use for the query
        String sourceImage1 = "Family1-Dad3.jpg";
        String sourceImage2 = "Family1-Son1.jpg";

        // The target images to find similarities in.
        List<String> targetImages = new ArrayList<>();
        targetImages.add("Family1-Dad1.jpg");
        targetImages.add("Family1-Dad2.jpg");

        // Detect faces in the source images
        List<UUID> source1ID = detectFaces(client, imageBaseURL + sourceImage1, sourceImage1);
        List<UUID> source2ID = detectFaces(client, imageBaseURL + sourceImage2, sourceImage2);

        // Create list to hold target image IDs
        List<UUID> targetIDs = new ArrayList<>(); 

        // Detect the faces in the target images
        for (String face : targetImages) {
            List<UUID> faceId = detectFaces(client, imageBaseURL + face, face);
            targetIDs.add(faceId.get(0));
        }

        // Verification example for faces of the same person.
        VerifyResult sameResult = client.faces().verifyFaceToFace(source1ID.get(0), targetIDs.get(0));
        System.out.println(sameResult.isIdentical() ? 
            "Faces from " + sourceImage1 + " & " + targetImages.get(0) + " are of the same person." : 
            "Faces from " + sourceImage1 + " & " + targetImages.get(0) + " are different people.");

        // Verification example for faces of different persons.
        VerifyResult differentResult = client.faces().verifyFaceToFace(source2ID.get(0), targetIDs.get(0));
        System.out.println(differentResult.isIdentical() ? 
            "Faces from " + sourceImage2 + " & " + targetImages.get(1) + " are of the same person." : 
            "Faces from " + sourceImage2 + " & " + targetImages.get(1) + " are different people.");

            System.out.println();
    }
    /**
     * END - Verify
     */

    /**
     * Identify Faces
    * To identify a face, a list of detected faces and a person group are used.
    * The list of similar faces are assigned to one person group person, 
    * to teach the AI how to identify future images of that person.
    * Uses the detectFaces() method from this quickstart.
    */
    public static void identifyFaces(FaceAPI client, String imageBaseURL, String personGroupID) {
        // Create a dictionary to hold all your faces
        Map<String, String[]> facesList = new HashMap<String, String[]>();
        facesList.put("Family1-Dad", new String[] { "Family1-Dad1.jpg", "Family1-Dad2.jpg" });
        facesList.put("Family1-Mom", new String[] { "Family1-Mom1.jpg", "Family1-Mom2.jpg" });
        facesList.put("Family1-Son", new String[] { "Family1-Son1.jpg", "Family1-Son2.jpg" });
        facesList.put("Family1-Daughter", new String[] { "Family1-Daughter1.jpg", "Family1-Daughter2.jpg" });
        facesList.put("Family2-Lady", new String[] { "Family2-Lady1.jpg", "Family2-Lady2.jpg" });
        facesList.put("Family2-Man", new String[] { "Family2-Man1.jpg", "Family2-Man2.jpg" });

        // A group photo that includes some of the persons you seek to identify from your dictionary.
        String groupPhoto = "identification1.jpg";

        System.out.println("Creating the person group " + personGroupID + " ...");
        // Create the person group, so our photos have one to belong to.
        client.personGroups().create(personGroupID, new CreatePersonGroupsOptionalParameter().withName(personGroupID));
        
        // Group the faces. Each array of similar faces will be grouped into a single person group person.
        for (String personName : facesList.keySet()) {
            // Associate the family member name with an ID, by creating a Person object.
            UUID personID = UUID.randomUUID();
            Person person = client.personGroupPersons().create(personGroupID, 
                    new CreatePersonGroupPersonsOptionalParameter().withName(personName));

            for (String personImage : facesList.get(personName)) {
                // Add each image in array to a person group person (represented by the key and person ID).
                client.personGroupPersons().addPersonFaceFromUrl(personGroupID, person.personId(), imageBaseURL + personImage, null);
            } 
        }  

        // Once images are added to a person group person, train the person group.
        System.out.println();
        System.out.println("Training person group " + personGroupID + " ...");
        client.personGroups().train(personGroupID);

        // Wait until the training is completed.
        while(true) {
            try {
                TimeUnit.SECONDS.sleep(1);
            } catch (InterruptedException e) { e.printStackTrace(); }
            
            // Get training status
            TrainingStatus status = client.personGroups().getTrainingStatus(personGroupID);
            if (status.status() == TrainingStatusType.SUCCEEDED) {
                System.out.println("Training status: " + status.status());
                break;
            }
            System.out.println("Training status: " + status.status());
        }
        System.out.println();

        // Detect faces from our group photo (which may contain one of our person group persons)
        List<UUID> detectedFaces = detectFaces(client, imageBaseURL + groupPhoto, groupPhoto);
        // Identifies which faces in group photo are in our person group. 
        List<IdentifyResult> identifyResults = client.faces().identify(personGroupID, detectedFaces, null);
        // Print each person group person (the person ID) identified from our results.
        System.out.println("Persons identified in group photo " + groupPhoto + ": ");
        for (IdentifyResult person : identifyResults) {
            System.out.println("Person ID: " + person.faceId().toString() 
                        + " with confidence " + person.candidates().get(0).confidence());
        }
    }
    /**
     * END - Identify Faces
     */
    
    /**
     * Group Faces
     * This method of grouping is useful if you don't need to create a person group. It will automatically group similar
     * images, whereas the person group method allows you to define the grouping.
     * A single "messyGroup" array contains face IDs for which no similarities were found.
     */
    public static void groupFaces(FaceAPI client, String imageBaseURL) {
        // Images we want to group
        List<String> imagesList = new ArrayList<>();
        imagesList.add("Family1-Dad1.jpg");
        imagesList.add("Family1-Dad2.jpg");
        imagesList.add("Family3-Lady1.jpg");
        imagesList.add("Family1-Daughter1.jpg");
        imagesList.add("Family1-Daughter2.jpg");
        imagesList.add("Family1-Daughter3.jpg");

        // Create empty dictionary to store the groups
        Map<String, String> faces = new HashMap<>();
        List<UUID> faceIds = new ArrayList<>();

        // First, detect the faces in your images
        for (String image : imagesList) {
            // Detect faces from image url (prints detected face results)
            List<UUID> detectedFaces = detectFaces(client, imageBaseURL + image, image);
            // Add detected faceId to faceIds and faces.
            faceIds.add(detectedFaces.get(0)); // get first in list, since all images only have 1 face.
            faces.put(detectedFaces.get(0).toString(), image);
        }

        // Group the faces. Grouping result is a collection that contains similar faces and a "messy group".
        GroupResult results = client.faces().group(faceIds);

        // Find the number of groups (inner lists) found in all images.
        // GroupResult.groups() returns a List<List<UUID>>.
        for (int i = 0; i < results.groups().size(); i++) {
            System.out.println("Found face group " + (i + 1) + ": ");
            for (UUID id : results.groups().get(i)) {
                // Print the IDs from each group found, as seen associated in your map.
                System.out.println(id);
            }
            System.out.println();
        }

        // MessyGroup contains all faces which are not similar to any other faces. 
        // The faces that cannot be grouped by similarities. Odd ones out.
        System.out.println("Found messy group: ");
        for (UUID mID : results.messyGroup()) {
            System.out.println(mID);
        }
        System.out.println();
    }
    /**
     * END - Group Faces
     */
    
    /**
     * Face Lists
     * This example adds images to a face list. Can add up to 1 million.
     */
    public static void faceLists(FaceAPI client, String imageBaseURL, String faceListID) {
        // Images we want to the face list
        List<String> imagesList = new ArrayList<>();
        imagesList.add("Family1-Dad1.jpg");
        imagesList.add("Family1-Dad2.jpg");
        imagesList.add("Family3-Lady1.jpg");
        imagesList.add("Family1-Daughter1.jpg");
        imagesList.add("Family1-Daughter2.jpg");
        imagesList.add("Family1-Daughter3.jpg");

        // Create an empty face list with a face list ID
        // (optional) Add the name you want to give the list to the CreateFaceListsOptionalParameter
        System.out.println("Creating the face list " + faceListID + " ...");
        client.faceLists().create(faceListID, new CreateFaceListsOptionalParameter().withName(faceListID));

        // Add each image from our ArrayList to our face list
        for (String image : imagesList) {
            // Returns a PersistedFace object
            client.faceLists().addFaceFromUrl(faceListID, imageBaseURL + image, null);
        }

        // Get the persisted faces we added to our face list.
        FaceList retrievedFaceList = client.faceLists().get(faceListID);

        // Print the UUIDs retrieved
        System.out.println("Face list IDs: ");
        for (PersistedFace face : retrievedFaceList.persistedFaces()) {
            System.out.println(face.persistedFaceId());
        }
        System.out.println();
    }
    /**
     * END - Face Lists
     */
    
    /**
     * Delete
     * The delete operations erase the person group and face list from your API account,
     * so you can test multiple times with the same name.
     */
    public static void delete(FaceAPI client, String personGroupID, String faceListID){
        // Delete the person group
        // There is also an option in the SDK to delete one Person 
        // from the person group, but we don't show that here.
        System.out.println("Deleting the person group...");
        client.personGroups().delete(personGroupID);
        System.out.println("Deleted the person group " + personGroupID);

        // Delete the entire face list
        // There is also an option in the SDK to delete one face 
        // from the list, but we don't show that here.
        System.out.println("Deleting the face list...");
        client.faceLists().delete(faceListID);
        System.out.println("Deleted the face list " + faceListID);
    }
    /**
     * END - Delete
     */
}

