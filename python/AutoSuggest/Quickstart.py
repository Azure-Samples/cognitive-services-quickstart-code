from azure.cognitiveservices.search.autosuggest import AutoSuggestSearchAPI
from msrest.authentication import CognitiveServicesCredentials

import json, os, sys

'''
Microsoft Azure Cognitive Services Bing Autosuggest - Get Search Suggestions

This script requires the Cognitive Services Bing Autosuggest module:
  python -m pip install azure-cognitiveservices-search_autosuggest

This script runs under Python 3.4 or later.
'''

subscription_key = os.environ['AUTOSUGGEST_SUBSCRIPTION_KEY']
if not subscription_key:
    raise Exception(
        'Please set/export the environment variable: {}'.format(subscription_key))

# Instantiate a Bing Autosuggest client
client = AutoSuggestSearchAPI(CognitiveServicesCredentials(subscription_key))

# Returns from the Suggestions class
result = client.auto_suggest('sail')

# Access all suggestions
suggestions = result.suggestion_groups[0]

# print results
for suggestion in suggestions.search_suggestions:
    print(suggestion.query)
    print(suggestion.display_text)
