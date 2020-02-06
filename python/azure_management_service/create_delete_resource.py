# Microsoft Azure Management
#
# This script requires the following modules:
#	python -m pip install azure-mgmt-cognitiveservices
#	python -m pip install msrestazure
#
# This script runs under Python 3.4 or later.

from msrestazure.azure_active_directory import ServicePrincipalCredentials
from azure.mgmt.cognitiveservices import CognitiveServicesManagementClient
from azure.mgmt.cognitiveservices.models import CognitiveServicesAccountCreateParameters, Sku

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

# Be sure to use the service pricipal application ID, not simply the ID. 

service_principal_application_id = "TODO_REPLACE"
service_principal_secret = "TODO_REPLACE"

# The ID of your Azure subscription. You can find this in the Azure Dashboard under Home > Subscriptions.
subscription_id = "TODO_REPLACE"

# The Active Directory tenant ID. You can find this in the Azure Dashboard under Home > Azure Active Directory.
tenant_id = "TODO_REPLACE"

# The name of the Azure resource group in which you want to create the resource.
# You can find resource groups in the Azure Dashboard under Home > Resource groups.
resource_group_name = "TODO_REPLACE"

credentials = ServicePrincipalCredentials (service_principal_application_id, service_principal_secret, tenant=tenant_id)
client = CognitiveServicesManagementClient (credentials, subscription_id)

def list_available_kinds_skus_locations () :
	print ("Available SKUs:")
	result = client.resource_skus.list ()
	print("Kind\tSKU Name\tSKU Tier\tLocations")
	for x in result:
		locations = ",".join (x.locations)
		print(x.kind + "\t" + x.name + "\t" + x.tier + "\t" + locations)

# Note Azure resources are also sometimes referred to as accounts.

def list_resources () :
	print ("Resources in resource group: " + resource_group_name)
	result = client.accounts.list_by_resource_group (resource_group_name)
	for x in result:
		print(x)
		print()

def create_resource (resource_name, kind, sku_name, location) :
	print ("Creating resource: " + resource_name + "...")
# The parameter "properties" must be an empty object.
	parameters = CognitiveServicesAccountCreateParameters (sku=Sku (name=sku_name), kind=kind, location=location, properties={})
	result = client.accounts.create (resource_group_name, resource_name, parameters)
	print ("Resource created.")
	print ("ID: " + result.id)
	print ("Provisioning state: " + result.provisioning_state)
	print ()

def delete_resource (resource_name) :
	print ("Deleting resource: " + resource_name + "...")
	client.accounts.delete (resource_group_name, resource_name)
	print ("Resource deleted.")
	print ()

# Uncomment this to list all resources for your Azure account.
#list_resources ()

# Uncomment this to list all available resource kinds, SKUs, and locations for your Azure account.
#list_available_kinds_skus_locations ()

# Create a resource with kind Text Translation, SKU F0 (free tier), location global.
create_resource ("test_resource", "TextTranslation", "F0", "Global")

# Delete the resource.
delete_resource ("test_resource")
