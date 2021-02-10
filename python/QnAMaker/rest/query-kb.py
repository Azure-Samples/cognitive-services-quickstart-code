# <dependencies>
import http.client, json, os, sys
from urllib.parse import urlparse
# </dependencies>

# Set the `authoring_key` and `authoring_endpoint` variables to your
# QnA Maker authoring subscription key and endpoint.
#
# These values can be found in the Azure portal (ms.portal.azure.com/).
# Look up your QnA Maker resource. Then, in the "Resource management"
# section, find the "Keys and Endpoint" page.
#
# The value of `authoring_endpoint` has the format https://YOUR-RESOURCE-NAME.cognitiveservices.azure.com.
#
# Set the `runtime_endpoint` variable to your QnA Maker runtime endpoint.
# The value of `runtime_endpoint` has the format https://YOUR-RESOURCE-NAME.azurewebsites.net.
#
# Set the `kb_id` variable to the ID of a knowledge base you have
# previously created.

# <constants>
subscription_key = 'PASTE_YOUR_QNA_MAKER_SUBSCRIPTION_KEY_HERE'

# Note http.client.HTTPSConnection wants only the host name, not the protocol (that is, 'https://')
authoring_endpoint = urlparse('PASTE_YOUR_QNA_MAKER_ENDPOINT_HERE').netloc

runtime_endpoint = urlparse('PASTE_YOUR_QNA_MAKER_RUNTIME_ENDPOINT_HERE').netloc

kb_id = 'PASTE_YOUR_QNA_MAKER_KB_ID_HERE'

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
