// <snippet_using>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.CognitiveServices;
using Microsoft.Azure.Management.CognitiveServices.Models;
// </snippet_using>



namespace ConsoleApp1
{
    class Program
    {

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

        static void Main(string[] args)
        {
            /* For more information about authenticating, see:
             * https://docs.microsoft.com/en-us/dotnet/azure/dotnet-sdk-azure-authenticate?view=azure-dotnet
             */

            // <snippet_assigns>
            // First we construct our armClient
            ArmClient client = new ArmClient(new DefaultAzureCredential());

            // Next we get a resource group object
            // ResourceGroup is a [Resource] object from above
            Subscription subscription = await client.GetDefaultSubscriptionAsync();
            ResourceGroup resourceGroup = await subscription.GetResourceGroups().GetAsync("myRgName");


            // </snippet_assigns>

            // <snippet_calls>
            // Create a resource with kind TextTranslation, F0 (free tier), location global.
            create_resource(client, "test_resource", "TextTranslation", "F0", "Global");

            // List all resources for your Azure account and resource group:
            list_resources(client);

            // Delete the resource.
            delete_resource(client, "test_resource");

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            // </snippet_calls>
        }

        // <snippet_create>
        static void create_resource(CognitiveServicesManagementClient client, string resource_name, string kind, string account_tier, string location)
        {

            ResourceIdentifier id = new ResourceIdentifier("/subscriptions/{subscription_id}/resourceGroups/{resourcegroup_name}/providers/Microsoft.Network/virtualNetworks/{vnet_name}");


            var createResult = await client.GetGenericResources().CreateOrUpdateAsync(WaitUntil.Completed, id, data);
            
            Console.WriteLine($"Resource {createResult.Value.Id.Name} in resource group {createResult.Value.Id.ResourceGroupName} created");

        }
        // </snippet_create>

        // <snippet_list>
        static void list_resources(CognitiveServicesManagementClient client)
        {

        }
        // </snippet_list>

        // <snippet_delete>
        static void delete_resource(CognitiveServicesManagementClient client, string resource_name)
        {

        }
        // </snippet_delete>

}
