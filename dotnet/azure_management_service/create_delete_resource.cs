using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* Note: Install the following NuGet packages:
Microsoft.Azure.Management.CognitiveServices 
Microsoft.Azure.Management.Fluent
Microsoft.Azure.Management.ResourceManager.Fluent
*/

using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.CognitiveServices;
using Microsoft.Azure.Management.CognitiveServices.Models;

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

        Be sure to use the service pricipal application ID, not simply the ID. 
        */

        const string  service_principal_application_id = "TODO_REPLACE";
        const string  service_principal_secret = "TODO_REPLACE";

        /* The ID of your Azure subscription. You can find this in the Azure Dashboard under Home > Subscriptions. */
        const string  subscription_id = "TODO_REPLACE";

        /* The Active Directory tenant ID. You can find this in the Azure Dashboard under Home > Azure Active Directory. */
        const string  tenant_id = "TODO_REPLACE";

        /* The name of the Azure resource group in which you want to create the resource.
        You can find resource groups in the Azure Dashboard under Home > Resource groups. */
        const string  resource_group_name = "TODO_REPLACE";

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

        static void create_resource(CognitiveServicesManagementClient client, string resource_name, string kind, string account_tier, string location)
        {
            Console.WriteLine("Creating resource: " + resource_name + "...");
            // The parameter "properties" must be an empty object.
            CognitiveServicesAccount parameters = 
                new CognitiveServicesAccount(null, null, kind, location, account_tier, new CognitiveServicesAccountProperties(), new Sku(account_tier));
            var result = client.Accounts.Create(resource_group_name, account_tier, parameters);
            Console.WriteLine("Resource created.");
            Console.WriteLine("ID: " + result.Id);
            Console.WriteLine("Kind: " + result.Kind);
            Console.WriteLine();
        }

        static void delete_resource(CognitiveServicesManagementClient client, string resource_name)
        {
            Console.WriteLine("Deleting resource: " + resource_name + "...");
            client.Accounts.Delete (resource_group_name, resource_name);

            Console.WriteLine("Resource deleted.");
            Console.WriteLine();
    }

    static void Main(string[] args)
        {
            /* For more information about authenticating, see:
             * https://docs.microsoft.com/en-us/dotnet/azure/dotnet-sdk-azure-authenticate?view=azure-dotnet
             */

            var service_principal_credentials = new ServicePrincipalLoginInformation ();
            service_principal_credentials.ClientId = service_principal_application_id;
            service_principal_credentials.ClientSecret = service_principal_secret;

            var credentials = SdkContext.AzureCredentialsFactory.FromServicePrincipal(service_principal_application_id, service_principal_secret, tenant_id, AzureEnvironment.AzureGlobalCloud);
            var client = new CognitiveServicesManagementClient(credentials);
            client.SubscriptionId = subscription_id;

            // Uncomment this to list all resources for your Azure account.
            //list_available_kinds_skus_locations(client);

            // Uncomment this to list all available resource kinds, SKUs, and locations for your Azure account.
            //list_resources(client);
        
            // Create a resource with kind Text Translation, F0 (free tier), location global.
            create_resource(client, "test_resource", "TextTranslation", "F0", "Global");

            // Delete the resource.
            delete_resource(client, "test_resource");

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
