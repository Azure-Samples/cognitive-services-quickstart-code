// <snippet_using>
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;
// </snippet_using>

namespace UsePredictionRuntime
{
    class Program
    {
        // <snippet_variables>
        // Use Language Understanding (LUIS) endpoint key
        // to create authentication credentials
        // Make sure the non-public key has been assigned to the app before running script
        var endpointPredictionkey = Environment.GetEnvironmentVariable("LUIS_PREDICTION_ENDPOINT_SUBSCRIPTION_KEY");   

        // Endpoint URL example value = "https://westus.api.cognitive.microsoft.com"
        var endpointPredictionEndpoint = Environment.GetEnvironmentVariable("LUIS_PREDICTION_ENDPOINT");   

        // App Id example value = "df67dcdb-c37d-46af-88e1-8b97951ca1c2"
        var endpointPredictionAppId = Environment.GetEnvironmentVariable("LUIS_PREDICTION_APP_ID");   
        // </snippet_variables>

        // <snippet_main>
        static void Main(string[] args)
        {

            var predictionResult = GetPrediction().Result;

            var prediction = predictionResult.Prediction;

            // Display query
            Console.WriteLine("Query:'{0}'", predictionResult.Query);

            Console.WriteLine("TopIntent :'{0}' ", prediction.TopIntent);

            foreach(var i in prediction.Intents)
            {
                Console.WriteLine(string.Format("{0}:{1}", i.Key, i.Value.Score));
            }

            foreach(var e in prediction.Entities)
            {
                Console.WriteLine(string.Format("{0}:{1}", e.Key, e.Value));
            }

            Console.Write("done");

        }
        // </snippet_main>

        // <snippet_maintask>
        static async Task<PredictionResponse> GetPrediction() {

            var credentials = new ApiKeyServiceClientCredentials(endpointPredictionkey);

            // Create Luis client and set endpoint
            // region of endpoint must match key's region
            var luisClient = new LUISRuntimeClient(credentials, new System.Net.Http.DelegatingHandler[] { });
            luisClient.Endpoint = endpointPredictionEndpoint;

            // Set query values

            // public Language Understanding Home Automation app
            var appId = endpointPredictionAppId;

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
        // </snippet_maintask>
    }
}