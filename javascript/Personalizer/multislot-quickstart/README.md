# About this Quickstart

Multi-slot personalization (Preview) allows you to target content in web layouts, carousels, and lists where more than one action (such as a product or piece of content) is shown to your users. With Personalizer multi-slot APIs, you can have the AI models in Personalizer learn what user contexts and products drive certain behaviors, considering and learning from the placement in your user interface. For example, Personalizer may learn that certain products or content drive more clicks as a sidebar or a footer than as a main highlight on a page.

This sample asks for the time of day and device type to determine which items to display on a retail app/website. You can select if that top choice is what you would pick.

# To try this sample

## Prerequisites
The solution is a [Node.js](https://nodejs.org) app, you will need [NPM](https://www.npmjs.com/) or an equivalent to install the packages and run the sample file (verified with Node.js v14.16.0 and NPM 6.14.11).

## Upgrade Persoanlizer instance to multi-slot

 1. Configure your Personalizer instance for multi-slot (see [Setting up](https://docs.microsoft.com/en-us/azure/cognitive-services/personalizer/how-to-multi-slot?pivots=programming-language-javascript))

## Set up the sample

- Clone the Azure Personalizer Samples repo.

    ```bash
    git clone https://github.com/Azure-Samples/cognitive-services-quickstart-code
    ```

- Navigate to _javascript/Personalizer/multislot-quickstart_.

- Run npm init -y command to create a package.json file.
    ```bash
    npm init -y
    ```

- Install the Npm packages for the quickstart
    ```bash
    npm install readline-sync uuid axios --save
    ```

- Open `sample.js` for editing.

## Set up Azure Personalizer Service

- Create a Personalizer instance in the Azure portal.

- You can find your key and endpoint in the resource's key and endpoint page, under resource management (Keys and Endpoint).

1. Update PersonalizationBaseUrl value ("<REPLACE-WITH-YOUR-PERSONALIZER-ENDPOINT>") in sample.js with the endpoint specific to your Personalizer service instance.

1. Update ResourceKey value ("<REPLACE-WITH-YOUR-PERSONALIZER-KEY>") in sample.js with the key specific to your Personalizer service instance.

## Run the sample

Build and run the sample with `node sample.js` at the CLI or terminal. The app will take input from the user interactively and send the data to the Personalizer instance.
