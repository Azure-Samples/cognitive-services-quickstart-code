using Microsoft.Azure.CognitiveServices.FormRecognizer;
using Microsoft.Azure.CognitiveServices.FormRecognizer.Models;

using System;
using System.IO;
using System.Threading.Tasks;

namespace FormRecognizerQuickStart
{
    class Program
    {
        // <snippet_variables>
        // // Add your Azure Form Recognizer subscription key and endpoint to your environment variables.
        private static string subscriptionKey = Environment.GetEnvironmentVariable("FORM_RECOGNIZER_SUBSCRIPTION_KEY");
        private static string formRecognizerEndpoint = Environment.GetEnvironmentVariable("FORM_RECOGNIZER__ENDPOINT");

        // SAS Url to Azure Blob Storage container; this used for training the custom model
        // For help using SAS see: 
        // https://docs.microsoft.com/en-us/azure/storage/common/storage-dotnet-shared-access-signature-part-1
        private const string trainingDataUrl = "<AzureBlobSaS>";

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
            IFormRecognizerClient formClient = new FormRecognizerClient(
                new ApiKeyServiceClientCredentials(subscriptionKey))
            {
                Endpoint = formRecognizerEndpoint
            };

            Console.WriteLine("Train Model with training data...");
            Guid modelId = await TrainModelAsync(formClient, trainingDataUrl);

            Console.WriteLine("Get list of extracted keys...");
            await GetListOfExtractedKeys(formClient, modelId);

            Console.WriteLine("Analyze PDF form...");
            await AnalyzePdfForm(formClient, modelId, pdfFormFile);

            Console.WriteLine("Analyze JPEG form...");
            await AnalyzeJpgForm(formClient, modelId, jpgFormFile);

            Console.WriteLine("Analyze PNG form...");
            await AnalyzePngForm(formClient, modelId, pngFormFile);

            Console.WriteLine("Get list of trained models ...");
            await GetListOfModels(formClient);

            Console.WriteLine("Delete Model...");
            await DeleteModel(formClient, modelId);
        }
        // </snippet_maintask>

        // <snippet_train>
        // Train model using training form data (pdf, jpg, png files)
        private static async Task<Guid> TrainModelAsync(
            IFormRecognizerClient formClient, string trainingDataUrl)
        {
            if (!Uri.IsWellFormedUriString(trainingDataUrl, UriKind.Absolute))
            {
                Console.WriteLine("\nInvalid trainingDataUrl:\n{0} \n", trainingDataUrl);
                return Guid.Empty;
            }

            try
            {
                TrainResult result = await formClient.TrainCustomModelAsync(new TrainRequest(trainingDataUrl));

                ModelResult model = await formClient.GetCustomModelAsync(result.ModelId);
                DisplayModelStatus(model);

                return result.ModelId;
            }
            catch (ErrorResponseException e)
            {
                Console.WriteLine("Train Model : " + e.Message);
                return Guid.Empty;
            }
        }
        // </snippet_train>

        // Get and display list of extracted keys for training data 
        // provided to train the model
        private static async Task GetListOfExtractedKeys(
            IFormRecognizerClient formClient, Guid modelId)
        {
            if (modelId == Guid.Empty)
            {
                Console.WriteLine("\nInvalid model Id.");
                return;
            }

            try
            {
                KeysResult kr = await formClient.GetExtractedKeysAsync(modelId);
                var clusters = kr.Clusters;
                foreach (var kvp in clusters)
                {
                    Console.WriteLine("  Cluster: " + kvp.Key + "");
                    foreach (var v in kvp.Value)
                    {
                        Console.WriteLine("\t" + v);
                    }
                }
            }
            catch (ErrorResponseException e)
            {
                Console.WriteLine("Get list of extracted keys : " + e.Message);
            }
        }

        // <snippet_analyzepdf>
        // Analyze PDF form data
        private static async Task AnalyzePdfForm(
            IFormRecognizerClient formClient, Guid modelId, string pdfFormFile)
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
                    AnalyzeResult result = await formClient.AnalyzeWithCustomModelAsync(modelId, stream, contentType: "application/pdf");

