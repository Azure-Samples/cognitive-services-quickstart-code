package main

import (
	"context"
	"encoding/json"
	"fmt"
	"github.com/Azure/azure-sdk-for-go/services/cognitiveservices/v2.0/luis/authoring"
	"github.com/Azure/go-autorest/autorest"
	"github.com/satori/go.uuid"
	"log"
	"os"
	"strings"
	"time"
)

/* This sample for the Azure Cognitive Services QnA Maker API shows how to:
 * - Create an application.
 * - Add intents to an application.
 * - Add entities to an application.
 * - Add utterances to an application.
 * - Train an application.
 * - Publish an application.
 * - Delete an application.
 * - List all applications.
 */
 
/*  Configure the local environment:
	* Set the LUIS_SUBSCRIPTION_KEY and LUIS_REGION environment variables 
	* on your local machine using the appropriate method for your preferred shell 
	* (Bash, PowerShell, Command Prompt, etc.). 
	*
	* For LUIS_REGION, use the same region you used to get your subscription keys. 
	*
	* If the environment variable is created after the application is launched in a console or with Visual
	* Studio, the shell (or Visual Studio) needs to be closed and reloaded to take the environment variable into account.
	*/
var subscription_key string = os.Getenv("LUIS_SUBSCRIPTION_KEY")
var region string = os.Getenv("LUIS_REGION")

// Replace this with the endpoint for your subscription key.
var endpoint string = "https://" + region + ".api.cognitive.microsoft.com"
//  END - Configure the local environment.

func create_app() (string, string) {
	// Create a new LUIS app
	// Get the context, which is required by the SDK methods.
	ctx := context.Background()

	client := authoring.NewAppsClient(endpoint)
	// Set the subscription key on the client.
	client.Authorizer = autorest.NewCognitiveServicesAuthorizer(subscription_key)

	name := "Contoso " + time.Now().Format("2006-01-02 15:04:05")
	description := "Flight booking app built with Azure SDK for Go."
	version := "0.1"
	locale  := "en-us"

	create_app_payload := authoring.ApplicationCreateObject { Name: &name, Description: &description, InitialVersionID: &version, Culture: &locale }

	result, err := client.Add(ctx, create_app_payload)
	if err != nil {
		log.Fatal(err)
	}

	fmt.Printf("Created LUIS app %s with ID %s\n", name, (*result.Value).String())
	return (*result.Value).String(), version
}

func add_entities (app_id string, app_version string) {
// Get the context, which is required by the SDK methods.
	ctx := context.Background()

	client := authoring.NewModelClient(endpoint)
	// Set the subscription key on the client.
	client.Authorizer = autorest.NewCognitiveServicesAuthorizer(subscription_key)

	uuid_app_id, err := uuid.FromString(app_id)
    if err != nil {
		log.Fatal(err)
    }
	entity_name := "Destination"
	_, err = client.AddEntity(ctx, uuid_app_id, app_version, authoring.ModelCreateObject { Name: &entity_name })
    if err != nil {
		log.Fatal(err)
    }
	
	h_entity_name := "Class"
	h_entity_children := []string{ "First", "Business", "Economy" }
	_, err = client.AddHierarchicalEntity(ctx, uuid_app_id, app_version, authoring.HierarchicalEntityModel { Name: &h_entity_name, Children: &h_entity_children })
    if err != nil {
		log.Fatal(err)
    }

	c_entity_name := "Flight"
	c_entity_children := []string{ "Class", "Destination" }
	_, err = client.AddCompositeEntity(ctx, uuid_app_id, app_version, authoring.CompositeEntityModel { Name: &c_entity_name, Children: &c_entity_children })
	    if err != nil {
		log.Fatal(err)
    }

	fmt.Println("Entities Destination, Class, Flight created.")
}

func add_intents(app_id string, app_version string) {
	ctx := context.Background()

	client := authoring.NewModelClient(endpoint)
	// Set the subscription key on the client.
	client.Authorizer = autorest.NewCognitiveServicesAuthorizer(subscription_key)

	uuid_app_id, err := uuid.FromString(app_id)
    if err != nil {
		log.Fatal(err)
    }

	name := "FindFlights"
	_, err = client.AddIntent(ctx, uuid_app_id, app_version, authoring.ModelCreateObject { Name: &name })

	fmt.Println("Intent FindFlights added.")
}

func create_utterance(intent string, text string, labels map[string]string) authoring.ExampleLabelObject {
    text = strings.ToLower(text)

	var results []authoring.EntityLabelObject

	for key, value := range labels {
		/* Make a copy of the key. The key is the entity name, and EntityLabelObject.EntityName
		requires a string reference, but we don't want to assign it the address of key, because
		key is a loop variable.
		*/
		entity_name := key
		value = strings.ToLower(value)
		start_index := int32(strings.Index(text, value))
		end_index := start_index + int32(len(value))
		if start_index > -1 {
			results = append(results, authoring.EntityLabelObject { EntityName: &entity_name, StartCharIndex: &start_index, EndCharIndex: &end_index })
		}
	}
	
	fmt.Printf("Created %d utterances.\n", len(results))

	return authoring.ExampleLabelObject { Text: &text, EntityLabels: &results, IntentName: &intent }
}

