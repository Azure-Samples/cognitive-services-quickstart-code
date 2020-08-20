import http.client, urllib.parse, json, time, sys

try:

  # Represents the various elements used to create HTTP request path
  # for QnA Maker operations.
  knowledge_base_id = "YOUR-KNOWLEDGE-BASE-ID";
  resource_key = "YOUR-RESOURCE-KEY";

  host = "YOUR-RESOURCE-NAME.api.cognitive.microsoft.com"
  route = "/qnamaker/v4.0/knowledgebases/" + knowledge_base_id;

  headers = {
    'Ocp-Apim-Subscription-Key': resource_key
  }

  conn = http.client.HTTPSConnection(host,port=443)
  conn.request ("POST", route, "", headers)

  response = conn.getresponse ()

  print(response.status)

except :
    print ("Unexpected error:", sys.exc_info()[0])
    print ("Unexpected error:", sys.exc_info()[1])
