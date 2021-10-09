package main

import (
	"context"
	"bufio"
	"fmt"
	"github.com/Azure/azure-sdk-for-go/services/preview/personalizer/v1.0/personalizer"
	"github.com/Azure/go-autorest/autorest"
	"github.com/nu7hatch/gouuid"
	"log"
	"os"
	"strconv"
	"strings"
)

/* This quickstart demonstrates the Personalizer service. A user is asked questions 
and their preferences are saved. The API tries to guess what the user wants, based
on their preferences. Then the user is asked to confirm if the guess is correct. The
user's response is then sent to the API for the service to learn better what the
user likes. Upon the 2nd round of questions, the API delivers a learned guess.

Prerequisites (install from command line):
	go get github.com/Azure/azure-sdk-for-go/services/preview/personalizer/v1.0/personalizer
	go get github.com/Azure/go-autorest/autorest
	go get github.com/nu7hatch/gouuid

How to run (from command line):
	go run PersonalizerQuickstart.go

References:
	SDK Source Code: https://github.com/Azure/azure-sdk-for-go/tree/master/services/preview/personalizer/v1.0/personalizer
	Personalizer Documentation: https://docs.microsoft.com/en-us/azure/cognitive-services/personalizer/index
	API Testing Console (REST): https://westus2.dev.cognitive.microsoft.com/docs/services/personalizer-api/operations/Rank
*/

// Declare global so don't have to pass it to all of the tasks.
var personalizerContext context.Context

func main() {

	personalizerKey := "PASTE_YOUR_PERSONALIZER_SUBSCRIPTION_KEY_HERE"
	personalizerEndpoint := "PASTE_YOUR_PERSONALIZER_ENDPOINT_HERE"

	personalizerClient := personalizer.New(personalizerEndpoint);
	personalizerClient.Authorizer = autorest.NewCognitiveServicesAuthorizer(personalizerKey)

	// Create an events client to handle the user rewards involved in training
	personalizerEventsClient := personalizer.NewEventsClient(personalizerEndpoint)

	personalizerContext = context.Background()

	// Start the Personalizer process with interactions of the user
	LearningLoop(personalizerClient, personalizerEventsClient)
}

// GetActions represents the content choices you want Personalizer to rank
func GetActions() []personalizer.RankableAction {
	// ID - READ-ONLY; Id of the action
	pastaID := "pasta"
	iceCreamID := "ice cream"
	juiceID := "juice"
	saladID := "salad"
	
	// Features - taste, spice level, nutrition level, cuisine or drink
	var pasta = []interface {} {
		map[string]interface{} { "taste": "salty", "spiceLevel": "medium" },
		map[string]interface{} { "nutritionLevel": 5, "cuisine": "italian" },
	}
	var iceCream = []interface {} { 
		map[string]interface{} { "taste": "sweet", "spiceLevel": "none" },
		map[string]interface{} { "nutritionLevel": 2 },
	}

	var juice = []interface {} {
		map[string]interface{} { "taste": "sweet", "spiceLevel": "none" },
		map[string]interface{} { "nutritionLevel": 5 },
		map[string]interface{} { "drink": true },
	} 

	var salad = []interface {} { 		
		map[string]interface{} { "taste": "salty", "spiceLevel": "low", },
		map[string]interface{} { "nutritionLevel": 8 },
	}

	// Cal API with ID and features
	action1 := personalizer.RankableAction{ ID: &pastaID, Features: &pasta }
	action2 := personalizer.RankableAction{ ID: &iceCreamID, Features: &iceCream }
	action3 := personalizer.RankableAction{ ID: &juiceID, Features: &juice }
	action4 := personalizer.RankableAction{ ID: &saladID, Features: &salad }

	actions := []personalizer.RankableAction{ action1, action2, action3, action4 }

	return actions
}

