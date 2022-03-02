// <snippet_using>
using System;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.CognitiveServices;
using Microsoft.Azure.Management.CognitiveServices.Models;
// </snippet_using>

/* Note: Install the following NuGet packages:
dotnet add package Microsoft.Azure.Management.CognitiveServices --version 8.0.0-preview
dotnet add package Microsoft.Azure.Management.Fluent
dotnet add package Microsoft.Azure.Management.ResourceManager.Fluent
*/

namespace ConsoleApp1
{
    class Program
    {
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

        Be sure to use the service principal application ID, not simply the ID. 
        */

        // <snippet_constants>
        const string  service_principal_application_id = "PASTE_YOUR_SERVICE_PRINCIPAL_APPLICATION_ID_HERE";
        const string  service_principal_secret = "PASTE_YOUR_SERVICE_PRINCIPAL_SECRET_HERE";

        /* The ID of your Azure subscription. You can find this in the Azure Dashboard under Home > Subscriptions. */
        const string  subscription_id = "PASTE_YOUR_SUBSCRIPTION_ID_HERE";

        /* The Active Directory tenant ID. You can find this in the Azure Dashboard under Home > Azure Active Directory. */
        const string  tenant_id = "PASTE_YOUR_TENANT_ID_HERE";

        /* The name of the Azure resource group in which you want to create the resource.
        You can find resource groups in the Azure Dashboard under Home > Resource groups. */
        const string  resource_group_name = "PASTE_YOUR_RESOURCE_GROUP_NAME_HERE";

        /* The name of the custom subdomain to use when you create the resource. This is optional.
        For example, if you create a Bing Search v7 resource with the custom subdomain name 'my-search-resource',
        your resource would have the endpoint https://my-search-resource.cognitiveservices.azure.com/.
        Note not all Cognitive Services allow custom subdomain names. */
        const string subdomain_name = "PASTE_YOUR_SUBDOMAIN_NAME_HERE";
        // </snippet_constants>

        // <snippet_list_avail>
        static void list_available_kinds_skus_locations(CognitiveServicesManagementClient client)
        {

            Console.WriteLine("Available SKUs:");
            var result = client.ResourceSkus.List();
            Console.WriteLine("Kind\tSKU Name\tSKU Tier\tLocations");
            foreach (var x in result) {
                var locations = "";
                foreach (var region in x.Locations)
                {
                    locations += region;
                }
                Console.WriteLine(x.Kind + "\t" + x.Name + "\t" + x.Tier + "\t" + locations);
            };
        }
        // </snippet_list_avail>

        // <snippet_list>
        static void list_resources(CognitiveServicesManagementClient client)
        {
            Console.WriteLine("Resources in resource group: " + resource_group_name);
            var result = client.Accounts.ListByResourceGroup(resource_group_name);
            foreach (var x in result)
            {
                Console.WriteLine("ID: " + x.Id);
                Console.WriteLine("Name: " + x.Name);
                Console.WriteLine("Type: " + x.Type);
                Console.WriteLine("Kind: " + x.Kind);
                Console.WriteLine();
            }
        }
        // </snippet_list>

        // <snippet_create>
        static void create_resource(CognitiveServicesManagementClient client, string resource_name, string kind, string account_tier, string location)
        {
            Console.WriteLine("Creating resource: " + resource_name + "...");
            /* NOTE If you do not want to use a custom subdomain name, remove the customSubDomainName
            property from AccountProperties. */
			      Account parameters = new Account(kind: kind, location: location, name: resource_name, sku: new Sku(account_tier), properties: new AccountProperties(customSubDomainName : subdomain_name));
            var result = client.Accounts.Create(resource_group_name, resource_name, parameters);
            Console.WriteLine("Resource created.");
            Console.WriteLine("ID: " + result.Id);
            Console.WriteLine("Kind: " + result.Kind);
            Console.WriteLine();
        }
        // </snippet_create>

        // <snippet_delete>
        static void delete_resource(CognitiveServicesManagementClient client, string resource_name)
        {
            Console.WriteLine("Deleting resource: " + resource_name + "...");
            client.Accounts.Delete (resource_group_name, resource_name);

            Console.WriteLine("Resource deleted.");
            Console.WriteLine();
        }
        // </snippet_delete>

        // <snippet_purge>
        static void purge_resource(CognitiveServicesManagementClient client, string resource_name, string resource_region)
        {
            Console.WriteLine("Purging resource: " + resource_name + "...");
            client.DeletedAccounts.Purge (resource_region, resource_group_name, resource_name);

            Console.WriteLine("Resource purged.");
            Console.WriteLine();
        }
        // </snippet_purge>

        static void Main(string[] args)
        {
            /* For more information about authenticating, see:
             * https://docs.microsoft.com/en-us/dotnet/azure/dotnet-sdk-azure-authenticate?view=azure-dotnet
             */

            // <snippet_assigns>
            var service_principal_credentials = new ServicePrincipalLoginInformation ();
            service_principal_credentials.ClientId = service_principal_application_id;
            service_principal_credentials.ClientSecret = service_principal_secret;

            var credentials = SdkContext.AzureCredentialsFactory.FromServicePrincipal(service_principal_application_id, service_principal_secret, tenant_id, AzureEnvironment.AzureGlobalCloud);
            var client = new CognitiveServicesManagementClient(credentials);
            client.SubscriptionId = subscription_id;
            // </snippet_assigns>

            // <snippet_calls>
            string resource_name = "test_resource";
            string resource_kind = "TextTranslation";
            string resource_sku = "F0";
            string resource_region = "Global";

            // Uncomment to list all available resource kinds, SKUs, and locations for your Azure account:
            // list_available_kinds_skus_locations(client);
        
            // Create a resource with kind TextTranslation, F0 (free tier), location global.
            create_resource(client, resource_name, resource_kind, resource_sku, resource_region);

            // Uncomment to list all resources for your Azure account and resource group:
            // list_resources(client);

            // Delete the resource.
            delete_resource(client, resource_name);

            /* NOTE: When you delete a resource, it is only soft-deleted. You must also purge it. Otherwise, if you try to create another
            resource with the same name or custom subdomain, you will receive an error stating that such a resource already exists. */
            purge_resource(client, resource_name, resource_region);

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            // </snippet_calls>
        }
    }
}
