// Note: Add the NuGet package Microsoft.Azure.CognitiveServices.Language.LUIS.Authoring to your solution.
// <Dependencies>
using Microsoft.Azure.CognitiveServices.Language.LUIS.Authoring;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Authoring.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
// </Dependencies>


/*
 * This sample builds a LUIS application, entities, and intents using the LUIS .NET SDK.
 * A separate sample trains and publishes the application.
 *
 * Be sure you understand how LUIS models work.  In particular, know what
 * intents, entities, and utterances are, and how they work together in the
 * context of a LUIS app. See the following:
 *
 * https://www.luis.ai/welcome
 * https://docs.microsoft.com/azure/cognitive-services/luis/luis-concept-intent
 * https://docs.microsoft.com/azure/cognitive-services/luis/luis-concept-entity-types
 * https://docs.microsoft.com/azure/cognitive-services/luis/luis-concept-utterance
 */

namespace LUIS_CS
{
    // <ApplicationInfo>
    struct ApplicationInfo
    {
        public Guid ID;
        public string Version;
    }
    // </ApplicationInfo>

    class Program
    {
        // <Variables>
        private static readonly string authoring_key = "PASTE_YOUR_LUIS_AUTHORING_SUBSCRIPTION_KEY_HERE";
        
        private static readonly string authoring_endpoint = "PASTE_YOUR_LUIS_AUTHORING_ENDPOINT_HERE";
        // </Variables>

        static Program()
        {
            if (null == authoring_key)
            {
                throw new Exception("Please set variable: " + authoring_key);
            }
            if (null == authoring_endpoint)
            {
                throw new Exception("Please set variable: " + authoring_endpoint);
            }
        }

        // <AuthoringCreateApplication>
        // Return the application ID and version.
        async static Task<ApplicationInfo> CreateApplication(LUISAuthoringClient client)
        {
            string app_name =           String.Format("Contoso {0}", DateTime.Now);
            string app_description =    "Flight booking app built with LUIS .NET SDK.";
            string app_version =        "0.1";
            string app_culture =        "en-us";

            var app_info = new ApplicationCreateObject()
            {
                Name = app_name,
                InitialVersionId = app_version,
                Description = app_description,
                Culture = app_culture
            };
            var app_id = await client.Apps.AddAsync(app_info);
            Console.WriteLine("Created new LUIS application {0}\n with ID {1}.", app_info.Name, app_id);
            return new ApplicationInfo() { ID = app_id, Version = app_version };
        }
        // </AuthoringCreateApplication>

        // <AuthoringAddEntities>
        // Create entity objects
        async static Task AddEntities(LUISAuthoringClient client, ApplicationInfo app_info)
        {
            // Add simple entity
            var simpleEntityIdLocation = await client.Model.AddEntityAsync(app_info.ID, app_info.Version, new ModelCreateObject()
            {
                Name = "Location"
            });

            // Add 'Origin' role to simple entity
            await client.Model.CreateEntityRoleAsync(app_info.ID, app_info.Version, simpleEntityIdLocation, new EntityRoleCreateObject()
            {
                Name = "Origin"
            });

            // Add 'Destination' role to simple entity
            await client.Model.CreateEntityRoleAsync(app_info.ID, app_info.Version, simpleEntityIdLocation, new EntityRoleCreateObject()
            {
                Name = "Destination"
            });

            // Add simple entity
            var simpleEntityIdClass = await client.Model.AddEntityAsync(app_info.ID, app_info.Version, new ModelCreateObject()
            {
                Name = "Class"
            });


            // Add prebuilt number and datetime
            await client.Model.AddPrebuiltAsync(app_info.ID, app_info.Version, new List<string>
            {
                "number",
                "datetimeV2",
                "geographyV2",
                "ordinal"
            });

            // Composite entity
            await client.Model.AddCompositeEntityAsync(app_info.ID, app_info.Version, new CompositeEntityModel()
            {
                Name = "Flight",
                Children = new List<string>() { "Location", "Class", "number", "datetimeV2", "geographyV2", "ordinal" }
            });
            Console.WriteLine("Created entities Location, Class, number, datetimeV2, geographyV2, ordinal.");
        }
        // </AuthoringAddEntities>

        // <AuthoringAddIntents>
        async static Task AddIntents(LUISAuthoringClient client, ApplicationInfo app_info)
        {
            await client.Model.AddIntentAsync(app_info.ID, app_info.Version, new ModelCreateObject()
            {
                Name = "FindFlights"
            });
            Console.WriteLine("Created intent FindFlights");
        }
        // </AuthoringAddIntents>