// GetTimeOfDay will get a user's input from the command line for the time of day  
func GetTimeOfDay() string {

	timeFeatures := []string { "morning", "afternoon", "evening", "night" }
	
	fmt.Println("What time of day is it (type number)? 1. morning 2. afternoon 3. evening 4. night, then press enter.")
	input := bufio.NewScanner(os.Stdin)
	var number string
	for input.Scan() {
		number = (*input).Text()
		break
	}
	n, err := strconv.ParseInt(number, 10, 64)
	if err != nil { log.Fatal(err) }

	var timeChosen = timeFeatures[n - 1] 
	fmt.Println(timeChosen)
	fmt.Println()

	return timeChosen
}

// GetUserPreferences will get a user's input from the command line for their taste preferences
func GetUserPreferences() string {

	tasteFeatures := []string { "salty", "sweet" }

	fmt.Println("What type of food would you prefer (type number)? 1.salty 2.sweet, then press enter.")
	input := bufio.NewScanner(os.Stdin)
	var number string
	for input.Scan() { 
		if (len((*input).Text()) > 0) {
			number = (*input).Text()
			break
		}
	 }
	n, err := strconv.ParseInt(number, 10, 64)
	if err != nil { log.Fatal(err) }	

	var tasteChosen = tasteFeatures[n - 1] 
	fmt.Println(tasteChosen)
	fmt.Println()
	
	return tasteChosen
}

// LearningLoop asks for user preferences, sends information to Personalizer to rank,
// presents ranked selections back to user for them to choose, finally sends reward
// back to Personalizer saying how well the service performed.
func LearningLoop(client personalizer.BaseClient, clientEvents personalizer.EventsClient) {
	keepGoing := true

	for (keepGoing) {

		eventIDuuid, err := uuid.NewV4()
		if err != nil { log.Fatal(err) }

		eventID := eventIDuuid.String()

		// Collect the user preferences
		var ContextRank = []interface{} {
			map[string]string{
				"taste": GetUserPreferences(),
			},
			map[string]string{
				"timeOfDay": GetTimeOfDay(),
			},
		}

		// Get all possible choices
		actions := GetActions()

		// Rank the actions (user choices) based on the preferences
		rankRequest := personalizer.RankRequest { ContextFeatures: &ContextRank, Actions: &actions, EventID: &eventID }
		response, err := client.Rank(personalizerContext, rankRequest)
		if err != nil { log.Fatal(err) }

		fmt.Println("Ranked actions with their probabilities:")

		// Print the ID and probability of the ranked actions
		for _, ranked := range *response.Ranking {
			fmt.Println(*ranked.ID, ": ", *ranked.Probability)
		}
		fmt.Println()

		// Suggest options to user based on rankings
		fmt.Println("Personalizer thinks you would like to have: ", *response.RewardActionID)
		fmt.Println()
		fmt.Println("Is this correct?(y/n)")
		var answer string
		input := bufio.NewScanner(os.Stdin)
		for input.Scan() {
			if (len((*input).Text()) > 0) {
				answer = (*input).Text()
				break
			}
		}

		// Calculate the reward value if user approved of rankings or not
		var rewardValue float64
		if strings.ToLower(answer) == "y" { 
			rewardValue = 1.0
		} else if strings.ToLower(answer)  == "n" { 
			rewardValue = 0.0
		} else {
			fmt.Print("Entered choice is invalid. Service assumes that you didn't like the recommended food choice.")
		}

		// Give the reward (user's input) back to the learning process
		reward := personalizer.RewardRequest { Value: &rewardValue } 

		// Let Personalizer know about the reward value
		clientEvents.Reward(personalizerContext, eventID, reward)

		fmt.Print("Press 'q' to quit or 'enter' to continue.")

		var answerContinue string
		inputContinue := bufio.NewScanner(os.Stdin)
		for inputContinue.Scan() {
			if (len((*inputContinue).Text()) > 0) {
				answerContinue = inputContinue.Text()
			}
			if strings.ToLower(answerContinue) == "q" {
				keepGoing = false
			}
			break
		}
		fmt.Println()
	}
}


