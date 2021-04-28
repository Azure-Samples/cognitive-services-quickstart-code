from io import BytesIO
import os
from PIL import Image, ImageDraw
import requests

from azure.cognitiveservices.vision.computervision import ComputerVisionClient
from msrest.authentication import CognitiveServicesCredentials

'''
This example uses a local image and a remote URL image for analysis. 
It detects the objects in them, draws a bounding box around them, and then detects the tags in the image.

Install the Computer Vision SDK:
pip install --upgrade azure-cognitiveservices-vision-computervision

References: 
Computer Vision SDK: https://docs.microsoft.com/en-us/python/api/azure-cognitiveservices-vision-computervision/?view=azure-python
Computer Vision documentation: https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/
Computer Vision API: https://westus.dev.cognitive.microsoft.com/docs/services/5cd27ec07268f6c679a3e641/operations/56f91f2e778daf14a499f21b
'''

# Local and remote (URL) images
# Download the objects image from here (and place in your root folder): 
# https://github.com/Azure-Samples/cognitive-services-sample-data-files/tree/master/ComputerVision/Images
local_image = "objects.jpg"
remote_image = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/objects.jpg"
# Select visual feature type(s) you want to focus on when analyzing an image
image_features = ['objects', 'tags']

'''
Authenticate
Authenticates your credentials and creates a client.
'''
subscription_key = 'PASTE_YOUR_COMPUTER_VISION_SUBSCRIPTION_KEY_HERE'
endpoint = 'PASTE_YOUR_COMPUTER_VISION_ENDPOINT_HERE'

computervision_client = ComputerVisionClient(endpoint, CognitiveServicesCredentials(subscription_key))


# Draws a bounding box around an object found in image
def drawRectangle(object, draw):
    # Represent all sides of a box
    rect = object.rectangle
    left = rect.x
    top = rect.y
    right = left + rect.w
    bottom = top + rect.h
    coordinates = ((left, top), (right, bottom))
    draw.rectangle(coordinates, outline='red')


# Gets the objects detected in the image
def getObjects(results, draw):
    # Print results of detection with bounding boxes
    print("OBJECTS DETECTED:")
    if len(results.objects) == 0:
        print("No objects detected.")
    else:
        for object in results.objects:
            print("object at location {}, {}, {}, {}".format(
                object.rectangle.x, object.rectangle.x + object.rectangle.w,
                object.rectangle.y, object.rectangle.y + object.rectangle.h))
            drawRectangle(object, draw)
        print()
        print('Bounding boxes drawn around objects... see popup.')
    print()


# Prints the tag found from the image
def getTags(results):
    # Print results with confidence score
    print("TAGS: ")
    if (len(results.tags) == 0):
        print("No tags detected.")
    else:
        for tag in results.tags:
            print("'{}' with confidence {:.2f}%".format(
                tag.name, tag.confidence * 100))
    print()


'''
Analyze Image - local
This example detects different kinds of objects with bounding boxes and the tags from the image.
'''
print("===== Analyze Image - local =====")
print()
# Get local image with different objects in it
local_image_objects = open(local_image, "rb")
# Opens image to get PIL type of image, for drawing to
image_l = Image.open(local_image)
draw = ImageDraw.Draw(image_l)

# Call API with local image to analyze the image
results_local = computervision_client.analyze_image_in_stream(local_image_objects, image_features)

# Show bounding boxes around objects
getObjects(results_local, draw)
# Print tags from image
getTags(results_local)

# Display the image in the users default image browser.
image_l.show()
print()


'''
Detect Objects - remote
This example detects different kinds of objects with bounding boxes in a remote image.
'''
print("===== Analyze Image - remote =====")
print()
# Call API with URL to analyze the image
results_remote = computervision_client.analyze_image(remote_image, image_features)

# Download the image from the url, so can display it in popup/browser
object_image = requests.get(remote_image)
image_r = Image.open(BytesIO(object_image.content))
draw = ImageDraw.Draw(image_r)

# Show bounding boxes around objects
getObjects(results_remote, draw)
# Print tags from image
getTags(results_remote)

# Display the image in the users default image browser.
image_r.show()
