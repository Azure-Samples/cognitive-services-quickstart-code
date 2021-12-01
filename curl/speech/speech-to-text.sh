# <request>
curl --location --request POST 'https://INSERT_REGION_HERE.stt.speech.microsoft.com/speech/recognition/conversation/cognitiveservices/v1?language=en-US' \
--header 'Ocp-Apim-Subscription-Key: INSERT_SUBSCRIPTION_KEY_HERE' \
--header 'Content-Type: audio/wav' \
--data-binary @'INSERT_AUDIO_FILE_PATH_HERE'
# </request>

# <response>
{
    "RecognitionStatus": "Success",
    "DisplayText": "My voice is my passport, verify me.",
    "Offset": 6600000,
    "Duration": 32100000
}
# </response>