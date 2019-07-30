# <snippet_imports>
import os.path
from pprint import pprint
import time
from io import BytesIO
from random import random
import uuid

from azure.cognitiveservices.vision.contentmoderator import ContentModeratorClient
import azure.cognitiveservices.vision.contentmoderator.models
from msrest.authentication import CognitiveServicesCredentials
# </snippet_imports>

# <snippet_vars>
CONTENTMODERATOR_ENDPOINT = "https://westus.api.cognitive.microsoft.com"
subscription_key = os.environ.get("CONTENT_MODERATOR_SUBSCRIPTION_KEY")
# </snippet_vars>

# <snippet_client>
client = ContentModeratorClient(
    endpoint=CONTENT_MODERATOR_ENDPOINT,
    credentials=CognitiveServicesCredentials(subscription_key)
)
# </snippet_client>

# <snippet_textfolder>
TEXT_FOLDER = os.path.join(os.path.dirname(
    os.path.realpath(__file__)), "text_files")
# </snippet_textfolder>

# <snippet_imagemodvars>
IMAGE_LIST = [
    "https://moderatorsampleimages.blob.core.windows.net/samples/sample2.jpg",
    "https://moderatorsampleimages.blob.core.windows.net/samples/sample5.png"
]
# </snippet_imagemodvars>

# <snippet_imagelistvars>
IMAGE_LIST = {
    "Sports": [
        "https://moderatorsampleimages.blob.core.windows.net/samples/sample4.png",
        "https://moderatorsampleimages.blob.core.windows.net/samples/sample6.png",
        "https://moderatorsampleimages.blob.core.windows.net/samples/sample9.png"
    ],
    "Swimsuit": [
        "https://moderatorsampleimages.blob.core.windows.net/samples/sample1.jpg",
        "https://moderatorsampleimages.blob.core.windows.net/samples/sample3.png",
        "https://moderatorsampleimages.blob.core.windows.net/samples/sample4.png",
        "https://moderatorsampleimages.blob.core.windows.net/samples/sample16.png"
    ]
}

IMAGES_TO_MATCH = [
    "https://moderatorsampleimages.blob.core.windows.net/samples/sample1.jpg",
    "https://moderatorsampleimages.blob.core.windows.net/samples/sample4.png",
    "https://moderatorsampleimages.blob.core.windows.net/samples/sample5.png",
    "https://moderatorsampleimages.blob.core.windows.net/samples/sample16.png"
]
# </snippet_imagelistvars>

def text_moderation():
    """TextModeration.
    This will moderate a given long text.
    """

    # <snippet_textmod>
    # Screen the input text: check for profanity,
    # do autocorrect text, and check for personally identifying
    # information (PII)
    with open(os.path.join(TEXT_FOLDER, 'content_moderator_text_moderation.txt'), "rb") as text_fd:
        screen = client.text_moderation.screen_text(
            text_content_type="text/plain",
            text_content=text_fd,
            language="eng",
            autocorrect=True,
            pii=True
        )
        assert isinstance(screen, Screen)
        pprint(screen.as_dict())
# </snippet_textmod>

