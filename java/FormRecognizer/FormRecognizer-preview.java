// <snippet_imports>
import com.azure.ai.formrecognizer.*;
import com.azure.ai.formrecognizer.training.*;
import com.azure.ai.formrecognizer.models.*;
import com.azure.ai.formrecognizer.training.models.*;

import java.util.concurrent.atomic.AtomicReference;
import java.util.List;
import java.util.Map;
import java.time.LocalDate;

import com.azure.core.credential.AzureKeyCredential;
import com.azure.core.http.rest.PagedIterable;
import com.azure.core.util.Context;
import com.azure.core.util.polling.SyncPoller;
// </snippet_imports>

public class FormRecognizer {

    // <snippet_creds>
    static final String key = "<replace-with-your-form-recognizer-key>";
    static final String endpoint = "<replace-with-your-form-recognizer-endpoint>";
    // </snippet_creds>

    public static void main(String[] args) {
        // <snippet_auth>
        FormRecognizerClient recognizerClient = new FormRecognizerClientBuilder()
            .credential(new AzureKeyCredential(key)).endpoint(endpoint).buildClient();

        FormTrainingClient trainingClient = new FormTrainingClientBuilder().credential(new AzureKeyCredential(key))
            .endpoint(endpoint).buildClient();
        // </snippet_auth>

        // <snippet_mainvars>
        String trainingDataUrl = "<SAS-URL-of-your-form-folder-in-blob-storage>";
        String formUrl = "<SAS-URL-of-a-form-in-blob-storage>";
        String receiptUrl = "https://docs.microsoft.com/azure/cognitive-services/form-recognizer/media" +
            "/contoso-allinone.jpg";
        String bcUrl = "https://raw.githubusercontent.com/Azure/azure-sdk-for-python/master/sdk/formrecognizer/azure-ai-formrecognizer/samples/sample_forms/business_cards/business-card-english.jpg";
        String invoiceUrl = "https://raw.githubusercontent.com/Azure/azure-sdk-for-python/master/sdk/formrecognizer/azure-ai-formrecognizer/samples/sample_forms/forms/Invoice_1.pdf";
        String idUrl = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-REST-api-samples/master/curl/form-recognizer/id-license.jpg"
        // </snippet_mainvars>

        // <snippet_maincalls>
        // Call Form Recognizer scenarios:
        System.out.println("Get form content...");
        GetContent(recognizerClient, formUrl);

        System.out.println("Analyze receipt...");
        AnalyzeReceipt(recognizerClient, receiptUrl);

        System.out.println("Analyze business card...");
        AnalyzeBusinessCard(recognizerClient, bcUrl);

        System.out.println("Analyze invoice...");
        AnalyzeInvoice(recognizerClient, invoiceUrl);

        System.out.println("Analyze id...");
        AnalyzeId(recognizerClient, idUrl);

        System.out.println("Train Model with training data...");
        String modelId = TrainModel(trainingClient, trainingDataUrl);

        System.out.println("Analyze PDF form...");
        AnalyzePdfForm(recognizerClient, modelId, formUrl);

        System.out.println("Manage models...");
        ManageModels(trainingClient, trainingDataUrl);
        // </snippet_maincalls>
    }

    // <snippet_getcontent_call>
    private static void GetContent(FormRecognizerClient recognizerClient, String invoiceUri) {
        String analyzeFilePath = invoiceUri;
        SyncPoller < FormRecognizerOperationResult, List < FormPage >> recognizeContentPoller = recognizerClient
            .beginRecognizeContentFromUrl(analyzeFilePath);

        List < FormPage > contentResult = recognizeContentPoller.getFinalResult();
        // </snippet_getcontent_call>
        // <snippet_getcontent_print>
        contentResult.forEach(formPage - > {
            // Table information
            System.out.println("----Recognizing content ----");
            System.out.printf("Has width: %f and height: %f, measured with unit: %s.%n", formPage.getWidth(),
                formPage.getHeight(), formPage.getUnit());
            formPage.getTables().forEach(formTable - > {
                System.out.printf("Table has %d rows and %d columns.%n", formTable.getRowCount(),
                    formTable.getColumnCount());
                formTable.getCells().forEach(formTableCell - > {
                    System.out.printf("Cell has text %s.%n", formTableCell.getText());
                });
                System.out.println();
            });
        });
    }
    // </snippet_getcontent_print>

