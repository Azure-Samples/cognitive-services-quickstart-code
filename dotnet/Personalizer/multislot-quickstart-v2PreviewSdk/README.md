# About this Quickstart

Multi-slot personalization (Preview) allows you to target content in web layouts, carousels, and lists where more than one action (such as a product or piece of content) is shown to your users. With Personalizer multi-slot APIs, you can have the AI models in Personalizer learn what user contexts and products drive certain behaviors, considering and learning from the placement in your user interface. For example, Personalizer may learn that certain products or content drive more clicks as a sidebar or a footer than as a main highlight on a page.

This sample asks for the time of day and device type to determine which items to display on a retail app/website. You can select if that top choice is what you would pick..

# To try this sample

## Prerequisites

The solution is a C# .NET console app project, so you will need [.NET 5.0](https://dotnet.microsoft.com/download/dotnet/5.0), and [Visual Studio 2019](https://visualstudio.microsoft.com/vs/).

## Upgrade Persoanlizer instance to multi-slot

 1. Configure your Personalizer instance for multi-slot (see [Setting up](https://docs.microsoft.com/en-us/azure/cognitive-services/personalizer/how-to-multi-slot?pivots=programming-language-csharp))

## Set up the sample

- Clone the Azure Personalizer Samples repo.

    ```bash
    git clone https://github.com/Azure-Samples/cognitive-services-quickstart-code.git
    ```

- Navigate to _dotnet/Personalizer/multislot-quickstart-v2PreviewSdk_.


## Set up Azure Personalizer Service

- Create a Personalizer instance in the Azure portal.

- You can find your key and endpoint in the resource's key and endpoint page, under resource management (Keys and Endpoint).

1. Update PersonalizationBaseUrl value ("<REPLACE-WITH-YOUR-PERSONALIZER-ENDPOINT>") in Program.cs with the endpoint specific to your Personalizer service instance.

1. Update ResourceKey value ("<REPLACE-WITH-YOUR-PERSONALIZER-KEY>") in Program.cs with the key specific to your Personalizer service instance.

## Run the sample

Build and run the sample by pressing **F5** in Visual Studio, or `dotnet buld` then `dotnet run` in the same directory as multislot-quickstart.csproj, if using .NET Core CLI. The app will take input from the user interactively and send the data to the Personalizer instance.
