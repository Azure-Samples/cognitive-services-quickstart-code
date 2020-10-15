// <Dependencies>
// NOTE Install the Microsoft.Azure.CognitiveServices.Knowledge.QnAMaker NuGet package.
using Microsoft.Azure.CognitiveServices.Knowledge.QnAMaker;
using Microsoft.Azure.CognitiveServices.Knowledge.QnAMaker.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
// </Dependencies>

namespace ConsoleApp1
{
    class Program
    {
        // <Main>
        static void Main(string[] args)
        {
            // <AuthoringAuthorization>
            string subscriptionKey = Environment.GetEnvironmentVariable("QNA_MAKER_SUBSCRIPTION_KEY");
            string endpoint = Environment.GetEnvironmentVariable("QNA_MAKER_ENDPOINT");

            // set tryPreview to 'true' for QnAMakerV2 resources
            bool tryPreview = false;  

            var authoringClient = new QnAMakerClient(new ApiKeyServiceClientCredentials(subscriptionKey), tryPreview)
                { Endpoint = endpoint };
            // </AuthoringAuthorization>

            // <RuntimeAuthorization>
            string runtimeEndpoint = Environment.GetEnvironmentVariable("QNA_MAKER_RUNTIME_ENDPOINT");
            if (tryPreview)
            {
                runtimeEndpoint = endpoint;
            }
            
            string endpointKey = GetQueryEndpointKey(authoringClient).Result;
            QnAMakerRuntimeClient runtimeClient;
            if (tryPreview)
            {
                runtimeClient = new QnAMakerRuntimeClient(new ApiKeyServiceClientCredentials(subscriptionKey), tryPreview)
                { RuntimeEndpoint = runtimeEndpoint };
            }
            else
            {
                runtimeClient = new QnAMakerRuntimeClient(new EndpointKeyServiceClientCredentials(endpointKey))
                { RuntimeEndpoint = runtimeEndpoint };
            }
            // </RuntimeAuthorization>

            Console.WriteLine("Creating KB...");
            var kbId = CreateSampleKb(authoringClient).Result;
            Console.WriteLine("Created KB with ID " + kbId + ".");
            Console.WriteLine("Downloading KB...");
            DownloadKb(authoringClient, kbId).Wait();
            Console.WriteLine("Updating KB...");
            UpdateKB(authoringClient, kbId).Wait();
            Console.WriteLine("Publishing KB...");
            PublishKb(authoringClient, kbId).Wait();
            Console.WriteLine("Querying KB...");
            GenerateAnswer(runtimeClient, kbId).Wait();
            Console.WriteLine("Deleting KB...");
            DeleteKB(authoringClient, kbId).Wait();
        }
        // </Main>

        // <CreateKBMethod>
        private static async Task<string> CreateSampleKb(IQnAMakerClient client)
        {
            var qna1 = new QnADTO
            {
                Answer = "Yes, You can use our [REST APIs](https://docs.microsoft.com/rest/api/cognitiveservices/qnamaker/knowledgebase) to manage your knowledge base.",
                Questions = new List<string> { "How do I manage my knowledgebase?" },
                Metadata = new List<MetadataDTO> {
                    new MetadataDTO { Name = "Category", Value = "api" },
                    new MetadataDTO { Name = "Language", Value = "REST" }
                },

            };

            var qna2 = new QnADTO
            {
                Answer = "Yes, You can use our [.NET SDK](https://www.nuget.org/packages/Microsoft.Azure.CognitiveServices.Knowledge.QnAMaker) with the [.NET Reference Docs](https://docs.microsoft.com/dotnet/api/microsoft.azure.cognitiveservices.knowledge.qnamaker?view=azure-dotnet) to manage your knowledge base.",
                Questions = new List<string> { "Can I program with C#?" },
                Metadata = new List<MetadataDTO> {
                    new MetadataDTO { Name = "Category", Value = "api" },
                    new MetadataDTO { Name = "Language", Value = ".NET" }
                }
            };

            var file1 = new FileDTO
            {
                FileName = "myfile.md",
                FileUri = "https://mydomain/myfile.md"

            };

            var createKbDto = new CreateKbDTO
            {
                Name = "QnA Maker .NET SDK Quickstart",
                QnaList = new List<QnADTO> { qna1, qna2 },
                //Files = new List<FileDTO> { file1 }

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

        // <DownloadKB>
        private static async Task DownloadKb(IQnAMakerClient client, string kbId)
        {
            // Note: If you have already published the KB, pass in EnvironmentType.Prod.
            var kbData = await client.Knowledgebase.DownloadAsync(kbId, EnvironmentType.Test);
            Console.WriteLine("KB Downloaded. It has {0} QnAs.", kbData.QnaDocuments.Count);

            // Do something meaningful with data
        }
        // </DownloadKB>

        // <UpdateKBMethod>
        private static async Task UpdateKB(IQnAMakerClient client, string kbId)
        {

            var urls = new List<string> {
                "https://docs.microsoft.com/azure/cognitive-services/QnAMaker/troubleshooting"
            };

            var updateOp = await client.Knowledgebase.UpdateAsync(kbId, new UpdateKbOperationDTO
            {
                // Create JSON of changes
                Add = new UpdateKbOperationDTOAdd
                {
                    QnaList = new List<QnADTO> {
                        new QnADTO {
                            Questions = new List<string> {
                                "bye",
                                "end",
                                "stop",
                                "quit",
                                "done"
                            },
                            Answer = "goodbye",
                            Metadata = new List<MetadataDTO> {
                                new MetadataDTO { Name = "Category", Value="Chitchat" },
                                new MetadataDTO { Name = "Chitchat", Value = "end" },
                            }
                        },
                        new QnADTO {
                            Questions = new List<string> {
                                "hello",
                                "hi",
                                "start"
                            },
                            Answer = "Hello, please select from the list of questions or enter a new question to continue.",
                            Metadata = new List<MetadataDTO> {
                                new MetadataDTO { Name = "Category", Value="Chitchat" },
                                new MetadataDTO { Name = "Chitchat", Value = "begin" }
                            },
                            Context = new QnADTOContext
                            {
                                IsContextOnly = false,
                                Prompts = new List<PromptDTO>
                                {
                                    new PromptDTO
                                    {
                                        DisplayOrder =1,
                                        DisplayText= "Use REST",
                                        QnaId=1

                                    },
                                    new PromptDTO
                                    {
                                        DisplayOrder =2,
                                        DisplayText= "Use .NET NuGet package",
                                        QnaId=2

                                    },
                                }
                            }
                        },
                    },
                    Urls = urls
                },
                Update = null,
                Delete = null
            }); ;

            // Loop while operation is success
            updateOp = await MonitorOperation(client, updateOp);
        }
        // </UpdateKBMethod>

        // <PublishKB>
        private static async Task PublishKb(IQnAMakerClient client, string kbId)
        {
            await client.Knowledgebase.PublishAsync(kbId);
        }
        // </PublishKB>

        // <GetQueryEndpointKey>
        private static async Task<String> GetQueryEndpointKey(IQnAMakerClient client)
        {
            var endpointKeysObject = await client.EndpointKeys.GetKeysAsync();

            return endpointKeysObject.PrimaryEndpointKey;
        }
        // </GetQueryEndpointKey>

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
    }
}
