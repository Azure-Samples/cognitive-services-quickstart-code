using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// NOTE: Install the Newtonsoft.Json NuGet package.
using Newtonsoft.Json;

namespace QnaQuickstartCreateKnowledgebase
{
    class Program
    {
        private const string authoringKeyVar = "QNA_MAKER_SUBSCRIPTION_KEY";
        private const string authoringEndpointVar = "QNA_MAKER_authoringEndpoint";

        private static readonly string authoringKey = "a67a3224522f450ca24f9eb2f2dd3d0c";//Environment.GetEnvironmentVariable(authoringKeyVar);
        private static readonly string authoringEndpoint = "https://westus.api.cognitive.microsoft.com";//Environment.GetEnvironmentVariable(authoringEndpointVar);

        // Represents the various elements used to create HTTP request URIs
        // for QnA Maker operations.
        static string service = "/qnamaker/v4.0";


        private const string resourceName = "diberry-qna-maker-central-us";

        /// <summary>
        /// Defines the data source used to create the knowledge base.
        /// The data source includes a QnA pair, with metadata,
        /// the URL for the QnA Maker FAQ article, and
        /// the URL for the Azure Bot Service FAQ article.
        /// </summary>
        static string kb = @"
{
  'name': 'QnA Maker FAQ',
  'qnaList': [
    {
      'id': 0,
      'answer': 'You can use our REST APIs to manage your knowledge base. See here for details: https://westus.dev.cognitive.microsoft.com/docs/services/58994a073d9e04097c7ba6fe/operations/58994a073d9e041ad42d9baa',
      'source': 'Custom Editorial',
      'questions': [
        'How do I programmatically update my knowledge base?'
      ],
      'metadata': [
        {
          'name': 'category',
          'value': 'api'
        }
      ]
    }
  ],
  'urls': [
    'https://docs.microsoft.com/en-in/azure/cognitive-services/qnamaker/faqs'
  ],
  'files': []
}
";

        /// <summary>
        /// Represents the HTTP response returned by an HTTP request.
        /// </summary>
        public struct Response
        {
            public HttpResponseHeaders headers;
            public string response;

            public string statusCode;

            public Response(HttpResponseMessage responseMessage, HttpResponseHeaders headers, string response)
            {
                this.headers = headers;
                this.response = response;
                this.statusCode = responseMessage.StatusCode.ToString();
            }
        }

        /// <summary>
        /// Formats and indents JSON for display.
        /// </summary>
        /// <param name="s">The JSON to format and indent.</param>
        /// <returns>A string containing formatted and indented JSON.</returns>
        static string PrettyPrint(string s)
        {
            return JsonConvert.SerializeObject(JsonConvert.DeserializeObject(s), Formatting.Indented);
        }

        /// <summary>
        /// Asynchronously sends a POST HTTP request.
        /// </summary>
        /// <param name="uri">The URI of the HTTP request.</param>
        /// <param name="body">The body of the HTTP request.</param>
        /// <returns>A <see cref="System.Threading.Tasks.Task{TResult}(QnAMaker.Program.Response)"/>
        /// object that represents the HTTP response."</returns>
        async static Task<Response> Post(string uri, string key, string body, Boolean authoringAuthorizationHeader=true)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(uri);

                if (!String.IsNullOrEmpty(body))
                {
                    request.Content = new StringContent(body, Encoding.UTF8, "application/json");
                }

                //if(authoringAuthorizationHeader){
                //    Console.WriteLine($"Ocp-Apim-Subscription-Key {key}");
                    request.Headers.Add("Ocp-Apim-Subscription-Key", key);
                //} else {
                //    Console.WriteLine($"Authorization Endpoint {key}");
                //    request.Headers.Add("Authorization", $"Endpoint {key}");
                //}

