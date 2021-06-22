//
// This quickstart shows how to predict the intent of an utterance by using the LUIS REST APIs.
//

using System;
using System.Net.Http;
using System.Web;

namespace predict_with_rest
{
    class Program
    {
        static void Main(string[] args)
        {
            //////////
            // Values to modify.

            // YOUR-APP-ID: The App ID GUID found on the www.luis.ai Application Settings page.
            var appId = "YOUR-APP-ID";

            // YOUR-PREDICTION-KEY: 32 character key.
            var predictionKey = "YOUR-PREDICTION-KEY";

            // YOUR-PREDICTION-ENDPOINT: Example is "https://westus.api.cognitive.microsoft.com/"
            var predictionEndpoint = "https://YOUR-PREDICTION-ENDPOINT/";

            // An utterance to test the pizza app.
            var utterance = "I want two large pepperoni pizzas on thin crust please";
            //////////

            MakeRequest(predictionKey, predictionEndpoint, appId, utterance);

            Console.WriteLine("Press ENTER to exit...");
            Console.ReadLine();
        }

        static async void MakeRequest(string predictionKey, string predictionEndpoint, string appId, string utterance)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // The request header contains your subscription key
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", predictionKey);

            // The "q" parameter contains the utterance to send to LUIS
            queryString["query"] = utterance;

            // These optional request parameters are set to their default values
            // queryString["verbose"] = "true";
            // queryString["show-all-intents"] = "true";
            // queryString["staging"] = "false";
            // queryString["timezoneOffset"] = "0";

            var predictionEndpointUri = String.Format("{0}luis/prediction/v3.0/apps/{1}/slots/production/predict?{2}", predictionEndpoint, appId, queryString);

            // Remove these before updating the article.
            Console.WriteLine("endpoint: " + predictionEndpoint);
            Console.WriteLine("appId: " + appId);
            Console.WriteLine("queryString: " + queryString);
            Console.WriteLine("endpointUri: " + predictionEndpointUri);

            var response = await client.GetAsync(predictionEndpointUri);

            var strResponseContent = await response.Content.ReadAsStringAsync();

            // Display the JSON result from LUIS.
            Console.WriteLine(strResponseContent.ToString());
        }
    }
}
