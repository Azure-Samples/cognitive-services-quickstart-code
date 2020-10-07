// <snippet_using>
using Azure;
using Azure.AI.FormRecognizer;
using Azure.AI.FormRecognizer.Models;
using Azure.AI.FormRecognizer.Training;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
// </snippet_using>

class Program
{

    // <snippet_creds>
    private static readonly string endpoint = "<your api endpoint>";
    private static readonly string apiKey = "<your Form Recognizer key>";
    private static readonly AzureKeyCredential credential = new AzureKeyCredential(apiKey);
    // </snippet_creds>

    // <snippet_urls>
    string trainingDataUrl = "<SAS-URL-of-your-form-folder-in-blob-storage>";
    string formUrl = "<SAS-URL-of-a-form-in-blob-storage>";
    string receiptUrl = "https://docs.microsoft.com/azure/cognitive-services/form-recognizer/media"
    + "/contoso-allinone.jpg";
    // </snippet_urls>

    // <snippet_main>
    static void Main(string[] args)
    {
        var t1 = RunFormRecognizerClient();
        Task.WaitAll(t1);

        // new code:
        var analyzeForm = RecognizeContent();
        Task.WaitAll(analyzeForm);

    }
    // </snippet_main>

    // <snippet_auth>
    static private FormRecognizerClient AuthenticateClient()
    {
        string endpoint = "<replace-with-your-form-recognizer-endpoint-here>";
        string apiKey = "<replace-with-your-form-recognizer-key-here>";
        var credential = new AzureKeyCredential(apiKey);
        var client = new FormRecognizerClient(new Uri(endpoint), credential);
        return client;
    }
    // </snippet_auth>
    static async Task RunFormRecognizerClient()
    {
        var trainingClient = new FormTrainingClient(new Uri(endpoint), credential);
        var recognizerClient = new FormRecognizerClient(new Uri(endpoint), credential);

        // <snippet_calls>
        // Call Form Recognizer scenarios:
        Console.WriteLine("Get form content...");
        await GetContent(recognizerClient, formUrl);

        Console.WriteLine("Analyze receipt...");
        await AnalyzeReceipt(recognizerClient, receiptUrl);

        Console.WriteLine("Train Model with training data...");
        Guid modelId = await TrainModel(trainingClient, trainingDataUrl);

        Console.WriteLine("Analyze PDF form...");
        await AnalyzePdfForm(recognizerClient, modelId, formUrl);

        Console.WriteLine("Manage models...");
        await ManageModels(trainingClient, trainingDataUrl);
    }
    // </snippet_calls>

    // <snippet_getcontent_call>
    private static async Task RecognizeContent(FormRecognizerClient recognizerClient)
    {
        var invoiceUri = "https://raw.githubusercontent.com/Azure/azure-sdk-for-python/master/sdk/formrecognizer/azure-ai-formrecognizer/tests/sample_forms/forms/Invoice_1.pdf";
        FormPageCollection formPages = await recognizeClient
            .StartRecognizeContentFromUri(new Uri(invoiceUri))
            .WaitForCompletionAsync();
        // </snippet_getcontent_call>

        // <snippet_getcontent_print>
        foreach (FormPage page in formPages)
        {
            Console.WriteLine($"Form Page {page.PageNumber} has {page.Lines.Count} lines.");

            for (int i = 0; i < page.Lines.Count; i++)
            {
                FormLine line = page.Lines[i];
                Console.WriteLine($"    Line {i} has {line.Words.Count} word{(line.Words.Count > 1 ? "s" : "")}, and text: '{line.Text}'.");
            }

            for (int i = 0; i < page.Tables.Count; i++)
            {
                FormTable table = page.Tables[i];
                Console.WriteLine($"Table {i} has {table.RowCount} rows and {table.ColumnCount} columns.");
                foreach (FormTableCell cell in table.Cells)
                {
                    Console.WriteLine($"    Cell ({cell.RowIndex}, {cell.ColumnIndex}) contains text: '{cell.Text}'.");
                }
            }
        }
    }
    // </snippet_getcontent_print>

