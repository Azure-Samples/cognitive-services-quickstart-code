# <snippet_imports>
from azure.cognitiveservices.vision.computervision import ComputerVisionClient
from azure.cognitiveservices.vision.computervision.models import OperationStatusCodes
from azure.cognitiveservices.vision.computervision.models import VisualFeatureTypes
from msrest.authentication import CognitiveServicesCredentials

from array import array
import os
from PIL import Image
import sys
import time
# </snippet_imports>

'''
Computer Vision Quickstart for Microsoft Azure Cognitive Services. 
Uses local and remote images in each example.

Prerequisites:
    - Install the Computer Vision SDK:
      pip install --upgrade azure-cognitiveservices-vision-computervision
    - Create folder and collect images: 
      Create a folder called "resources" in your root folder.
      Go to this website to download images:
        https://github.com/Azure-Samples/cognitive-services-sample-data-files/tree/master/ComputerVision/Images
      Add the following 7 images (or use your own) to your "resources" folder: 
        faces.jpg, gray-shirt-logo.jpg, handwritten_text.jpg, landmark.jpg, 
        objects.jpg, printed_text.jpg and type-image.jpg

Run the entire file to demonstrate the following examples:
    - Describe Image
    - Categorize Image
    - Tag Image
    - Detect Faces
    - Detect Adult or Racy Content
    - Detect Color Scheme
    - Detect Domain-specific Content (celebrities/landmarks)
    - Detect Image Types (clip art/line drawing)
    - Detect Objects
    - Detect Brands
    - Generate Thumbnail
    - Batch Read File (recognize both handwritten and printed text) 
    - Recognize Printed Text with OCR

References:
    - SDK: https://docs.microsoft.com/en-us/python/api/azure-cognitiveservices-vision-computervision/azure.cognitiveservices.vision.computervision?view=azure-python
    - Documentaion: https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/index
    - API: https://westus.dev.cognitive.microsoft.com/docs/services/5adf991815e1060e6355ad44/operations/56f91f2e778daf14a499e1fa
'''

'''
Quickstart variables
These variables are shared by several examples
'''
# Images used for the examples: Describe an image, Categorize an image, Tag an image, 
# Detect faces, Detect adult or racy content, Detect the color scheme, 
# Detect domain-specific content, Detect image types, Detect objects
local_image_path = "resources\\faces.jpg"
# <snippet_remoteimage>
remote_image_url = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/landmark.jpg"
# </snippet_remoteimage>
'''
END - Quickstart variables
'''

'''
Authenticate
Authenticates your credentials and creates a client.
'''
# <snippet_vars>
subscription_key = "<your subscription key>"
endpoint = "<your API endpoint>"

# </snippet_vars>

# <snippet_client>
computervision_client = ComputerVisionClient(endpoint, CognitiveServicesCredentials(subscription_key))
# </snippet_client>
'''
END - Authenticate
'''

'''
Describe an Image - local
This example describes the contents of an image with the confidence score.
'''
print("===== Describe an Image - local =====")
# Open local image file
local_image = open(local_image_path, "rb")

# Call API
description_result = computervision_client.describe_image_in_stream(local_image)

# Get the captions (descriptions) from the response, with confidence level
print("Description of local image: ")
if (len(description_result.captions) == 0):
    print("No description detected.")
else:
    for caption in description_result.captions:
        print("'{}' with confidence {:.2f}%".format(caption.text, caption.confidence * 100))
print()
'''
END - Describe an Image - local
'''

# <snippet_describe>
'''
Describe an Image - remote
This example describes the contents of an image with the confidence score.
'''
print("===== Describe an image - remote =====")
# Call API
description_results = computervision_client.describe_image(remote_image_url )

# Get the captions (descriptions) from the response, with confidence level
print("Description of remote image: ")
if (len(description_results.captions) == 0):
    print("No description detected.")
else:
    for caption in description_results.captions:
        print("'{}' with confidence {:.2f}%".format(caption.text, caption.confidence * 100))
# </snippet_describe>
print()
'''
END - Describe an Image - remote
'''

'''
Categorize an Image -  local
This example extracts categories from a local image with a confidence score
'''
print("===== Categorize an Image - local =====")
# Open local image file
local_image = open(local_image_path, "rb")
# Select visual feature type(s)
local_image_features = ["categories"]
# Call API
categorize_results_local = computervision_client.analyze_image_in_stream(local_image, local_image_features)