def terms_lists():
    """TermsList.
    This will screen text using a term list.
    """

    # <snippet_termslist_create>
    #
    # Create list
    #
    print("\nCreating list")
    custom_list = client.list_management_term_lists.create(
        content_type="application/json",
        body={
            "name": "Term list name",
            "description": "Term list description",
        }
    )
    print("List created:")
    assert isinstance(custom_list, TermList)
    pprint(custom_list.as_dict())
    list_id = custom_list.id
    # </snippet_termslist_create>

    # <snippet_termslist_details>
    #
    # Update list details
    #
    print("\nUpdating details for list {}".format(list_id))
    updated_list = client.list_management_term_lists.update(
        list_id=list_id,
        content_type="application/json",
        body={
            "name": "New name",
            "description": "New description"
        }
    )
    assert isinstance(updated_list, TermList)
    pprint(updated_list.as_dict())
    # </snippet_termslist_details>

    # <snippet_termslist_add>
    #
    # Add terms
    #
    print("\nAdding terms to list {}".format(list_id))
    client.list_management_term.add_term(
        list_id=list_id,
        term="term1",
        language="eng"
    )
    client.list_management_term.add_term(
        list_id=list_id,
        term="term2",
        language="eng"
    )
    # </snippet_termslist_add>

    # <snippet_termslist_getterms>
    #
    # Get all terms ids
    #
    print("\nGetting all term IDs for list {}".format(list_id))
    terms = client.list_management_term.get_all_terms(
        list_id=list_id, language="eng")
    assert isinstance(terms, Terms)
    terms_data = terms.data
    assert isinstance(terms_data, TermsData)
    pprint(terms_data.as_dict())
    # </snippet_termslist_getterms>

    # <snippet_termslist_refresh>
    #
    # Refresh the index
    #
    print("\nRefreshing the search index for list {}".format(list_id))
    refresh_index = client.list_management_term_lists.refresh_index_method(
        list_id=list_id, language="eng")
    assert isinstance(refresh_index, RefreshIndex)
    pprint(refresh_index.as_dict())

    print("\nWaiting {} minutes to allow the server time to propagate the index changes.".format(
        LATENCY_DELAY))
    time.sleep(LATENCY_DELAY * 60)
    # </snippet_termslist_refresh>

    # <snippet_termslist_screen>
    #
    # Screen text
    #
    with open(os.path.join(TEXT_FOLDER, 'content_moderator_term_list.txt'), "rb") as text_fd:
        screen = client.text_moderation.screen_text(
            text_content_type="text/plain",
            text_content=text_fd,
            language="eng",
            autocorrect=False,
            pii=False,
            list_id=list_id
        )
        assert isinstance(screen, Screen)
        pprint(screen.as_dict())
    # </snippet_termslist_screen>

    # <snippet_termslist_remove>
    #
    # Remove terms
    #
    term_to_remove = "term1"
    print("\nRemove term {} from list {}".format(term_to_remove, list_id))
    client.list_management_term.delete_term(
        list_id=list_id,
        term=term_to_remove,
        language="eng"
    )
    # </snippet_termslist_remove>

    #
    # Refresh the index
    #
    print("\nRefreshing the search index for list {}".format(list_id))
    refresh_index = client.list_management_term_lists.refresh_index_method(
        list_id=list_id, language="eng")
    assert isinstance(refresh_index, RefreshIndex)
    pprint(refresh_index.as_dict())

    print("\nWaiting {} minutes to allow the server time to propagate the index changes.".format(
        LATENCY_DELAY))
    time.sleep(LATENCY_DELAY * 60)

    #
    # Re-Screen text
    #
    with open(os.path.join(TEXT_FOLDER, 'content_moderator_term_list.txt'), "rb") as text_fd:
        print('\nScreening text "{}" using term list {}'.format(text, list_id))
        screen = client.text_moderation.screen_text(
            text_content_type="text/plain",
            text_content=text_fd,
            language="eng",
            autocorrect=False,
            pii=False,
            list_id=list_id
        )
        assert isinstance(screen, Screen)
        pprint(screen.as_dict())

    # <snippet_termslist_removeall>
    #
    # Delete all terms
    #
    print("\nDelete all terms in the image list {}".format(list_id))
    client.list_management_term.delete_all_terms(
        list_id=list_id, language="eng")
    # </snippet_termslist_removeall>

    # <snippet_termslist_deletelist>
    #
    # Delete list
    #
    print("\nDelete the term list {}".format(list_id))
    client.list_management_term_lists.delete(list_id=list_id)
    # </snippet_termslist_deletelist>


