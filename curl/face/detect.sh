# <detection_model_2>
curl -H "Ocp-Apim-Subscription-Key: TODO_INSERT_YOUR_FACE_SUBSCRIPTION_KEY_HERE" "TODO_INSERT_YOUR_FACE_ENDPOINT_HERE/face/v1.0/detect?detectionModel=detection_02&returnFaceId=true&returnFaceLandmarks=false" -H "Content-Type: application/json" --data-ascii "{\"url\":\"https://upload.wikimedia.org/wikipedia/commons/c/c3/RH_Louise_Lillian_Gish.jpg\"}"
# </detection_model_2>

# <detection_model_1>
curl -H "Ocp-Apim-Subscription-Key: TODO_INSERT_YOUR_FACE_SUBSCRIPTION_KEY_HERE" "TODO_INSERT_YOUR_FACE_ENDPOINT_HERE/face/v1.0/detect?detectionModel=detection_01&returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses,emotion,hair,makeup,occlusion,accessories,blur,exposure,noise" -H "Content-Type: application/json" --data-ascii "{\"url\":\"https://upload.wikimedia.org/wikipedia/commons/c/c3/RH_Louise_Lillian_Gish.jpg\"}"
# </detection_model_1>

# <detect_for_similar>
curl -H "Ocp-Apim-Subscription-Key: TODO_INSERT_YOUR_FACE_SUBSCRIPTION_KEY_HERE" "TODO_INSERT_YOUR_FACE_ENDPOINT_HERE/face/v1.0/detect?detectionModel=detection_02&returnFaceId=true&returnFaceLandmarks=false" -H "Content-Type: application/json" --data-ascii "{\"url\":\"https://csdx.blob.core.windows.net/resources/Face/Images/Family1-Dad1.jpg\"}"
# </detect_for_similar>

# <similar_group>
https://csdx.blob.core.windows.net/resources/Face/Images/Family1-Daughter1.jpg
https://csdx.blob.core.windows.net/resources/Face/Images/Family1-Mom1.jpg
https://csdx.blob.core.windows.net/resources/Face/Images/Family1-Son1.jpg
https://csdx.blob.core.windows.net/resources/Face/Images/Family2-Lady1.jpg
https://csdx.blob.core.windows.net/resources/Face/Images/Family2-Man1.jpg
https://csdx.blob.core.windows.net/resources/Face/Images/Family3-Lady1.jpg
https://csdx.blob.core.windows.net/resources/Face/Images/Family3-Man1.jpg
# </similar_group>

# <similar_matcher>
https://csdx.blob.core.windows.net/resources/Face/Images/findsimilar.jpg
# </similar_matcher>

# <similar>
curl -v -X POST "https://westus.api.cognitive.microsoft.com/face/v1.0/findsimilars" -H "Content-Type: application/json" -H "Ocp-Apim-Subscription-Key: {subscription key}" --data-ascii "{body}" 
# </similar>

# <similar_body>
{
    "faceId": "",
    "faceIds": [],
    "maxNumOfCandidatesReturned": 10,
    "mode": "matchPerson"
}
# </similar_body>