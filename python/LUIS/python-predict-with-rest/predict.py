########### Python 3.6 #############

#
# This quickstart shows how to predict the intent of an utterance by using the LUIS REST APIs.
#

import requests

try:

    ##########
    # Values to modify.

    # YOUR-APP-ID: The App ID GUID found on the www.luis.ai Application Settings page.
    appId = 'YOUR-APP-ID'

    # YOUR-PREDICTION-KEY: Your LUIS authoring key, 32 character value.
    prediction_key = 'YOUR-PREDICTION-KEY'

    # YOUR-PREDICTION-ENDPOINT: Replace with your authoring key endpoint.
    # For example, "https://westus.api.cognitive.microsoft.com/"
    prediction_endpoint = 'https://YOUR-PREDICTION-ENDPOINT/'

    # The utterance you want to use.
    utterance = 'I want two large pepperoni pizzas on thin crust please'
    ##########

    # The headers to use in this REST call.
    headers = {
    }

    # The URL parameters to use in this REST call.
    params ={
        'query': utterance,
        'timezoneOffset': '0',
        'verbose': 'true',
        'show-all-intents': 'true',
        'spellCheck': 'false',
        'staging': 'false',
        'subscription-key': prediction_key
    }


    # Make the REST call.
    response = requests.get(f'{prediction_endpoint}luis/prediction/v3.0/apps/{appId}/slots/production/predict', headers=headers, params=params)

    # Display the results on the console.
    print(response.json())


except Exception as e:
    # Display the error string.
    print(f'{e}')