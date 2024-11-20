// <snippet_1>
using Microsoft.Azure.CognitiveServices.Personalizer;
using Microsoft.Azure.CognitiveServices.Personalizer.Models;

class Program
{
    private static readonly string ApiKey = "REPLACE_WITH_YOUR_PERSONALIZER_KEY";
    private static readonly string ServiceEndpoint = "REPLACE_WITH_YOUR_ENDPOINT_URL";

    static PersonalizerClient InitializePersonalizerClient(string url)
    {
        PersonalizerClient client = new PersonalizerClient(
            new ApiKeyServiceClientCredentials(ApiKey))
        { Endpoint = url };

        return client;
    }

    static Dictionary<string, ActionFeatures> actions = new Dictionary<string, ActionFeatures>
    {
    {"pasta", new ActionFeatures(
                    new BrandInfo(company: "pasta_inc"),
                    new ItemAttributes(
                        quantity: 1,
                        category: "Italian",
                        price: 12),
                    new DietaryAttributes(
                        vegan: false,
                        lowCarb: false,
                        highProtein: false,
                        vegetarian: false,
                        lowFat: true,
                        lowSodium: true))},
    {"bbq", new ActionFeatures(
                    new BrandInfo(company: "ambisco"),
                    new ItemAttributes(
                        quantity: 2,
                        category: "bbq",
                        price: 20),
                    new DietaryAttributes(
                        vegan: false,
                        lowCarb: true,
                        highProtein: true,
                        vegetarian: false,
                        lowFat: false,
                        lowSodium: false))},
    {"bao", new ActionFeatures(
                    new BrandInfo(company: "bao_and_co"),
                    new ItemAttributes(
                        quantity: 4,
                        category: "Chinese",
                        price: 8),
                    new DietaryAttributes(
                        vegan: false,
                        lowCarb: true,
                        highProtein: true,
                        vegetarian: false,
                        lowFat: true,
                        lowSodium: false))},
    {"hummus", new ActionFeatures(
                    new BrandInfo(company: "garbanzo_inc"),
                    new ItemAttributes(
                        quantity: 1,
                        category: "Breakfast",
                        price: 5),
                    new DietaryAttributes(
                        vegan: true,
                        lowCarb: false,
                        highProtein: true,
                        vegetarian: true,
                        lowFat: false,
                        lowSodium: false))},
    {"veg_platter", new ActionFeatures(
                    new BrandInfo(company: "farm_fresh"),
                    new ItemAttributes(
                        quantity: 1,
                        category: "produce",
                        price: 7),
                    new DietaryAttributes(
                        vegan: true,
                        lowCarb: true,
                        highProtein: false,
                        vegetarian: true,
                        lowFat: true,
                        lowSodium: true ))},
};

    static IList<RankableAction> GetActions()
    {
        IList<RankableAction> rankableActions = new List<RankableAction>();
        foreach (var action in actions)
        {
            rankableActions.Add(new RankableAction
            {
                Id = action.Key,
                Features = new List<object>() { action.Value }
            });
        }

        return rankableActions;
    }

    public class BrandInfo
    {
        public string Company { get; set; }
        public BrandInfo(string company)
        {
            Company = company;
        }
    }

    public class ItemAttributes
    {
        public int Quantity { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public ItemAttributes(int quantity, string category, double price)
        {
            Quantity = quantity;
            Category = category;
            Price = price;
        }
    }

    public class DietaryAttributes
    {
        public bool Vegan { get; set; }
        public bool LowCarb { get; set; }
        public bool HighProtein { get; set; }
        public bool Vegetarian { get; set; }
        public bool LowFat { get; set; }
        public bool LowSodium { get; set; }
        public DietaryAttributes(bool vegan, bool lowCarb, bool highProtein, bool vegetarian, bool lowFat, bool lowSodium)
        {
            Vegan = vegan;
            LowCarb = lowCarb;
            HighProtein = highProtein;
            Vegetarian = vegetarian;
            LowFat = lowFat;
            LowSodium = lowSodium;

        }
    }