# Print category results with confidence score
print("Categories from local image: ")
if (len(categorize_results_local.categories) == 0):
    print("No categories detected.")
else:
    for category in categorize_results_local.categories:
        print("'{}' with confidence {:.2f}%".format(category.name, category.score * 100))
print()
'''
END - Categorize an Image - local
'''

# <snippet_categorize>
'''
Categorize an Image - remote
This example extracts (general) categories from a remote image with a confidence score.
'''
print("===== Categorize an image - remote =====")
# Select the visual feature(s) you want.
remote_image_features = ["categories"]
# Call API with URL and features
categorize_results_remote = computervision_client.analyze_image(remote_image_url , remote_image_features)

# Print results with confidence score
print("Categories from remote image: ")
if (len(categorize_results_remote.categories) == 0):
    print("No categories detected.")
else:
    for category in categorize_results_remote.categories:
        print("'{}' with confidence {:.2f}%".format(category.name, category.score * 100))
# </snippet_categorize>
print()
'''
 END - Categorize an Image - remote
'''

'''
Tag an Image - local
This example returns a tag (key word) for each thing in the image.
'''
print("===== Tag an Image - local =====")
# Open local image file
local_image = open(local_image_path, "rb")
# Call API local image
tags_result_local = computervision_client.tag_image_in_stream(local_image)

# Print results with confidence score
print("Tags in the local image: ")
if (len(tags_result_local.tags) == 0):
    print("No tags detected.")
else:
    for tag in tags_result_local.tags:
        print("'{}' with confidence {:.2f}%".format(tag.name, tag.confidence * 100))
print()
'''
END - Tag an Image - local
'''

# <snippet_tags>
'''
Tag an Image - remote
This example returns a tag (key word) for each thing in the image.
'''
print("===== Tag an image - remote =====")
# Call API with remote image
tags_result_remote = computervision_client.tag_image(remote_image_url )

# Print results with confidence score
print("Tags in the remote image: ")
if (len(tags_result_remote.tags) == 0):
    print("No tags detected.")
else:
    for tag in tags_result_remote.tags:
        print("'{}' with confidence {:.2f}%".format(tag.name, tag.confidence * 100))
# </snippet_tags>
print()
'''
END - Tag an Image - remote
'''

'''
Detect Faces - local
This example detects faces in a local image, gets their gender and age, 
and marks them with a bounding box.
'''
print("===== Detect Faces - local =====")
# Open local image
local_image = open(local_image_path, "rb")
# Select visual features(s) you want
local_image_features = ["faces"]
# Call API with local image and features
detect_faces_results_local = computervision_client.analyze_image_in_stream(local_image, local_image_features)

# Print results with confidence score
print("Faces in the local image: ")
if (len(detect_faces_results_local.faces) == 0):
    print("No faces detected.")
else:
    for face in detect_faces_results_local.faces:
        print("'{}' of age {} at location {}, {}, {}, {}".format(face.gender, face.age, \
        face.face_rectangle.left, face.face_rectangle.top, \
        face.face_rectangle.left + face.face_rectangle.width, \
        face.face_rectangle.top + face.face_rectangle.height))
print()
'''
END - Detect Faces - local
'''

# <snippet_faces>
'''
Detect Faces - remote
This example detects faces in a remote image, gets their gender and age, 
and marks them with a bounding box.
'''
print("===== Detect Faces - remote =====")
# Get an image with faces
remote_image_url_faces = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/faces.jpg"
# Select the visual feature(s) you want.
remote_image_features = ["faces"]
# Call the API with remote URL and features
detect_faces_results_remote = computervision_client.analyze_image(remote_image_url_faces, remote_image_features)

# Print the results with gender, age, and bounding box
print("Faces in the remote image: ")
if (len(detect_faces_results_remote.faces) == 0):
    print("No faces detected.")
else:
    for face in detect_faces_results_remote.faces:
        print("'{}' of age {} at location {}, {}, {}, {}".format(face.gender, face.age, \
        face.face_rectangle.left, face.face_rectangle.top, \
        face.face_rectangle.left + face.face_rectangle.width, \
        face.face_rectangle.top + face.face_rectangle.height))
# </snippet_faces>
print()
'''
END - Detect Faces - remote
'''

'''
Detect Adult or Racy Content - local
This example detects adult or racy content in a local image, then prints the adult/racy score.
The score is ranged 0.0 - 1.0 with smaller numbers indicating negative results.
'''
print("===== Detect Adult or Racy Content - local =====")
# Open local file
local_image = open(local_image_path, "rb")
# Select visual features you want
local_image_features = ["adult"]
# Call API with local image and features
detect_adult_results_local = computervision_client.analyze_image_in_stream(local_image, local_image_features)

