import os
import azure.cognitiveservices.speech as speechsdk

'''
This Speech quickstart displays text captured from an audio file.

Prerequisites:
  - Add your Cognitive Services or Speech subscription key to your environment variables.
  - Replace the service region from either the Cognitive Services or Speech endpoint, 
    (key and region must be from the same service).
  - Install the Speech SDK module:
    pip install azure.cognitiveservices.speech 
  - Use your own .wav file or download a sample one from here:
    https://github.com/Azure-Samples/cognitive-services-sample-data-files
  - Place audio file in your root folder.

Python SDK: https://docs.microsoft.com/en-us/python/api/azure-cognitiveservices-speech/?view=azure-python
'''

# Creates an instance of a speech config with specified subscription key and service region.
speech_key = 'PASTE_YOUR_SPEECH_SUBSCRIPTION_KEY_HERE'
# Use the region from your Speech endpoint (e.g., 'westus', 'eastus', etc.)
service_region = 'PASTE_YOUR_SPEECH_ENDPOINT_REGION_HERE'
speech_config = speechsdk.SpeechConfig(subscription=speech_key, region=service_region)

# Creates an audio configuration that points to an audio file.
# Alternatively, replace with your own audio filename.
audio_filename = "whatstheweatherlike.wav"
audio_input = speechsdk.AudioConfig(filename=audio_filename)

# Creates a recognizer with the given settings
speech_recognizer = speechsdk.SpeechRecognizer(speech_config=speech_config, audio_config=audio_input)

print("Recognizing first result...")

# Starts speech recognition, and returns after a single utterance is recognized. The end of a
# single utterance is determined by listening for silence at the end or until a maximum of 15
# seconds of audio is processed.  The task returns the recognition text as result.
# Note: Since recognize_once() returns only a single utterance, it is suitable only for single
# shot recognition like command or query.
# For long-running multi-utterance recognition, use start_continuous_recognition() instead.
result = speech_recognizer.recognize_once()

# Checks result.
if result.reason == speechsdk.ResultReason.RecognizedSpeech:
    print("Recognized: {}".format(result.text))
elif result.reason == speechsdk.ResultReason.NoMatch:
    print("No speech could be recognized: {}".format(result.no_match_details))
elif result.reason == speechsdk.ResultReason.Canceled:
    cancellation_details = result.cancellation_details
    print("Speech Recognition canceled: {}".format(cancellation_details.reason))
    if cancellation_details.reason == speechsdk.CancellationReason.Error:
        print("Error details: {}".format(cancellation_details.error_details))
