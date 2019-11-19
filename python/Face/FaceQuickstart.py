# <snippet_imports>
import asyncio
import io
import glob
import os
import sys
import time
import uuid
import requests
from urllib.parse import urlparse
from io import BytesIO
from PIL import Image, ImageDraw
from azure.cognitiveservices.vision.face import FaceClient
from msrest.authentication import CognitiveServicesCredentials
from azure.cognitiveservices.vision.face.models import TrainingStatusType, Person, SnapshotObjectType, OperationStatusType
# </snippet_imports>

'''
Face Quickstart

Examples include:
    - Detect Faces: detects faces in an image.
    - Find Similar: finds a similar face in an image using ID from Detect Faces.
    - Verify: compares two images to check if they are the same person or not.
    - Person Group: creates a person group and uses it to identify faces in other images.
    - Large Person Group: similar to person group, but with different API calls to handle scale.
    - Face List: creates a list of single-faced images, then gets data from list.
    - Large Face List: creates a large list for single-faced images, trains it, then gets data.
    - Snapshot: copies a person group from one region to another, or from one Azure subscription to another.

Prerequisites:
    - Python 3+
    - Install Face SDK: pip install azure-cognitiveservices-vision-face
    - In your root folder, add all images downloaded from here:
      https://github.com/Azure-examples/cognitive-services-sample-data-files/tree/master/Face/images
How to run:
    - Run from command line or an IDE
    - If the Person Group or Large Person Group (or Face List / Large Face List) examples get
      interrupted after creation, be sure to delete your created person group (lists) from the API,
      as you cannot create a new one with the same name. Use 'Person group - List' to check them all,
      and 'Person Group - Delete' to remove one. The examples have a delete function in them, but at the end.
      Person Group API: https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395244
      Face List API: https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f3039524d
References:
    - Documentation: https://docs.microsoft.com/en-us/azure/cognitive-services/face/
    - SDK: https://docs.microsoft.com/en-us/python/api/azure-cognitiveservices-vision-face/azure.cognitiveservices.vision.face?view=azure-python
    - All Face APIs: https://docs.microsoft.com/en-us/azure/cognitive-services/face/APIReference
'''

# <snippet_subvars>
# Set the FACE_SUBSCRIPTION_KEY environment variable with your key as the value.
# This key will serve all examples in this document.
KEY = os.environ['FACE_SUBSCRIPTION_KEY']

# Set the FACE_ENDPOINT environment variable with the endpoint from your Face service in Azure.
# This endpoint will be used in all examples in this quickstart.
ENDPOINT = os.environ['FACE_ENDPOINT']
# </snippet_subvars>

# <snippet_verify_baseurl>
# Base url for the Verify and Facelist/Large Facelist operations
IMAGE_BASE_URL = 'https://csdx.blob.core.windows.net/resources/Face/Images/'
# </snippet_verify_baseurl>

# <snippet_persongroupvars>
# Used in the Person Group Operations,  Snapshot Operations, and Delete Person Group examples.
# You can call list_person_groups to print a list of preexisting PersonGroups.
# SOURCE_PERSON_GROUP_ID should be all lowercase and alphanumeric. For example, 'mygroupname' (dashes are OK).
PERSON_GROUP_ID = 'my-unique-person-group'

# Used for the Snapshot and Delete Person Group examples.
TARGET_PERSON_GROUP_ID = str(uuid.uuid4()) # assign a random ID (or name it anything)
# </snippet_persongroupvars>

# <snippet_snapshotvars>
'''
Snapshot operations variables
These are only used for the snapshot example. Set your environment variables accordingly.
'''
# Source endpoint, the location/subscription where the original person group is located.
SOURCE_ENDPOINT = ENDPOINT
# Source subscription key. Must match the source endpoint region.
SOURCE_KEY = os.environ['FACE_SUBSCRIPTION_KEY']
# Source subscription ID. Found in the Azure portal in the Overview page of your Face (or any) resource.
SOURCE_ID = os.environ['AZURE_SUBSCRIPTION_ID']
# Person group name that will get created in this quickstart's Person Group Operations example.
SOURCE_PERSON_GROUP_ID = PERSON_GROUP_ID
# Target endpoint. This is your 2nd Face subscription.
TARGET_ENDPOINT = os.environ['FACE_ENDPOINT2']
# Target subscription key. Must match the target endpoint region.
TARGET_KEY = os.environ['FACE_SUBSCRIPTION_KEY2']
# Target subscription ID. It will be the same as the source ID if created Face resources from the same subscription (but moving from region to region). If they are differnt subscriptions, add the other target ID here.
TARGET_ID = os.environ['AZURE_SUBSCRIPTION_ID']
# NOTE: We do not need to specify the target PersonGroup ID here because we generate it with this example.
# Each new location you transfer a person group to will have a generated, new person group ID for that region.
# </snippet_snapshotvars>

