#!/bin/bash

# Call script with following command-line call
#
# `bash delete-knowledge-base.sh param-value-1 param-value-2 param-value-3`
# `bash delete-knowledge-base.sh my-resource-name 012345 9999999`

# Param $1 is your QnA Maker resource name
# Param $2 is your QnA Maker resource key
# Param $3 is your knowledge base id

# -v param to curl requests verbose response which includes the HTTP response
# Response is 204 with no results

curl https://$1.cognitiveservices.azure.com/qnamaker/v4.0/knowledgebases/$3 \
-X DELETE \
-v \
-H "Ocp-Apim-Subscription-Key: $2"
