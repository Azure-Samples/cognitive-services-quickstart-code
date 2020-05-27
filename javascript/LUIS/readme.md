# Create, train, publish, delete a Language understanding app

## Install

```javascript
npm install
```

## Run app creation and management

Set environment variables:

* LUIS_AUTHORING_KEY
* LUIS_AUTHORING_ENDPOINT

```javascript
node luis_authoring_quickstart.js
```

### Sample output

```console
Created LUIS app with ID e137a439-b3e0-4e16-a7a8-a9746e0715f7
Entity Destination created.
Entity Class created.
Entity Flight created.
Intent FindFlights added.
Created 3 entity labels.
Created 3 entity labels.
Created 3 entity labels.
Example utterances added.
Waiting for train operation to finish...
Current model status: ["Queued"]
Current model status: ["InProgress"]
Current model status: ["InProgress"]
Current model status: ["InProgress"]
Current model status: ["Success"]
Application published. Endpoint URL: https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/e137a439-b3e0-4e16-a7a8-a9746e0715f7
Application with ID e137a439-b3e0-4e16-a7a8-a9746e0715f7 deleted. Operation result: Operation Successful
```

## Query runtime to get prediction results

This quickstart uses the public IoT app.

Set environment variables:

* LUIS_RUNTIME_KEY
* LUIS_RUNTIME_ENDPOINT
* LUIS_APP_ID = df67dcdb-c37d-46af-88e1-8b97951ca1c2
* LUIS_APP_SLOT_NAME = production

```javascript
node luis_prediction.js
```

### Sample output

The prediction result returns a JSON object:

```console
{"query":"turn on all lights","prediction":{"topIntent":"HomeAutomation.TurnOn","intents":{"HomeAutomation.TurnOn":{"score":0.5375382},"None":{"score":0.08687421},"HomeAutomation.TurnOff":{"score":0.0207554}},"entities":{"HomeAutomation.Operation":["on"],"$instance":{"HomeAutomation.Operation":[{"type":"HomeAutomation.Operation","text":"on","startIndex":5,"length":2,"score":0.724984169,"modelTypeId":-1,"modelType":"Unknown","recognitionSources":["model"]}]}}}}
```