'''
Authenticate
All examples use the same client, except for Snapshot Operations.
'''
# <snippet_auth>
# Create an authenticated FaceClient.
face_client = FaceClient(ENDPOINT, CognitiveServicesCredentials(KEY))
# </snippet_auth>
'''
END - Authenticate
'''

'''
Detect faces 
Detect faces in two images (get ID), draw rectangle around a third image.
'''
print('-----------------------------')
print()
print('DETECT FACES')
print()
# <snippet_detect>
# Detect a face in an image that contains a single face
single_face_image_url = 'https://www.biography.com/.image/t_share/MTQ1MzAyNzYzOTgxNTE0NTEz/john-f-kennedy---mini-biography.jpg'
single_image_name = os.path.basename(single_face_image_url)
detected_faces = face_client.face.detect_with_url(url=single_face_image_url)
if not detected_faces:
	raise Exception('No face detected from image {}'.format(single_image_name))

# Display the detected face ID in the first single-face image.
# Face IDs are used for comparison to faces (their IDs) detected in other images.
print('Detected face ID from', single_image_name, ':')
for face in detected_faces: print (face.face_id)
print()

# Save this ID for use in Find Similar
first_image_face_ID = detected_faces[0].face_id
# </snippet_detect>

# <snippet_detectgroup>
# Detect the faces in an image that contains multiple faces
# Each detected face gets assigned a new ID
multi_face_image_url = "http://www.historyplace.com/kennedy/president-family-portrait-closeup.jpg"
multi_image_name = os.path.basename(multi_face_image_url)
detected_faces2 = face_client.face.detect_with_url(url=multi_face_image_url)
# </snippet_detectgroup>

print('Detected face IDs from', multi_image_name, ':')
if not detected_faces2:
	raise Exception('No face detected from image {}.'.format(multi_image_name))
else:
    for face in detected_faces2:
        print(face.face_id)
print()

'''
Print image and draw rectangles around faces
'''
# <snippet_frame>
# Detect a face in an image that contains a single face
single_face_image_url = 'https://raw.githubusercontent.com/Microsoft/Cognitive-Face-Windows/master/Data/detection1.jpg'
single_image_name = os.path.basename(single_face_image_url)
detected_faces = face_client.face.detect_with_url(url=single_face_image_url)
if not detected_faces:
	raise Exception('No face detected from image {}'.format(single_image_name))

# Convert width height to a point in a rectangle
def getRectangle(faceDictionary):
    rect = faceDictionary.face_rectangle
    left = rect.left
    top = rect.top
    bottom = left + rect.height
    right = top + rect.width
    return ((left, top), (bottom, right))


# Download the image from the url
response = requests.get(single_face_image_url)
img = Image.open(BytesIO(response.content))

# For each face returned use the face rectangle and draw a red box.
print('Drawing rectangle around face... see popup for results.')
draw = ImageDraw.Draw(img)
for face in detected_faces:
    draw.rectangle(getRectangle(face), outline='red')

# Display the image in the users default image browser.
img.show()
# </snippet_frame>

print()
'''
END - Detect faces
'''

'''
Find a similar face
This example uses detected faces in a group photo to find a similar face using a single-faced image as query.
'''
print('-----------------------------')
print()
print('FIND SIMILAR')
print()
# <snippet_findsimilar>
# Search through faces detected in group image for the single face from first image.
# First, create a list of the face IDs found in the second image.
second_image_face_IDs = list(map(lambda x: x.face_id, detected_faces2))
# Next, find similar face IDs like the one detected in the first image.
similar_faces = face_client.face.find_similar(face_id=first_image_face_ID, face_ids=second_image_face_IDs)
if not similar_faces[0]:
	print('No similar faces found in', multi_image_name, '.')
# </snippet_findsimilar>

