import os

from azure.ai.textanalytics import TextAnalyticsClient
from azure.core.credentials import AzureKeyCredential

'''
Microsoft Azure Cognitive Services Text Analytics - Get sentiment

Install the Text Analytics SDK from a command prompt or IDE terminal:
python -m pip install --upgrade azure.ai.textanalytics
'''

subscription_key = "PASTE_YOUR_TEXT_ANALYTICS_SUBSCRIPTION_KEY_HERE"
endpoint = "PASTE_YOUR_TEXT_ANALYTICS_ENDPOINT_HERE"

'''
AUTHENTICATE
Create a Text Analytics client.
'''
credential = AzureKeyCredential (subscription_key)
text_analytics_client = TextAnalyticsClient (endpoint=endpoint, credential=credential)

'''
TEXT ANALYTICS
Gets the sentiment value of a body of text.
Values closer to zero (0.0) indicate a negative sentiment, while values closer to one (1.0) indicate a positive sentiment. 
'''
try:
    documents = ["I had the best day of my life.", "This was a waste of my time. The speaker put me to sleep.", "No tengo dinero ni nada que dar...", "L'hotel veneziano era meraviglioso. È un bellissimo pezzo di architettura."]

    response = text_analytics_client.analyze_sentiment(documents=documents)

    for result in response :
        print ("Sentiment: " + result.sentiment)
        print ("Confidence scores:")
        print ("Positive: " + str (result.confidence_scores.positive))
        print ("Neutral: " + str (result.confidence_scores.neutral))
        print ("Negative: " + str (result.confidence_scores.negative))
        print ()

except Exception as err:
    print("Encountered exception. {}".format(err))
