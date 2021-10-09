// <snippet_imports>
const { FormRecognizerClient, FormTrainingClient, AzureKeyCredential } = require("@azure/ai-form-recognizer");
const fs = require("fs");
// </snippet_imports>

// <snippet_creds>
const apiKey = "PASTE_YOUR_FORM_RECOGNIZER_SUBSCRIPTION_KEY_HERE";
const endpoint = "PASTE_YOUR_FORM_RECOGNIZER_ENDPOINT_HERE";
// </snippet_creds>

// <snippet_auth>
const trainingClient = new FormTrainingClient(endpoint, new AzureKeyCredential(apiKey));
const client = new FormRecognizerClient(endpoint, new AzureKeyCredential(apiKey));
// </snippet_auth>

// <snippet_getcontent>
async function recognizeContent() {
    const formUrl = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-REST-api-samples/master/curl/form-recognizer/simple-invoice.png";
    const poller = await client.beginRecognizeContentFromUrl(formUrl);
    const pages = await poller.pollUntilDone();

    if (!pages || pages.length === 0) {
        throw new Error("Expecting non-empty list of pages!");
    }

    for (const page of pages) {
        console.log(
            `Page ${page.pageNumber}: width ${page.width} and height ${page.height} with unit ${page.unit}`
        );
        for (const table of page.tables) {
            for (const cell of table.cells) {
                console.log(`cell [${cell.rowIndex},${cell.columnIndex}] has text ${cell.text}`);
            }
        }
    }
}

recognizeContent().catch((err) => {
    console.error("The sample encountered an error:", err);
});
// </snippet_getcontent>

// <snippet_receipts>
async function recognizeReceipt() {
    receiptUrl = "https://raw.githubusercontent.com/Azure/azure-sdk-for-python/master/sdk/formrecognizer/azure-ai-formrecognizer/tests/sample_forms/receipt/contoso-receipt.png";
    const poller = await client.beginRecognizeReceiptsFromUrl(receiptUrl, {
        onProgress: (state) => { console.log(`status: ${state.status}`); }
    });

    const receipts = await poller.pollUntilDone();

    if (!receipts || receipts.length <= 0) {
        throw new Error("Expecting at lease one receipt in analysis result");
    }

    const receipt = receipts[0];
    console.log("First receipt:");
    const receiptTypeField = receipt.fields["ReceiptType"];
    if (receiptTypeField.valueType === "string") {
        console.log(`  Receipt Type: '${receiptTypeField.value || "<missing>"}', with confidence of ${receiptTypeField.confidence}`);
    }
    const merchantNameField = receipt.fields["MerchantName"];
    if (merchantNameField.valueType === "string") {
        console.log(`  Merchant Name: '${merchantNameField.value || "<missing>"}', with confidence of ${merchantNameField.confidence}`);
    }
    const transactionDate = receipt.fields["TransactionDate"];
    if (transactionDate.valueType === "date") {
        console.log(`  Transaction Date: '${transactionDate.value || "<missing>"}', with confidence of ${transactionDate.confidence}`);
    }
    const itemsField = receipt.fields["Items"];
    if (itemsField.valueType === "array") {
        for (const itemField of itemsField.value || []) {
            if (itemField.valueType === "object") {
                const itemNameField = itemField.value["Name"];
                if (itemNameField.valueType === "string") {
                    console.log(`    Item Name: '${itemNameField.value || "<missing>"}', with confidence of ${itemNameField.confidence}`);
                }
            }
        }
    }
    const totalField = receipt.fields["Total"];
    if (totalField.valueType === "number") {
        console.log(`  Total: '${totalField.value || "<missing>"}', with confidence of ${totalField.confidence}`);
    }
}

recognizeReceipt().catch((err) => {
    console.error("The sample encountered an error:", err);
});
// </snippet_receipts>