# <snippet_findsimilar_print>
# Print the details of the similar faces detected
print('Similar faces found in', multi_image_name + ':')
for face in similar_faces:
	first_image_face_ID = face.face_id
	# The similar face IDs of the single face image and the group image do not need to match, they are only used for identification purposes in each image.
	# The similar faces are matched using the Cognitive Services algorithm in find_similar().
	face_info = next(x for x in detected_faces2 if x.face_id == first_image_face_ID)
	if face_info:
		print('  Face ID: ', first_image_face_ID)
		print('  Face rectangle:')
		print('    Left: ', str(face_info.face_rectangle.left))
		print('    Top: ', str(face_info.face_rectangle.top))
		print('    Width: ', str(face_info.face_rectangle.width))
		print('    Height: ', str(face_info.face_rectangle.height))
# </snippet_findsimilar_print>
print()
'''
END - Find Similar
'''

'''
Verify
The Verify operation takes a face ID from DetectedFace or PersistedFace and either another face ID or a Person object and determines whether they belong to the same person. If you pass in a Person object, you can optionally pass in a PersonGroup to which that Person belongs to improve performance.
'''
print('-----------------------------')
print()
print('VERIFY')
print()
# <snippet_verify_photos>
# Create a list to hold the target photos of the same person
target_image_file_names = ['Family1-Dad1.jpg', 'Family1-Dad2.jpg']
# The source photos contain this person
source_image_file_name1 = 'Family1-Dad3.jpg'
source_image_file_name2 = 'Family1-Son1.jpg'
# </snippet_verify_photos>

# <snippet_verify_detect>
# Detect face(s) from source image 1, returns a list[DetectedFaces]
detected_faces1 = face_client.face.detect_with_url(IMAGE_BASE_URL + source_image_file_name1)
# Add the returned face's face ID
source_image1_id = detected_faces1[0].face_id
print('{} face(s) detected from image {}.'.format(len(detected_faces1), source_image_file_name1))

# Detect face(s) from source image 2, returns a list[DetectedFaces]
detected_faces2 = face_client.face.detect_with_url(IMAGE_BASE_URL + source_image_file_name2)
# Add the returned face's face ID
source_image2_id = detected_faces2[0].face_id
print('{} face(s) detected from image {}.'.format(len(detected_faces2), source_image_file_name2))

# List for the target face IDs (uuids)
detected_faces_ids = []
# Detect faces from target image url list, returns a list[DetectedFaces]
for image_file_name in target_image_file_names:
    detected_faces = face_client.face.detect_with_url(IMAGE_BASE_URL + image_file_name)
    # Add the returned face's face ID
    detected_faces_ids.append(detected_faces[0].face_id)
    print('{} face(s) detected from image {}.'.format(len(detected_faces), image_file_name))
# </snippet_verify_detect>

# <snippet_verify>
# Verification example for faces of the same person. The higher the confidence, the more identical the faces in the images are.
# Since target faces are the same person, in this example, we can use the 1st ID in the detected_faces_ids list to compare.
verify_result_same = face_client.face.verify_face_to_face(source_image1_id, detected_faces_ids[0])
print('Faces from {} & {} are of the same person, with confidence: {}'
    .format(source_image_file_name1, target_image_file_names[0], verify_result_same.confidence)
    if verify_result_same.is_identical
    else 'Faces from {} & {} are of a different person, with confidence: {}'
        .format(source_image_file_name1, target_image_file_names[0], verify_result_same.confidence))

# Verification example for faces of different persons.
# Since target faces are same person, in this example, we can use the 1st ID in the detected_faces_ids list to compare.
verify_result_diff = face_client.face.verify_face_to_face(source_image2_id, detected_faces_ids[0])
print('Faces from {} & {} are of the same person, with confidence: {}'
    .format(source_image_file_name2, target_image_file_names[0], verify_result_diff.confidence)
    if verify_result_diff.is_identical
    else 'Faces from {} & {} are of a different person, with confidence: {}'
        .format(source_image_file_name2, target_image_file_names[0], verify_result_diff.confidence))
# </snippet_verify>
print()
'''
END - VERIFY
'''

'''
Create/Train/Detect/Identify Person Group
This example creates a Person Group, then trains it. It can then be used to detect and identify faces in other group images.
'''
print('-----------------------------')
print()
print('PERSON GROUP OPERATIONS')
print()

