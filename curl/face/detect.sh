# <detection_model_2>
curl -H "Ocp-Apim-Subscription-Key: TODO_INSERT_YOUR_FACE_SUBSCRIPTION_KEY_HERE" "TODO_INSERT_YOUR_FACE_ENDPOINT_HERE/face/v1.0/detect?detectionModel=detection_02&returnFaceId=true&returnFaceLandmarks=false" -H "Content-Type: application/json" --data-ascii "{\"url\":\"https://upload.wikimedia.org/wikipedia/commons/c/c3/RH_Louise_Lillian_Gish.jpg\"}"
# </detection_model_2>

# <detection_model_1>
curl -H "Ocp-Apim-Subscription-Key: TODO_INSERT_YOUR_FACE_SUBSCRIPTION_KEY_HERE" "TODO_INSERT_YOUR_FACE_ENDPOINT_HERE/face/v1.0/detect?detectionModel=detection_01&returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses,emotion,hair,makeup,occlusion,accessories,blur,exposure,noise" -H "Content-Type: application/json" --data-ascii "{\"url\":\"https://upload.wikimedia.org/wikipedia/commons/c/c3/RH_Louise_Lillian_Gish.jpg\"}"
# </detection_model_1>

# <detection_model_2_with_stream>
curl --location --request POST 'TODO_INSERT_YOUR_FACE_ENDPOINT_HERE/face/v1.0/detect?detectionModel=detection_02&returnFaceId=true&returnFaceLandmarks=false' --header 'Ocp-Apim-Subscription-Key: TODO_INSERT_YOUR_FACE_SUBSCRIPTION_KEY_HERE' --header 'Content-Type: application/octet-stream' --data-binary 'TODO_INSERT_PATH_TO_IMAGE_FILE_HERE'
# </detection_model_2_with_stream>

# <detection_model_1_with_stream>
curl --location --request POST 'TODO_INSERT_YOUR_FACE_ENDPOINT_HERE/face/v1.0/detect?detectionModel=detection_01&returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses,emotion,hair,makeup,occlusion,accessories,blur,exposure,noise' --header 'Ocp-Apim-Subscription-Key: TODO_INSERT_YOUR_FACE_SUBSCRIPTION_KEY_HERE' --header 'Content-Type: application/octet-stream' --data-binary 'TODO_INSERT_PATH_TO_IMAGE_FILE_HERE'
# </detection_model_1_with_stream>
