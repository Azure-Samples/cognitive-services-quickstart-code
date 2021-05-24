import os

from azure.cognitiveservices.search.autosuggest import AutoSuggestClient
from msrest.authentication import CognitiveServicesCredentials

'''
Microsoft Azure Cognitive Services - Bing Autosuggest - Get Search Suggestions

Uses the general Cognitive Services key/endpoint. It's used when you want to 
combine many Cognitive Services with just one authentication key/endpoint. 
Services are not combined here, but could be potentially. 

Install the Cognitive Services Bing Autosuggest SDK module:
  python -m pip install azure-cognitiveservices-search_autosuggest

Use Python 3.4+
'''

subscription_key = "PASTE_YOUR_AUTO_SUGGEST_SUBSCRIPTION_KEY_HERE"
endpoint = "PASTE_YOUR_AUTO_SUGGEST_ENDPOINT_HERE"

'''
AUTHENTICATE
Create an Autosuggest client.
'''
credentials = CognitiveServicesCredentials(subscription_key)
autosuggest_client = AutoSuggestClient(endpoint, CognitiveServicesCredentials(subscription_key))

'''
AUTOSUGGEST
This example uses a query term to search for autocompletion suggestions for the term.
'''
# Returns from the Suggestions class
result = autosuggest_client.auto_suggest('sail')

# Access all suggestions
suggestions = result.suggestion_groups[0]

# print results
for suggestion in suggestions.search_suggestions:
    print(suggestion.query)
    print(suggestion.display_text)
