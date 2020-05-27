//
// This quickstart shows how to add utterances to a LUIS model using the REST APIs.
//

using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

// 3rd party NuGet packages
using JsonFormatterPlus;

namespace AddUtterances
{
    class Program
    {
        //////////
        // Values to modify.

        // YOUR-APP-ID: The App ID GUID found on the www.luis.ai Application Settings page.
        static string appID = "YOUR-APP-ID";

        // YOUR-AUTHORING-KEY: Your LUIS authoring key, 32 character value.
        static string authoringKey = "YOUR-AUTHORING-KEY";

        // YOUR-AUTHORING-ENDPOINT: Replace this endpoint with your authoring key endpoint.
        // For example, "https://your-resource-name.cognitiveservices.azure.com/"
        static string authoringEndpoint = "https://YOUR-AUTHORING-ENDPOINT/";

        // NOTE: Replace this your version number.
        static string appVersion = "0.1";
        //////////

        static string host = String.Format("{0}luis/authoring/v3.0-preview/apps/{1}/versions/{2}/", authoringEndpoint, appID, appVersion);

        // GET request with authentication
        async static Task<HttpResponseMessage> SendGet(string uri)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Get;
                request.RequestUri = new Uri(uri);
                request.Headers.Add("Ocp-Apim-Subscription-Key", authoringKey);
                return await client.SendAsync(request);
            }
        }

        // POST request with authentication
        async static Task<HttpResponseMessage> SendPost(string uri, string requestBody)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(uri);

                if (!String.IsNullOrEmpty(requestBody))
                {
                    request.Content = new StringContent(requestBody, Encoding.UTF8, "text/json");
                }

                request.Headers.Add("Ocp-Apim-Subscription-Key", authoringKey);
                return await client.SendAsync(request);
            }
        }

        // Add utterances as string with POST request
        async static Task AddUtterances(string utterances)
        {
            string uri = host + "examples";

            var response = await SendPost(uri, utterances);
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Added utterances.");
            Console.WriteLine(JsonFormatter.Format(result));
        }

        // Train app after adding utterances
        async static Task Train()
        {
            string uri = host  + "train";

            var response = await SendPost(uri, null);
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Sent training request.");
            Console.WriteLine(JsonFormatter.Format(result));
        }

        // Check status of training
        async static Task Status()
        {
            var response = await SendGet(host  + "train");
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Requested training status.");
            Console.WriteLine(JsonFormatter.Format(result));
        }

        // Add utterances, train, check status
        static void Main(string[] args)
        {
            string utterances = @"
            [
                {
                    'text': 'order a pizza',
                    'intentName': 'ModifyOrder',
                    'entityLabels': [
                        {
                            'entityName': 'Order',
                            'startCharIndex': 6,
                            'endCharIndex': 12
                        }
                    ]
                },
                {
                    'text': 'order a large pepperoni pizza',
                    'intentName': 'ModifyOrder',
                    'entityLabels': [
                        {
                            'entityName': 'Order',
                            'startCharIndex': 6,
                            'endCharIndex': 28
                        },
                        {
                            'entityName': 'FullPizzaWithModifiers',
                            'startCharIndex': 6,
                            'endCharIndex': 28
                        },
                        {
                            'entityName': 'PizzaType',
                            'startCharIndex': 14,
                            'endCharIndex': 28
                        },
                        {
                            'entityName': 'Size',
                            'startCharIndex': 8,
                            'endCharIndex': 12
                        }
                    ]
                },
                {
                    'text': 'I want two large pepperoni pizzas on thin crust',
                    'intentName': 'ModifyOrder',
                    'entityLabels': [
                        {
                            'entityName': 'Order',
                            'startCharIndex': 7,
                            'endCharIndex': 46
                        },
                        {
                            'entityName': 'FullPizzaWithModifiers',
                            'startCharIndex': 7,
                            'endCharIndex': 46
                        },
                        {
                            'entityName': 'PizzaType',
                            'startCharIndex': 17,
                            'endCharIndex': 32
                        },
                        {
                            'entityName': 'Size',
                            'startCharIndex': 11,
                            'endCharIndex': 15
                        },
                        {
                            'entityName': 'Quantity',
                            'startCharIndex': 7,
                            'endCharIndex': 9
                        },
                        {
                            'entityName': 'Crust',
                            'startCharIndex': 37,
                            'endCharIndex': 46
                        }
                    ]
                }
            ]
            ";

            AddUtterances(utterances).Wait();
            Train().Wait();
            Status().Wait();
        }
    }
}