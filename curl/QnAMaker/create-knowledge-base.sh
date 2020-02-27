#!/bin/bash

# Call script with following command-line call
#
# `bash create-knowledge-base.sh param-value-1 param-value-2`
# `bash create-knowledge-base.sh my-resource-name 012345`

# Param $1 is your QnA Maker resource name
# Param $2 is your QnA Maker resource key
# These values are found in the Azure portal - https://portal.azure.com

# cURL returns
# {
#  "operationState": "NotStarted",
#  "createdTimestamp": "2020-02-27T04:11:22Z",
#  "lastActionTimestamp": "2020-02-27T04:11:22Z",
#  "userId": "9596077b3e0441eb93d5080d6a15c64b",
#  "operationId": "95a4f700-9899-4c98-bda8-5449af9faef8"
#}

# In order to get knowledge base id for continuing operations, get status
# using the operationID

curl https://$1.cognitiveservices.azure.com/qnamaker/v4.0/knowledgebases/create \
-X POST \
-H "Ocp-Apim-Subscription-Key: $2" \
-H "Content-Type:application/json" \
-H "Content-Size:107" \
-d '{ name: "QnA Maker FAQ",urls: [ "https://docs.microsoft.com/en-in/azure/cognitive-services/qnamaker/faqs"]}'