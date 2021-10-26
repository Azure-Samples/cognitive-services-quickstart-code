# About this Quickstart

This interactive sample takes the time of day and the users's device preference as context, and sends it to an Azure Personalizer instance, which then returns the top personalized item to display in each slot. The user then inputs whether or not Personalizer chose the correct item for each slot, send a reward, which is data used to improve Personalizer's prediction model.

# To try this sample

## Prerequisites

The solution is a C# .NET console app project, so you will need [.NET 5.0](https://dotnet.microsoft.com/download/dotnet/5.0), and [Visual Studio 2019](https://visualstudio.microsoft.com/vs/).

## Set up the sample

- Clone the Azure Personalizer Samples repo.

    ```bash
    git clone https://github.com/Azure-Samples/cognitive-services-quickstart-code.git
    ```

- Navigate to _dotnet/Personalizer/multislot-quickstart-v2PreviewSdk_.


## Set up Azure Personalizer Service

- Create a Personalizer instance in the Azure portal.

- Set script variables **ApiKey** and **ServiceEndpoint**. These values can be found in your Cognitive Services Quick start tab in the Azure portal. 

## Run the sample

Build and run the sample by pressing **F5** in Visual Studio, or `dotnet buld` then `dotnet run` in the same directory as multislot-quickstart.csproj, if using .NET Core CLI. The app will take input from the user interactively and send the data to the Personalizer instance.