//<snippet_bc>
async function recognizeBusinessCards() {
    bcUrl = "https://github.com/Azure-Samples/cognitive-services-REST-api-samples/curl/form-recognizer/businessCard.png";
    const poller = await client.beginRecognizeBusinessCardsFromUrl(bcUrl, {
        onProgress: (state) => {
            console.log(`status: ${state.status}`);
        }
    });

    const [businessCard] = await poller.pollUntilDone();

    if (businessCard === undefined) {
        throw new Error("Failed to extract data from at least one business card.");
    }

    const contactNames = businessCard.fields["ContactNames"].value;
    if (Array.isArray(contactNames)) {
        console.log("- Contact Names:");
        for (const contactName of contactNames) {
            if (contactName.valueType === "object") {
                const firstName = contactName.value?.["FirstName"].value ?? "<no first name>";
                const lastName = contactName.value?.["LastName"].value ?? "<no last name>";
                console.log(`  - ${firstName} ${lastName} (${contactName.confidence} confidence)`);
            }
        }
    }

    printSimpleArrayField(businessCard, "CompanyNames");
    printSimpleArrayField(businessCard, "Departments");
    printSimpleArrayField(businessCard, "JobTitles");
    printSimpleArrayField(businessCard, "Emails");
    printSimpleArrayField(businessCard, "Websites");
    printSimpleArrayField(businessCard, "Addresses");
    printSimpleArrayField(businessCard, "MobilePhones");
    printSimpleArrayField(businessCard, "Faxes");
    printSimpleArrayField(businessCard, "WorkPhones");
    printSimpleArrayField(businessCard, "OtherPhones");
}

// Helper function to print array field values. 
function printSimpleArrayField(businessCard, fieldName) {
    const fieldValues = businessCard.fields[fieldName]?.value;
    if (Array.isArray(fieldValues)) {
        console.log(`- ${fieldName}:`);
        for (const item of fieldValues) {
            console.log(`  - ${item.value ?? "<no value>"} (${item.confidence} confidence)`);
        }
    } else if (fieldValues === undefined) {
        console.log(`No ${fieldName} were found in the document.`);
    } else {
        console.error(
            `Error: expected field "${fieldName}" to be an Array, but it was a(n) ${businessCard.fields[fieldName].valueType}`
        );
    }
}

recognizeBusinessCards().catch((err) => {
    console.error("The sample encountered an error:", err);
});

//</snippet_bc >

//<snippet_invoice>
async function recognizeInvoices() {
    invoiceUrl = "https://github.com/Azure-Samples/cognitive-services-REST-api-samples/curl/form-recognizer/invoice_sample.jpg";

    const poller = await client.beginRecognizeInvoicesFromUrl(invoiceUrl, {
        onProgress: (state) => {
            console.log(`status: ${state.status}`);
        }
    });

    const [invoice] = await poller.pollUntilDone();
    if (invoice === undefined) {
        throw new Error("Failed to extract data from at least one invoice.");
    }

    // Helper function to print fields.
    function fieldToString(field) {
        const {
            name,
            valueType,
            value,
            confidence
        } = field;
        return `${name} (${valueType}): '${value}' with confidence ${confidence}'`;
    }

    console.log("Invoice fields:");

    for (const [name, field] of Object.entries(invoice.fields)) {
        if (field.valueType !== "array" && field.valueType !== "object") {
            console.log(`- ${name} ${fieldToString(field)}`);
        }
    }

    let idx = 0;

    console.log("- Items:");

    const items = invoice.fields["Items"]?.value;
    for (const item of items ?? []) {
        const value = item.value;

        const subFields = [
            "Description",
            "Quantity",
            "Unit",
            "UnitPrice",
            "ProductCode",
            "Date",
            "Tax",
            "Amount"
        ]
            .map((fieldName) => value[fieldName])
            .filter((field) => field !== undefined);

        console.log(
            [
                `  - Item #${idx}`,
                // Now we will convert those fields into strings to display
                ...subFields.map((field) => `    - ${fieldToString(field)}`)
            ].join("\n")
        );
    }
}

recognizeInvoices().catch((err) => {
    console.error("The sample encountered an error:", err);
});
//</snippet_invoice >

