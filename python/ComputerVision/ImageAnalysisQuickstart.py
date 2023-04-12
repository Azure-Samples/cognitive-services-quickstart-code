'''
Computer Vision Quickstart for Microsoft Azure Cognitive Services. 
Uses local and remote images in each example.

Prerequisites:
    - Install the Computer Vision SDK:
      pip install --upgrade azure-cognitiveservices-vision-computervision
    - Install PIL:
      pip install --upgrade pillow
    - Create folder and collect images: 
      Create a folder called "images" in the same folder as this script.
      Go to this website to download images:
        https://github.com/Azure-Samples/cognitive-services-sample-data-files/tree/master/ComputerVision/Images
      Add the following 7 images (or use your own) to your "images" folder: 
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

References:
    - SDK: https://docs.microsoft.com/en-us/python/api/azure-cognitiveservices-vision-computervision/azure.cognitiveservices.vision.computervision?view=azure-python
    - Documentaion: https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/index
    - API: https://westus.dev.cognitive.microsoft.com/docs/services/computer-vision-v3-2/operations/5d986960601faab4bf452005
'''

# <snippet_imports_and_vars>
# <snippet_imports>
from azure.cognitiveservices.vision.computervision import ComputerVisionClient
from azure.cognitiveservices.vision.computervision.models import OperationStatusCodes
from azure.cognitiveservices.vision.computervision.models import VisualFeatureTypes, Details
from msrest.authentication import CognitiveServicesCredentials

from array import array
import os
from PIL import Image
import sys
import time
# </snippet_imports>

'''
Authenticate
Authenticates your credentials and creates a client.
'''
# <snippet_vars>
subscription_key = "PASTE_YOUR_COMPUTER_VISION_KEY_HERE"
endpoint = "PASTE_YOUR_COMPUTER_VISION_ENDPOINT_HERE"
# </snippet_vars>
# </snippet_imports_and_vars>

# <snippet_client>
computervision_client = ComputerVisionClient(endpoint, CognitiveServicesCredentials(subscription_key))
# </snippet_client>
'''
END - Authenticate
'''

'''
Quickstart variables
These variables are shared by several examples
'''
# Images used for the examples: Describe an image, Categorize an image, Tag an image, 
# Detect faces, Detect adult or racy content, Detect the color scheme, 
# Detect domain-specific content, Detect image types, Detect objects
images_folder = os.path.join (os.path.dirname(os.path.abspath(__file__)), "images")
# <snippet_remoteimage>
remote_image_url = "https://moderatorsampleimages.blob.core.windows.net/samples/sample16.png"
# </snippet_remoteimage>
'''
END - Quickstart variables
'''

'''
Describe an Image - local
This example describes the contents of an image with the confidence score.
'''
print("===== Describe an Image - local =====")
# Open local image file
local_image_path = os.path.join (images_folder, "faces.jpg")
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

# <snippet_features_remote>
print("===== Analyze an image - remote =====")
# Select the visual feature(s) you want.
remote_image_features = [VisualFeatureTypes.categories,VisualFeatureTypes.brands,VisualFeatureTypes.adult,VisualFeatureTypes.color,VisualFeatureTypes.description,VisualFeatureTypes.faces,VisualFeatureTypes.image_type,VisualFeatureTypes.objects,VisualFeatureTypes.tags]
remote_image_details = [Details.celebrities,Details.landmarks]
# </snippet_features_remote>

# <snippet_analyze>
# Call API with URL and features
results_remote = computervision_client.analyze_image(remote_image_url , remote_image_features, remote_image_details)

# Print results with confidence score
print("Categories from remote image: ")
if (len(results_remote.categories) == 0):
    print("No categories detected.")
else:
    for category in results_remote.categories:
        print("'{}' with confidence {:.2f}%".format(category.name, category.score * 100))
print()

# Detect faces
# Print the results with gender, age, and bounding box
print("Faces in the remote image: ")
if (len(results_remote.faces) == 0):
    print("No faces detected.")
else:
    for face in results_remote.faces:
        print("'{}' of age {} at location {}, {}, {}, {}".format(face.gender, face.age, \
        face.face_rectangle.left, face.face_rectangle.top, \
        face.face_rectangle.left + face.face_rectangle.width, \
        face.face_rectangle.top + face.face_rectangle.height))

