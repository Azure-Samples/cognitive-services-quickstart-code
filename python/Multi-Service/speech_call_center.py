
# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE.md file in the project root for full license information.

import os

from azure.ai.textanalytics import single_analyze_sentiment
import azure.cognitiveservices.speech as speechsdk

'''
Azure Speech recognition and Text Analytics sample.
Performs one-shot speech recognition from the default microphone and then analyzes that response text.

Inlcude these libraries:
pip install --upgrade azure-cognitiveservices-speech
pip install azure-ai-textanalytics

Speech SDK: https://docs.microsoft.com/en-us/python/api/azure-cognitiveservices-speech/?view=azure-python
Text Analytics SDK: https://docs.microsoft.com/en-us/python/api/azure-cognitiveservices-language-textanalytics/?view=azure-python
Text Analytics: https://azuresdkdocs.blob.core.windows.net/$web/python/azure-ai-textanalytics/1.0.0b1/azure.ai.textanalytics.html
'''

# Add your Azure Cognitive Services key and endpoint to your environment variables.
subscription_key = os.environ['COGNITIVE_SERVICES_SUBSCRIPTION_KEY']
endpoint = os.environ['COGNITIVE_SERVICES_ENDPOINT']

# Authenticate, you may need to change the region to your own.
speech_config = speechsdk.SpeechConfig(subscription=subscription_key, region='westus')

# Creates a speech recognizer using microphone as audio input.
# The default language is "en-us".
speech_recognizer = speechsdk.SpeechRecognizer(speech_config=speech_config)

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
analytics_response = single_analyze_sentiment(endpoint=endpoint, credential=subscription_key, input_text=result.text)
print("Document Sentiment: {}".format(analytics_response.sentiment))
print("Overall scores: positive={0:.3f}; neutral={1:.3f}; negative={2:.3f} \n".format(
    analytics_response.document_scores.positive,
    analytics_response.document_scores.neutral,
    analytics_response.document_scores.negative,
))