# <snippet_persongroup_create>
'''
Create the PersonGroup
'''
# Create empty Person Group. Person Group ID must be lower case, alphanumeric, and/or with '-', '_'.
print('Person group:', PERSON_GROUP_ID)
face_client.person_group.create(person_group_id=PERSON_GROUP_ID, name=PERSON_GROUP_ID)

# Define woman friend
woman = face_client.person_group_person.create(PERSON_GROUP_ID, "Woman")
# Define man friend
man = face_client.person_group_person.create(PERSON_GROUP_ID, "Man")
# Define child friend
child = face_client.person_group_person.create(PERSON_GROUP_ID, "Child")
# </snippet_persongroup_create>

# <snippet_persongroup_assign>
'''
Detect faces and register to correct person
'''
# Find all jpeg images of friends in working directory
woman_images = [file for file in glob.glob('*.jpg') if file.startswith("woman")]
man_images = [file for file in glob.glob('*.jpg') if file.startswith("man")]
child_images = [file for file in glob.glob('*.jpg') if file.startswith("child")]

# Add to a woman person
for image in woman_images:
    w = open(image, 'r+b')
    face_client.person_group_person.add_face_from_stream(PERSON_GROUP_ID, woman.person_id, w)

# Add to a man person
for image in man_images:
    m = open(image, 'r+b')
    face_client.person_group_person.add_face_from_stream(PERSON_GROUP_ID, man.person_id, m)

# Add to a child person
for image in child_images:
    ch = open(image, 'r+b')
    face_client.person_group_person.add_face_from_stream(PERSON_GROUP_ID, child.person_id, ch)
# </snippet_persongroup_assign>

# <snippet_persongroup_train>
'''
Train PersonGroup
'''
print()
print('Training the person group...')
# Train the person group
face_client.person_group.train(PERSON_GROUP_ID)

while (True):
    training_status = face_client.person_group.get_training_status(PERSON_GROUP_ID)
    print("Training status: {}.".format(training_status.status))
    print()
    if (training_status.status is TrainingStatusType.succeeded):
        break
    elif (training_status.status is TrainingStatusType.failed):
        sys.exit('Training the person group has failed.')
    time.sleep(5)
# </snippet_persongroup_train>

# <snippet_identify_testimage>
'''
Identify a face against a defined PersonGroup
'''
# Group image for testing against
group_photo = 'test-image-person-group.jpg'
IMAGES_FOLDER = os.path.join(os.path.dirname(os.path.realpath(__file__)))
# Get test image
test_image_array = glob.glob(os.path.join(IMAGES_FOLDER, group_photo))
image = open(test_image_array[0], 'r+b')

# Detect faces
face_ids = []
faces = face_client.face.detect_with_stream(image)
for face in faces:
    face_ids.append(face.face_id)
# </snippet_identify_testimage>

# <snippet_identify>
# Identify faces
results = face_client.face.identify(face_ids, PERSON_GROUP_ID)
print('Identifying faces in {}'.format(os.path.basename(image.name)))
if not results:
    print('No person identified in the person group for faces from {}.'.format(os.path.basename(image.name)))
for person in results:
    print('Person for face ID {} is identified in {} with a confidence of {}.'.format(person.face_id, os.path.basename(image.name), person.candidates[0].confidence)) # Get topmost confidence score
# </snippet_identify>
print()
'''
END - Create/Train/Detect/Identify Person Group
'''

'''
Create/List/Delete Large Person Group
Uses the same list used for creating a regular-sized person group.
The operations are similar in structure as the Person Group example.
'''
print('-----------------------------')
print()
print('LARGE PERSON GROUP OPERATIONS')
print()

# Large Person Group ID, should be all lowercase and alphanumeric. For example, 'mygroupname' (dashes are OK).
LARGE_PERSON_GROUP_ID = 'my-unique-large-person-group'

# Create empty Large Person Group. Person Group ID must be lower case, alphanumeric, and/or with '-', '_'.
# The name and the ID can be either the same or different
print('Large person group:', LARGE_PERSON_GROUP_ID)
face_client.large_person_group.create(large_person_group_id=LARGE_PERSON_GROUP_ID, name=LARGE_PERSON_GROUP_ID)

