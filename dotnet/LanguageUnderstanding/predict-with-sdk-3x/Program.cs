// <snippet_using>
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Authoring;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Authoring.Models;
// </snippet_using>

namespace UseAuthoring
{
    class Program
    {
        // <snippet_variables>
        // Use Language Understanding (LUIS) authoring key
        // to create authentication credentials
        // Make sure the non-public key has been assigned to the app before running script
        var authoringKey = Environment.GetEnvironmentVariable("LUIS_AUTHORING_KEY");   

        // Endpoint URL example value = "https://westus.api.cognitive.microsoft.com"
        var authoringEndpoint = Environment.GetEnvironmentVariable("LUIS_AUTHORING_ENDPOINT");   

        // App Id example value = "df67dcdb-c37d-46af-88e1-8b97951ca1c2"
        var appId = Environment.GetEnvironmentVariable("LUIS_APP_ID");   
        // </snippet_variables>

        // <snippet_main>
        static void Main(string[] args)
        {

            var credentials = new ApiKeyServiceClientCredentials(endpointPredictionkey);

            // Create Luis client and set endpoint
            // region of endpoint must match key's region
            var luisClient = new LUISRuntimeClient(credentials, new System.Net.Http.DelegatingHandler[] { });
            luisClient.Endpoint = authoringEndpoint;

            // Intent - ModifyOrder
            // Entity - machine-learned - PizzaOrder
            //      descriptor - phrase list - for pizza sizes
            //      subcomponent - size - constrained by list entity
            //      subcomponent - quantity - contrained by prebuilt number entity
            CreateIntent("ModifyOrder");
        }
        // </snippet_main>

        // <snippet_maintask_create_intent>
        static async Task<PredictionResponse> CreateIntent(client, intentName) {

            Using(async client =>
            {
                var newIntent = new ModelCreateObject
                {
                    Name = "TestIntent"
                };

                var newIntentId = await client.Model.AddIntentAsync(GlobalAppId, versionId, newIntent);
                var intents = await client.Model.ListIntentsAsync(GlobalAppId, versionId);
                await client.Model.DeleteIntentAsync(GlobalAppId, versionId, newIntentId);

                Assert.True(newIntentId != Guid.Empty);
                Assert.Contains(intents, i => i.Id.Equals(newIntentId) && i.Name.Equals(newIntent.Name));
            });
        }
        // </snippet_maintask_create_intent>
    }
}