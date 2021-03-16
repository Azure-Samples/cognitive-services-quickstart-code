import json, os, requests

subscription_key = "PASTE_YOUR_FACE_SUBSCRIPTION_KEY_HERE"

face_api_url = "PASTE_YOUR_FACE_ENDPOINT_HERE" + '/face/v1.0/detect'

image_url = 'https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/faces.jpg'

headers = {'Ocp-Apim-Subscription-Key': subscription_key}

params = {
    'detectionModel': 'detection_02',
    'returnFaceId': 'true'
}

response = requests.post(face_api_url, params=params,
                         headers=headers, json={"url": image_url})
print(json.dumps(response.json()))
