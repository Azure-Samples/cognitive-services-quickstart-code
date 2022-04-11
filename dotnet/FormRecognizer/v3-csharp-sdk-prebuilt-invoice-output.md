# Quickstart output: C# SDK prebuilt invoice model (beta)

[Reference documentation](https://docs.microsoft.com/dotnet/api/azure.ai.formrecognizer.documentanalysis?view=azure-dotnet-preview&preserve-view=true) | [Library Source Code](https://github.com/Azure/azure-sdk-for-net/tree/Azure.AI.FormRecognizer_4.0.0-beta.3/sdk/formrecognizer/Azure.AI.FormRecognizer/) | [Package (NuGet)](https://www.nuget.org/packages/Azure.AI.FormRecognizer/4.0.0-beta.3) | [Samples](https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/formrecognizer/Azure.AI.FormRecognizer/samples/README.md)

You can get started using the Azure Form Recognizer prebuilt model with the [C# programming language quickstart](https://docs.microsoft.com/azure/applied-ai-services/form-recognizer/quickstarts/try-v3-csharp-sdk#prebuilt-model). The prebuilt model analyzes and extracts common fields from specific document types using a prebuilt model. Here is the expected outcome from the prebult invoice model quickstart code:

## Prebuilt invoice model output

Document 0:

Vendor Name: 'CONTOSO LTD.', with confidence 0.962

Customer Name: 'MICROSOFT CORPORATION', with confidence 0.951

Item:

  Description: 'Test for 23 fields', with confidence 0.899

  Amount: '100', with confidence 0.902

Sub Total: '100', with confidence 0.979

Total Tax: '10', with confidence 0.979

Invoice Total: '110', with confidence 0.973