# Define woman friend , by creating a large person group person
woman = face_client.large_person_group_person.create(LARGE_PERSON_GROUP_ID, "Woman")
# Define man friend
man = face_client.large_person_group_person.create(LARGE_PERSON_GROUP_ID, "Man")
# Define child friend
child = face_client.large_person_group_person.create(LARGE_PERSON_GROUP_ID, "Child")

'''
Detect faces and register to correct person
'''
# Find all jpeg images of friends in working directory
woman_images = [file for file in glob.glob('*.jpg') if file.startswith("woman")]
man_images = [file for file in glob.glob('*.jpg') if file.startswith("man")]
child_images = [file for file in glob.glob('*.jpg') if file.startswith("child")]

# Add to a woman person
for image in woman_images:
    w = open(image, 'r+b')
    face_client.large_person_group_person.add_face_from_stream(LARGE_PERSON_GROUP_ID, woman.person_id, w)

# Add to a man person
for image in man_images:
    m = open(image, 'r+b')
    face_client.large_person_group_person.add_face_from_stream(LARGE_PERSON_GROUP_ID, man.person_id, m)

# Add to a child person
for image in child_images:
    ch = open(image, 'r+b')
    face_client.large_person_group_person.add_face_from_stream(LARGE_PERSON_GROUP_ID, child.person_id, ch)

'''
Train LargePersonGroup
'''
print()
print('Training the large person group...')
# Train the person group
face_client.large_person_group.train(LARGE_PERSON_GROUP_ID)
# Check training status
while (True):
    training_status = face_client.large_person_group.get_training_status(LARGE_PERSON_GROUP_ID)
    print("Training status: {}.".format(training_status.status))
    print()
    if (training_status.status is TrainingStatusType.succeeded):
        break
    elif (training_status.status is TrainingStatusType.failed):
        sys.exit('Training the large person group has failed.')
    time.sleep(5)

# Now that we have created and trained a large person group, we can retrieve data from it.
# Returns a list[Person] of how many Persons were created/defined in the large person group.
person_list_large = face_client.large_person_group_person.list(large_person_group_id=LARGE_PERSON_GROUP_ID, start='')

print('Persisted Face IDs from {} large person group persons:'.format(len(person_list_large)))
print()
for person_large in person_list_large:
    for persisted_face_id in person_large.persisted_face_ids:
        print('The person {} has an image with ID: {}'.format(person_large.name, persisted_face_id))
print()

# After testing, delete the large person group, LargePersonGroupPersons also get deleted.
face_client.large_person_group.delete(LARGE_PERSON_GROUP_ID)
print("Deleted the large person group.")
print()
'''
END - Create/List/Delete Large Person Group
'''

'''
FACELIST
This example adds single-faced images from URL to a list, then gets data from the list.
'''
print('-----------------------------')
print()
print('FACELIST OPERATIONS')
print()

# Create our list of URL images
image_file_names = [
    "Family1-Dad1.jpg",
    "Family1-Daughter1.jpg",
    "Family1-Mom1.jpg",
    "Family1-Son1.jpg",
    "Family2-Lady1.jpg",
    "Family2-Man1.jpg",
    "Family3-Lady1.jpg",
    "Family3-Man1.jpg"
]

# Create an empty face list with an assigned ID.
face_list_id = "my-face-list"
print("Creating face list: {}...".format(face_list_id))
print()
face_client.face_list.create(face_list_id=face_list_id, name=face_list_id)

# Add each face in our array to the facelist
for image_file_name in image_file_names:
    face_client.face_list.add_face_from_url(
        face_list_id=face_list_id,
        url=IMAGE_BASE_URL + image_file_name,
        user_data=image_file_name
    )

# Get persisted faces from the face list.
the_face_list = face_client.face_list.get(face_list_id)
if not the_face_list :
    raise Exception("No persisted face in face list {}.".format(face_list_id))

print('Persisted face ids of images in face list:')
print()
for persisted_face in the_face_list.persisted_faces:
    print(persisted_face.persisted_face_id)

# Delete the face list, so you can retest (recreate) the list with same name.
face_client.face_list.delete(face_list_id=face_list_id)
print()
print("Deleted the face list: {}.\n".format(face_list_id))
'''
END - FACELIST
'''

'''
LARGE FACELIST
This example adds single-faced images from URL to a large-capacity list, then gets data from the list.
This list could handle up to 1 million images.
'''
print('-----------------------------')
print()
print('LARGE FACELIST OPERATIONS')
print()

