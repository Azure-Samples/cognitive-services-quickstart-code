using System;
using System.Net.Http;
using System.Text;

namespace QnAMakerAnswerQuestion
{
    class Program
    {
        private const string endpointVar = "QNA_MAKER_RESOURCE_ENDPOINT";
        private const string endpointKeyVar = "QNA_MAKER_ENDPOINT_KEY";
        private const string kbIdVar = "QNA_MAKER_KB_ID";

        // Your QnA Maker resource endpoint.
        // From Publish Page: HOST
        // Example: https://YOUR-RESOURCE-NAME.azurewebsites.net/
        private static readonly string endpoint = Environment.GetEnvironmentVariable(endpointVar);
        // Authorization endpoint key
        // From Publish Page
        // Note this is not the same as your QnA Maker subscription key.
        private static readonly string endpointKey = Environment.GetEnvironmentVariable(endpointKeyVar);
        private static readonly string kbId = Environment.GetEnvironmentVariable(kbIdVar);

        /// <summary>
        /// Static constuctor. Verifies that we found the subscription key and
        /// endpoint in the environment variables.
        /// </summary>
        static Program()
        {
            if (null == endpointKey)
            {
                throw new Exception("Please set/export the environment variable: " + endpointKeyVar);
            }
            if (null == endpoint)
            {
                throw new Exception("Please set/export the environment variable: " + endpointVar);
            }
            if (null == kbId)
            {
                throw new Exception("Please set/export the environment variable: " + kbIdVar);
            }
        }

        static void Main(string[] args)
        {
            var uri = endpoint + "/qnamaker/v4.0/knowledgebases/" + kbId + "/generateAnswer";

            // JSON format for passing question to service
            string question = @"{'question': 'Is the QnA Maker Service free?','top': 3}";

            // Create http client
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // POST method
                request.Method = HttpMethod.Post;

                // Add host + service to get full URI
                request.RequestUri = new Uri(uri);

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
        }
    }
}
