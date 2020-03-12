# Microsoft Azure Language Understanding (LUIS) - Build App
#
# This script builds a LUIS application using the Azure SDK for Python.
# It adds entities, intents, and utterances to the application.
# Finally it trains and publishes the application, and prints the
# application endpoint that you can use for predictions.
#
# This script requires the Cognitive Services LUIS Python module:
#     python -m pip install azure-cognitiveservices-language-luis
#
# This script runs under Python 3.4 or later.

# Be sure you understand how LUIS models work.  In particular, know what
# intents, entities, and utterances are, and how they work together in the
# context of a LUIS app. See the following:
#
# https://www.luis.ai/welcome
# https://docs.microsoft.com/azure/cognitive-services/luis/luis-concept-intent
# https://docs.microsoft.com/azure/cognitive-services/luis/luis-concept-entity-types
# https://docs.microsoft.com/azure/cognitive-services/luis/luis-concept-utterance

# <Dependencies>
from azure.cognitiveservices.language.luis.authoring import LUISAuthoringClient
from msrest.authentication import CognitiveServicesCredentials

import datetime, json, os, sys, time
# </Dependencies>

# <AuthorizationVariables>
key_var_name = 'LUIS_AUTHORING_KEY'
if not key_var_name in os.environ:
	raise Exception('Please set/export the environment variable: {}'.format(key_var_name))
authoring_key = os.environ[key_var_name]

endpoint_var_name = 'LUIS_AUTHORING_ENDPOINT'
if not endpoint_var_name in os.environ:
	raise Exception('Please set/export the environment variable: {}'.format(endpoint_var_name))
authoring_endpoint = os.environ[endpoint_var_name]
# </AuthorizationVariables>

# <Client>
# Instantiate a LUIS client
client = LUISAuthoringClient(authoring_endpoint, CognitiveServicesCredentials(authoring_key))
# </Client>

# <createApp>
def create_app():
	# Create a new LUIS app
	app_name    = "Contoso {}".format(datetime.datetime.now())
	app_desc    = "Flight booking app built with LUIS Python SDK."
	app_version = "0.1"
	app_locale  = "en-us"

	app_id = client.apps.add(dict(name=app_name,
									initial_version_id=app_version,
									description=app_desc,
									culture=app_locale))

	print("Created LUIS app {}\n    with ID {}".format(app_name, app_id))
	return app_id, app_version
# </createApp>

# Declare entities:
#
#   Location - A simple entity that can have one of two roles (Origin, Destination)
#
#   Class - A simple entity that represents the flight class
#
#	Prebuilt entities - These further describe the flight
#
# Creating an entity (or other LUIS object) returns its ID.
# We don't use IDs further in this script, so we don't keep the return value.
# <addEntities>
def add_entities(app_id, app_version):

	locationEntityId = client.model.add_entity(app_id, app_version, name="Location")
	print("locationEntityId {} added.".format(locationEntityId))

	client.model.create_entity_role(app_id, app_version, locationEntityId, name="Origin")
	print("'Origin' role added to location entity.")

	client.model.create_entity_role(app_id, app_version, locationEntityId, name="Destination")
	print("'Destination' role added to location entity.")

	classEntityId = client.model.add_entity(app_id, app_version, name="Class")
	print("classEntityId {} added.".format(classEntityId))

	client.model.add_prebuilt(app_id, app_version, ["number", "datetimeV2", "geographyV2", "ordinal"])
	print("Prebuilt entities 'number', 'datetimeV2', 'geographyV2', 'ordinal' added.")

# Notes
# - A child entity cannot have the name of an entity that already exists (for example, "Location.")
# - A child entity cannot have the name of a prebuilt entity (for example, "number.")
# - If a child entity has an instanceOf property, the value must be the name of a prebuilt entity (for example, "number.")
# - For a non child entity, the instanceOf property value can be the name of a prebuilt entity or an entity we created (for example, "Location.")
# - If an instanceOf property value has the name of a prebuilt entity, we must have already added that prebuilt entity to the application with model.add_prebuilt().
	flightEntityId = client.model.add_entity(app_id, app_version, name="Flight", children=[
		dict(name="Flight_Number", instanceOf="number"),
		dict(name="Departure_DateTime", instanceOf="datetimeV2"),
		dict(name="Arrival_DateTime", instanceOf="datetimeV2"),
		dict(name="Origin_Location", instanceOf="geographyV2"),
		dict(name="Destination_Location", instanceOf="geographyV2"),
		dict(name="Takeoff_Order", instanceOf="ordinal")
	])
	print("flightEntityId {} added.".format(flightEntityId))
# </addEntities>

