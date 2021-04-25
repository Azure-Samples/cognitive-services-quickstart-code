# To run this sample, install the following modules.
# pip install azure-cognitiveservices-language-luis

# <Dependencies>
from azure.cognitiveservices.language.luis.authoring import LUISAuthoringClient
from azure.cognitiveservices.language.luis.runtime import LUISRuntimeClient
from msrest.authentication import CognitiveServicesCredentials
from functools import reduce

import json, time
# </Dependencies>

def quickstart(): 

	# <AuthoringEndpointAndKeys>
	authoringKey = 'PASTE_YOUR_LUIS_AUTHORING_SUBSCRIPTION_KEY_HERE'
	authoringEndpoint = 'PASTE_YOUR_LUIS_AUTHORING_ENDPOINT_HERE'
	# </AuthoringEndpointAndKeys>

	# <AuthoringCreateClient>
	client = LUISAuthoringClient(authoringEndpoint, CognitiveServicesCredentials(authoringKey))
	# </AuthoringCreateClient>

	# <ApplicationNameAndVersion>
	appName = "Contoso Pizza Company"
	versionId = "0.1"
	# </ApplicationNameAndVersion>
	
	# Create app
	app_id = create_app(client, appName, versionId)

	# <IntentName>
	intentName = "OrderPizzaIntent"
	# </IntentName>
	
	# <AddIntent>
	client.model.add_intent(app_id, versionId, intentName)
	# </AddIntent>
	
	# Add Entities
	add_entities(client, app_id, versionId)
	
	# Add labeled examples
	add_labeled_examples(client,app_id, versionId, intentName)

	# <TrainAppVersion>
	client.train.train_version(app_id, versionId)
	waiting = True
	while waiting:
		info = client.train.get_status(app_id, versionId)

		# get_status returns a list of training statuses, one for each model. Loop through them and make sure all are done.
		waiting = any(map(lambda x: 'Queued' == x.details.status or 'InProgress' == x.details.status, info))
		if waiting:
			print ("Waiting 10 seconds for training to complete...")
			time.sleep(10)
		else: 
			print ("trained")
			waiting = False
	# </TrainAppVersion>
	
	# <PublishVersion>
	responseEndpointInfo = client.apps.publish(app_id, versionId, is_staging=False)
	# </PublishVersion>
	
	# <PredictionEndpointAndKeys>
	predictionKey = 'PASTE_YOUR_LUIS_PREDICTION_SUBSCRIPTION_KEY_HERE'
	predictionEndpoint = 'PASTE_YOUR_LUIS_PREDICTION_ENDPOINT_HERE'
	# </PredictionEndpointAndKeys>
	
	# <PredictionCreateClient>
	runtimeCredentials = CognitiveServicesCredentials(predictionKey)
	clientRuntime = LUISRuntimeClient(endpoint=predictionEndpoint, credentials=runtimeCredentials)
	# </PredictionCreateClient>

    # <QueryPredictionEndpoint>
    # Production == slot name
	predictionRequest = { "query" : "I want two small pepperoni pizzas with more salsa" }
	
	predictionResponse = clientRuntime.prediction.get_slot_prediction(app_id, "Production", predictionRequest)
	print("Top intent: {}".format(predictionResponse.prediction.top_intent))
	print("Sentiment: {}".format (predictionResponse.prediction.sentiment))
	print("Intents: ")

	for intent in predictionResponse.prediction.intents:
		print("\t{}".format (json.dumps (intent)))
	print("Entities: {}".format (predictionResponse.prediction.entities))
    # </QueryPredictionEndpoint>

def create_app(client, appName, versionId):

    # <AuthoringCreateApplication>
	# define app basics
	appDefinition = {
        "name": appName,
        "initial_version_id": versionId,
        "culture": "en-us"
    }

	# create app
	app_id = client.apps.add(appDefinition)

	# get app id - necessary for all other changes
	print("Created LUIS app with ID {}".format(app_id))
	# </AuthoringCreateApplication>
	
	return app_id
	
# </createApp>

