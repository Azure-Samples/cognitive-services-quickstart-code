// <snippet_imports>
"use strict";
/* To run this sample, install the following modules.
 * npm install @azure/arm-cognitiveservices
 * npm install @azure/ms-rest-js
 * npm install @azure/ms-rest-nodeauth
 */
var Arm = require("@azure/arm-cognitiveservices");
var msRestNodeAuth = require("@azure/ms-rest-nodeauth");
// </snippet_imports>

/*
The application ID and secret of the service principal you are using to connect to the Azure Management Service.

To create a service principal with the Azure CLI, see:
https://docs.microsoft.com/en-us/cli/azure/create-an-azure-service-principal-azure-cli?view=azure-cli-latest
To install the Azure CLI, see:
https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest

To create a service principal with Azure PowerShell, see: 
https://docs.microsoft.com/en-us/powershell/azure/create-azure-service-principal-azureps?view=azps-3.3.0
To install Azure PowerShell, see:
https://github.com/Azure/azure-powershell

When you create a service principal, you will see it has both an ID and an application ID.
For example, if you create a service principal with Azure PowerShell, it should look like the following:

Secret                : System.Security.SecureString
ServicePrincipalNames : {<application ID>, <application URL>}
ApplicationId         : <application ID>
ObjectType            : ServicePrincipal
DisplayName           : <name>
Id                    : <ID>
Type                  :

Be sure to use the service pricipal application ID, not simply the ID. 
*/

// <snippet_constants>
const service_principal_application_id = "TODO_REPLACE";
const service_principal_secret = "TODO_REPLACE";

/* The ID of your Azure subscription. You can find this in the Azure Dashboard under Home > Subscriptions. */
const subscription_id = "TODO_REPLACE";

/* The Active Directory tenant ID. You can find this in the Azure Dashboard under Home > Azure Active Directory. */
const tenant_id = "TODO_REPLACE";

/* The name of the Azure resource group in which you want to create the resource.
You can find resource groups in the Azure Dashboard under Home > Resource groups. */
const resource_group_name = "TODO_REPLACE";
// </snippet_constants>

// <snippet_list_avail>
async function list_available_kinds_skus_locations (client) {
	console.log ("Available SKUs:");
	var result = await client.list ();
	console.log("Kind\tSKU Name\tSKU Tier\tLocations");
	result.forEach (function (x) {
		var locations = x.locations.join(",");
		console.log(x.kind + "\t" + x.name + "\t" + x.tier + "\t" + locations);
	});
}
// </snippet_list_avail>

// <snippet_list>
async function list_resources (client) {
	console.log ("Resources in resource group: " + resource_group_name);
	var result = await client.listByResourceGroup (resource_group_name);
	result.forEach (function (x) {
		console.log(x);
		console.log();
	});
}
// </snippet_list>

// <snippet_create>
function create_resource (client, resource_name, kind, sku_name, location) {
	console.log ("Creating resource: " + resource_name + "...");
	// The parameter "properties" must be an empty object.
	var parameters = { sku : { name: sku_name }, kind : kind, location : location, properties : {} };

    return client.create(resource_group_name, resource_name, parameters)
        .then((result) => {
		console.log("Resource created.");
		print();
		console.log("ID: " + result.id);
		console.log("Kind: " + result.kind);
		console.log();
        })
        .catch((err) =>{ 
            	console.log(err)
        })
}
// </snippet_create>

// <snippet_delete>
async function delete_resource (client, resource_name) {
	console.log ("Deleting resource: " + resource_name + "...");
	await client.deleteMethod (resource_group_name, resource_name)
	console.log ("Resource deleted.");
	console.log ();
}
// </snippet_delete>

// <snippet_main_auth>
async function quickstart() {
	const credentials = await msRestNodeAuth.loginWithServicePrincipalSecret (service_principal_application_id, service_principal_secret, tenant_id);
	const client = new Arm.CognitiveServicesManagementClient (credentials, subscription_id);
	// Note Azure resources are also sometimes referred to as accounts.
	const accounts_client = new Arm.Accounts (client);
	const resource_skus_client = new Arm.ResourceSkus (client);
	// </snippet_main_auth>

	// <snippet_main_calls>
	// Uncomment this to list all available resource kinds, SKUs, and locations for your Azure account.
	list_available_kinds_skus_locations (resource_skus_client);

	// Create a resource with kind Text Translation, SKU F0 (free tier), location global.
	await create_resource (accounts_client, "test_resource", "TextTranslation", "F0", "Global");

	// Uncomment this to list all resources for your Azure account.
	list_resources (accounts_client);

	// Delete the resource.
	delete_resource (accounts_client, "test_resource");
}
// </snippet_main_calls>

// <snippet_main>
try {
    quickstart();
}
catch (error) {
    console.log(error);
}
// </snippet_main>
