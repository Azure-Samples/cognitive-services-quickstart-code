import os

from msrest.authentication import CognitiveServicesCredentials
from azure.cognitiveservices.search.entitysearch import EntitySearchClient
from azure.cognitiveservices.search.entitysearch.models import Place, ErrorResponseException

'''
Microsoft Azure Cognitive Services - Entity Search

This quickstart uses the name of a famous person and the name/city of a 
restaurant to search for a description and a telephone number.

Uses the general Cognitive Services key/endpoint. It's used when you want to 
combine many Cognitive Services with just one authentication key/endpoint. 
Services are not combined here, but could be potentially. 

Install the Entity Search SDK from a command prompt or IDE terminal:
  python -m pip install azure-cognitiveservices-search-entitysearch
'''

subscription_key = 'PASTE_YOUR_BING_SEARCH_SUBSCRIPTION_KEY_HERE'
endpoint = 'PASTE_YOUR_BING_SEARCH_ENDPOINT_HERE'

# Search queries
person_query = 'Allan Turing'
restaurant_query = 'Shiro\'s Sushi Seattle'

'''
AUTHENTICATE
Create an Entity Search client.
'''
entity_search_client = EntitySearchClient(endpoint, CognitiveServicesCredentials(subscription_key))

'''
Bing Entity Search
Takes an entity (a famous person) and prints a short description about them.
'''
# Search for the dominant entity
try:
  entity_data = entity_search_client.entities.search(query=person_query)

  if entity_data.entities.value:
    # Find the entity that represents the dominant one
    main_entities = [entity for entity in entity_data.entities.value
                      if entity.entity_presentation_info.entity_scenario == 'DominantEntity']

    if main_entities:
      print()
      print('Searched for ' + person_query + ' and found this description:')
      print()
      print(main_entities[0].description)
    else:
      print('Couldn\'t find main entity ' + person_query + '!')

  else:
    print('Didn\'t see any data..')

except Exception as err:
  print('Encountered exception. {}'.format(err))

print()  

# Search for a known restaurant and print out its phone number
try:
  entity_data = entity_search_client.entities.search(query=restaurant_query)

  if entity_data.places.value:

    restaurant = entity_data.places.value[0]

    try:
        telephone = restaurant.telephone
        print('Searched for ' + restaurant_query + ' and found this phone number: ')
        print()
        print(telephone)
    except AttributeError:
        print("Couldn't find a place!")

  else:
    print("Didn't see any data..")

except Exception as err:
  print("Encountered exception. {}".format(err))
