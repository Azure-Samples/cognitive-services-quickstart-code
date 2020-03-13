package main

/* To install the needed packages, run:
go get github.com/Azure/azure-sdk-for-go/services/cognitiveservices/v1.0/localsearch
go get github.com/Azure/go-autorest/autorest
*/
import (
	"context"
	"fmt"
	"github.com/Azure/azure-sdk-for-go/services/cognitiveservices/v1.0/localsearch"
	"github.com/Azure/go-autorest/autorest"
	"log"
	"os"
)

func main() {
/* Note In your Bing Search resource, select pricing tier S10 to access Bing Local.
 * See https://azure.microsoft.com/en-us/pricing/details/cognitive-services/search-api/.
 */
	if "" == os.Getenv("BING_LOCAL_SUBSCRIPTION_KEY") {
		log.Fatal("Please set/export the environment variable BING_LOCAL_SUBSCRIPTION_KEY.")
	}
	var subscription_key string = os.Getenv("BING_LOCAL_SUBSCRIPTION_KEY")
	if "" == os.Getenv("BING_LOCAL_ENDPOINT") {
		log.Fatal("Please set/export the environment variable BING_LOCAL_ENDPOINT.")
	}
	var endpoint string = os.Getenv("BING_LOCAL_ENDPOINT")

	// Get the context, which is required by the SDK methods.
	ctx := context.Background()

	client := localsearch.NewLocalClient()
	// Set the subscription key on the client.
	client.Authorizer = autorest.NewCognitiveServicesAuthorizer(subscription_key)
	client.BaseURI = endpoint + "/bing"

/* Note: If you do not include the location parameter, you might not get any results.

From code comments in
https://github.com/Azure/azure-sdk-for-go/blob/master/services/cognitiveservices/v1.0/localsearch/local.go

location - a semicolon-delimited list of key/value pairs that describe the
client's geographical location. Bing uses the location information to determine
safe search behavior and to return relevant local content.

Specify the key/value pair as <key>:<value>. The following are the keys that you
use to specify the user's location.

lat (required): The latitude of the client's location, in degrees. The latitude
must be greater than or equal to -90.0 and less than or equal to +90.0. Negative
values indicate southern latitudes and positive values indicate northern
latitudes.

long (required): The longitude of the client's location, in degrees. The
longitude must be greater than or equal to -180.0 and less than or equal to
+180.0. Negative values indicate western longitudes and positive values indicate
eastern longitudes.

re (required): The radius, in meters, which specifies the horizontal accuracy of
the coordinates. Pass the value returned by the device's location service.
Typical values might be 22m for GPS/Wi-Fi, 380m for cell tower triangulation,
and 18,000m for reverse IP lookup.
*/
	result, err := client.Search (ctx, "restaurant", "", "", "", "", "", "lat:47.608013;long:-122.335167;re:100m", "", "", "", "", "", "", "", []localsearch.ResponseFormat{"Json"}, "", "")
	if nil != err {
		log.Fatal(err)
	}

	places := *(*result.Places).Value
	if len(places) > 0 {
		fmt.Println ("Results:\n")
		for _, item := range places {
			place, success := item.AsThing()
			if success {
				fmt.Println ("Name: " + *place.Name)
				if nil != place.URL {
					fmt.Println ("URL: " + *place.URL)
				}
				if nil != place.WebSearchURL {
					fmt.Println ("Web search URL: " + *place.WebSearchURL)
				}
				fmt.Println ()
			}
		}
	} else {
		fmt.Println("No places found for this query.")
	}
}