# Create our list of URL images
image_file_names_large = [
    "Family1-Dad1.jpg",
    "Family1-Daughter1.jpg",
    "Family1-Mom1.jpg",
    "Family1-Son1.jpg",
    "Family2-Lady1.jpg",
    "Family2-Man1.jpg",
    "Family3-Lady1.jpg",
    "Family3-Man1.jpg"
]

# Create an empty face list with an assigned ID.
large_face_list_id = "my-large-face-list"
print("Creating large face list: {}...".format(large_face_list_id))
print()
face_client.large_face_list.create(large_face_list_id=large_face_list_id, name=large_face_list_id)

# Add each face in our array to the large face list
# Returns a PersistedFace
for image_file_name in image_file_names_large:
    face_client.large_face_list.add_face_from_url(
        large_face_list_id=large_face_list_id,
        url=IMAGE_BASE_URL + image_file_name,
        user_data=image_file_name
    )

# Train the large list. Must train before getting any data from the list.
# Training is not required of the regular-sized facelist.
print("Train large face list {}".format(large_face_list_id))
print()
face_client.large_face_list.train(large_face_list_id=large_face_list_id)

# Get training status
while (True):
    training_status_list = face_client.large_face_list.get_training_status(large_face_list_id=large_face_list_id)
    print("Training status: {}.".format(training_status_list.status))
    if training_status_list.status is TrainingStatusType.failed:
        raise Exception("Training failed with message {}.".format(training_status_list.message))
    if (training_status_list.status is TrainingStatusType.succeeded):
        break
    time.sleep(5)

# Returns a list[PersistedFace]. Can retrieve data from each face.
large_face_list_faces = face_client.large_face_list.list_faces(large_face_list_id)
if not large_face_list_faces :
    raise Exception("No persisted face in face list {}.".format(large_face_list_id))

print('Face ids of images in large face list:')
print()
for large_face in large_face_list_faces:
    print(large_face.persisted_face_id)

# Delete the large face list, so you can retest (recreate) the list with same name.
face_client.large_face_list.delete(large_face_list_id=large_face_list_id)
print()
print("Deleted the large face list: {}.\n".format(large_face_list_id))
'''
END - LARGE FACELIST
'''

'''
Snapshot Operations
This example transfers a person group from one region to another region.
You can also transfer it to another subscription (change the target subscription key).
It uses the same client as the above examples for its source client.
'''
print('-----------------------------')
print()
print('SNAPSHOT OPERATIONS')
print()

# <snippet_snapshot_auth>
'''
Authenticate
'''
# Use your source client already created (it has the person group ID you need in it).
face_client_source = face_client
# Create a new FaceClient instance for your target with authentication.
face_client_target = FaceClient(TARGET_ENDPOINT, CognitiveServicesCredentials(TARGET_KEY))
# </snippet_snapshot_auth>

