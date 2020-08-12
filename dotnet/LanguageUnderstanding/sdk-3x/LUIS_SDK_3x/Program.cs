/* To run this sample, install the following modules.
 * dotnet add package Microsoft.Azure.CognitiveServices.Language.LUIS.Authoring --version 3.0.0
 * dotnet add package Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime --version 3.0.0
 */

// <Dependencies>
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Authoring;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Authoring.Models;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;
using Newtonsoft.Json;
// </Dependencies>

namespace MlEntitySample
{
    public static class MlEntitySample
    {
        public static async Task Main()
        {
            // <VariablesYouChange>
            var key = "REPLACE-WITH-YOUR-AUTHORING-KEY";

            var authoringResourceName = "REPLACE-WITH-YOUR-AUTHORING-RESOURCE-NAME";
            var predictionResourceName = "REPLACE-WITH-YOUR-PREDICTION-RESOURCE-NAME";
            // </VariablesYouChange>

            // <VariablesYouDontNeedToChangeChange>
            var authoringEndpoint = String.Format("https://{0}.cognitiveservices.azure.com/", authoringResourceName);
            var predictionEndpoint = String.Format("https://{0}.cognitiveservices.azure.com/", predictionResourceName);

            var appName = "Contoso Pizza Company";
            var versionId = "0.1";
            var intentName = "OrderPizzaIntent";
            // </VariablesYouDontNeedToChangeChange>

            // <AuthoringCreateClient> 
            var credentials = new Microsoft.Azure.CognitiveServices.Language.LUIS.Authoring.ApiKeyServiceClientCredentials(key);
            var client = new LUISAuthoringClient(credentials) { Endpoint = authoringEndpoint };
            // </AuthoringCreateClient>

            // Create app
            var appId = await CreateApplication(client, appName, versionId);

            // <AddIntent>
            await client.Model.AddIntentAsync(appId, versionId, new ModelCreateObject()
            {
                Name = intentName
            });
            // </AddIntent>

            // Add Entities
            await AddEntities(client, appId, versionId);

            // Add Labeled example utterance
            await AddLabeledExample(client, appId, versionId, intentName);

            // <TrainAppVersion>
            await client.Train.TrainVersionAsync(appId, versionId);
            while (true)
            {
                var status = await client.Train.GetStatusAsync(appId, versionId);
                if (status.All(m => m.Details.Status == "Success"))
                {
                    // Assumes that we never fail, and that eventually we'll always succeed.
                    break;
                }
            }
            // </TrainAppVersion>

            // <PublishVersion>
            await client.Apps.PublishAsync(appId, new ApplicationPublishObject { VersionId = versionId, IsStaging=false});
            // </PublishVersion>

            // <PredictionCreateClient>
            var runtimeClient = new LUISRuntimeClient(credentials) { Endpoint = predictionEndpoint };
            // </PredictionCreateClient>
            
            // <QueryPredictionEndpoint>
            // Production == slot name
            var request = new PredictionRequest { Query = "I want two small pepperoni pizzas with more salsa" };
            var prediction = await runtimeClient.Prediction.GetSlotPredictionAsync(appId, "Production", request);
            Console.Write(JsonConvert.SerializeObject(prediction, Formatting.Indented));
            // </QueryPredictionEndpoint>
        }

        async static Task<Guid> CreateApplication(LUISAuthoringClient client, string appName, string versionId)
        {
            // <AuthoringCreateApplication>
            var newApp = new ApplicationCreateObject
            {
                Culture = "en-us",
                Name = appName,
                InitialVersionId = versionId
            };

            var appId = await client.Apps.AddAsync(newApp);
            // </AuthoringCreateApplication>

            Console.WriteLine("New app ID {0}.", appId);
            return appId;
        }


