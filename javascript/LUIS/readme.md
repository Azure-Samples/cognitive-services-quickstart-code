# Create, train, publish, delete a Language understanding app

## To use this sample

1. Create Language understanding authoring resource in Azure portal.
1. Get resource's key and host.
1. Copy `.env.sample` into `.env`.
1. Edit values for your key and host into `.env`.
1. Install dependencies with `npm install`.
1. Run sample with `npm start`.

# Install

```javascript
npm install
```

## Run

```javascript
npm start
```

## Sample output

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

