import os

from azure.cognitiveservices.language.spellcheck import SpellCheckAPI
from msrest.authentication import CognitiveServicesCredentials

'''
This Bing Spell Check quickstart checks some misspelled words and suggests corrections.

Prerequisites:
  - Add your Bing Spell Check subscription key to your environment variables, using
    BING_SPELL_CHECK_SUBSCRIPTION_KEY as a variable name.
  - Install the following modules:
    pip install azure.cognitiveservices.language.spellcheck
    pip install msrest

Python SDK: https://docs.microsoft.com/en-us/python/api/overview/azure/cognitiveservices/spellcheck?view=azure-python
'''

# Add your Bing Spell Check subscription key to your environment variables.
SUBSCRIPTION_KEY = os.environ['BING_SPELL_CHECK_SUBSCRIPTION_KEY']

# Create a client
client = SpellCheckAPI(CognitiveServicesCredentials(SUBSCRIPTION_KEY))

try:
    # Original query
    query = 'bill gtaes was ehre toody'
    print('Original query:\n', query)
    print()
    # Check the query for misspellings
    # mode can be 'proof' or 'spell'
    result = client.spell_checker(query, mode='proof')

    # Print the suggested corrections
    print('Suggested correction:')
    for token in result.flagged_tokens:
        for suggestion_object in token.suggestions:
            print(suggestion_object.suggestion)

except Exception as err:
    print("Encountered exception. {}".format(err))