    // <snippet_receipt_call>
    private static async Task AnalyzeReceipt(
        FormRecognizerClient recognizerClient, string receiptUri)
    {
        RecognizedFormCollection receipts = await recognizeClient.StartRecognizeReceiptsFromUri(new Uri(receiptUrl)).WaitForCompletionAsync();
        // </snippet_receipt_call>

        // <snippet_receipt_print>
        foreach (RecognizedForm receipt in receipts)
        {
            FormField merchantNameField;
            if (receipt.Fields.TryGetValue("MerchantName", out merchantNameField))
            {
                if (merchantNameField.Value.ValueType == FieldValueType.String)
                {
                    string merchantName = merchantNameField.Value.AsString();

                    Console.WriteLine($"Merchant Name: '{merchantName}', with confidence {merchantNameField.Confidence}");
                }
            }

            FormField transactionDateField;
            if (receipt.Fields.TryGetValue("TransactionDate", out transactionDateField))
            {
                if (transactionDateField.Value.ValueType == FieldValueType.Date)
                {
                    DateTime transactionDate = transactionDateField.Value.AsDate();

                    Console.WriteLine($"Transaction Date: '{transactionDate}', with confidence {transactionDateField.Confidence}");
                }
            }

            FormField itemsField;
            if (receipt.Fields.TryGetValue("Items", out itemsField))
            {
                if (itemsField.Value.ValueType == FieldValueType.List)
                {
                    foreach (FormField itemField in itemsField.Value.AsList())
                    {
                        Console.WriteLine("Item:");

                        if (itemField.Value.ValueType == FieldValueType.Dictionary)
                        {
                            IReadOnlyDictionary<string, FormField> itemFields = itemField.Value.AsDictionary();

                            FormField itemNameField;
                            if (itemFields.TryGetValue("Name", out itemNameField))
                            {
                                if (itemNameField.Value.ValueType == FieldValueType.String)
                                {
                                    string itemName = itemNameField.Value.AsString();

                                    Console.WriteLine($"    Name: '{itemName}', with confidence {itemNameField.Confidence}");
                                }
                            }

                            FormField itemTotalPriceField;
                            if (itemFields.TryGetValue("TotalPrice", out itemTotalPriceField))
                            {
                                if (itemTotalPriceField.Value.ValueType == FieldValueType.Float)
                                {
                                    float itemTotalPrice = itemTotalPriceField.Value.AsFloat();

                                    Console.WriteLine($"    Total Price: '{itemTotalPrice}', with confidence {itemTotalPriceField.Confidence}");
                                }
                            }
                        }
                    }
                }
            }
            FormField totalField;
            if (receipt.Fields.TryGetValue("Total", out totalField))
            {
                if (totalField.Value.ValueType == FieldValueType.Float)
                {
                    float total = totalField.Value.AsFloat();

                    Console.WriteLine($"Total: '{total}', with confidence '{totalField.Confidence}'");
                }
            }
        }
    }
    // </snippet_receipt_print>

    // <snippet_train>
    private static async Task<Guid> TrainModel(
        FormRecognizerClient trainingClient, string trainingDataUrl)
    {
        CustomFormModel model = await trainingClient
        .StartTrainingAsync(new Uri(trainingDataUrl), useTrainingLabels: false)
        .WaitForCompletionAsync();

        Console.WriteLine($"Custom Model Info:");
        Console.WriteLine($"    Model Id: {model.ModelId}");
        Console.WriteLine($"    Model Status: {model.Status}");
        Console.WriteLine($"    Training model started on: {model.TrainingStartedOn}");
        Console.WriteLine($"    Training model completed on: {model.TrainingCompletedOn}");
        // </snippet_train>
        // <snippet_train_response>
        foreach (CustomFormSubmodel submodel in model.Submodels)
        {
            Console.WriteLine($"Submodel Form Type: {submodel.FormType}");
            foreach (CustomFormModelField field in submodel.Fields.Values)
            {
                Console.Write($"    FieldName: {field.Name}");
                if (field.Label != null)
                {
                    Console.Write($", FieldLabel: {field.Label}");
                }
                Console.WriteLine("");
            }
        }
        // </snippet_train_response>
        // <snippet_train_return>
        return model.ModelId;
    }
    // </snippet_train_return>

    // <snippet_trainlabels>
    private static async Task<Guid> TrainModelWithLabelsAsync(
        FormRecognizerClient trainingClient, string trainingDataUrl)
    {
        CustomFormModel model = await trainingClient
        .StartTrainingAsync(new Uri(trainingDataUrl), useTrainingLabels: true)
        .WaitForCompletionAsync();
        Console.WriteLine($"Custom Model Info:");
        Console.WriteLine($"    Model Id: {model.ModelId}");
        Console.WriteLine($"    Model Status: {model.Status}");
        Console.WriteLine($"    Training model started on: {model.TrainingStartedOn}");
        Console.WriteLine($"    Training model completed on: {model.TrainingCompletedOn}");
        // </snippet_trainlabels>
        // <snippet_trainlabels_response>
        foreach (CustomFormSubmodel submodel in model.Submodels)
        {
            Console.WriteLine($"Submodel Form Type: {submodel.FormType}");
            foreach (CustomFormModelField field in submodel.Fields.Values)
            {
                Console.Write($"    FieldName: {field.Name}");
                if (field.Label != null)
                {
                    Console.Write($", FieldLabel: {field.Label}");
                }
                Console.WriteLine("");
            }
        }
        return model.ModelId;
    }
    // </snippet_trainlabels_response>

