using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.CognitiveServices.Search.AutoSuggest;

namespace Autosuggest_CS
{
    class Program
    {
        private const string key_var = "AUTOSUGGEST_SUBSCRIPTION_KEY";
        private static readonly string subscription_key = Environment.GetEnvironmentVariable(key_var);

        // Note you must use the same region as you used to get your subscription key.
        private const string endpoint_var = "AUTOSUGGEST_ENDPOINT";
        private static readonly string endpoint = Environment.GetEnvironmentVariable(endpoint_var);

        static Program()
        {
            if (null == subscription_key)
            {
                throw new Exception("Please set/export the environment variable: " + key_var);
            }
            if (null == endpoint)
            {
                throw new Exception("Please set/export the environment variable: " + endpoint_var);
            }
        }

        async static Task RunQuickstart()
        {
            // Generate the credentials and create the client.
            var credentials = new Microsoft.Azure.CognitiveServices.Search.AutoSuggest.ApiKeyServiceClientCredentials(subscription_key);
            var client = new AutoSuggestClient(credentials, new System.Net.Http.DelegatingHandler[] { })
            {
                Endpoint = endpoint
            };

            var result = await client.AutoSuggestMethodAsync("xb");
            var groups = result.SuggestionGroups;
            if (groups.Count > 0) {
                var group = groups[0];
                Console.Write("First suggestion group: {0}\n", group.Name);
                var suggestions = group.SearchSuggestions;
                if (suggestions.Count > 0)
                {
                    Console.WriteLine("First suggestion:");
                    Console.WriteLine("Query: {0}", suggestions[0].Query);
                    Console.WriteLine("Display text: {0}", suggestions[0].DisplayText);
                }
                else
                {
                    Console.WriteLine("No suggestions found in this group.");
                }
            }
            else
            {
                Console.WriteLine("No suggestions found.");
            }
        }

        static void Main(string[] args)
        {
            Task.WaitAll(RunQuickstart());
            Console.WriteLine("Press any key to exit.");
            Console.Read();
        }
    }
}