    // <snippet_receipts_call>
    private static void AnalyzeReceipt(FormRecognizerClient recognizerClient, String receiptUri) {
        SyncPoller < FormRecognizerOperationResult, List < RecognizedForm >> syncPoller = recognizerClient
            .beginRecognizeReceiptsFromUrl(receiptUri);
        List < RecognizedForm > receiptPageResults = syncPoller.getFinalResult();
        // </snippet_receipts_call>
        // <snippet_receipts_print>
        for (int i = 0; i < receiptPageResults.size(); i++) {
            RecognizedForm recognizedForm = receiptPageResults.get(i);
            Map < String, FormField > recognizedFields = recognizedForm.getFields();
            System.out.printf("----------- Recognized Receipt page %d -----------%n", i);
            FormField merchantNameField = recognizedFields.get("MerchantName");
            if (merchantNameField != null) {
                if (FieldValueType.STRING == merchantNameField.getValue().getValueType()) {
                    String merchantName = merchantNameField.getValue().asString();
                    System.out.printf("Merchant Name: %s, confidence: %.2f%n", merchantName,
                        merchantNameField.getConfidence());
                }
            }
            FormField merchantAddressField = recognizedFields.get("MerchantAddress");
            if (merchantAddressField != null) {
                if (FieldValueType.STRING == merchantAddressField.getValue().getValueType()) {
                    String merchantAddress = merchantAddressField.getValue().asString();
                    System.out.printf("Merchant Address: %s, confidence: %.2f%n", merchantAddress,
                        merchantAddressField.getConfidence());
                }
            }
            FormField transactionDateField = recognizedFields.get("TransactionDate");
            if (transactionDateField != null) {
                if (FieldValueType.DATE == transactionDateField.getValue().getValueType()) {
                    LocalDate transactionDate = transactionDateField.getValue().asDate();
                    System.out.printf("Transaction Date: %s, confidence: %.2f%n", transactionDate,
                        transactionDateField.getConfidence());
                }
            }
            // </snippet_receipts_print>
            // <snippet_receipts_print_items>
            FormField receiptItemsField = recognizedFields.get("Items");
            if (receiptItemsField != null) {
                System.out.printf("Receipt Items: %n");
                if (FieldValueType.LIST == receiptItemsField.getValue().getValueType()) {
                    List < FormField > receiptItems = receiptItemsField.getValue().asList();
                    receiptItems.stream()
                        .filter(receiptItem - > FieldValueType.MAP == receiptItem.getValue().getValueType())
                        .map(formField - > formField.getValue().asMap())
                        .forEach(formFieldMap - > formFieldMap.forEach((key, formField) - > {
                            if ("Name".equals(key)) {
                                if (FieldValueType.STRING == formField.getValue().getValueType()) {
                                    String name = formField.getValue().asString();
                                    System.out.printf("Name: %s, confidence: %.2fs%n", name,
                                        formField.getConfidence());
                                }
                            }
                            if ("Quantity".equals(key)) {
                                if (FieldValueType.FLOAT == formField.getValue().getValueType()) {
                                    Float quantity = formField.getValue().asFloat();
                                    System.out.printf("Quantity: %f, confidence: %.2f%n", quantity,
                                        formField.getConfidence());
                                }
                            }
                            if ("Price".equals(key)) {
                                if (FieldValueType.FLOAT == formField.getValue().getValueType()) {
                                    Float price = formField.getValue().asFloat();
                                    System.out.printf("Price: %f, confidence: %.2f%n", price,
                                        formField.getConfidence());
                                }
                            }
                            if ("TotalPrice".equals(key)) {
                                if (FieldValueType.FLOAT == formField.getValue().getValueType()) {
                                    Float totalPrice = formField.getValue().asFloat();
                                    System.out.printf("Total Price: %f, confidence: %.2f%n", totalPrice,
                                        formField.getConfidence());
                                }
                            }
                        }));
                }
            }
        }
    }
    // </snippet_receipts_print_items>

