// <dependencies>
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

// NOTE: Install the Newtonsoft.Json NuGet package.
// > dotnet add package Newtonsoft.Json
using Newtonsoft.Json;
// </dependencies>

namespace QnAMakerAnswerQuestion
{
    class Program
    {
// <constants>
        private static readonly string authoringKey = "PASTE_YOUR_QNA_MAKER_AUTHORING_SUBSCRIPTION_KEY_HERE";
        private static readonly string authoringEndpoint = "PASTE_YOUR_QNA_MAKER_AUTHORING_ENDPOINT_HERE";
        private static readonly string runtimeEndpoint = "PASTE_YOUR_QNA_MAKER_RUNTIME_ENDPOINT_HERE";
        private static readonly string kbId = "PASTE_YOUR_QNA_MAKER_KB_ID_HERE";
// </constants>

        static void Main(string[] args)
        {
// <get>
			var runtimeKeyUri = authoringEndpoint + "/qnamaker/v4.0/endpointKeys";
			var endpointKey = "";

            // Create http client
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // GET method
                request.Method = HttpMethod.Get;

                // Add host + service to get full URI
                request.RequestUri = new Uri(runtimeKeyUri);

                // set authorization
                request.Headers.Add("Ocp-Apim-Subscription-Key", authoringKey);

                // Send request to Azure service, get response
                var response = client.SendAsync(request).Result;
				var responseBody = response.Content.ReadAsStringAsync().Result;

                // Deserialize the JSON into key-value pairs, to retrieve the
                // state of the operation.
                var fields = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody);
				endpointKey = fields["primaryEndpointKey"];
			}
// </get>

// <post>
            var queryUri = runtimeEndpoint + "/qnamaker/v4.0/knowledgebases/" + kbId + "/generateAnswer";

            // JSON format for passing question to service
            string question = @"{'question': 'Is the QnA Maker Service free?','top': 3}";

            // Create http client
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // POST method
                request.Method = HttpMethod.Post;

                // Add host + service to get full URI
                request.RequestUri = new Uri(queryUri);

                // set question
                request.Content = new StringContent(question, Encoding.UTF8, "application/json");

                // set authorization
                request.Headers.Add("Authorization", "EndpointKey " + endpointKey);

                // Send request to Azure service, get response
                var response = client.SendAsync(request).Result;
                var jsonResponse = response.Content.ReadAsStringAsync().Result;

                // Output JSON response
                Console.WriteLine(jsonResponse);

                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
            }
// </post>
        }
    }
}