        // <AuthoringBatchAddUtterancesForIntent>
        async static Task AddUtterances(LUISAuthoringClient client, ApplicationInfo app_info)
        {
            var utterances = new List<ExampleLabelObject>()
            {
                CreateUtterance ("FindFlights", "find flights in economy to Madrid on July 1st", new Dictionary<string, string>() { {"Flight", "economy to Madrid"}, { "Location", "Madrid" }, { "Class", "economy" } }),
                CreateUtterance ("FindFlights", "find flights from seattle to London in first class", new Dictionary<string, string>() { { "Flight", "London in first class" }, { "Location", "London" }, { "Class", "first" } }),
                CreateUtterance ("FindFlights", "find flights to London in first class", new Dictionary<string, string>()  { { "Flight", "London in first class" }, { "Location", "London" }, { "Class", "first" } }),

                //Role not supported in SDK yet
                //CreateUtterance ("FindFlights", "find flights to Paris in first class", new Dictionary<string, string>()  { { "Flight", "London in first class" }, { "Location::Destination", "Paris" }, { "Class", "first" } })
            };
            var resultsList = await client.Examples.BatchAsync(app_info.ID, app_info.Version, utterances);

            foreach (var x in resultsList)
            {
                var result = (!x.HasError.GetValueOrDefault()) ? "succeeded": "failed";
                Console.WriteLine("{0} {1}", x.Value.ExampleId, result);
            }
        }
        // Create utterance with marked text for entities
        static ExampleLabelObject CreateUtterance(string intent, string utterance, Dictionary<string, string> labels)
        {
            var entity_labels = labels.Select(kv => CreateLabel(utterance, kv.Key, kv.Value)).ToList();
            return new ExampleLabelObject()
            {
                IntentName = intent,
                Text = utterance,
                EntityLabels = entity_labels
            };
        }
        // Mark beginning and ending of entity text in utterance
        static EntityLabelObject CreateLabel(string utterance, string key, string value)
        {
            var start_index = utterance.IndexOf(value, StringComparison.InvariantCultureIgnoreCase);
            return new EntityLabelObject()
            {
                EntityName = key,
                StartCharIndex = start_index,
                EndCharIndex = start_index + value.Length
            };
        }
        // </AuthoringBatchAddUtterancesForIntent>


        // <AuthoringTrainVersion>
        async static Task Train_App(LUISAuthoringClient client, ApplicationInfo app)
        {
            var response = await client.Train.TrainVersionAsync(app.ID, app.Version);
            Console.WriteLine("Training status: " + response.Status);
        }
        // </AuthoringTrainVersion>


        // <AuthoringPublishVersionAndSlot>
        // Publish app, display endpoint URL for the published application.
        async static Task Publish_App(LUISAuthoringClient client, ApplicationInfo app)
        {
            ApplicationPublishObject obj = new ApplicationPublishObject
            {
                VersionId = app.Version,
                IsStaging = true
            };
            var info = await client.Apps.PublishAsync(app.ID, obj);
            Console.WriteLine("Endpoint URL: " + info.EndpointUrl);
        }
        // </AuthoringPublishVersionAndSlot>



        async static Task RunQuickstart()
        {
            // <AuthoringCreateClient>
            // Generate the credentials and create the client.
            var credentials = new Microsoft.Azure.CognitiveServices.Language.LUIS.Authoring.ApiKeyServiceClientCredentials(authoring_key);
            var client = new LUISAuthoringClient(credentials, new System.Net.Http.DelegatingHandler[] { })
            {
                Endpoint = authoring_endpoint
            };
            // </AuthoringCreateClient>


            Console.WriteLine("Creating application...");
            var app = await CreateApplication(client);
            Console.WriteLine();

            Console.WriteLine("Adding entities to application...");
            await AddEntities(client, app);
            Console.WriteLine();

            Console.WriteLine("Adding intents to application...");
            await AddIntents(client, app);
            Console.WriteLine();

            Console.WriteLine("Adding utterances to application...");
            await AddUtterances(client, app);
            Console.WriteLine();

            Console.WriteLine("Training application...");
            await Train_App(client, app);
            Console.WriteLine("Waiting 30 seconds for training to complete...");
            System.Threading.Thread.Sleep(30000);
            Console.WriteLine();

            Console.WriteLine("Publishing application...");
            await Publish_App(client, app);
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            Task.WaitAll(RunQuickstart());
            Console.WriteLine("Press any key to exit.");
            Console.Read();
        }
    }
}