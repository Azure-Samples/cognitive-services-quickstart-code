import json, os, sys

from azure.cognitiveservices.search.autosuggest import AutoSuggestClient
from msrest.authentication import CognitiveServicesCredentials

'''
Microsoft Azure Cognitive Services Bing Autosuggest - Get Search Suggestions

Install the Bing Autosuggest module:
  python -m pip install azure-cognitiveservices-search_autosuggest
'''

subscription_key = "PASTE_YOUR_AUTO_SUGGEST_SUBSCRIPTION_KEY_HERE"
endpoint = "PASTE_YOUR_AUTO_SUGGEST_ENDPOINT_HERE"

# Instantiate a Bing Autosuggest client
client = AutoSuggestClient(endpoint, CognitiveServicesCredentials(subscription_key))

# Returns from the Suggestions class
result = client.auto_suggest('sail')

# Access all suggestions
suggestions = result.suggestion_groups[0]

# print results
for suggestion in suggestions.search_suggestions:
    print(suggestion.query)
    print(suggestion.display_text)
