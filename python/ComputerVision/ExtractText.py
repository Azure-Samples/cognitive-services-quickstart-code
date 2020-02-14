# Python Imaging Library (PIL)
import glob
from io import BytesIO
import os
from PIL import Image, ImageDraw
import time

from azure.cognitiveservices.vision.computervision import ComputerVisionClient
from azure.cognitiveservices.vision.computervision.models import TextOperationStatusCodes
from msrest.authentication import CognitiveServicesCredentials

'''
This sample crops an existing image then uses the Computer Vision API 
Batch Read FIle to extract text from the cropped image. The extracted text is
returned along with bounding boxes. There is an optional feature to draw a rectangle
around the text you want to crop, then show the image.

Computer Vision SDK: https://docs.microsoft.com/en-us/python/api/azure-cognitiveservices-vision-computervision/azure.cognitiveservices.vision.computervision?view=azure-python
Computer Vision API: https://westus.dev.cognitive.microsoft.com/docs/services/5cd27ec07268f6c679a3e641/operations/56f91f2e778daf14a499f21b
'''

'''
Authenticate Computer Vision client
'''
key = os.environ['COMPUTER_VISION_SUBSCRIPTION_KEY']
endpoint = os.environ['COMPUTER_VISION_ENDPOINT']

client = ComputerVisionClient(endpoint=endpoint, credentials=CognitiveServicesCredentials(key))

'''
Load and crop images
'''
images_list = []
cropped_images_path = []
working_directory = os.path.dirname(__file__)

# Create an Image object from each image in a directory
for filename in glob.glob('Images\*.jpg'):  # assuming all images are jpg
    imageObject = Image.open(filename)
    images_list.append(imageObject)
    path = os.path.join(working_directory, filename.replace('Images\\', 'CroppedImages\\'))
    cropped_images_path.append(path)

# Optional, draw bounding box around desired line of text, show image
# original_image = Image.open('Images\coffee1.jpg').convert("RGBA")
# draw = ImageDraw.Draw(original_image)
# draw.rectangle(((110, 540), (425, 630)), outline="red")
# original_image.show()
 
# Crop each image in your list at the same place
cropped_images_list = []
for image in images_list:
    # Don't exceed your image height and width
    # w, h = image.size
    # print('Image width & height:', w, h)

    cropped = image.crop((110,540,425,630)) # edges: left, top, right, bottom
    cropped_images_list.append(cropped)

    # Optional, to display cropped image
    # cropped.show()

# Save the cropped images
for i in range(len(cropped_images_list)):
    # Convert cropped images back to PIL.JpegImagePlugin.JpegImageFile type
    b = BytesIO()
    cropped_images_list[i].save(b, format="jpeg")

b.close()

'''
Call the API
'''
# Use the Batch Read File API to extract text from the cropped image
for cropped_path in cropped_images_path:
    cropped_bytes = open(cropped_path, "rb")
    # Call API
    results = client.batch_read_file_in_stream(cropped_bytes, raw=True)
    # To get the read results, we need the operation ID from operation location
    operation_location = results.headers["Operation-Location"]
    # Operation ID is at the end of the URL
    operation_id = operation_location.split("/")[-1]

    # Wait for the "get_read_operation_result()" to retrieve the results
    while True:
        get_printed_text_results = client.get_read_operation_result(operation_id)
        if get_printed_text_results.status not in ['NotStarted', 'Running']:
            break
        time.sleep(1)

'''
Print results
'''
# Print the extracted text, line by line
if get_printed_text_results.status == TextOperationStatusCodes.succeeded:
    for text_result in get_printed_text_results.recognition_results:
        for line in text_result.lines:
            print("Extracted text:")
            print(line.text)
            print()
            print('Bounding box for each line:')
            print(line.bounding_box)
            # Optional, print each word of each line
            # print(line.words)
print()
