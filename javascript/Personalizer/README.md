# About this Quickstart

This interactive sample takes the time of day and the users's taste preference as context, and sends it to an Azure Personalizer instance, which then returns the top personalized food choice, along with the recommendation probability distribution of each food item. The user then inputs whether or not Personalizer predicted correctly, which is data used to improve Personalizer's prediction model.

# To try this sample

## Prerequisites

The solution is a Node.js app, you will need [NPM](https://www.npmjs.com/) or an equivalent to install the packages and run the sample file.

## Set up the sample

- Clone the Azure Personalizer Samples repo.

    ```bash
    git clone https://github.com/Azure-Samples/cognitive-services-quickstart-code
    ```

- Navigate to _javascript/Personalizer_.

- Open `sample.js` for editing.

## Set up Azure Personalizer Service

- Create a Personalizer instance in the Azure portal.

- Set your environment variables `serviceKey` and `baseUri`. These values can be found in your Cognitive Services Quick start tab in the Azure portal.

## Run the sample

Build and run the sample with `npm start` at the CLI or terminal. The app will take input from the user interactively and send the data to the Personalizer instance.
