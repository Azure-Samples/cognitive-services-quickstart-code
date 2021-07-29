// <dependencies>
using System;
using System.Net.Http;
// </dependencies>

namespace QnAMakerPublishQuickstart
{
    class Program
    {
// <constants>
        private static readonly string subscriptionKey = "PASTE_YOUR_QNA_MAKER_AUTHORING_SUBSCRIPTION_KEY_HERE";
        private static readonly string endpoint = "PASTE_YOUR_QNA_MAKER_AUTHORING_ENDPOINT_HERE";
        private static readonly string kbId = "PASTE_YOUR_QNA_MAKER_KB_ID_HERE";
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
