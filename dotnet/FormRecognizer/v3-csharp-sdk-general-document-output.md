# Quickstart output: Form Recognizer C# SDK v3.0 general document model

[Reference documentation](https://docs.microsoft.com/dotnet/api/azure.ai.formrecognizer.documentanalysis?view=azure-dotnet-preview&preserve-view=true) | [Library Source Code](https://github.com/Azure/azure-sdk-for-net/tree/Azure.AI.FormRecognizer_4.0.0-beta.3/sdk/formrecognizer/Azure.AI.FormRecognizer/) | [Package (NuGet)](https://www.nuget.org/packages/Azure.AI.FormRecognizer/4.0.0-beta.3) | [Samples](https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/formrecognizer/Azure.AI.FormRecognizer/samples/README.md)

You can get started using the Azure Form Recognizer general document model with the [C# programming language quickstart](quickstarts/try-v3-csharp-sdk.md#general-document-model). The general document model analyzes and extracts text, tables, structure, key-value pairs, and named entities from forms a documents. Here is the expected outcome from the general document model quickstart code:

## General document model output
  

Detected key-value pairs:

  Found key with no value: '?'

  Found key-value pair: 'QUARTERLY REPORT PURSUANT TO SECTION 13 OR 15(d) OF THE SECURITIES EXCHANGE ACT OF 1934' and ':selected:'

  Found key-value pair: 'For the Quarterly Period Ended March 31, 2020' and 'OR'

  Found key with no value: '?'

  Found key-value pair: 'TRANSITION REPORT PURSUANT TO SECTION 13 OR 15(d) OF THE SECURITIES EXCHANGE ACT OF 1934' and ':unselected:'

  Found key with no value: 'For the Transition Period From'

  Found key-value pair: 'to Commission File Number' and '001-37845'

  Found key-value pair: '(I.R.S. ID)' and '91-1144442'

  Found key-value pair: 'Securities registered pursuant to Section 12(g) of the Act:' and 'NONE'

  Found key-value pair: 'Yes ?' and ':selected:'

  Found key-value pair: 'No ?' and ':unselected:'

  Found key-value pair: 'Yes ?' and ':selected:'

  Found key-value pair: 'No ?' and ':unselected:'

  Found key-value pair: 'Large accelerated filer ?' and ':selected:'

  Found key-value pair: 'Accelerated filer ?' and ':unselected:'

  Found key-value pair: 'Non-accelerated filer ?' and ':unselected:'

  Found key with no value: 'Smaller reporting company'

  Found key with no value: '?'

  Found key-value pair: 'Emerging growth company' and ':unselected:'

  Found key with no value: '?'

  Found key with no value: '?'

  Found key-value pair: 'No ?' and ':selected:'

Detected entities:

  Found entity '$0.00000625' with category 'Quantity' and sub-category 'Currency'.

  Found entity 'MSFT' with category 'Organization'.

  Found entity 'NASDAQ' with category 'Organization' and sub-category 'StockExchange'.

  Found entity '2.125%' with category 'Quantity' and sub-category 'Percentage'.

  Found entity '2021' with category 'DateTime' and sub-category 'DateRange'.

  Found entity 'MSFT' with category 'Organization'.

  Found entity 'NASDAQ' with category 'Organization' and sub-category 'StockExchange'.

  Found entity '3.125%' with category 'Quantity' and sub-category 'Percentage'.

  Found entity '2028' with category 'DateTime' and sub-category 'DateRange'.

  Found entity 'MSFT' with category 'Organization'.

  Found entity 'NASDAQ' with category 'Organization' and sub-category 'StockExchange'.

  Found entity '2.625%' with category 'Quantity' and sub-category 'Percentage'.

  Found entity '2033' with category 'DateTime' and sub-category 'DateRange'.

  Found entity 'MSFT' with category 'Organization'.

  Found entity 'NASDAQ' with category 'Organization' and sub-category 'StockExchange'.

  Found entity 'April 24, 2020' with category 'DateTime' and sub-category 'Date'.

  Found entity '$0.00000625' with category 'Quantity' and sub-category 'Currency'.

  Found entity '7,583,440,247' with category 'Quantity' and sub-category 'Number'.

  Found entity 'UNITED STATES' with category 'Location' and sub-category 'GPE'.

  Found entity 'EXCHANGE' with category 'Organization' and sub-category 'Medical'.

  Found entity 'Washington, D.C.' with category 'Location' and sub-category 'GPE'.

  Found entity '20549' with category 'Quantity' and sub-category 'Number'.

  Found entity '10' with category 'Quantity' and sub-category 'Number'.

  Found entity 'QUARTERLY' with category 'DateTime' and sub-category 'Set'.

  Found entity '13' with category 'Quantity' and sub-category 'Number'.

  Found entity '15' with category 'Quantity' and sub-category 'Number'.

  Found entity '1934' with category 'DateTime' and sub-category 'DateRange'.

  Found entity 'Quarterly' with category 'DateTime' and sub-category 'Set'.

  Found entity 'March 31, 2020' with category 'DateTime' and sub-category 'Date'.

  Found entity '13' with category 'Quantity' and sub-category 'Number'.

  Found entity '15' with category 'Quantity' and sub-category 'Number'.

  Found entity '1934' with category 'DateTime' and sub-category 'DateRange'.

  Found entity 'MICROSOFT CORPORATION' with category 'Organization'.

  Found entity 'WASHINGTON' with category 'Location' and sub-category 'GPE'.

  Found entity 'ONE MICROSOFT WAY' with category 'Address'.

  Found entity 'ONE' with category 'Quantity' and sub-category 'Number'.

  Found entity 'REDMOND, WASHINGTON' with category 'Address'.

  Found entity '98052-6399

 (425) 882-8080

 www.microsoft.com/investor

 Securities registered pursuant to Section 12(b) of the Act:

 Securities registered pursuant to Section 12(g)' with category 'Quantity' and sub-category 'Dimension'.

  Found entity 'www.microsoft.com/investor' with category 'URL'.

  Found entity '1' with category 'Quantity' and sub-category 'Number'.

  Found entity '13' with category 'Quantity' and sub-category 'Number'.

  Found entity '15' with category 'Quantity' and sub-category 'Number'.

  Found entity '1934' with category 'DateTime' and sub-category 'DateRange'.

  Found entity '12 months' with category 'DateTime' and sub-category 'Duration'.

  Found entity '2' with category 'Quantity' and sub-category 'Number'.

  Found entity '90 days' with category 'DateTime' and sub-category 'Duration'.

  Found entity '405' with category 'Quantity' and sub-category 'Number'.

  Found entity '232.405' with category 'Quantity' and sub-category 'Number'.

  Found entity '12 months' with category 'DateTime' and sub-category 'Duration'.

  Found entity '12b' with category 'Quantity' and sub-category 'Number'.

  Found entity '2' with category 'Quantity' and sub-category 'Number'.

  Found entity '13' with category 'Quantity' and sub-category 'Number'.

  Found entity '12b' with category 'Quantity' and sub-category 'Number'.

  Found entity '2' with category 'Quantity' and sub-category 'Number'.

Document Page 1 has 69 line(s), 425 word(s),

and 14 selection mark(s).

  Line 0 has content: 'UNITED STATES'.

    Its bounding box is:

      Upper left => X: 3.4915, Y= 0.6828

      Upper right => X: 5.0116, Y= 0.6828

      Lower right => X: 5.0116, Y= 0.8265

      Lower left => X: 3.4915, Y= 0.8265

  Line 1 has content: 'SECURITIES AND EXCHANGE COMMISSION'.

    Its bounding box is:

      Upper left => X: 2.1937, Y= 0.9061

      Upper right => X: 6.297, Y= 0.9061

      Lower right => X: 6.297, Y= 1.0498

      Lower left => X: 2.1937, Y= 1.0498

  Line 2 has content: 'Washington, D.C. 20549'.

    Its bounding box is:

      Upper left => X: 3.4629, Y= 1.1179

      Upper right => X: 5.031, Y= 1.1179

      Lower right => X: 5.031, Y= 1.2483

      Lower left => X: 3.4629, Y= 1.2483

  Line 3 has content: 'FORM 10-Q'.

    Its bounding box is:

      Upper left => X: 3.7352, Y= 1.4211

      Upper right => X: 4.7769, Y= 1.4211

      Lower right => X: 4.7769, Y= 1.5763

      Lower left => X: 3.7352, Y= 1.5763

  Line 4 has content: '?'.

    Its bounding box is:

      Upper left => X: 0.6694, Y= 1.7746

      Upper right => X: 0.7764, Y= 1.7746

      Lower right => X: 0.7764, Y= 1.8833

      Lower left => X: 0.6694, Y= 1.8833

  Line 5 has content: 'QUARTERLY REPORT PURSUANT TO SECTION 13 OR 15(d) OF THE SECURITIES EXCHANGE ACT OF'.

    Its bounding box is:

      Upper left => X: 0.996, Y= 1.7804

      Upper right => X: 7.8449, Y= 1.7804

      Lower right => X: 7.8449, Y= 1.9108

      Lower left => X: 0.996, Y= 1.9108

  Line 6 has content: '1934'.

    Its bounding box is:

      Upper left => X: 1.001, Y= 1.9542

      Upper right => X: 1.2967, Y= 1.9542

      Lower right => X: 1.2967, Y= 2.0559

      Lower left => X: 1.001, Y= 2.0559

  Line 7 has content: 'For the Quarterly Period Ended March 31, 2020'.

    Its bounding box is:

      Upper left => X: 0.9982, Y= 2.1626

      Upper right => X: 3.4543, Y= 2.1626

      Lower right => X: 3.4543, Y= 2.2665

      Lower left => X: 0.9982, Y= 2.2665

  Line 8 has content: 'OR'.

    Its bounding box is:

      Upper left => X: 4.1471, Y= 2.2972

      Upper right => X: 4.3587, Y= 2.2972

      Lower right => X: 4.3587, Y= 2.4049

      Lower left => X: 4.1471, Y= 2.4049

  Line 9 has content: '?'.

    Its bounding box is:

      Upper left => X: 0.6694, Y= 2.6955

      Upper right => X: 0.777, Y= 2.6955

      Lower right => X: 0.777, Y= 2.8042

      Lower left => X: 0.6694, Y= 2.8042

  Line 10 has content: 'TRANSITION REPORT PURSUANT TO SECTION 13 OR 15(d) OF THE SECURITIES EXCHANGE ACT OF'.

    Its bounding box is:

      Upper left => X: 0.9929, Y= 2.7029

      Upper right => X: 7.8449, Y= 2.7029

      Lower right => X: 7.8449, Y= 2.8333

      Lower left => X: 0.9929, Y= 2.8333

  Line 11 has content: '1934'.

    Its bounding box is:

      Upper left => X: 1.001, Y= 2.8775

      Upper right => X: 1.2967, Y= 2.8775

      Lower right => X: 1.2967, Y= 2.9792

      Lower left => X: 1.001, Y= 2.9792

  Line 12 has content: 'For the Transition Period From'.

    Its bounding box is:

      Upper left => X: 0.9982, Y= 3.0873

      Upper right => X: 2.6112, Y= 3.0873

      Lower right => X: 2.6112, Y= 3.1679

      Lower left => X: 0.9982, Y= 3.1679

  Line 13 has content: 'to'.

    Its bounding box is:

      Upper left => X: 3.1754, Y= 3.0889

      Upper right => X: 3.275, Y= 3.0889

      Lower right => X: 3.275, Y= 3.1679

      Lower left => X: 3.1754, Y= 3.1679

  Line 14 has content: 'Commission File Number 001-37845'.

    Its bounding box is:

      Upper left => X: 3.2447, Y= 3.2697

      Upper right => X: 5.2571, Y= 3.2697

      Lower right => X: 5.2571, Y= 3.3573

      Lower left => X: 3.2447, Y= 3.3573

  Line 15 has content: 'MICROSOFT CORPORATION'.

    Its bounding box is:

      Upper left => X: 2.5452, Y= 3.5647

      Upper right => X: 5.952, Y= 3.5647

      Lower right => X: 5.952, Y= 3.7497

      Lower left => X: 2.5452, Y= 3.7497

  Line 16 has content: 'WASHINGTON'.

    Its bounding box is:

      Upper left => X: 2.0004, Y= 3.9639

      Upper right => X: 2.8111, Y= 3.9639

      Lower right => X: 2.8111, Y= 4.0514

      Lower left => X: 2.0004, Y= 4.0514

  Line 17 has content: '(STATE OF INCORPORATION)'.

    Its bounding box is:

      Upper left => X: 1.7151, Y= 4.1057

      Upper right => X: 3.1046, Y= 4.1057

      Lower right => X: 3.1046, Y= 4.197

      Lower left => X: 1.7151, Y= 4.197

  Line 18 has content: '91-1144442'.

    Its bounding box is:

      Upper left => X: 5.7788, Y= 3.9649

      Upper right => X: 6.3997, Y= 3.9649

      Lower right => X: 6.3997, Y= 4.0514

      Lower left => X: 5.7788, Y= 4.0514

  Line 19 has content: '(I.R.S. ID)'.

    Its bounding box is:

      Upper left => X: 5.8792, Y= 4.1057

      Upper right => X: 6.3016, Y= 4.1057

      Lower right => X: 6.3016, Y= 4.197

      Lower left => X: 5.8792, Y= 4.197

  Line 20 has content: 'ONE MICROSOFT WAY, REDMOND, WASHINGTON 98052-6399'.

    Its bounding box is:

      Upper left => X: 2.5939, Y= 4.2851

      Upper right => X: 5.9056, Y= 4.2851

      Lower right => X: 5.9056, Y= 4.3835

      Lower left => X: 2.5939, Y= 4.3835

  Line 21 has content: '(425) 882-8080'.

    Its bounding box is:

      Upper left => X: 3.8758, Y= 4.4135

      Upper right => X: 4.6237, Y= 4.4135

      Lower right => X: 4.6237, Y= 4.5173

      Lower left => X: 3.8758, Y= 4.5173

  Line 22 has content: 'www.microsoft.com/investor'.

    Its bounding box is:

      Upper left => X: 3.4906, Y= 4.541

      Upper right => X: 5.0096, Y= 4.541

      Lower right => X: 5.0096, Y= 4.6229

      Lower left => X: 3.4906, Y= 4.6229

  Line 23 has content: 'Securities registered pursuant to Section 12(b) of the Act:'.

    Its bounding box is:

      Upper left => X: 0.6345, Y= 4.7405

      Upper right => X: 3.6169, Y= 4.7405

      Lower right => X: 3.6169, Y= 4.8514

      Lower left => X: 0.6345, Y= 4.8514

  Line 24 has content: 'Title of each class'.

    Its bounding box is:

      Upper left => X: 0.6319, Y= 4.9689

      Upper right => X: 1.5695, Y= 4.9689

      Lower right => X: 1.5695, Y= 5.0563

      Lower left => X: 0.6319, Y= 5.0563

  Line 25 has content: 'Trading Symbol'.

    Its bounding box is:

      Upper left => X: 4.3444, Y= 4.9689

      Upper right => X: 5.1604, Y= 4.9689

      Lower right => X: 5.1604, Y= 5.0797

      Lower left => X: 4.3444, Y= 5.0797

  Line 26 has content: 'Name of exchange on which registered'.

    Its bounding box is:

      Upper left => X: 5.5923, Y= 4.9689

      Upper right => X: 7.6228, Y= 4.9689

      Lower right => X: 7.6228, Y= 5.0797

      Lower left => X: 5.5923, Y= 5.0797

  Line 27 has content: 'Common stock, $0.00000625 par value per share'.

    Its bounding box is:

      Upper left => X: 0.6347, Y= 5.1719

      Upper right => X: 3.3476, Y= 5.1719

      Lower right => X: 3.3476, Y= 5.2865

      Lower left => X: 0.6347, Y= 5.2865

  Line 28 has content: 'MSFT'.

    Its bounding box is:

      Upper left => X: 4.6026, Y= 5.1772

      Upper right => X: 4.9132, Y= 5.1772

      Lower right => X: 4.9132, Y= 5.2648

      Lower left => X: 4.6026, Y= 5.2648

  Line 29 has content: 'NASDAQ'.

    Its bounding box is:

      Upper left => X: 6.3604, Y= 5.1772

      Upper right => X: 6.8629, Y= 5.1772

      Lower right => X: 6.8629, Y= 5.2717

      Lower left => X: 6.3604, Y= 5.2717

  Line 30 has content: '2.125% Notes due 2021'.

    Its bounding box is:

      Upper left => X: 0.6321, Y= 5.3347

      Upper right => X: 1.9098, Y= 5.3347

      Lower right => X: 1.9098, Y= 5.4241

      Lower left => X: 0.6321, Y= 5.4241

  Line 31 has content: 'MSFT'.

    Its bounding box is:

      Upper left => X: 4.6026, Y= 5.3347

      Upper right => X: 4.9132, Y= 5.3347

      Lower right => X: 4.9132, Y= 5.4223

      Lower left => X: 4.6026, Y= 5.4223

  Line 32 has content: 'NASDAQ'.

    Its bounding box is:

      Upper left => X: 6.3604, Y= 5.3347

      Upper right => X: 6.8629, Y= 5.3347

      Lower right => X: 6.8629, Y= 5.4292

      Lower left => X: 6.3604, Y= 5.4292

  Line 33 has content: '3.125% Notes due 2028'.

    Its bounding box is:

      Upper left => X: 0.6337, Y= 5.493

      Upper right => X: 1.9238, Y= 5.493

      Lower right => X: 1.9238, Y= 5.5825

      Lower left => X: 0.6337, Y= 5.5825

  Line 34 has content: 'MSFT'.

    Its bounding box is:

      Upper left => X: 4.6026, Y= 5.493

      Upper right => X: 4.9132, Y= 5.493

      Lower right => X: 4.9132, Y= 5.5806

      Lower left => X: 4.6026, Y= 5.5806

  Line 35 has content: 'NASDAQ'.

    Its bounding box is:

      Upper left => X: 6.3604, Y= 5.493

      Upper right => X: 6.8629, Y= 5.493

      Lower right => X: 6.8629, Y= 5.5876

      Lower left => X: 6.3604, Y= 5.5876

  Line 36 has content: '2.625% Notes due 2033'.

    Its bounding box is:

      Upper left => X: 0.6321, Y= 5.6505

      Upper right => X: 1.9238, Y= 5.6505

      Lower right => X: 1.9238, Y= 5.74

      Lower left => X: 0.6321, Y= 5.74

  Line 37 has content: 'MSFT'.

    Its bounding box is:

      Upper left => X: 4.6026, Y= 5.6505

      Upper right => X: 4.9132, Y= 5.6505

      Lower right => X: 4.9132, Y= 5.7381

      Lower left => X: 4.6026, Y= 5.7381

  Line 38 has content: 'NASDAQ'.

    Its bounding box is:

      Upper left => X: 6.3604, Y= 5.6505

      Upper right => X: 6.8629, Y= 5.6505

      Lower right => X: 6.8629, Y= 5.7451

      Lower left => X: 6.3604, Y= 5.7451

  Line 39 has content: 'Securities registered pursuant to Section 12(g) of the Act:'.

    Its bounding box is:

      Upper left => X: 0.6345, Y= 5.8505

      Upper right => X: 3.6169, Y= 5.8505

      Lower right => X: 3.6169, Y= 5.9614

      Lower left => X: 0.6345, Y= 5.9614

  Line 40 has content: 'NONE'.

    Its bounding box is:

      Upper left => X: 0.6379, Y= 6.0789

      Upper right => X: 0.9646, Y= 6.0789

      Lower right => X: 0.9646, Y= 6.1663

      Lower left => X: 0.6379, Y= 6.1663

  Line 41 has content: 'Indicate by check mark whether the registrant (1) has filed all reports required to be filed by Section 13 or 15(d) of the Securities Exchange'.

    Its bounding box is:

      Upper left => X: 0.661, Y= 6.2705

      Upper right => X: 7.8456, Y= 6.2705

      Lower right => X: 7.8456, Y= 6.3814

      Lower left => X: 0.661, Y= 6.3814

  Line 42 has content: 'Act of 1934 during the preceding 12 months (or for such shorter period that the registrant was required to file such reports), and (2) has'.

    Its bounding box is:

      Upper left => X: 0.65, Y= 6.4064

      Upper right => X: 7.8446, Y= 6.4064

      Lower right => X: 7.8446, Y= 6.5172

      Lower left => X: 0.65, Y= 6.5172

  Line 43 has content: 'been subject to such filing requirements for the past 90 days.'.

    Its bounding box is:

      Upper left => X: 0.6578, Y= 6.5497

      Upper right => X: 3.8221, Y= 6.5497

      Lower right => X: 3.8221, Y= 6.6605

      Lower left => X: 0.6578, Y= 6.6605

  Line 44 has content: 'Yes ?'.

    Its bounding box is:

      Upper left => X: 3.9638, Y= 6.549

      Upper right => X: 4.3353, Y= 6.549

      Lower right => X: 4.3353, Y= 6.6371

      Lower left => X: 3.9638, Y= 6.6371

  Line 45 has content: 'No ?'.

    Its bounding box is:

      Upper left => X: 4.4902, Y= 6.5477

      Upper right => X: 4.8008, Y= 6.5477

      Lower right => X: 4.8008, Y= 6.6371

      Lower left => X: 4.4902, Y= 6.6371

  Line 46 has content: 'Indicate by check mark whether the registrant has submitted electronically every Interactive Data File required to be submitted pursuant to Rule'.

    Its bounding box is:

      Upper left => X: 0.6611, Y= 6.7519

      Upper right => X: 7.845, Y= 6.7519

      Lower right => X: 7.845, Y= 6.8614

      Lower left => X: 0.6611, Y= 6.8614

  Line 47 has content: '405 of Regulation S-T (§232.405 of this chapter) during the preceding 12 months (or for such shorter period that the registrant was required to'.

    Its bounding box is:

      Upper left => X: 0.6517, Y= 6.8863

      Upper right => X: 7.8459, Y= 6.8863

      Lower right => X: 7.8459, Y= 6.9972

      Lower left => X: 0.6517, Y= 6.9972

  Line 48 has content: 'submit such files).'.

    Its bounding box is:

      Upper left => X: 0.6538, Y= 7.0296

      Upper right => X: 1.5594, Y= 7.0296

      Lower right => X: 1.5594, Y= 7.1405

      Lower left => X: 0.6538, Y= 7.1405

  Line 49 has content: 'Yes ?'.

    Its bounding box is:

      Upper left => X: 1.6938, Y= 7.0289

      Upper right => X: 2.0577, Y= 7.0289

      Lower right => X: 2.0577, Y= 7.1171

      Lower left => X: 1.6938, Y= 7.1171

  Line 50 has content: 'No ?'.

    Its bounding box is:

      Upper left => X: 2.2066, Y= 7.0276

      Upper right => X: 2.5115, Y= 7.0276

      Lower right => X: 2.5115, Y= 7.1171

      Lower left => X: 2.2066, Y= 7.1171

  Line 51 has content: 'Indicate by check mark whether the registrant is a large accelerated filer, an accelerated filer, a non-accelerated filer, a smaller reporting'.

    Its bounding box is:

      Upper left => X: 0.6611, Y= 7.2304

      Upper right => X: 7.8417, Y= 7.2304

      Lower right => X: 7.8417, Y= 7.3413

      Lower left => X: 0.6611, Y= 7.3413

  Line 52 has content: 'company, or an emerging growth company. See the definitions of "large accelerated filer," "accelerated filer," "smaller reporting company,"'.

    Its bounding box is:

      Upper left => X: 0.6547, Y= 7.3663

      Upper right => X: 7.8449, Y= 7.3663

      Lower right => X: 7.8449, Y= 7.4772

      Lower left => X: 0.6547, Y= 7.4772

  Line 53 has content: 'and "emerging growth company" in Rule 12b-2 of the Exchange Act.'.

    Its bounding box is:

      Upper left => X: 0.6543, Y= 7.5021

      Upper right => X: 4.1966, Y= 7.5021

      Lower right => X: 4.1966, Y= 7.613

      Lower left => X: 0.6543, Y= 7.613

  Line 54 has content: 'Large accelerated filer ?'.

    Its bounding box is:

      Upper left => X: 0.6586, Y= 7.709

      Upper right => X: 1.9534, Y= 7.709

      Lower right => X: 1.9534, Y= 7.8206

      Lower left => X: 0.6586, Y= 7.8206

  Line 55 has content: 'Non-accelerated filer ?'.

    Its bounding box is:

      Upper left => X: 0.659, Y= 7.8827

      Upper right => X: 1.8756, Y= 7.8827

      Lower right => X: 1.8756, Y= 7.9721

      Lower left => X: 0.659, Y= 7.9721

  Line 56 has content: 'Accelerated filer ?'.

    Its bounding box is:

      Upper left => X: 5.3358, Y= 7.7077

      Upper right => X: 6.3181, Y= 7.7077

      Lower right => X: 6.3181, Y= 7.7971

      Lower left => X: 5.3358, Y= 7.7971

  Line 57 has content: 'Smaller reporting company ?'.

    Its bounding box is:

      Upper left => X: 5.3412, Y= 7.8827

      Upper right => X: 6.8831, Y= 7.8827

      Lower right => X: 6.8831, Y= 7.9956

      Lower left => X: 5.3412, Y= 7.9956

  Line 58 has content: 'Emerging growth company ?'.

    Its bounding box is:

      Upper left => X: 5.3452, Y= 8.0585

      Upper right => X: 6.8764, Y= 8.0585

      Lower right => X: 6.8764, Y= 8.1714

      Lower left => X: 5.3452, Y= 8.1714

  Line 59 has content: 'If an emerging growth company, indicate by check mark if the registrant has elected not to use the extended transition period for complying'.

    Its bounding box is:

      Upper left => X: 0.661, Y= 8.2614

      Upper right => X: 7.842, Y= 8.2614

      Lower right => X: 7.842, Y= 8.3722

      Lower left => X: 0.661, Y= 8.3722

  Line 60 has content: 'with any new or revised financial accounting standards provided pursuant to Section 13(a) of the Exchange Act. ?'.

    Its bounding box is:

      Upper left => X: 0.6504, Y= 8.4139

      Upper right => X: 6.6275, Y= 8.4139

      Lower right => X: 6.6275, Y= 8.5248

      Lower left => X: 0.6504, Y= 8.5248

  Line 61 has content: 'Indicate by check mark whether the registrant is a shell company (as defined in Rule 12b-2 of the Exchange Act).'.

    Its bounding box is:

      Upper left => X: 0.6609, Y= 8.6172

      Upper right => X: 6.3833, Y= 8.6172

      Lower right => X: 6.3833, Y= 8.7281

      Lower left => X: 0.6609, Y= 8.7281

  Line 62 has content: 'Yes ?'.

    Its bounding box is:

      Upper left => X: 6.5188, Y= 8.6152

      Upper right => X: 6.8824, Y= 8.6152

      Lower right => X: 6.8824, Y= 8.7046

      Lower left => X: 6.5188, Y= 8.7046

  Line 63 has content: 'No ?'.

    Its bounding box is:

      Upper left => X: 7.0307, Y= 8.6165

      Upper right => X: 7.3351, Y= 8.6165

      Lower right => X: 7.3351, Y= 8.7046

      Lower left => X: 7.0307, Y= 8.7046

  Line 64 has content: 'Indicate the number of shares outstanding of each of the issuer's classes of common stock, as of the latest practicable date.'.

    Its bounding box is:

      Upper left => X: 0.6611, Y= 8.818

      Upper right => X: 6.9374, Y= 8.818

      Lower right => X: 6.9374, Y= 8.9289

      Lower left => X: 0.6611, Y= 8.9289

  Line 65 has content: 'Class'.

    Its bounding box is:

      Upper left => X: 0.6549, Y= 9.0975

      Upper right => X: 0.9237, Y= 9.0975

      Lower right => X: 0.9237, Y= 9.1745

      Lower left => X: 0.6549, Y= 9.1745

  Line 66 has content: 'Outstanding as of April 24, 2020'.

    Its bounding box is:

      Upper left => X: 6.1303, Y= 9.105

      Upper right => X: 7.707, Y= 9.105

      Lower right => X: 7.707, Y= 9.2026

      Lower left => X: 6.1303, Y= 9.2026

  Line 67 has content: 'Common Stock, $0.00000625 par value per share'.

    Its bounding box is:

      Upper left => X: 0.6559, Y= 9.3141

      Upper right => X: 3.2571, Y= 9.3141

      Lower right => X: 3.2571, Y= 9.4301

      Lower left => X: 0.6559, Y= 9.4301

  Line 68 has content: '7,583,440,247 shares'.

    Its bounding box is:

      Upper left => X: 6.5747, Y= 9.3458

      Upper right => X: 7.7069, Y= 9.3458

      Lower right => X: 7.7069, Y= 9.4475

      Lower left => X: 6.5747, Y= 9.4475

  Selection Mark 0 is Selected.

    Its bounding box is:

      Upper left => X: 0.6694, Y= 1.7746

      Upper right => X: 0.7764, Y= 1.7746

      Lower right => X: 0.7764, Y= 1.8833

      Lower left => X: 0.6694, Y= 1.8833

  Selection Mark 1 is Unselected.

    Its bounding box is:

      Upper left => X: 0.6694, Y= 2.6955

      Upper right => X: 0.777, Y= 2.6955

      Lower right => X: 0.777, Y= 2.8042

      Lower left => X: 0.6694, Y= 2.8042

  Selection Mark 2 is Selected.

    Its bounding box is:

      Upper left => X: 4.2484, Y= 6.549

      Upper right => X: 4.3353, Y= 6.549

      Lower right => X: 4.3353, Y= 6.6371

      Lower left => X: 4.2484, Y= 6.6371

  Selection Mark 3 is Unselected.

    Its bounding box is:

      Upper left => X: 4.7134, Y= 6.5477

      Upper right => X: 4.8008, Y= 6.5477

      Lower right => X: 4.8008, Y= 6.6358

      Lower left => X: 4.7134, Y= 6.6358

  Selection Mark 4 is Selected.

    Its bounding box is:

      Upper left => X: 1.9708, Y= 7.0289

      Upper right => X: 2.0577, Y= 7.0289

      Lower right => X: 2.0577, Y= 7.1171

      Lower left => X: 1.9708, Y= 7.1171

  Selection Mark 5 is Unselected.

    Its bounding box is:

      Upper left => X: 2.4242, Y= 7.0276

      Upper right => X: 2.5115, Y= 7.0276

      Lower right => X: 2.5115, Y= 7.1158

      Lower left => X: 2.4242, Y= 7.1158

  Selection Mark 6 is Selected.

    Its bounding box is:

      Upper left => X: 1.8666, Y= 7.709

      Upper right => X: 1.9534, Y= 7.709

      Lower right => X: 1.9534, Y= 7.7971

      Lower left => X: 1.8666, Y= 7.7971

  Selection Mark 7 is Unselected.

    Its bounding box is:

      Upper left => X: 6.2307, Y= 7.7077

      Upper right => X: 6.3181, Y= 7.7077

      Lower right => X: 6.3181, Y= 7.7958

      Lower left => X: 6.2307, Y= 7.7958

  Selection Mark 8 is Unselected.

    Its bounding box is:

      Upper left => X: 1.7882, Y= 7.8827

      Upper right => X: 1.8756, Y= 7.8827

      Lower right => X: 1.8756, Y= 7.9708

      Lower left => X: 1.7882, Y= 7.9708

  Selection Mark 9 is Unselected.

    Its bounding box is:

      Upper left => X: 6.7957, Y= 7.8827

      Upper right => X: 6.8831, Y= 7.8827

      Lower right => X: 6.8831, Y= 7.9708

      Lower left => X: 6.7957, Y= 7.9708

  Selection Mark 10 is Unselected.

    Its bounding box is:

      Upper left => X: 6.7891, Y= 8.0585

      Upper right => X: 6.8764, Y= 8.0585

      Lower right => X: 6.8764, Y= 8.1467

      Lower left => X: 6.7891, Y= 8.1467

  Selection Mark 11 is Unselected.

    Its bounding box is:

      Upper left => X: 6.5447, Y= 8.416

      Upper right => X: 6.6275, Y= 8.416

      Lower right => X: 6.6275, Y= 8.5

      Lower left => X: 6.5447, Y= 8.5

  Selection Mark 12 is Unselected.

    Its bounding box is:

      Upper left => X: 6.795, Y= 8.6152

      Upper right => X: 6.8824, Y= 8.6152

      Lower right => X: 6.8824, Y= 8.7033

      Lower left => X: 6.795, Y= 8.7033

  Selection Mark 13 is Selected.

    Its bounding box is:

      Upper left => X: 7.2483, Y= 8.6165

      Upper right => X: 7.3351, Y= 8.6165

      Lower right => X: 7.3351, Y= 8.7046

      Lower left => X: 7.2483, Y= 8.7046

The following tables were extracted:

  Table 0 has 5 rows and 3 columns.

    Cell (0, 0) has kind 'columnHeader' and content: 'Title of each class'.

    Cell (0, 1) has kind 'columnHeader' and content: 'Trading Symbol'.

    Cell (0, 2) has kind 'columnHeader' and content: 'Name of exchange on which registered'.

    Cell (1, 0) has kind '' and content: 'Common stock, $0.00000625 par value per share'.

    Cell (1, 1) has kind '' and content: 'MSFT'.

    Cell (1, 2) has kind '' and content: 'NASDAQ'.

    Cell (2, 0) has kind '' and content: '2.125% Notes due 2021'.

    Cell (2, 1) has kind '' and content: 'MSFT'.

    Cell (2, 2) has kind '' and content: 'NASDAQ'.

    Cell (3, 0) has kind '' and content: '3.125% Notes due 2028'.

    Cell (3, 1) has kind '' and content: 'MSFT'.

    Cell (3, 2) has kind '' and content: 'NASDAQ'.

    Cell (4, 0) has kind '' and content: '2.625% Notes due 2033'.

    Cell (4, 1) has kind '' and content: 'MSFT'.

    Cell (4, 2) has kind '' and content: 'NASDAQ'.

  Table 1 has 2 rows and 2 columns.

    Cell (0, 0) has kind 'columnHeader' and content: 'Class'.

    Cell (0, 1) has kind 'columnHeader' and content: 'Outstanding as of April 24, 2020'.

    Cell (1, 0) has kind '' and content: 'Common Stock, $0.00000625 par value per share'.

    Cell (1, 1) has kind '' and content: '7,583,440,247 shares'.
    
    
