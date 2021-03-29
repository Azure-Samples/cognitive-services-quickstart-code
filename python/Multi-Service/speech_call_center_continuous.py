
# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE.md file in the project root for full license information.

import os
import time

from azure.cognitiveservices.language.textanalytics import TextAnalyticsClient
import azure.cognitiveservices.speech as speechsdk
from msrest.authentication import CognitiveServicesCredentials

'''
Azure Speech recognition and Text Analytics sample.
Performs continuous speech recognition from the default computer microphone and then analyzes that response text.
To test, run from command line with microphone ready. To stop, let 15 seconds of silence go by.

Install these libraries:
pip install --upgrade azure-cognitiveservices-speech
pip install azure-cognitiveservices-language-textanalytics

Speech SDK: https://docs.microsoft.com/en-us/python/api/azure-cognitiveservices-speech/?view=azure-python
Text Analytics SDK: https://docs.microsoft.com/en-us/python/api/azure-cognitiveservices-language-textanalytics/?view=azure-python
Text Analytics: https://azuresdkdocs.blob.core.windows.net/$web/python/azure-ai-textanalytics/1.0.0b1/azure.ai.textanalytics.html
'''

speech_subscription_key = 'PASTE_YOUR_SPEECH_SUBSCRIPTION_KEY_HERE'
# Set this to the region for your Speech resource (for example, westus, eastus, and so on).
speech_region = 'westus'

text_analytics_endpoint = 'PASTE_YOUR_TEXT_ANALYTICS_ENDPOINT_HERE'
text_analytics_subscription_key = 'PASTE_YOUR_TEXT_ANALYTICS_SUBSCRIPTION_KEY_HERE'

# Authenticate, you may need to change the region to your own.
speech_config = speechsdk.SpeechConfig(subscription=speech_subscription_key, region=speech_region)

# Creates a speech recognizer using a microphone as audio input.
# The default language is "en-us".
speech_recognizer = speechsdk.SpeechRecognizer(speech_config=speech_config)

text_analytics_client = TextAnalyticsClient(text_analytics_endpoint, CognitiveServicesCredentials(text_analytics_subscription_key))

# Used to stop the continuous speech to text
done = False
# Closes the continuous recognition of speech channel
def stop_cb(evt):
    # Callback that stops continuous recognition upon receiving an event `evt`
    #print('CLOSING on {}'.format(evt))
    speech_recognizer.stop_continuous_recognition()
    global done 
    done = True

# Takes the text result from speech and applies analysis on it for sentiment
def process_results(evt):
    # Check the result
    if evt.result.reason == speechsdk.ResultReason.RecognizedSpeech:
        print("Recognized: {}".format(evt.result.text))
        print()

        # Text Analytics, analyze the Speech to Text response in terms of sentiment.
        documents = [{"id": "1", "language": "en", "text": evt.result.text}]
        analytics_response = text_analytics_client.sentiment(documents)
        print("Document Sentiment: {}".format(analytics_response.sentiment))
        print("Overall scores: positive={0:.3f}; neutral={1:.3f}; negative={2:.3f} \n".format(
            analytics_response.document_scores.positive,
            analytics_response.document_scores.neutral,
            analytics_response.document_scores.negative,
            )
        )
        return evt.result.text 
    elif evt.result.reason == speechsdk.ResultReason.NoMatch:
        print("No speech could be recognized")
        print('Closing...')
        # The stop function is called when silent for 15 seconds.
        return stop_cb(evt)
    elif evt.result.reason == speechsdk.ResultReason.Canceled:
        cancellation_details = evt.result.cancellation_details
        print("Speech Recognition canceled: {}".format(
            cancellation_details.reason))
        if cancellation_details.reason == speechsdk.CancellationReason.Error:
            print("Error details: {}".format(
                cancellation_details.error_details))
print()

# Start the recognition of the continuous speech process
def speech_recognize_continuous_from_mic():
    # Register the connect callbacks to the events fired by the speech recognizer
    speech_recognizer.recognized.connect(lambda evt: process_results(evt))
    speech_recognizer.session_started.connect(lambda evt: process_results(evt))
    speech_recognizer.session_stopped.connect(lambda evt: stop_cb(evt))
    speech_recognizer.canceled.connect(lambda evt: stop_cb(evt))

    # This is where we start continuous speech recognition, after the above registering is done.
    print('Speak a phrase into your microphone...')
    speech_recognizer.start_continuous_recognition()

    global done
    while not done:
        time.sleep(.5)

speech_recognize_continuous_from_mic()
