// <dependencies>
using System;
using System.Net.Http;
// </dependencies>

namespace QnAMakerPublishQuickstart
{
    class Program
    {
// <constants>
        private const string subscriptionKeyVar = "QNA_MAKER_SUBSCRIPTION_KEY";
        private const string endpointVar = "QNA_MAKER_ENDPOINT";
        private const string kbIdVar = "QNA_MAKER_KB_ID";

        private static readonly string subscriptionKey = Environment.GetEnvironmentVariable(subscriptionKeyVar);
        private static readonly string endpoint = Environment.GetEnvironmentVariable(endpointVar);
        private static readonly string kbId = Environment.GetEnvironmentVariable(kbIdVar);

        /// <summary>
        /// Static constuctor. Verifies that we found the subscription key and
        /// endpoint in the environment variables.
        /// </summary>
        static Program()
        {
            if (null == subscriptionKey)
            {
                throw new Exception("Please set/export the environment variable: " + subscriptionKeyVar);
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
// </constants>

        static void Main(string[] args)
        {
// <post>
            var uri = endpoint + "/qnamaker/v4.0/knowledgebases/" + kbId;

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(uri);
                request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                // Send request to Azure service, get response
                // returns 204 with no content
                var response = client.SendAsync(request).Result;

                Console.WriteLine("KB published successfully? " + response.IsSuccessStatusCode);

                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
            }
// </post>
        }
    }
}
