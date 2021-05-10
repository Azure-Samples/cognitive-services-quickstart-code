# <snippet_imports>
import os
from azure.core.exceptions import ResourceNotFoundError
from azure.ai.formrecognizer import FormRecognizerClient
from azure.ai.formrecognizer import FormTrainingClient
from azure.core.credentials import AzureKeyCredential
# </snippet_imports>

# <snippet_creds>
endpoint = "PASTE_YOUR_FORM_RECOGNIZER_ENDPOINT_HERE"
key = "PASTE_YOUR_FORM_RECOGNIZER_SUBSCRIPTION_KEY_HERE"
# </snippet_creds>

# <snippet_auth>
form_recognizer_client = FormRecognizerClient(endpoint, AzureKeyCredential(key))
form_training_client = FormTrainingClient(endpoint, AzureKeyCredential(key))
# </snippet_auth>

# <snippet_getcontent>
formUrl = "https://raw.githubusercontent.com/Azure/azure-sdk-for-python/master/sdk/formrecognizer/azure-ai-formrecognizer/tests/sample_forms/forms/Form_1.jpg"

poller = form_recognizer_client.begin_recognize_content_from_url(formUrl)
page = poller.result()

table = page[0].tables[0] # page 1, table 1
print("Table found on page {}:".format(table.page_number))
for cell in table.cells:
    print("Cell text: {}".format(cell.text))
    print("Location: {}".format(cell.bounding_box))
    print("Confidence score: {}\n".format(cell.confidence))
# </snippet_getcontent>

# <snippet_receipts>
receiptUrl = "https://raw.githubusercontent.com/Azure/azure-sdk-for-python/master/sdk/formrecognizer/azure-ai-formrecognizer/tests/sample_forms/receipt/contoso-receipt.png"

poller = form_recognizer_client.begin_recognize_receipts_from_url(receiptUrl)
result = poller.result()

for receipt in result:
    for name, field in receipt.fields.items():
        if name == "Items":
            print("Receipt Items:")
            for idx, items in enumerate(field.value):
                print("...Item #{}".format(idx + 1))
                for item_name, item in items.value.items():
                    print("......{}: {} has confidence {}".format(item_name, item.value, item.confidence))
        else:
            print("{}: {} has confidence {}".format(name, field.value, field.confidence))
# </snippet_receipts>

# <snippet_bc>
bcUrl = "https://raw.githubusercontent.com/Azure/azure-sdk-for-python/master/sdk/formrecognizer/azure-ai-formrecognizer/samples/sample_forms/business_cards/business-card-english.jpg"

poller = form_recognizer_client.begin_recognize_business_cards_from_url(bcUrl)
business_cards = poller.result()

for idx, business_card in enumerate(business_cards):
    print("--------Recognizing business card #{}--------".format(idx+1))
    contact_names = business_card.fields.get("ContactNames")
    if contact_names:
        for contact_name in contact_names.value:
            print("Contact First Name: {} has confidence: {}".format(
                contact_name.value["FirstName"].value, contact_name.value["FirstName"].confidence
            ))
            print("Contact Last Name: {} has confidence: {}".format(
                contact_name.value["LastName"].value, contact_name.value["LastName"].confidence
            ))
    company_names = business_card.fields.get("CompanyNames")
    if company_names:
        for company_name in company_names.value:
            print("Company Name: {} has confidence: {}".format(company_name.value, company_name.confidence))
    departments = business_card.fields.get("Departments")
    if departments:
        for department in departments.value:
            print("Department: {} has confidence: {}".format(department.value, department.confidence))
    job_titles = business_card.fields.get("JobTitles")
    if job_titles:
        for job_title in job_titles.value:
            print("Job Title: {} has confidence: {}".format(job_title.value, job_title.confidence))
    emails = business_card.fields.get("Emails")
    if emails:
        for email in emails.value:
            print("Email: {} has confidence: {}".format(email.value, email.confidence))
    websites = business_card.fields.get("Websites")
    if websites:
        for website in websites.value:
            print("Website: {} has confidence: {}".format(website.value, website.confidence))
    addresses = business_card.fields.get("Addresses")
    if addresses:
        for address in addresses.value:
            print("Address: {} has confidence: {}".format(address.value, address.confidence))
    mobile_phones = business_card.fields.get("MobilePhones")
    if mobile_phones:
        for phone in mobile_phones.value:
            print("Mobile phone number: {} has confidence: {}".format(phone.value, phone.confidence))
    faxes = business_card.fields.get("Faxes")
    if faxes:
        for fax in faxes.value:
            print("Fax number: {} has confidence: {}".format(fax.value, fax.confidence))
    work_phones = business_card.fields.get("WorkPhones")
    if work_phones:
        for work_phone in work_phones.value:
            print("Work phone number: {} has confidence: {}".format(work_phone.value, work_phone.confidence))
    other_phones = business_card.fields.get("OtherPhones")
    if other_phones:
        for other_phone in other_phones.value:
            print("Other phone number: {} has confidence: {}".format(other_phone.value, other_phone.confidence))
