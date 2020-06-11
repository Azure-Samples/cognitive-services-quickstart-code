/*
 * ==========================================
   Tasks Included
 * ==========================================
 * Create a knowledgebase
 * Update a knowledgebase
 * Publish a knowledgebase, waiting for publishing to complete
 * Get prediction runtime endpoint key
 * Download a knowledgebase
 * Delete a knowledgebase

 * ==========================================
   Further reading
 * General documentation: https://docs.microsoft.com/azure/cognitive-services/QnAMaker
 * Reference documentation: https://docs.microsoft.com/en-in/dotnet/api/microsoft.azure.cognitiveservices.knowledge.qnamaker?view=azure-dotnet
 * ==========================================

 */
namespace Knowledgebase_Quickstart
{
    // <Dependencies>
    using Microsoft.Azure.CognitiveServices.Knowledge.QnAMaker;
    using Microsoft.Azure.CognitiveServices.Knowledge.QnAMaker.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    // </Dependencies>

    class Program
    {

        // <Main>
        static void Main(string[] args)
        {
            // <Resourcevalues>
            String key = "REPLACE-WITH-QNA-MAKER-KEY";
            String resource_name = "REPLACE-WITH-RESOURCE-NAME";
            String endpoint_url = $"https://{resource_name}.cognitiveservices.azure.com";
            // </Resourcevalues>

            // <Authorization>
            var client = new QnAMakerClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint_url };
            // </Authorization>


            // Create a KB
            Console.WriteLine("Creating KB...");
            var kbId = CreateSampleKb(client).Result;
            Console.WriteLine("Created KB with ID : {0}", kbId);

            // Update the KB
            Console.WriteLine("Updating KB...");
            UpdateKB(client, kbId).Wait();
            Console.WriteLine("KB Updated.");

            // <PublishKB>
            Console.Write("Publishing KB...");
            client.Knowledgebase.PublishAsync(kbId).Wait();
            Console.WriteLine("KB Published.");
            // </PublishKB>

            // <EndpointKey>
            var primaryPredictionEndpointKey = GetPredictionEndpointKey(client).Result;
            var runtimeClient = new QnAMakerRuntimeClient(new EndpointKeyServiceClientCredentials(primaryPredictionEndpointKey)) { RuntimeEndpoint = $"https://{resource_name}.azurewebsites.net" };
            // </EndpointKey>

            // <DownloadKB>
            Console.Write("Downloading KB...");
            var kbData = client.Knowledgebase.DownloadAsync(kbId, EnvironmentType.Prod).Result;
            Console.WriteLine("KB Downloaded. It has {0} QnAs.", kbData.QnaDocuments.Count);
            // </DownloadKB>


            // <GenerateAnswer>
            Console.Write("Querying Endpoint...");
            var response = runtimeClient.Runtime.GenerateAnswerAsync(kbId, new QueryDTO { Question = "How do I manage my knowledgebase?" }).Result;
            Console.WriteLine("Endpoint Response: {0}.", response.Answers[0].Answer);
            // </GenerateAnswer>


            // <DeleteKB>
            Console.Write("Deleting KB...");
            client.Knowledgebase.DeleteAsync(kbId).Wait();
            Console.WriteLine("KB Deleted.");
            // </DeleteKB>
        }
        // </Main>

        // <GetPredictionEndpointKey>
        private static async Task<String> GetPredictionEndpointKey(IQnAMakerClient client)
        {
            // Get prediction runtime key
            var endpointKeysObject = await client.EndpointKeys.GetKeysAsync();

            return endpointKeysObject.PrimaryEndpointKey;

        }
        // </GetPredictionEndpointKey>


        // <UpdateKBMethod>
        private static async Task UpdateKB(IQnAMakerClient client, string kbId)
        {
            // Update kb
            var updateOp = await client.Knowledgebase.UpdateAsync(kbId, new UpdateKbOperationDTO
            {
                // Create JSON of changes
                Add = new UpdateKbOperationDTOAdd { QnaList = new List<QnADTO> { new QnADTO { Questions = new List<string> { "bye" }, Answer = "goodbye" } } },
                Update = null,
                Delete = null
            });

            // Loop while operation is success
            updateOp = await MonitorOperation(client, updateOp);
        }
        // </UpdateKBMethod>

        // <CreateKBMethod>
        private static async Task<string> CreateSampleKb(IQnAMakerClient client)
        {
            var qna1 = new QnADTO
            {
                Answer = "You can use our REST APIs to manage your knowledge base.",
                Questions = new List<string> { "How do I manage my knowledgebase?" },
                Metadata = new List<MetadataDTO> { new MetadataDTO { Name = "Category", Value = "api" } }
            };

            var file1 = new FileDTO
            {
                FileName="myFileName",
                FileUri="https://mydomain/myfile.md"

            };


            var urls = new List<string> {
                "https://docs.microsoft.com/en-in/azure/cognitive-services/QnAMaker/troubleshooting"
            };

            var createKbDto = new CreateKbDTO
            {
                Name = "QnA Maker .NET SDK Quickstart",
                QnaList = new List<QnADTO> { qna1 },
                //Files = new List<FileDTO> { file1 },
                Urls = urls

            };

            var createOp = await client.Knowledgebase.CreateAsync(createKbDto);
            createOp = await MonitorOperation(client, createOp);

            return createOp.ResourceLocation.Replace("/knowledgebases/", string.Empty);
        }
        // </CreateKBMethod>

        // <MonitorOperation>
        private static async Task<Operation> MonitorOperation(IQnAMakerClient client, Operation operation)
        {
            // Loop while operation is success
            for (int i = 0;
                i < 20 && (operation.OperationState == OperationStateType.NotStarted || operation.OperationState == OperationStateType.Running);
                i++)
            {
                Console.WriteLine("Waiting for operation: {0} to complete.", operation.OperationId);
                await Task.Delay(5000);
                operation = await client.Operations.GetDetailsAsync(operation.OperationId);
            }

            if (operation.OperationState != OperationStateType.Succeeded)
            {
                throw new Exception($"Operation {operation.OperationId} failed to completed.");
            }
            return operation;
        }
        // </MonitorOperation>
    }
}