    // <snippet_bc_call>
    private static void AnalyzeBusinessCard(FormRecognizerClient recognizerClient, String bcUrl) {
        SyncPoller < FormRecognizerOperationResult, List < RecognizedForm >> recognizeBusinessCardPoller = client
            .beginRecognizeBusinessCardsFromUrl(businessCardUrl);

        List < RecognizedForm > businessCardPageResults = recognizeBusinessCardPoller.getFinalResult();
        // </snippet_bc_call>
        // <snippet_bc_print>
        for (int i = 0; i < businessCardPageResults.size(); i++) {
            RecognizedForm recognizedForm = businessCardPageResults.get(i);
            Map < String, FormField > recognizedFields = recognizedForm.getFields();
            System.out.printf("----------- Recognized business card info for page %d -----------%n", i);
            FormField contactNamesFormField = recognizedFields.get("ContactNames");
            if (contactNamesFormField != null) {
                if (FieldValueType.LIST == contactNamesFormField.getValue().getValueType()) {
                    List < FormField > contactNamesList = contactNamesFormField.getValue().asList();
                    contactNamesList.stream()
                        .filter(contactName - > FieldValueType.MAP == contactName.getValue().getValueType())
                        .map(contactName - > {
                            System.out.printf("Contact name: %s%n", contactName.getValueData().getText());
                            return contactName.getValue().asMap();
                        }).forEach(contactNamesMap - > contactNamesMap.forEach((key, contactName) - > {
                            if ("FirstName".equals(key)) {
                                if (FieldValueType.STRING == contactName.getValue().getValueType()) {
                                    String firstName = contactName.getValue().asString();
                                    System.out.printf("\tFirst Name: %s, confidence: %.2f%n", firstName,
                                        contactName.getConfidence());
                                }
                            }
                            if ("LastName".equals(key)) {
                                if (FieldValueType.STRING == contactName.getValue().getValueType()) {
                                    String lastName = contactName.getValue().asString();
                                    System.out.printf("\tLast Name: %s, confidence: %.2f%n", lastName,
                                        contactName.getConfidence());
                                }
                            }
                        }));
                }
            }

            FormField jobTitles = recognizedFields.get("JobTitles");
            if (jobTitles != null) {
                if (FieldValueType.LIST == jobTitles.getValue().getValueType()) {
                    List < FormField > jobTitlesItems = jobTitles.getValue().asList();
                    jobTitlesItems.stream().forEach(jobTitlesItem - > {
                        if (FieldValueType.STRING == jobTitlesItem.getValue().getValueType()) {
                            String jobTitle = jobTitlesItem.getValue().asString();
                            System.out.printf("Job Title: %s, confidence: %.2f%n", jobTitle,
                                jobTitlesItem.getConfidence());
                        }
                    });
                }
            }

            FormField departments = recognizedFields.get("Departments");
            if (departments != null) {
                if (FieldValueType.LIST == departments.getValue().getValueType()) {
                    List < FormField > departmentsItems = departments.getValue().asList();
                    departmentsItems.stream().forEach(departmentsItem - > {
                        if (FieldValueType.STRING == departmentsItem.getValue().getValueType()) {
                            String department = departmentsItem.getValue().asString();
                            System.out.printf("Department: %s, confidence: %.2f%n", department,
                                departmentsItem.getConfidence());
                        }
                    });
                }
            }

            FormField emails = recognizedFields.get("Emails");
            if (emails != null) {
                if (FieldValueType.LIST == emails.getValue().getValueType()) {
                    List < FormField > emailsItems = emails.getValue().asList();
                    emailsItems.stream().forEach(emailsItem - > {
                        if (FieldValueType.STRING == emailsItem.getValue().getValueType()) {
                            String email = emailsItem.getValue().asString();
                            System.out.printf("Email: %s, confidence: %.2f%n", email, emailsItem.getConfidence());
                        }
                    });
                }
            }

            FormField websites = recognizedFields.get("Websites");
            if (websites != null) {
                if (FieldValueType.LIST == websites.getValue().getValueType()) {
                    List < FormField > websitesItems = websites.getValue().asList();
                    websitesItems.stream().forEach(websitesItem - > {
                        if (FieldValueType.STRING == websitesItem.getValue().getValueType()) {
                            String website = websitesItem.getValue().asString();
                            System.out.printf("Web site: %s, confidence: %.2f%n", website,
                                websitesItem.getConfidence());
                        }
                    });
                }
            }

            FormField mobilePhones = recognizedFields.get("MobilePhones");
            if (mobilePhones != null) {
                if (FieldValueType.LIST == mobilePhones.getValue().getValueType()) {
                    List < FormField > mobilePhonesItems = mobilePhones.getValue().asList();
                    mobilePhonesItems.stream().forEach(mobilePhonesItem - > {
                        if (FieldValueType.PHONE_NUMBER == mobilePhonesItem.getValue().getValueType()) {
                            String mobilePhoneNumber = mobilePhonesItem.getValue().asPhoneNumber();
                            System.out.printf("Mobile phone number: %s, confidence: %.2f%n", mobilePhoneNumber,
                                mobilePhonesItem.getConfidence());
                        }
                    });
                }
            }

            FormField otherPhones = recognizedFields.get("OtherPhones");
            if (otherPhones != null) {
                if (FieldValueType.LIST == otherPhones.getValue().getValueType()) {
                    List < FormField > otherPhonesItems = otherPhones.getValue().asList();
                    otherPhonesItems.stream().forEach(otherPhonesItem - > {
                        if (FieldValueType.PHONE_NUMBER == otherPhonesItem.getValue().getValueType()) {
                            String otherPhoneNumber = otherPhonesItem.getValue().asPhoneNumber();
                            System.out.printf("Other phone number: %s, confidence: %.2f%n", otherPhoneNumber,
                                otherPhonesItem.getConfidence());
                        }
                    });
                }
            }

            FormField faxes = recognizedFields.get("Faxes");
            if (faxes != null) {
                if (FieldValueType.LIST == faxes.getValue().getValueType()) {
                    List < FormField > faxesItems = faxes.getValue().asList();
                    faxesItems.stream().forEach(faxesItem - > {
                        if (FieldValueType.PHONE_NUMBER == faxesItem.getValue().getValueType()) {
                            String faxPhoneNumber = faxesItem.getValue().asPhoneNumber();
                            System.out.printf("Fax phone number: %s, confidence: %.2f%n", faxPhoneNumber,
                                faxesItem.getConfidence());
                        }
                    });
                }
            }

            FormField addresses = recognizedFields.get("Addresses");
            if (addresses != null) {
                if (FieldValueType.LIST == addresses.getValue().getValueType()) {
                    List < FormField > addressesItems = addresses.getValue().asList();
                    addressesItems.stream().forEach(addressesItem - > {
                        if (FieldValueType.STRING == addressesItem.getValue().getValueType()) {
                            String address = addressesItem.getValue().asString();
                            System.out.printf("Address: %s, confidence: %.2f%n", address,
                                addressesItem.getConfidence());
                        }
                    });
                }
            }

            FormField companyName = recognizedFields.get("CompanyNames");
            if (companyName != null) {
                if (FieldValueType.LIST == companyName.getValue().getValueType()) {
                    List < FormField > companyNameItems = companyName.getValue().asList();
                    companyNameItems.stream().forEach(companyNameItem - > {
                        if (FieldValueType.STRING == companyNameItem.getValue().getValueType()) {
                            String companyNameValue = companyNameItem.getValue().asString();
                            System.out.printf("Company name: %s, confidence: %.2f%n", companyNameValue,
                                companyNameItem.getConfidence());
                        }
                    });
                }
            }
        }
    }
    // </snippet_bc_print>

