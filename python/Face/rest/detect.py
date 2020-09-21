import json, os, requests

subscription_key = os.environ['FACE_SUBSCRIPTION_KEY']
assert subscription_key

face_api_url = os.environ['FACE_ENDPOINT'] + '/face/v1.0/detect'

image_url = 'https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/faces.jpg'

headers = {'Ocp-Apim-Subscription-Key': subscription_key}

params = {
	'detectionModel': 'detection_02',
    'returnFaceId': 'true'
}

response = requests.post(face_api_url, params=params,
                         headers=headers, json={"url": image_url})
print(json.dumps(response.json()))