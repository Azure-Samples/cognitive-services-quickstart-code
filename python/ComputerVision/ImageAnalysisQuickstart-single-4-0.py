# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE.md file in the project root for full license information.

# Azure AI Vision SDK -- Python Image Analysis Quickstart

# <snippet_single>
import azure.ai.vision as visionsdk

# Replace the string "PASTE_YOUR_COMPUTER_VISION_ENDPOINT_HERE" with your Computer Vision endpoint, found in the Azure portal.
# The endpoint has the form "https://<your-computer-vision-resource-name>.cognitiveservices.azure.com".
# Replace the string "PASTE_YOUR_COMPUTER_VISION_SUBSCRIPTION_KEY_HERE" with your Computer Vision key. The key is a 32-character
# HEX number (no dashes), found in the Azure portal. Similar to "d0dbd4c2a93346f18c785a426da83e15".
computer_vision_endpoint, computer_vision_key = "PASTE_YOUR_COMPUTER_VISION_ENDPOINT_HERE", "PASTE_YOUR_COMPUTER_VISION_SUBSCRIPTION_KEY_HERE"

service_options = visionsdk.VisionServiceOptions(endpoint=computer_vision_endpoint, key=computer_vision_key)

# Specify the URL of the image to analyze
image_url = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/landmark.jpg"
vision_source = visionsdk.VisionSource(url=image_url)

# Set the language and one or more visual features as analysis options
image_analysis_options =  visionsdk.ImageAnalysisOptions()
image_analysis_options.features = (
    visionsdk.ImageAnalysisFeature.CAPTIONS
)

# Create the image analyzer object
image_analyzer = visionsdk.ImageAnalyzer(config=service_options, input=vision_source, options=image_analysis_options)

# Do image analysis for the specified visual features
result = image_analyzer.analyze()

# Checks result.
if result.reason == visionsdk.ImageAnalysisResultReason.ANALYZED:
    if result.captions is not None:
            print(' Captions:')
            for caption in result.captions:
                print('   \'{}\', Confidence {:.4f}'.format(caption.content, caption.confidence))
    
elif result.reason == visionsdk.ImageAnalysisResultReason.STOPPED:
    error_details = visionsdk.ImageAnalysisErrorDetails(result)
    print("Analysis failed.")
    print("Error reason: {}".format(error_details.error_code))
    print("Error message: {}".format(error_details.message))
    print("Did you set the computer vision endpoint and key?")
# </snippet_single>