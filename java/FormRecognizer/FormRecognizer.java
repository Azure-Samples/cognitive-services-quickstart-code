
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
        String receiptUrl = "https://docs.microsoft.com/azure/cognitive-services/form-recognizer/media"
                + "/contoso-allinone.jpg";
        // </snippet_mainvars>

        // <snippet_maincalls>
        // Call Form Recognizer scenarios:
        System.out.println("Get form content...");
        GetContent(recognizerClient, formUrl);

        System.out.println("Analyze receipt...");
        AnalyzeReceipt(recognizerClient, receiptUrl);

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
        SyncPoller<FormRecognizerOperationResult, List<FormPage>> recognizeContentPoller = recognizerClient
                .beginRecognizeContentFromUrl(analyzeFilePath);

        List<FormPage> contentResult = recognizeContentPoller.getFinalResult();
        // </snippet_getcontent_call>
        // <snippet_getcontent_print>
        contentResult.forEach(formPage -> {
            // Table information
            System.out.println("----Recognizing content ----");
            System.out.printf("Has width: %f and height: %f, measured with unit: %s.%n", formPage.getWidth(),
                    formPage.getHeight(), formPage.getUnit());
            formPage.getTables().forEach(formTable -> {
                System.out.printf("Table has %d rows and %d columns.%n", formTable.getRowCount(),
                        formTable.getColumnCount());
                formTable.getCells().forEach(formTableCell -> {
                    System.out.printf("Cell has text %s.%n", formTableCell.getText());
                });
                System.out.println();
            });
        });
    }
    // </snippet_getcontent_print>

    // <snippet_receipts_call>
    private static void AnalyzeReceipt(FormRecognizerClient recognizerClient, String receiptUri) {
        SyncPoller<FormRecognizerOperationResult, List<RecognizedForm>> syncPoller = recognizerClient
                .beginRecognizeReceiptsFromUrl(receiptUri);
        List<RecognizedForm> receiptPageResults = syncPoller.getFinalResult();
        // </snippet_receipts_call>
        // <snippet_receipts_print>
        for (int i = 0; i < receiptPageResults.size(); i++) {
            RecognizedForm recognizedForm = receiptPageResults.get(i);
            Map<String, FormField> recognizedFields = recognizedForm.getFields();
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
                    List<FormField> receiptItems = receiptItemsField.getValue().asList();
                    receiptItems.stream()
                            .filter(receiptItem -> FieldValueType.MAP == receiptItem.getValue().getValueType())
                            .map(formField -> formField.getValue().asMap())
                            .forEach(formFieldMap -> formFieldMap.forEach((key, formField) -> {
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

    // <snippet_train_call>
    private static String TrainModel(FormTrainingClient trainingClient, String trainingDataUrl) {
        SyncPoller<FormRecognizerOperationResult, CustomFormModel> trainingPoller = trainingClient
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
        customFormModel.getSubmodels().forEach(customFormSubmodel -> {
            // Since the training data is unlabeled, we are unable to return the accuracy of
            // this model
            System.out.printf("The subModel has form type %s%n", customFormSubmodel.getFormType());
            customFormSubmodel.getFields().forEach((field, customFormModelField) -> System.out
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
        SyncPoller<FormRecognizerOperationResult, CustomFormModel> trainingPoller = trainingClient
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
        customFormModel.getSubmodels().forEach(customFormSubmodel -> {
            System.out.printf("The subModel with form type %s has accuracy: %.2f%n", customFormSubmodel.getFormType(),
                    customFormSubmodel.getAccuracy());
            customFormSubmodel.getFields()
                    .forEach((label, customFormModelField) -> System.out.printf(
                            "The model found field '%s' to have name: %s with an accuracy: %.2f%n", label,
                            customFormModelField.getName(), customFormModelField.getAccuracy()));
        });
        return customFormModel.getModelId();
    }
    // </snippet_trainlabels_print>

    // <snippet_analyze_call>
    // Analyze PDF form data
    private static void AnalyzePdfForm(FormRecognizerClient formClient, String modelId, String pdfFormUrl) {
        SyncPoller<FormRecognizerOperationResult, List<RecognizedForm>> recognizeFormPoller = formClient
                .beginRecognizeCustomFormsFromUrl(modelId, pdfFormUrl);

        List<RecognizedForm> recognizedForms = recognizeFormPoller.getFinalResult();
        // </snippet_analyze_call>

        // <snippet_analyze_print>
        for (int i = 0; i < recognizedForms.size(); i++) {
            final RecognizedForm form = recognizedForms.get(i);
            System.out.printf("----------- Recognized custom form info for page %d -----------%n", i);
            System.out.printf("Form type: %s%n", form.getFormType());
            form.getFields().forEach((label, formField) ->
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
        AtomicReference<String> modelId = new AtomicReference<>();

        // First, we see how many custom models we have, and what our limit is
        AccountProperties accountProperties = trainingClient.getAccountProperties();
        System.out.printf("The account has %s custom models, and we can have at most %s custom models",
                accountProperties.getCustomModelCount(), accountProperties.getCustomModelLimit());
        // </snippet_manage_count>
        // <snippet_manage_list>
        // Next, we get a paged list of all of our custom models
        PagedIterable<CustomFormModelInfo> customModels = trainingClient.listCustomModels();
        System.out.println("We have following models in the account:");
        customModels.forEach(customFormModelInfo -> {
            System.out.printf("Model Id: %s%n", customFormModelInfo.getModelId());
            // get custom model info
            modelId.set(customFormModelInfo.getModelId());
            CustomFormModel customModel = trainingClient.getCustomModel(customFormModelInfo.getModelId());
            System.out.printf("Model Id: %s%n", customModel.getModelId());
            System.out.printf("Model Status: %s%n", customModel.getModelStatus());
            System.out.printf("Training started on: %s%n", customModel.getTrainingStartedOn());
            System.out.printf("Training completed on: %s%n", customModel.getTrainingCompletedOn());
            customModel.getSubmodels().forEach(customFormSubmodel -> {
                System.out.printf("Custom Model Form type: %s%n", customFormSubmodel.getFormType());
                System.out.printf("Custom Model Accuracy: %.2f%n", customFormSubmodel.getAccuracy());
                if (customFormSubmodel.getFields() != null) {
                    customFormSubmodel.getFields().forEach((fieldText, customFormModelField) -> {
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