                    Console.WriteLine("\nExtracted data from:" + pdfFormFile);
                    DisplayAnalyzeResult(result);
                }
            }
            catch (ErrorResponseException e)
            {
                Console.WriteLine("Analyze PDF form : " + e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Analyze PDF form : " + ex.Message);
            }
        }
        // </snippet_analyzepdf>

        // Analyze JPEG form data
        private static async Task AnalyzeJpgForm(
            IFormRecognizerClient formClient, Guid modelId, string jpgFormFile)
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
                    AnalyzeResult result = await formClient.AnalyzeWithCustomModelAsync(modelId, stream, contentType: "image/jpeg");

                    Console.WriteLine("\nExtracted data from:" + jpgFormFile);
                    DisplayAnalyzeResult(result);
                }
            }
            catch (ErrorResponseException e)
            {
                Console.WriteLine("Analyze JPG form  : " + e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Analyze JPG form  : " + ex.Message);
            }
        }

        // Analyze PNG form data
        private static async Task AnalyzePngForm(
            IFormRecognizerClient formClient, Guid modelId, string pngFormFile)
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
                    AnalyzeResult result = await formClient.AnalyzeWithCustomModelAsync(modelId, stream, contentType: "image/png");

                    Console.WriteLine("\nExtracted data from:" + pngFormFile);
                    DisplayAnalyzeResult(result);
                }
            }
            catch (ErrorResponseException e)
            {
                Console.WriteLine("Analyze PNG form  " + e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Analyze PNG  form  : " + ex.Message);
            }
        }

        // <snippet_getmodellist>
        // Get and display list of trained the models
        private static async Task GetListOfModels(
            IFormRecognizerClient formClient)
        {
            try
            {
                ModelsResult models = await formClient.GetCustomModelsAsync();
                foreach (ModelResult m in models.ModelsProperty)
                {
                    Console.WriteLine(m.ModelId + " " + m.Status + " " + m.CreatedDateTime + " " + m.LastUpdatedDateTime);
                }
                Console.WriteLine();
            }
            catch (ErrorResponseException e)
            {
                Console.WriteLine("Get list of models : " + e.Message);
            }
        }
        // </snippet_getmodellist>

        // <snippet_deletemodel>
        // Delete a model
        private static async Task DeleteModel(
            IFormRecognizerClient formClient, Guid modelId)
        {
            try
            {
                Console.Write("Deleting model: {0}...", modelId.ToString());
                await formClient.DeleteCustomModelAsync(modelId);
                Console.WriteLine("done.\n");
            }
            catch (ErrorResponseException e)
            {
                Console.WriteLine("Delete model : " + e.Message);
            }
        }
        // </snippet_deletemodel>

        // <snippet_displayanalyze>
        // Display analyze status
        private static void DisplayAnalyzeResult(AnalyzeResult result)
        {
            foreach (var page in result.Pages)
            {
                Console.WriteLine("\tPage#: " + page.Number);
                Console.WriteLine("\tCluster Id: " + page.ClusterId);
                foreach (var kv in page.KeyValuePairs)
                {
                    if (kv.Key.Count > 0)
                        Console.Write(kv.Key[0].Text);

                    if (kv.Value.Count > 0)
                        Console.Write(" - " + kv.Value[0].Text);

                    Console.WriteLine();
                }
                Console.WriteLine();

                foreach (var t in page.Tables)
                {
                    Console.WriteLine("Table id: " + t.Id);
                    foreach (var c in t.Columns)
                    {
                        foreach (var h in c.Header)
                            Console.Write(h.Text + "\t");

                        foreach (var e in c.Entries)
                        {
                            foreach (var ee in e)
                                Console.Write(ee.Text + "\t");
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }
            }
        }
        // </snippet_displayanalyze>

        // <snippet_displaymodel>
        // Display model status
        private static void DisplayModelStatus(ModelResult model)
        {
            Console.WriteLine("\nModel :");
            Console.WriteLine("\tModel id: " + model.ModelId);
            Console.WriteLine("\tStatus:  " + model.Status);
            Console.WriteLine("\tCreated: " + model.CreatedDateTime);
            Console.WriteLine("\tUpdated: " + model.LastUpdatedDateTime);
        }
        // </snippet_displaymodel>

       
    }
}

