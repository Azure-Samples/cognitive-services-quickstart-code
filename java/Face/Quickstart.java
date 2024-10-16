// <snippet_single>
import java.util.Arrays;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;
import java.util.UUID;

import com.azure.ai.vision.face.FaceClient;
import com.azure.ai.vision.face.FaceClientBuilder;
import com.azure.ai.vision.face.models.DetectOptions;
import com.azure.ai.vision.face.models.FaceAttributeType;
import com.azure.ai.vision.face.models.FaceDetectionModel;
import com.azure.ai.vision.face.models.FaceDetectionResult;
import com.azure.ai.vision.face.models.FaceRecognitionModel;
import com.azure.ai.vision.face.models.QualityForRecognition;
import com.azure.core.credential.KeyCredential;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import org.apache.http.HttpHeaders;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpDelete;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.client.methods.HttpPut;
import org.apache.http.client.utils.URIBuilder;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.HttpClients;
import org.apache.http.message.BasicHeader;
import org.apache.http.util.EntityUtils;

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
        List<BasicHeader> headers = Arrays.asList(new BasicHeader("Ocp-Apim-Subscription-Key", SUBSCRIPTION_KEY), new BasicHeader(HttpHeaders.CONTENT_TYPE, "application/json"));
        HttpClient httpClient = HttpClients.custom().setDefaultHeaders(headers).build();
        createLargePersonGroup(httpClient, recognitionModel);
        // The similar faces will be grouped into a single large person group person.
        for (String groupedFace : personDictionary.keySet()) {
            // Limit TPS
            Thread.sleep(250);
            String personId = createLargePersonGroupPerson(httpClient, groupedFace);
            System.out.println("Create a person group person '" + groupedFace + "'.");

            // Add face to the large person group person.
            for (String similarImage : personDictionary.get(groupedFace)) {
                System.out.println("Check whether image is of sufficient quality for recognition");
                DetectOptions options = new DetectOptions(FaceDetectionModel.DETECTION_03, recognitionModel, false).setReturnFaceAttributes(Arrays.asList(FaceAttributeType.QUALITY_FOR_RECOGNITION));
                List<FaceDetectionResult> detectedFaces1 = client.detect(url + similarImage, options);
                if (detectedFaces1.stream().anyMatch(f -> f.getFaceAttributes().getQualityForRecognition() != QualityForRecognition.HIGH)) {
                    continue;
                }

                if (detectedFaces1.size() != 1) {
                    continue;
                }

                // add face to the large person group
                System.out.println("Add face to the person group person(" + groupedFace + ") from image `" + similarImage + "`");
                addFaceToLargePersonGroup(httpClient, personId, url + similarImage);
            }
        }

        // Start to train the large person group.
        System.out.println();
        System.out.println("Train person group " + LARGE_PERSON_GROUP_ID + ".");
        trainLargePersonGroup(httpClient);

        // Wait until the training is completed.
        while (true) {
            Thread.sleep(1000);
            String trainingStatus = getLargePersonGroupTrainingStatus(httpClient);
            System.out.println("Training status: " + trainingStatus + ".");
            if ("succeeded".equals(trainingStatus)) {
                break;
            }
        }
        System.out.println();

        System.out.println("Pausing for 60 seconds to avoid triggering rate limit on free account...");
        Thread.sleep(60000);

        // Detect faces from source image url.
        List<FaceDetectionResult> detectedFaces = detectFaceRecognize(client, url + sourceImageFileName, recognitionModel);
        // Add detected faceId to sourceFaceIds.
        List<String> sourceFaceIds = detectedFaces.stream().map(FaceDetectionResult::getFaceId).collect(Collectors.toList());

        // Identify the faces in a large person group.
        List<Map<String, Object>> identifyResults = identifyFacesInLargePersonGroup(httpClient, sourceFaceIds);

        for (Map<String, Object> identifyResult : identifyResults) {
            String faceId = identifyResult.get("faceId").toString();
            List<Map<String, Object>> candidates = new Gson().fromJson(new Gson().toJson(identifyResult.get("candidates")), new TypeToken<List<Map<String, Object>>>(){});
            if (candidates.isEmpty()) {
                System.out.println("No person is identified for the face in: " + sourceImageFileName + " - " + faceId + ".");
                continue;
            }

            Map<String, Object> candidate = candidates.stream().findFirst().orElseThrow();
            String personName = getLargePersonGroupPersonName(httpClient, candidate.get("personId").toString());
            System.out.println("Person '" + personName + "' is identified for the face in: " + sourceImageFileName + " - " + faceId + ", confidence: " + candidate.get("confidence") + ".");

            Map<String, Object> verifyResult = verifyFaceWithLargePersonGroupPerson(httpClient, faceId, candidate.get("personId").toString());
            System.out.println("Verification result: is a match? " + verifyResult.get("isIdentical") + ". confidence: " + verifyResult.get("confidence"));
        }
        System.out.println();

        // Delete large person group.
        System.out.println("========DELETE PERSON GROUP========");
        System.out.println();
        deleteLargePersonGroup(httpClient);
        System.out.println("Deleted the person group " + LARGE_PERSON_GROUP_ID + ".");
        System.out.println();
    }

    private static void createLargePersonGroup(HttpClient httpClient, FaceRecognitionModel recognitionModel) throws Exception {
        HttpPut request = new HttpPut(new URIBuilder(ENDPOINT + "/face/v1.0/largepersongroups/" + LARGE_PERSON_GROUP_ID).build());
        request.setEntity(new StringEntity(new Gson().toJson(Map.of("name", LARGE_PERSON_GROUP_ID, "recognitionModel", recognitionModel.toString()))));
        httpClient.execute(request);
        request.releaseConnection();
    }

    private static String createLargePersonGroupPerson(HttpClient httpClient, String name) throws Exception {
        HttpPost request = new HttpPost(new URIBuilder(ENDPOINT + "/face/v1.0/largepersongroups/" + LARGE_PERSON_GROUP_ID + "/persons").build());
        request.setEntity(new StringEntity(new Gson().toJson(Map.of("name", name))));
        String response = EntityUtils.toString(httpClient.execute(request).getEntity());
        request.releaseConnection();
        return new Gson().fromJson(response, new TypeToken<Map<String, Object>>(){}).get("personId").toString();
    }

    private static void addFaceToLargePersonGroup(HttpClient httpClient, String personId, String url) throws Exception {
        URIBuilder builder = new URIBuilder(ENDPOINT + "/face/v1.0/largepersongroups/" + LARGE_PERSON_GROUP_ID + "/persons/" + personId + "/persistedfaces");
        builder.setParameter("detectionModel", "detection_03");
        HttpPost request = new HttpPost(builder.build());
        request.setEntity(new StringEntity(new Gson().toJson(Map.of("url", url))));
        httpClient.execute(request);
        request.releaseConnection();
    }

    private static void trainLargePersonGroup(HttpClient httpClient) throws Exception {
        HttpPost request = new HttpPost(new URIBuilder(ENDPOINT + "/face/v1.0/largepersongroups/" + LARGE_PERSON_GROUP_ID + "/train").build());
        httpClient.execute(request);
        request.releaseConnection();
    }

    private static String getLargePersonGroupTrainingStatus(HttpClient httpClient) throws Exception {
        HttpGet request = new HttpGet(new URIBuilder(ENDPOINT + "/face/v1.0/largepersongroups/" + LARGE_PERSON_GROUP_ID + "/training").build());
        String response = EntityUtils.toString(httpClient.execute(request).getEntity());
        request.releaseConnection();
        return new Gson().fromJson(response, new TypeToken<Map<String, Object>>(){}).get("status").toString();
    }

    private static List<Map<String, Object>> identifyFacesInLargePersonGroup(HttpClient httpClient, List<String> sourceFaceIds) throws Exception {
        HttpPost request = new HttpPost(new URIBuilder(ENDPOINT + "/face/v1.0/identify").build());
        request.setEntity(new StringEntity(new Gson().toJson(Map.of("faceIds", sourceFaceIds, "largePersonGroupId", LARGE_PERSON_GROUP_ID))));
        String response = EntityUtils.toString(httpClient.execute(request).getEntity());
        request.releaseConnection();
        return new Gson().fromJson(response, new TypeToken<List<Map<String, Object>>>(){});
    }

    private static String getLargePersonGroupPersonName(HttpClient httpClient, String personId) throws Exception {
        HttpGet request = new HttpGet(new URIBuilder(ENDPOINT + "/face/v1.0/largepersongroups/" + LARGE_PERSON_GROUP_ID + "/persons/" + personId).build());
        String response = EntityUtils.toString(httpClient.execute(request).getEntity());
        request.releaseConnection();
        return new Gson().fromJson(response, new TypeToken<Map<String, Object>>(){}).get("name").toString();
    }

    private static Map<String, Object> verifyFaceWithLargePersonGroupPerson(HttpClient httpClient, String faceId, String personId) throws Exception {
        HttpPost request = new HttpPost(new URIBuilder(ENDPOINT + "/face/v1.0/verify").build());
        request.setEntity(new StringEntity(new Gson().toJson(Map.of("faceId", faceId, "personId", personId, "largePersonGroupId", LARGE_PERSON_GROUP_ID))));
        String response = EntityUtils.toString(httpClient.execute(request).getEntity());
        request.releaseConnection();
        return new Gson().fromJson(response, new TypeToken<Map<String, Object>>(){});
    }

    private static void deleteLargePersonGroup(HttpClient httpClient) throws Exception {
        HttpDelete request = new HttpDelete(new URIBuilder(ENDPOINT + "/face/v1.0/largepersongroups/" + LARGE_PERSON_GROUP_ID).build());
        httpClient.execute(request);
        request.releaseConnection();
    }
}
// </snippet_single>