# <snippet_snapshot_take>
'''
Snapshot operations in 4 steps
'''
async def run():
    # STEP 1, take a snapshot of your person group, then track status.
    # This list must include all subscription IDs from which you want to access the snapshot.
    source_list = [SOURCE_ID, TARGET_ID]
    # You may have many sources, if transferring from many regions
    # remove any duplicates from the list. Passing the same subscription ID more than once causes
    # the Snapshot.take operation to fail.
    source_list = list(dict.fromkeys(source_list))

    # Note Snapshot.take is not asynchronous.
    # For information about Snapshot.take see:
    # https://github.com/Azure/azure-sdk-for-python/blob/master/azure-cognitiveservices-vision-face/azure/cognitiveservices/vision/face/operations/snapshot_operations.py#L36
    take_snapshot_result = face_client_source.snapshot.take(
        type=SnapshotObjectType.person_group,
        object_id=PERSON_GROUP_ID,
        apply_scope=source_list,
        # Set this to tell Snapshot.take to return the response; otherwise it returns None.
        raw=True
        )
    # Get operation ID from response for tracking
    # Snapshot.type return value is of type msrest.pipeline.ClientRawResponse. See:
    # https://docs.microsoft.com/en-us/python/api/msrest/msrest.pipeline.clientrawresponse?view=azure-python
    take_operation_id = take_snapshot_result.response.headers['Operation-Location'].replace('/operations/', '')

    print('Taking snapshot( operation ID:', take_operation_id, ')...')
    # </snippet_snapshot_take>

    # <snippet_snapshot_wait>
    # STEP 2, Wait for snapshot taking to complete.
    take_status = await wait_for_operation(face_client_source, take_operation_id)

    # Get snapshot id from response.
    snapshot_id = take_status.resource_location.replace ('/snapshots/', '')

    print('Snapshot ID:', snapshot_id)
    print('Taking snapshot... Done\n')
    # </snippet_snapshot_wait>

    # <snippet_snapshot_apply>
    # STEP 3, apply the snapshot to target region(s)
    # Snapshot.apply is not asynchronous.
    # For information about Snapshot.apply see:
    # https://github.com/Azure/azure-sdk-for-python/blob/master/azure-cognitiveservices-vision-face/azure/cognitiveservices/vision/face/operations/snapshot_operations.py#L366
    apply_snapshot_result = face_client_target.snapshot.apply(
        snapshot_id=snapshot_id,
        # Generate a new UUID for the target person group ID.
        object_id=TARGET_PERSON_GROUP_ID,
        # Set this to tell Snapshot.apply to return the response; otherwise it returns None.
        raw=True
        )
    apply_operation_id = apply_snapshot_result.response.headers['Operation-Location'].replace('/operations/', '')
    print('Applying snapshot( operation ID:', apply_operation_id, ')...')
    # </snippet_snapshot_apply>

    # <snippet_snapshot_wait2>
    # STEP 4, wait for applying snapshot process to complete.
    await wait_for_operation(face_client_target, apply_operation_id)
    print('Applying snapshot... Done\n')
    print('End of transfer.')
    print()
    # </snippet_snapshot_wait2>

# <snippet_waitforop>
# Helper function that waits and checks status of API call processing.
async def wait_for_operation(client, operation_id):
    # Track progress of taking the snapshot.
    # Note Snapshot.get_operation_status is not asynchronous.
    # For information about Snapshot.get_operation_status see:
    # https://github.com/Azure/azure-sdk-for-python/blob/master/azure-cognitiveservices-vision-face/azure/cognitiveservices/vision/face/operations/snapshot_operations.py#L466
    result = client.snapshot.get_operation_status(operation_id=operation_id)

    status = result.status.lower()
    print('Operation status:', status)
    if ('notstarted' == status or 'running' == status):
        print("Waiting 10 seconds...")
        await asyncio.sleep(10)
        result = await wait_for_operation(client, operation_id)
    elif ('failed' == status):
        raise Exception("Operation failed. Reason:" + result.message)
    return result
# </snippet_waitforop>

'''
Nice-to-have List API calls
Use these to programmatically list your person groups or snapshots from your Azure account.
These are not used in this quickstart.
'''
# OPTIONAL Prints a list of existing person groups.
def list_person_groups(client):
    # Note PersonGroup.list is not asynchronous.
    ids = list(map(lambda x: x.PERSON_GROUP_ID, client.person_group.list()))
    for x in ids: print (x)

# OPTIONAL: Prints a list of existing snapshots.
def list_snapshots(client):
    snapshots = client.snapshot.list()
    for x in snapshots:
        print ("Snapshot ID: " + x.id)
        print ("Snapshot type: " + x.type)
        print ()

# Run the snapshot example
asyncio.run(run())
'''
END - SNAPSHOT OPERATIONS
'''

'''
Delete Person Group
For testing purposes, delete the person group made in the Person Group Operations,
and the target person group from the Snapshot Operations (uses a different client).
List the person groups in your account through the online testing console to check:
https://westus2.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395248
'''
print('-----------------------------')
print()
print('DELETE PERSON GROUP')
print()
# <snippet_deletegroup>
# Delete the main person group.
face_client.person_group.delete(person_group_id=PERSON_GROUP_ID)
print("Deleted the person group {} from the source location.".format(PERSON_GROUP_ID))
print()
# </snippet_deletegroup>

# <snippet_deletetargetgroup>
# Delete the person group in the target region.
face_client_target.person_group.delete(TARGET_PERSON_GROUP_ID)
print("Deleted the person group {} from the target location.".format(TARGET_PERSON_GROUP_ID))
# </snippet_deletetargetgroup>
print()
print('-----------------------------')
print()
print('End of quickstart.')
