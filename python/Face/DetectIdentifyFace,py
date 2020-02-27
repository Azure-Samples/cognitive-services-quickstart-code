import glob
import os
import sys
import time

from azure.cognitiveservices.vision.face import FaceClient
from azure.cognitiveservices.vision.face.models import TrainingStatusType
from msrest.authentication import CognitiveServicesCredentials

'''
This sample shows a mixed-use case for Person Group, Person Group Person, Detect, and Identify Face API calls.
It starts by creating a Person Group with a few images of the same person.     
It then uses different test images to try to identify if they are found in the person group or not.
Finally, for all images NOT found in the person group, a new Person object is made of each unique image
and added to the existing person group.        

Install the Face SDK library:
pip install --upgrade azure-cognitiveservices-vision-face

'''
Create and train a person group and add a person group person to it.
'''
def build_person_group(client, person_group_id, pgp_name):
    print('Create and build a person group...')
    # Create empty Person Group. Person Group ID must be lower case, alphanumeric, and/or with '-', '_'.
    print('Person group ID:', person_group_id)
    client.person_group.create(person_group_id = person_group_id, name=person_group_id)

    # Create a person group person.
    woman = client.person_group_person.create(person_group_id, pgp_name)
    # Find all jpeg images of women in working directory.
    woman_images = [file for file in glob.glob('*.jpg') if file.startswith("woman")]
    # Add images to a woman Person object
    for image_w in woman_images:
        with open(image_w, 'rb') as w:
            client.person_group_person.add_face_from_stream(person_group_id, woman.person_id, w)

    # Train the person group, after a Person object with many images were added to it.
    client.person_group.train(person_group_id)

    # Wait for training to finish.
    while (True):
        training_status = client.person_group.get_training_status(person_group_id)
        print("Training status: {}.".format(training_status.status))
        if (training_status.status is TrainingStatusType.succeeded):
            break
        elif (training_status.status is TrainingStatusType.failed):
            sys.exit('Training the person group has failed.')
        time.sleep(5)


'''
Detect all faces in query image list, then add their face IDs to a new list.
'''
def detect_faces(client, query_images_list):
    print('Detecting faces in query images list...')

    face_ids = {} # Keep track of the image ID and the related image in a dictionary
    for image_name in query_images_list:
        image = open(image_name, 'rb') # BufferedReader
        print("Opening image: ", image.name)
        time.sleep(5)

        # Detect the faces in the query images list one at a time, returns list[DetectedFace]
        faces = client.face.detect_with_stream(image)  

        # Add all detected face IDs to a list
        for face in faces:
            print('Face ID', face.face_id, 'found in image', os.path.splitext(image.name)[0]+'.jpg')
            # Add the ID to a dictionary with image name as a key.
            # This assumes there is only one face per image (since you can't have duplicate keys)
            face_ids[image.name] = face.face_id

    return face_ids

'''
Identify a similar image face to the query image face in the existing person group, if any
'''
# Use all face IDs from query images to search for similar faces in our person group.
def identify_face(client, person_group_id, face_ids):
    # Get all image IDs (values) of the detected faces from query inmages
    ids = []
    for value in face_ids.values():
        # Make the list
        ids.append(value) 

    print('Identifying found faces in person group...')
    # We'll create a list of faces not found, for use later in creating a new Person object
    faces_not_found = face_ids
    # For each query face detected, check if they are in the existing person group
    for id in ids:
        # Identify person(s) matching the query face, returns list[IdentifyResult]
        results = client.face.identify([id], person_group_id)

        # Go through each IdentifyResult result to find the candidate (match).
        for result in results:
            for cand in result.candidates: # list[IdentifyCandidate]
                # Get the image name associated with our detected face ID
                image_name = [k for k, v in face_ids.items() if v == id][0]
                print('Found a face from Person ID', cand.person_id, 'from the image', image_name)
                # Delete the dictionary entry for the found face
                faces_not_found.pop(image_name)

    # Return a dictionary of faces not found
    return faces_not_found

'''
Create a new Person object for each unique face NOT found in the person group
'''
# For any query face not in the person group, create a new Person object for them.
# This assumes all faces are of different people, as you'd only want one Person object
# for all images belonging to the same person.
def create_new_person(client, faces_not_found, person_group_id, test_images):

    print('Creating Person object for any face not found in the person group...')

    for image_name in faces_not_found.keys():
        # Name each new Person object we create
        name = 'person-created-' + str(time.strftime("%Y_%m_%d_%H_%M_%S"))
        # Create a new Person object and add that image to it.
        new_person = client.person_group_person.create(person_group_id, name)
        img = open(image_name, 'rb')
        # Add the new person to your Person object and your person group
        face_client.person_group_person.add_face_from_stream(person_group_id, new_person.person_id, img)
        print('New Person Created:', name)


if __name__ == '__main__':

    # Add your Face key and endpoint to your environment variables
    ENDPOINT = os.environ['FACE_ENDPOINT']
    KEY = os.environ['FACE_SUBSCRIPTION_KEY']
    # Create a client
    face_client = FaceClient(ENDPOINT, CognitiveServicesCredentials(KEY))

    # Name your person group. Must be lowercase, alphanumeric, or using dash or underscore.
    PERSON_GROUP_ID = 'sample-person-group'
    # Name your person group person that will be added to our person group.
    pgp_name = 'Woman'

    # Create a query images list.
    test_images = [file for file in glob.glob('*.jpg') if file.startswith("child")]
    WORKING_DIR = os.path.join(os.path.dirname(os.path.realpath(__file__)))
    # The one image with a person found in the existing person group
    test_images.append('extra-woman-image.jpg')

    print()
    # Person Group operations
    build_person_group(face_client, PERSON_GROUP_ID, pgp_name)

    print()
    # DETECT faces in all query images. Adds those detected to face_ids list.
    ids = detect_faces(face_client, test_images)

    print()
    # IDENTIFY a similar face (if any) in person group with query face IDs. 
    # Returns a dictionary of IDs/images of those identified. 
    faces_not_found = identify_face(face_client, PERSON_GROUP_ID, ids)

    print()
    # CREATE NEW PERSON from each image NOT found in existing person group
    create_new_person(face_client, faces_not_found, PERSON_GROUP_ID, test_images)

    print()
    # DELETE the person group at the end, since testing.
    # This will prevent duplicate person group IDs being sent to the API, when you test again.       
    face_client.person_group.delete(person_group_id=PERSON_GROUP_ID)
    print("Deleted the person group {} from the source location.".format(PERSON_GROUP_ID))
    print()