    // <snippet_invoice_call>
    private static void AnalyzeInvoice(FormRecognizerClient recognizerClient, String invoiceUrl) {
        SyncPoller < FormRecognizerOperationResult, List < RecognizedForm >> recognizeInvoicesPoller = client
            .beginRecognizeInvoicesFromUrl(invoiceUrl);

        List < RecognizedForm > recognizedInvoices = recognizeInvoicesPoller.getFinalResult();
        // </snippet_invoice_call>
        // <snippet_invoice_print>
        for (int i = 0; i < recognizedInvoices.size(); i++) {
            RecognizedForm recognizedInvoice = recognizedInvoices.get(i);
            Map < String, FormField > recognizedFields = recognizedInvoice.getFields();
            System.out.printf("----------- Recognized invoice info for page %d -----------%n", i);
            FormField vendorNameField = recognizedFields.get("VendorName");
            if (vendorNameField != null) {
                if (FieldValueType.STRING == vendorNameField.getValue().getValueType()) {
                    String merchantName = vendorNameField.getValue().asString();
                    System.out.printf("Vendor Name: %s, confidence: %.2f%n", merchantName,
                        vendorNameField.getConfidence());
                }
            }

            FormField vendorAddressField = recognizedFields.get("VendorAddress");
            if (vendorAddressField != null) {
                if (FieldValueType.STRING == vendorAddressField.getValue().getValueType()) {
                    String merchantAddress = vendorAddressField.getValue().asString();
                    System.out.printf("Vendor address: %s, confidence: %.2f%n", merchantAddress,
                        vendorAddressField.getConfidence());
                }
            }

            FormField customerNameField = recognizedFields.get("CustomerName");
            if (customerNameField != null) {
                if (FieldValueType.STRING == customerNameField.getValue().getValueType()) {
                    String merchantAddress = customerNameField.getValue().asString();
                    System.out.printf("Customer Name: %s, confidence: %.2f%n", merchantAddress,
                        customerNameField.getConfidence());
                }
            }

            FormField customerAddressRecipientField = recognizedFields.get("CustomerAddressRecipient");
            if (customerAddressRecipientField != null) {
                if (FieldValueType.STRING == customerAddressRecipientField.getValue().getValueType()) {
                    String customerAddr = customerAddressRecipientField.getValue().asString();
                    System.out.printf("Customer Address Recipient: %s, confidence: %.2f%n", customerAddr,
                        customerAddressRecipientField.getConfidence());
                }
            }

            FormField invoiceIdField = recognizedFields.get("InvoiceId");
            if (invoiceIdField != null) {
                if (FieldValueType.STRING == invoiceIdField.getValue().getValueType()) {
                    String invoiceId = invoiceIdField.getValue().asString();
                    System.out.printf("Invoice Id: %s, confidence: %.2f%n", invoiceId, invoiceIdField.getConfidence());
                }
            }

            FormField invoiceDateField = recognizedFields.get("InvoiceDate");
            if (customerNameField != null) {
                if (FieldValueType.DATE == invoiceDateField.getValue().getValueType()) {
                    LocalDate invoiceDate = invoiceDateField.getValue().asDate();
                    System.out.printf("Invoice Date: %s, confidence: %.2f%n", invoiceDate,
                        invoiceDateField.getConfidence());
                }
            }

            FormField invoiceTotalField = recognizedFields.get("InvoiceTotal");
            if (customerAddressRecipientField != null) {
                if (FieldValueType.FLOAT == invoiceTotalField.getValue().getValueType()) {
                    Float invoiceTotal = invoiceTotalField.getValue().asFloat();
                    System.out.printf("Invoice Total: %.2f, confidence: %.2f%n", invoiceTotal,
                        invoiceTotalField.getConfidence());
                }
            }
        }
    }
    // </snippet_invoice_print>