# </snippet_bc>

# <snippet_invoice>
invoiceUrl = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-REST-api-samples/master/curl/form-recognizer/simple-invoice.png"

poller = form_recognizer_client.begin_recognize_invoices_from_url(invoiceUrl)
invoices = poller.result()

for idx, invoice in enumerate(invoices):
    print("--------Recognizing invoice #{}--------".format(idx+1))
    vendor_name = invoice.fields.get("VendorName")
    if vendor_name:
        print("Vendor Name: {} has confidence: {}".format(vendor_name.value, vendor_name.confidence))
    vendor_address = invoice.fields.get("VendorAddress")
    if vendor_address:
        print("Vendor Address: {} has confidence: {}".format(vendor_address.value, vendor_address.confidence))
    customer_name = invoice.fields.get("CustomerName")
    if customer_name:
        print("Customer Name: {} has confidence: {}".format(customer_name.value, customer_name.confidence))
    customer_address = invoice.fields.get("CustomerAddress")
    if customer_address:
        print("Customer Address: {} has confidence: {}".format(customer_address.value, customer_address.confidence))
    customer_address_recipient = invoice.fields.get("CustomerAddressRecipient")
    if customer_address_recipient:
        print("Customer Address Recipient: {} has confidence: {}".format(customer_address_recipient.value, customer_address_recipient.confidence))
    invoice_id = invoice.fields.get("InvoiceId")
    if invoice_id:
        print("Invoice Id: {} has confidence: {}".format(invoice_id.value, invoice_id.confidence))
    invoice_date = invoice.fields.get("InvoiceDate")
    if invoice_date:
        print("Invoice Date: {} has confidence: {}".format(invoice_date.value, invoice_date.confidence))
    invoice_total = invoice.fields.get("InvoiceTotal")
    if invoice_total:
        print("Invoice Total: {} has confidence: {}".format(invoice_total.value, invoice_total.confidence))
    due_date = invoice.fields.get("DueDate")
    if due_date:
        print("Due Date: {} has confidence: {}".format(due_date.value, due_date.confidence))
# </snippet_invoice>


# <snippet_train>
# To train a model you need an Azure Storage account.
# Use the SAS URL to access your training files.
trainingDataUrl = "PASTE_YOUR_SAS_URL_OF_YOUR_FORM_FOLDER_IN_BLOB_STORAGE_HERE"

poller = form_training_client.begin_training(trainingDataUrl, use_training_labels=False)
model = poller.result()

print("Model ID: {}".format(model.model_id))
print("Status: {}".format(model.status))
print("Training started on: {}".format(model.training_started_on))
print("Training completed on: {}".format(model.training_completed_on))

print("\nRecognized fields:")
for submodel in model.submodels:
    print(
        "The submodel with form type '{}' has recognized the following fields: {}".format(
            submodel.form_type,
            ", ".join(
                [
                    field.label if field.label else name
                    for name, field in submodel.fields.items()
                ]
            ),
        )
    )

# Training result information
for doc in model.training_documents:
    print("Document name: {}".format(doc.name))
    print("Document status: {}".format(doc.status))
    print("Document page count: {}".format(doc.page_count))
    print("Document errors: {}".format(doc.errors))
# </snippet_train>

# <snippet_trainlabels>
# To train a model you need an Azure Storage account.
# Use the SAS URL to access your training files.
trainingDataUrl = "PASTE_YOUR_SAS_URL_OF_YOUR_FORM_FOLDER_IN_BLOB_STORAGE_HERE"

poller = form_training_client.begin_training(trainingDataUrl, use_training_labels=True)
model = poller.result()

