using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MultiSlotQuickstart
{
    class MultiSlotQuickstart
    {
        //Replace 'PersonalizationBaseUrl' and 'ResourceKey' with your valid endpoint values.
        private const string PersonalizationBaseUrl = "PASTE_YOUR_PERSONALIZER_ENDPOINT_HERE";
        private const string ResourceKey = "PASTE_YOUR_PERSONALIZER_SUBSCRIPTION_KEY_HERE";
        private static string MultiSlotRankUrl = string.Concat(PersonalizationBaseUrl, "personalizer/v1.1-preview.1/multislot/rank");
        private static string MultiSlotRewardUrlBase = string.Concat(PersonalizationBaseUrl, "personalizer/v1.1-preview.1/multislot/events/");

        static async Task Main(string[] args)
        {
            Console.WriteLine($"Welcome to this Personalizer Quickstart!\n" +
                    $"This code will help you understand how to use the Personalizer APIs (multislot rank and multislot reward).\n" +
                    $"Each iteration represents a user interaction and will demonstrate how context, actions, slots, and rewards work.\n" +
                    $"Note: Personalizer AI models learn from a large number of user interactions:\n" +
                    $"You won't be able to tell the difference in what Personalizer returns by simulating a few events by hand.\n" +
                    $"If you want a sample that focuses on seeing how Personalizer learns, see the Python Notebook sample.");

            IList<Action> actions = GetActions();
            IList<Slot> slots = GetSlots();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("ocp-apim-subscription-key", ResourceKey);
                int iteration = 1;
                bool runLoop = true;
                do
                {
                    Console.WriteLine($"\nIteration: {iteration++}");
                    string timeOfDayFeature = GetTimeOfDayForContext();
                    string deviceFeature = GetDeviceForContext();

                    IList<Context> context = GetContext(timeOfDayFeature, deviceFeature);

                    string eventId = Guid.NewGuid().ToString();

                    string rankRequestBody = JsonSerializer.Serialize(new MultiSlotRankRequest()
                    {
                        ContextFeatures = context,
                        Actions = actions,
                        Slots = slots,
                        EventId = eventId,
                        DeferActivation = false
                    });

                    //Ask Personalizer what action to show for each slot
                    MultiSlotRankResponse multiSlotRankResponse = await SendMultiSlotRank(client, rankRequestBody, MultiSlotRankUrl);

                    MultiSlotReward multiSlotRewards = new MultiSlotReward()
                    {
                        Reward = new List<SlotReward>()
                    };

                    for (int i = 0; i < multiSlotRankResponse.Slots.Count(); ++i)
                    {
                        Console.WriteLine($"\nPersonalizer service decided you should display: { multiSlotRankResponse.Slots[i].RewardActionId} in slot {multiSlotRankResponse.Slots[i].Id}. Is this correct? (y/n)");
                        SlotReward reward = new SlotReward()
                        {
                            SlotId = multiSlotRankResponse.Slots[i].Id
                        };

                        string answer = GetKey();

                        if (answer == "Y")
                        {
                            reward.Value = 1;
                            Console.WriteLine("\nGreat! The application will send Personalizer a reward of 1 so it learns from this choice of action for this slot.");
                        }
                        else if (answer == "N")
                        {
                            reward.Value = 0;
                            Console.WriteLine("\nYou didn't like the recommended item. The application will send Personalizer a reward of 0 for this choice of action for this slot.");
                        }
                        else
                        {
                            reward.Value = 0;
                            Console.WriteLine("\nEntered choice is invalid. Service assumes that you didn't like the recommended item.");
                        }
                        multiSlotRewards.Reward.Add(reward);
                    }

                    string rewardRequestBody = JsonSerializer.Serialize(multiSlotRewards);

                    // Send the reward for the action based on user response for each slot.
                    await SendMultiSlotReward(client, rewardRequestBody, MultiSlotRewardUrlBase, multiSlotRankResponse.EventId);

                    Console.WriteLine("\nPress q to break, any other key to continue:");
                    runLoop = !(GetKey() == "Q");
                } while (runLoop);
            }
        }

        private static IList<Action> GetActions()
        {
            IList<Action> actions = new List<Action>
            {
                new Action
                {
                    Id = "Red-Polo-Shirt-432",
                    Features =
                    new List<object>() { new { onSale = "true", price = "20", category = "Clothing" } }
                },
                new Action
                {
                    Id = "Tennis-Racket-133",
                    Features =
                    new List<object>() { new { onSale = "false", price = "70", category = "Sports" } }
                },
                new Action
                {
                    Id = "31-Inch-Monitor-771",
                    Features =
                    new List<object>() { new { onSale = "true", price = "200", category = "Electronics" } }
                },
                new Action
                {
                    Id = "XBox-Series X-117",
                    Features =
                    new List<object>() { new { onSale = "false", price = "499", category = "Electronics" } }
                }
            };
            return actions;
        }

        private static IList<Slot> GetSlots()
        {
            IList<Slot> slots = new List<Slot>
            {
                new Slot
                {
                    Id = "BigHeroPosition",
                    Features = new List<object>() { new { size = "large", position = "left" } },
                    ExcludedActions = new List<string>() { "31-Inch-Monitor-771" },
                    BaselineAction = "Red-Polo-Shirt-432"

                },
                new Slot
                {
                    Id = "SmallSidebar",
                    Features = new List<object>() { new { size = "small", position = "right" } },
                    ExcludedActions = new List<string>() { "Tennis-Racket-133" },
                    BaselineAction = "XBox-Series X-117"
                },
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

        private static string GetKey()
        {
            return Console.ReadKey().Key.ToString().Last().ToString().ToUpper();
        }

        private static IList<Context> GetContext(string time, string device)
        {
            IList<Context> context = new List<Context>
            {
                new Context
                {
                    Features = new {timeOfDay = time, device = device }
                }
            };

            return context;
        }

        private static async Task<MultiSlotRankResponse> SendMultiSlotRank(HttpClient client, string rankRequestBody, string rankUrl)
        {
            try
            {
                var rankBuilder = new UriBuilder(new Uri(rankUrl));
                HttpRequestMessage rankRequest = new HttpRequestMessage(HttpMethod.Post, rankBuilder.Uri);
                rankRequest.Content = new StringContent(rankRequestBody, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.SendAsync(rankRequest);
                response.EnsureSuccessStatusCode();
                MultiSlotRankResponse rankResponse = JsonSerializer.Deserialize<MultiSlotRankResponse>(await response.Content.ReadAsByteArrayAsync());
                return rankResponse;
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
                Console.WriteLine("Please make sure multi-slot feature is enabled. To do so, follow multi-slot Personalizer documentation to update your loop settings to enable multi-slot functionality.");
                throw;
            }
        }

        private static async Task SendMultiSlotReward(HttpClient client, string rewardRequestBody, string rewardUrlBase, string eventId)
        {
            string rewardUrl = String.Concat(rewardUrlBase, eventId, "/reward");
            var rewardBuilder = new UriBuilder(new Uri(rewardUrl));
            HttpRequestMessage rewardRequest = new HttpRequestMessage(HttpMethod.Post, rewardBuilder.Uri);
            rewardRequest.Content = new StringContent(rewardRequestBody, Encoding.UTF8, "application/json");

            await client.SendAsync(rewardRequest);
        }

        private class Action
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("features")]
            public object Features { get; set; }
        }

        private class Slot
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("features")]
            public List<object> Features { get; set; }

            [JsonPropertyName("excludedActions")]
            public List<string> ExcludedActions { get; set; }

            [JsonPropertyName("baselineAction")]
            public string BaselineAction { get; set; }
        }

        private class Context
        {
            [JsonPropertyName("features")]
            public object Features { get; set; }
        }

        private class MultiSlotRankRequest
        {
            [JsonPropertyName("contextFeatures")]
            public IList<Context> ContextFeatures { get; set; }

            [JsonPropertyName("actions")]
            public IList<Action> Actions { get; set; }

            [JsonPropertyName("slots")]
            public IList<Slot> Slots { get; set; }

            [JsonPropertyName("eventId")]
            public string EventId { get; set; }

            [JsonPropertyName("deferActivation")]
            public bool DeferActivation { get; set; }
        }

        private class MultiSlotRankResponse
        {
            [JsonPropertyName("slots")]
            public IList<SlotResponse> Slots { get; set; }

            [JsonPropertyName("eventId")]
            public string EventId { get; set; }
        }

        private class SlotResponse
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("rewardActionId")]
            public string RewardActionId { get; set; }
        }

        private class MultiSlotReward
        {
            [JsonPropertyName("reward")]
            public List<SlotReward> Reward { get; set; }
        }

        private class SlotReward
        {
            [JsonPropertyName("slotId")]
            public string SlotId { get; set; }

            [JsonPropertyName("value")]
            public float Value { get; set; }
        }
    }
}
