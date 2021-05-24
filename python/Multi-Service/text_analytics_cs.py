import os

from azure.cognitiveservices.language.textanalytics import TextAnalyticsClient
from msrest.authentication import CognitiveServicesCredentials

'''
Microsoft Azure Cognitive Services Text Analytics - Get sentiment

This example, using the general Cognitive Services key/endpoint. It's used when you want to 
combine many Cognitive Services with just one authentication key/endpoint. 
Services are not combined here, but could be potentially. 

Install the Text Analytics SDK from a command prompt or IDE terminal:
  pip install --upgrade azure-cognitiveservices-language-textanalytics
'''

subscription_key = "PASTE_YOUR_TEXT_ANALYTICS_SUBSCRIPTION_KEY_HERE"
endpoint = "PASTE_YOUR_TEXT_ANALYTICS_ENDPOINT_HERE"

'''
AUTHENTICATE
Create a Text Analytics client.
'''
credentials = CognitiveServicesCredentials(subscription_key)
text_analytics_client = TextAnalyticsClient(endpoint=endpoint, credentials=credentials)

'''
TEXT ANALYTICS
Gets the sentiment value of a body of text.
Values closer to zero (0.0) indicate a negative sentiment, while values closer to one (1.0) indicate a positive sentiment. 
'''
try:
    documents = [
        {"id": "1", "language": "en", "text": "I had the best day of my life."},
        {"id": "2", "language": "en",
            "text": "This was a waste of my time. The speaker put me to sleep."},
        {"id": "3", "language": "es", "text": "No tengo dinero ni nada que dar..."},
        {"id": "4", "language": "it",
            "text": "L'hotel veneziano era meraviglioso. È un bellissimo pezzo di architettura."}
    ]

    response = text_analytics_client.sentiment(documents=documents)
    for document in response.documents:
        print("Document Id: ", document.id, ", Sentiment Score: ",
                "{:.2f}".format(document.score))

except Exception as err:
    print("Encountered exception. {}".format(err))

