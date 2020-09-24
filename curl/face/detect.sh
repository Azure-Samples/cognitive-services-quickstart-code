# <detection_model_2>
curl -H "Ocp-Apim-Subscription-Key: TODO_INSERT_YOUR_FACE_SUBSCRIPTION_KEY_HERE" "TODO_INSERT_YOUR_FACE_ENDPOINT_HERE/face/v1.0/detect?detectionModel=detection_02&returnFaceId=true&returnFaceLandmarks=false" -H "Content-Type: application/json" --data-ascii "{\"url\":\"https://upload.wikimedia.org/wikipedia/commons/c/c3/RH_Louise_Lillian_Gish.jpg\"}"
# </detection_model_2>

# <detection_model_1>
curl -H "Ocp-Apim-Subscription-Key: TODO_INSERT_YOUR_FACE_SUBSCRIPTION_KEY_HERE" "TODO_INSERT_YOUR_FACE_ENDPOINT_HERE/face/v1.0/detect?detectionModel=detection_01&returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses,emotion,hair,makeup,occlusion,accessories,blur,exposure,noise" -H "Content-Type: application/json" --data-ascii "{\"url\":\"https://upload.wikimedia.org/wikipedia/commons/c/c3/RH_Louise_Lillian_Gish.jpg\"}"
# </detection_model_1>