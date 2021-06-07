from io import BytesIO
import os
from PIL import Image, ImageDraw
import requests
from urllib.parse import urlparse

from msrest.authentication import CognitiveServicesCredentials
from azure.cognitiveservices.vision.face import FaceClient

'''
Microsoft Azure Cognitive Services - Face

This quickstart uses URL images of a single person and one of many people to:
  - detect faces in an image
  - find similar faces in another image

Uses the general Cognitive Services key/endpoint. It's used when you want to 
combine many Cognitive Services with just one authentication key/endpoint. 
Services are not combined here, but could be potentially. 

Install the Face SDK from a command prompt or IDE terminal:
  pip install --upgrade azure-cognitiveservices-vision-face
'''

# Images for detection/comparison
single_face_image_url = 'https://www.biography.com/.image/t_share/MTQ1MzAyNzYzOTgxNTE0NTEz/john-f-kennedy---mini-biography.jpg'
multi_face_image_url = "http://www.historyplace.com/kennedy/president-family-portrait-closeup.jpg"

subscription_key = 'PASTE_YOUR_FACE_SUBSCRIPTION_KEY_HERE'
endpoint = 'PASTE_YOUR_FACE_ENDPOINT_HERE'

'''
AUTHENTICATE
Create a Face client
'''
face_client = FaceClient(endpoint, CognitiveServicesCredentials(subscription_key))

'''
FACE 
Detect faces in 2 images, then find a similar face between them.
'''
# Detect a face in an image that contains a single face
single_image_name = os.path.basename(single_face_image_url)
detected_faces = face_client.face.detect_with_url(url=single_face_image_url)
if not detected_faces:
	raise Exception('No face detected from image {}'.format(single_image_name))

# Display the detected face ID in the first single-faced image.
# Face IDs are used for comparison to faces (their IDs) detected in other images.
print('Detected face ID from', single_image_name, ':')
for x in detected_faces:
    print(' ', x.face_id)
    print()

# Select an ID for comparison to faces detected in a second image.
first_image_face_ID = detected_faces[0].face_id
print('Using the single face to search with ...')

# Detect the faces in an image that contains multiple faces
multi_image_name = os.path.basename(multi_face_image_url)
# Returns list[DetectedFace]
detected_faces2 = face_client.face.detect_with_url(url=multi_face_image_url)
if not detected_faces:
	raise Exception('No face detected from image {}.'.format(multi_image_name))

# Search through faces detected in group image for the single face from first image.
# First, create a list of the face IDs found in the second image.
second_image_face_IDs = list(map(lambda x: x.face_id, detected_faces2))
# Next, find similar face IDs like the one detected in the first image.
similar_faces = face_client.face.find_similar(face_id=first_image_face_ID, face_ids=second_image_face_IDs)
if not similar_faces[0]:
	print('No similar faces found in', multi_image_name, '.')

# Prepare to draw a rectangle around the similar face found
# First, download the query image (multi-faced image) from the url
downloaded_faces_image = requests.get(multi_face_image_url)
img = Image.open(BytesIO(downloaded_faces_image.content))

# Second, convert width & height of the face rectangle to a point in a rectangle
def getRectangle(face):
    rect = face.face_rectangle
    left = rect.left
    top = rect.top
    right = left + rect.width
    bottom = top + rect.height
    return ((left, top), (right, bottom))

# Print the details of the similar faces detected
print('Similar faces found in', multi_image_name + ':')
for face in similar_faces:
    matched_face_ID = face.face_id
	# The similar face IDs of the single face image and the group image do not need to match, they are only used for identification purposes in each image. The similar faces are matched using the Cognitive Services algorithm in find_similar().
    face_similar = next(x for x in detected_faces2 if x.face_id == matched_face_ID) # Look at each DetectedFace from group image
    if face_similar:
        print('  Face ID: ', first_image_face_ID)
        print('  Face rectangle:')
        print('    Left: ', str(face_similar.face_rectangle.left))
        print('    Top: ', str(face_similar.face_rectangle.top))
        print('    Width: ', str(face_similar.face_rectangle.width))
        print('    Height: ', str(face_similar.face_rectangle.height))

        # Now draw rectangles at the similar face locations in the group image
        print()
        print('Drawing rectangle around similar face(s)... see popup for results.')
        draw = ImageDraw.Draw(img)
        draw.rectangle(getRectangle(face_similar), outline='red')

        # Display the image in the users default image app/browser.
        img.show()
