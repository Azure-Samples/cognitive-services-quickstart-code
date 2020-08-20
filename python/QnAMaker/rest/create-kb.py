import http.client, urllib.parse, json, time

# Represents the various elements used to create HTTP request path for QnA Maker operations.
# Replace this with a valid subscription key.
# User host = '<your-resource-name>.cognitiveservices.azure.com' 
host = '<your-resource-name>.cognitiveservices.azure.com'
service = '/qnamaker/v4.0'
method = '/knowledgebases/create'

# Builds the path URL.
path = service + method

# Replace this with a valid subscription key.
subscriptionKey = '<your-qna-maker-subscription-key>'

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
    "https://docs.microsoft.com/en-in/azure/cognitive-services/qnamaker/faqs",
    "https://docs.microsoft.com/en-us/bot-framework/resources-bot-framework-faq"
  ],
  "files": []
}

# Convert the request to a string.
content = json.dumps(kb_model)

def pretty_print(content):
  # Note: We convert content to and from an object so we can pretty-print it.
  return json.dumps(json.loads(content), indent=4)


def create_kb(path, content):
  print('Calling ' + host + path + '.')
  headers = {
    'Ocp-Apim-Subscription-Key': subscriptionKey,
    'Content-Type': 'application/json',
    'Content-Length': len (content)
  }
  conn = http.client.HTTPSConnection(host)
  conn.request ("POST", path, content, headers)
  response = conn.getresponse ()

  return response.getheader('Location'), response.read ()

def check_status(path):
  print('Calling ' + host + path + '.')
  headers = {'Ocp-Apim-Subscription-Key': subscriptionKey}
  conn = http.client.HTTPSConnection(host)
  conn.request("GET", path, None, headers)
  response = conn.getresponse ()
  return response.getheader('Retry-After'), response.read ()

# Call create_kb
operation, result = create_kb(path, content)
print(pretty_print(result))

# Add operation ID to URL route
path = service + operation

# Set done to false
done = False

# Continue until done
while False == done:

  # Gets the status of the operation.
  wait, status = check_status(path)

  # Print status checks in JSON with presentable formatting
  print(pretty_print(status))

  # Convert the JSON response into an object and get the value of the operationState field.
  state = json.loads(status)['operationState']

  # If the operation isn't finished, wait and query again.
  if state == 'Running' or state == 'NotStarted':
    print('Waiting ' + wait + ' seconds...')
    time.sleep(int(wait))
  else:
    done = True # request has been processed, if successful, knowledge base is created