                var response = await client.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();
                return new Response(response, response.Headers, responseBody);
            }
        }

        async static Task<Response> QueryPost(string uri, string key, string body, Boolean authoringAuthorizationHeader=true)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(uri);

                if (!String.IsNullOrEmpty(body))
                {
                    request.Content = new StringContent(body, Encoding.UTF8, "application/json");
                }

                //if(authoringAuthorizationHeader){
                //    Console.WriteLine($"Ocp-Apim-Subscription-Key {key}");
                    //request.Headers.Add("Ocp-Apim-Subscription-Key", key);
                //} else {
                //    Console.WriteLine($"Authorization Endpoint {key}");
                    request.Headers.Add("Authorization", $"EndpointKey {key}");
                //}

                //request.Headers.Add("Content-type", "application/json");
                var response = await client.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();
                return new Response(response, response.Headers, responseBody);
            }
        }

        /// <summary>
        /// Asynchronously sends a GET HTTP request.
        /// </summary>
        /// <param name="uri">The URI of the HTTP request.</param>
        /// <returns>A <see cref="System.Threading.Tasks.Task{TResult}(QnAMaker.Program.Response)"/>
        /// object that represents the HTTP response."</returns>
        async static Task<Response> Get(string uri, string key)
        {
                using (var client = new HttpClient())
                using (var request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Get;
                    request.RequestUri = new Uri(uri);
                    request.Headers.Add("Ocp-Apim-Subscription-Key", key);

                    var response = await client.SendAsync(request);
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("get response body ${responseBody}");
                    return new Response(response, response.Headers, responseBody);
                }

        }

        /// <summary>
        /// Gets the status of the specified QnA Maker operation.
        /// </summary>
        /// <param name="operation">The QnA Maker operation to check.</param>
        /// <returns>A <see cref="System.Threading.Tasks.Task{TResult}(QnAMaker.Program.Response)"/>
        /// object that represents the HTTP response."</returns>
        /// <remarks>The method constructs the URI to get the status of a QnA Maker operation, and
        /// then asynchronously invokes the <see cref="QnAMaker.Program.Get(string)"/> method
        /// to send the HTTP request.</remarks>
        async static Task<String> GetStatus(string operation)
        {

            // Iteratively gets the state of the operation creating the
            // knowledge base. Once the operation state is set to something other
            // than "Running" or "NotStarted", the loop ends.
            var knowledgeBaseID = "";
            string uri = authoringEndpoint + service + operation;

            var done = false;
            while (true != done)
            {

                // Writes the HTTP request URI to the console, for display purposes.
                Console.WriteLine("Calling " + uri + ".");

                var response = await Get(uri, authoringKey);

                // Displays the JSON in the HTTP response returned by the
                // GetStatus(string) method.
                Console.WriteLine(PrettyPrint(response.response));

                // Deserialize the JSON into key-value pairs, to retrieve the
                // state of the operation.
                var fields = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.response);

                // Gets and checks the state of the operation.
                String state = fields["operationState"];
                if (state.CompareTo("Running") == 0 || state.CompareTo("NotStarted") == 0)
                {
                    // QnA Maker is still creating the knowledge base. The thread is
                    // paused for a number of seconds equal to the Retry-After header value,
                    // and then the loop continues.
                    var wait = response.headers.GetValues("Retry-After").First();
                    Console.WriteLine("Waiting " + wait + " seconds...");
                    Thread.Sleep(Int32.Parse(wait) * 1000);
                }
                else
                {
                    // QnA Maker has completed creating the knowledge base.
                    done = true;
                    knowledgeBaseID = fields["resourceLocation"].Replace("/knowledgebases/", "");
                }
            }
            Console.WriteLine($"knowledge base is {knowledgeBaseID}");
            return knowledgeBaseID;


        }

        /// <summary>
        /// Creates a knowledge base, periodically checking status
        /// until the knowledge base is created.
        /// </summary>
        async static Task<String> CreateKB()
        {
            string method = "/knowledgebases/create";
            string uri = authoringEndpoint + service + method;

            // Starts the QnA Maker operation to create the knowledge base.
            var response = await Post(uri, authoringKey, kb);

            // Retrieves the operation ID, so the operation's status can be
            // checked periodically.
            var operation = response.headers.GetValues("Location").First();

            var knowledgeBaseID = await GetStatus(operation);
            Console.WriteLine($"knowledge base is {knowledgeBaseID}");
            return knowledgeBaseID;
        }

        async static Task<Response> PublishKB(string knowledgeBaseID)
        {

            string uri = authoringEndpoint + service + $"/knowledgebases/{knowledgeBaseID}";

            Console.WriteLine($"uri is {uri}");
            var response = await Post(uri, authoringKey, null);

            Console.WriteLine($"KB published successfully? {(response.statusCode == "NoContent" ? "Yes" : "No")}");

            return response;
        }

        async static Task<String> GetEndpointKey()
        {

            string uri = authoringEndpoint + service + "/endpointkeys";

            Console.WriteLine($"uri is {uri}");
            var response = await Get(uri, authoringKey);
            var fields = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.response);

            var endpointKey = fields["primaryEndpointKey"];

            return endpointKey;
        }

        async static Task<string> Query(string endpointKey, string  knowledgeBaseID){
            string queryEndpoint = $"https://{resourceName}.azurewebsites.net";
            var uri =  $"{queryEndpoint}/qnamaker/knowledgebases/{knowledgeBaseID}/generateAnswer";
            string question = @"{'question': 'Is the QnA Maker Service free?','top': 3}";

            Console.WriteLine($"query uri = {uri}");
            Console.WriteLine($"query endpointKey = {endpointKey}");

            var response = await QueryPost(uri, endpointKey, question, false);

            return response.response;

        }

        static void Main(string[] args)
        {
            // Invoke the CreateKB() method to create a knowledge base, periodically
            // checking the status of the QnA Maker operation until the
            // knowledge base is created.
            var knowledgeBaseID = CreateKB().Result;
            Console.WriteLine($"knowledge base is {knowledgeBaseID}");
            var result = PublishKB(knowledgeBaseID).Result;
            var endpointKey = GetEndpointKey().Result;
            Console.WriteLine($"endpointKey = {endpointKey}");
            var queryResponse = Query(endpointKey, knowledgeBaseID).Result;

            Console.Write(PrettyPrint(queryResponse));
        }
    }
}
