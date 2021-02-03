# How to run this sample

The Python v3.x files create and query a Language Understanding (LUIS) app, printing SDK method results to the console when appropriate.

## Create LUIS app

1. Add your variables for LUIS as strings.

1. Run `python application_quickstart.py` to run the file.

    The output sent to the console is:

    ```console
    Creating application...
    Created LUIS app Contoso 2020-01-21 22:43:10.741412
        with ID 916a0c7c-27eb-4637-be14-7c3264c65b52

    Adding entities to application...
    destinationEntityId 2c8a1a26-591d-4861-99d2-8a718e526188 added.
    classEntityId fe928009-3e22-4432-be4d-09d05ee3a68d added.
    flightEntityId 90a5eb54-95ed-42c7-af14-45e363ed9ecd added.

    Adding intents to application...
    Intent FindFlights 92dc1db5-8966-4cd8-bfed-1b0eff430756 added.

    Adding utterances to application...
    3 example utterance(s) added.

    Training application...
    Waiting 10 seconds for training to complete...

    Publishing application...
    Application published. Endpoint URL: https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/916a0c7c-27eb-4637-be14-7c3264c65b52
    ```

## Query LUIS app

1. Add your variables for LUIS as strings.

    * LUIS_RUNTIME_KEY
    * LUIS_RUNTIME_ENDPOINT
    * LUIS_APP_ID - The public LUIS IoT app ID is `df67dcdb-c37d-46af-88e1-8b97951ca1c2`.
    * LUIS_APP_SLOT_NAME - The `LUIS_APP_SLOT_NAME` choices are `production` or `staging`.

1. Run `python prediction_quickstart.py` to run the file.

The output is for the `turn on all lights` query is:

```console
    Top intent: HomeAutomation.TurnOn
    Sentiment: None
    Intents:
            "HomeAutomation.TurnOn"
    Entities: {'HomeAutomation.Operation': ['on']}
```
