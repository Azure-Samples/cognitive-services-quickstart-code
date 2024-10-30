# <detect_for_similar>
curl -v -X POST "https://{resource endpoint}/face/v1.0/detect?detectionModel=detection_03&recognitionModel=recognition_04&returnFaceId=true" -H "Content-Type: application/json" -H "Ocp-Apim-Subscription-Key: {subscription key}" --data-ascii "{\"url\":\"https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/Family1-Dad1.jpg\"}"
# </detect_for_similar>

# <similar_group>
https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/Family1-Daughter1.jpg
https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/Family1-Mom1.jpg
https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/Family1-Son1.jpg
https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/Family2-Lady1.jpg
https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/Family2-Man1.jpg
https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/Family3-Lady1.jpg
https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/Family3-Man1.jpg
# </similar_group>

# <similar_matcher>
https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/findsimilar.jpg
# </similar_matcher>

# <similar>
curl -v -X POST "https://{resource endpoint}/face/v1.0/findsimilars" -H "Content-Type: application/json" -H "Ocp-Apim-Subscription-Key: {subscription key}" --data-ascii "{body}" 
# </similar>

# <similar_body>
{
    "faceId": "ID_OF_FINDSIMILAR_JPG",
    "faceIds": ["ID_OF_FAMILY1_DAUGHTER1_JPG", "ID_OF_FAMILY1_MOM1_JPG", ..., "ID_OF_FAMILY3_MAN1_JPG"],
    "maxNumOfCandidatesReturned": 10,
    "mode": "matchPerson"
}
# </similar_body>

# IDENTIFICATION

# detect faces, save IDs
# <identify_detect>
curl -v -X POST "https://{resource endpoint}/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=false&recognitionModel=recognition_04&returnRecognitionModel=false&detectionModel=detection_03&faceIdTimeToLive=86400" -H "Content-Type: application/json" -H "Ocp-Apim-Subscription-Key: {subscription key}" --data-ascii "{\"url\":\"https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/identification1.jpg\"}" 
# </identify_detect>

# Create persongroup
# <identify_create_persongroup>
curl -v -X PUT "https://{resource endpoint}/face/v1.0/largepersongroups/{largePersonGroupId}" -H "Content-Type: application/json" -H "Ocp-Apim-Subscription-Key: {subscription key}" --data-ascii "{
    \"name\": \"large-person-group-name\",
    \"userData\": \"User-provided data attached to the large person group.\",
    \"recognitionModel\": \"recognition_04\"
}" 
# </identify_create_persongroup>

# create persons 
# <identify_create_person>
curl -v -X POST "https://{resource endpoint}/face/v1.0/largepersongroups/{largePersonGroupId}/persons" -H "Content-Type: application/json" -H "Ocp-Apim-Subscription-Key: {subscription key}" --data-ascii "{
    \"name\": \"Family1-Dad\",
    \"userData\": \"User-provided data attached to the person.\"
}" 
# </identify_create_person>

# Add faces to person
# <identify_add_face>
curl -v -X POST "https://{resource endpoint}/face/v1.0/largepersongroups/{largePersonGroupId}/persons/{personId}/persistedfaces?detectionModel=detection_03" -H "Content-Type: application/json" -H "Ocp-Apim-Subscription-Key: {subscription key}" --data-ascii "{\"url\":\"https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/Family1-Dad1.jpg\"}" 
# </identify_add_face>

# Train persongroup
# <identify_train>
curl -v -X POST "https://{resource endpoint}/face/v1.0/largepersongroups/{largePersonGroupId}/train" -H "Ocp-Apim-Subscription-Key: {subscription key}" --data "" 
# </identify_train>

# Check training status
# <identify_check_status>
curl -v "https://{resource endpoint}/face/v1.0/largepersongroups/{largePersonGroupId}/training" -H "Ocp-Apim-Subscription-Key: {subscription key}"
# </identify_check_status>

# call identify operation
# <identify_identify>
curl -v -X POST "https://{resource endpoint}/face/v1.0/identify" -H "Content-Type: application/json" -H "Ocp-Apim-Subscription-Key: {subscription key}" --data-ascii "{
    \"largePersonGroupId\": \"INSERT_PERSONGROUP_ID\",
    \"faceIds\": [
        \"INSERT_SOURCE_FACE_ID\"
    ],
    \"maxNumOfCandidatesReturned\": 1,
    \"confidenceThreshold\": 0.5
}"
# </identify_identify>

# delete the persongroup
# <identify_delete>
curl -v -X DELETE "https://{resource endpoint}/face/v1.0/largepersongroups/{largePersonGroupId}" -H "Ocp-Apim-Subscription-Key: {subscription key}"
# </identify_delete>

# <verify>
curl -v -X POST "https://{resource endpoint}/face/v1.0/verify" \
-H "Content-Type: application/json" \
-H "Ocp-Apim-Subscription-Key: {subscription key}" \
--data-ascii "{
    \"faceId\": \"INSERT_SOURCE_FACE_ID\",
    \"personId\": \"INSERT_PERSON_ID\",
    \"largePersonGroupId\": \"INSERT_PERSONGROUP_ID\"
}" 
# </verify>
