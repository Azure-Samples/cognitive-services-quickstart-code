// <snippet_using>
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;
// </snippet_using>

namespace UseRuntime
{
    class Program
    {
        // <snippet_variables>
        // Use Language Understanding (LUIS) prediction endpoint key
        // to create authentication credentials
        private static string predictionKey = Environment.GetEnvironmentVariable("LUIS_PREDICTION_KEY");

        // Endpoint URL example value = "https://YOUR-RESOURCE-NAME.api.cognitive.microsoft.com"
        private static string predictionEndpoint = Environment.GetEnvironmentVariable("LUIS_ENDPOINT_NAME");

        // App Id example value = "df67dcdb-c37d-46af-88e1-8b97951ca1c2"
        private static string appId = Environment.GetEnvironmentVariable("LUIS_APP_ID");
        // </snippet_variables>

        // <snippet_main>
        static void Main(string[] args)
        {

            // Get prediction
            var predictionResult = GetPredictionAsync().Result;

            var prediction = predictionResult.Prediction;

            // Display query
            Console.WriteLine("Query:'{0}'", predictionResult.Query);
            Console.WriteLine("TopIntent :'{0}' ", prediction.TopIntent);

            foreach (var i in prediction.Intents)
            {
                Console.WriteLine(string.Format("{0}:{1}", i.Key, i.Value.Score));
            }

            foreach (var e in prediction.Entities)
            {
                Console.WriteLine(string.Format("{0}:{1}", e.Key, e.Value));
            }

            Console.Write("done");

        }
        // </snippet_main>

        // <snippet_create_client>
        static LUISRuntimeClient CreateClient()
        {
            var credentials = new ApiKeyServiceClientCredentials(predictionKey);
            var luisClient = new LUISRuntimeClient(credentials, new System.Net.Http.DelegatingHandler[] { })
            {
                Endpoint = predictionEndpoint
            };

            return luisClient;

        }
        // </snippet_create_client>

        // <snippet_maintask>
        static async Task<PredictionResponse> GetPredictionAsync()
        {

            // Get client 
            using (var luisClient = CreateClient())
            {

                var requestOptions = new PredictionRequestOptions
                {
                    DatetimeReference = DateTime.Parse("2019-01-01"),
                    PreferExternalEntities = true
                };

                var predictionRequest = new PredictionRequest
                {
                    Query = "turn on the bedroom light",
                    Options = requestOptions
                };

                // get prediction
                return await luisClient.Prediction.GetSlotPredictionAsync(
                    Guid.Parse(appId),
                    slotName: "production",
                    predictionRequest,
                    verbose: true,
                    showAllIntents: true,
                    log: true);
            }
        }
        // </snippet_maintask>
    }
}
