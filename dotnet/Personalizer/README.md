# About this Quickstart

This interactive sample takes the time of day and the users's taste preference as context, and sends it to an Azure Personalizer instance, which then returns the top personalized food choice, along with the recommendation probability distribution of each food item. The user then inputs whether or not Personalizer predicted correctly, which is data used to improve Personalizer's prediction model.

# To try this sample

## Prerequisites

The solution is a C# .NET Core console app project, so you will need [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1), and [Visual Studio 2019](https://visualstudio.microsoft.com/vs/) or [.NET Core CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/).

## Set up the sample

- Clone the Azure Personalizer Samples repo.

    ```bash
    git clone https://github.com/Azure-Samples/cognitive-services-quickstart-code.git
    ```

- Navigate to _dotnet/Personalizer_.

- Open `PersonalizerExample.sln`, if using Visual Studio.

## Set up Azure Personalizer Service

- Create a Personalizer instance in the Azure portal.

- Set script variables **ApiKey** and **ServiceEndpoint**. These values can be found in your Cognitive Services Quick start tab in the Azure portal. 

## Run the sample

Build and run the sample by pressing **F5** in Visual Studio, or `dotnet buld` then `dotnet run` in the same directory as PersonalizerExample.csproj, if using .NET Core CLI. The app will take input from the user interactively and send the data to the Personalizer instance.