    public class ActionFeatures
    {
        public BrandInfo BrandInfo { get; set; }
        public ItemAttributes ItemAttributes { get; set; }
        public DietaryAttributes DietaryAttributes { get; set; }
        public ActionFeatures(BrandInfo brandInfo, ItemAttributes itemAttributes, DietaryAttributes dietaryAttributes)
        {
            BrandInfo = brandInfo;
            ItemAttributes = itemAttributes;
            DietaryAttributes = dietaryAttributes;
        }
    }

    public static Context GetContext()
    {
        return new Context(
                user: GetRandomUser(),
                timeOfDay: GetRandomTimeOfDay(),
                location: GetRandomLocation(),
                appType: GetRandomAppType());
    }

    static string[] timesOfDay = new string[] { "morning", "afternoon", "evening" };

    static string[] locations = new string[] { "west", "east", "midwest" };

    static string[] appTypes = new string[] { "edge", "safari", "edge_mobile", "mobile_app" };

    static IList<UserProfile> users = new List<UserProfile>
{
    new UserProfile(
        name: "Bill",
        dietaryPreferences: new Dictionary<string, bool> { { "low_carb", true } },
        avgOrderPrice: "0-20"),
    new UserProfile(
        name: "Satya",
        dietaryPreferences: new Dictionary<string, bool> { { "low_sodium", true} },
        avgOrderPrice: "201+"),
    new UserProfile(
        name: "Amy",
        dietaryPreferences: new Dictionary<string, bool> { { "vegan", true }, { "vegetarian", true } },
        avgOrderPrice: "21-50")
};

    static string GetRandomTimeOfDay()
    {
        var random = new Random();
        var timeOfDayIndex = random.Next(timesOfDay.Length);
        Console.WriteLine($"TimeOfDay: {timesOfDay[timeOfDayIndex]}");
        return timesOfDay[timeOfDayIndex];
    }

    static string GetRandomLocation()
    {
        var random = new Random();
        var locationIndex = random.Next(locations.Length);
        Console.WriteLine($"Location: {locations[locationIndex]}");
        return locations[locationIndex];
    }

    static string GetRandomAppType()
    {
        var random = new Random();
        var appIndex = random.Next(appTypes.Length);
        Console.WriteLine($"AppType: {appTypes[appIndex]}");
        return appTypes[appIndex];
    }

    static UserProfile GetRandomUser()
    {
        var random = new Random();
        var userIndex = random.Next(users.Count);
        Console.WriteLine($"\nUser: {users[userIndex].Name}");
        return users[userIndex];
    }

    public class UserProfile
    {
        // Mark name as non serializable so that it is not part of the context features
        [NonSerialized()]
        public string Name;
        public Dictionary<string, bool> DietaryPreferences { get; set; }
        public string AvgOrderPrice { get; set; }

        public UserProfile(string name, Dictionary<string, bool> dietaryPreferences, string avgOrderPrice)
        {
            Name = name;
            DietaryPreferences = dietaryPreferences;
            AvgOrderPrice = avgOrderPrice;
        }
    }

    public class Context
    {
        public UserProfile User { get; set; }
        public string TimeOfDay { get; set; }
        public string Location { get; set; }
        public string AppType { get; set; }

