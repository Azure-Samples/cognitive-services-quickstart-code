# IDENTIFICATION

# detect faces, save IDs
# <identify_detect>
curl.exe -v -X POST "https://{resource endpoint}/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=false&recognitionModel=recognition_04&returnRecognitionModel=false&detectionModel=detection_03&faceIdTimeToLive=86400" -H "Content-Type: application/json" -H "Ocp-Apim-Subscription-Key: {subscription key}" --data-ascii "{""url"":""https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/identification1.jpg""}" 
# </identify_detect>

# Create persongroup
# <identify_create_persongroup>
curl.exe -v -X PUT "https://{resource endpoint}/face/v1.0/largepersongroups/{largePersonGroupId}" -H "Content-Type: application/json" -H "Ocp-Apim-Subscription-Key: {subscription key}" --data-ascii "{ `
    ""name"": ""large-person-group-name"", `
    ""userData"": ""User-provided data attached to the large person group."", `
    ""recognitionModel"": ""recognition_04"" `
}" 
# </identify_create_persongroup>

# create persons 
# <identify_create_person>
curl.exe -v -X POST "https://{resource endpoint}/face/v1.0/largepersongroups/{largePersonGroupId}/persons" -H "Content-Type: application/json" -H "Ocp-Apim-Subscription-Key: {subscription key}" --data-ascii "{ `
    ""name"": ""Family1-Dad"", `
    ""userData"": ""User-provided data attached to the person."" `
}" 
# </identify_create_person>

# Add faces to person
# <identify_add_face>
curl.exe -v -X POST "https://{resource endpoint}/face/v1.0/largepersongroups/{largePersonGroupId}/persons/{personId}/persistedfaces?detectionModel=detection_03" -H "Content-Type: application/json" -H "Ocp-Apim-Subscription-Key: {subscription key}" --data-ascii "{""url"":""https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/Family1-Dad1.jpg""}" 
# </identify_add_face>

# Train persongroup
# <identify_train>
curl.exe -v -X POST "https://{resource endpoint}/face/v1.0/largepersongroups/{largePersonGroupId}/train" -H "Ocp-Apim-Subscription-Key: {subscription key}" --data "" 
# </identify_train>

# Check training status
# <identify_check_status>
curl.exe -v "https://{resource endpoint}/face/v1.0/largepersongroups/{largePersonGroupId}/training" -H "Ocp-Apim-Subscription-Key: {subscription key}"
# </identify_check_status>

# call identify operation
# <identify_identify>
curl.exe -v -X POST "https://{resource endpoint}/face/v1.0/identify" -H "Content-Type: application/json" -H "Ocp-Apim-Subscription-Key: {subscription key}" --data-ascii "{ `
    ""largePersonGroupId"": ""INSERT_PERSONGROUP_ID"", `
    ""faceIds"": [ `
        ""INSERT_SOURCE_FACE_ID"" `
    ], `
    ""maxNumOfCandidatesReturned"": 1, `
    ""confidenceThreshold"": 0.5 `
}"
# </identify_identify>

# delete the persongroup
# <identify_delete>
curl.exe -v -X DELETE "https://{resource endpoint}/face/v1.0/largepersongroups/{largePersonGroupId}" -H "Ocp-Apim-Subscription-Key: {subscription key}"
# </identify_delete>

# <verify>
curl.exe -v -X POST "https://{resource endpoint}/face/v1.0/verify" `
-H "Content-Type: application/json" `
-H "Ocp-Apim-Subscription-Key: {subscription key}" `
--data-ascii "{ `
    ""faceId"": ""INSERT_SOURCE_FACE_ID"", `
    ""personId"": ""INSERT_PERSON_ID"", `
    ""largePersonGroupId"": ""INSERT_PERSONGROUP_ID"" `
}" 
# </verify>
