# Quickstarts using Cognitive Services authentication

This repo has quickstarts from the select Cognitive Services that can use a general [Cognitive Services authentication](https://docs.microsoft.com/en-us/azure/cognitive-services/cognitive-services-apis-create-account?tabs=multiservice%2Cwindows)  (subscription key and endpoint) in place of the authentication used from a service-specific resource. For instance, a Face quickstart normally uses authentication from a Face resource created in the [Azure portal](https://portal.azure.com), but if you wanted to combine Bing Search with Face for a custom sample, you can use a single Cognitive Services authentication for both Face and Bing Search to create each client. This simplifies the process by consolidating your resources created in Azure to one resource.

The Cognitive Services that can be combined to use a single Cognitive Services resource are not combined here, but each quickstart shows the Cognitive Services authentication for each applicable service. Often it is only a difference of changing the key and endpoint, but for some services, adding additional text to the endpoint is necessary. For example, Bing Search requires you add `bing/v7.0` to the endpoint, so that it looks like this:

```
web_search_client = WebSearchAPI(CognitiveServicesCredentials(subscription_key), endpoint + 'bing/v7.0')
```

The endpoint requirements might change in the future, but these quickstarts use the current requirements.

## List of applicable Cognitive Services

* [AUTOSUGGEST](https://docs.microsoft.com/en-us/azure/cognitive-services/bing-autosuggest/get-suggested-search-terms) - Populate your searches drop-down with suggested queries others have searched for with Bing
* [COMPUTER VISION](https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/index) - Analyze images
* [CONTENT MODERATOR](https://docs.microsoft.com/en-us/azure/cognitive-services/content-moderator/index) - Check text, image or videos for offensive or undesirable content
* [CUSTOM SEARCH](https://docs.microsoft.com/en-us/azure/cognitive-services/custom-vision-service/index) - Specify domains and webpages you care about searching with Bing
* [ENTITY SEARCH](https://docs.microsoft.com/en-us/azure/cognitive-services/bing-entities-search/overview) - Get information about detected people, places, media titles and more in search queries with Bing
* [FACE](https://docs.microsoft.com/en-us/azure/cognitive-services/face/index) - Recognize people and their attributes in an image
* [LANGUAGE UNDERSTANDING](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/index) - Extract meaning from natural language
* [SPEECH](https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/index) - Transform speech-to-text, text-to-speech and recognize speakers
* [SPELL CHECK](https://docs.microsoft.com/en-us/azure/cognitive-services/bing-spell-check/overview) - Perform contextual grammar and spell checking with Bing
* [TEXT ANALYTICS](https://docs.microsoft.com/en-us/azure/cognitive-services/text-analytics/index) - Detect sentiment, key phrases, entities and human language type in text
* [TRANSLATOR TEXT](https://docs.microsoft.com/en-us/azure/cognitive-services/translator/index) - Translate text in near real-time
* [VIDEO INDEXER](https://docs.microsoft.com/en-us/azure/media-services/video-indexer/index) - Analyze video and audio
* [WEB SEARCH](https://docs.microsoft.com/en-us/azure/cognitive-services/bing-web-search/overview) - Integrate Bing’s search capabilities in your applications

Some services require more complex steps to use the Cognitive Services general authentication, so we omitted examples for Custom Search, Language Understanding (LUIS), Translator Text, and Video Indexer, as they would not be suitable for a brief quickstart.

## How to use these quickstarts
1. Install each Cognitive Service library that you want to use, such as: <br>
   `pip install --upgrade azure-cognitiveservices-vision-face` <br>
   Each service installation command is at the top of its respective quickstart.
1. Create an Azure resource for [Cognitive Services](https://docs.microsoft.com/en-us/azure/cognitive-services/cognitive-services-apis-create-account?tabs=multiservice%2Cwindows)
1. Add the subscription key and endpoint constants to your environment variables with the following naming: <br>
   COGNITIVE_SERVICES_SUBSCRIPTION_KEY <br>
   COGNITIVE_SERVICES_ENDPOINT <br>
   NOTE: some services only use a subscription key
1. Run from the command line or your favorite IDE.