        public Context(UserProfile user, string timeOfDay, string location, string appType)
        {
            User = user;
            TimeOfDay = timeOfDay;
            Location = location;
            AppType = appType;
        }
    }
    public static float GetRewardScore(Context context, string actionId)
    {
        float rewardScore = 0.0f;
        string userName = context.User.Name;
        ActionFeatures actionFeatures = actions[actionId];
        if (userName.Equals("Bill"))
        {
            if (actionFeatures.ItemAttributes.Price < 10 && !context.Location.Equals("midwest"))
            {
                rewardScore = 1.0f;
                Console.WriteLine($"\nBill likes to be economical when he's not in the midwest visiting his friend Warren. He bought {actionId} because it was below a price of $10.");
            }
            else if (actionFeatures.DietaryAttributes.LowCarb && context.Location.Equals("midwest"))
            {
                rewardScore = 1.0f;
                Console.WriteLine($"\nBill is visiting his friend Warren in the midwest. There he's willing to spend more on food as long as it's low carb, so Bill bought {actionId}.");
            }
            else if (actionFeatures.ItemAttributes.Price >= 10 && !context.Location.Equals("midwest"))
            {
                Console.WriteLine($"\nBill didn't buy {actionId} because the price was too high when not visting his friend Warren in the midwest.");
            }
            else if (!actionFeatures.DietaryAttributes.LowCarb && context.Location.Equals("midwest"))
            {
                Console.WriteLine($"\nBill didn't buy {actionId} because it's not low-carb, and he's in the midwest visitng his friend Warren.");
            }
        }
        else if (userName.Equals("Satya"))
        {
            if (actionFeatures.DietaryAttributes.LowSodium)
            {
                rewardScore = 1.0f;
                Console.WriteLine($"\nSatya is health conscious, so he bought {actionId} since it's low in sodium.");
            }
            else
            {
                Console.WriteLine($"\nSatya did not buy {actionId} because it's not low sodium.");
            }
        }
        else if (userName.Equals("Amy"))
        {
            if (actionFeatures.DietaryAttributes.Vegan || actionFeatures.DietaryAttributes.Vegetarian)
            {
                rewardScore = 1.0f;
                Console.WriteLine($"\nAmy likes to eat plant-based foods, so she bought {actionId} because it's vegan or vegetarian friendly.");
            }
            else
            {
                Console.WriteLine($"\nAmy did not buy {actionId} because it's not vegan or vegetarian.");
            }
        }
        return rewardScore;
    }
    // ...
    // </snippet_1>

    //*********************

// <snippet_2>
    static void Main(string[] args)
    {
        int iteration = 1;
        bool runLoop = true;

        // Get the actions list to choose from personalizer with their features.
        IList<RankableAction> actions = GetActions();

        // Initialize Personalizer client.
        PersonalizerClient client = InitializePersonalizerClient(ServiceEndpoint);

        do
        {
            Console.WriteLine("\nIteration: " + iteration++);

            // <rank>
            // Get context information.
            Context context = GetContext();

            // Create current context from user specified data.
            IList<object> currentContext = new List<object>() {
            context
        };

            // Generate an ID to associate with the request.
            string eventId = Guid.NewGuid().ToString();

            // Rank the actions
            var request = new RankRequest(actions: actions, contextFeatures: currentContext, eventId: eventId);
            RankResponse response = client.Rank(request);
            // </rank>

            Console.WriteLine($"\nPersonalizer service thinks {context.User.Name} would like to have: {response.RewardActionId}.");

            // <reward>
            float reward = GetRewardScore(context, response.RewardActionId);

            // Send the reward for the action based on user response.
            client.Reward(response.EventId, new RewardRequest(reward));
            // </reward>

            Console.WriteLine("\nPress q to break, any other key to continue:");
            runLoop = !(GetKey() == "Q");

        } while (runLoop);
    }

        private static string GetKey()
    {
        return Console.ReadKey().Key.ToString().Last().ToString().ToUpper();
    }

}
// </snippet_2>


// <snippet_multi>
    static void Main(string[] args)
    {
    int iteration = 1;
    int runLoop = 0;

    // Get the actions list to choose from personalizer with their features.
    IList<RankableAction> actions = GetActions();

    // Initialize Personalizer client.
    PersonalizerClient client = InitializePersonalizerClient(ServiceEndpoint);

    do
    {
        Console.WriteLine("\nIteration: " + iteration++);

        // <rank>
        // Get context information.
        Context context = GetContext();

        // Create current context from user specified data.
        IList<object> currentContext = new List<object>() {
            context
        };

        // Generate an ID to associate with the request.
        string eventId = Guid.NewGuid().ToString();

        // Rank the actions
        var request = new RankRequest(actions: actions, contextFeatures: currentContext, eventId: eventId);
        RankResponse response = client.Rank(request);
        // </rank>

        Console.WriteLine($"\nPersonalizer service thinks {context.User.Name} would like to have: {response.RewardActionId}.");

        // <reward>
        float reward = GetRewardScore(context, response.RewardActionId);

        // Send the reward for the action based on user response.
        client.Reward(response.EventId, new RewardRequest(reward));
        // </reward>

        runLoop = runLoop + 1;

    } while (runLoop < 1000);
}
// </snippet_multi>
