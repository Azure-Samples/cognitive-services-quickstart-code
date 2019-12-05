# Microsoft Azure Language Understanding (LUIS) - Build App
#
# This script builds a LUIS app, entities, and intents using the Python
# LUIS SDK.  A separate sample trains and publishes the app.
#
# This script requires the Cognitive Services LUIS Python module:
#	 python -m pip install azure-cognitiveservices-language-luis
#
# This script runs under Python 3.4 or later.

from azure.cognitiveservices.language.luis.authoring import LUISAuthoringClient
from azure.cognitiveservices.language.luis.runtime import LUISRuntimeClient
from msrest.authentication import CognitiveServicesCredentials

import json, os, sys, time

authoring_key_var_name = 'LUIS_AUTHORING_KEY'
if not authoring_key_var_name in os.environ:
	raise Exception('Please set/export the environment variable: {}'.format(authoring_key_var_name))
authoring_subscription_key = os.environ[authoring_key_var_name]

authoring_endpoint_var_name = 'LUIS_AUTHORING_ENDPOINT'
if not authoring_endpoint_var_name in os.environ:
	raise Exception('Please set/export the environment variable: {}'.format(authoring_endpoint_var_name))
authoring_endpoint = os.environ[authoring_endpoint_var_name]

runtime_key_var_name = 'LUIS_RUNTIME_KEY'
if not runtime_key_var_name in os.environ:
	raise Exception('Please set/export the environment variable: {}'.format(runtime_key_var_name))
runtime_subscription_key = os.environ[runtime_key_var_name]

runtime_endpoint_var_name = 'LUIS_RUNTIME_ENDPOINT'
if not runtime_endpoint_var_name in os.environ:
	raise Exception('Please set/export the environment variable: {}'.format(runtime_endpoint_var_name))
runtime_endpoint = os.environ[runtime_endpoint_var_name]

# Instantiate LUIS clients
authoring_client = LUISAuthoringClient(authoring_endpoint, CognitiveServicesCredentials(authoring_subscription_key))
runtime_client = LUISRuntimeClient(runtime_endpoint, CognitiveServicesCredentials(runtime_subscription_key))

def create_app():
	app_id = authoring_client.apps.add_custom_prebuilt_domain(domain_name="HomeAutomation", culture="en-us")
	print("Created LUIS app with ID {}".format(app_id))
	return app_id

def train_app(app_id, app_version):
	response = authoring_client.train.train_version(app_id, app_version)
	waiting = True
	while waiting:
		info = authoring_client.train.get_status(app_id, app_version)
		# get_status returns a list of training statuses, one for each model. Loop through them and make sure all are done.
		waiting = any(map(lambda x: 'Queued' == x.details.status or 'InProgress' == x.details.status, info))
		if waiting:
			print ("Waiting 10 seconds for training to complete...")
			time.sleep(10)

def publish_app(app_id, app_version):
	response = authoring_client.apps.publish(app_id, version_id=app_version, is_staging=True)
	print("Application published. Endpoint URL: " + response.endpoint_url)
	return response.endpoint_url

def predict(app_id, slot_name):
	request = { "query" : "Turn on the lights." }
# Note be sure to specify, using the slot_name parameter, whether your application is in staging or production.
# By default, applications are created in staging.
	response = runtime_client.prediction.get_slot_prediction(app_id=app_id, slot_name=slot_name, prediction_request=request)
	print("Top intent: {}".format(response.prediction.top_intent))
	print("Sentiment: {}".format (response.prediction.sentiment))
	print("Intents: ")
	for intent in response.prediction.intents:
		print("\t{}".format (json.dumps (intent)))
	print("Entities: {}".format (response.prediction.entities))

print("Creating application...")
app_id = create_app()
print()

print ("Training application...")
app_version = 0.1
train_app(app_id, app_version)
print ()

print ("Publishing application...")
predict_url = publish_app(app_id, app_version)
print ()

print ("Querying application...")
slot_name = "staging"
predict(app_id, slot_name)
