// <Dependencies>
using Azure;
using Azure.AI.Personalizer;
using System;
using System.Collections.Generic;
using System.Linq;
// </Dependencies>

namespace PersonalizerExampleSingleSlotSdkV2
{
    class Program
    {
        // <classVariables>
        // The key specific to your personalizer resource instance; e.g. "0123456789abcdef0123456789ABCDEF"
        private static readonly string ApiKey = "REPLACE-WITH-YOUR-PERSONALIZER-KEY";

        // The endpoint specific to your personalizer resource instance; e.g. https://<your-resource-name>.cognitiveservices.azure.com
        private static readonly string ServiceEndpoint = "https://REPLACE-WITH-YOUR-PERSONALIZER-RESOURCE-NAME.cognitiveservices.azure.com";
        // </classVariables>

        // <mainLoop>
        static void Main(string[] args)
        {
            int iteration = 1;
            bool runLoop = true;

            IList<PersonalizerRankableAction> actions = GetActions();

            PersonalizerClient client = InitializePersonalizerClient(new Uri(ServiceEndpoint));

            do
            {
                Console.WriteLine("\nIteration: " + iteration++);

                string timeOfDayFeature = GetUsersTimeOfDay();
                string tasteFeature = GetUsersTastePreference();

                IList<object> currentContext = new List<object>() {
                    new { time = timeOfDayFeature },
                    new { taste = tasteFeature }
                };

                IList<string> excludeActions = new List<string> { "juice" };

                string eventId = Guid.NewGuid().ToString();

                var rankOptions = new PersonalizerRankOptions(actions, currentContext, excludeActions, eventId);
                PersonalizerRankResult result = client.Rank(rankOptions);

                Console.WriteLine("\nPersonalizer service thinks you would like to have: " + result.RewardActionId + ". Is this correct? (y/n)");

                float reward = 0.0f;
                string answer = GetKey();

                if (answer == "Y")
                {
                    reward = 1f;
                    Console.WriteLine("\nGreat! Enjoy your food.");
                }
                else if (answer == "N")
                {
                    reward = 0f;
                    Console.WriteLine("\nYou didn't like the recommended food choice.");
                }
                else
                {
                    Console.WriteLine("\nEntered choice is invalid. Service assumes that you didn't like the recommended food choice.");
                }

                Console.WriteLine("\nPersonalizer service ranked the actions with the probabilities as below:");
                foreach (var rankedResponse in result.Ranking)
                {
                    Console.WriteLine(rankedResponse.Id + " " + rankedResponse.Probability);
                }

                client.Reward(result.EventId, reward);

                Console.WriteLine("\nPress q to break, any other key to continue:");
                runLoop = !(GetKey() == "Q");

            } while (runLoop);
        }
        // </mainLoop>

        // <authorization>
        /// <summary>
        /// Initializes the personalizer client.
        /// </summary>
        /// <param name="url">Azure endpoint</param>
        /// <returns>Personalizer client instance</returns>
        static PersonalizerClient InitializePersonalizerClient(Uri url)
        {
            return new PersonalizerClient(url, new AzureKeyCredential(ApiKey));
        }
        // </authorization>

        // <createAction>
        /// <summary>
        /// Creates personalizer actions feature list.
        /// </summary>
        /// <returns>List of actions for personalizer.</returns>
        static IList<PersonalizerRankableAction> GetActions()
        {
            IList<PersonalizerRankableAction> actions = new List<PersonalizerRankableAction>
            {
                new PersonalizerRankableAction(
                    id: "pasta",
                    features:
                    new List<object>() { new { taste = "salty", spiceLevel = "medium" }, new { nutritionLevel = 5, cuisine = "italian" } }
                ),

                new PersonalizerRankableAction(
                    id: "ice cream",
                    features:
                    new List<object>() { new { taste = "sweet", spiceLevel = "none" }, new { nutritionalLevel = 2 } }
                ),

                new PersonalizerRankableAction(
                    id: "juice",
                    features:
                    new List<object>() { new { taste = "sweet", spiceLevel = "none" }, new { nutritionLevel = 5 }, new { drink = true } }
                ),

                new PersonalizerRankableAction(
                    id: "salad",
                    features:
                    new List<object>() { new { taste = "salty", spiceLevel = "low" }, new { nutritionLevel = 8 } }
                )
            };

            return actions;
        }
        // </createAction>

        // <createUserFeatureTimeOfDay>
        /// <summary>
        /// Get users time of the day context.
        /// </summary>
        /// <returns>Time of day feature selected by the user.</returns>
        static string GetUsersTimeOfDay()
        {
            string[] timeOfDayFeatures = new string[] { "morning", "afternoon", "evening", "night" };

            Console.WriteLine("\nWhat time of day is it (enter number)? 1. morning 2. afternoon 3. evening 4. night");
            if (!int.TryParse(GetKey(), out int timeIndex) || timeIndex < 1 || timeIndex > timeOfDayFeatures.Length)
            {
                Console.WriteLine("\nEntered value is invalid. Setting feature value to " + timeOfDayFeatures[0] + ".");
                timeIndex = 1;
            }

            return timeOfDayFeatures[timeIndex - 1];
        }
        // </createUserFeatureTimeOfDay>

        // <createUserFeatureTastePreference>
        /// <summary>
        /// Gets user food preference.
        /// </summary>
        /// <returns>Food taste feature selected by the user.</returns>
        static string GetUsersTastePreference()
        {
            string[] tasteFeatures = new string[] { "salty", "sweet" };

            Console.WriteLine("\nWhat type of food would you prefer (enter number)? 1. salty 2. sweet");
            if (!int.TryParse(GetKey(), out int tasteIndex) || tasteIndex < 1 || tasteIndex > tasteFeatures.Length)
            {
                Console.WriteLine("\nEntered value is invalid. Setting feature value to " + tasteFeatures[0] + ".");
                tasteIndex = 1;
            }

            return tasteFeatures[tasteIndex - 1];
        }
        // </createUserFeatureTastePreference>

        // <readCommandLine>
        private static string GetKey()
        {
            return Console.ReadKey().Key.ToString().Last().ToString().ToUpper();
        }
        // </readCommandLine>
    }
}
