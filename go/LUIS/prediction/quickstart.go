package main

import (
	"context"
	"encoding/json"
	"fmt"
	"github.com/Azure/azure-sdk-for-go/services/cognitiveservices/v2.0/luis/authoring"
	"github.com/Azure/azure-sdk-for-go/services/cognitiveservices/v3.0/luis/runtime"
	"github.com/Azure/go-autorest/autorest"
	"github.com/satori/go.uuid"
	"log"
	"os"
	"time"
)
 
var authoring_key string = "PASTE_YOUR_LUIS_AUTHORING_SUBSCRIPTION_KEY_HERE"
var authoring_endpoint string = "PASTE_YOUR_LUIS_AUTHORING_ENDPOINT_HERE"
var runtime_key string = "PASTE_YOUR_LUIS_PREDICTION_SUBSCRIPTION_KEY_HERE"
var runtime_endpoint string = "PASTE_YOUR_LUIS_PREDICTION_ENDPOINT_HERE"
//  END - Configure the local environment.

func create_app() (string) {
	// Create a new LUIS app
	// Get the context, which is required by the SDK methods.
	ctx := context.Background()

	client := authoring.NewAppsClient(authoring_endpoint)
	// Set the subscription key on the client.
	client.Authorizer = autorest.NewCognitiveServicesAuthorizer(authoring_key)

	domain_name := "HomeAutomation"
	culture := "en-us"

	create_app_payload := authoring.PrebuiltDomainCreateObject { DomainName: &domain_name, Culture: &culture }

	result, err := client.AddCustomPrebuiltDomain(ctx, create_app_payload)
	if err != nil {
		log.Fatal(err)
	}

	fmt.Printf("Created LUIS app %s with ID %s\n", domain_name, (*result.Value).String())
	return (*result.Value).String()
}

func train_app(app_id string, app_version string) {
	// Get the context, which is required by the SDK methods.
	ctx := context.Background()

	client := authoring.NewTrainClient(authoring_endpoint)
	// Set the subscription key on the client.
	client.Authorizer = autorest.NewCognitiveServicesAuthorizer(authoring_key)

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

	client := authoring.NewAppsClient(authoring_endpoint)
	// Set the subscription key on the client.
	client.Authorizer = autorest.NewCognitiveServicesAuthorizer(authoring_key)

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

func predict(app_id string) {
	ctx := context.Background()

	client := runtime.NewPredictionClient(runtime_endpoint)
	// Set the subscription key on the client.
	client.Authorizer = autorest.NewCognitiveServicesAuthorizer(runtime_key)

	uuid_app_id, err := uuid.FromString(app_id)
    if err != nil {
		log.Fatal(err)
    }

	slot_name := "staging"
	query := "turn on the lights"
	request := runtime.PredictionRequest { Query: &query }
	verbose := false
	show_all_intents := true
	log_parameter := false

// Note be sure to specify, using the slot_name parameter, whether your application is in staging or production.
// By default, applications are created in staging.
	response, err := client.GetSlotPrediction(ctx, uuid_app_id, slot_name, request, &verbose, &show_all_intents, &log_parameter)
    if err != nil {
		log.Fatal(err)
    }

	prediction := *response.Prediction
	result, err := json.Marshal(prediction)
    if err != nil {
        log.Fatal(err)
    }
	fmt.Println("Result:")
    fmt.Println(string(result))
}

func delete_app(app_id string, app_version string) {
	// Get the context, which is required by the SDK methods.
	ctx := context.Background()

	client := authoring.NewAppsClient(authoring_endpoint)
	// Set the subscription key on the client.
	client.Authorizer = autorest.NewCognitiveServicesAuthorizer(authoring_key)

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

func main() {
	app_id := create_app()
	app_version := "0.1"
	train_app(app_id, app_version)
	publish_app(app_id, app_version)
	predict(app_id)
	delete_app(app_id, app_version)
}
