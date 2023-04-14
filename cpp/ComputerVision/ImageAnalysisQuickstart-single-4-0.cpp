//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
//
// Azure AI Vision SDK -- C++ Image Analysis Quickstart
//
// <snippet-single>
#include <memory>
#include <iostream>
#include <vision_api_cxx_image_analyzer.h>

using namespace Azure::AI::Vision::Service;
using namespace Azure::AI::Vision::Input;
using namespace Azure::AI::Vision::ImageAnalysis;

void AnalyzeImage()
{
    std::shared_ptr<VisionServiceOptions> serviceOptions = VisionServiceOptions::FromEndpoint("PASTE_YOUR_COMPUTER_VISION_ENDPOINT_HERE", "PASTE_YOUR_COMPUTER_VISION_KEY_HERE");

    // specify the URL of the image to analyze
    std::shared_ptr<VisionSource> imageSource = VisionSource::FromUrl("https://learn.microsoft.com/azure/cognitive-services/computer-vision/media/quickstarts/presentation.png");

    // Creates the options object that will control the ImageAnalyzer
    std::shared_ptr<ImageAnalysisOptions> analysisOptions = ImageAnalysisOptions::Create();

    // You must define one or more features to extract during image analysis
    analysisOptions->SetFeatures(
        { ImageAnalysisFeature::Caption,
            ImageAnalysisFeature::Text,
        });

    std::shared_ptr<ImageAnalyzer> analyzer = ImageAnalyzer::Create(serviceOptions, imageSource, analysisOptions);

    std::cout << "Please wait for image analysis results..." << std::endl;
    std::shared_ptr<ImageAnalysisResult> result = analyzer->Analyze();

    if (result->GetReason() == ImageAnalysisResultReason::Analyzed)
    {
        const Nullable<ContentCaption>& captions = result->GetCaption();
        if (captions.HasValue())
        {
            std::cout << " Caption:" << std::endl;
            std::cout << "   \"" << captions.Value().Content;
            std::cout << "\", Confidence " << captions.Value().Confidence << std::endl;
        }

        const Nullable<DetectedText>& detectedText = result->GetText();
        if (detectedText.HasValue())
        {
            std::cout << " Text:\n";
            for (const DetectedTextLine& line : detectedText.Value().Lines)
            {
                std::cout << "   Line: \"" << line.Content << "\"";
                //std::cout << ", Bounding polygon " << PolygonToString(line.BoundingPolygon) << "}\n";

                for (const DetectedTextWord& word : line.Words)
                {
                    std::cout << "     Word: \"" << word.Content << "\"";
                    //std::cout << ", Bounding polygon " << PolygonToString(word.BoundingPolygon);
                    std::cout << ", Confidence " << word.Confidence << std::endl;
                }
            }
        }
    }
    else if (result->GetReason() == ImageAnalysisResultReason::Error)
    {
        std::shared_ptr<ImageAnalysisErrorDetails> errorDetails = ImageAnalysisErrorDetails::FromResult(result);
        std::cout << "Analysis failed." << std::endl;
        std::cout << "   Error reason = " << (int)errorDetails->GetReason() << std::endl;
        std::cout << "   Error code = " << errorDetails->GetErrorCode() << std::endl;
        std::cout << "   Error message = " << errorDetails->GetMessage() << std::endl;
        std::cout << " Did you set the computer vision endpoint and key?" << std::endl;
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