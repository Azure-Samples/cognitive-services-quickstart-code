# <dependencies>
import http.client, os, sys
from urllib.parse import urlparse
# </dependencies>

# Set the `subscription_key` and `authoring_endpoint` variables to your
# QnA Maker authoring subscription key and endpoint.
#
# These values can be found in the Azure portal (ms.portal.azure.com/).
# Look up your QnA Maker resource. Then, in the "Resource management"
# section, find the "Keys and Endpoint" page.
#
# The value of `authoring_endpoint` has the format https://YOUR-RESOURCE-NAME.cognitiveservices.azure.com.
#
# Set the `kb_id` variable to the ID of a knowledge base you have
# previously created.

# <constants>
subscription_key = 'PASTE_YOUR_QNA_MAKER_AUTHORING_SUBSCRIPTION_KEY_HERE'

# Note http.client.HTTPSConnection wants only the host name, not the protocol (that is, 'https://')
authoring_endpoint = urlparse('PASTE_YOUR_QNA_MAKER_AUTHORING_ENDPOINT_HERE').netloc

kb_id = 'PASTE_YOUR_QNA_MAKER_KB_ID_HERE'

publish_kb_method = '/qnamaker/v4.0/knowledgebases/' + kb_id
# </constants>

# <main>
try:
  headers = {
    'Ocp-Apim-Subscription-Key': subscription_key
  }

  conn = http.client.HTTPSConnection(authoring_endpoint,port=443)
  conn.request ("POST", publish_kb_method, "", headers)

  response = conn.getresponse ()

# Note status code 204 means success.
  print(response.status)

except :
    print ("Unexpected error:", sys.exc_info()[0])
    print ("Unexpected error:", sys.exc_info()[1])
# </main>
