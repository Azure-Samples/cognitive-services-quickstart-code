import os, time

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

# Add all license plate images to this local folder. You can download license plate images from:
# https://github.com/Azure-Samples/cognitive-services-sample-data-files/tree/master/Multi-service
plates_folder = 'License_Plates'

computer_vision_subscription_key = 'PASTE_YOUR_COMPUTER_VISION_SUBSCRIPTION_KEY_HERE'
computer_vision_endpoint = 'PASTE_YOUR_COMPUTER_VISION_ENDPOINT_HERE'

speech_subscription_key = 'PASTE_YOUR_SPEECH_SUBSCRIPTION_KEY_HERE'
speech_region = 'westus' # Set this to the region for your Speech endpoint

# States we have plates for
available_states = {'ak': 'Alaska', 'il': 'Illinois', 'mi': 'Michigan', 'pa': 'Pennsylvania', 'az': 'Arizona'}

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
def text_from_image(image_path):
	# Call API to get text from image
	with open(image_path, 'rb') as f :
		rawResponse = computer_vision_client.read_in_stream(f, raw=True)

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
	for read_result in results.analyze_result.read_results:
		for line in read_result.lines:
			print (line.text)
	print ()

'''
Use Speech Service covert text into speech
'''
def text_to_speech(text):
# Remove this statement to enable text to speech.
	return

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
def get_number(requested_state):
	current_dir = os.path.dirname(os.path.abspath(__file__))
	image_path = os.path.join (current_dir, plates_folder, requested_state + '.jpg')
	number_found = text_from_image(image_path)
	text_to_speech(number_found)

def request_state():
	print()
	# User types a state they want a license plate number for
	your_state = 'Type the state abbreviation you\'re looking for: AK, IL, MI, PA, or AZ.'
	print(your_state)
	text_to_speech(your_state)

	# Get the user's state request
	requested_state = input().lower() 
	print()

	# Check they typed a state from an existing states list
	if requested_state in available_states.keys() :
		# Find the chosen state (converts to full state name) that matches a plate
		get_number(requested_state) 
	else :
		response = 'Sorry, we do not have a license plate for that state.'
		print(response)
		text_to_speech(response)

if __name__ == "__main__":
	print('Welcome to the license plate retrieval system.')

	done = False
	while not done :
		request_state()

		try_again = 'Would you like to find another license plate number? Y or N'
		print(try_again)
		text_to_speech(try_again)
		answer = input().lower()
		if 'y' != answer : done = True
   
	print()
	print('Finished.')
	text_to_speech('Finished')
