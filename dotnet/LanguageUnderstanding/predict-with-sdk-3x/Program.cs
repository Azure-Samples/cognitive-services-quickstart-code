using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;

namespace UsePredictionRuntime
{
    class Program
    {
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
        static async Task<PredictionResponse> GetPrediction() {

            // Use Language Understanding or Cognitive Services key
            // to create authentication credentials
            var endpointPredictionkey = Environment.GetEnvironmentVariable("LUIS_PREDICTION_ENDPOINT_SUBSCRIPTION_KEY");
            var credentials = new ApiKeyServiceClientCredentials(endpointPredictionkey);

            // Create Luis client and set endpoint
            // region of endpoint must match key's region
            var luisClient = new LUISRuntimeClient(credentials, new System.Net.Http.DelegatingHandler[] { });
            luisClient.Endpoint = "https://westus.api.cognitive.microsoft.com";

            // Set query values

            // public Language Understanding Home Automation app
            var appId = "df67dcdb-c37d-46af-88e1-8b97951ca1c2";

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
}