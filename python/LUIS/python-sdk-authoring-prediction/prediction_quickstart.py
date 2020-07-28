# Microsoft Azure Language Understanding (LUIS) - Build App
#
# This script queries a public LUIS app for IoT using the Python
# LUIS SDK.
#
# This script requires the Cognitive Services LUIS Python module:
#     python -m pip install azure-cognitiveservices-language-luis
#
# This script runs under Python 3.4 or later.

# For more information about LUIS, see
#
# https://www.luis.ai/welcome
# https://docs.microsoft.com/en-us/azure/cognitive-services/luis

# <Dependencies>
from azure.cognitiveservices.language.luis.runtime import LUISRuntimeClient
from msrest.authentication import CognitiveServicesCredentials

import datetime, json, os, time
# </Dependencies>

# <AuthorizationVariables>
runtime_key = 'REPLACE-WITH-YOUR-ASSIGNED-PREDICTION-KEY'
print("runtime_key: {}".format(runtime_key))

runtime_endpoint = 'https://REPLACE-WITH-YOUR-RESOURCE-NAME.cognitiveservices.azure.com'
print("runtime_endpoint: {}".format(runtime_endpoint))
# </AuthorizationVariables>

# <OtherVariables>
# Use public app ID or replace with your own trained and published app's ID
# to query your own app
# public appID = 'df67dcdb-c37d-46af-88e1-8b97951ca1c2'
luisAppID = 'REPLACE-WITH-YOUR-LUIS_APP_ID'
print("luisAppID: {}".format(luisAppID))

# `production` or `staging`
luisSlotName = 'production'
print("luisSlotName: {}".format(luisSlotName))
# </OtherVariables>


# <Client>
# Instantiate a LUIS runtime client
clientRuntime = LUISRuntimeClient(runtime_endpoint, CognitiveServicesCredentials(runtime_key))
# </Client>


# <predict>
def predict(app_id, slot_name):

	request = { "query" : "turn on all lights" }

	# Note be sure to specify, using the slot_name parameter, whether your application is in staging or production.
	response = clientRuntime.prediction.get_slot_prediction(app_id=app_id, slot_name=slot_name, prediction_request=request)

	print("Top intent: {}".format(response.prediction.top_intent))
	print("Sentiment: {}".format (response.prediction.sentiment))
	print("Intents: ")

	for intent in response.prediction.intents:
		print("\t{}".format (json.dumps (intent)))
	print("Entities: {}".format (response.prediction.entities))
# </predict>

predict(luisAppID, luisSlotName)