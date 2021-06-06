# Python 3.x quickstart for multi-slot Personalizer

Multi-slot personalization (Preview) allows you to target content in web layouts, carousels, and lists where more than one action (such as a product or piece of content) is shown to your users. With Personalizer multi-slot APIs, you can have the AI models in Personalizer learn what user contexts and products drive certain behaviors, considering and learning from the placement in your user interface. For example, Personalizer may learn that certain products or content drive more clicks as a sidebar or a footer than as a main highlight on a page.

This sample asks for the time of day and device type to determine which items to display on a retail app/website. You can select if that top choice is what you would pick.

## Upgrade Persoanlizer instance to multi-slot

 1. Configure your Personalizer instance for multi-slot (see [Setting up](https://docs.microsoft.com/en-us/azure/cognitive-services/personalizer/how-to-multi-slot?pivots=programming-language-python))

## Run samples

- You can find your key and endpoint in the resource's key and endpoint page, under resource management (Keys and Endpoint).

1. Update PERSONALIZATION_BASE_URL value ("<REPLACE-WITH-YOUR-PERSONALIZER-ENDPOINT>") in sample.py with the endpoint specific to your Personalizer service instance.

1. Update RESOURCE_KEY value ("<REPLACE-WITH-YOUR-PERSONALIZER-KEY>") in sample.py with the key specific to your Personalizer service instance.

1. Run app with command:

    ```
    python sample.py
    ```