import http.client, os
from urllib.parse import urlparse

key_var_name = 'QNA_MAKER_SUBSCRIPTION_KEY'
if not key_var_name in os.environ:
	raise Exception('Please set/export the environment variable: {}'.format(key_var_name))
subscription_key = os.environ[key_var_name]

authoring_endpoint_var_name = 'QNA_MAKER_ENDPOINT'
if not authoring_endpoint_var_name in os.environ:
	raise Exception('Please set/export the environment variable: {}'.format(authoring_endpoint_var_name))
# Note http.client.HTTPSConnection wants only the host name, not the protocol (that is, 'https://')
authoring_endpoint = urlparse(os.environ[authoring_endpoint_var_name]).netloc

kb_var_name = 'QNA_MAKER_KB_ID'
if not kb_var_name in os.environ:
	raise Exception('Please set/export the environment variable: {}'.format(kb_var_name))
kb_id = os.environ[kb_var_name]

publish_kb_method = '/qnamaker/v4.0/knowledgebases/' + kb_id

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