    // <snippet_analyze>
    // Analyze PDF form data
    private static async Task AnalyzePdfForm(
        FormRecognizerClient recognizerClient, Guid modelId, string formUrl)
    {
        RecognizedFormCollection forms = await recognizeClient
        .StartRecognizeCustomFormsFromUri(modelId, new Uri(invoiceUri))
        .WaitForCompletionAsync();
        // </snippet_analyze>
        // <snippet_analyze_response>
        foreach (RecognizedForm form in forms)
        {
            Console.WriteLine($"Form of type: {form.FormType}");
            foreach (FormField field in form.Fields.Values)
            {
                Console.WriteLine($"Field '{field.Name}: ");

                if (field.LabelData != null)
                {
                    Console.WriteLine($"    Label: '{field.LabelData.Text}");
                }

                Console.WriteLine($"    Value: '{field.ValueData.Text}");
                Console.WriteLine($"    Confidence: '{field.Confidence}");
            }
            Console.WriteLine("Table data:");
            foreach (FormPage page in form.Pages.Values)
            {
                for (int i = 0; i < page.Tables.Count; i++)
                {
                    FormTable table = page.Tables[i];
                    Console.WriteLine($"Table {i} has {table.RowCount} rows and {table.ColumnCount} columns.");
                    foreach (FormTableCell cell in table.Cells)
                    {
                        Console.WriteLine($"    Cell ({cell.RowIndex}, {cell.ColumnIndex}) contains {(cell.IsHeader ? "header" : "text")}: '{cell.Text}'");
                    }
                }
            }
        }
    }
    // </snippet_analyze_response>

    // <snippet_manage>
    private static async Task ManageModels(
        FormRecognizerClient trainingClient, string trainingFileUrl)
    {
        // </snippet_manage>
        // <snippet_manage_model_count>
        // Check number of models in the FormRecognizer account, 
        // and the maximum number of models that can be stored.
        AccountProperties accountProperties = trainingClient.GetAccountProperties();
        Console.WriteLine($"Account has {accountProperties.CustomModelCount} models.");
        Console.WriteLine($"It can have at most {accountProperties.CustomModelLimit} models.");
        // </snippet_manage_model_count>

        // <snippet_manage_model_list>
        Pageable<CustomFormModelInfo> models = trainingClient.GetCustomModels();

        foreach (CustomFormModelInfo modelInfo in models)
        {
            Console.WriteLine($"Custom Model Info:");
            Console.WriteLine($"    Model Id: {modelInfo.ModelId}");
            Console.WriteLine($"    Model Status: {modelInfo.Status}");
            Console.WriteLine($"    Training model started on: {modelInfo.TrainingStartedOn}");
            Console.WriteLine($"    Training model completed on: {modelInfo.TrainingCompletedOn}");
        }
        // </snippet_manage_model_list>

        // <snippet_manage_model_get>
        // Create a new model to store in the account
        CustomFormModel model = await trainingClient.StartTrainingAsync(
            new Uri(trainingFileUrl)).WaitForCompletionAsync();

        // Get the model that was just created
        CustomFormModel modelCopy = trainingClient.GetCustomModel(modelId);

        Console.WriteLine($"Custom Model {modelCopy.ModelId} recognizes the following form types:");

        foreach (CustomFormSubmodel submodel in modelCopy.Submodels)
        {
            Console.WriteLine($"Submodel Form Type: {submodel.FormType}");
            foreach (CustomFormModelField field in submodel.Fields.Values)
            {
                Console.Write($"    FieldName: {field.Name}");
                if (field.Label != null)
                {
                    Console.Write($", FieldLabel: {field.Label}");
                }
                Console.WriteLine("");
            }
        }
        // </snippet_manage_model_get>

        // <snippet_manage_model_delete>
        // Delete the model from the account.
        trainingClient.DeleteModel(model.ModelId);
    }
    // </snippet_manage_model_delete>
}

