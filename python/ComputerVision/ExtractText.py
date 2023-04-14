import os, pathlib, time
from io import BytesIO
from PIL import Image, ImageDraw

from azure.cognitiveservices.vision.computervision import ComputerVisionClient
from azure.cognitiveservices.vision.computervision.models import OperationStatusCodes
from msrest.authentication import CognitiveServicesCredentials

'''
This sample crops an existing image then uses the Computer Vision SDK 
to extract text from the cropped image. The extracted text is
returned along with bounding boxes. There is an optional feature to draw a rectangle
around the text you want to crop, then show the image.

Prerequisites:
1. Install the Computer Vision SDK:
pip install --upgrade azure-cognitiveservices-vision-computervision
2. Create and add an Images folder to your working directory. 
Add the images you want to crop into the Images folder. Once the script runs, cropped images will
automatically be added to the CroppedImages folder.
3. Make sure your images are either JPG or PNG. Change the file extension in the code if needed.

Computer Vision SDK: https://docs.microsoft.com/en-us/python/api/azure-cognitiveservices-vision-computervision/azure.cognitiveservices.vision.computervision?view=azure-python
Computer Vision API: https://westus.dev.cognitive.microsoft.com/docs/services/5cd27ec07268f6c679a3e641/operations/56f91f2e778daf14a499f21b
'''

'''
Authenticate Computer Vision client
'''
key = 'PASTE_YOUR_COMPUTER_VISION_KEY_HERE'
endpoint = 'PASTE_YOUR_COMPUTER_VISION_ENDPOINT_HERE'

client = ComputerVisionClient(endpoint=endpoint, credentials=CognitiveServicesCredentials(key))

'''
Load and crop images
'''
images_list = []
cropped_images_paths = []
cropped_images_list = []
working_directory = pathlib.Path (os.path.dirname(__file__))
os.makedirs(os.path.join (working_directory, 'CroppedImages'), exist_ok=True)

# Create an Image object from each image in a the Images folder.
for image_path in working_directory.glob('Images/*.jpg'):  # assume all images are jpg
    imageObject = Image.open(image_path)
    images_list.append(imageObject)
    cropped_images_paths.append(pathlib.Path (str(image_path).replace('Images', 'CroppedImages')))

# Optional, draw bounding box around desired line of text, show image
# original_image = Image.open('Images/coffee1.jpg').convert("RGBA")
# draw = ImageDraw.Draw(original_image)
# draw.rectangle(((110, 540), (425, 630)), outline="red")
# original_image.show()
 
# Crop each image in your list at the same place
for image in images_list:
    # Don't exceed your image height and width
    # w, h = image.size
    # print('Image width & height:', w, h)

    cropped = image.crop((110,540,425,630)) # edges: left, top, right, bottom
    cropped_images_list.append(cropped)

    # Optional, to display cropped image
    # cropped.show()

# Save the cropped images
# See:
# https://stackoverflow.com/a/51726283
for i in range(len(cropped_images_list)):
    # Convert cropped images back to PIL.JpegImagePlugin.JpegImageFile type
    b = BytesIO()
    cropped_images_list[i].save(b, format="jpeg")
    cropped_images_list[i] = Image.open(b)
    # Save cropped image to file.
    cropped_images_list[i].save(cropped_images_paths[i], format="jpeg")    
    b.close()
    
'''
Call the API
'''
# Use the recognize_printed_text_in_stream method to extract text from the cropped image
for cropped_image_path in cropped_images_paths:
    cropped_bytes = open(cropped_image_path, "rb")
    # Call API
    result = client.recognize_printed_text_in_stream(cropped_bytes)

# Print the extracted text, line by line
    for region in result.regions:
        for line in region.lines:
            for word in line.words:
                print("Extracted text:")
                print(word.text)
                print()
                print('Bounding box for each word:')
                print(word.bounding_box)
    print()
