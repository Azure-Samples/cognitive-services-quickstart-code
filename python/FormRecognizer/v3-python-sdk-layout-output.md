# Quickstart output: Python SDK v3.0 layout model

[Reference documentation](/python/api/azure-ai-formrecognizer/azure.ai.formrecognizer?view=azure-python-preview&preserve-view=true) | [Library source code](https://github.com/Azure/azure-sdk-for-python/tree/azure-ai-formrecognizer_3.2.0b3/sdk/formrecognizer/azure-ai-formrecognizer/) | [Package (PyPi)](https://pypi.org/project/azure-ai-formrecognizer/3.2.0b3/) | [Samples](https://github.com/Azure/azure-sdk-for-python/blob/azure-ai-formrecognizer_3.2.0b3/sdk/formrecognizer/azure-ai-formrecognizer/samples/README.md)

You can get started using the Azure Form Recognizer layout model with the [Python programming language quickstart](https://docs.microsoft.com/azure/applied-ai-services/form-recognizer/quickstarts/try-v3-python-sdk#layout-model). The layout model analyzes and extracts tables, lines, words, and selection marks like radio buttons and check boxes from forms and documents, without the need to train a model. Here is the expected outcome from the layout model quickstart code:

## Layout model output

----Analyzing layout from page #1----

Page has width: 8.5 and height: 11.0, measured with unit: inch

...Line # 0 has word count 2 and text 'UNITED STATES' within bounding box '[3.4915, 0.6828], [5.0116, 0.6828], [5.0116, 0.8265], [3.4915, 0.8265]'

......Word 'UNITED' has a confidence of 1.0

......Word 'STATES' has a confidence of 1.0

...Line # 1 has word count 4 and text 'SECURITIES AND EXCHANGE COMMISSION' within bounding box '[2.1937, 0.9061], [6.297, 0.9061], [6.297, 1.0498], [2.1937, 1.0498]'

......Word 'SECURITIES' has a confidence of 1.0

......Word 'AND' has a confidence of 1.0

......Word 'EXCHANGE' has a confidence of 1.0

......Word 'COMMISSION' has a confidence of 1.0

...Line # 2 has word count 3 and text 'Washington, D.C. 20549' within bounding box '[3.4629, 1.1179], [5.031, 1.1179], [5.031, 1.2483], [3.4629, 1.2483]'

......Word 'Washington,' has a confidence of 1.0

......Word 'D.C.' has a confidence of 1.0

......Word '20549' has a confidence of 1.0

...Line # 3 has word count 2 and text 'FORM 10-Q' within bounding box '[3.7352, 1.4211], [4.7769, 1.4211], [4.7769, 1.5763], [3.7352, 1.5763]'

......Word 'FORM' has a confidence of 1.0

......Word '10-Q' has a confidence of 1.0

...Line # 4 has word count 1 and text '☒' within bounding box '[0.6694, 1.7746], [0.7764, 1.7746], [0.7764, 1.8833], [0.6694, 1.8833]'

......Word '☒' has a confidence of 1.0

...Line # 5 has word count 14 and text 'QUARTERLY REPORT PURSUANT TO SECTION 13 OR 15(d) OF THE SECURITIES EXCHANGE ACT OF' within bounding box '[0.996, 1.7804], [7.8449, 1.7804], [7.8449, 1.9108], [0.996, 1.9108]'

......Word 'QUARTERLY' has a confidence of 1.0

......Word 'REPORT' has a confidence of 1.0

......Word 'PURSUANT' has a confidence of 1.0

......Word 'TO' has a confidence of 1.0

......Word 'SECTION' has a confidence of 1.0

......Word '13' has a confidence of 1.0

......Word 'OR' has a confidence of 1.0

......Word '15(d)' has a confidence of 1.0

......Word 'OF' has a confidence of 1.0

......Word 'THE' has a confidence of 1.0

......Word 'SECURITIES' has a confidence of 1.0

......Word 'EXCHANGE' has a confidence of 1.0

......Word 'ACT' has a confidence of 1.0

......Word 'OF' has a confidence of 1.0

...Line # 6 has word count 1 and text '1934' within bounding box '[1.001, 1.9542], [1.2967, 1.9542], [1.2967, 2.0559], [1.001, 2.0559]'

......Word '1934' has a confidence of 1.0

...Line # 7 has word count 8 and text 'For the Quarterly Period Ended March 31, 2020' within bounding box '[0.9982, 2.1626], [3.4543, 2.1626], [3.4543, 2.2665], [0.9982, 2.2665]'

......Word 'For' has a confidence of 1.0

......Word 'the' has a confidence of 1.0

......Word 'Quarterly' has a confidence of 1.0

......Word 'Period' has a confidence of 1.0

......Word 'Ended' has a confidence of 1.0

......Word 'March' has a confidence of 1.0

......Word '31,' has a confidence of 1.0

......Word '2020' has a confidence of 1.0

...Line # 8 has word count 1 and text 'OR' within bounding box '[4.1471, 2.2972], [4.3587, 2.2972], [4.3587, 2.4049], [4.1471, 2.4049]'

......Word 'OR' has a confidence of 1.0

...Line # 9 has word count 1 and text '☐' within bounding box '[0.6694, 2.6955], [0.777, 2.6955], [0.777, 2.8042], [0.6694, 2.8042]'

......Word '☐' has a confidence of 1.0

...Line # 10 has word count 14 and text 'TRANSITION REPORT PURSUANT TO SECTION 13 OR 15(d) OF THE SECURITIES EXCHANGE ACT OF' within bounding box '[0.9929, 2.7029], [7.8449, 2.7029], [7.8449, 2.8333], [0.9929, 2.8333]'

......Word 'TRANSITION' has a confidence of 1.0

......Word 'REPORT' has a confidence of 1.0

......Word 'PURSUANT' has a confidence of 1.0

......Word 'TO' has a confidence of 1.0

......Word 'SECTION' has a confidence of 1.0

......Word '13' has a confidence of 1.0

......Word 'OR' has a confidence of 1.0

......Word '15(d)' has a confidence of 1.0

......Word 'OF' has a confidence of 1.0

......Word 'THE' has a confidence of 1.0

......Word 'SECURITIES' has a confidence of 1.0

......Word 'EXCHANGE' has a confidence of 1.0

......Word 'ACT' has a confidence of 1.0

......Word 'OF' has a confidence of 1.0

...Line # 11 has word count 1 and text '1934' within bounding box '[1.001, 2.8775], [1.2967, 2.8775], [1.2967, 2.9792], [1.001, 2.9792]'

......Word '1934' has a confidence of 1.0

...Line # 12 has word count 5 and text 'For the Transition Period From' within bounding box '[0.9982, 3.0873], [2.6112, 3.0873], [2.6112, 3.1679], [0.9982, 3.1679]'

......Word 'For' has a confidence of 1.0

......Word 'the' has a confidence of 1.0

......Word 'Transition' has a confidence of 1.0

......Word 'Period' has a confidence of 1.0

......Word 'From' has a confidence of 1.0

...Line # 13 has word count 1 and text 'to' within bounding box '[3.1754, 3.0889], [3.275, 3.0889], [3.275, 3.1679], [3.1754, 3.1679]'

......Word 'to' has a confidence of 1.0

...Line # 14 has word count 4 and text 'Commission File Number 001-37845' within bounding box '[3.2447, 3.2697], [5.2571, 3.2697], [5.2571, 3.3573], [3.2447, 3.3573]'

......Word 'Commission' has a confidence of 1.0

......Word 'File' has a confidence of 1.0

......Word 'Number' has a confidence of 1.0

......Word '001-37845' has a confidence of 1.0

...Line # 15 has word count 2 and text 'MICROSOFT CORPORATION' within bounding box '[2.5452, 3.5647], [5.952, 3.5647], [5.952, 3.7497], [2.5452, 3.7497]'

......Word 'MICROSOFT' has a confidence of 1.0

......Word 'CORPORATION' has a confidence of 1.0

...Line # 16 has word count 1 and text 'WASHINGTON' within bounding box '[2.0004, 3.9639], [2.8111, 3.9639], [2.8111, 4.0514], [2.0004, 4.0514]'

......Word 'WASHINGTON' has a confidence of 1.0

...Line # 17 has word count 3 and text '(STATE OF INCORPORATION)' within bounding box '[1.7151, 4.1057], [3.1046, 4.1057], [3.1046, 4.197], [1.7151, 4.197]'

......Word '(STATE' has a confidence of 1.0

......Word 'OF' has a confidence of 1.0

......Word 'INCORPORATION)' has a confidence of 1.0

...Line # 18 has word count 1 and text '91-1144442' within bounding box '[5.7788, 3.9649], [6.3997, 3.9649], [6.3997, 4.0514], [5.7788, 4.0514]'

......Word '91-1144442' has a confidence of 1.0

...Line # 19 has word count 2 and text '(I.R.S. ID)' within bounding box '[5.8792, 4.1057], [6.3016, 4.1057], [6.3016, 4.197], [5.8792, 4.197]'

......Word '(I.R.S.' has a confidence of 1.0

......Word 'ID)' has a confidence of 1.0

...Line # 20 has word count 6 and text 'ONE MICROSOFT WAY, REDMOND, WASHINGTON 98052-6399' within bounding box '[2.5939, 4.2851], [5.9056, 4.2851], [5.9056, 4.3835], [2.5939, 4.3835]'

......Word 'ONE' has a confidence of 1.0

......Word 'MICROSOFT' has a confidence of 1.0

......Word 'WAY,' has a confidence of 1.0

......Word 'REDMOND,' has a confidence of 1.0

......Word 'WASHINGTON' has a confidence of 1.0

......Word '98052-6399' has a confidence of 1.0

...Line # 21 has word count 2 and text '(425) 882-8080' within bounding box '[3.8758, 4.4135], [4.6237, 4.4135], [4.6237, 4.5173], [3.8758, 4.5173]'

......Word '(425)' has a confidence of 1.0

......Word '882-8080' has a confidence of 1.0

...Line # 22 has word count 1 and text 'www.microsoft.com/investor' within bounding box '[3.4906, 4.541], [5.0096, 4.541], [5.0096, 4.6229], [3.4906, 4.6229]'

......Word 'www.microsoft.com/investor' has a confidence of 1.0

...Line # 23 has word count 9 and text 'Securities registered pursuant to Section 12(b) of the Act:' within bounding box '[0.6345, 4.7405], [3.6169, 4.7405], [3.6169, 4.8514], [0.6345, 4.8514]'

......Word 'Securities' has a confidence of 1.0

......Word 'registered' has a confidence of 1.0

......Word 'pursuant' has a confidence of 1.0

......Word 'to' has a confidence of 1.0

......Word 'Section' has a confidence of 1.0

......Word '12(b)' has a confidence of 1.0

......Word 'of' has a confidence of 1.0

......Word 'the' has a confidence of 1.0

......Word 'Act:' has a confidence of 1.0

...Line # 24 has word count 4 and text 'Title of each class' within bounding box '[0.6319, 4.9689], [1.5695, 4.9689], [1.5695, 5.0563], [0.6319, 5.0563]'

......Word 'Title' has a confidence of 1.0

......Word 'of' has a confidence of 1.0

......Word 'each' has a confidence of 1.0

......Word 'class' has a confidence of 1.0

...Line # 25 has word count 2 and text 'Trading Symbol' within bounding box '[4.3444, 4.9689], [5.1604, 4.9689], [5.1604, 5.0797], [4.3444, 5.0797]'

......Word 'Trading' has a confidence of 1.0

......Word 'Symbol' has a confidence of 1.0

...Line # 26 has word count 6 and text 'Name of exchange on which registered' within bounding box '[5.5923, 4.9689], [7.6228, 4.9689], [7.6228, 5.0797], [5.5923, 5.0797]'

......Word 'Name' has a confidence of 1.0

......Word 'of' has a confidence of 1.0

......Word 'exchange' has a confidence of 1.0

......Word 'on' has a confidence of 1.0

......Word 'which' has a confidence of 1.0

......Word 'registered' has a confidence of 1.0

...Line # 27 has word count 7 and text 'Common stock, $0.00000625 par value per share' within bounding box '[0.6347, 5.1719], [3.3476, 5.1719], [3.3476, 5.2865], [0.6347, 5.2865]'

......Word 'Common' has a confidence of 1.0

......Word 'stock,' has a confidence of 1.0

......Word '$0.00000625' has a confidence of 1.0

......Word 'par' has a confidence of 1.0

......Word 'value' has a confidence of 1.0

......Word 'per' has a confidence of 1.0

......Word 'share' has a confidence of 1.0

...Line # 28 has word count 1 and text 'MSFT' within bounding box '[4.6026, 5.1772], [4.9132, 5.1772], [4.9132, 5.2648], [4.6026, 5.2648]'

......Word 'MSFT' has a confidence of 1.0

...Line # 29 has word count 1 and text 'NASDAQ' within bounding box '[6.3604, 5.1772], [6.8629, 5.1772], [6.8629, 5.2717], [6.3604, 5.2717]'

......Word 'NASDAQ' has a confidence of 1.0

...Line # 30 has word count 4 and text '2.125% Notes due 2021' within bounding box '[0.6321, 5.3347], [1.9098, 5.3347], [1.9098, 5.4241], [0.6321, 5.4241]'

......Word '2.125%' has a confidence of 1.0

......Word 'Notes' has a confidence of 1.0

......Word 'due' has a confidence of 1.0

......Word '2021' has a confidence of 1.0

...Line # 31 has word count 1 and text 'MSFT' within bounding box '[4.6026, 5.3347], [4.9132, 5.3347], [4.9132, 5.4223], [4.6026, 5.4223]'

......Word 'MSFT' has a confidence of 1.0

...Line # 32 has word count 1 and text 'NASDAQ' within bounding box '[6.3604, 5.3347], [6.8629, 5.3347], [6.8629, 5.4292], [6.3604, 5.4292]'

......Word 'NASDAQ' has a confidence of 1.0

...Line # 33 has word count 4 and text '3.125% Notes due 2028' within bounding box '[0.6337, 5.493], [1.9238, 5.493], [1.9238, 5.5825], [0.6337, 5.5825]'

......Word '3.125%' has a confidence of 1.0

......Word 'Notes' has a confidence of 1.0

......Word 'due' has a confidence of 1.0

......Word '2028' has a confidence of 1.0

...Line # 34 has word count 1 and text 'MSFT' within bounding box '[4.6026, 5.493], [4.9132, 5.493], [4.9132, 5.5806], [4.6026, 5.5806]'

......Word 'MSFT' has a confidence of 1.0

...Line # 35 has word count 1 and text 'NASDAQ' within bounding box '[6.3604, 5.493], [6.8629, 5.493], [6.8629, 5.5876], [6.3604, 5.5876]'

......Word 'NASDAQ' has a confidence of 1.0

...Line # 36 has word count 4 and text '2.625% Notes due 2033' within bounding box '[0.6321, 5.6505], [1.9238, 5.6505], [1.9238, 5.74], [0.6321, 5.74]'

......Word '2.625%' has a confidence of 1.0

......Word 'Notes' has a confidence of 1.0

......Word 'due' has a confidence of 1.0

......Word '2033' has a confidence of 1.0

...Line # 37 has word count 1 and text 'MSFT' within bounding box '[4.6026, 5.6505], [4.9132, 5.6505], [4.9132, 5.7381], [4.6026, 5.7381]'

......Word 'MSFT' has a confidence of 1.0

...Line # 38 has word count 1 and text 'NASDAQ' within bounding box '[6.3604, 5.6505], [6.8629, 5.6505], [6.8629, 5.7451], [6.3604, 5.7451]'

......Word 'NASDAQ' has a confidence of 1.0

...Line # 39 has word count 9 and text 'Securities registered pursuant to Section 12(g) of the Act:' within bounding box '[0.6345, 5.8505], [3.6169, 5.8505], [3.6169, 5.9614], [0.6345, 5.9614]'

......Word 'Securities' has a confidence of 1.0

......Word 'registered' has a confidence of 1.0

......Word 'pursuant' has a confidence of 1.0

......Word 'to' has a confidence of 1.0

......Word 'Section' has a confidence of 1.0

......Word '12(g)' has a confidence of 1.0

......Word 'of' has a confidence of 1.0

......Word 'the' has a confidence of 1.0

......Word 'Act:' has a confidence of 1.0

...Line # 40 has word count 1 and text 'NONE' within bounding box '[0.6379, 6.0789], [0.9646, 6.0789], [0.9646, 6.1663], [0.6379, 6.1663]'

......Word 'NONE' has a confidence of 1.0

...Line # 41 has word count 25 and text 'Indicate by check mark whether the registrant (1) has filed all reports required to be filed by Section 13 or 15(d) of the Securities Exchange' within bounding box '[0.661, 6.2705], [7.8456, 6.2705], [7.8456, 6.3814], [0.661, 6.3814]'

......Word 'Indicate' has a confidence of 1.0

......Word 'by' has a confidence of 1.0

......Word 'check' has a confidence of 1.0

......Word 'mark' has a confidence of 1.0

......Word 'whether' has a confidence of 1.0

......Word 'the' has a confidence of 1.0

......Word 'registrant' has a confidence of 1.0

......Word '(1)' has a confidence of 1.0

......Word 'has' has a confidence of 1.0

......Word 'filed' has a confidence of 1.0

......Word 'all' has a confidence of 1.0

......Word 'reports' has a confidence of 1.0

......Word 'required' has a confidence of 1.0

......Word 'to' has a confidence of 1.0

......Word 'be' has a confidence of 1.0

......Word 'filed' has a confidence of 1.0

......Word 'by' has a confidence of 1.0

......Word 'Section' has a confidence of 1.0

......Word '13' has a confidence of 1.0

......Word 'or' has a confidence of 1.0

......Word '15(d)' has a confidence of 1.0

......Word 'of' has a confidence of 1.0

......Word 'the' has a confidence of 1.0

......Word 'Securities' has a confidence of 1.0

......Word 'Exchange' has a confidence of 1.0

...Line # 42 has word count 25 and text 'Act of 1934 during the preceding 12 months (or for such shorter period that the registrant was required to file such reports), and (2) has' within bounding box '[0.65, 6.4064], [7.8446, 6.4064], [7.8446, 6.5172], [0.65, 6.5172]'

......Word 'Act' has a confidence of 1.0

......Word 'of' has a confidence of 1.0

......Word '1934' has a confidence of 1.0

......Word 'during' has a confidence of 1.0

......Word 'the' has a confidence of 1.0

......Word 'preceding' has a confidence of 1.0

......Word '12' has a confidence of 1.0

......Word 'months' has a confidence of 1.0

......Word '(or' has a confidence of 1.0

......Word 'for' has a confidence of 1.0

......Word 'such' has a confidence of 1.0

......Word 'shorter' has a confidence of 1.0

......Word 'period' has a confidence of 1.0

......Word 'that' has a confidence of 1.0

......Word 'the' has a confidence of 1.0

......Word 'registrant' has a confidence of 1.0

......Word 'was' has a confidence of 1.0

......Word 'required' has a confidence of 1.0

......Word 'to' has a confidence of 1.0

......Word 'file' has a confidence of 1.0

......Word 'such' has a confidence of 1.0

......Word 'reports),' has a confidence of 1.0

......Word 'and' has a confidence of 1.0

......Word '(2)' has a confidence of 1.0

......Word 'has' has a confidence of 1.0

...Line # 43 has word count 11 and text 'been subject to such filing requirements for the past 90 days.' within bounding box '[0.6578, 6.5497], [3.8221, 6.5497], [3.8221, 6.6605], [0.6578, 6.6605]'

......Word 'been' has a confidence of 1.0

......Word 'subject' has a confidence of 1.0

......Word 'to' has a confidence of 1.0

......Word 'such' has a confidence of 1.0

......Word 'filing' has a confidence of 1.0

......Word 'requirements' has a confidence of 1.0

......Word 'for' has a confidence of 1.0

......Word 'the' has a confidence of 1.0

......Word 'past' has a confidence of 1.0

......Word '90' has a confidence of 1.0

......Word 'days.' has a confidence of 1.0

...Line # 44 has word count 2 and text 'Yes ☒' within bounding box '[3.9638, 6.549], [4.3353, 6.549], [4.3353, 6.6371], [3.9638, 6.6371]'

......Word 'Yes' has a confidence of 1.0

......Word '☒' has a confidence of 1.0

...Line # 45 has word count 2 and text 'No ☐' within bounding box '[4.4902, 6.5477], [4.8008, 6.5477], [4.8008, 6.6371], [4.4902, 6.6371]'

......Word 'No' has a confidence of 1.0

......Word '☐' has a confidence of 1.0

...Line # 46 has word count 21 and text 'Indicate by check mark whether the registrant has submitted electronically every Interactive Data File required to be submitted pursuant to Rule' within bounding box '[0.6611, 6.7519], [7.845, 6.7519], [7.845, 6.8614], [0.6611, 6.8614]'

......Word 'Indicate' has a confidence of 1.0

......Word 'by' has a confidence of 1.0

......Word 'check' has a confidence of 1.0

......Word 'mark' has a confidence of 1.0

......Word 'whether' has a confidence of 1.0

......Word 'the' has a confidence of 1.0

......Word 'registrant' has a confidence of 1.0

......Word 'has' has a confidence of 1.0

......Word 'submitted' has a confidence of 1.0

......Word 'electronically' has a confidence of 1.0

......Word 'every' has a confidence of 1.0

......Word 'Interactive' has a confidence of 1.0

......Word 'Data' has a confidence of 1.0

......Word 'File' has a confidence of 1.0

......Word 'required' has a confidence of 1.0

......Word 'to' has a confidence of 1.0

......Word 'be' has a confidence of 1.0

......Word 'submitted' has a confidence of 1.0

......Word 'pursuant' has a confidence of 1.0

......Word 'to' has a confidence of 1.0

......Word 'Rule' has a confidence of 1.0

...Line # 47 has word count 24 and text '405 of Regulation S-T (§232.405 of this chapter) during the preceding 12 months (or for such shorter period that the registrant was required to' within bounding box '[0.6517, 6.8863], [7.8459, 6.8863], [7.8459, 6.9972], [0.6517, 6.9972]'

......Word '405' has a confidence of 1.0

......Word 'of' has a confidence of 1.0

......Word 'Regulation' has a confidence of 1.0

......Word 'S-T' has a confidence of 1.0

......Word '(§232.405' has a confidence of 1.0

......Word 'of' has a confidence of 1.0

......Word 'this' has a confidence of 1.0

......Word 'chapter)' has a confidence of 1.0

......Word 'during' has a confidence of 1.0

......Word 'the' has a confidence of 1.0

......Word 'preceding' has a confidence of 1.0

......Word '12' has a confidence of 1.0

......Word 'months' has a confidence of 1.0

......Word '(or' has a confidence of 1.0

......Word 'for' has a confidence of 1.0

......Word 'such' has a confidence of 1.0

......Word 'shorter' has a confidence of 1.0

......Word 'period' has a confidence of 1.0

......Word 'that' has a confidence of 1.0

......Word 'the' has a confidence of 1.0

......Word 'registrant' has a confidence of 1.0

......Word 'was' has a confidence of 1.0

......Word 'required' has a confidence of 1.0

......Word 'to' has a confidence of 1.0

...Line # 48 has word count 3 and text 'submit such files).' within bounding box '[0.6538, 7.0296], [1.5594, 7.0296], [1.5594, 7.1405], [0.6538, 7.1405]'

......Word 'submit' has a confidence of 1.0

......Word 'such' has a confidence of 1.0

......Word 'files).' has a confidence of 1.0

...Line # 49 has word count 2 and text 'Yes ☒' within bounding box '[1.6938, 7.0289], [2.0577, 7.0289], [2.0577, 7.1171], [1.6938, 7.1171]'

......Word 'Yes' has a confidence of 1.0

......Word '☒' has a confidence of 1.0

...Line # 50 has word count 2 and text 'No ☐' within bounding box '[2.2066, 7.0276], [2.5115, 7.0276], [2.5115, 7.1171], [2.2066, 7.1171]'

......Word 'No' has a confidence of 1.0

......Word '☐' has a confidence of 1.0

...Line # 51 has word count 21 and text 'Indicate by check mark whether the registrant is a large accelerated filer, an accelerated filer, a non-accelerated filer, a smaller reporting' within bounding box '[0.6611, 7.2304], [7.8417, 7.2304], [7.8417, 7.3413], [0.6611, 7.3413]'

......Word 'Indicate' has a confidence of 1.0

......Word 'by' has a confidence of 1.0

......Word 'check' has a confidence of 1.0

......Word 'mark' has a confidence of 1.0

......Word 'whether' has a confidence of 1.0

......Word 'the' has a confidence of 1.0

......Word 'registrant' has a confidence of 1.0

......Word 'is' has a confidence of 1.0

......Word 'a' has a confidence of 1.0

......Word 'large' has a confidence of 1.0

......Word 'accelerated' has a confidence of 1.0

......Word 'filer,' has a confidence of 1.0

......Word 'an' has a confidence of 1.0

......Word 'accelerated' has a confidence of 1.0

......Word 'filer,' has a confidence of 1.0

......Word 'a' has a confidence of 1.0

......Word 'non-accelerated' has a confidence of 1.0

......Word 'filer,' has a confidence of 1.0

......Word 'a' has a confidence of 1.0

......Word 'smaller' has a confidence of 1.0

......Word 'reporting' has a confidence of 1.0

...Line # 52 has word count 18 and text 'company, or an emerging growth company. See the definitions of "large accelerated filer," "accelerated filer," "smaller reporting company,"' within bounding box '[0.6547, 7.3663], [7.8449, 7.3663], [7.8449, 7.4772], [0.6547, 7.4772]'

......Word 'company,' has a confidence of 1.0

......Word 'or' has a confidence of 1.0

......Word 'an' has a confidence of 1.0

......Word 'emerging' has a confidence of 1.0

......Word 'growth' has a confidence of 1.0

......Word 'company.' has a confidence of 1.0

......Word 'See' has a confidence of 1.0

......Word 'the' has a confidence of 1.0

......Word 'definitions' has a confidence of 1.0

......Word 'of' has a confidence of 1.0

......Word '"large' has a confidence of 1.0

......Word 'accelerated' has a confidence of 1.0

......Word 'filer,"' has a confidence of 1.0

......Word '"accelerated' has a confidence of 1.0

......Word 'filer,"' has a confidence of 1.0

......Word '"smaller' has a confidence of 1.0

......Word 'reporting' has a confidence of 1.0

......Word 'company,"' has a confidence of 1.0

...Line # 53 has word count 11 and text 'and "emerging growth company" in Rule 12b-2 of the Exchange Act.' within bounding box '[0.6543, 7.5021], [4.1966, 7.5021], [4.1966, 7.613], [0.6543, 7.613]'

......Word 'and' has a confidence of 1.0

......Word '"emerging' has a confidence of 1.0

......Word 'growth' has a confidence of 1.0

......Word 'company"' has a confidence of 1.0

......Word 'in' has a confidence of 1.0

......Word 'Rule' has a confidence of 1.0

......Word '12b-2' has a confidence of 1.0

......Word 'of' has a confidence of 1.0

......Word 'the' has a confidence of 1.0

......Word 'Exchange' has a confidence of 1.0

......Word 'Act.' has a confidence of 1.0

...Line # 54 has word count 4 and text 'Large accelerated filer ☒' within bounding box '[0.6586, 7.709], [1.9534, 7.709], [1.9534, 7.8206], [0.6586, 7.8206]'

......Word 'Large' has a confidence of 1.0

......Word 'accelerated' has a confidence of 1.0

......Word 'filer' has a confidence of 1.0

......Word '☒' has a confidence of 1.0

...Line # 55 has word count 3 and text 'Non-accelerated filer ☐' within bounding box '[0.659, 7.8827], [1.8756, 7.8827], [1.8756, 7.9721], [0.659, 7.9721]'

......Word 'Non-accelerated' has a confidence of 1.0

......Word 'filer' has a confidence of 1.0

......Word '☐' has a confidence of 1.0

...Line # 56 has word count 3 and text 'Accelerated filer ☐' within bounding box '[5.3358, 7.7077], [6.3181, 7.7077], [6.3181, 7.7971], [5.3358, 7.7971]'

......Word 'Accelerated' has a confidence of 1.0

......Word 'filer' has a confidence of 1.0

......Word '☐' has a confidence of 1.0

...Line # 57 has word count 4 and text 'Smaller reporting company ☐' within bounding box '[5.3412, 7.8827], [6.8831, 7.8827], [6.8831, 7.9956], [5.3412, 7.9956]'

......Word 'Smaller' has a confidence of 1.0

......Word 'reporting' has a confidence of 1.0

......Word 'company' has a confidence of 1.0

......Word '☐' has a confidence of 1.0

...Line # 58 has word count 4 and text 'Emerging growth company ☐' within bounding box '[5.3452, 8.0585], [6.8764, 8.0585], [6.8764, 8.1714], [5.3452, 8.1714]'

......Word 'Emerging' has a confidence of 1.0

......Word 'growth' has a confidence of 1.0

......Word 'company' has a confidence of 1.0

......Word '☐' has a confidence of 1.0

...Line # 59 has word count 23 and text 'If an emerging growth company, indicate by check mark if the registrant has elected not to use the extended transition period for complying' within bounding box '[0.661, 8.2614], [7.842, 8.2614], [7.842, 8.3722], [0.661, 8.3722]'

......Word 'If' has a confidence of 1.0

......Word 'an' has a confidence of 1.0

......Word 'emerging' has a confidence of 1.0

......Word 'growth' has a confidence of 1.0

......Word 'company,' has a confidence of 1.0

......Word 'indicate' has a confidence of 1.0

......Word 'by' has a confidence of 1.0

......Word 'check' has a confidence of 1.0

......Word 'mark' has a confidence of 1.0

......Word 'if' has a confidence of 1.0

......Word 'the' has a confidence of 1.0

......Word 'registrant' has a confidence of 1.0

......Word 'has' has a confidence of 1.0

......Word 'elected' has a confidence of 1.0

......Word 'not' has a confidence of 1.0

......Word 'to' has a confidence of 1.0

......Word 'use' has a confidence of 1.0

......Word 'the' has a confidence of 1.0

......Word 'extended' has a confidence of 1.0

......Word 'transition' has a confidence of 1.0

......Word 'period' has a confidence of 1.0

......Word 'for' has a confidence of 1.0

......Word 'complying' has a confidence of 1.0

...Line # 60 has word count 18 and text 'with any new or revised financial accounting standards provided pursuant to Section 13(a) of the Exchange Act. ☐' within bounding box '[0.6504, 8.4139], [6.6275, 8.4139], [6.6275, 8.5248], [0.6504, 8.5248]'

......Word 'with' has a confidence of 1.0

......Word 'any' has a confidence of 1.0

......Word 'new' has a confidence of 1.0

......Word 'or' has a confidence of 1.0

......Word 'revised' has a confidence of 1.0

......Word 'financial' has a confidence of 1.0

......Word 'accounting' has a confidence of 1.0

......Word 'standards' has a confidence of 1.0

......Word 'provided' has a confidence of 1.0

......Word 'pursuant' has a confidence of 1.0

......Word 'to' has a confidence of 1.0

......Word 'Section' has a confidence of 1.0

......Word '13(a)' has a confidence of 1.0

......Word 'of' has a confidence of 1.0

......Word 'the' has a confidence of 1.0

......Word 'Exchange' has a confidence of 1.0

......Word 'Act.' has a confidence of 1.0

......Word '☐' has a confidence of 1.0

...Line # 61 has word count 20 and text 'Indicate by check mark whether the registrant is a shell company (as defined in Rule 12b-2 of the Exchange Act).' within bounding box '[0.6609, 8.6172], [6.3833, 8.6172], [6.3833, 8.7281], [0.6609, 8.7281]'

......Word 'Indicate' has a confidence of 1.0

......Word 'by' has a confidence of 1.0

......Word 'check' has a confidence of 1.0

......Word 'mark' has a confidence of 1.0

......Word 'whether' has a confidence of 1.0

......Word 'the' has a confidence of 1.0

......Word 'registrant' has a confidence of 1.0

......Word 'is' has a confidence of 1.0

......Word 'a' has a confidence of 1.0

......Word 'shell' has a confidence of 1.0

......Word 'company' has a confidence of 1.0

......Word '(as' has a confidence of 1.0

......Word 'defined' has a confidence of 1.0

......Word 'in' has a confidence of 1.0

......Word 'Rule' has a confidence of 1.0

......Word '12b-2' has a confidence of 1.0

......Word 'of' has a confidence of 1.0

......Word 'the' has a confidence of 1.0

......Word 'Exchange' has a confidence of 1.0

......Word 'Act).' has a confidence of 1.0

...Line # 62 has word count 2 and text 'Yes ☐' within bounding box '[6.5188, 8.6152], [6.8824, 8.6152], [6.8824, 8.7046], [6.5188, 8.7046]'

......Word 'Yes' has a confidence of 1.0

......Word '☐' has a confidence of 1.0

...Line # 63 has word count 2 and text 'No ☒' within bounding box '[7.0307, 8.6165], [7.3351, 8.6165], [7.3351, 8.7046], [7.0307, 8.7046]'

......Word 'No' has a confidence of 1.0

......Word '☒' has a confidence of 1.0

...Line # 64 has word count 21 and text 'Indicate the number of shares outstanding of each of the issuer's classes of common stock, as of the latest practicable date.' within bounding box '[0.6611, 8.818], [6.9374, 8.818], [6.9374, 8.9289], [0.6611, 8.9289]'

......Word 'Indicate' has a confidence of 1.0

......Word 'the' has a confidence of 1.0

......Word 'number' has a confidence of 1.0

......Word 'of' has a confidence of 1.0

......Word 'shares' has a confidence of 1.0

......Word 'outstanding' has a confidence of 1.0

......Word 'of' has a confidence of 1.0

......Word 'each' has a confidence of 1.0

......Word 'of' has a confidence of 1.0

......Word 'the' has a confidence of 1.0

......Word 'issuer's' has a confidence of 1.0

......Word 'classes' has a confidence of 1.0

......Word 'of' has a confidence of 1.0

......Word 'common' has a confidence of 1.0

......Word 'stock,' has a confidence of 1.0

......Word 'as' has a confidence of 1.0

......Word 'of' has a confidence of 1.0

......Word 'the' has a confidence of 1.0

......Word 'latest' has a confidence of 1.0

......Word 'practicable' has a confidence of 1.0

......Word 'date.' has a confidence of 1.0

...Line # 65 has word count 1 and text 'Class' within bounding box '[0.6549, 9.0975], [0.9237, 9.0975], [0.9237, 9.1745], [0.6549, 9.1745]'

......Word 'Class' has a confidence of 1.0

...Line # 66 has word count 6 and text 'Outstanding as of April 24, 2020' within bounding box '[6.1303, 9.105], [7.707, 9.105], [7.707, 9.2026], [6.1303, 9.2026]'

......Word 'Outstanding' has a confidence of 1.0

......Word 'as' has a confidence of 1.0

......Word 'of' has a confidence of 1.0

......Word 'April' has a confidence of 1.0

......Word '24,' has a confidence of 1.0

......Word '2020' has a confidence of 1.0

...Line # 67 has word count 7 and text 'Common Stock, $0.00000625 par value per share' within bounding box '[0.6559, 9.3141], [3.2571, 9.3141], [3.2571, 9.4301], [0.6559, 9.4301]'

......Word 'Common' has a confidence of 1.0

......Word 'Stock,' has a confidence of 1.0

......Word '$0.00000625' has a confidence of 1.0

......Word 'par' has a confidence of 1.0

......Word 'value' has a confidence of 1.0

......Word 'per' has a confidence of 1.0

......Word 'share' has a confidence of 1.0

...Line # 68 has word count 2 and text '7,583,440,247 shares' within bounding box '[6.5747, 9.3458], [7.7069, 9.3458], [7.7069, 9.4475], [6.5747, 9.4475]'

......Word '7,583,440,247' has a confidence of 1.0

......Word 'shares' has a confidence of 1.0

...Selection mark is 'selected' within bounding box '[0.6694, 1.7746], [0.7764, 1.7746], [0.7764, 1.8833], [0.6694, 1.8833]' and has a confidence of 1.0

...Selection mark is 'unselected' within bounding box '[0.6694, 2.6955], [0.777, 2.6955], [0.777, 2.8042], [0.6694, 2.8042]' and has a confidence of 1.0

...Selection mark is 'selected' within bounding box '[4.2484, 6.549], [4.3353, 6.549], [4.3353, 6.6371], [4.2484, 6.6371]' and has a confidence of 1.0

...Selection mark is 'unselected' within bounding box '[4.7134, 6.5477], [4.8008, 6.5477], [4.8008, 6.6358], [4.7134, 6.6358]' and has a confidence of 1.0

...Selection mark is 'selected' within bounding box '[1.9708, 7.0289], [2.0577, 7.0289], [2.0577, 7.1171], [1.9708, 7.1171]' and has a confidence of 1.0

...Selection mark is 'unselected' within bounding box '[2.4242, 7.0276], [2.5115, 7.0276], [2.5115, 7.1158], [2.4242, 7.1158]' and has a confidence of 1.0

...Selection mark is 'selected' within bounding box '[1.8666, 7.709], [1.9534, 7.709], [1.9534, 7.7971], [1.8666, 7.7971]' and has a confidence of 1.0

...Selection mark is 'unselected' within bounding box '[6.2307, 7.7077], [6.3181, 7.7077], [6.3181, 7.7958], [6.2307, 7.7958]' and has a confidence of 1.0

...Selection mark is 'unselected' within bounding box '[1.7882, 7.8827], [1.8756, 7.8827], [1.8756, 7.9708], [1.7882, 7.9708]' and has a confidence of 1.0

...Selection mark is 'unselected' within bounding box '[6.7957, 7.8827], [6.8831, 7.8827], [6.8831, 7.9708], [6.7957, 7.9708]' and has a confidence of 1.0

...Selection mark is 'unselected' within bounding box '[6.7891, 8.0585], [6.8764, 8.0585], [6.8764, 8.1467], [6.7891, 8.1467]' and has a confidence of 1.0

...Selection mark is 'unselected' within bounding box '[6.5447, 8.416], [6.6275, 8.416], [6.6275, 8.5], [6.5447, 8.5]' and has a confidence of 1.0

...Selection mark is 'unselected' within bounding box '[6.795, 8.6152], [6.8824, 8.6152], [6.8824, 8.7033], [6.795, 8.7033]' and has a confidence of 1.0

...Selection mark is 'selected' within bounding box '[7.2483, 8.6165], [7.3351, 8.6165], [7.3351, 8.7046], [7.2483, 8.7046]' and has a confidence of 1.0

Table # 0 has 5 rows and 3 columns

Table # 0 location on page: 1 is [0.5806, 4.8968], [7.6778, 4.8993], [7.6769, 5.8015], [0.5802, 5.7994]

...Cell[0][0] has content 'Title of each class'

...content on page 1 is within bounding box '[0.5612, 4.8988], [3.8348, 4.8925], [3.8421, 5.1198], [0.5612, 5.1198]'

...Cell[0][1] has content 'Trading Symbol'

...content on page 1 is within bounding box '[3.8348, 4.8925], [5.3626, 4.8988], [5.3626, 5.1262], [3.8421, 5.1198]'

...Cell[0][2] has content 'Name of exchange on which registered'

...content on page 1 is within bounding box '[5.3626, 4.8988], [7.6832, 4.8988], [7.6832, 5.1262], [5.3626, 5.1262]'

...Cell[1][0] has content 'Common stock, $0.00000625 par value per share'

...content on page 1 is within bounding box '[0.5612, 5.1198], [3.8421, 5.1198], [3.8421, 5.2967], [0.5612, 5.2967]'

...Cell[1][1] has content 'MSFT'

...content on page 1 is within bounding box '[3.8421, 5.1198], [5.3626, 5.1262], [5.3626, 5.2967], [3.8421, 5.2967]'

...Cell[1][2] has content 'NASDAQ'

...content on page 1 is within bounding box '[5.3626, 5.1262], [7.6832, 5.1262], [7.6832, 5.303], [5.3626, 5.2967]'

...Cell[2][0] has content '2.125% Notes due 2021'

...content on page 1 is within bounding box '[0.5612, 5.2967], [3.8421, 5.2967], [3.8494, 5.4546], [0.5612, 5.4546]'

...Cell[2][1] has content 'MSFT'

...content on page 1 is within bounding box '[3.8421, 5.2967], [5.3626, 5.2967], [5.3626, 5.4546], [3.8494, 5.4546]'

...Cell[2][2] has content 'NASDAQ'

...content on page 1 is within bounding box '[5.3626, 5.2967], [7.6832, 5.303], [7.6832, 5.4546], [5.3626, 5.4546]'

...Cell[3][0] has content '3.125% Notes due 2028'

...content on page 1 is within bounding box '[0.5612, 5.4546], [3.8494, 5.4546], [3.8494, 5.6125], [0.5612, 5.6188]'

...Cell[3][1] has content 'MSFT'

...content on page 1 is within bounding box '[3.8494, 5.4546], [5.3626, 5.4546], [5.3626, 5.6125], [3.8494, 5.6125]'

...Cell[3][2] has content 'NASDAQ'

...content on page 1 is within bounding box '[5.3626, 5.4546], [7.6832, 5.4546], [7.6905, 5.6188], [5.3626, 5.6125]'

...Cell[4][0] has content '2.625% Notes due 2033'

...content on page 1 is within bounding box '[0.5612, 5.6188], [3.8494, 5.6125], [3.8494, 5.802], [0.5612, 5.802]'

...Cell[4][1] has content 'MSFT'

...content on page 1 is within bounding box '[3.8494, 5.6125], [5.3626, 5.6125], [5.3626, 5.802], [3.8494, 5.802]'

...Cell[4][2] has content 'NASDAQ'

...content on page 1 is within bounding box '[5.3626, 5.6125], [7.6905, 5.6188], [7.6905, 5.802], [5.3626, 5.802]'

Table # 1 has 2 rows and 2 columns

Table # 1 location on page: 1 is [0.6079, 9.0633], [7.8849, 9.0618], [7.8862, 9.5052], [0.6087, 9.5067]

...Cell[0][0] has content 'Class'

...content on page 1 is within bounding box '[0.5817, 9.0377], [4.7078, 9.0435], [4.7004, 9.2485], [0.5817, 9.2426]'

...Cell[0][1] has content 'Outstanding as of April 24, 2020'

...content on page 1 is within bounding box '[4.7078, 9.0435], [7.8341, 9.0494], [7.8416, 9.2485], [4.7004, 9.2485]'

...Cell[1][0] has content 'Common Stock, $0.00000625 par value per share'

...content on page 1 is within bounding box '[0.5817, 9.2426], [4.7004, 9.2485], [4.6929, 9.4886], [0.5817, 9.4827]'

...Cell[1][1] has content '7,583,440,247 shares'

...content on page 1 is within bounding box '[4.7004, 9.2485], [7.8416, 9.2485], [7.8491, 9.4886], [4.6929, 9.4886]'

---
