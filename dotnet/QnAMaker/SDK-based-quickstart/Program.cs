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
            var authoringKey = "REPLACE-WITH-YOUR-QNA-MAKER-KEY";
            var resourceName = "REPLACE-WITH-YOUR-RESOURCE-NAME";

            // <AuthorizationAuthoring>
            var client = new QnAMakerClient(new ApiKeyServiceClientCredentials(authoringKey)) { Endpoint = $"https://{resourceName}.cognitiveservices.azure.com" };
            // </AuthorizationAuthoring>

            var kbId = CreateSampleKb(client).Result;
            UpdateKB(client, kbId).Wait();
            PublishKb(client, kbId).Wait();
            DownloadKb(client, kbId).Wait();
            var primaryPredictionEndpointKey = GetPredictionEndpointKey(client).Result;

            // <AuthorizationPrediction>
            var runtimeClient = new QnAMakerRuntimeClient(new EndpointKeyServiceClientCredentials(primaryPredictionEndpointKey)) { RuntimeEndpoint = $"https://{resourceName}.azurewebsites.net" };
            // </AuthorizationPrediction>

            GenerateAnswer(runtimeClient, kbId).Wait();
            DeleteKB(client, kbId).Wait();
        }
        // </Main>

        // <GetPredictionEndpointKey>
        private static async Task<String> GetPredictionEndpointKey(IQnAMakerClient client)
        {
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
                FileName = "myFileName",
                FileUri = "https://mydomain/myfile.md"

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

        // <PublishKB>
        private static async Task PublishKb(IQnAMakerClient client, string kbId)
        {
            await client.Knowledgebase.PublishAsync(kbId);
        }
        // </PublishKB>

        // <DownloadKB>
        private static async Task DownloadKb(IQnAMakerClient client, string kbId)
        {
            var kbData = await client.Knowledgebase.DownloadAsync(kbId, EnvironmentType.Prod);
            Console.WriteLine("KB Downloaded. It has {0} QnAs.", kbData.QnaDocuments.Count);

            // Do something meaningful with data
        }
        // </DownloadKB>

        // <GenerateAnswer>
        private static async Task GenerateAnswer(IQnAMakerRuntimeClient runtimeClient, string kbId)
        {
            var response = await runtimeClient.Runtime.GenerateAnswerAsync(kbId, new QueryDTO { Question = "How do I manage my knowledgebase?" });
            Console.WriteLine("Endpoint Response: {0}.", response.Answers[0].Answer);

            // Do something meaningful with answer
        }
        // </GenerateAnswer>

        // <DeleteKB>
        private static async Task DeleteKB(IQnAMakerClient client, string kbId)
        {
            await client.Knowledgebase.DeleteAsync(kbId);
        }
        // </DeleteKB>

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