# Print results with adult/racy score
print("Analyzing local image for adult or racy content ... ")
print("Is adult content: {} with confidence {:.2f}".format(detect_adult_results_local .adult.is_adult_content, detect_adult_results_local .adult.adult_score * 100))
print("Has racy content: {} with confidence {:.2f}".format(detect_adult_results_local .adult.is_racy_content, detect_adult_results_local .adult.racy_score * 100))
print()
'''
END - Detect Adult or Racy Content - local
'''

# <snippet_adult>
'''
Detect Adult or Racy Content - remote
This example detects adult or racy content in a remote image, then prints the adult/racy score.
The score is ranged 0.0 - 1.0 with smaller numbers indicating negative results.
'''
print("===== Detect Adult or Racy Content - remote =====")
# Select the visual feature(s) you want
remote_image_features = ["adult"]
# Call API with URL and features
detect_adult_results_remote = computervision_client.analyze_image(remote_image_url, remote_image_features)

# Print results with adult/racy score
print("Analyzing remote image for adult or racy content ... ")
print("Is adult content: {} with confidence {:.2f}".format(detect_adult_results_remote.adult.is_adult_content, detect_adult_results_remote.adult.adult_score * 100))
print("Has racy content: {} with confidence {:.2f}".format(detect_adult_results_remote.adult.is_racy_content, detect_adult_results_remote.adult.racy_score * 100))
# </snippet_adult>
print()
'''
END - Detect Adult or Racy Content - remote
'''

'''
Detect Color - local
This example detects the different aspects of its color scheme in a local image.
'''
print("===== Detect Color - local =====")
# Open local image
local_image = open(local_image_path, "rb")
# Select visual feature(s) you want
local_image_features = ["color"]
# Call API with local image and features
detect_color_results_local = computervision_client.analyze_image_in_stream(local_image, local_image_features)

# Print results of the color scheme detected
print("Getting color scheme of the local image: ")
print("Is black and white: {}".format(detect_color_results_local.color.is_bw_img))
print("Accent color: {}".format(detect_color_results_local.color.accent_color))
print("Dominant background color: {}".format(detect_color_results_local.color.dominant_color_background))
print("Dominant foreground color: {}".format(detect_color_results_local.color.dominant_color_foreground))
print("Dominant colors: {}".format(detect_color_results_local.color.dominant_colors))
print()
'''
END - Detect Color - local
'''

# <snippet_color>
'''
Detect Color - remote
This example detects the different aspects of its color scheme in a remote image.
'''
print("===== Detect Color - remote =====")
# Select the feature(s) you want
remote_image_features = ["color"]
# Call API with URL and features
detect_color_results_remote = computervision_client.analyze_image(remote_image_url, remote_image_features)

# Print results of color scheme
print("Getting color scheme of the remote image: ")
print("Is black and white: {}".format(detect_color_results_remote.color.is_bw_img))
print("Accent color: {}".format(detect_color_results_remote.color.accent_color))
print("Dominant background color: {}".format(detect_color_results_remote.color.dominant_color_background))
print("Dominant foreground color: {}".format(detect_color_results_remote.color.dominant_color_foreground))
print("Dominant colors: {}".format(detect_color_results_remote.color.dominant_colors))
# </snippet_color>
print()
'''
END - Detect Color - remote
'''

'''
Detect Domain-specific Content - local
This example detects celebrites and landmarks in local images.
'''
print("===== Detect Domain-specific Content - local =====")
# Open local image file containing a celebtriy
local_image = open(local_image_path, "rb")
# Call API with the type of content (celebrities) and local image
detect_domain_results_celebs_local = computervision_client.analyze_image_by_domain_in_stream("celebrities", local_image)

# Print which celebrities (if any) were detected
print("Celebrities in the local image:")
if len(detect_domain_results_celebs_local.result["celebrities"]) == 0:
    print("No celebrities detected.")
else:
    for celeb in detect_domain_results_celebs_local.result["celebrities"]:
        print(celeb["name"])

# Open local image file containing a landmark
local_image_path_landmark = "resources\\landmark.jpg"
local_image_landmark = open(local_image_path_landmark, "rb")
# Call API with type of content (landmark) and local image
detect_domain_results_landmark_local = computervision_client.analyze_image_by_domain_in_stream("landmarks", local_image_landmark)
print()

