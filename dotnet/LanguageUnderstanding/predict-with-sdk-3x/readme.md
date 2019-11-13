# How to run this sample

The .Net Core file queries a public Language Understanding (LUIS) app and prints the results to the console.

1. Add your own LUIS prediction key, endpoint URL, and app id in the environment variables:

    * LUIS_PREDICTION_KEY
    * LUIS_ENDPOINT
    * LUIS_APP_ID 
    
1. Run `dotnet build` to build the file.
1. Run `dotnet run` to run the file.

The output is:

```console
PS C:\Users\sam\cognitive-services-quickstart-code\dotnet\LanguageUnderstanding\predict-with-sdk-3x> dotnet run
Query:'turn on the bedroom light'
TopIntent :'HomeAutomation.TurnOn'
HomeAutomation.TurnOn:0.1548855
None:0.142956868
HomeAutomation.TurnOff:0.0307567716
done
```