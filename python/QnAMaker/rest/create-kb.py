# <dependencies>
import http.client, json, os, time
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

# <constants>
subscription_key = 'PASTE_YOUR_QNA_MAKER_SUBSCRIPTION_KEY_HERE'

# Note http.client.HTTPSConnection wants only the host name, not the protocol (that is, 'https://')
authoring_endpoint = urlparse('PASTE_YOUR_QNA_MAKER_ENDPOINT_HERE').netloc

create_kb_method = '/qnamaker/v4.0/knowledgebases/create'
# </constants>

# <model>
kb_model = {
  "name": "QnA Maker FAQ",
  "qnaList": [
    {
      "id": 0,
      "answer": "You can use our REST APIs to manage your Knowledge Base. See here for details: https://westus.dev.cognitive.microsoft.com/docs/services/58994a073d9e04097c7ba6fe/operations/58994a073d9e041ad42d9baa",
      "source": "Custom Editorial",
      "questions": [
        "How do I programmatically update my Knowledge Base?"
      ],
      "metadata": [
        {
          "name": "category",
          "value": "api"
        }
      ]
    }
  ],
  "urls": [
    "https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/faqs"
  ],
  "files": []
}

# Convert the request to a string.
content = json.dumps(kb_model)
# </model>

# <pretty>
def pretty_print(content):
  # Note: We convert content to and from an object so we can pretty-print it.
  return json.dumps(json.loads(content), indent=4)
# </pretty>

# <create_kb>
def create_kb(create_kb_method, content):
  print('Calling ' + authoring_endpoint + create_kb_method + '.')
  headers = {
    'Ocp-Apim-Subscription-Key': subscription_key,
    'Content-Type': 'application/json',
    'Content-Length': len (content)
  }
  conn = http.client.HTTPSConnection(authoring_endpoint)
  conn.request ("POST", create_kb_method, content, headers)
  response = conn.getresponse ()

  return response.getheader('Location'), response.read ()
# </create_kb>

# <get_status>
def check_status(check_status_method):
  print('Calling ' + authoring_endpoint + check_status_method + '.')
  headers = {'Ocp-Apim-Subscription-Key': subscription_key}
  conn = http.client.HTTPSConnection(authoring_endpoint)
  conn.request("GET", check_status_method, None, headers)
  response = conn.getresponse ()
  return response.read ()
# </get_status>

# <main>
# Call create_kb
operation, result = create_kb(create_kb_method, content)
print(pretty_print(result))

# Add operation ID to URL route
check_status_method = '/qnamaker/v4.0' + operation

# Set done to false
done = False

# Continue until done
while False == done:

  # Gets the status of the operation.
  status = check_status(check_status_method)

  # Print status checks in JSON with presentable formatting
  print(pretty_print(status))

  # Convert the JSON response into an object and get the value of the operationState field.
  state = json.loads(status)['operationState']

  # If the operation isn't finished, wait and query again.
  if state == 'Running' or state == 'NotStarted':
    print('Waiting 10 seconds...')
    time.sleep(10)
  else:
    done = True # request has been processed, if successful, knowledge base is created
# </main>