# Print results of landmark detected
print("Landmarks in the local image:")
if len(detect_domain_results_landmark_local.result["landmarks"]) == 0:
    print("No landmarks detected.")
else:
    for landmark in detect_domain_results_landmark_local.result["landmarks"]:
        print(landmark["name"])
print()
'''
END - Detect Domain-specific Content - local
'''

# <snippet_celebs>
'''
Detect Domain-specific Content - remote
This example detects celebrites and landmarks in remote images.
'''
print("===== Detect Domain-specific Content - remote =====")
# URL of one or more celebrities
remote_image_url_celebs = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/faces.jpg"
# Call API with content type (celebrities) and URL
detect_domain_results_celebs_remote = computervision_client.analyze_image_by_domain("celebrities", remote_image_url_celebs)

# Print detection results with name
print("Celebrities in the remote image:")
if len(detect_domain_results_celebs_remote.result["celebrities"]) == 0:
    print("No celebrities detected.")
else:
    for celeb in detect_domain_results_celebs_remote.result["celebrities"]:
        print(celeb["name"])
# </snippet_celebs>

# <snippet_landmarks>
# Call API with content type (landmarks) and URL
detect_domain_results_landmarks = computervision_client.analyze_image_by_domain("landmarks", remote_image_url)
print()

print("Landmarks in the remote image:")
if len(detect_domain_results_landmarks.result["landmarks"]) == 0:
    print("No landmarks detected.")
else:
    for landmark in detect_domain_results_landmarks.result["landmarks"]:
        print(landmark["name"])
# </snippet_landmarks>
print()
'''
END - Detect Domain-specific Content - remote
'''

'''
Detect Image Types - local
This example detects an image's type (clip art/line drawing).
'''
print("===== Detect Image Types - local =====")
# Open local image
local_image_path_type = "resources\\type-image.jpg"
local_image_type = open(local_image_path_type, "rb")
# Select visual feature(s) you want
local_image_features = [VisualFeatureTypes.image_type]
# Call API with local image and features
detect_type_results_local = computervision_client.analyze_image_in_stream(local_image_type, local_image_features)

# Print type results with degree of accuracy
print("Type of local image:")
if detect_type_results_local.image_type.clip_art_type == 0:
    print("Image is not clip art.")
elif detect_type_results_local.image_type.line_drawing_type == 1:
    print("Image is ambiguously clip art.")
elif detect_type_results_local.image_type.line_drawing_type == 2:
    print("Image is normal clip art.")
else:
    print("Image is good clip art.")

if detect_type_results_local.image_type.line_drawing_type == 0:
    print("Image is not a line drawing.")
else:
    print("Image is a line drawing")
print()
'''
END - Detect Image Types - local
'''

# <snippet_type>
'''
Detect Image Types - remote
This example detects an image's type (clip art/line drawing).
'''
print("===== Detect Image Types - remote =====")
# Get URL of an image with a type
remote_image_url_type = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/type-image.jpg"
# Select visual feature(s) you want
remote_image_features = [VisualFeatureTypes.image_type]
# Call API with URL and features
detect_type_results_remote = computervision_client.analyze_image(remote_image_url_type, remote_image_features)

# Prints type results with degree of accuracy
print("Type of remote image:")
if detect_type_results_remote.image_type.clip_art_type == 0:
    print("Image is not clip art.")
elif detect_type_results_remote.image_type.line_drawing_type == 1:
    print("Image is ambiguously clip art.")
elif detect_type_results_remote.image_type.line_drawing_type == 2:
    print("Image is normal clip art.")
else:
    print("Image is good clip art.")

if detect_type_results_remote.image_type.line_drawing_type == 0:
    print("Image is not a line drawing.")
else:
    print("Image is a line drawing")
# </snippet_type>
print()
'''
END - Detect Image Types - remote
'''

'''
Detect Objects - local
This example detects different kinds of objects with bounding boxes in a local image.
'''
print("===== Detect Objects - local =====")
# Get local image with different objects in it
local_image_path_objects = "resources\\objects.jpg"
local_image_objects = open(local_image_path_objects, "rb")
# Call API with local image
detect_objects_results_local = computervision_client.detect_objects_in_stream(local_image_objects)

# Print results of detection with bounding boxes
print("Detecting objects in local image:")
if len(detect_objects_results_local.objects) == 0:
    print("No objects detected.")
