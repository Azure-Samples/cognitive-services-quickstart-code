To install the Azure CLI, see:
https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest

Open a command prompt and browse to the folder where you installed the Azure CLI, then to the wbin folder. For example:

```
cd C:\Program Files (x86)\Microsoft SDKs\Azure\CLI2\wbin\
```

Run:
```
az login
```

The Azure CLI opens a page in your default browser where you can log into Azure. Once you have logged in, the Azure CLI prints a list of subscriptions to which you have access.

To get an Azure access token, run:

```
az account get-access-token
```

The Azure CLI prints output similar to the following:
```
{
  "accessToken": "eyJ0...",
  "expiresOn": "2020-01-22 11:57:42.441401",
  "subscription": "<Your Azure subscription ID>",
  "tenant": "<Your Azure Active Directory Tenant ID>",
  "tokenType": "Bearer"
}
```

Copy the value of the access token and paste it into the following curl commands in place of <token>. To copy from a command prompt, left click and drag to select the text to copy, then press Enter to copy the text to the clipboard.

To get a list of subscriptions to which your Azure account has access, run the following command.
- Replace <subscription ID> with your Azure subscription ID. You can find this in the Azure Dashboard under Home > Subscriptions.
- Replace <token> with your Azure access token.

```
curl https://management.azure.com/subscriptions/<subscription ID>/providers/Microsoft.CognitiveServices/accounts?api-version=2017-04-18 -H "Authorization: Bearer <token>"
```

For more information see:
https://docs.microsoft.com/en-us/rest/api/cognitiveservices/accountmanagement/accounts/list

To list all available resource kinds, SKUs, and locations for your Azure account, run the following command.
- Replace <subscription ID> with your Azure subscription ID. You can find this in the Azure Dashboard under Home > Subscriptions.
- Replace <token> with your Azure access token.

```
curl https://management.azure.com/subscriptions/<subscription ID>/providers/Microsoft.CognitiveServices/skus?api-version=2017-04-18 -H "Authorization: Bearer <token>"
```

For more information see:
https://docs.microsoft.com/en-us/rest/api/cognitiveservices/accountmanagement/resourceskus/list

To create a new resource, run the following command.
- Replace <subscription ID> with your Azure subscription ID. You can find this in the Azure Dashboard under Home > Subscriptions.
- Replace <resource group name> with the name of the Azure resource group in which you wish to create the resource. You can find resource groups in the Azure Dashboard under Home > Resource groups.
- Replace <account name> with the name of the new resource. Note Azure resources are also sometimes referred to as accounts.
- Replace <resource type> with the resource type of the new resource (for example, "Text Translation").
- Replace <location> with the location of the new resource (for example, "westus" or "global").
- Replace <sku name> with the subscription tier of the new resource (for example, "F0").
- Replace <token> with your Azure access token.

```
curl https://management.azure.com/subscriptions/<subscription ID>/resourceGroups/<resource group name>/providers/Microsoft.CognitiveServices/accounts/<account name>?api-version=2017-04-18 -X PUT -H "Content-Type: application/json" -d "{ 'kind' : '<resource type>', 'location' : '<location>', 'sku' : { 'name' : '<sku name>' }}" -H "Authorization: Bearer <token>"
```

For example, the following creates a resource with kind Text Translation, SKU F0 (free tier), location global. You will still need to replace <subscription ID>, <resource group name>, <account name>, and <token>.
```
curl https://management.azure.com/subscriptions/<subscription ID>/resourceGroups/<resource group name>/providers/Microsoft.CognitiveServices/accounts/<account name>?api-version=2017-04-18 -X PUT -H "Content-Type: application/json" -d "{ 'kind' : 'TextTranslation', 'location' : 'Global', 'sku' : { 'name' : 'F0' }}" -H "Authorization: Bearer <token>"
```

For more information see:
https://docs.microsoft.com/en-us/rest/api/cognitiveservices/accountmanagement/accounts/create

To delete a resource, run the following command.
- Replace <subscription ID> with your Azure subscription ID. You can find this in the Azure Dashboard under Home > Subscriptions.
- Replace <resource group name> with the name of the Azure resource group from which you wish to delete the resource. You can find resource groups in the Azure Dashboard under Home > Resource groups.
- Replace <account name> with the name of the resource to delete. Note Azure resources are also sometimes referred to as accounts.
- Replace <token> with your Azure access token.

```
curl https://management.azure.com/subscriptions/<subscription ID>/resourceGroups/<resource group name>/providers/Microsoft.CognitiveServices/accounts/<account name>?api-version=2017-04-18 -X DELETE -H "Authorization: Bearer <token>"
```

For more information see:
https://docs.microsoft.com/en-us/rest/api/cognitiveservices/accountmanagement/accounts/delete
