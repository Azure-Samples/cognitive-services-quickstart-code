
# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE.md file in the project root for full license information.

import os

from azure.cognitiveservices.language.textanalytics import TextAnalyticsClient
import azure.cognitiveservices.speech as speechsdk
from msrest.authentication import CognitiveServicesCredentials

'''
Azure Speech recognition and Text Analytics sample.
Performs one-shot speech recognition from the default microphone and then analyzes that response text.

Inlcude these libraries:
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

# Starts speech recognition, and returns after a single utterance is recognized.
# For long-running multi-utterance recognition, use start_continuous_recognition() instead.
print('Speak a phrase into your microphone...')
result = speech_recognizer.recognize_once()
print()

# Check the result
if result.reason == speechsdk.ResultReason.RecognizedSpeech:
    print("Recognized: {}".format(result.text))
elif result.reason == speechsdk.ResultReason.NoMatch:
    print("No speech could be recognized")
elif result.reason == speechsdk.ResultReason.Canceled:
    cancellation_details = result.cancellation_details
    print("Speech Recognition canceled: {}".format(
        cancellation_details.reason))
    if cancellation_details.reason == speechsdk.CancellationReason.Error:
        print("Error details: {}".format(
            cancellation_details.error_details))
print()

# Text Analytics, analyze the Speech to Text response in terms of sentiment.
documents = [{"id": "1", "language": "en", "text": result.text}]
analytics_response = text_analytics_client.sentiment(documents)
print("Document Sentiment: {}".format(analytics_response.sentiment))
print("Overall scores: positive={0:.3f}; neutral={1:.3f}; negative={2:.3f} \n".format(
    analytics_response.document_scores.positive,
    analytics_response.document_scores.neutral,
    analytics_response.document_scores.negative,
))