# Adult content
# Print results with adult/racy score
print("Analyzing remote image for adult or racy content ... ")
print("Is adult content: {} with confidence {:.2f}".format(results_remote.adult.is_adult_content, results_remote.adult.adult_score * 100))
print("Has racy content: {} with confidence {:.2f}".format(results_remote.adult.is_racy_content, results_remote.adult.racy_score * 100))
# </snippet_adult>
print()

# Detect colors
# Print results of color scheme
print("Getting color scheme of the remote image: ")
print("Is black and white: {}".format(results_remote.color.is_bw_img))
print("Accent color: {}".format(results_remote.color.accent_color))
print("Dominant background color: {}".format(results_remote.color.dominant_color_background))
print("Dominant foreground color: {}".format(results_remote.color.dominant_color_foreground))
print("Dominant colors: {}".format(results_remote.color.dominant_colors))
# </snippet_color>
print()

# Detect image type
# Prints type results with degree of accuracy
print("Type of remote image:")
if results_remote.image_type.clip_art_type == 0:
    print("Image is not clip art.")
elif results_remote.image_type.line_drawing_type == 1:
    print("Image is ambiguously clip art.")
elif results_remote.image_type.line_drawing_type == 2:
    print("Image is normal clip art.")
else:
    print("Image is good clip art.")

if results_remote.image_type.line_drawing_type == 0:
    print("Image is not a line drawing.")
else:
    print("Image is a line drawing")

# Detect brands
print("Detecting brands in remote image: ")
if len(results_remote.brands) == 0:
    print("No brands detected.")
else:
    for brand in results_remote.brands:
        print("'{}' brand detected with confidence {:.1f}% at location {}, {}, {}, {}".format( \
        brand.name, brand.confidence * 100, brand.rectangle.x, brand.rectangle.x + brand.rectangle.w, \
        brand.rectangle.y, brand.rectangle.y + brand.rectangle.h))

# Detect objects
# Print detected objects results with bounding boxes
print("Detecting objects in remote image:")
if len(results_remote.objects) == 0:
    print("No objects detected.")
else:
    for object in detect_objects_results_remote.objects:
        print("object at location {}, {}, {}, {}".format( \
        object.rectangle.x, object.rectangle.x + object.rectangle.w, \
        object.rectangle.y, object.rectangle.y + object.rectangle.h))


# Describe image
# Get the captions (descriptions) from the response, with confidence level
print("Description of remote image: ")
if (len(results_remote.description.captions) == 0):
    print("No description detected.")
else:
    for caption in results_remote.description.captions:
        print("'{}' with confidence {:.2f}%".format(caption.text, caption.confidence * 100))
print()

# Return tags
# Print results with confidence score
print("Tags in the remote image: ")
if (len(results_remote.tags) == 0):
    print("No tags detected.")
else:
    for tag in results_remote.tags:
        print("'{}' with confidence {:.2f}%".format(tag.name, tag.confidence * 100))

# Detect celebrities
print("Celebrities in the remote image:")
if (len(results_remote.categories.detail.celebrities) == 0):
    print("No celebrities detected.")
else:
    for celeb in results_remote.categories.detail.celebrities:
        print(celeb["name"])

# Detect landmarks
print("Landmarks in the remote image:")
if len(results_remote.categories.detail.landmarks) == 0:
    print("No landmarks detected.")
else:
    for landmark in results_remote.categories.detail.landmarks:
        print(landmark["name"])

# </snippet_analyze>















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
local_image_path_landmark = os.path.join (images_folder, "landmark.jpg")
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


'''
Detect Image Types - local
This example detects an image's type (clip art/line drawing).
'''
print("===== Detect Image Types - local =====")
# Open local image
local_image_path_type = os.path.join (images_folder, "type-image.jpg")
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


'''
Detect Objects - local
This example detects different kinds of objects with bounding boxes in a local image.
'''
print("===== Detect Objects - local =====")
# Get local image with different objects in it
local_image_path_objects = os.path.join (images_folder, "objects.jpg")
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

'''
Detect Brands - local
This example detects common brands like logos and puts a bounding box around them.
'''
print("===== Detect Brands - local =====")
# Open image file
local_image_path_shirt = os.path.join (images_folder, "gray-shirt-logo.jpg")
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

'''
Generate Thumbnail
This example creates a thumbnail from both a local and URL image.
'''
print("===== Generate Thumbnail =====")

# Generate a thumbnail from a local image
local_image_path_thumb = os.path.join (images_folder, "objects.jpg")
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

print("End of Computer Vision quickstart.")
