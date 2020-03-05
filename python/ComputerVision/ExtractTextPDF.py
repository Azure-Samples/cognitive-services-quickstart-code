import time, os
from azure.cognitiveservices.vision.computervision import ComputerVisionClient
from msrest.authentication import CognitiveServicesCredentials
from azure.cognitiveservices.vision.computervision.models import TextRecognitionMode
from azure.cognitiveservices.vision.computervision.models import TextOperationStatusCodes

'''
Extract Text - Comptuper Vision API
This sample will extract printed and handwritten text from an image.
Steps:
   - Authenticate
   - Read and extract from PDF
   - Display extracted text results in console
'''

'''
Authenticate
'''
# Set COMPUTER_VISION_SUBSCRIPTION_KEY in your environment variables with your Face key as a value.
# Set COMPUTER_VISION_REGION in your environment variables.
key = os.environ['COMPUTER_VISION_SUBSCRIPTION_KEY']
endpoint = os.environ['COMPUTER_VISION_ENDPOINT']

# Set credentials
credentials = CognitiveServicesCredentials(key)
# Create client
client = ComputerVisionClient(endpoint, credentials)


'''
Read and extract from the image
'''
def pdf_text():
    # Images PDF with text
    filepath = open('TextImages.pdf','rb')

    # Async SDK call that "reads" the image
    response = client.batch_read_file_in_stream(filepath, raw=True)

    # Don't forget to close the file
    filepath.close()

    # Get ID from returned headers
    operation_location = response.headers["Operation-Location"]
    operation_id = operation_location.split("/")[-1]

    # SDK call that gets what is read
    while True:
        result = client.get_read_operation_result(operation_id)
        if result.status not in ['NotStarted', 'Running']:
            break
        time.sleep(1)
    return result


'''
Display extracted text and bounding box
'''
# Displays text captured and its bounding box (position in the image)
result = pdf_text()
if result.status == TextOperationStatusCodes.succeeded:
    for textResult in result.recognition_results:
        for line in textResult.lines:
            print(line.text)
            print(line.bounding_box)
        print()
