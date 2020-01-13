# Create, update, publish, delete a knowledge base

## To use this sample

1. Create QnA Maker resource in Azure portal.
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
PS C:\samples\cognitive-services-quickstart-code\javascript\QnAMaker\sdk> npm start

> qnamaker_quickstart@1.0.0 start C:\samples\cognitive-services-quickstart-code\javascript\QnAMaker\sdk
> node qnamaker_quickstart.js

Operation state - Running
Operation state - Running
Operation state - Running
Operation state - Running
Operation state - Running
Operation state - Succeeded
Create operation 200, KB ID 99df758d-f23f-4931-ab83-e738fe978e69
Operation state - Running
Operation state - Running
Operation state - Running
Operation state - Succeeded
Update operation state 200 - HTTP status 200
Publish request succeeded - HTTP status 204
GetEndpointKeys request succeeded - HTTP status 200 - primary key 8482830b-681e-400e-b8a3-4016278aba64
QnA Maker FAQ stored in English language with 1 sources, last updated 2020-01-12T16:54:40Z
New KB name stored in English language with 1 sources, last updated 2020-01-12T17:32:16Z
New KB name stored in English language with 1 sources, last updated 2020-01-13T00:27:46Z
Delete operation state succeeded - HTTP status 204
done
```