else:
    for object in detect_objects_results_local.objects:
        print("object at location {}, {}, {}, {}".format( \
        object.rectangle.x, object.rectangle.x + object.rectangle.w, \
        object.rectangle.y, object.rectangle.y + object.rectangle.h))
print()
'''
END - Detect Objects - local
'''

# <snippet_objects>
'''
Detect Objects - remote
This example detects different kinds of objects with bounding boxes in a remote image.
'''
print("===== Detect Objects - remote =====")
# Get URL image with different objects
remote_image_url_objects = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/objects.jpg"
# Call API with URL
detect_objects_results_remote = computervision_client.detect_objects(remote_image_url_objects)

# Print detected objects results with bounding boxes
print("Detecting objects in remote image:")
if len(detect_objects_results_remote.objects) == 0:
    print("No objects detected.")
else:
    for object in detect_objects_results_remote.objects:
        print("object at location {}, {}, {}, {}".format( \
        object.rectangle.x, object.rectangle.x + object.rectangle.w, \
        object.rectangle.y, object.rectangle.y + object.rectangle.h))
# </snippet_objects>
print()
'''
END - Detect Objects - remote
'''

'''
Detect Brands - local
This example detects common brands like logos and puts a bounding box around them.
'''
print("===== Detect Brands - local =====")
# Open image file
local_image_path_shirt = "resources\\gray-shirt-logo.jpg"
local_image_shirt = open(local_image_path_shirt, "rb")
# Select the visual feature(s) you want
local_image_features = ["brands"]
# Call API with image and features
detect_brands_results_local = computervision_client.analyze_image_in_stream(local_image_shirt, local_image_features)

# Print detection results with bounding box and confidence score
print("Detecting brands in local image: ")
if len(detect_brands_results_local.brands) == 0:
    print("No brands detected.")
else:
    for brand in detect_brands_results_local.brands:
        print("'{}' brand detected with confidence {:.1f}% at location {}, {}, {}, {}".format( \
        brand.name, brand.confidence * 100, brand.rectangle.x, brand.rectangle.x + brand.rectangle.w, \
        brand.rectangle.y, brand.rectangle.y + brand.rectangle.h))
print()
'''
END - Detect brands - local
'''

# <snippet_brands>
'''
Detect Brands - remote
This example detects common brands like logos and puts a bounding box around them.
'''
print("===== Detect Brands - remote =====")
# Get a URL with a brand logo
remote_image_url = "https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/images/gray-shirt-logo.jpg"
# Select the visual feature(s) you want
remote_image_features = ["brands"]
# Call API with URL and features
detect_brands_results_remote = computervision_client.analyze_image(remote_image_url, remote_image_features)

print("Detecting brands in remote image: ")
if len(detect_brands_results_remote.brands) == 0:
    print("No brands detected.")
else:
    for brand in detect_brands_results_remote.brands:
        print("'{}' brand detected with confidence {:.1f}% at location {}, {}, {}, {}".format( \
        brand.name, brand.confidence * 100, brand.rectangle.x, brand.rectangle.x + brand.rectangle.w, \
        brand.rectangle.y, brand.rectangle.y + brand.rectangle.h))
# </snippet_brands>
print()
'''
END - Detect Brands - remote
'''

'''
Generate Thumbnail
This example creates a thumbnail from both a local and URL image.
'''
print("===== Generate Thumbnail =====")

# Generate a thumbnail from a local image
local_image_path_thumb = "resources\\objects.jpg"
local_image_thumb = open(local_image_path_objects, "rb")

print("Generating thumbnail from a local image...")
# Call the API with a local image, set the width/height if desired (pixels)
# Returns a Generator object, a thumbnail image binary (list).
thumb_local = computervision_client.generate_thumbnail_in_stream(100, 100, local_image_thumb, True)

# Write the image binary to file
with open("thumb_local.png", "wb") as f:
    for chunk in thumb_local:
        f.write(chunk)

# Uncomment/use this if you are writing many images as thumbnails from a list
# for i, image in enumerate(thumb_local, start=0):
#      with open('thumb_{0}.jpg'.format(i), 'wb') as f:
#         f.write(image)

print("Thumbnail saved to local folder.")
print()

# Generate a thumbnail from a URL image
# URL of faces
remote_image_url_thumb = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/faces.jpg"

print("Generating thumbnail from a URL image...")
# Returns a Generator object, a thumbnail image binary (list).
thumb_remote = computervision_client.generate_thumbnail(
    100, 100, remote_image_url_thumb, True)

