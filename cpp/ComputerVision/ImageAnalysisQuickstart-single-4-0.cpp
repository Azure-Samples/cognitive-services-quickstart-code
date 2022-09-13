//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
//
// Azure AI Vision SDK -- C++ Image Analysis Quickstart
//
// <snippet-single>
#include <memory>
#include <iostream>
#include <vision_api_cxx.h>

using namespace Azure::AI::Vision::Service;
using namespace Azure::AI::Vision::Input;
using namespace Azure::AI::Vision::Preview::ImageAnalysis;
using namespace Azure::AI::Vision::Preview::ImageAnalysis::Options;
using namespace Azure::AI::Vision::Preview::ImageAnalysis::Results;

void AnalyzeImage()
{
    // Replace the string "PASTE_YOUR_COMPUTER_VISION_ENDPOINT_HERE" with your Computer Vision endpoint, found in the Azure portal.
    // The endpoint has the form "https://<your-computer-vision-resource-name>.cognitiveservices.azure.com".
    // Replace the string "PASTE_YOUR_COMPUTER_VISION_SUBSCRIPTION_KEY_HERE" with your Computer Vision key. The key is a 32-character
    // HEX number (no dashes), found in the Azure portal. Similar to "d0dbd4c2a93346f18c785a426da83e15".
    std::shared_ptr<VisionServiceOptions> serviceOptions = VisionServiceOptions::FromEndpoint("PASTE_YOUR_COMPUTER_VISION_ENDPOINT_HERE", "PASTE_YOUR_COMPUTER_VISION_SUBSCRIPTION_KEY_HERE");

    // Specify the image file on disk to analyze
    std::shared_ptr<VisionSource> imageSource = VisionSource::FromFile("laptop-on-kitchen-table.jpg");

    // specify the URL of the image to analyze
    std::shared_ptr<VisionSource> imageSource = VisionSource::FromUrl("https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/landmark.jpg");

    // Creates the options object that will control the ImageAnalyzer
    std::shared_ptr<ImageAnalysisOptions> analysisOptions = ImageAnalysisOptions::Create();

    // You must define one or more features to extract during image analysis
    analysisOptions->SetFeatures({ImageAnalysisFeature::Descriptions, ImageAnalysisFeature::Objects});

    std::shared_ptr<ImageAnalyzer> analyzer = ImageAnalyzer::Create(serviceOptions, imageSource, analysisOptions);

    std::cout << "Please wait for image analysis results..." << std::endl;
    std::shared_ptr<ImageAnalysisResult> result = analyzer->Analyze();

    if (result->GetReason() == ImageAnalysisResultReason::Analyzed)
    {
        if (result->GetTags().HasValue())
        {
            std::vector<ImageTag> tags = result->GetTags().Value();
            std::cout << "Tags:" << std::endl;
            for (ImageTag tag : tags)
            {
                std::cout << "\t\"" << tag.Name << "\", Confidence " << tag.Confidence << std::endl;
            }
        }
    }
    else if (result->GetReason() == ImageAnalysisResultReason::Stopped)
    {
        std::cout << "Analysis failed." << std::endl;

        std::shared_ptr<ImageAnalysisStopDetails> stopDetails = ImageAnalysisStopDetails::FromResult(result);

        if (stopDetails->GetReason() == ImageAnalysisStopReason::Error)
        {
            std::shared_ptr<ImageAnalysisErrorDetails> errorDetails = ImageAnalysisErrorDetails::FromResult(result);

            std::cout << "Error reason =  " << (int)errorDetails->GetReason() << std::endl;
            std::cout << "Error message = " << errorDetails->GetMessage() << std::endl;
            std::cout << "Did you set the computer vision endpoint and key?" << std::endl;
        }
    }
}

int main()
{
    try
    {
        AnalyzeImage();
    }
    catch (std::exception e)
    {
        std::cout << e.what();
    }

    return 0;
}
// </snippet-single>