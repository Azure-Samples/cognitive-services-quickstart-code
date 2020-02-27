#!/bin/bash

# Call script with following command-line call
# Knowledge base must be published first
#
# `bash get-query-endpoint-key.sh param-value-1 param-value2`
# `bash get-query-endpoint-key.sh my-resource-name 012345`

# Param $1 is your QnA Maker resource name
# Param $2 is your QnA Maker resource key

# Results are:
# {
#  "primaryEndpointKey": "73e88a14-694a-44d5-883b-184a68aa8530",
#  "secondaryEndpointKey": "b2c98c16-ca31-4294-8626-6c57454a5063",
#  "installedVersion": "4.0.5",
#  "lastStableVersion": "4.0.6"
#}


curl https://$1.cognitiveservices.azure.com/qnamaker/v4.0/endpointkeys \
-X GET \
-H "Ocp-Apim-Subscription-Key: $2"