print("Model ID: {}".format(model.model_id))
print("Status: {}".format(model.status))
print("Training started on: {}".format(model.training_started_on))
print("Training completed on: {}".format(model.training_completed_on))

print("\nRecognized fields:")
for submodel in model.submodels:
    print(
        "The submodel with form type '{}' has recognized the following fields: {}".format(
            submodel.form_type,
            ", ".join(
                [
                    field.label if field.label else name
                    for name, field in submodel.fields.items()
                ]
            ),
        )
    )

# Training result information
for doc in model.training_documents:
    print("Document name: {}".format(doc.name))
    print("Document status: {}".format(doc.status))
    print("Document page count: {}".format(doc.page_count))
    print("Document errors: {}".format(doc.errors))
# </snippet_trainlabels>

# <snippet_analyze>
# Model ID from when you trained your model.
model_id = "<your custom model id>"

poller = form_recognizer_client.begin_recognize_custom_forms_from_url(
    model_id=model_id, form_url=formUrl)
result = poller.result()

for recognized_form in result:
    print("Form type: {}".format(recognized_form.form_type))
    for name, field in recognized_form.fields.items():
        print("Field '{}' has label '{}' with value '{}' and a confidence score of {}".format(
            name,
            field.label_data.text if field.label_data else name,
            field.value,
            field.confidence
        ))
# </snippet_analyze>

# <snippet_manage_count>
account_properties = form_training_client.get_account_properties()
print("Our account has {} custom models, and we can have at most {} custom models".format(
    account_properties.custom_model_count, account_properties.custom_model_limit
))
# </snippet_manage_count>

# <snippet_manage_list>
# Next, we get a paged list of all of our custom models
custom_models = form_training_client.list_custom_models()

print("We have models with the following ids:")

# Let's pull out the first model
first_model = next(custom_models)
print(first_model.model_id)
for model in custom_models:
    print(model.model_id)
# </snippet_manage_list>

# <snippet_manage_getmodel>
model_id = "<model_id from the Train a Model sample>"

custom_model = form_training_client.get_custom_model(model_id=model_id)
print("Model ID: {}".format(custom_model.model_id))
print("Status: {}".format(custom_model.status))
print("Training started on: {}".format(custom_model.training_started_on))
print("Training completed on: {}".format(custom_model.training_completed_on))
# </snippet_manage_getmodel>

# <snippet_manage_delete>
form_training_client.delete_model(model_id=custom_model.model_id)

try:
    form_training_client.get_custom_model(model_id=custom_model.model_id)
except ResourceNotFoundError:
    print("Successfully deleted model with id {}".format(custom_model.model_id))
# </snippet_manage_delete>

# <snippet_id>
idURL = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-REST-api-samples/master/curl/form-recognizer/id-license.jpg"

poller = form_recognizer_client.begin_recognize_id_documents(idURL)
id_documents = poller.result()

for idx, id_document in enumerate(id_documents):
    print("--------Recognizing ID document #{}--------".format(idx+1))
    first_name = id_document.fields.get("FirstName")
    if first_name:
        print("First Name: {} has confidence: {}".format(first_name.value, first_name.confidence))
    last_name = id_document.fields.get("LastName")
    if last_name:
        print("Last Name: {} has confidence: {}".format(last_name.value, last_name.confidence))
    document_number = id_document.fields.get("DocumentNumber")
    if document_number:
        print("Document Number: {} has confidence: {}".format(document_number.value, document_number.confidence))
    dob = id_document.fields.get("DateOfBirth")
    if dob:
        print("Date of Birth: {} has confidence: {}".format(dob.value, dob.confidence))
    doe = id_document.fields.get("DateOfExpiration")
    if doe:
        print("Date of Expiration: {} has confidence: {}".format(doe.value, doe.confidence))
    sex = id_document.fields.get("Sex")
    if sex:
        print("Sex: {} has confidence: {}".format(sex.value, sex.confidence))
    address = id_document.fields.get("Address")
    if address:
        print("Address: {} has confidence: {}".format(address.value, address.confidence))
    country = id_document.fields.get("Country")
    if country:
        print("Country: {} has confidence: {}".format(country.value, country.confidence))
    region = id_document.fields.get("Region")
    if region:
        print("Region: {} has confidence: {}".format(region.value, region.confidence))
# </snippet_id>
