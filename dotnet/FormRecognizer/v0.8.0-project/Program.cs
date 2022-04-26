// This code sample uses an earlier version of the Form Recognizer SDK.

// <snippet_using>
using Azure;
using Azure.AI.FormRecognizer;
using Azure.AI.FormRecognizer.Models;
using Azure.AI.FormRecognizer.Training;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
// </snippet_using>

namespace FormRecognizerQuickStart
{
    class Program
    {
        // <snippet_variables>
        // // Add your Azure Form Recognizer subscription key and endpoint to your environment variables.
        private static string subscriptionKey = "PASTE_YOUR_FORM_RECOGNIZER_SUBSCRIPTION_KEY_HERE";
        private static string formRecognizerEndpoint = "PASTE_YOUR_FORM_RECOGNIZER_ENDPOINT_HERE";

        // SAS Url to Azure Blob Storage container; this used for training the custom model
        // For help using SAS see: 
        // https://docs.microsoft.com/en-us/azure/storage/common/storage-dotnet-shared-access-signature-part-1
        private const string trainingDataUrl = "PASTE_YOUR_SAS_URL_OF_YOUR_FORM_FOLDER_IN_BLOB_STORAGE_HERE";

        // Local path to a form to be analyzed
        // Any one or all of file formats (pdf,jpg or png)can be used with a trained model. 
        // For example,  
        //  pdf file  : "c:\documents\invoice.pdf" 
        //  jpeg file : "c:\documents\invoice.jpg"
        //  png file  : "c:\documents\invoice.png"
        private const string pdfFormFile = @"<pdfFormFileLocalPath>";
        private const string jpgFormFile = @"<jpgFormFileLocalPath>";
        private const string pngFormFile = @"<pngFormFileLocalPath>";
        // </snippet_variables>

        // <snippet_main>
        static void Main(string[] args)
        {
            var t1 = RunFormRecognizerClient();

            Task.WaitAll(t1);
        }
        // </snippet_main>

        // <snippet_maintask>
        static async Task RunFormRecognizerClient()
        {
            // Create form client object with Form Recognizer subscription key
            var formTrainingClient = new FormTrainingClient(new Uri(formRecognizerEndpoint), new AzureKeyCredential(subscriptionKey));

            Console.WriteLine("Train Model with training data...");
            string modelId = await TrainModelAsync(formTrainingClient, trainingDataUrl);

            // Choose any of the following three Analyze tasks:

            Console.WriteLine("Analyze PDF form...");
            await AnalyzePdfForm(formTrainingClient, modelId, pdfFormFile);

            Console.WriteLine("Analyze JPEG form...");
            await AnalyzeJpgForm(formTrainingClient, modelId, jpgFormFile);

            Console.WriteLine("Analyze PNG form...");
            await AnalyzePngForm(formTrainingClient, modelId, pngFormFile);
            
            Console.WriteLine("Get list of trained models ...");
            GetListOfModels(formTrainingClient);

            Console.WriteLine("Delete Model...");
            await DeleteModel(formTrainingClient, modelId);
        }
        // </snippet_maintask>

        // <snippet_train>
        // Train model using training form data (pdf, jpg, png files)
        private static async Task<string> TrainModelAsync(
            FormTrainingClient formTrainingClient, string trainingDataUrl)
        {
            if (!Uri.IsWellFormedUriString(trainingDataUrl, UriKind.Absolute))
            {
                Console.WriteLine("\nInvalid trainingDataUrl:\n{0} \n", trainingDataUrl);
                return null;
            }

            try
            {
                CustomFormModel result = await formTrainingClient.StartTrainingAsync(new Uri(trainingDataUrl), useTrainingLabels: false).WaitForCompletionAsync();
                
                DisplayModelStatus(result);

                return result.ModelId;
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine("Train Model : " + e.Message);
                return null;
            }
        }
        
        // <snippet_analyzepdf>
        // Analyze PDF form data
        private static async Task AnalyzePdfForm(
            FormTrainingClient formTrainingClient, string modelId, string pdfFormFile)
        {
            if (string.IsNullOrEmpty(pdfFormFile))
            {
                Console.WriteLine("\nInvalid pdfFormFile.");
                return;
            }

            try
            {
                using (FileStream stream = new FileStream(pdfFormFile, FileMode.Open))
                {
                    RecognizedFormCollection result = await formTrainingClient.GetFormRecognizerClient().StartRecognizeCustomFormsAsync(modelId, stream).WaitForCompletionAsync();

                    Console.WriteLine("\nExtracted data from:" + pdfFormFile);
                    DisplayAnalyzeResult(result);
                }
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine("Analyze PDF form : " + ex.Message);
            }
        }
        // </snippet_analyzepdf>

