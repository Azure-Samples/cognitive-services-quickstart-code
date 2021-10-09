using System;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.SpellCheck;
using Microsoft.Azure.CognitiveServices.Language.SpellCheck.Models;

/*
 * This Bing Spell Check quickstart takes in some misspelled words and suggests corrections.
 * 
 * Prerequisites:
 * - Copy/paste this code into your Program.cs of a new Console App project in Visual Studio.
 * - Add your Bing Spell Check subscription key to your environment variables, using BING_SPELL_CHECK_SUBSCRIPTION_KEY as variable names.
 * - Install the NuGet package: Microsoft.Azure.CognitiveServices.Language.SpellCheck
 *   
 * References:
 * - Bing Spell Check documentation: https://docs.microsoft.com/en-us/azure/cognitive-services/bing-spell-check/index
 * - Dotnet SDK: https://docs.microsoft.com/en-us/dotnet/api/overview/azure/cognitiveservices/client/bingspellcheck?view=azure-dotnet
 * - API: https://docs.microsoft.com/en-us/rest/api/cognitiveservices-bingsearch/bing-spell-check-api-v7-reference
 */

namespace BingSpellCheckQuickstart
{
	class Program
	{
		static void Main(string[] args)
		{
			string query = "bill Gatas was ehre"; // Bill Gates was here

			// Authenticate
			string key = "PASTE_YOUR_SPELL_CHECK_SUBSCRIPTION_KEY_HERE";

			var client = new SpellCheckClient(
				new ApiKeyServiceClientCredentials(key));

			// Call API to check spelling of query
			checkSpelling(client, query).Wait();

		}

		public static async Task checkSpelling(SpellCheckClient client, string query)
		{
			var result = await client.SpellCheckerAsync(text: query, mode: "proof");

			Console.WriteLine("Original query: \n" + query);
			Console.WriteLine();
			Console.WriteLine("Misspelled words:");
			foreach (SpellingFlaggedToken token in result.FlaggedTokens)
			{
				Console.WriteLine(token.Token);
			}

			Console.WriteLine();
			Console.WriteLine("Suggested corrections:");
			foreach (SpellingFlaggedToken token in result.FlaggedTokens)
			{
				foreach(SpellingTokenSuggestion suggestion in token.Suggestions)
				{
					// Confidence values range from 0 (none) to 1.0 (full confidence)
					Console.WriteLine(suggestion.Suggestion + " with confidence " + Math.Round((decimal)suggestion.Score, 2));
				}
			}
		}
	}
}
