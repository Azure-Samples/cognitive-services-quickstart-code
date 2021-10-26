using System;
using Azure;
using Azure.AI.Personalizer;
using System.Collections.Generic;
using System.Linq;


namespace MultiSlotQuickstartMultiSlotSdkV2
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
            Console.WriteLine($"Welcome to this Personalizer Quickstart!\n" +
            $"This code will help you understand how to use the Personalizer APIs (multislot rank and multislot reward).\n" +
            $"Each iteration represents a user interaction and will demonstrate how context, actions, slots, and rewards work.\n" +
            $"Note: Personalizer AI models learn from a large number of user interactions:\n" +
            $"You won't be able to tell the difference in what Personalizer returns by simulating a few events by hand.\n" +
            $"If you want a sample that focuses on seeing how Personalizer learns, see the Python Notebook sample.");

            int iteration = 1;
            bool runLoop = true;

            IList<PersonalizerRankableAction> actions = GetActions();
            IList<PersonalizerSlotOptions> slots = GetSlots();
            PersonalizerClient client = InitializePersonalizerClient(new Uri(ServiceEndpoint));

            do
            {
                Console.WriteLine("\nIteration: " + iteration++);

                string timeOfDayFeature = GetTimeOfDayForContext();
                string deviceFeature = GetDeviceForContext();

                IList<object> currentContext = GetContext(timeOfDayFeature, deviceFeature);

                string eventId = Guid.NewGuid().ToString();

                var multiSlotRankOptions = new PersonalizerRankMultiSlotOptions(actions, slots, currentContext, eventId);
                PersonalizerMultiSlotRankResult multiSlotRankResult = client.RankMultiSlot(multiSlotRankOptions);

                for (int i = 0; i < multiSlotRankResult.Slots.Count(); ++i)
                {
                    string slotId = multiSlotRankResult.Slots[i].SlotId;
                    Console.WriteLine($"\nPersonalizer service decided you should display: { multiSlotRankResult.Slots[i].RewardActionId} in slot {slotId}. Is this correct? (y/n)");

                    string answer = GetKey();

                    if (answer == "Y")
                    {
                        client.RewardMultiSlot(eventId, slotId, 1f);
                        Console.WriteLine("\nGreat! The application will send Personalizer a reward of 1 so it learns from this choice of action for this slot.");
                    }
                    else if (answer == "N")
                    {
                        client.RewardMultiSlot(eventId, slotId, 0f);
                        Console.WriteLine("\nYou didn't like the recommended item. The application will send Personalizer a reward of 0 for this choice of action for this slot.");
                    }
                    else
                    {
                        client.RewardMultiSlot(eventId, slotId, 0f);
                        Console.WriteLine("\nEntered choice is invalid. Service assumes that you didn't like the recommended item.");
                    }
                }

                Console.WriteLine("\nPress q to break, any other key to continue:");
                runLoop = !(GetKey() == "Q");

            } while (runLoop);
        }

        static PersonalizerClient InitializePersonalizerClient(Uri url)
        {
            return new PersonalizerClient(url, new AzureKeyCredential(ApiKey));
        }


        private static IList<PersonalizerRankableAction> GetActions()
        {
            IList<PersonalizerRankableAction> actions = new List<PersonalizerRankableAction>
            {
                new PersonalizerRankableAction(
                    id: "Red-Polo-Shirt-432",
                    features:
                    new List<object>() { new { onSale = "true", price = "20", category = "Clothing" } }
                ),

                new PersonalizerRankableAction(
                    id: "Tennis-Racket-133",
                    features:
                    new List<object>() { new { onSale = "false", price = "70", category = "Sports" } }
                ),

                new PersonalizerRankableAction(
                    id: "31-Inch-Monitor-771",
                    features:
                    new List<object>() { new { onSale = "true", price = "200", category = "Electronics" } }
                ),

                new PersonalizerRankableAction(
                    id: "XBox-Series X-117",
                    features:
                    new List<object>() { new { onSale = "false", price = "499", category = "Electronics" } }
                )
            };

            return actions;
        }

        private static IList<PersonalizerSlotOptions> GetSlots()
        {
            IList<PersonalizerSlotOptions> slots = new List<PersonalizerSlotOptions>
            {
                new PersonalizerSlotOptions(
                    id: "BigHeroPosition",
                    features: new List<object>() { new { size = "large", position = "left" } },
                    excludedActions: new List<string>() { "31-Inch-Monitor-771" },
                    baselineAction: "Red-Polo-Shirt-432"

                ),

                new PersonalizerSlotOptions(
                    id: "SmallSidebar",
                    features: new List<object>() { new { size = "small", position = "right" } },
                    excludedActions: new List<string>() { "Tennis-Racket-133" },
                    baselineAction: "XBox-Series X-117"
                ),
            };

            return slots;
        }

        static string GetTimeOfDayForContext()
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

        static string GetDeviceForContext()
        {
            string[] deviceFeatures = new string[] { "mobile", "tablet", "desktop" };

            Console.WriteLine("\nWhat is the device type (enter number)? 1. Mobile 2. Tablet 3. Desktop");
            if (!int.TryParse(GetKey(), out int deviceIndex) || deviceIndex < 1 || deviceIndex > deviceFeatures.Length)
            {
                Console.WriteLine("\nEntered value is invalid. Setting feature value to " + deviceFeatures[0] + ".");
                deviceIndex = 1;
            }

            return deviceFeatures[deviceIndex - 1];
        }

        private static IList<object> GetContext(string time, string device)
        {
            return new List<object>()
            {
                new { time = time },
                new { device = device }
            };
        }

        private static string GetKey()
        {
            return Console.ReadKey().Key.ToString().Last().ToString().ToUpper();
        }
    }
}
