// <snippet_imports>
"use strict";

/* To run this sample, install the following modules.
 * npm install @azure/arm-cognitiveservices @azure/identity
 */
var Arm = require("@azure/arm-cognitiveservices");
var Identity = require("@azure/identity");
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
const service_principal_application_id =
  "PASTE_YOUR_SERVICE_PRINCIPAL_APPLICATION_ID_HERE";
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

// <snippet_list_avail>
async function list_available_kinds_skus_locations(client) {
  console.log("Available SKUs:");
  var result = client.list();
  console.log("Kind\tSKU Name\tSKU Tier\tLocations");
  for await (let item of result) {
    var locations = item.locations.join(",");
    console.log(item.kind + "\t" + item.name + "\t" + item.tier + "\t" + locations);
  }
}
// </snippet_list_avail>

// <snippet_list>
async function list_resources(client) {
  console.log("Resources in resource group: " + resource_group_name);
  var result = client.listByResourceGroup(resource_group_name);
  for await (let item of result) {
    console.log(item);
    console.log();
  }
}
// </snippet_list>

// <snippet_create>
async function create_resource(
  client,
  resource_name,
  resource_kind,
  resource_sku,
  resource_region
) {
  console.log("Creating resource: " + resource_name + "...");
  /* NOTE If you do not want to use a custom subdomain name, remove the customSubDomainName
property from the properties object. */
  var parameters = {
    sku: { name: resource_sku },
    kind: resource_kind,
    location: resource_region,
    properties: { customSubDomainName: subdomain_name },
  };
  return client
    .beginCreateAndWait(resource_group_name, resource_name, parameters)
    .then((result) => {
      console.log("Resource created.");
      console.log();
      console.log("ID: " + result.id);
      console.log("Kind: " + result.kind);
      console.log();
    })
    .catch((err) => {
      console.log(err);
    });
}
// </snippet_create>

// <snippet_delete>
async function delete_resource(client, resource_name) {
  console.log("Deleting resource: " + resource_name + "...");
  await client.beginDeleteAndWait(resource_group_name, resource_name);
  console.log("Resource deleted.");
  console.log();
}
// </snippet_delete>

// <snippet_purge>
async function purge_resource(client, resource_name, resource_region) {
  console.log("Purging resource: " + resource_name + "...");
  await client.beginPurgeAndWait(
    resource_region,
    resource_group_name,
    resource_name
  );
  console.log("Resource purged.");
  console.log();
}
// </snippet_purge>

// <snippet_main_auth>
async function quickstart() {
  /* For more information see:
https://www.npmjs.com/package/@azure/arm-cognitiveservices/v/6.0.0
https://github.com/Azure/azure-sdk-for-js/blob/main/sdk/identity/identity/samples/AzureIdentityExamples.md#authenticating-a-service-principal-with-a-client-secret
*/
  const credentials = new Identity.ClientSecretCredential(
    tenant_id,
    service_principal_application_id,
    service_principal_secret
  );
  const client = new Arm.CognitiveServicesManagementClient(
    credentials,
    subscription_id
  );
  // Note Azure resources are also sometimes referred to as accounts.
  const accounts_client = client.accounts;
  const resource_skus_client = client.resourceSkus;
  const deleted_accounts_client = client.deletedAccounts;
  // </snippet_main_auth>

  // <snippet_main_calls>
  const resource_name = "test_resource";
  const resource_kind = "TextTranslation";
  const resource_sku = "F0";
  const resource_region = "Global";

  // Uncomment to list all available resource kinds, SKUs, and locations for your Azure account.
//  await list_available_kinds_skus_locations(resource_skus_client);

  // Create a resource with kind Text Translation, SKU F0 (free tier), location global.
  await create_resource(
    accounts_client,
    resource_name,
    resource_kind,
    resource_sku,
    resource_region
  );

  // Uncomment to list all resources for your Azure account.
//  await list_resources(accounts_client);

  // Delete the resource.
  await delete_resource(accounts_client, resource_name);

  /* NOTE: When you delete a resource, it is only soft-deleted. You must also purge it. Otherwise, if you try to create another
	resource with the same name or custom subdomain, you will receive an error stating that such a resource already exists. */
  // Purge the resource.
  await purge_resource(deleted_accounts_client, resource_name, resource_region);
}
// </snippet_main_calls>

// <snippet_main>
try {
  quickstart();
} catch (error) {
  console.log(error);
}
// </snippet_main>
