# <snippet_single>
import os
import time
import uuid

from azure.core.credentials import AzureKeyCredential
from azure.ai.vision.face import FaceAdministrationClient, FaceClient
from azure.ai.vision.face.models import FaceAttributeTypeRecognition04, FaceDetectionModel, FaceRecognitionModel, QualityForRecognition


# This key will serve all examples in this document.
KEY = os.environ["FACE_APIKEY"]

# This endpoint will be used in all examples in this quickstart.
ENDPOINT = os.environ["FACE_ENDPOINT"]

# Used in the Large Person Group Operations and Delete Large Person Group examples.
# LARGE_PERSON_GROUP_ID should be all lowercase and alphanumeric. For example, 'mygroupname' (dashes are OK).
LARGE_PERSON_GROUP_ID = str(uuid.uuid4())  # assign a random ID (or name it anything)

# Create an authenticated FaceClient.
with FaceAdministrationClient(endpoint=ENDPOINT, credential=AzureKeyCredential(KEY)) as face_admin_client, \
     FaceClient(endpoint=ENDPOINT, credential=AzureKeyCredential(KEY)) as face_client:
    '''
    Create the LargePersonGroup
    '''
    # Create empty Large Person Group. Large Person Group ID must be lower case, alphanumeric, and/or with '-', '_'.
    print("Person group:", LARGE_PERSON_GROUP_ID)
    face_admin_client.large_person_group.create(
        large_person_group_id=LARGE_PERSON_GROUP_ID,
        name=LARGE_PERSON_GROUP_ID,
        recognition_model=FaceRecognitionModel.RECOGNITION04,
    )

    # Define woman friend
    woman = face_admin_client.large_person_group.create_person(
        large_person_group_id=LARGE_PERSON_GROUP_ID,
        name="Woman",
    )
    # Define man friend
    man = face_admin_client.large_person_group.create_person(
        large_person_group_id=LARGE_PERSON_GROUP_ID,
        name="Man",
    )
    # Define child friend
    child = face_admin_client.large_person_group.create_person(
        large_person_group_id=LARGE_PERSON_GROUP_ID,
        name="Child",
    )

    '''
    Detect faces and register them to each person
    '''
    # Find all jpeg images of friends in working directory (TBD pull from web instead)
    woman_images = [
        "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/Family1-Mom1.jpg",
        "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/Family1-Mom2.jpg",
    ]
    man_images = [
        "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/Family1-Dad1.jpg",
        "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/Family1-Dad2.jpg",
    ]
    child_images = [
        "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/Family1-Son1.jpg",
        "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/Family1-Son2.jpg",
    ]

    # Add to woman person
    for image in woman_images:
        # Check if the image is of sufficent quality for recognition.
        sufficient_quality = True
        detected_faces = face_client.detect_from_url(
            url=image,
            detection_model=FaceDetectionModel.DETECTION03,
            recognition_model=FaceRecognitionModel.RECOGNITION04,
            return_face_id=True,
            return_face_attributes=[FaceAttributeTypeRecognition04.QUALITY_FOR_RECOGNITION],
        )
        for face in detected_faces:
            if face.face_attributes.quality_for_recognition != QualityForRecognition.HIGH:
                sufficient_quality = False
                break

        if not sufficient_quality:
            continue

        if len(detected_faces) != 1:
            continue

        face_admin_client.large_person_group.add_face_from_url(
            large_person_group_id=LARGE_PERSON_GROUP_ID,
            person_id=woman.person_id,
            url=image,
            detection_model=FaceDetectionModel.DETECTION03,
        )
        print(f"face {face.face_id} added to person {woman.person_id}")


    # Add to man person
    for image in man_images:
        # Check if the image is of sufficent quality for recognition.
        sufficient_quality = True
        detected_faces = face_client.detect_from_url(
            url=image,
            detection_model=FaceDetectionModel.DETECTION03,
            recognition_model=FaceRecognitionModel.RECOGNITION04,
            return_face_id=True,
            return_face_attributes=[FaceAttributeTypeRecognition04.QUALITY_FOR_RECOGNITION],
        )
        for face in detected_faces:
            if face.face_attributes.quality_for_recognition != QualityForRecognition.HIGH:
                sufficient_quality = False
                break

        if not sufficient_quality:
            continue

        if len(detected_faces) != 1:
            continue

        face_admin_client.large_person_group.add_face_from_url(
            large_person_group_id=LARGE_PERSON_GROUP_ID,
            person_id=man.person_id,
            url=image,
            detection_model=FaceDetectionModel.DETECTION03,
        )
        print(f"face {face.face_id} added to person {man.person_id}")

    # Add to child person
    for image in child_images:
        # Check if the image is of sufficent quality for recognition.
        sufficient_quality = True
        detected_faces = face_client.detect_from_url(
            url=image,
            detection_model=FaceDetectionModel.DETECTION03,
            recognition_model=FaceRecognitionModel.RECOGNITION04,
            return_face_id=True,
            return_face_attributes=[FaceAttributeTypeRecognition04.QUALITY_FOR_RECOGNITION],
        )
        for face in detected_faces:
            if face.face_attributes.quality_for_recognition != QualityForRecognition.HIGH:
                sufficient_quality = False
                break
        if not sufficient_quality:
            continue

        if len(detected_faces) != 1:
            continue

        face_admin_client.large_person_group.add_face_from_url(
            large_person_group_id=LARGE_PERSON_GROUP_ID,
            person_id=child.person_id,
            url=image,
            detection_model=FaceDetectionModel.DETECTION03,
        )
        print(f"face {face.face_id} added to person {child.person_id}")

    '''
    Train LargePersonGroup
    '''
    # Train the large person group and set the polling interval to 5s
    print(f"Train the person group {LARGE_PERSON_GROUP_ID}")
    poller = face_admin_client.large_person_group.begin_train(
        large_person_group_id=LARGE_PERSON_GROUP_ID,
        polling_interval=5,
    )

    poller.wait()
    print(f"The person group {LARGE_PERSON_GROUP_ID} is trained successfully.")

    '''
    Identify a face against a defined LargePersonGroup
    '''
    # Group image for testing against
    test_image = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/identification1.jpg"

    print("Pausing for 60 seconds to avoid triggering rate limit on free account...")
    time.sleep(60)

    # Detect faces
    face_ids = []
    # We use detection model 03 to get better performance, recognition model 04 to support quality for
    # recognition attribute.
    faces = face_client.detect_from_url(
        url=test_image,
        detection_model=FaceDetectionModel.DETECTION03,
        recognition_model=FaceRecognitionModel.RECOGNITION04,
        return_face_id=True,
        return_face_attributes=[FaceAttributeTypeRecognition04.QUALITY_FOR_RECOGNITION],
    )
    for face in faces:
        # Only take the face if it is of sufficient quality.
        if face.face_attributes.quality_for_recognition != QualityForRecognition.LOW:
            face_ids.append(face.face_id)

    # Identify faces
    identify_results = face_client.identify_from_large_person_group(
        face_ids=face_ids,
        large_person_group_id=LARGE_PERSON_GROUP_ID,
    )
    print("Identifying faces in image")
    for identify_result in identify_results:
        if identify_result.candidates:
            print(f"Person is identified for face ID {identify_result.face_id} in image, with a confidence of "
                  f"{identify_result.candidates[0].confidence}.")  # Get topmost confidence score

            # Verify faces
            verify_result = face_client.verify_from_large_person_group(
                face_id=identify_result.face_id,
                large_person_group_id=LARGE_PERSON_GROUP_ID,
                person_id=identify_result.candidates[0].person_id,
            )
            print(f"verification result: {verify_result.is_identical}. confidence: {verify_result.confidence}")
        else:
            print(f"No person identified for face ID {identify_result.face_id} in image.")

    print()

    # Delete the large person group
    face_admin_client.large_person_group.delete(LARGE_PERSON_GROUP_ID)
    print(f"The person group {LARGE_PERSON_GROUP_ID} is deleted.")

    print()
    print("End of quickstart.")

# </snippet_single>