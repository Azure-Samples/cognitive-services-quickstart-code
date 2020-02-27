#!/bin/bash

# Call script with following command-line call
#
# `bash get-operation-status.sh param-value-1 param-value2 param-value-3`
# `bash get-operation-status.sh my-resource-name 012345 678678678`

# Param $1 is your QnA Maker resource name
# Param $2 is your QnA Maker resource key
# Param $3 is your operationId, returned from a previous API call such as create or train

# cURL returns
# {
#   "operationState": "Succeeded",
#   "createdTimestamp": "2020-02-27T04:54:07Z",
#   "lastActionTimestamp": "2020-02-27T04:54:19Z",
#   "resourceLocation": "/knowledgebases/fe3971b7-cfaa-41fa-8d9f-6ceb673eb865",
#   "userId": "f596077b3e0441eb93d5080d6a15c64b",
#   "operationId": "f293f218-d080-48f0-a766-47993e9b26a8"
# }


curl https://$1.cognitiveservices.azure.com/qnamaker/v4.0/operations/$3 \
-X GET \
-H "Ocp-Apim-Subscription-Key: $2"