    //<snippet_id_call>
    private static void AnalyzeId(FormRecognizerClient recognizerClient, String idUrl) {
        SyncPoller < FormRecognizerOperationResult, List < RecognizedForm >> analyzeIDDocumentPoller = client.beginRecognizeIdDocumentsFromUrl(idUrl);

        List < RecognizedForm > idDocumentResults = analyzeIDDocumentPoller.getFinalResult();
    //</snippet_id_call>

   //<snippet_id_print>
        for (int i = 0; i < idDocumentResults.size(); i++) {
            RecognizedForm recognizedForm = idDocumentResults.get(i);
            Map < String, FormField > recognizedFields = recognizedForm.getFields();
            System.out.printf("----------- Recognized license info for page %d -----------%n", i);
            FormField addressField = recognizedFields.get("Address");
            if (addressField != null) {
                if (FieldValueType.STRING == addressField.getValue().getValueType()) {
                    String address = addressField.getValue().asString();
                    System.out.printf("Address: %s, confidence: %.2f%n",
                        address, addressField.getConfidence());
                }
            }

            FormField countryFormField = recognizedFields.get("Country");
            if (countryFormField != null) {
                if (FieldValueType.STRING == countryFormField.getValue().getValueType()) {
                    String country = countryFormField.getValue().asCountry();
                    System.out.printf("Country: %s, confidence: %.2f%n",
                        country, countryFormField.getConfidence());
                }
            }

            FormField dateOfBirthField = recognizedFields.get("DateOfBirth");
            if (dateOfBirthField != null) {
                if (FieldValueType.DATE == dateOfBirthField.getValue().getValueType()) {
                    LocalDate dateOfBirth = dateOfBirthField.getValue().asDate();
                    System.out.printf("Date of Birth: %s, confidence: %.2f%n",
                        dateOfBirth, dateOfBirthField.getConfidence());
                }
            }

            FormField dateOfExpirationField = recognizedFields.get("DateOfExpiration");
            if (dateOfExpirationField != null) {
                if (FieldValueType.DATE == dateOfExpirationField.getValue().getValueType()) {
                    LocalDate expirationDate = dateOfExpirationField.getValue().asDate();
                    System.out.printf("Document date of expiration: %s, confidence: %.2f%n",
                        expirationDate, dateOfExpirationField.getConfidence());
                }
            }

            FormField documentNumberField = recognizedFields.get("DocumentNumber");
            if (documentNumberField != null) {
                if (FieldValueType.STRING == documentNumberField.getValue().getValueType()) {
                    String documentNumber = documentNumberField.getValue().asString();
                    System.out.printf("Document number: %s, confidence: %.2f%n",
                        documentNumber, documentNumberField.getConfidence());
                }
            }

            FormField firstNameField = recognizedFields.get("FirstName");
            if (firstNameField != null) {
                if (FieldValueType.STRING == firstNameField.getValue().getValueType()) {
                    String firstName = firstNameField.getValue().asString();
                    System.out.printf("First Name: %s, confidence: %.2f%n",
                        firstName, documentNumberField.getConfidence());
                }
            }

            FormField lastNameField = recognizedFields.get("LastName");
            if (lastNameField != null) {
                if (FieldValueType.STRING == lastNameField.getValue().getValueType()) {
                    String lastName = lastNameField.getValue().asString();
                    System.out.printf("Last name: %s, confidence: %.2f%n",
                        lastName, lastNameField.getConfidence());
                }
            }

            FormField regionField = recognizedFields.get("Region");
            if (regionField != null) {
                if (FieldValueType.STRING == regionField.getValue().getValueType()) {
                    String region = regionField.getValue().asString();
                    System.out.printf("Region: %s, confidence: %.2f%n",
                        region, regionField.getConfidence());
                }
            }
        }
        //</snippet_id_print>

        // <snippet_train_call>
        private static String TrainModel(FormTrainingClient trainingClient, String trainingDataUrl) {
            SyncPoller < FormRecognizerOperationResult, CustomFormModel > trainingPoller = trainingClient
                .beginTraining(trainingDataUrl, false);

            CustomFormModel customFormModel = trainingPoller.getFinalResult();

            // Model Info
            System.out.printf("Model Id: %s%n", customFormModel.getModelId());
            System.out.printf("Model Status: %s%n", customFormModel.getModelStatus());
            System.out.printf("Training started on: %s%n", customFormModel.getTrainingStartedOn());
            System.out.printf("Training completed on: %s%n%n", customFormModel.getTrainingCompletedOn());
            // </snippet_train_call>
            // <snippet_train_print>
            System.out.println("Recognized Fields:");
            // looping through the subModels, which contains the fields they were trained on
            // Since the given training documents are unlabeled, we still group them but
            // they do not have a label.
            customFormModel.getSubmodels().forEach(customFormSubmodel - > {
                // Since the training data is unlabeled, we are unable to return the accuracy of
                // this model
                System.out.printf("The subModel has form type %s%n", customFormSubmodel.getFormType());
                customFormSubmodel.getFields().forEach((field, customFormModelField) - > System.out
                    .printf("The model found field '%s' with label: %s%n", field, customFormModelField.getLabel()));
            });
            // </snippet_train_print>
            // <snippet_train_return>
            return customFormModel.getModelId();
        }
        // </snippet_train_return>

        // <snippet_trainlabels_call>
        private static String TrainModelWithLabels(FormTrainingClient trainingClient, String trainingDataUrl) {
            // Train custom model
            String trainingSetSource = trainingDataUrl;
            SyncPoller < FormRecognizerOperationResult, CustomFormModel > trainingPoller = trainingClient
                .beginTraining(trainingSetSource, true);

            CustomFormModel customFormModel = trainingPoller.getFinalResult();

            // Model Info
            System.out.printf("Model Id: %s%n", customFormModel.getModelId());
            System.out.printf("Model Status: %s%n", customFormModel.getModelStatus());
            System.out.printf("Training started on: %s%n", customFormModel.getTrainingStartedOn());
            System.out.printf("Training completed on: %s%n%n", customFormModel.getTrainingCompletedOn());
            // </snippet_trainlabels_call>

            // <snippet_trainlabels_print>
            // looping through the subModels, which contains the fields they were trained on
            // The labels are based on the ones you gave the training document.
            System.out.println("Recognized Fields:");
            // Since the data is labeled, we are able to return the accuracy of the model
            customFormModel.getSubmodels().forEach(customFormSubmodel - > {
                System.out.printf("The subModel with form type %s has accuracy: %.2f%n", customFormSubmodel.getFormType(),
                    customFormSubmodel.getAccuracy());
                customFormSubmodel.getFields()
                .forEach((label, customFormModelField) - > System.out.printf(
                    "The model found field '%s' to have name: %s with an accuracy: %.2f%n", label,
                    customFormModelField.getName(), customFormModelField.getAccuracy()));
            });
            return customFormModel.getModelId();
        }
        // </snippet_trainlabels_print>

        // <snippet_analyze_call>
        // Analyze PDF form data
        private static void AnalyzePdfForm(FormRecognizerClient formClient, String modelId, String pdfFormUrl) {
            SyncPoller < FormRecognizerOperationResult, List < RecognizedForm >> recognizeFormPoller = formClient
                .beginRecognizeCustomFormsFromUrl(modelId, pdfFormUrl);

            List < RecognizedForm > recognizedForms = recognizeFormPoller.getFinalResult();
            // </snippet_analyze_call>

            // <snippet_analyze_print>
            for (int i = 0; i < recognizedForms.size(); i++) {
                final RecognizedForm form = recognizedForms.get(i);
                System.out.printf("----------- Recognized custom form info for page %d -----------%n", i);
                System.out.printf("Form type: %s%n", form.getFormType());
                form.getFields().forEach((label, formField) - >
                    // label data is populated if you are using a model trained with unlabeled data,
                    // since the service needs to make predictions for labels if not explicitly
                    // given to it.
                    System.out.printf("Field '%s' has label '%s' with a confidence " + "score of %.2f.%n", label,
                        formField.getLabelData().getText(), formField.getConfidence()));
            }
        }
        // </snippet_analyze_print>

        // <snippet_manage>
        private static void ManageModels(FormTrainingClient trainingClient, String trainingFileUrl) {
            // </snippet_manage>
            // <snippet_manage_count>
            AtomicReference < String > modelId = new AtomicReference < > ();

            // First, we see how many custom models we have, and what our limit is
            AccountProperties accountProperties = trainingClient.getAccountProperties();
            System.out.printf("The account has %s custom models, and we can have at most %s custom models",
                accountProperties.getCustomModelCount(), accountProperties.getCustomModelLimit());
            // </snippet_manage_count>
            // <snippet_manage_list>
            // Next, we get a paged list of all of our custom models
            PagedIterable < CustomFormModelInfo > customModels = trainingClient.listCustomModels();
            System.out.println("We have following models in the account:");
            customModels.forEach(customFormModelInfo - > {
                System.out.printf("Model Id: %s%n", customFormModelInfo.getModelId());
                // get custom model info
                modelId.set(customFormModelInfo.getModelId());
                CustomFormModel customModel = trainingClient.getCustomModel(customFormModelInfo.getModelId());
                System.out.printf("Model Id: %s%n", customModel.getModelId());
                System.out.printf("Model Status: %s%n", customModel.getModelStatus());
                System.out.printf("Training started on: %s%n", customModel.getTrainingStartedOn());
                System.out.printf("Training completed on: %s%n", customModel.getTrainingCompletedOn());
                customModel.getSubmodels().forEach(customFormSubmodel - > {
                    System.out.printf("Custom Model Form type: %s%n", customFormSubmodel.getFormType());
                    System.out.printf("Custom Model Accuracy: %.2f%n", customFormSubmodel.getAccuracy());
                    if (customFormSubmodel.getFields() != null) {
                        customFormSubmodel.getFields().forEach((fieldText, customFormModelField) - > {
                            System.out.printf("Field Text: %s%n", fieldText);
                            System.out.printf("Field Accuracy: %.2f%n", customFormModelField.getAccuracy());
                        });
                    }
                });
            });
            // </snippet_manage_list>
            // <snippet_manage_delete>
            // Delete Custom Model
            System.out.printf("Deleted model with model Id: %s, operation completed with status: %s%n", modelId.get(),
                trainingClient.deleteModelWithResponse(modelId.get(), Context.NONE).getStatusCode());
        }
        // </snippet_manage_delete>
    }