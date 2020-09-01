# <dependencies>
import http.client, json, os, sys
from urllib.parse import urlparse
# </dependencies>

# <constants>
key_var_name = 'QNA_MAKER_SUBSCRIPTION_KEY'
if not key_var_name in os.environ:
	raise Exception('Please set/export the environment variable: {}'.format(key_var_name))
subscription_key = os.environ[key_var_name]

authoring_endpoint_var_name = 'QNA_MAKER_ENDPOINT'
if not authoring_endpoint_var_name in os.environ:
	raise Exception('Please set/export the environment variable: {}'.format(authoring_endpoint_var_name))
# Note http.client.HTTPSConnection wants only the host name, not the protocol (that is, 'https://')
authoring_endpoint = urlparse(os.environ[authoring_endpoint_var_name]).netloc

runtime_endpoint_var_name = 'QNA_MAKER_RUNTIME_ENDPOINT'
if not runtime_endpoint_var_name in os.environ:
	raise Exception('Please set/export the environment variable: {}'.format(runtime_endpoint_var_name))
runtime_endpoint = urlparse(os.environ[runtime_endpoint_var_name]).netloc

kb_var_name = 'QNA_MAKER_KB_ID'
if not kb_var_name in os.environ:
	raise Exception('Please set/export the environment variable: {}'.format(kb_var_name))
kb_id = os.environ[kb_var_name]

get_endpoint_key_method = "/qnamaker/v4.0/endpointKeys"

query_kb_method = "/qnamaker/knowledgebases/" + kb_id + "/generateAnswer";

# JSON format for passing question to service
question = "{'question': 'Is the QnA Maker Service free?','top': 3}";
# </constants>

# <main>
try:
	authoring_conn = http.client.HTTPSConnection(authoring_endpoint,port=443)
	headers = {
		'Ocp-Apim-Subscription-Key': subscription_key
	}
	authoring_conn.request ("GET", get_endpoint_key_method, "", headers)
	response = authoring_conn.getresponse ()
	endpoint_key = json.loads(response.read())["primaryEndpointKey"]

	runtime_conn = http.client.HTTPSConnection(runtime_endpoint,port=443)
	headers = {
		# Note this differs from the "Ocp-Apim-Subscription-Key"/<subscription key> used by most Cognitive Services.
		'Authorization': 'EndpointKey ' + endpoint_key,
		'Content-Type': 'application/json'
	}
	runtime_conn.request ("POST", query_kb_method, question, headers)
	response = runtime_conn.getresponse ()
	answer = response.read ()
	print(json.dumps(json.loads(answer), indent=4))

except :
    print ("Unexpected error:", sys.exc_info()[0])
    print ("Unexpected error:", sys.exc_info()[1])
# </main>