//<snippet_id>
async function recognizeIdDocuments() {
    idUrl = "https://github.com/Azure-Samples/cognitive-services-REST-api-samples/curl/form-recognizer/id-license.jpg";
    const poller = await client.beginRecognizeIdDocumentsFromUrl(idUrl, {
        onProgress: (state) => {
            console.log(`status: ${state.status}`);
        }
    });

    const [idDocument] = await poller.pollUntilDone();

    if (idDocument === undefined) {
        throw new Error("Failed to extract data from at least one identity document.");
    }

    console.log("Document Type:", idDocument.formType);

    console.log("Identity Document Fields:");

    function printField(fieldName) {
        // Fields are extracted from the `fields` property of the document result
        const field = idDocument.fields[fieldName];
        console.log(
            `- ${fieldName} (${field?.valueType}): '${field?.value ?? "<missing>"}', with confidence ${field?.confidence
            }`
        );
    }

    printField("FirstName");
    printField("LastName");
    printField("DocumentNumber");
    printField("DateOfBirth");
    printField("DateOfExpiration");
    printField("Sex");
    printField("Address");
    printField("Country");
    printField("Region");
}

recognizeIdDocuments().catch((err) => {
    console.error("The sample encountered an error:", err);
});

//</snippet_id >

// <snippet_train>
async function trainModel() {

    const containerSasUrl = "<SAS-URL-of-your-form-folder-in-blob-storage>";

    const poller = await trainingClient.beginTraining(containerSasUrl, false, {
        onProgress: (state) => { console.log(`training status: ${state.status}`); }
    });
    const model = await poller.pollUntilDone();

    if (!model) {
        throw new Error("Expecting valid training result!");
    }

    console.log(`Model ID: ${model.modelId}`);
    console.log(`Status: ${model.status}`);
    console.log(`Training started on: ${model.trainingStartedOn}`);
    console.log(`Training completed on: ${model.trainingCompletedOn}`);

    if (model.submodels) {
        for (const submodel of model.submodels) {
            // since the training data is unlabeled, we are unable to return the accuracy of this model
            console.log("We have recognized the following fields");
            for (const key in submodel.fields) {
                const field = submodel.fields[key];
                console.log(`The model found field '${field.name}'`);
            }
        }
    }
    // Training document information
    if (model.trainingDocuments) {
        for (const doc of model.trainingDocuments) {
            console.log(`Document name: ${doc.name}`);
            console.log(`Document status: ${doc.status}`);
            console.log(`Document page count: ${doc.pageCount}`);
            console.log(`Document errors: ${doc.errors}`);
        }
    }
}

trainModel().catch((err) => {
    console.error("The sample encountered an error:", err);
});
// </snippet_train>

// <snippet_trainlabels>
async function trainModelLabels() {

    const containerSasUrl = "<SAS-URL-of-your-form-folder-in-blob-storage>";

    const poller = await trainingClient.beginTraining(containerSasUrl, true, {
        onProgress: (state) => { console.log(`training status: ${state.status}`); }
    });
    const model = await poller.pollUntilDone();

    if (!model) {
        throw new Error("Expecting valid training result!");
    }

    console.log(`Model ID: ${model.modelId}`);
    console.log(`Status: ${model.status}`);
    console.log(`Training started on: ${model.trainingStartedOn}`);
    console.log(`Training completed on: ${model.trainingCompletedOn}`);

    if (model.submodels) {
        for (const submodel of model.submodels) {
            // since the training data is unlabeled, we are unable to return the accuracy of this model
            console.log("We have recognized the following fields");
            for (const key in submodel.fields) {
                const field = submodel.fields[key];
                console.log(`The model found field '${field.name}'`);
            }
        }
    }
    // Training document information
    if (model.trainingDocuments) {
        for (const doc of model.trainingDocuments) {
            console.log(`Document name: ${doc.name}`);
            console.log(`Document status: ${doc.status}`);
            console.log(`Document page count: ${doc.pageCount}`);
            console.log(`Document errors: ${doc.errors}`);
        }
    }
}

