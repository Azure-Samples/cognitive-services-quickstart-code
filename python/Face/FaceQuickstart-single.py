# <snippet_single>
import os
import sys
import time
import uuid

from azure.core.credentials import AzureKeyCredential
from azure.ai.vision.face import FaceAdministrationClient, FaceClient
from azure.ai.vision.face.models import (
    FaceAttributeTypeRecognition04,
    FaceDetectionModel,
    FaceRecognitionModel,
    QualityForRecognition,
)


# This key will serve all examples in this document.
KEY = os.environ["VISION_KEY"]

# This endpoint will be used in all examples in this quickstart.
ENDPOINT = os.environ["VISION_ENDPOINT"]

# Used in the Person Group Operations and Delete Person Group examples.
# You can call list_person_groups to print a list of preexisting PersonGroups.
# SOURCE_PERSON_GROUP_ID should be all lowercase and alphanumeric. For example, 'mygroupname' (dashes are OK).
PERSON_GROUP_ID = str(uuid.uuid4())  # assign a random ID (or name it anything)

# Create an authenticated FaceAdministrationClient and FaceClient.
# FaceAdministrationClient is used for create, update, get, or delete Person Group and Person. Also it can add or
# remove face from a Person. FaceClient is for calling detect, identify and verify.
with FaceAdministrationClient(endpoint=ENDPOINT, credential=AzureKeyCredential(KEY)) as face_admin_client, \
     FaceClient(endpoint=ENDPOINT, credential=AzureKeyCredential(KEY)) as face_client:
    '''
    Create the PersonGroup
    '''
    # Create empty Person Group. Person Group ID must be lower case, alphanumeric, and/or with '-', '_'.
    print('Person group:', PERSON_GROUP_ID)
    face_admin_client.create_person_group(
        person_group_id=PERSON_GROUP_ID, name=PERSON_GROUP_ID, recognition_model=FaceRecognitionModel.RECOGNITION_04)

    # Define woman friend
    woman = face_admin_client.create_person_group_person(PERSON_GROUP_ID, name="Woman")
    # Define man friend
    man = face_admin_client.create_person_group_person(PERSON_GROUP_ID, name="Man")
    # Define child friend
    child = face_admin_client.create_person_group_person(PERSON_GROUP_ID, name="Child")

    '''
    Detect faces and register them to each person
    '''
    # Find all jpeg images of friends in working directory (TBD pull from web instead)
    woman_images = [
        "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/Family1-Mom1.jpg",  # noqa: E501
        "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/Family1-Mom2.jpg",  # noqa: E501
    ]
    man_images = [
        "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/Family1-Dad1.jpg",  # noqa: E501
        "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/Family1-Dad2.jpg",  # noqa: E501
    ]
    child_images = [
        "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/Family1-Son1.jpg",  # noqa: E501
        "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/Family1-Son2.jpg",  # noqa: E501
    ]

    # Add to woman person
    for image in woman_images:
        # Check if the image is of sufficent quality for recognition.
        sufficientQuality = True
        detected_faces = face_client.detect_from_url(
            url=image,
            detection_model=FaceDetectionModel.DETECTION_03,
            recognition_model=FaceRecognitionModel.RECOGNITION_04,
            return_face_id=True,
            return_face_attributes=[FaceAttributeTypeRecognition04.QUALITY_FOR_RECOGNITION])
        for face in detected_faces:
            if face.face_attributes.quality_for_recognition != QualityForRecognition.HIGH:
                sufficientQuality = False
                break
            face_admin_client.add_person_group_person_face_from_url(PERSON_GROUP_ID, woman.person_id, url=image)
            print(f"face {face.face_id} added to person {woman.person_id}")

        if not sufficientQuality:
            continue

    # Add to man person
    for image in man_images:
        # Check if the image is of sufficent quality for recognition.
        sufficientQuality = True
        detected_faces = face_client.detect_from_url(
            url=image,
            detection_model=FaceDetectionModel.DETECTION_03,
            recognition_model=FaceRecognitionModel.RECOGNITION_04,
            return_face_id=True,
            return_face_attributes=[FaceAttributeTypeRecognition04.QUALITY_FOR_RECOGNITION])
        for face in detected_faces:
            if face.face_attributes.quality_for_recognition != QualityForRecognition.HIGH:
                sufficientQuality = False
                break
            face_admin_client.add_person_group_person_face_from_url(PERSON_GROUP_ID, man.person_id, url=image)
            print(f"face {face.face_id} added to person {man.person_id}")

        if not sufficientQuality:
            continue

    # Add to child person
    for image in child_images:
        # Check if the image is of sufficent quality for recognition.
        sufficientQuality = True
        detected_faces = face_client.detect_from_url(
            url=image,
            detection_model=FaceDetectionModel.DETECTION_03,
            recognition_model=FaceRecognitionModel.RECOGNITION_04,
            return_face_id=True,
            return_face_attributes=[FaceAttributeTypeRecognition04.QUALITY_FOR_RECOGNITION])
        for face in detected_faces:
            if face.face_attributes.quality_for_recognition != QualityForRecognition.HIGH:
                sufficientQuality = False
                print("{} has insufficient quality".format(face))
                break
            face_admin_client.add_person_group_person_face_from_url(PERSON_GROUP_ID, child.person_id, url=image)
            print(f"face {face.face_id} added to person {child.person_id}")

        if not sufficientQuality:
            continue

    '''
    Train PersonGroup
    '''
    # Train the person group
    print(f"Train the person group {PERSON_GROUP_ID}")
    try:
        poller = face_admin_client.begin_train_person_group(PERSON_GROUP_ID, polling_interval=30)
        poller.wait()  # Keep polling until the "Train" operation completes.
    except Exception as error:
        face_admin_client.delete_person_group(person_group_id=PERSON_GROUP_ID)
        sys.exit(f"Training the person group has failed. Error: {error}")
    print(f"The person group {PERSON_GROUP_ID} is trained successfully.")

    '''
    Identify a face against a defined PersonGroup
    '''
    # Group image for testing against
    test_image = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/identification1.jpg"  # noqa: E501

    print("Pausing for 10 seconds to avoid triggering rate limit on free account...")
    time.sleep(10)

    # Detect faces
    face_ids = []
    # We use detection model 03 to get better performance, recognition model 04 to support quality for
    # recognition attribute.
    faces = face_client.detect_from_url(
        url=test_image,
        detection_model=FaceDetectionModel.DETECTION_03,
        recognition_model=FaceRecognitionModel.RECOGNITION_04,
        return_face_id=True,
        return_face_attributes=[FaceAttributeTypeRecognition04.QUALITY_FOR_RECOGNITION])
    for face in faces:
        # Only take the face if it is of sufficient quality.
        if face.face_attributes.quality_for_recognition == QualityForRecognition.HIGH or \
                face.face_attributes.quality_for_recognition == QualityForRecognition.MEDIUM:
            face_ids.append(face.face_id)

    # Identify faces
    results = face_client.identify_from_person_group(face_ids=face_ids, person_group_id=PERSON_GROUP_ID)
    print("Identifying faces in image")
    if not results:
        print("No person identified in the person group")
    for identifiedFace in results:
        if len(identifiedFace.candidates) > 0:
            print(f"Person is identified for face ID {identifiedFace.face_id} in image, with a confidence of "
                  f"{identifiedFace.candidates[0].confidence}.")  # Get topmost confidence score

            # Verify faces
            verify_result = face_client.verify_from_person_group(
                face_id=identifiedFace.face_id,
                person_group_id=PERSON_GROUP_ID,
                person_id=identifiedFace.candidates[0].person_id)
            print(f"verification result: {verify_result.is_identical}. confidence: {verify_result.confidence}")
        else:
            print(f"No person identified for face ID {identifiedFace.face_id} in image.")

    print()

    # Delete the person group
    face_admin_client.delete_person_group(person_group_id=PERSON_GROUP_ID)
    print(f"The person group {PERSON_GROUP_ID} is deleted.")

    print()
    print("End of quickstart.")

# </snippet_single>
