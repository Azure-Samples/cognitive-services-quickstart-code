from io import BytesIO
import os
from PIL import Image, ImageDraw
import requests

from azure.cognitiveservices.vision.face import FaceClient
from azure.cognitiveservices.vision.face.models import FaceAttributeType
from msrest.authentication import CognitiveServicesCredentials

'''
This example detects faces from 2 different images, then returns information about their facial features.
The features can be set to a variety of properties, see the SDK for all available options.    

Prequisites:
Install the Face SDK: pip install --upgrade azure-cognitiveservices-vision-face

References:
Face SDK: https://docs.microsoft.com/en-us/python/api/azure-cognitiveservices-vision-face/?view=azure-python
Face documentation: https://docs.microsoft.com/en-us/azure/cognitive-services/face/
Face API: https://docs.microsoft.com/en-us/azure/cognitive-services/face/apireference
'''

'''
Authenticate the Face service
'''
# Set the FACE_SUBSCRIPTION_KEY environment variable with your key as the value.
# This key will serve all examples in this document.
KEY = os.environ['FACE_SUBSCRIPTION_KEY']

# Set the FACE_ENDPOINT environment variable with the endpoint from your Face service in Azure.
# This endpoint will be used in all examples in this quickstart.
ENDPOINT = os.environ['FACE_ENDPOINT']

# Create an authenticated FaceClient.
face_client = FaceClient(ENDPOINT, CognitiveServicesCredentials(KEY))


'''
Detect face(s) with attributes in a URL image
'''
# Image of face(s)
face1_url = 'https://upload.wikimedia.org/wikipedia/commons/f/f2/Kristofer_Hivju_%28Cropped%2C_2015%29.jpg'
face1_name = os.path.basename(face1_url)
face2_url = 'https://upload.wikimedia.org/wikipedia/commons/thumb/a/ae/The_famous_mustache_and_goatee.jpg/220px-The_famous_mustache_and_goatee.jpg'
face2_name = os.path.basename(face2_url)

# List of url images
url_images = [face1_url, face2_url]

# Attributes you want returned with the API call, a list of FaceAttributeType enum (string format)
face_attributes = ['age', 'gender', 'headPose', 'smile', 'facialHair', 'glasses', 'emotion']

# Detect a face with attributes, returns a list[DetectedFace]
for image in url_images:
    detected_faces = face_client.face.detect_with_url(url=image, return_face_attributes=face_attributes)
    if not detected_faces:
        raise Exception(
            'No face detected from image {}'.format(os.path.basename(image)))

    '''
    Display the detected face with attributes and bounding box
    '''
    # Face IDs are used for comparison to faces (their IDs) detected in other images.
    for face in detected_faces:
        print()
        print('Detected face ID from', os.path.basename(image), ':')
        # ID of detected face
        print(face.face_id)
        # Show all facial attributes from the results
        print()
        print('Facial attributes detected:')
        print('Age:', face.face_attributes.age)
        print('Gender:', face.face_attributes.gender)
        print('Head pose:', face.face_attributes.head_pose)
        print('Smile:', face.face_attributes.smile)
        print('Facial hair:', face.face_attributes.facial_hair)
        print('Glasses:', face.face_attributes.glasses)
        print('Emotion:', face.face_attributes.emotion)
        print()

    # Convert width height to a point in a rectangle
    def getRectangle(faceDictionary):
        rect = faceDictionary.face_rectangle
        left = rect.left
        top = rect.top
        right = left + rect.width
        bottom = top + rect.height

        return ((left, top), (right, bottom))

    # Download the image from the url, so can display it in popup/browser
    response = requests.get(image)
    img = Image.open(BytesIO(response.content))

    # For each face returned use the face rectangle and draw a red box.
    print('Drawing rectangle around face... see popup for results.')
    print()
    draw = ImageDraw.Draw(img)
    for face in detected_faces:
        draw.rectangle(getRectangle(face), outline='red')

    # Display the image in the users default image browser.
    img.show()