def image_moderation():
    """ImageModeration.
    This will review an image using workflow and job.
    """

    # <snippet_imagemod>
    for image_url in IMAGE_LIST:
        print("\nEvaluate image {}".format(image_url))

        print("\nEvaluate for adult and racy content.")
        evaluation = client.image_moderation.evaluate_url_input(
            content_type="application/json",
            cache_image=True,
            data_representation="URL",
            value=image_url
        )
        assert isinstance(evaluation, Evaluate)
        pprint(evaluation.as_dict())

        print("\nDetect and extract text.")
        evaluation = client.image_moderation.ocr_url_input(
            language="eng",
            content_type="application/json",
            data_representation="URL",
            value=image_url,
            cache_image=True,
        )
        assert isinstance(evaluation, OCR)
        pprint(evaluation.as_dict())

        print("\nDetect faces.")
        evaluation = client.image_moderation.find_faces_url_input(
            content_type="application/json",
            cache_image=True,
            data_representation="URL",
            value=image_url
        )
        assert isinstance(evaluation, FoundFaces)
        pprint(evaluation.as_dict())
# </snippet_imagemod>

def image_lists():
    """ImageList.
    This will review an image using workflow and job.
    """

    # <snippet_imagelist_create>
    #
    # Create list
    #
    print("Creating list MyList\n")
    custom_list = client.list_management_image_lists.create(
        content_type="application/json",
        body={
            "name": "MyList",
            "description": "A sample list",
            "metadata": {
                "key_one": "Acceptable",
                "key_two": "Potentially racy"
            }
        }
    )
    print("List created:")
    assert isinstance(custom_list, ImageList)
    pprint(custom_list.as_dict())
    list_id = custom_list.id
    # </snippet_imagelist_create>

    # <snippet_imagelist_add>
    #
    # Add images
    #
    def add_images(list_id, image_url, label):
        """Generic add_images from url and label."""
        print("\nAdding image {} to list {} with label {}.".format(
            image_url, list_id, label))
        try:
            added_image = client.list_management_image.add_image_url_input(
                list_id=list_id,
                content_type="application/json",
                data_representation="URL",
                value=image_url,
                label=label
            )
        except APIErrorException as err:
            # sample4 will fail
            print("Unable to add image to list: {}".format(err))
        else:
            assert isinstance(added_image, Image)
            pprint(added_image.as_dict())
            return added_image

    print("\nAdding images to list {}".format(list_id))
    index = {}  # Keep an index url to id for later removal
    for label, urls in IMAGE_LIST.items():
        for url in urls:
            image = add_images(list_id, url, label)
            if image:
                index[url] = image.content_id

    # </snippet_imagelist_add>

    # <snippet_imagelist_getimages>
    #
    # Get all images ids
    #
    print("\nGetting all image IDs for list {}".format(list_id))
    image_ids = client.list_management_image.get_all_image_ids(list_id=list_id)
    assert isinstance(image_ids, ImageIds)
    pprint(image_ids.as_dict())
    # </snippet_imagelist_getimages>

    # <snippet_imagelist_updatedetails>
    #
    # Update list details
    #
    print("\nUpdating details for list {}".format(list_id))
    updated_list = client.list_management_image_lists.update(
        list_id=list_id,
        content_type="application/json",
        body={
            "name": "Swimsuits and sports"
        }
    )
    assert isinstance(updated_list, ImageList)
    pprint(updated_list.as_dict())
    # </snippet_imagelist_updatedetails>

    # <snippet_imagelist_getdetails>
    #
    # Get list details
    #
    print("\nGetting details for list {}".format(list_id))
    list_details = client.list_management_image_lists.get_details(
        list_id=list_id)
    assert isinstance(list_details, ImageList)
    pprint(list_details.as_dict())
    # </snippet_imagelist_getdetails>

    # <snippet_imagelist_refresh>
    #
    # Refresh the index
    #
    print("\nRefreshing the search index for list {}".format(list_id))
    refresh_index = client.list_management_image_lists.refresh_index_method(
        list_id=list_id)
    assert isinstance(refresh_index, RefreshIndex)
    pprint(refresh_index.as_dict())

    print("\nWaiting {} minutes to allow the server time to propagate the index changes.".format(
        LATENCY_DELAY))
    time.sleep(LATENCY_DELAY * 60)
    # </snippet_imagelist_refresh>

    # <snippet_imagelist_match>
    #
    # Match images against the image list.
    #
    for image_url in IMAGES_TO_MATCH:
        print("\nMatching image {} against list {}".format(image_url, list_id))
        match_result = client.image_moderation.match_url_input(
            content_type="application/json",
            list_id=list_id,
            data_representation="URL",
            value=image_url,
        )
        assert isinstance(match_result, MatchResponse)
        print("Is match? {}".format(match_result.is_match))
        print("Complete match details:")
        pprint(match_result.as_dict())
    # </snippet_imagelist_match>

    # <snippet_imagelist_remove>
    #
    # Remove images
    #
    correction = "https://moderatorsampleimages.blob.core.windows.net/samples/sample16.png"
    print("\nRemove image {} from list {}".format(correction, list_id))
    client.list_management_image.delete_image(
        list_id=list_id,
        image_id=index[correction]
    )
    # </snippet_imagelist_remove>

    #
    # Refresh the index
    #
    print("\nRefreshing the search index for list {}".format(list_id))
    client.list_management_image_lists.refresh_index_method(list_id=list_id)

    print("\nWaiting {} minutes to allow the server time to propagate the index changes.".format(
        LATENCY_DELAY))
    time.sleep(LATENCY_DELAY * 60)

    #
    # Re-match
    #
    print("\nMatching image. The removed image should not match")
    for image_url in IMAGES_TO_MATCH:
        print("\nMatching image {} against list {}".format(image_url, list_id))
        match_result = client.image_moderation.match_url_input(
            content_type="application/json",
            list_id=list_id,
            data_representation="URL",
            value=image_url,
        )
        assert isinstance(match_result, MatchResponse)
        print("Is match? {}".format(match_result.is_match))
        print("Complete match details:")
        pprint(match_result.as_dict())
    # <snippet_imagelist_removeall>
    #
    # Delete all images
    #
    print("\nDelete all images in the image list {}".format(list_id))
    client.list_management_image.delete_all_images(list_id=list_id)
    # </snippet_imagelist_removeall>

    # <snippet_imagelist_delete>
    #
    # Delete list
    #
    print("\nDelete the image list {}".format(list_id))
    client.list_management_image_lists.delete(list_id=list_id)
    # </snippet_imagelist_delete>

    #
    # Get all list ids
    #
    print("\nVerify that the list {} was deleted.".format(list_id))
    image_lists = client.list_management_image_lists.get_all_image_lists()
    assert not any(list_id == image_list.id for image_list in image_lists)

