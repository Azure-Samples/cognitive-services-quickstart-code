# Personalizer Multislot simulation in an Azure notebook

This tutorial simulates a Multislot Personalizer loop _system_ which suggests which products a customer should buy when displayed in different slots. The users and their preferences are store in a [user dataset](simulated_users.json). Information about the products is also available in a [product dataset](products.json). Information about the slots is also available in a [slot dataset](slots.json).

Run the system for 25,000 requests and then create graph showing how fast and accurately the system learned. 

Run an offline counterfactual evaluation to select an optimized learning policy, and apply that policy.

Run the system again, but for 5,000 requests and again create the graph showing the accuracy of the system.

## Prerequisites

* [Azure notebooks](https://notebooks.azure.com/) account
* [Personalizer resource](https://ms.portal.azure.com/#create/Microsoft.CognitiveServicesPersonalizer)

## How to use this sample

All the instructions are in the notebook. Here is an abbreviated explanation.

1. Create a new Azure notebook project.
1. Upload the files in this directory to the Azure notebook project. 
1. Open the MultislotPersonalizer.ipynb file and change the following values:

    * The value for `<your-resource-name>` in the `personalization_base_url` to the value for your Personalizer resource
    * The value for `<your-resource-key>` variable to one of the Personalizer resource keys. 

1. Run each cell from top to bottom. Wait until each cell is complete before running the next cell. 

## References

* [Tutorial on docs.microsoft.com](https://docs.microsoft.com/azure/cognitive-services/personalizer/tutorial-use-azure-notebook-generate-loop-data)