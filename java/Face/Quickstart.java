// <snippet_single>
import java.util.Arrays;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;
import java.util.UUID;

import com.azure.ai.vision.face.FaceClient;
import com.azure.ai.vision.face.FaceClientBuilder;
import com.azure.ai.vision.face.administration.FaceAdministrationClient;
import com.azure.ai.vision.face.administration.FaceAdministrationClientBuilder;
import com.azure.ai.vision.face.administration.LargePersonGroupClient;
import com.azure.ai.vision.face.models.DetectOptions;
import com.azure.ai.vision.face.models.FaceAttributeType;
import com.azure.ai.vision.face.models.FaceDetectionModel;
import com.azure.ai.vision.face.models.FaceDetectionResult;
import com.azure.ai.vision.face.models.FaceIdentificationCandidate;
import com.azure.ai.vision.face.models.FaceIdentificationResult;
import com.azure.ai.vision.face.models.FaceRecognitionModel;
import com.azure.ai.vision.face.models.FaceTrainingResult;
import com.azure.ai.vision.face.models.FaceVerificationResult;
import com.azure.ai.vision.face.models.QualityForRecognition;
import com.azure.core.credential.KeyCredential;
import com.azure.core.util.polling.SyncPoller;

public class Quickstart {
    // LARGE_PERSON_GROUP_ID should be all lowercase and alphanumeric. For example, 'mygroupname' (dashes are OK).
    private static final String LARGE_PERSON_GROUP_ID = UUID.randomUUID().toString();

    // URL path for the images.
    private static final String IMAGE_BASE_URL = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/";

    // From your Face subscription in the Azure portal, get your subscription key and endpoint.
    private static final String SUBSCRIPTION_KEY = System.getenv("FACE_APIKEY");
    private static final String ENDPOINT = System.getenv("FACE_ENDPOINT");

    public static void main(String[] args) throws Exception {
        // Recognition model 4 was released in 2021 February.
        // It is recommended since its accuracy is improved
        // on faces wearing masks compared with model 3,
        // and its overall accuracy is improved compared
        // with models 1 and 2.
        FaceRecognitionModel RECOGNITION_MODEL4 = FaceRecognitionModel.RECOGNITION_04;

        // Authenticate.
        FaceClient client = authenticate(ENDPOINT, SUBSCRIPTION_KEY);

        // Identify - recognize a face(s) in a large person group (a large person group is created in this example).
        identifyInLargePersonGroup(client, IMAGE_BASE_URL, RECOGNITION_MODEL4);

        System.out.println("End of quickstart.");
    }

    /*
     *	AUTHENTICATE
     *	Uses subscription key and region to create a client.
     */
    public static FaceClient authenticate(String endpoint, String key) {
        return new FaceClientBuilder().endpoint(endpoint).credential(new KeyCredential(key)).buildClient();
    }


    // Detect faces from image url for recognition purposes. This is a helper method for other functions in this quickstart.
    // Parameter `returnFaceId` of `DetectOptions` must be set to `true` (by default) for recognition purposes.
    // Parameter `returnFaceAttributes` is set to include the QualityForRecognition attribute. 
    // Recognition model must be set to recognition_03 or recognition_04 as a result.
    // Result faces with insufficient quality for recognition are filtered out. 
    // The field `faceId` in returned `DetectedFace`s will be used in Verify and Identify.
    // It will expire 24 hours after the detection call.
    private static List<FaceDetectionResult> detectFaceRecognize(FaceClient faceClient, String url, FaceRecognitionModel recognitionModel) {
        // Detect faces from image URL.
        DetectOptions options = new DetectOptions(FaceDetectionModel.DETECTION_03, recognitionModel, true).setReturnFaceAttributes(Arrays.asList(FaceAttributeType.QUALITY_FOR_RECOGNITION));
        List<FaceDetectionResult> detectedFaces = faceClient.detect(url, options);
        List<FaceDetectionResult> sufficientQualityFaces = detectedFaces.stream().filter(f -> f.getFaceAttributes().getQualityForRecognition() != QualityForRecognition.LOW).collect(Collectors.toList());
        System.out.println(detectedFaces.size() + " face(s) with " + sufficientQualityFaces.size() + " having sufficient quality for recognition.");

        return sufficientQualityFaces;
    }

