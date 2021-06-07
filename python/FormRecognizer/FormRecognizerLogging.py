# <snippet_logging>
import sys
import logging
from azure.ai.formrecognizer import FormRecognizerClient
from azure.core.credentials import AzureKeyCredential

# Create a logger for the 'azure' SDK
logger = logging.getLogger('azure')
logger.setLevel(logging.DEBUG)

# Configure a console output
handler = logging.StreamHandler(stream=sys.stdout)
logger.addHandler(handler)

endpoint = "PASTE_YOUR_FORM_RECOGNIZER_ENDPOINT_HERE"
credential = AzureKeyCredential("PASTE_YOUR_FORM_RECOGNIZER_SUBSCRIPTION_KEY_HERE")

# This client will log detailed information about its HTTP sessions, at DEBUG level
form_recognizer_client = FormRecognizerClient(endpoint, credential, logging_enable=True)
# </snippet_logging>

# <snippet_example>
receiptUrl = "https://raw.githubusercontent.com/Azure/azure-sdk-for-python/master/sdk/formrecognizer/azure-ai-formrecognizer/tests/sample_forms/receipt/contoso-receipt.png"
poller = form_recognizer_client.begin_recognize_receipts_from_url(receiptUrl, logging_enable=True)
# </snippet_example>