        async static Task AddEntities(LUISAuthoringClient client, Guid appId, string versionId)
        {

            // <AuthoringCreatePrebuiltEntity>
            // Add Prebuilt entity
            await client.Model.AddPrebuiltAsync(appId, versionId, new[] { "number" });
            // </AuthoringCreatePrebuiltEntity>

            // <AuthoringCreateMLEntity>
            // Define ml entity with children and grandchildren
            var mlEntityDefinition = new EntityModelCreateObject
            {
                Name = "Pizza order",
                Children = new[]
                {
                    new ChildEntityModelCreateObject
                    {
                        Name = "Pizza",
                        Children = new[]
                        {
                            new ChildEntityModelCreateObject { Name = "Quantity" },
                            new ChildEntityModelCreateObject { Name = "Type" },
                            new ChildEntityModelCreateObject { Name = "Size" }
                        }
                    },
                    new ChildEntityModelCreateObject
                    {
                        Name = "Toppings",
                        Children = new[]
                        {
                            new ChildEntityModelCreateObject { Name = "Type" },
                            new ChildEntityModelCreateObject { Name = "Quantity" }
                        }
                    }
                }
            };

            // Add ML entity 
            var mlEntityId = await client.Model.AddEntityAsync(appId, versionId, mlEntityDefinition); ;
            // </AuthoringCreateMLEntity>

            // <AuthoringCreatePhraselist >
            // Add phraselist feature
            var phraselistId = await client.Features.AddPhraseListAsync(appId, versionId, new PhraselistCreateObject
            {
                EnabledForAllModels = false,
                IsExchangeable = true,
                Name = "QuantityPhraselist",
                Phrases = "few,more,extra"
            });
            // </AuthoringCreatePhraselist >

            // <AuthoringGetModelObject>
            // Get entity and subentities
            var model = await client.Model.GetEntityAsync(appId, versionId, mlEntityId);
            var toppingQuantityId = GetModelGrandchild(model, "Toppings", "Quantity");
            var pizzaQuantityId = GetModelGrandchild(model, "Pizza", "Quantity");
            // </AuthoringGetModelObject>

            // <AuthoringAddModelAsFeature>
            // add model as feature to subentity model
            await client.Features.AddEntityFeatureAsync(appId, versionId, pizzaQuantityId, new ModelFeatureInformation { ModelName = "number", IsRequired = true });
            await client.Features.AddEntityFeatureAsync(appId, versionId, toppingQuantityId, new ModelFeatureInformation { ModelName = "number"});
            // </AuthoringAddModelAsFeature>

            // <AuthoringAddFeatureToModel>
            // add phrase list as feature to subentity model
            await client.Features.AddEntityFeatureAsync(appId, versionId, toppingQuantityId, new ModelFeatureInformation { FeatureName = "QuantityPhraselist" });
            // </AuthoringAddFeatureToModel>
        }

        
        async static Task AddLabeledExample(LUISAuthoringClient client, Guid appId, string versionId, string intentName)
        {
            // <AuthoringAddLabeledExamples>
            // Define labeled example
            var labeledExampleUtteranceWithMLEntity = new ExampleLabelObject
            {
                Text = "I want two small seafood pizzas with extra cheese.",
                IntentName = intentName,
                EntityLabels = new[]
                {
                    new EntityLabelObject
                    {
                        StartCharIndex = 7,
                        EndCharIndex = 48,
                        EntityName = "Pizza order",
                        Children = new[]
                        {
                            new EntityLabelObject
                            {
                                StartCharIndex = 7,
                                EndCharIndex = 30,
                                EntityName = "Pizza",
                                Children = new[]
                                {
                                    new EntityLabelObject { StartCharIndex = 7, EndCharIndex = 9, EntityName = "Quantity" },
                                    new EntityLabelObject { StartCharIndex = 11, EndCharIndex = 15, EntityName = "Size" },
                                    new EntityLabelObject { StartCharIndex = 17, EndCharIndex = 23, EntityName = "Type" }
                                }
                            },
                            new EntityLabelObject
                            {
                                StartCharIndex = 37,
                                EndCharIndex = 48,
                                EntityName = "Toppings",
                                Children = new[]
                                {
                                    new EntityLabelObject { StartCharIndex = 37, EndCharIndex = 41, EntityName = "Quantity" },
                                    new EntityLabelObject { StartCharIndex = 43, EndCharIndex = 48, EntityName = "Type" }
                                }
                            }
                        }
                    },
                }
            };

            // Add an example for the entity.
            // Enable nested children to allow using multiple models with the same name.
            // The quantity subentity and the phraselist could have the same exact name if this is set to True
            await client.Examples.AddAsync(appId, versionId, labeledExampleUtteranceWithMLEntity, enableNestedChildren: true); 
            // </AuthoringAddLabeledExamples>
        }

        // <AuthoringSortModelObject>
        static Guid GetModelGrandchild(NDepthEntityExtractor model, string childName, string grandchildName)
        {
            return model.Children.
                Single(c => c.Name == childName).
                Children.
                Single(c => c.Name == grandchildName).Id;
        }
        // </AuthoringSortModelObject>
    }
}