    /*
     * IDENTIFY FACES
     * To identify faces, you need to create and define a large person group.
     * The Identify operation takes one or several face IDs from DetectedFace or PersistedFace and a LargePersonGroup and returns
     * a list of Person objects that each face might belong to. Returned Person objects are wrapped as Candidate objects,
     * which have a prediction confidence value.
     */
    public static void identifyInLargePersonGroup(FaceClient client, String url, FaceRecognitionModel recognitionModel) throws Exception {
        System.out.println("========IDENTIFY FACES========");
        System.out.println();

        // Create a dictionary for all your images, grouping similar ones under the same key.
        Map<String, String[]> personDictionary = new LinkedHashMap<String, String[]>();
        personDictionary.put("Family1-Dad", new String[]{"Family1-Dad1.jpg", "Family1-Dad2.jpg"});
        personDictionary.put("Family1-Mom", new String[]{"Family1-Mom1.jpg", "Family1-Mom2.jpg"});
        personDictionary.put("Family1-Son", new String[]{"Family1-Son1.jpg", "Family1-Son2.jpg"});
        // A group photo that includes some of the persons you seek to identify from your dictionary.
        String sourceImageFileName = "identification1.jpg";

        // Create a large person group.
        System.out.println("Create a person group (" + LARGE_PERSON_GROUP_ID + ").");
        FaceAdministrationClient faceAdministrationClient = new FaceAdministrationClientBuilder().endpoint(ENDPOINT).credential(new KeyCredential(SUBSCRIPTION_KEY)).buildClient();
        LargePersonGroupClient largePersonGroupClient = faceAdministrationClient.getLargePersonGroupClient(LARGE_PERSON_GROUP_ID);
        largePersonGroupClient.create(LARGE_PERSON_GROUP_ID, null, recognitionModel);
        // The similar faces will be grouped into a single large person group person.
        for (String groupedFace : personDictionary.keySet()) {
            // Limit TPS
            Thread.sleep(250);
            String personId = largePersonGroupClient.createPerson(groupedFace).getPersonId();
            System.out.println("Create a person group person '" + groupedFace + "'.");

            // Add face to the large person group person.
            for (String similarImage : personDictionary.get(groupedFace)) {
                System.out.println("Check whether image is of sufficient quality for recognition");
                DetectOptions options = new DetectOptions(FaceDetectionModel.DETECTION_03, recognitionModel, false).setReturnFaceAttributes(Arrays.asList(FaceAttributeType.QUALITY_FOR_RECOGNITION));
                List<FaceDetectionResult> facesInImage = client.detect(url + similarImage, options);
                if (facesInImage.stream().anyMatch(f -> f.getFaceAttributes().getQualityForRecognition() != QualityForRecognition.HIGH)) {
                    continue;
                }

                if (facesInImage.size() != 1) {
                    continue;
                }

                // add face to the large person group
                System.out.println("Add face to the person group person(" + groupedFace + ") from image `" + similarImage + "`");
                largePersonGroupClient.addFace(personId, url + similarImage, null, FaceDetectionModel.DETECTION_03, null);
            }
        }

        // Start to train the large person group.
        System.out.println();
        System.out.println("Train person group " + LARGE_PERSON_GROUP_ID + ".");
        SyncPoller<FaceTrainingResult, Void> poller = largePersonGroupClient.beginTrain();

        // Wait until the training is completed.
        poller.waitForCompletion();
        System.out.println("Training status: succeeded.");
        System.out.println();

        System.out.println("Pausing for 60 seconds to avoid triggering rate limit on free account...");
        Thread.sleep(60000);

        // Detect faces from source image url.
        List<FaceDetectionResult> detectedFaces = detectFaceRecognize(client, url + sourceImageFileName, recognitionModel);
        // Add detected faceId to sourceFaceIds.
        List<String> sourceFaceIds = detectedFaces.stream().map(FaceDetectionResult::getFaceId).collect(Collectors.toList());

        // Identify the faces in a large person group.
        List<FaceIdentificationResult> identifyResults = client.identifyFromLargePersonGroup(sourceFaceIds, LARGE_PERSON_GROUP_ID);

        for (FaceIdentificationResult identifyResult : identifyResults) {
            if (identifyResult.getCandidates().isEmpty()) {
                System.out.println("No person is identified for the face in: " + sourceImageFileName + " - " + identifyResult.getFaceId() + ".");
                continue;
            }

            FaceIdentificationCandidate candidate = identifyResult.getCandidates().stream().findFirst().orElseThrow();
            String personName = largePersonGroupClient.getPerson(candidate.getPersonId()).getName();
            System.out.println("Person '" + personName + "' is identified for the face in: " + sourceImageFileName + " - " + identifyResult.getFaceId() + ", confidence: " + candidate.getConfidence() + ".");

            FaceVerificationResult verifyResult = client.verifyFromLargePersonGroup(identifyResult.getFaceId(), LARGE_PERSON_GROUP_ID, candidate.getPersonId());
            System.out.println("Verification result: is a match? " + verifyResult.isIdentical() + ". confidence: " + verifyResult.getConfidence());
        }
        System.out.println();

        // Delete large person group.
        System.out.println("========DELETE PERSON GROUP========");
        System.out.println();
        largePersonGroupClient.delete();
        System.out.println("Deleted the person group " + LARGE_PERSON_GROUP_ID + ".");
        System.out.println();
    }
}
// </snippet_single>