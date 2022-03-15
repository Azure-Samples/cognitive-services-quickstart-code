# Quickstart output: Python SDK v3.0 prebuilt-invoice model

[Reference documentation](/python/api/azure-ai-formrecognizer/azure.ai.formrecognizer?view=azure-python-preview&preserve-view=true) | [Library source code](https://github.com/Azure/azure-sdk-for-python/tree/azure-ai-formrecognizer_3.2.0b3/sdk/formrecognizer/azure-ai-formrecognizer/) | [Package (PyPi)](https://pypi.org/project/azure-ai-formrecognizer/3.2.0b3/) | [Samples](https://github.com/Azure/azure-sdk-for-python/blob/azure-ai-formrecognizer_3.2.0b3/sdk/formrecognizer/azure-ai-formrecognizer/samples/README.md)

You can get started using the Azure Form Recognizer layout model with the [Python programming language quickstart](https://docs.microsoft.com/azure/applied-ai-services/form-recognizer/quickstarts/try-v3-python-sdk#prebuilt-model). The layout model analyzes and extracts tables, lines, words, and selection marks like radio buttons and check boxes from forms and documents, without the need to train a model. Here is the expected outcome from the prebuilt-invoice model quickstart code:

## Prebuilt-invoice output

--------Recognizing invoice # 1--------

Vendor Name: CONTOSO LTD. has confidence: 0.919

Vendor Address: 123 456th St New York, NY, 10001 has confidence: 0.907

Vendor Address Recipient: Contoso Headquarters has confidence: 0.919

Customer Name: MICROSOFT CORPORATION has confidence: 0.84

Customer Id: CID-12345 has confidence: 0.956

Customer Address: 123 Other St, Redmond WA, 98052 has confidence: 0.909

Customer Address Recipient: Microsoft Corp has confidence: 0.917

Invoice Id: INV-100 has confidence: 0.972

Invoice Date: 2019-11-15 has confidence: 0.971

Invoice Total: CurrencyValue(amount=110.0, symbol=$) has confidence: 0.97

Due Date: 2019-12-15 has confidence: 0.973

Purchase Order: PO-3333 has confidence: 0.957

Billing Address: 123 Bill St, Redmond WA, 98052 has confidence: 0.91

Billing Address Recipient: Microsoft Finance has confidence: 0.919

Shipping Address: 123 Ship St, Redmond WA, 98052 has confidence: 0.909

Shipping Address Recipient: Microsoft Delivery has confidence: 0.912

Invoice items:

...Item #1

......Description: Test for 23 fields has confidence: 0.93

......Quantity: 1.0 has confidence: 0.968

......Unit Price: CurrencyValue(amount=1.0, symbol=None) has confidence: 0.872

......Amount: CurrencyValue(amount=100.0, symbol=$) has confidence: 0.983

Subtotal: CurrencyValue(amount=100.0, symbol=$) has confidence: 0.974

Total Tax: CurrencyValue(amount=10.0, symbol=$) has confidence: 0.973

Previous Unpaid Balance: CurrencyValue(amount=500.0, symbol=$) has confidence: 0.956

Amount Due: CurrencyValue(amount=610.0, symbol=$) has confidence: 0.703

Service Address: 123 Service St, Redmond WA, 98052 has confidence: 0.908

Service Address Recipient: Microsoft Services has confidence: 0.914

Remittance Address: 123 Remit St New York, NY, 10001 has confidence: 0.908

Remittance Address Recipient: Contoso Billing has confidence: 0.919

---

