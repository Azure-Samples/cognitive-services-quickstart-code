// <snippet_imports>
import com.microsoft.azure.*;
import com.microsoft.azure.arm.resources.Region;
import com.microsoft.azure.credentials.*;
import com.microsoft.azure.management.cognitiveservices.v2017_04_18.*;
import com.microsoft.azure.management.cognitiveservices.v2017_04_18.implementation.*;

import java.io.*;
import java.lang.Object.*;
import java.util.*;
import java.net.*;
// </snippet_imports>

/* To compile and run, enter the following at a command prompt:
 * javac Quickstart.java -cp .;lib\*
 * java -cp .;lib\* Quickstart
 * This presumes your libraries are stored in a folder named "lib"
 * directly under the current folder. If not, please adjust the
 * -classpath/-cp value accordingly.
 */

public class Quickstart {
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
	*/

	// <snippet_constants>
	/*
	Be sure to use the service pricipal application ID, not simply the ID. 
	*/
    private static String applicationId = "INSERT APPLICATION ID HERE";
	private static String applicationSecret = "INSERT APPLICATION SECRET HERE";

	/* The ID of your Azure subscription. You can find this in the Azure Dashboard under Home > Subscriptions. */
    private static String subscriptionId = "INSERT SUBSCRIPTION ID HERE";

	/* The Active Directory tenant ID. You can find this in the Azure Dashboard under Home > Azure Active Directory. */
	private static String tenantId = "INSERT TENANT ID HERE";

	/* The name of the Azure resource group in which you want to create the resource.
	You can find resource groups in the Azure Dashboard under Home > Resource groups. */
	private static String resourceGroupName = "INSERT RESOURCE GROUP NAME HERE";
	// </snippet_constants>

	public static void main(String[] args) {

		// <snippet_auth>
		// auth
		private static ApplicationTokenCredentials credentials = new ApplicationTokenCredentials(applicationId, tenantId, applicationSecret, AzureEnvironment.AZURE);

		CognitiveServicesManager client = CognitiveServicesManager.authenticate(credentials, subscriptionId);
		// </snippet_auth>

		// <snippet_calls>
		// list all available resource kinds, SKUs, and locations for your Azure account.
		list_available_kinds_skus_locations (client);

		// list all resources for your Azure account.
		list_resources (client);

		// Create a resource with kind Text Translation, SKU F0 (free tier), location global.
		String resourceId = create_resource (client, "test_resource", resourceGroupName, "TextAnalytics", "S0", Region.US_WEST);

		// Delete the resource.
		delete_resource (client, resourceId);
	}
	// </snippet_calls>

	// <snippet_list_avail>
	public void list_available_kinds_skus_locations (CognitiveServicesManager client) {
		System.out.println ("Available SKUs:");
		System.out.println("Kind\tSKU Name\tSKU Tier\tLocations");
		ResourceSkus skus = client.resourceSkus();
		// See https://github.com/ReactiveX/RxJava/wiki/Blocking-Observable-Operators
		for (ResourceSku sku : skus.listAsync().toBlocking().toIterable()) {
			String locations = String.join (",", sku.locations());
			System.out.println (sku.kind() + "\t" + sku.name() + "\t" + sku.tier() + "\t" + locations);
		}
	}
	// </snippet_list_avail>

	// Note: Region values are listed in:
	// https://github.com/Azure/autorest-clientruntime-for-java/blob/master/azure-arm-client-runtime/src/main/java/com/microsoft/azure/arm/resources/Region.java
	// <snippet_create>
	public String create_resource (CognitiveServicesManager client, String resourceName, String resourceGroupName, String kind, String skuName, Region region) {
		System.out.println ("Creating resource: " + resourceName + "...");

		CognitiveServicesAccount result = client.accounts().define(resourceName)
			.withRegion(region)
			.withExistingResourceGroup(resourceGroupName)
			.withKind(kind)
			.withSku(new Sku().withName(skuName))
			.create();

		System.out.println ("Resource created.");
		System.out.println ("ID: " + result.id());
		System.out.println ("Provisioning state: " + result.properties().provisioningState().toString());
		System.out.println ();

		return result.id();
	}
	// </snippet_create>

	// <snippet_list>
	public void list_resources (CognitiveServicesManager client) {
		System.out.println ("Resources in resource group: " + resourceGroupName);
		// Note Azure resources are also sometimes referred to as accounts.
		Accounts accounts = client.accounts();
		for (CognitiveServicesAccount account : accounts.listByResourceGroupAsync(resourceGroupName).toBlocking().toIterable()) {
			System.out.println ("Kind: " + account.kind ());
			System.out.println ("SKU Name: " + account.sku().name());
			System.out.println ();
		}
	}
	// </snippet_list>
	
	// <snippet_delete>
	public void delete_resource (CognitiveServicesManager client, String resourceId) {
		System.out.println ("Deleting resource: " + resourceId + "...");
		client.accounts().deleteByIds (resourceId);
		System.out.println ("Resource deleted.");
		System.out.println ();
	}
	// </snippet_delete>
}
