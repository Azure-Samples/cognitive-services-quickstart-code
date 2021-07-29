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
        private static readonly string subscription_key = "PASTE_YOUR_AUTO_SUGGEST_SUBSCRIPTION_KEY_HERE";
        private static readonly string endpoint = "PASTE_YOUR_AUTO_SUGGEST_ENDPOINT_HERE";

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