# <snippet_imagereview1>
def image_review(subscription_key):
    """ImageReview.
    This will create a review for images.
    """

    # The name of the team to assign the job to.
    # This must be the team name you used to create your Content Moderator account. You can
    # retrieve your team name from the Review tool web site. Your team name is the Id
    # associated with your subscription.
    team_name = "<insert your team name here>"

    # An image to review
    image_url = "https://moderatorsampleimages.blob.core.windows.net/samples/sample5.png"

    # Where you want to receive the approval/refuse event. This is the only way to get this information.
    call_back_endpoint = "https://requestb.in/qmsakwqm"
# </snippet_imagereview1>
# <snippet_imagereview2>
    # Create review
    print("Create review for {}.\n".format(image_url))
    review_item = {
        "type": "Image",             # Possible values include: 'Image', 'Text'
        "content": image_url,        # How to download the image
        "content_id": uuid.uuid4(),  # Random id
        "callback_endpoint": call_back_endpoint,
        "metadata": [{
            "key": "sc",
            "value": True  # will be sent to Azure as "str" cast.
        }]
    }

    reviews = client.reviews.create_reviews(
        url_content_type="application/json",
        team_name=team_name,
        create_review_body=[review_item]  # As many review item as you need
    )

    # Get review ID
    review_id = reviews[0]  # Ordered list of string of review ID

    print("\nGet review details")
    review_details = client.reviews.get_review(
        team_name=team_name, review_id=review_id)
    pprint(review_details.as_dict())

    # wait for user input through the Review tool web portal
    input("\nPerform manual reviews on the Content Moderator Review Site, and hit enter here.")

    # Check the results of the human review
    print("\nGet review details")
    review_details = client.reviews.get_review(
        team_name=team_name, review_id=review_id)
    pprint(review_details.as_dict())
# </snippet_imagereview2>
