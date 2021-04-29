import glob
import os
import time

import azure.cognitiveservices.speech as speechsdk
from msrest.authentication import CognitiveServicesCredentials
from azure.cognitiveservices.vision.computervision import ComputerVisionClient

'''
This sample uses the Computer Vision and Speech Service SDKs from Cognitive Services.
With local images of license plates, a user can enter a state abbreviation into the command line,
and get back the plate number from that state. There are spoken (text-To-Speech) prompts along the way.

Install the SDKs for both Computer Vision and Speech Service:
  pip install --upgrade azure-cognitiveservices-vision-computervision
  pip install --upgrade azure-cognitiveservices-speech
  
Download the license plate images and create a folder for them called 'License Plates':
https://github.com/Azure-Samples/cognitive-services-sample-data-files/tree/master/Multi-service

References:
Computer Vision SDK: https://docs.microsoft.com/en-us/python/api/azure-cognitiveservices-vision-computervision/?view=azure-python
Computer Vision documentation: https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/
Computer Vision API: https://westus.dev.cognitive.microsoft.com/docs/services/5cd27ec07268f6c679a3e641/operations/56f91f2e778daf14a499f21b
Speech SDK: https://docs.microsoft.com/en-us/python/api/azure-cognitiveservices-speech/azure.cognitiveservices.speech?view=azure-python
Speech SDK documentation: https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/
Speech API: https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/rest-text-to-speech
'''

# Add all license plate images to this local folder
plates_folder = 'License Plates\\'

computer_vision_subscription_key = 'PASTE_YOUR_COMPUTER_VISION_SUBSCRIPTION_KEY_HERE'
computer_vision_endpoint = 'PASTE_YOUR_COMPUTER_VISION_ENDPOINT_HERE'

speech_subscription_key = 'PASTE_YOUR_SPEECH_SUBSCRIPTION_KEY_HERE'
speech_region = 'westus' # Set this to the region for your Speech endpoint

# List of all 50 states for reference
# states = ['Alabama', 'Alaska', 'Arizona', 'Arkansas', 'California', 'Colorado', 'Connecticut', 'Delaware', 'Florida', 'Georgia', 'Hawaii', 'Idaho', 'Illinois', 'Indiana', 'Iowa', 'Kansas', 'Kentucky', 'Louisiana', 'Maine', 'Maryland', 'Massachusetts', 'Michigan', 'Minnesota', 'Mississippi', 'Missouri', 'Montana', 'Nebraska', 'Nevada', 'New Hampshire', 'New Jersey', 'New Mexico', 'New York', 'North Carolina', 'North Dakota', 'Ohio', 'Oklahoma', 'Oregon', 'Pennsylvania', 'Rhode Island', 'South Carolina', 'South Dakota', 'Tennessee', 'Texas', 'Utah', 'Vermont', 'Virginia', 'Washington', 'West Virginia', 'Wisconsin', 'Wyoming']

# States we have plates for
states = {'AK': 'Alaska', 'IL': 'Illinois', 'MI': 'Michigan', 'PA': 'Pennsylvania', 'AZ': 'Arizona'}

# Initialize a Speech client
speech_config = speechsdk.SpeechConfig(subscription=speech_subscription_key, region=speech_region)
# Creates a speech synthesizer using the default speaker as audio output.
speech_synthesizer = speechsdk.SpeechSynthesizer(speech_config=speech_config)
# Initialize a Computer Vision client
computer_vision_client = ComputerVisionClient(computer_vision_endpoint, CognitiveServicesCredentials(computer_vision_subscription_key))


'''
Use Computer Vision to get text from an image
'''                         
 # Returns the license plate number               
def text_from_image(image):
    # Call API to get text from image
    plate = open(image, 'rb')
    rawResponse = computer_vision_client.read_in_stream(plate, raw=True)

    # Get ID from returned headers
    operationLocation = rawResponse.headers["Operation-Location"]
    operationId = os.path.basename(operationLocation)

    # SDK call that gets what is read
    results = None
    while True:
        # Returns a ReadOperationResult
        results = computer_vision_client.get_read_result(operationId)
        if results.status.lower () not in ['notstarted', 'running']:
            break
        print ('Waiting for read result...')
        time.sleep(10)

    # Get the state from the 1st line of text that's read
    for result in results.recognition_results:
        for word in result.lines[0].words:
            state_name = states[os.path.basename(image[:-4]).upper()] # use abbreviation to get full state name
            if word.text.lower() == state_name.lower():
                # Read license number from 2nd line of text
                plate_number = result.lines[1].words[0]
                plate_found = 'The plate number for the state of ' + state_name + ' is ' + plate_number.text + '.'
                print(plate_found)
                return plate_found
    

'''
Use Speech Service covert text into speech
'''
def text_to_speech(text):
    # Synthesizes the received text to speech.
    # The synthesized speech is expected to be heard on the speaker with this line executed.
    result = speech_synthesizer.speak_text_async(text).get()

    # Checks result.
    if result.reason == speechsdk.ResultReason.SynthesizingAudioCompleted:
        # Optional -- print message to user upon success
        print()
    elif result.reason == speechsdk.ResultReason.Canceled:
        cancellation_details = result.cancellation_details
        print("Speech synthesis canceled: {}".format(cancellation_details.reason))
        if cancellation_details.reason == speechsdk.CancellationReason.Error:
            if cancellation_details.error_details:
                print("Error details: {}".format(cancellation_details.error_details))
        print("Did you update the subscription info?")


'''
Opens the license plate image to get the number
'''
def get_number(plates, chosen_state):
    # Get the license plate number
    for plate in plates:
        image_path = plate
        plate = os.path.basename(plate) # rename to match user input name
        if (chosen_state + '.jpg') == plate:
            number_found = text_from_image(image_path)
            text_to_speech(number_found)

            while True:
                # Get another plate number or quit
                try_again = 'Would you like to find another license plate number? Y or N'
                print(try_again)
                text_to_speech(try_again)
                # User responds
                answer = input().lower()
                if answer == 'n':
                    return False
                elif answer == 'y':
                    return True

def request_state(image_plates):
    while True:
        print()
        # User types a state they want a license plate number for
        your_state = 'Type the state abbreviation you\'re looking for: AK, IL, MI, PA, or AZ.'
        print(your_state)
        text_to_speech(your_state)

        # Get the user's state request
        chosen_state = input().lower() 
        print()

        # Check they typed a state from an existing states list
        for s in states.keys():
            s = s.lower()
            if s == chosen_state:
                # Find the chosen state (converts to full state name) that matches a plate
                keep_going = get_number(image_plates, chosen_state) 
                if not keep_going:
                    return
                else:
                    break


if __name__ == "__main__":
    # Get all plates from images directory
    image_plates = [plate for plate in glob.glob(plates_folder + '*')]

    print('Welcome to the license plate retrieval system.')

    request_state(image_plates)
   
    print()
    print('Finished.')
    text_to_speech('Finished')