def add_entities(client, app_id, versionId):

	# <AuthoringAddEntities>
	# Add Prebuilt entity
	client.model.add_prebuilt(app_id, versionId, prebuilt_extractor_names=["number"])

	# define machine-learned entity
	mlEntityDefinition = [
	{
		"name": "Pizza",
		"children": [
			{ "name": "Quantity" },
			{ "name": "Type" },
			{ "name": "Size" }
		]
	},
	{
		"name": "Toppings",
		"children": [
			{ "name": "Type" },
			{ "name": "Quantity" }
		]
	}]

	# add entity to app
	modelId = client.model.add_entity(app_id, versionId, name="Pizza order", children=mlEntityDefinition)
	
	# define phraselist - add phrases as significant vocabulary to app
	phraseList = {
		"enabledForAllModels": False,
		"isExchangeable": True,
		"name": "QuantityPhraselist",
		"phrases": "few,more,extra"
	}
	
	# add phrase list to app
	phraseListId = client.features.add_phrase_list(app_id, versionId, phraseList)
	
	# Get entity and subentities
	modelObject = client.model.get_entity(app_id, versionId, modelId)
	toppingQuantityId = get_grandchild_id(modelObject, "Toppings", "Quantity")
	pizzaQuantityId = get_grandchild_id(modelObject, "Pizza", "Quantity")

	# add model as feature to subentity model
	prebuiltFeatureRequiredDefinition = { "model_name": "number", "is_required": True }
	client.features.add_entity_feature(app_id, versionId, pizzaQuantityId, prebuiltFeatureRequiredDefinition)
	
	# add model as feature to subentity model
	prebuiltFeatureNotRequiredDefinition = { "model_name": "number" }
	client.features.add_entity_feature(app_id, versionId, toppingQuantityId, prebuiltFeatureNotRequiredDefinition)

    # add phrase list as feature to subentity model
	phraseListFeatureDefinition = { "feature_name": "QuantityPhraselist", "model_name": None }
	client.features.add_entity_feature(app_id, versionId, toppingQuantityId, phraseListFeatureDefinition)
    # </AuthoringAddEntities>
	

def add_labeled_examples(client, app_id, versionId, intentName):

	# <AuthoringAddLabeledExamples>
    # Define labeled example
    labeledExampleUtteranceWithMLEntity = {
        "text": "I want two small seafood pizzas with extra cheese.",
        "intentName": intentName,
        "entityLabels": [
            {
                "startCharIndex": 7,
                "endCharIndex": 48,
                "entityName": "Pizza order",
                "children": [
                    {
                        "startCharIndex": 7,
                        "endCharIndex": 30,
                        "entityName": "Pizza",
                        "children": [
                            {
                                "startCharIndex": 7,
                                "endCharIndex": 9,
                                "entityName": "Quantity"
                            },
                            {
                                "startCharIndex": 11,
                                "endCharIndex": 15,
                                "entityName": "Size"
                            },
                            {
                                "startCharIndex": 17,
                                "endCharIndex": 23,
                                "entityName": "Type"
                            }]
                    },
                    {
                        "startCharIndex": 37,
                        "endCharIndex": 48,
                        "entityName": "Toppings",
                        "children": [
                            {
                                "startCharIndex": 37,
                                "endCharIndex": 41,
                                "entityName": "Quantity"
                            },
                            {
                                "startCharIndex": 43,
                                "endCharIndex": 48,
                                "entityName": "Type"
                            }]
                    }
                ]
            }
        ]
    }

    print("Labeled Example Utterance:", labeledExampleUtteranceWithMLEntity)

    # Add an example for the entity.
    # Enable nested children to allow using multiple models with the same name.
	# The quantity subentity and the phraselist could have the same exact name if this is set to True
    client.examples.add(app_id, versionId, labeledExampleUtteranceWithMLEntity, { "enableNestedChildren": True })
	# </AuthoringAddLabeledExamples>
	
# <AuthoringSortModelObject>
def get_grandchild_id(model, childName, grandChildName):
	
	theseChildren = next(filter((lambda child: child.name == childName), model.children))
	theseGrandchildren = next(filter((lambda child: child.name == grandChildName), theseChildren.children))
	
	grandChildId = theseGrandchildren.id
	
	return grandChildId
# </AuthoringSortModelObject>

quickstart()