func add_utterances(app_id string, app_version string) {
	ctx := context.Background()

	client := authoring.NewExamplesClient(endpoint)
	// Set the subscription key on the client.
	client.Authorizer = autorest.NewCognitiveServicesAuthorizer(subscription_key)

	uuid_app_id, err := uuid.FromString(app_id)
    if err != nil {
		log.Fatal(err)
    }

	// Each entity must have at least one utterance before we train the application.
	utterance_1 := create_utterance("FindFlights", "find flights in economy to Madrid", map[string]string{ "Flight": "economy to Madrid", "Destination": "Madrid", "Class": "economy" })
	utterance_2 := create_utterance("FindFlights", "find flights to London in first class", map[string]string { "Flight": "London in first class", "Destination": "London", "Class": "first" })
	utterances := []authoring.ExampleLabelObject{ utterance_1, utterance_2 }

	_, err = client.Batch(ctx, uuid_app_id, app_version, utterances)
	    if err != nil {
		log.Fatal(err)
    }

	fmt.Println("Example utterances added.")
}

func train_app(app_id string, app_version string) {
	// Get the context, which is required by the SDK methods.
	ctx := context.Background()

	client := authoring.NewTrainClient(endpoint)
	// Set the subscription key on the client.
	client.Authorizer = autorest.NewCognitiveServicesAuthorizer(subscription_key)

	uuid_app_id, err := uuid.FromString(app_id)
    if err != nil {
		log.Fatal(err)
    }

	_, err = client.TrainVersion(ctx, uuid_app_id, app_version)
    if err != nil {
		log.Fatal(err)
    }

	// Wait for the train operation to finish.
	fmt.Println ("Waiting for train operation to finish...")

	waiting := true
	for true == waiting {
		result, err := client.GetStatus(ctx, uuid_app_id, app_version)
		if err != nil {
			log.Fatal(err)
		}
// GetStatus returns a list of training statuses, one for each model. Loop through them and make sure all are done.
		waiting = false
		for _, item := range *result.Value {
			details := *item.Details
			status := string (details.Status)
			if "Queued" == status || "InProgress" == status {
				waiting = true
			}
			if "Fail" == status {
				fmt.Println("Training operation failed. Result:")
				json_result, err := json.Marshal(result)
				if err != nil {
					log.Fatal(err)
				}
				log.Fatal(string(json_result))
			}
		}
		if true == waiting {
			fmt.Println("Waiting 10 seconds for training to complete...")
			time.Sleep(10 * time.Second)
		}
	}
}

func publish_app(app_id string, app_version string) {
	// Get the context, which is required by the SDK methods.
	ctx := context.Background()

	client := authoring.NewAppsClient(endpoint)
	// Set the subscription key on the client.
	client.Authorizer = autorest.NewCognitiveServicesAuthorizer(subscription_key)

	uuid_app_id, err := uuid.FromString(app_id)
    if err != nil {
		log.Fatal(err)
    }

	is_staging := true
	response, err := client.Publish(ctx, uuid_app_id, authoring.ApplicationPublishObject{ VersionID: &app_version, IsStaging: &is_staging })
    if err != nil {
		log.Fatal(err)
    }
	fmt.Println("Application published. Endpoint URL: " + *response.EndpointURL)
}

func delete_app(app_id string, app_version string) {
	// Get the context, which is required by the SDK methods.
	ctx := context.Background()

	client := authoring.NewAppsClient(endpoint)
	// Set the subscription key on the client.
	client.Authorizer = autorest.NewCognitiveServicesAuthorizer(subscription_key)

	uuid_app_id, err := uuid.FromString(app_id)
    if err != nil {
		log.Fatal(err)
    }

	force := false
	_, err = client.Delete(ctx, uuid_app_id, &force)
    if err != nil {
		log.Fatal(err)
    }
	fmt.Println("Application deleted.")
}

func list_apps() {
	// Get the context, which is required by the SDK methods.
	ctx := context.Background()

	client := authoring.NewAppsClient(endpoint)
	// Set the subscription key on the client.
	client.Authorizer = autorest.NewCognitiveServicesAuthorizer(subscription_key)

	var skip int32 = 0
	var max int32 = 100
	result, err := client.List(ctx, &skip, &max)
	if err != nil {
		log.Fatal(err)
	}
	for _, item := range *result.Value {
		fmt.Println(*item.ID)
	}

}

func main() {
	app_id, app_version := create_app()
	list_apps()
	add_entities(app_id, app_version)
	add_intents(app_id, app_version)
	add_utterances(app_id, app_version)
	train_app(app_id, app_version)
	publish_app(app_id, app_version)
	delete_app(app_id, app_version)
}
