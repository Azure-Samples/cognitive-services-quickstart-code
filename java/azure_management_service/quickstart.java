// <snippet_imports>
import com.azure.core.management.*;
import com.azure.core.management.profile.*;
import com.azure.identity.*;
import com.azure.resourcemanager.cognitiveservices.*;
import com.azure.resourcemanager.cognitiveservices.implementation.*;
import com.azure.resourcemanager.cognitiveservices.models.*;

import java.io.*;
import java.lang.Object.*;
import java.util.*;
import java.net.*;
// </snippet_imports>

/* To compile and run in Windows, enter the following at a command prompt:
 * javac Quickstart.java -cp .;lib\*
 * java -cp .;lib\* Quickstart
 *
 * This presumes your libraries are stored in a folder named "lib"
 * directly under the current folder. If not, please adjust the
 * -classpath/-cp value accordingly.
 *
 * To compile and run in Linux, enter the following at a shell:
 * javac -classpath .:./lib/* Quickstart.java
 * java -classpath .:./lib/* Quickstart
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
	
	private static String applicationId = "PASTE_YOUR_SERVICE_PRINCIPAL_APPLICATION_ID_HERE";
	private static String applicationSecret = "PASTE_YOUR_SERVICE_PRINCIPAL_SECRET_HERE";

	/* The ID of your Azure subscription. You can find this in the Azure Dashboard under Home > Subscriptions. */
	private static String subscriptionId = "PASTE_YOUR_SUBSCRIPTION_ID_HERE";

	/* The Active Directory tenant ID. You can find this in the Azure Dashboard under Home > Azure Active Directory. */
	private static String tenantId = "PASTE_YOUR_TENANT_ID_HERE";

	/* The name of the Azure resource group in which you want to create the resource.
	You can find resource groups in the Azure Dashboard under Home > Resource groups. */
	private static String resourceGroupName = "PASTE_YOUR_RESOURCE_GROUP_NAME_HERE";

	/* The name of the custom subdomain to use when you create the resource. This is optional.
	For example, if you create a Bing Search v7 resource with the custom subdomain name 'my-search-resource',
	your resource would have the endpoint https://my-search-resource.cognitiveservices.azure.com/.
	Note not all Cognitive Services allow custom subdomain names. */
	private static String subDomainName = "PASTE_YOUR_SUBDOMAIN_NAME_HERE";
	// </snippet_constants>

	public static void main(String[] args) {

		// <snippet_auth>
		/* For more information see:
		https://github.com/Azure/azure-sdk-for-java/blob/main/sdk/resourcemanager/docs/AUTH.md
		*/

		ClientSecretCredential credential = new ClientSecretCredentialBuilder()
			.clientId(applicationId)
			.clientSecret(applicationSecret)
			.tenantId(tenantId)
			.build();
		AzureProfile profile = new AzureProfile(tenantId, subscriptionId, AzureEnvironment.AZURE);

		CognitiveServicesManager client = CognitiveServicesManager.authenticate(credential, profile);
		// </snippet_auth>

		// <snippet_calls>
		String resourceName = "test_resource";
		String resourceKind = "TextTranslation";
		String resourceSku = "F0";
		Region resourceRegion = Region.US_WEST;

		// Uncomment to list all available resource kinds, SKUs, and locations for your Azure account.
		// list_available_kinds_skus_locations (client);

		// Create a resource with kind Text Translation, SKU F0 (free tier), location US West.
		String resourceId = create_resource (client, resourceName, resourceGroupName, resourceKind, resourceSku, resourceRegion);

		// Uncomment this to list all resources for your Azure account.
		// list_resources (client, resourceGroupName);

		// Delete the resource.
		delete_resource (client, resourceId);

		/* NOTE: When you delete a resource, it is only soft-deleted. You must also purge it. Otherwise, if you try to create another
		resource with the same name or custom subdomain, you will receive an error stating that such a resource already exists. */
		purge_resource (client, resourceName, resourceGroupName, resourceRegion);
		// </snippet_calls>
	}

	// <snippet_list_avail>
	public static void list_available_kinds_skus_locations (CognitiveServicesManager client) {
		System.out.println ("Available SKUs:");
		System.out.println("Kind\tSKU Name\tSKU Tier\tLocations");
		ResourceSkus skus = client.resourceSkus();
		for (ResourceSku sku : skus.list()) {
			String locations = String.join (",", sku.locations());
			System.out.println (sku.kind() + "\t" + sku.name() + "\t" + sku.tier() + "\t" + locations);
		}
	}
	// </snippet_list_avail>

	// Note: Region values are listed in:
	// https://github.com/Azure/azure-sdk-for-java/blob/main/sdk/core/azure-core-management/src/main/java/com/azure/core/management/Region.java
	// <snippet_create>
	public static String create_resource (CognitiveServicesManager client, String resourceName, String resourceGroupName, String resourceKind, String resourceSku, Region resourceRegion) {
		System.out.println ("Creating resource: " + resourceName + "...");

		/* NOTE: If you do not want to use a custom subdomain name, remove the withCustomSubDomainName
		setter from the AccountProperties object. */
		Account result = client.accounts().define(resourceName)
			.withExistingResourceGroup(resourceGroupName)
			// Note: Do not call withRegion() first, as it does not exist on the Blank interface returned by define().
			.withRegion(resourceRegion)
			.withKind(resourceKind)
			.withSku(new Sku().withName(resourceSku))
			.withProperties(new AccountProperties().withCustomSubDomainName(subDomainName))
			.create();

		System.out.println ("Resource created.");
		System.out.println ("ID: " + result.id());
		System.out.println ("Provisioning state: " + result.properties().provisioningState().toString());
		System.out.println ();

		return result.id();
	}
	// </snippet_create>

	// <snippet_list>
	public static void list_resources (CognitiveServicesManager client, String resourceGroupName) {
		System.out.println ("Resources in resource group: " + resourceGroupName);
		// Note Azure resources are also sometimes referred to as accounts.
		Accounts accounts = client.accounts();
		for (Account account : accounts.listByResourceGroup(resourceGroupName)) {
			System.out.println ("ID: " + account.id());
			System.out.println ("Kind: " + account.kind ());
			System.out.println ("SKU Name: " + account.sku().name());
			System.out.println ("Custom subdomain name: " + account.properties().customSubDomainName());
			System.out.println ();
		}
	}
	// </snippet_list>
	
	// <snippet_delete>
	public static void delete_resource (CognitiveServicesManager client, String resourceId) {
		System.out.println ("Deleting resource: " + resourceId + "...");
		client.accounts().deleteById (resourceId);
		System.out.println ("Resource deleted.");
		System.out.println ();
	}
	// </snippet_delete>

	// <snippet_purge>
	public static void purge_resource (CognitiveServicesManager client, String resourceName, String resourceGroupName, Region resourceRegion) {
		System.out.println ("Purging resource: " + resourceName + "...");
		client.deletedAccounts().purge(resourceRegion.toString(), resourceGroupName, resourceName, null);
		System.out.println ("Resource purged.");
		System.out.println ();		
	}
	// </snippet_purge>
}
