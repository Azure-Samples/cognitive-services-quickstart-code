# Quickstart output: JavaScript SDK v3.0 general document model

[Reference documentation](/javascript/api/@azure/ai-form-recognizer/?view=azure-node-preview&preserve-view=true) | [Library source code](https://github.com/Azure/azure-sdk-for-js/tree/@azure/ai-form-recognizer_4.0.0-beta.3/sdk/formrecognizer/ai-form-recognizer/) | [Package (npm)](https://www.npmjs.com/package/@azure/ai-form-recognizer/v/4.0.0-beta.3) | [Samples](https://github.com/Azure/azure-sdk-for-js/blob/main/sdk/formrecognizer/ai-form-recognizer/samples/v4-beta/javascript/README.md)

You can get started using the Azure Form Recognizer general document model with the [JavaScript programming language quickstart](https://docs.microsoft.com/azure/applied-ai-services/form-recognizer/quickstarts/try-v3-javascript-sdk#general-document-model). The general document model analyzes and extracts text, tables, structure, key-value pairs, and named entities from forms and documents. Here is the expected outcome from the general document model quickstart code:

## General document model output

Key-Value Pairs:

- Key  : "For the Quarterly Period Ended"

  Value: "March 31, 2020" (0.35)

- Key  : "From"

  Value: "1934" (0.119)

- Key  : "to"

  Value: "<undefined>" (0.317)

- Key  : "Commission File Number"

  Value: "001-37845" (0.87)

- Key  : "(I.R.S. ID)"

  Value: "91-1144442" (0.87)

- Key  : "Class"

  Value: "Common Stock, $0.00000625 par value per share" (0.748)

- Key  : "Outstanding as of April 24, 2020"

  Value: "7,583,440,247 shares" (0.838)

Entities:

- "$0.00000625" Quantity - Currency (0.8)

- "MSFT" Organization - <none> (0.99)

- "NASDAQ" Organization - StockExchange (0.99)

- "2.125%" Quantity - Percentage (0.8)

- "2021" DateTime - DateRange (0.8)

- "MSFT" Organization - <none> (0.99)

- "NASDAQ" Organization - StockExchange (0.99)

- "3.125%" Quantity - Percentage (0.8)

- "2028" DateTime - DateRange (0.8)

- "MSFT" Organization - <none> (0.99)

- "NASDAQ" Organization - StockExchange (0.99)

- "2.625%" Quantity - Percentage (0.8)

- "2033" DateTime - DateRange (0.8)

- "MSFT" Organization - <none> (0.99)

- "NASDAQ UNITED" Organization - <none> (0.97)

- "April 24, 2020" DateTime - Date (0.8)

- "$0.00000625" Quantity - Currency (0.8)

- "7,583,440,247" Quantity - Number (0.8)

- "Washington, D.C" Location - GPE (0.68)

- "20549" Quantity - Number (0.8)

- "10" Quantity - Number (0.8)

- "QUARTERLY" DateTime - Set (0.8)

- "13" Quantity - Number (0.8)

- "15" Quantity - Number (0.8)

- "1934" DateTime - DateRange (0.8)

- "Quarterly" DateTime - Set (0.8)

- "March 31, 2020" DateTime - Date (0.8)

- "13" Quantity - Number (0.8)

- "15" Quantity - Number (0.8)

- "1934" DateTime - DateRange (0.8)

- "MICROSOFT CORPORATION" Organization - <none> (0.91)

- "WASHINGTON" Location - GPE (0.73)

- "91-1144442 (I.R.S. ID) ONE MICROSOFT WAY, REDMOND, WASHINGTON 98052-6399 (425) 882-8080 www.microsoft.com/investor Securities registered pursuant to Section 12(b) of the Act: Securities registered pursuant to Section 12(g)" Quantity - Dimension (0.8)

- "ONE MICROSOFT WAY, REDMOND, WASHINGTON" Address - <none> (0.85)

- "www.microsoft.com/investor" URL - <none> (0.8)

- "1" Quantity - Number (0.8)

- "13" Quantity - Number (0.8)

- "15" Quantity - Number (0.8)

- "1934" DateTime - DateRange (0.8)

- "12 months" DateTime - Duration (0.8)

- "2" Quantity - Number (0.8)

- "past 90 days" DateTime - DateRange (0.8)

- "405" Quantity - Number (0.8)

- "232.405" Quantity - Number (0.8)

- "12 months" DateTime - Duration (0.8)

- "12b" Quantity - Number (0.8)

- "2" Quantity - Number (0.8)

- "13" Quantity - Number (0.8)

- "12b" Quantity - Number (0.8)

- "2" Quantity - Number (0.8)

  ---
