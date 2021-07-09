# <snippet_imports>
import time
from azure.identity import ClientSecretCredential
from azure.mgmt.cognitiveservices import CognitiveServicesManagementClient
from azure.mgmt.cognitiveservices.models import Account, Sku
# </snippet_imports>

# Microsoft Azure Management
#
# This script requires the following modules:
#	python -m pip install azure-mgmt-cognitiveservices
#	python -m pip install azure.identity
#
# SDK: https://docs.microsoft.com/en-us/python/api/azure-mgmt-cognitiveservices/azure.mgmt.cognitiveservices?view=azure-python 
#
# This script runs under Python 3.4 or later.
# The application ID and secret of the service principal you are using to connect to the Azure Management Service.

# To create a service principal with the Azure CLI, see:
# https://docs.microsoft.com/en-us/cli/azure/create-an-azure-service-principal-azure-cli?view=azure-cli-latest
# To install the Azure CLI, see:
# https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest

# To create a service principal with Azure PowerShell, see: 
# https://docs.microsoft.com/en-us/powershell/azure/create-azure-service-principal-azureps?view=azps-3.3.0
# To install Azure PowerShell, see:
# https://github.com/Azure/azure-powershell

# When you create a service principal, you will see it has both an ID and an application ID.
# For example, if you create a service principal with Azure PowerShell, it should look like the following:

# Secret                : System.Security.SecureString
# ServicePrincipalNames : {<application ID>, <application URL>}
# ApplicationId         : <application ID>
# ObjectType            : ServicePrincipal
# DisplayName           : <name>
# Id                    : <ID>
# Type                  :

# <snippet_constants>
# Be sure to use the service pricipal application ID, not simply the ID. 
service_principal_application_id = "PASTE_YOUR_SERVICE_PRINCIPAL_APPLICATION_ID_HERE"
service_principal_secret = "PASTE_YOUR_SERVICE_PRINCIPAL_SECRET_HERE"

# The ID of your Azure subscription. You can find this in the Azure Dashboard under Home > Subscriptions.
subscription_id = "PASTE_YOUR_SUBSCRIPTION_ID_HERE"

# The Active Directory tenant ID. You can find this in the Azure Dashboard under Home > Azure Active Directory.
tenant_id = "PASTE_YOUR_TENANT_ID_HERE"

# The name of the Azure resource group in which you want to create the resource.
# You can find resource groups in the Azure Dashboard under Home > Resource groups.
resource_group_name = "PASTE_YOUR_RESOURCE_GROUP_NAME_HERE"

# The name of the custom subdomain to use when you create the resource. This is optional.
# For example, if you create a Bing Search v7 resource with the custom subdomain name 'my-search-resource',
# your resource would have the endpoint https://my-search-resource.cognitiveservices.azure.com/.
# Note not all Cognitive Services allow custom subdomain names.
subdomain_name = "PASTE_YOUR_SUBDOMAIN_NAME_HERE"

# How many seconds to wait between checking the status of an async operation.
wait_time = 10
# </snippet_constants>

# <snippet_auth>
credential = ClientSecretCredential(tenant_id, service_principal_application_id, service_principal_secret)
client = CognitiveServicesManagementClient(credential, subscription_id)
# </snippet_auth>

# <snippet_list_avail>
def list_available_kinds_skus_locations():
	print("Available SKUs:")
	result = client.resource_skus.list()
	print("Kind\tSKU Name\tSKU Tier\tLocations")
	for x in result:
		locations = ",".join(x.locations)
		print(x.kind + "\t" + x.name + "\t" + x.tier + "\t" + locations)
# </snippet_list_avail>

# Note Azure resources are also sometimes referred to as accounts.

# <snippet_list>
def list_resources():
	print("Resources in resource group: " + resource_group_name)
	result = client.accounts.list_by_resource_group(resource_group_name)
	for x in result:
		print(x.name)
		print(x)
		print()
# </snippet_list>

# <snippet_create>
def create_resource (resource_name, kind, sku_name, location) :
	print("Creating resource: " + resource_name + "...")

# NOTE If you do not want to use a custom subdomain name, remove the customSubDomainName
# property from the properties object.
	parameters = Account(sku=Sku(name=sku_name), kind=kind, location=location, properties={ 'custom_sub_domain_name' : subdomain_name })

	poller = client.accounts.begin_create(resource_group_name, resource_name, parameters)
	while (False == poller.done ()) :
		print ("Waiting {wait_time} seconds for operation to finish.".format (wait_time = wait_time))
		time.sleep (wait_time)
# This will raise an exception if the server responded with an error.
	result = poller.result ()

	print("Resource created.")
	print()
	print("ID: " + result.id)
	print("Name: " + result.name)
	print("Type: " + result.type)
	print()
# </snippet_create>

# <snippet_delete>
def delete_resource(resource_name) :
	print("Deleting resource: " + resource_name + "...")

	poller = client.accounts.begin_delete(resource_group_name, resource_name)
	while (False == poller.done ()) :
		print ("Waiting {wait_time} seconds for operation to finish.".format (wait_time = wait_time))
		time.sleep (wait_time)
# This will raise an exception if the server responded with an error.
	result = poller.result ()

	print("Resource deleted.")
# </snippet_delete>

# <snippet_purge>
def purge_resource(resource_name, location) :
	poller = client.deleted_accounts.begin_purge(location, resource_group_name, resource_name)
	while (False == poller.done ()) :
		print ("Waiting {wait_time} seconds for operation to finish.".format (wait_time = wait_time))
		time.sleep (wait_time)
# This will raise an exception if the server responded with an error.
	result = poller.result ()

	print("Resource purged.")
# </snippet_purge>

# <snippet_calls>
resource_name = "test_resource"
resource_kind = "TextTranslation"
resource_sku = "F0"
resource_location = "Global"

# Uncomment this to list all available resource kinds, SKUs, and locations for your Azure account.
#list_available_kinds_skus_locations ()

# Create a resource with kind Text Translation, SKU F0 (free tier), location global.
create_resource(resource_name, resource_kind, resource_sku, resource_location)

# Uncomment this to list all resources for your Azure account.
#list_resources()

# Delete the resource.
delete_resource(resource_name)

# NOTE: Deleting a resource only soft-deletes it. To delete it permanently, you must purge it.
# Otherwise, if you later try to create a resource with the same name, you will receive the following error:
# azure.core.exceptions.ResourceExistsError: (FlagMustBeSetForRestore) An existing resource with ID '<your resource ID>' has been soft-deleted. To restore the resource, you must specify 'restore' to be 'true' in the property. If you don't want to restore existing resource, please purge it first.
# Code: FlagMustBeSetForRestore

# Purge the resource.
purge_resource(resource_name, resource_location)

# </snippet_calls>
