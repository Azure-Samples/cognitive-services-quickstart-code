#!/bin/bash

# Call script with following command-line call
#
# `bash get-answer-from-runtim.sh param-value-1 param-value-2 param-value-3`
# `bash get-answer-from-runtim.sh my-resource-name 012345 898989889`

# Param $1 is your QnA Maker resource name
# Param $2 is your QnA Maker endpoint key
# Param $3 is your knowledge base id

curl https://$1.azurewebsites.net/qnamaker/knowledgebases/$3/generateAnswer \
-X POST \
-H "Authorization: EndpointKey $2" \
-H "Content-Type:application/json" \
-H "Content-Size:159" \
-d '{"question": "qna maker and luis","top": 6,"isTest": true,  "scoreThreshold": 20, "strictFilters": [], "userId": "sd53lsY="}'