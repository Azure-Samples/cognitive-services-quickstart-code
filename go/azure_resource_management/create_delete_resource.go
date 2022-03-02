// <snippet_imports>
package main

import (
	"context"
	"fmt"
	"github.com/Azure/go-autorest/autorest/azure/auth"
	"github.com/Azure/azure-sdk-for-go/services/cognitiveservices/mgmt/2021-04-30/cognitiveservices"
	"log"
//	"os"
)
// </snippet_imports>

// <snippet_constants>
const service_principal_application_id = "PASTE_YOUR_SERVICE_PRINCIPAL_APPLICATION_ID_HERE";
const service_principal_secret = "PASTE_YOUR_SERVICE_PRINCIPAL_SECRET_HERE";

/* The ID of your Azure subscription. You can find this in the Azure Dashboard under Home > Subscriptions. */
const subscription_id = "PASTE_YOUR_SUBSCRIPTION_ID_HERE";

/* The Active Directory tenant ID. You can find this in the Azure Dashboard under Home > Azure Active Directory. */
const tenant_id = "PASTE_YOUR_TENANT_ID_HERE";

/* The name of the Azure resource group in which you want to create the resource.
You can find resource groups in the Azure Dashboard under Home > Resource groups. */
const resource_group_name = "PASTE_YOUR_RESOURCE_GROUP_NAME_HERE";

/* The name of the custom subdomain to use when you create the resource. This is optional.
For example, if you create a Bing Search v7 resource with the custom subdomain name 'my-search-resource',
your resource would have the endpoint https://my-search-resource.cognitiveservices.azure.com/.
Note not all Cognitive Services allow custom subdomain names.
*/
const subdomain_name = "PASTE_YOUR_SUBDOMAIN_NAME_HERE";
// </snippet_constants>

func main() {
	// <snippet_main_auth>
	/* For more information about authentication, see:
	https://docs.microsoft.com/en-us/azure/developer/go/azure-sdk-authorization
	https://github.com/Azure-Samples/azure-sdk-for-go-samples/blob/master/cognitiveservices/account.go
	*/
	var config = auth.NewClientCredentialsConfig (service_principal_application_id, service_principal_secret, tenant_id)
	authorizer, err := config.Authorizer ()
	if nil != err {
		log.Fatal (err)
	}

	var accounts_client = cognitiveservices.NewAccountsClient (subscription_id)
	accounts_client.Authorizer = authorizer

	var deleted_accounts_client = cognitiveservices.NewDeletedAccountsClient (subscription_id)
	deleted_accounts_client.Authorizer = authorizer
	// </snippet_main_auth>

	// <snippet_main_calls>
	const resource_name = "test_resource";
	const resource_kind = "TextTranslation";
	const resource_sku = "F0";
	const resource_region = "Global";

	// Uncomment to list all resources for your Azure account.
	// list_resources (accounts_client)

	create_resource (accounts_client, resource_name, resource_group_name, resource_kind, resource_sku, resource_region)

	// Uncomment to list all available resource kinds, SKUs, and locations for your Azure account.
	list_available_kinds_skus_locations (accounts_client, resource_name)

	delete_resource (accounts_client, resource_name, resource_group_name)

	purge_resource (deleted_accounts_client, resource_name, resource_group_name, resource_region)
	// </snippet_main_calls>
}

// <snippet_list_avail>
func list_available_kinds_skus_locations (client cognitiveservices.AccountsClient, resource_name string) {
	ctx := context.Background()
	results, err := client.ListSkus (ctx, resource_group_name, resource_name)
	if nil != err {
		log.Fatal (err)
	}
	for _, result := range *results.Value {
		fmt.Println ("Resource type: " + *result.ResourceType)
		fmt.Println ("SKU: " + *result.Sku.Name)
		fmt.Println ()
	}
}
// </snippet_list_avail>

// <snippet_list>
func list_resources (client cognitiveservices.AccountsClient) {
	ctx := context.Background()
	results, err := client.ListByResourceGroup (ctx, resource_group_name)
	if nil != err {
		log.Fatal (err)
	}
	for results.NotDone () {
		for _, result := range results.Values () {
			fmt.Println ("ID: " + *result.ID)
			fmt.Println ("Name: " + *result.Name)
			fmt.Println ("Kind: " + *result.Kind)
			fmt.Println ("SKU: " + *result.Sku.Name)
			fmt.Println ("Location: " + *result.Location)
			fmt.Println ()
		}
		err = results.NextWithContext (ctx)
		if nil != err {
			log.Fatal (err)
		}
	}
}
// </snippet_list>

// <snippet_create>
func create_resource (client cognitiveservices.AccountsClient, resource_name string, resource_group_name string, resource_kind string, resource_sku string, resource_region string) {
	ctx := context.Background()

	var properties = cognitiveservices.Account {
		Kind: &resource_kind,
		Sku: &cognitiveservices.Sku {
			Name: &resource_sku,
			Tier: cognitiveservices.SkuTierFree,
		},
		Location: &resource_region,
		Properties: nil,
	}

	fmt.Println ("Creating resource...")
	_, err := client.Create (ctx, resource_group_name, resource_name, properties)
	if nil != err {
		log.Fatal (err)
	}
	fmt.Println ("Resource created.")
}
// </snippet_create>

// <snippet_delete>
func delete_resource (client cognitiveservices.AccountsClient, resource_name string, resource_group_name string) {
	ctx := context.Background()

	fmt.Println ("Deleting resource...")
	_, err := client.Delete (ctx, resource_group_name, resource_name)
	if nil != err {
		log.Fatal (err)
	}
	fmt.Println ("Resource deleted.")
}
// </snippet_delete>

// <snippet_purge>
func purge_resource (client cognitiveservices.DeletedAccountsClient, resource_name string, resource_group_name string, resource_region string) {
	ctx := context.Background()

	fmt.Println ("Purging resource...")
	_, err := client.Purge (ctx, resource_region, resource_group_name, resource_name)
	if nil != err {
		log.Fatal (err)
	}
	fmt.Println ("Resource purged.")

}
// </snippet_purge>
