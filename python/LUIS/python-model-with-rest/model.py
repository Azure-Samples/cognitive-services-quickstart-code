########### Python 3.6 #############

#
# This quickstart shows how to add utterances to a LUIS model using the REST APIs.
#

import requests

try:

    ##########
    # Values to modify.

    # YOUR-APP-ID: The App ID GUID found on the www.luis.ai Application Settings page.
    appId = "YOUR-APP-ID"

    # YOUR-AUTHORING-KEY: Your LUIS authoring key, 32 character value.
    authoring_key = "YOUR-AUTHORING-KEY"

    # YOUR-AUTHORING-ENDPOINT: Replace this with your authoring key endpoint.
    # For example, "https://your-resource-name.cognitiveservices.azure.com/"
    authoring_endpoint = "https://YOUR-AUTHORING-ENDPOINT/"

    # The version number of your LUIS app
    app_version = "0.1"
    ##########

    # The headers to use in this REST call.
    headers = {'Ocp-Apim-Subscription-Key': authoring_key}

    # The URL parameters to use in this REST call.
    params ={
        #'timezoneOffset': '0',
        #'verbose': 'true',
        #'show-all-intents': 'true',
        #'spellCheck': 'false',
        #'staging': 'false'
    }

    # List of example utterances to send to the LUIS app.
    data = """[
    {
        'text': 'order a pizza',
        'intentName': 'ModifyOrder',
        'entityLabels': [
            {
                'entityName': 'Order',
                'startCharIndex': 6,
                'endCharIndex': 12
            }
        ]
    },
    {
        'text': 'order a large pepperoni pizza',
        'intentName': 'ModifyOrder',
        'entityLabels': [
            {
                'entityName': 'Order',
                'startCharIndex': 6,
                'endCharIndex': 28
            },
            {
                'entityName': 'FullPizzaWithModifiers',
                'startCharIndex': 6,
                'endCharIndex': 28
            },
            {
                'entityName': 'PizzaType',
                'startCharIndex': 14,
                'endCharIndex': 28
            },
            {
                'entityName': 'Size',
                'startCharIndex': 8,
                'endCharIndex': 12
            }
        ]
    },
    {
        'text': 'I want two large pepperoni pizzas on thin crust',
        'intentName': 'ModifyOrder',
        'entityLabels': [
            {
                'entityName': 'Order',
                'startCharIndex': 7,
                'endCharIndex': 46
            },
            {
                'entityName': 'FullPizzaWithModifiers',
                'startCharIndex': 7,
                'endCharIndex': 46
            },
            {
                'entityName': 'PizzaType',
                'startCharIndex': 17,
                'endCharIndex': 32
            },
            {
                'entityName': 'Size',
                'startCharIndex': 11,
                'endCharIndex': 15
            },
            {
                'entityName': 'Quantity',
                'startCharIndex': 7,
                'endCharIndex': 9
            },
            {
                'entityName': 'Crust',
                'startCharIndex': 37,
                'endCharIndex': 46
            }
        ]
    }
]
"""


    # Make the REST call to POST the list of example utterances.
    response = requests.post(f'{authoring_endpoint}luis/authoring/v3.0-preview/apps/{appId}/versions/{app_version}/examples',
        headers=headers, params=params, data=data)

    # Display the results on the console.
    print('Add the list of utterances:')
    print(response.json())


    # Make the REST call to initiate a training session.
    response = requests.post(f'{authoring_endpoint}luis/authoring/v3.0-preview/apps/{appId}/versions/{app_version}/train',
        headers=headers, params=params, data=None)

    # Display the results on the console.
    print('Request training:')
    print(response.json())


    # Make the REST call to request the status of training.
    response = requests.get(f'{authoring_endpoint}luis/authoring/v3.0-preview/apps/{appId}/versions/{app_version}/train',
        headers=headers, params=params, data=None)

    # Display the results on the console.
    print('Request training status:')
    print(response.json())


except Exception as e:
    # Display the error string.
    print(f'{e}')
