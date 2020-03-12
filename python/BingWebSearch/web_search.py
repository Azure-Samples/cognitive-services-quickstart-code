import logging
import os
import sys

from azure.cognitiveservices.search.websearch import WebSearchClient
from azure.cognitiveservices.search.websearch.models import SafeSearch
from msrest.authentication import CognitiveServicesCredentials

'''
This quickstart performs Bing Search searches with queries. Different filters are used and results returned.

Install the Bing Search v7 SDK:
python -m pip install azure-cognitiveservices-search-websearch

Bing Web Search documentation: https://docs.microsoft.com/en-us/azure/cognitive-services/bing-web-search/
Bing Web Search SDK: https://docs.microsoft.com/en-us/python/api/azure-cognitiveservices-search-websearch/azure.cognitiveservices.search.websearch?view=azure-python
Bing Web Search v7 API: https://docs.microsoft.com/en-us/rest/api/cognitiveservices-bingsearch/bing-web-api-v7-reference
'''

'''
WebSearchResultTypesLookup.
Searches for a query (Xbox) and prints the name and URL for the first few web, image, news and video results.
'''
def search_different_types(client):
    print()
    web_data = client.web.search(query="xbox")
    print(">>>>> Searched for Query: \"Xbox\"")

    # WebPages
    if web_data.web_pages.value:
        print("Webpage Results: {}".format(len(web_data.web_pages.value)))

        for i in range(3):
            print("Name: {} ".format(web_data.web_pages.value[i].name))
            print("URL: {} ".format(web_data.web_pages.value[i].url))
    else:
        print("Didn't see any Web data..")

    # Images
    print('-------------')
    if web_data.images.value:
        print("Image Results: {}".format(len(web_data.images.value)))

        for i in range(3):
            print("Name: {} ".format(web_data.images.value[i].name))
            print("URL: {} ".format(web_data.images.value[i].url))
    else:
        print("Didn't see any Images..")

    # News
    print('-------------')
    if web_data.news.value:
        print("News Results: {}".format(len(web_data.news.value)))

        for i in range(3):
            print("Name: {} ".format(web_data.news.value[i].name))
            print("URL: {} ".format(web_data.news.value[i].url))
    else:
        print("Didn't see any News..")

    # Videos
    print('-------------')
    if web_data.videos.value:
        print("Videos Results: {}".format(len(web_data.videos.value)))

        for i in range(3):
            print("Name: {} ".format(web_data.videos.value[i].name))
            print("URL: {} ".format(web_data.videos.value[i].url))
    else:
        print("Didn't see any Videos..")

'''
WebResultsWithCountAndOffset.
Searches for (Best restaurants in Seattle), verifies number of results, 
and prints the name and URL of the first few results.
'''
def search_with_count_and_offset(client):
    print()
    web_data = client.web.search(query="Best restaurants in Seattle", offset=10, count=20)

    print(">>>>> Searched for Query: \"Best restaurants in Seattle\"")

    if web_data.web_pages.value:
        print("Webpage Results: {}".format(len(web_data.web_pages.value)))

        for i in range(3):
            print("Name: {} ".format(web_data.web_pages.value[i].name))
            print("URL: {} ".format(web_data.web_pages.value[i].url))
    else:
        print("Didn't see any Web data..")


'''
WebSearchWithResponseFilter.
Searches for (Microsoft) with response filters to news and prints details of the first few news results.
'''
def search_with_response_filter(client):
    print()
    web_data = client.web.search(query="Microsoft", response_filter=["News"])

    print(">>>>> Searched for Query: \"Microsoft\" with response filters \"News\"")

    # News attribute since I filtered "News"
    if web_data.news.value:
        print("Webpage Results: {}".format(len(web_data.news.value)))

        for i in range(3):
            print("Name: {} ".format(web_data.news.value[i].name))
            print("URL: {} ".format(web_data.news.value[i].url))
    else:
        print("Didn't see any Web data..")


'''
WebSearchWithAnswerCountPromoteAndSafeSearch.
Searches for (Lady Gaga) with answerCount and promotes parameters and prints details of the first few answers.
'''
def search_with_answer_count_promote_and_safe_search(client):
    print()
    web_data = client.web.search(
        query="Lady Gaga",
        answer_count=2,
        promote=["videos"],
        safe_search=SafeSearch.strict  # or directly "Strict"
    )
    print(">>>>> Searched for Query: \"Lady Gaga\"")

    if web_data.web_pages.value:
        print("Webpage Results: {}".format(len(web_data.web_pages.value)))

        for i in range(3):
            print("Name: {} ".format(web_data.web_pages.value[i].name))
            print("URL: {} ".format(web_data.web_pages.value[i].url))
    else:
        print("Didn't see any Web data..")


if __name__ == "__main__":
    # Add your Bing Search V7 subscription key to your environment variables.
    SUBSCRIPTION_KEY = os.environ['BING_SEARCH_V7_SUBSCRIPTION_KEY']
    ENDPOINT = os.environ['BING_SEARCH_V7_ENDPOINT']

    # Initialize a client
    client = WebSearchClient(endpoint=ENDPOINT, credentials=CognitiveServicesCredentials(SUBSCRIPTION_KEY))
    
    # Perform a search for different types: web pages, images, news, videos
    search_different_types(client)
    # Uses count and offset to start or end the certain on certain pages of the results
    search_with_count_and_offset(client)
    # Adds a response filter topic ("news") to the web page search
    search_with_response_filter(client)
    # Answer count restricts the types in the response (since the results contain many types).
    # Promote will focus on a search with ths promoted type, for example if you promote "videos".
    # Safe search sets the results to the level you want to restrict them (for inappropriate or adult content).
    search_with_answer_count_promote_and_safe_search(client)
