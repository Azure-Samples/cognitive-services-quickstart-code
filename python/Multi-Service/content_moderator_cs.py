import os
from pprint import pprint

from msrest.authentication import CognitiveServicesCredentials
from azure.cognitiveservices.vision.contentmoderator import ContentModeratorClient
from azure.cognitiveservices.vision.contentmoderator.models import ( Evaluate, OCR, FoundFaces )

'''
This quickstart uses Content Moderator to moderate a list of images.

Uses the general Cognitive Services key/endpoint. It's used when you want to 
combine many Cognitive Services with just one authentication key/endpoint. 
Services are not combined here, but could be potentially. 

Install the Content Moderator SDK from a command prompt or IDE terminal:
  pip install --upgrade azure-cognitiveservices-vision-contentmoderator

The Content Moderator SDK: 
https://docs.microsoft.com/en-us/python/api/azure-cognitiveservices-vision-contentmoderator/?view=azure-python
'''

subscription_key = "PASTE_YOUR_CONTENT_MODERATOR_SUBSCRIPTION_KEY_HERE"
endpoint = "PASTE_YOUR_CONTENT_MODERATOR_ENDPOINT_HERE"

# List of URL images used to moderate.
IMAGE_LIST = [
    "https://moderatorsampleimages.blob.core.windows.net/samples/sample2.jpg",
    "https://moderatorsampleimages.blob.core.windows.net/samples/sample5.png"
]

'''
AUTHENTICATE
Create a Content Moderator client.
'''
client = ContentModeratorClient(
    endpoint=endpoint,
    credentials=CognitiveServicesCredentials(subscription_key)
)

'''
CONTENT MODERATOR
This quickstart moderates an image, then text and faces within the image.
'''
print('IMAGE MODERATION')
print()

# Image moderation, using image at [0]
print("Evaluate the image '{}' for adult and racy content:".format(os.path.basename(IMAGE_LIST[0])))
mod_image = client.image_moderation.evaluate_url_input(content_type="application/json", cache_image=True,
                                                       data_representation="URL", value=IMAGE_LIST[0])
assert isinstance(mod_image, Evaluate)
# Format for printing
mod_results = list(mod_image.as_dict().items())
for result in mod_results:
    print(result)

# Moderating text in an image, using image at [0]
print("\nDetect, extract, and moderate text for image {}:".format(
    os.path.basename(IMAGE_LIST[0])))
mod_image = client.image_moderation.ocr_url_input(language="eng", content_type="application/json",
                                                  data_representation="URL", value=IMAGE_LIST[0], cache_image=True)
assert isinstance(mod_image, OCR)
# Format for printing
mod_results = list(mod_image.as_dict().items())
for result in mod_results:
    print(result)

# Moderating faces in an image, using image at [1]
print("\nDetect faces and moderate for image {}:".format(
    os.path.basename(IMAGE_LIST[1])))
mod_image = client.image_moderation.find_faces_url_input(content_type="application/json", cache_image=True,
                                                         data_representation="URL", value=IMAGE_LIST[1])
assert isinstance(mod_image, FoundFaces)
# Format for printing
mod_results = list(mod_image.as_dict().items())
for result in mod_results:
    print(result)
print()