# Declare an intent, FindFlights, that recognizes a user's Flight request
# Creating an intent returns its ID, which we don't need, so don't keep.
# <addIntents>
def add_intents(app_id, app_version):
	intentId = client.model.add_intent(app_id, app_version, "FindFlights")

	print("Intent FindFlights {} added.".format(intentId))
# </addIntents>


# Helper function for creating the utterance data structure.
# <createUtterance>
def create_utterance(intent, utterance, *labels):
    """Add an example LUIS utterance from utterance text and a list of
       labels.  Each label is a 2-tuple containing a label name and the
       text within the utterance that represents that label.

       Utterances apply to a specific intent, which must be specified."""

    text = utterance.lower()

    def label(name, value):
        value = value.lower()
        start = text.index(value)
        return dict(entity_name=name, start_char_index=start,
                    end_char_index=start + len(value))

    return dict(text=text, intent_name=intent,
                entity_labels=[label(n, v) for (n, v) in labels])
# </createUtterance>

# Add example utterances for the intent.  Each utterance includes labels
# that identify the entities within each utterance by index.  LUIS learns
# how to find entities within user utterances from the provided examples.
#
# Example utterance: "find flights in economy to Madrid"
# Labels: Flight -> "economy to Madrid" (composite of Destination and Class)
#         Destination -> "Madrid"
#         Class -> "economy"
# <addUtterances>
def add_utterances(app_id, app_version):
	# Now define the utterances
	utterances = [create_utterance("FindFlights", "find flights in economy to Madrid",
							("Flight", "economy to Madrid"),
							("Location", "Madrid"),
							("Class", "economy")),

				  create_utterance("FindFlights", "find flights to London in first class",
							("Flight", "London in first class"),
							("Location", "London"),
							("Class", "first")),

				  create_utterance("FindFlights", "find flights from Seattle to London in first class",
							("Flight", "flights from seattle to London in first class"),
							("Location", "Seattle"),
							("Location", "London"),
							("Class", "first"))]

	# Add the utterances in batch. You may add any number of example utterances
	# for any number of intents in one call.
	info = client.examples.batch(app_id, app_version, utterances)
	failed = filter(lambda x: x.has_error, info)
	if any(failed):
		print ("Adding utterances failed. Results:")
		for x in list(failed):
			print (x.error.message)
			print ()
			sys.exit(0)
	else:
		print("{} example utterance(s) added.".format(len(utterances)))
# </addUtterances>

# <train>
def train_app(app_id, app_version):
	response = client.train.train_version(app_id, app_version)
	waiting = True
	while waiting:
		info = client.train.get_status(app_id, app_version)
		# get_status returns a list of training statuses, one for each model. Loop through them and make sure all are done.
		waiting = any(map(lambda x: 'queued' in x.details.status.casefold() or 'inprogress' in x.details.status.casefold(), info))
		if waiting:
			print ("Waiting 10 seconds for training to complete...")
			time.sleep(10)
		else:
			if any(filter(lambda x: 'fail' in x.details.status.casefold(), info)):
				print ("Training failed. Results:")
# Do not loop through the result of filter(lambda x: 'fail' in x.details.status.casefold(), info),
# because for some reason that loses the first item in the list.
				for x in info:
					if 'fail' in x.details.status.casefold():
						print ("Model ID: " + x.model_id)
						print ("Reason: " + x.details.failure_reason)
						print ()
				sys.exit(0)
# </train>

# <publish>
def publish_app(app_id, app_version):
	responseEndpointInfo = client.apps.publish(app_id, app_version, is_staging=True)
	print("Application published. Endpoint URL: " + responseEndpointInfo.endpoint_url)
# </publish>

# <predict>
def predict(app_id, publishInfo, slot_name):

	request = { "query" : "Find flight to seattle" }

	# Note be sure to specify, using the slot_name parameter, whether your application is in staging or production.
	response = clientRuntime.prediction.get_slot_prediction(app_id=app_id, slot_name=slot_name, prediction_request=request)

	print("Top intent: {}".format(response.prediction.top_intent))
	print("Sentiment: {}".format (response.prediction.sentiment))
	print("Intents: ")

	for intent in response.prediction.intents:
		print("\t{}".format (json.dumps (intent)))
	print("Entities: {}".format (response.prediction.entities))
# </predict>

print("Creating application...")
app_id, app_version = create_app()
print()

print ("Adding entities to application...")
add_entities(app_id, app_version)
print ()

print ("Adding intents to application...")
add_intents(app_id, app_version)
print ()

print ("Adding utterances to application...")
add_utterances(app_id, app_version)
print ()

print ("Training application...")
train_app(app_id, app_version)
print ()

print ("Publishing application...")
publish_app(app_id, app_version)