trainModelLabels().catch((err) => {
    console.error("The sample encountered an error:", err);
});
// </snippet_trainlabels>

// <snippet_analyze>
async function recognizeCustom() {
    // Model ID from when you trained your model.
    const modelId = "<modelId>";
    const formUrl = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-REST-api-samples/master/curl/form-recognizer/simple-invoice.png";

    const poller = await client.beginRecognizeCustomForms(modelId, formUrl, {
        onProgress: (state) => { console.log(`status: ${state.status}`); }
    });
    const forms = await poller.pollUntilDone();

    console.log("Forms:");
    for (const form of forms || []) {
        console.log(`${form.formType}, page range: ${form.pageRange}`);
        console.log("Pages:");
        for (const page of form.pages || []) {
            console.log(`Page number: ${page.pageNumber}`);
            console.log("Tables");
            for (const table of page.tables || []) {
                for (const cell of table.cells) {
                    console.log(`cell (${cell.rowIndex},${cell.columnIndex}) ${cell.text}`);
                }
            }
        }

        console.log("Fields:");
        for (const fieldName in form.fields) {
            // each field is of type FormField
            const field = form.fields[fieldName];
            console.log(
                `Field ${fieldName} has value '${field.value}' with a confidence score of ${field.confidence}`
            );
        }
    }
}

recognizeCustom().catch((err) => {
    console.error("The sample encountered an error:", err);
});
// </snippet_analyze>

// <snippet_manage_count>
async function countModels() {
    // First, we see how many custom models we have, and what our limit is
    const accountProperties = await trainingClient.getAccountProperties();
    console.log(
        `Our account has ${accountProperties.customModelCount} custom models, and we can have at most ${accountProperties.customModelLimit} custom models`
    );
}
countModels().catch((err) => {
    console.error("The sample encountered an error:", err);
});
// </snippet_manage_count>

// <snippet_manage_list>
async function listModels() {

    // returns an async iteratable iterator that supports paging
    const result = trainingClient.listCustomModels();
    let i = 0;
    for await (const modelInfo of result) {
        console.log(`model ${i++}:`);
        console.log(modelInfo);
    }
}

listModels().catch((err) => {
    console.error("The sample encountered an error:", err);
});
// </snippet_manage_list>

// <snippet_manage_listids>
async function listModelIds() {
    // using `iter.next()`
    i = 1;
    let iter = trainingClient.listCustomModels();
    let modelItem = await iter.next();
    while (!modelItem.done) {
        console.log(`model ${i++}: ${modelItem.value.modelId}`);
        modelItem = await iter.next();
    }
}
// </snippet_manage_listids>

// <snippet_manage_listpages>
async function listModelsByPage() {
    // using `byPage()`
    i = 1;
    for await (const response of trainingClient.listCustomModels().byPage()) {
        for (const modelInfo of response.modelList) {
            console.log(`model ${i++}: ${modelInfo.modelId}`);
        }
    }
}
listModelsByPage().catch((err) => {
    console.error("The sample encountered an error:", err);
});
// </snippet_manage_listpages>

// <snippet_manage_getmodel>
async function getModel(modelId) {
    // Now we'll get the first custom model in the paged list
    const model = await client.getCustomModel(modelId);
    console.log("--- First Custom Model ---");
    console.log(`Model Id: ${model.modelId}`);
    console.log(`Status: ${model.status}`);
    console.log("Documents used in training:");
    for (const doc of model.trainingDocuments || []) {
        console.log(`- ${doc.name}`);
    }
}
// </snippet_manage_getmodel>

// <snippet_manage_delete>
async function deleteModel(modelId) {
    await client.deleteModel(modelId);
    try {
        const deleted = await client.getCustomModel(modelId);
        console.log(deleted);
    } catch (err) {
        // Expected
        console.log(`Model with id ${modelId} has been deleted`);
    }
}
// </snippet_manage_delete>

