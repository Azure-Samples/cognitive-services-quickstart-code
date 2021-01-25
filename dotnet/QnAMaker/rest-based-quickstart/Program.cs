// <snippet_using>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


// NOTE: Install the Newtonsoft.Json NuGet package.
// > dotnet add package Newtonsoft.Json
using Newtonsoft.Json;
// </snippet_using>

namespace QnaMakerRestDotnet
{
    class Program
    {

        // <snippet_variables>
        // 36 character key from Azure portal for QnA Maker resource
        static string authoringKey = "QNA_MAKER_AUTHORING_RESOURCE_KEY";

        // example://https://YOUR-RESOURCE_NAME.cognitiveservices.azure.com with NO trailing forward slash
        static string authoringEndpoint = "QNA_MAKER_AUTHORING_RESOURCE_ENDPOINT";
        static string resourceName = "QNA_MAKER_AUTHORING_RESOURCE_NAME";

        static string service = "/qnamaker/v4.0";
        // </snippet_variables>


        // <snippet_knowledge_base_json>
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
        // </snippet_knowledge_base_json>

        // <snippet_response_structure>
        /// Represents the HTTP response returned by an HTTP request.
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
        // </snippet_response_structure>

        // <snippet_pretty_print>
        static string PrettyPrint(string s)
        {
            return JsonConvert.SerializeObject(JsonConvert.DeserializeObject(s), Formatting.Indented);
        }
        // </snippet_pretty_print>

        // <snippet_post>
        async static Task<Response> Post(string uri, string key, string body, Boolean authoringAuthorizationHeader=true)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(uri);

                Console.WriteLine($"uri = {uri}");
                Console.WriteLine($"body = {body}");

                if (!String.IsNullOrEmpty(body))
                {
                    request.Content = new StringContent(body, Encoding.UTF8, "application/json");
                }

                if(authoringAuthorizationHeader){
                    Console.WriteLine($"Ocp-Apim-Subscription-Key {key}");
                    request.Headers.Add("Ocp-Apim-Subscription-Key", key);
                } else {
                    Console.WriteLine($"Authorization EndpointKey {key}");
                    request.Headers.Add("Authorization", $"EndpointKey {key}");
                }

                var response = await client.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();
                return new Response(response, response.Headers, responseBody);
            }
        }
        // </snippet_post>

        // <snippet_get>
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
        // </snippet_get>

        // <snippet_get_status>
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
        // </snippet_get_status>

        // <snippet_create_kb>
        async static Task<String> CreateKB()
        {
            string uri = authoringEndpoint + service + "/knowledgebases/create";

            // Starts the QnA Maker operation to create the knowledge base.
            var response = await Post(uri, authoringKey, kb);

            // Retrieves the operation ID, so the operation's status can be
            // checked periodically.
            var operation = response.headers.GetValues("Location").First();

            return await GetStatus(operation);
        }
        // </snippet_create_kb>

        // <snippet_publish_kb>
        async static Task<Response> PublishKB(string knowledgeBaseID)
        {
            string uri = authoringEndpoint + service + $"/knowledgebases/{knowledgeBaseID}";

            var response = await Post(uri, authoringKey, null);

            Console.WriteLine($"KB published successfully? {(response.statusCode == "NoContent" ? "Yes" : "No")}");

            return response;
        }
        // </snippet_publish_kb>

        // <snippet_get_endpoint_key>
        async static Task<String> GetEndpointKey()
        {

            string uri = authoringEndpoint + service + "/endpointkeys";

            var response = await Get(uri, authoringKey);

            var fields = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.response);

            var endpointKey = fields["primaryEndpointKey"];

            return endpointKey;
        }
        // </snippet_get_endpoint_key>

        // <snippet_query>
        async static Task<string> Query(string endpointKey, string  knowledgeBaseID){

            var uri =  $"https://{resourceName}.azurewebsites.net/qnamaker/knowledgebases/{knowledgeBaseID}/generateAnswer";

            string question = @"{'question': 'Is the QnA Maker Service free?','top': 3}";

            var response = await Post(uri, endpointKey, question, false);

            return response.response;

        }
        // </snippet_query>

        // <snippet_main>
        static void Main(string[] args)
        {
            // Create knowledge base - get ID
            var knowledgeBaseID = CreateKB().Result;
            Console.WriteLine($"knowledge base is {knowledgeBaseID}");

            // publish
            var result = PublishKB(knowledgeBaseID).Result;

            // get endpoint key after publish
            var endpointKey = GetEndpointKey().Result;
            Console.WriteLine($"endpointKey = {endpointKey}");

            // query endpoint
            var queryResponse = Query(endpointKey, knowledgeBaseID).Result;

            // print results
            Console.Write(PrettyPrint(queryResponse));
        }
        // </snippet_main>
    }
}
