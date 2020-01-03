from io import BytesIO
import os
from PIL import Image, ImageDraw
import requests

from msrest.authentication import CognitiveServicesCredentials
from azure.cognitiveservices.search.websearch import WebSearchAPI
from azure.cognitiveservices.search.websearch.models import AnswerType, SafeSearch

'''
This quickstart performs a Bing Web Search with a celebrity name, returning similar images.

Uses the general Cognitive Services key/endpoint. It's used when you want to 
combine many Cognitive Services with just one authentication key/endpoint. 
Services are not combined here, but could be potentially. 

Install the Bing Web Search SDK from a command prompt or IDE terminal:
  python -m pip install azure-cognitiveservices-search-websearch
'''

# URL image, used as a reference only
# To detect the faces and find the celebrity in this photo, use the Computer Vision service (optional).
query_image_url = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/faces.jpg"

# The name of the celebrity you want to search for on the web.
celebrity_name = 'Bern Collaco'

# Add your Cognitive Services subscription key and endpoint to your environment variables.
subscription_key = os.environ['COGNITIVE_SERVICES_SUBSCRIPTION_KEY']
endpoint = os.environ['COGNITIVE_SERVICES_ENDPOINT']

'''
Authenticate a client. 
'''
web_search_client = WebSearchAPI(CognitiveServicesCredentials(
    subscription_key), endpoint + 'bing/v7.0')

'''
Bing Web Search
Using the name of a celebrity, search for other images of them and return the source URL and image.
This example uses the API calls:
  search()
'''
print()
print("===== Bing Web Search =====")
print("Searching the web for:", celebrity_name)
print()

web_data = web_search_client.web.search(
    query=celebrity_name,  # query search term
    response_filter=['Images'],  # return only images
    safe_search='Strict',  # filter the search to omit adult or racy content
    market='en-US'  # search only in this market
)

# If the search response contains images, get 5 of them.
if hasattr(web_data.images, 'value'):
    web_image_list = web_data.images.value  # returns list[ImageObject]
    # Get number of results
    print("Number of images found: {}".format(len(web_image_list)))
    # Display first 5 images found, if have response 200
    for i in range(5):
        # Download the query image from the url
        downloaded_found_image = requests.get(web_image_list[i].content_url)
        if downloaded_found_image.status_code == 200:
            img_found = Image.open(BytesIO(downloaded_found_image.content))
            img_found.show()
            # Print image URL
            print(web_image_list[i].content_url)
        else:
            print('Image number', i + 1, 'was not successful.')
else:
    print("Didn't find any images...")

