/*
 * ==========================================
   Install QnA Maker package with command
 * ==========================================
 *
 * dotnet add package Microsoft.Azure.CognitiveServices.Knowledge.QnAMaker --version 3.0.0-preview.1
 *
 * ==========================================
   Tasks Included
 * ==========================================
 * Create a knowledgebase
 * Update a knowledgebase
 * Publish a knowledgebase, waiting for publishing to complete
 * Get Query runtime endpoint key
 * Download a knowledgebase
 * Get answer
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
            // <Resourcevariables>
            var authoringKey = "REPLACE-WITH-YOUR-QNA-MAKER-KEY";
            var resourceName = "REPLACE-WITH-YOUR-RESOURCE-NAME";

            var authoringURL = $"https://{resourceName}.cognitiveservices.azure.com";
            var queryingURL = $"https://{resourceName}.azurewebsites.net";
            // </Resourcevariables>

            // <TryManagedPreview>
            // To be set to 'true' to use QnAMakerV2 Public Preview resources 
            // Use the package Microsoft.Azure.CognitiveServices.Knowledge.QnAMaker version 3.0.0-preview.1
            var tryManagedPreview = false;
            // </TryManagedPreview>


            // <AuthorizationAuthor>
            var client = new QnAMakerClient(new ApiKeyServiceClientCredentials(authoringKey))
            { Endpoint = authoringURL };
            // </AuthorizationAuthor>

            var kbId = CreateSampleKb(client).Result;
            UpdateKB(client, kbId).Wait();
            PublishKb(client, kbId).Wait();
            DownloadKb(client, kbId).Wait();
            var primaryQueryEndpointKey = GetQueryEndpointKey(client).Result;

            // <AuthorizationQuery>
            if (tryManagedPreview)
            {
                GenerateAnswerPreview(client, kbId).Wait();
            }
            else
            {
                var runtimeClient = new QnAMakerRuntimeClient(new EndpointKeyServiceClientCredentials(primaryQueryEndpointKey))
                { RuntimeEndpoint = queryingURL };
                GenerateAnswer(runtimeClient, kbId).Wait();
            }
            // </AuthorizationQuery>

            DeleteKB(client, kbId).Wait();
        }
        // </Main>

        // <GetQueryEndpointKey>
        private static async Task<String> GetQueryEndpointKey(IQnAMakerClient client)
        {
            var endpointKeysObject = await client.EndpointKeys.GetKeysAsync();

            return endpointKeysObject.PrimaryEndpointKey;
        }
        // </GetQueryEndpointKey>

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
                FileName = "myfile.tsv",
                FileUri = "https://mydomain/myfile.tsv"

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
        private static async Task GenerateAnswer(QnAMakerRuntimeClient runtimeClient, string kbId)
        {
            var response = await runtimeClient.Runtime.GenerateAnswerAsync(kbId, new QueryDTO { Question = "How do I manage my knowledgebase?" });
            Console.WriteLine("Endpoint Response: {0}.", response.Answers[0].Answer);

            // Do something meaningful with answer
        }
        // </GenerateAnswer>

        // <GenerateAnswerManagedPreview>
        private static async Task GenerateAnswerPreview(IQnAMakerClient client, string kbId)
        { 
            var response = await client.Knowledgebase.GenerateAnswerAsync(kbId, 
            new QueryDTO { 
                Question = "Deleted accidentally. how to recover? ",

                // This can be used to query for short answers 
                // -- Available with QnA Maker Managed Public Preview resources
                AnswerSpanRequest = new QueryDTOAnswerSpanRequest
                {
                    Enable = true,
                    ScoreThreshold = 0.0,
                    TopAnswersWithSpan = 1
                },
                IsTest = true,
                Top = 3,
                
                // By providing context of previous user question and qna id, 
                // get contextually relevant answer for current question

                //Context = new QueryDTOContext
                //{
                //   PreviousQnaId = 2,
                //   PreviousUserQuery = "where is my qnamaker resource?"
                //},
                
                StrictFilters = new List<MetadataDTO>(),

                // RankerType = "QuestionOnly",

                ScoreThreshold = 10,
                UserId = "SDKUser",
                
            });

            Console.WriteLine("Endpoint Response -- Answer: {0}.", response.Answers[0].Answer);            
            Console.WriteLine("Endpoint Response -- Short Answer: {0}.", response.Answers[0]?.AnswerSpan?.Text);

            // Do something meaningful with answer
        }
        // </GenerateAnswerManagedPreview>

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
