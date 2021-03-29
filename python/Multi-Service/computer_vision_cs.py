from io import BytesIO
import os
from PIL import Image, ImageDraw
import requests

from msrest.authentication import CognitiveServicesCredentials
from azure.cognitiveservices.vision.computervision import ComputerVisionClient

'''
Microsoft Azure Cognitive Services - Computer Vision 

This quickstart uses a URL image of people to:
  - detect faces
  - draw rectangles around them
  - detect age
  - describe the image
  - detect domain-specific content (celebrities)

Uses the general Cognitive Services key/endpoint. It's used when you want to 
combine many Cognitive Services with just one authentication key/endpoint. 
Services are not combined here, but could be potentially. 

Install the Computer Vision SDK from a command prompt or IDE terminal:
  pip install azure-cognitiveservices-vision-computervision
'''

# URL image
query_image_url = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/faces.jpg"

subscription_key = 'INSERT_YOUR_COMPUTER_VISION_SUBSCRIPTION_KEY_HERE'
endpoint = 'INSERT_YOUR_COMPUTER_VISION_ENDPOINT_HERE'

'''
Authenticate a client. 
'''
computer_vision_client = ComputerVisionClient(endpoint, CognitiveServicesCredentials(subscription_key))

'''
Computer Vision
This example uses the API calls:
  analyze_image() and describe_image()
'''
print()
print("===== Computer Vision =====")
# Select the visual feature(s) you want.
image_features = ["faces"]
# Call the API with detect faces feature, returns an ImageAnalysis which has a list[FaceDescription]
detected_faces = computer_vision_client.analyze_image(
    query_image_url, image_features)

# Print the results with age and bounding box
print("Face age and location in the image: ")
if (len(detected_faces.faces) == 0):
    print("No faces detected.")
else:
    for face in detected_faces.faces:
        print("Face of age {} at location {}, {}, {}, {}".format(
            face.age,
            face.face_rectangle.left, face.face_rectangle.top,
            face.face_rectangle.left + face.face_rectangle.width,
            face.face_rectangle.top + face.face_rectangle.height
        ))

# Draw rectangles at the face locations, display in popup
# First, convert width & height to a point in a rectangle


def getRectangle(face):
    rect = face.face_rectangle
    left = rect.left
    top = rect.top
    right = left + rect.width
    bottom = top + rect.height
    return ((left, top), (right, bottom))


# Download the query image from the url
downloaded_faces_image = requests.get(query_image_url)
img = Image.open(BytesIO(downloaded_faces_image.content))

# For each face returned use the face rectangle and draw a red box.
print()
print('Drawing rectangle around face(s)... see popup for results.')
draw = ImageDraw.Draw(img)
for face in detected_faces.faces:  # list[FaceDescription]
    draw.rectangle(getRectangle(face), outline='red')

# Display the image in the users default image browser.
img.show()

# Call API to describe image
description_result = computer_vision_client.describe_image(query_image_url)

# Get the captions (descriptions) from the response, with confidence level
print()
print("Description of image: ")
if (len(description_result.captions) == 0):
    print("No description detected.")
else:
    for caption in description_result.captions:
        print("'{}' with confidence {:.2f}%".format(
            caption.text, caption.confidence * 100))
print()

# Detect domain-specific content, celebrities, in image
# Call API with content type (celebrities) and URL
detect_domain_celebrity_result = computer_vision_client.analyze_image_by_domain(
    "celebrities", query_image_url)
# Print detection results with name
celebrities = detect_domain_celebrity_result.result["celebrities"]
celebrity_name = ''
print("Celebrities in the image:")
if len(detect_domain_celebrity_result.result["celebrities"]) == 0:
    print("No celebrities detected.")
else:
    for celeb in celebrities:
        celebrity_name = celeb["name"]
        print(celeb["name"])