# Write the image binary to file
with open("thumb_remote.png", "wb") as f:
    for chunk in thumb_remote:
        f.write(chunk)

print("Thumbnail saved to local folder.")

# Uncomment/use this if you are writing many images as thumbnails from a list
# for i, image in enumerate(thumb_remote, start=0):
#      with open('thumb_{0}.jpg'.format(i), 'wb') as f:
#         f.write(image)

print()
'''
END - Generate Thumbnail
'''

'''
Batch Read File, recognize handwritten text - local
This example extracts text from a handwritten local image, then prints results.
This API call can also recognize remote image text (shown in next example, Batch Read File - remote).
'''
print("===== Batch Read File - local =====")
# Get image of handwriting
local_image_handwritten_path = "resources\\handwritten_text.jpg"
# Open the image
local_image_handwritten = open(local_image_handwritten_path, "rb")

# Call API with image and raw response (allows you to get the operation location)
read_results = computervision_client.read_in_stream(local_image_handwritten, raw=True)
# Get the operation location (URL with ID as last appendage)
operation_location_local = read_results.headers["Operation-Location"]
# Take the ID off and use to get results
operation_id_local = operation_location_local.split("/")[-1]

# Call the "GET" API and wait for the retrieval of the results
while True:
    read_result = computervision_client.get_read_result(operation_id_local)
    if read_result.status not in ['notStarted', 'running']:
        break
    time.sleep(1)

# Print results, line by line
if read_result.status == OperationStatusCodes.succeeded:
    for text_result in read_result.analyze_result.read_results:
        for line in text_result.lines:
            print(line.text)
            print(line.bounding_box)
print()
'''
END - Batch Read File - local
'''

# <snippet_read_call>
'''
Read File, recognize handwritten text - remote
This example will extract handwritten text in an image, then print results, line by line.
This API call can also recognize handwriting (not shown).
'''
print("===== Batch Read File - remote =====")
# Get an image with handwritten text
remote_image_handw_text_url = "https://raw.githubusercontent.com/MicrosoftDocs/azure-docs/master/articles/cognitive-services/Computer-vision/Images/readsample.jpg"

# Call API with URL and raw response (allows you to get the operation location)
raw_http_response = computervision_client.read(remote_image_handw_text_url,  raw=True)
# </snippet_read_call>

# <snippet_read_response>
# Get the operation location (URL with an ID at the end) from the response
operation_location_remote = raw_http_response.headers["Operation-Location"]
# Grab the ID from the URL
operation_id = operation_location_remote.split("/")[-1]

# Call the "GET" API and wait for it to retrieve the results 
while True:
    read_result = computervision_client.get_read_result(operation_id)
    if read_result.status not in ['notStarted', 'running']:
        break
    time.sleep(1)

# Print the detected text, line by line
if read_result.status == OperationStatusCodes.succeeded:
    for text_result in read_result.analyze_result.read_results:
        for line in text_result.lines:
            print(line.text)
            print(line.bounding_box)
print()
# </snippet_read_response>
'''
END - Batch Read File - remote
'''

'''
Recognize Printed Text with OCR - local
This example will extract, using OCR, printed text in an image, then print results line by line.
'''
print("===== Detect Printed Text with OCR - local =====")
# Get an image with printed text
local_image_printed_text_path = "resources\\printed_text.jpg"
local_image_printed_text = open(local_image_printed_text_path, "rb")

ocr_result_local = computervision_client.recognize_printed_text_in_stream(local_image_printed_text)
for region in ocr_result_local.regions:
    for line in region.lines:
        print("Bounding box: {}".format(line.bounding_box))
        s = ""
        for word in line.words:
            s += word.text + " "
        print(s)
print()
'''
END - Recognize Printed Text with OCR - local
'''

'''
Recognize Printed Text with OCR - remote
This example will extract, using OCR, printed text in an image, then print results line by line.
'''
print("===== Detect Printed Text with OCR - remote =====")
remote_printed_text_image_url = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/printed_text.jpg"

ocr_result_remote = computervision_client.recognize_printed_text(remote_printed_text_image_url)
for region in ocr_result_remote.regions:
    for line in region.lines:
        print("Bounding box: {}".format(line.bounding_box))
        s = ""
        for word in line.words:
            s += word.text + " "
        print(s)
print()
'''
END - Recognize Printed Text with OCR - remote
'''

print("End of Computer Vision quickstart.")
