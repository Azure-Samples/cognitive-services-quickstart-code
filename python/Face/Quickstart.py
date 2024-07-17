# <snippet_single>
import os
import time
import uuid
import requests

from azure.core.credentials import AzureKeyCredential
from azure.ai.vision.face import FaceClient
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
# PERSON_GROUP_ID should be all lowercase and alphanumeric. For example, 'mygroupname' (dashes are OK).
PERSON_GROUP_ID = str(uuid.uuid4())  # assign a random ID (or name it anything)

HEADERS = {"Ocp-Apim-Subscription-Key": KEY, "Content-Type": "application/json"}

# Create an authenticated FaceClient.
with FaceClient(endpoint=ENDPOINT, credential=AzureKeyCredential(KEY)) as face_client:
    '''
    Create the PersonGroup
    '''
    # Create empty Person Group. Person Group ID must be lower case, alphanumeric, and/or with '-', '_'.
    print("Person group:", PERSON_GROUP_ID)
    response = requests.put(
        ENDPOINT + f"/face/v1.0/largepersongroups/{PERSON_GROUP_ID}",
        headers=HEADERS,
        json={"name": PERSON_GROUP_ID, "recognitionModel": "recognition_04"})
    response.raise_for_status()

    # Define woman friend
    response = requests.post(ENDPOINT + f"/face/v1.0/largepersongroups/{PERSON_GROUP_ID}/persons", headers=HEADERS, json={"name": "Woman"})
    response.raise_for_status()
    woman = response.json()
    # Define man friend
    response = requests.post(ENDPOINT + f"/face/v1.0/largepersongroups/{PERSON_GROUP_ID}/persons", headers=HEADERS, json={"name": "Man"})
    response.raise_for_status()
    man = response.json()
    # Define child friend
    response = requests.post(ENDPOINT + f"/face/v1.0/largepersongroups/{PERSON_GROUP_ID}/persons", headers=HEADERS, json={"name": "Child"})
    response.raise_for_status()
    child = response.json()

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

        if not sufficientQuality:
            continue

        if len(detected_faces) != 1:
            continue

        response = requests.post(
            ENDPOINT + f"/face/v1.0/largepersongroups/{PERSON_GROUP_ID}/persons/{woman['personId']}/persistedFaces",
            headers=HEADERS,
            json={"url": image})
        response.raise_for_status()
        print(f"face {face.face_id} added to person {woman['personId']}")


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

        if not sufficientQuality:
            continue

        if len(detected_faces) != 1:
            continue

        response = requests.post(
            ENDPOINT + f"/face/v1.0/largepersongroups/{PERSON_GROUP_ID}/persons/{man['personId']}/persistedFaces",
            headers=HEADERS,
            json={"url": image})
        response.raise_for_status()
        print(f"face {face.face_id} added to person {man['personId']}")

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
                break
        if not sufficientQuality:
            continue

        if len(detected_faces) != 1:
            continue

        response = requests.post(
            ENDPOINT + f"/face/v1.0/largepersongroups/{PERSON_GROUP_ID}/persons/{child['personId']}/persistedFaces",
            headers=HEADERS,
            json={"url": image})
        response.raise_for_status()
        print(f"face {face.face_id} added to person {child['personId']}")

    '''
    Train PersonGroup
    '''
    # Train the person group
    print(f"Train the person group {PERSON_GROUP_ID}")
    response = requests.post(ENDPOINT + f"/face/v1.0/largepersongroups/{PERSON_GROUP_ID}/train", headers=HEADERS)
    response.raise_for_status()

    while (True):
        response = requests.get(ENDPOINT + f"/face/v1.0/largepersongroups/{PERSON_GROUP_ID}/training", headers=HEADERS)
        response.raise_for_status()
        training_status = response.json()["status"]
        if training_status == "succeeded":
            break
    print(f"The person group {PERSON_GROUP_ID} is trained successfully.")

    '''
    Identify a face against a defined PersonGroup
    '''
    # Group image for testing against
    test_image = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/Face/images/identification1.jpg"  # noqa: E501

    print("Pausing for 60 seconds to avoid triggering rate limit on free account...")
    time.sleep(60)

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
        if face.face_attributes.quality_for_recognition != QualityForRecognition.LOW:
            face_ids.append(face.face_id)

    # Identify faces
    response = requests.post(
        ENDPOINT + f"/face/v1.0/identify",
        headers=HEADERS,
        json={"faceIds": face_ids, "largePersonGroupId": PERSON_GROUP_ID})
    response.raise_for_status()
    results = response.json()
    print("Identifying faces in image")
    if not results:
        print("No person identified in the person group")
    for identifiedFace in results:
        if len(identifiedFace["candidates"]) > 0:
            print(f"Person is identified for face ID {identifiedFace['faceId']} in image, with a confidence of "
                  f"{identifiedFace['candidates'][0]['confidence']}.")  # Get topmost confidence score

            # Verify faces
            response = requests.post(
                ENDPOINT + f"/face/v1.0/verify",
                headers=HEADERS,
                json={"faceId": identifiedFace["faceId"], "personId": identifiedFace["candidates"][0]["personId"], "largePersonGroupId": PERSON_GROUP_ID})
            response.raise_for_status()
            verify_result = response.json()
            print(f"verification result: {verify_result['isIdentical']}. confidence: {verify_result['confidence']}")
        else:
            print(f"No person identified for face ID {identifiedFace['faceId']} in image.")

    print()

    # Delete the person group
    response = requests.delete(ENDPOINT + f"/face/v1.0/largepersongroups/{PERSON_GROUP_ID}", headers=HEADERS)
    response.raise_for_status()
    print(f"The person group {PERSON_GROUP_ID} is deleted.")

    print()
    print("End of quickstart.")

# </snippet_single>