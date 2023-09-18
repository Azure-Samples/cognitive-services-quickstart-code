import time, os
from azure.cognitiveservices.vision.computervision import ComputerVisionClient
from msrest.authentication import CognitiveServicesCredentials
from azure.cognitiveservices.vision.computervision.models import OperationStatusCodes

'''
Extract Text PDF - Computer Vision API
This sample will extract printed and handwritten text from images in a PDF.
The images include both printed and handwritten text, including signatures.        

Download the sample PDF here: 
https://github.com/Azure-Samples/cognitive-services-sample-data-files/blob/master/ComputerVision/Images/printed_handwritten.pdf

Place the PDF in your working directory.

Install the Computer Vision SDK:
pip install --upgrade azure-cognitiveservices-vision-computervision

Steps:
   - Authenticate
   - Read and extract from PDF
   - Display extracted text results in console

References: 
Computer Vision Batch Read File documentation: 
https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/concept-recognizing-text
SDK: 
https://docs.microsoft.com/en-us/python/api/azure-cognitiveservices-vision-computervision/azure.cognitiveservices.vision.computervision?view=azure-python
API: 
https://westus.dev.cognitive.microsoft.com/docs/services/5cd27ec07268f6c679a3e641/operations/56f91f2e778daf14a499f21b 
'''

'''
Authenticate
'''
key = 'PASTE_YOUR_COMPUTER_VISION_KEY_HERE'
endpoint = 'PASTE_YOUR_COMPUTER_VISION_ENDPOINT_HERE'

# Set credentials
credentials = CognitiveServicesCredentials(key)
# Create client
client = ComputerVisionClient(endpoint, credentials)


'''
Read and extract from the image
'''
def pdf_text():
    # Images PDF with text
    filepath = open('printed_handwritten.pdf','rb')

    # Async SDK call that "reads" the image
    response = client.read_in_stream(filepath, raw=True)

    # Don't forget to close the file
    filepath.close()

    # Get ID from returned headers
    operation_location = response.headers["Operation-Location"]
    operation_id = operation_location.split("/")[-1]

    # SDK call that gets what is read
    while True:
        result = client.get_read_result(operation_id)
        if result.status.lower () not in ['notstarted', 'running']:
            break
        print ('Waiting for result...')
        time.sleep(10)
    return result


'''
Display extracted text and bounding box
'''
# Displays text captured and its bounding box (position in the image)
result = pdf_text()
if result.status == OperationStatusCodes.succeeded:
    for readResult in result.analyze_result.read_results:
        for line in readResult.lines:
            print(line.text)
            print(line.bounding_box)
        print()