        // Analyze JPEG form data
        private static async Task AnalyzeJpgForm(
            FormTrainingClient formTrainingClient, string modelId, string jpgFormFile)
        {
            if (string.IsNullOrEmpty(jpgFormFile))
            {
                Console.WriteLine("\nInvalid jpgFormFile.");
                return;
            }

            try
            {
                using (FileStream stream = new FileStream(jpgFormFile, FileMode.Open))
                {
                    RecognizedFormCollection result = await formTrainingClient.GetFormRecognizerClient().StartRecognizeCustomFormsAsync(modelId, stream).WaitForCompletionAsync();

                    Console.WriteLine("\nExtracted data from:" + jpgFormFile);
                    DisplayAnalyzeResult(result);
                }
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine("Analyze JPG form  : " + ex.Message);
            }
        }

        // Analyze PNG form data
        private static async Task AnalyzePngForm(
            FormTrainingClient formTrainingClient, string modelId, string pngFormFile)
        {
            if (string.IsNullOrEmpty(pngFormFile))
            {
                Console.WriteLine("\nInvalid pngFormFile.");
                return;
            }

            try
            {
                using (FileStream stream = new FileStream(pngFormFile, FileMode.Open))
                {
                    RecognizedFormCollection result = await formTrainingClient.GetFormRecognizerClient().StartRecognizeCustomFormsAsync(modelId, stream).WaitForCompletionAsync();

                    Console.WriteLine("\nExtracted data from:" + pngFormFile);
                    DisplayAnalyzeResult(result);
                }
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine("Analyze PNG  form  : " + ex.Message);
            }
        }

        // <snippet_getmodellist>
        // Get and display list of trained the models
        private static void GetListOfModels(
            FormTrainingClient formTrainingClient)
        {
            try
            {
                Pageable<CustomFormModelInfo> models = formTrainingClient.GetCustomModels();
                foreach (CustomFormModelInfo m in models)
                {
                    Console.WriteLine(m.ModelId + " " + m.Status + " " + m.TrainingStartedOn + " " + m.TrainingCompletedOn);
                }
                Console.WriteLine();
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine("Get list of models : " + e.Message);
            }
        }
        // </snippet_getmodellist>

        // <snippet_deletemodel>
        // Delete a model
        private static async Task DeleteModel(
            FormTrainingClient formTrainingClient, string modelId)
        {
            try
            {
                Console.Write("Deleting model: {0}...", modelId.ToString());
                await formTrainingClient.DeleteModelAsync(modelId);
                Console.WriteLine("done.\n");
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine("Delete model : " + e.Message);
            }
            Console.ReadLine();
        }
        // </snippet_deletemodel>

        // <snippet_displayanalyze>
        // Display analyze status
        private static void DisplayAnalyzeResult(RecognizedFormCollection result)
        {
            foreach (var page in result)
            {
                Console.WriteLine("\tPage#: " + page.Pages.Count);
                Console.WriteLine("\tModelId Id: " + page.ModelId);
                foreach (var kv in page.Fields)
                {
                    if (kv.Key != null)
                        Console.Write(kv.Value.LabelData.Text);

                    if (kv.Value != null)
                        Console.Write(" - " + kv.Value.ValueData.Text);

                    Console.WriteLine();
                }
                Console.WriteLine();

                foreach (var t in page.Pages.First().Tables)
                {
                    Console.WriteLine("Page Number: " + t.PageNumber);
                    foreach (var c in t.Cells)
                    {
                        Console.Write(c.Text + "\t");
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }
            }
        }
        // </snippet_displayanalyze>

        // <snippet_displaymodel>
        // Display model status
        private static void DisplayModelStatus(CustomFormModel model)
        {
            Console.WriteLine("\nModel :");
            Console.WriteLine("\tModel id: " + model.ModelId);
            Console.WriteLine("\tStatus:  " + model.Status);
            Console.WriteLine("\tTraining model started on: " + model.TrainingStartedOn);
            Console.WriteLine("\tTraining model completed on: " + model.TrainingCompletedOn);
        }
        // </snippet_displaymodel>
    }
}

