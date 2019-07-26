---
services: cognitive-services, form recognizer
platforms: dotnet, c#
author: 
---

# Cognitive Services Form Recognizer C# SDK quickstart samples

This repository provides the latest sample code for Cognitive Services Form Recognizer SDK quickstarts. These samples target the [Microsoft.Azure.CognitiveServices.Vision.FormRecognizer 0.8.0-preview](https://www.nuget.org/packages/Microsoft.Azure.CognitiveServices.Vision.FormRecognizer/0.8.0-preview) client library.

## Features

These samples demonstrate how to use the Form Recognizer client library for C# to perform the following actions

* Train a custom model
* Analyze form using previously trained custom model


## Getting Started

### Prerequisites

* You need a **subscription key** for the Form Recognizer service to run the samples.  You can get free trial subscription keys from [Try Cognitive Services](https://azure.microsoft.com/try/cognitive-services/).
* Any edition of [Visual Studio 2017](https://www.visualstudio.com/downloads/).

### Quickstart

1. Clone or download the repository.
1. Navigate to the *FormRecognizer* folder.
1. Double-click the *FormRecognizer.sln* file to open the solution in Visual Studio.
1. Install the Form Recognizer client library NuGet package.
   1. On the top menu, click **Tools**, select **NuGet Package Manager**, then **Manage NuGet Packages for Solution**.
   1. Click the **Browse** tab and then select **Include prerelease**.
   1. In the **Search** box type "Microsoft.Azure.CognitiveServices.FormRecognizer".
   1. Select **Microsoft.Azure.CognitiveServices.FormRecognizer** when it displays, then click the checkbox next to your project name, and **Install**.
1. Open *Program.cs*.
1. Replace `<Subscription Key>` with your valid subscription key.
1. Change `formRecognizerEndpoint` to the Azure region associated with your subscription keys, if necessary.
1. Replace <`AzureBlobSaS>` with the SAS for the container in the Azure Blob Storage.
1. Run the program.

## Resources

* [Form Recognizer service documentation](https://docs.microsoft.com/en-us/azure/cognitive-services/form-recognizer/)
* [Form Recognizer API](https://westus2.dev.cognitive.microsoft.com/docs/services/form-recognizer-api)
* [Microsoft.Azure.CognitiveServices.Vision.FormRecognizer 0.8.0-preview](https://www.nuget.org/packages/Microsoft.Azure.CognitiveServices.Vision.FormRecognizer/0.8.0-preview) NuGet package