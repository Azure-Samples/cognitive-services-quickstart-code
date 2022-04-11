# Output: Python SDK prebuilt-read model (beta)

[Reference documentation](/python/api/azure-ai-formrecognizer/azure.ai.formrecognizer?view=azure-python-preview&preserve-view=true) | [Library source code](https://github.com/Azure/azure-sdk-for-python/tree/azure-ai-formrecognizer_3.2.0b3/sdk/formrecognizer/azure-ai-formrecognizer/) | [Package (PyPi)](https://pypi.org/project/azure-ai-formrecognizer/3.2.0b3/) | [Samples](https://github.com/Azure/azure-sdk-for-python/blob/azure-ai-formrecognizer_3.2.0b3/sdk/formrecognizer/azure-ai-formrecognizer/samples/README.md)

The prebuilt-read model extracts printed and handwritten textual elements including lines, words, locations, and detected languages from documents (PDF and TIFF) and images (JPG, PNG, and BMP). The read model is the foundation for all Form Recognizer models. Here is the expected outcome from the prebuilt-read how-to article:

Document contains content:  While healthcare is still in the early stages of its Al journey, we

are seeing pharmaceutical and other life sciences organizations

making major investments in Al and related technologies."

TOM LAWRY | National Director for Al, Health and Life Sciences | Microsoft

As pharmaceutical and other life sciences organizations invest

in and deploy advanced technologies, they are beginning to see

benefits in diverse areas across their organizations. Companies

are looking to incorporate automation and continuing smart

factory investments to reduce costs in drug discovery, research

and development, and manufacturing and supply chain

management. Many life sciences organizations are also choosing

to stay with more virtual approaches in the "new normal" -

particularly in clinical trials and sales and marketing areas.

Enhancing the patient

and provider experience

Clinical trial sponsors are continually seeking to make clinical

trials faster and to improve the experience for patients and

physicians. The COVID-19 pandemic has accelerated the

adoption of decentralized clinical trials, with an increase in

trial activities conducted remotely and in participants' homes.

In a Mckinsey survey,' up to 98 percent of patients reported

satisfaction with telemedicine. In the same report, 72 percent of

physicians surveyed reported similar or better experiences with

remote engagement compared with in-person visits.

The shift of trial activities closer to patients has been enabled by

a myriad of evolving technologies and services (e.g ., electronic

consent, telehealth and remote patient monitoring). The aim

to use technology to improve the patient experience and

convenience has also broadened trial access to reach a broader,

more diverse patient population.

"It's an interesting and exciting time right now," said Keren

Priyadarshini, Regional Business Lead - Asia, Worldwide

Health, Microsoft. "It used to be that physicians were key.

Now, suddenly, patients are feeling empowered by technology.

Pharmaceutical companies and other life sciences companies

are realizing they have to pay attention to the patient

experience in addition to the physician experience.

Enhanced patient experiences can be delivered in many different

ways. One example of a life sciences product that leverages the

intelligent cloud to directly affect the patient experience is the

Tandem® Diabetes Care insulin pump. The Tandem® t:slim X2

insulin pump with Basal-IQ technology enables patients with

Type 1 diabetes to predict and prevent the low levels of blood

sugar that cause hypoglycemia.2 The algorithm-driven, software-

updatable pump improves the patient experience by automating

chronic disease management and eliminating the need for

constant finger pricks to check glucose levels.

Tandem was able to create and deploy this innovation by

leveraging the Al and machine learning capabilities of the

intelligent cloud. As Al and other technologies continue to

advance, potential use cases will multiply. "Speed to value is

going to continue to accelerate," said Lawry.

In addition to enhancing the patient experience,

pharmaceutical and other life sciences companies can

leverage advanced technologies to improve relationships with

providers. For example, COVID-19 is driving changes in the

way companies interact with clinicians. Prior to COVID-19,

75 percent of physicians preferred in-person sales visits from

medtech reps; likewise, 77 percent of physicians preferred in-

person sales visits from pharma reps.3

Since the advent of COVID-19, however, physician

preferences are moving toward virtual visits. Only 53 percent

of physicians now express a preference for in-person visits

from medtech reps and only 40 percent prefer in-person visits

from pharma reps." That puts the onus on pharmaceutical and

life sciences organizations to deliver valuable and engaging

virtual visits to providers.

One way to do that is to leverage text analytics capabilities to

enhance the provider information stored in the organization's

customer relationship management (CRM) system. For

example, a rep setting up a visit with 'Dr. X' could run text

analytics on publicly available resources on the web to identify

on which specific topics Dr. X has been writing about and

commenting. "All kinds of publicly available information can

All kinds of publicly

available information

can be mined with text

analytics technology,

which can be used to arm the

sales rep with relevant information even

before he or she meets the doctor. It's a

totally different, digital game now."

KEREN PRIYADARSHINI | Regional Business Lead - Asia,

Worldwide Health | Microsoft

EMBRACING DIGITAL TRANSFORMATION IN LIFE SCIENCES ORGANIZATIONS

2

----Analyzing Read from page #1----

Page has width: 915.0 and height: 1190.0, measured with unit: pixel

...Line # 0 has text content 'While healthcare is still in the early stages of its Al journey, we' within bounding box '[259.0, 55.0], [816.0, 56.0], [816.0, 79.0], [259.0, 77.0]'

...Line # 1 has text content 'are seeing pharmaceutical and other life sciences organizations' within bounding box '[258.0, 83.0], [825.0, 83.0], [825.0, 106.0], [258.0, 106.0]'

...Line # 2 has text content 'making major investments in Al and related technologies."' within bounding box '[259.0, 112.0], [784.0, 112.0], [784.0, 136.0], [259.0, 136.0]'

...Line # 3 has text content 'TOM LAWRY | National Director for Al, Health and Life Sciences | Microsoft' within bounding box '[257.0, 151.0], [638.0, 152.0], [638.0, 168.0], [257.0, 167.0]'

...Line # 4 has text content 'As pharmaceutical and other life sciences organizations invest' within bounding box '[75.0, 241.0], [423.0, 241.0], [423.0, 258.0], [75.0, 257.0]'

...Line # 5 has text content 'in and deploy advanced technologies, they are beginning to see' within bounding box '[76.0, 259.0], [435.0, 260.0], [435.0, 277.0], [76.0, 276.0]'

...Line # 6 has text content 'benefits in diverse areas across their organizations. Companies' within bounding box '[75.0, 278.0], [427.0, 279.0], [427.0, 295.0], [75.0, 294.0]'

...Line # 7 has text content 'are looking to incorporate automation and continuing smart' within bounding box '[77.0, 297.0], [413.0, 297.0], [413.0, 313.0], [77.0, 312.0]'

...Line # 8 has text content 'factory investments to reduce costs in drug discovery, research' within bounding box '[76.0, 314.0], [427.0, 314.0], [427.0, 330.0], [76.0, 330.0]'

...Line # 9 has text content 'and development, and manufacturing and supply chain' within bounding box '[77.0, 332.0], [388.0, 332.0], [388.0, 349.0], [77.0, 349.0]'

...Line # 10 has text content 'management. Many life sciences organizations are also choosing' within bounding box '[76.0, 350.0], [440.0, 350.0], [440.0, 366.0], [76.0, 367.0]'

...Line # 11 has text content 'to stay with more virtual approaches in the "new normal" -' within bounding box '[76.0, 369.0], [404.0, 368.0], [404.0, 384.0], [76.0, 385.0]'

...Line # 12 has text content 'particularly in clinical trials and sales and marketing areas.' within bounding box '[76.0, 386.0], [396.0, 386.0], [396.0, 403.0], [76.0, 403.0]'

...Line # 13 has text content 'Enhancing the patient' within bounding box '[77.0, 422.0], [267.0, 423.0], [267.0, 447.0], [77.0, 445.0]'

...Line # 14 has text content 'and provider experience' within bounding box '[76.0, 447.0], [285.0, 448.0], [285.0, 469.0], [76.0, 468.0]'

...Line # 15 has text content 'Clinical trial sponsors are continually seeking to make clinical' within bounding box '[76.0, 482.0], [425.0, 481.0], [425.0, 499.0], [76.0, 499.0]'

...Line # 16 has text content 'trials faster and to improve the experience for patients and' within bounding box '[76.0, 501.0], [414.0, 501.0], [414.0, 517.0], [76.0, 517.0]'

...Line # 17 has text content 'physicians. The COVID-19 pandemic has accelerated the' within bounding box '[76.0, 517.0], [409.0, 517.0], [409.0, 535.0], [76.0, 535.0]'

...Line # 18 has text content 'adoption of decentralized clinical trials, with an increase in' within bounding box '[76.0, 536.0], [409.0, 536.0], [409.0, 553.0], [76.0, 553.0]'

...Line # 19 has text content 'trial activities conducted remotely and in participants' homes.' within bounding box '[76.0, 555.0], [429.0, 555.0], [429.0, 572.0], [76.0, 571.0]'

...Line # 20 has text content 'In a Mckinsey survey,' up to 98 percent of patients reported' within bounding box '[77.0, 572.0], [421.0, 573.0], [421.0, 590.0], [77.0, 589.0]'

...Line # 21 has text content 'satisfaction with telemedicine. In the same report, 72 percent of' within bounding box '[77.0, 591.0], [441.0, 592.0], [441.0, 608.0], [77.0, 607.0]'

...Line # 22 has text content 'physicians surveyed reported similar or better experiences with' within bounding box '[76.0, 610.0], [438.0, 609.0], [438.0, 625.0], [76.0, 626.0]'

...Line # 23 has text content 'remote engagement compared with in-person visits.' within bounding box '[76.0, 629.0], [377.0, 628.0], [377.0, 643.0], [76.0, 644.0]'

...Line # 24 has text content 'The shift of trial activities closer to patients has been enabled by' within bounding box '[74.0, 657.0], [443.0, 658.0], [443.0, 675.0], [74.0, 674.0]'

...Line # 25 has text content 'a myriad of evolving technologies and services (e.g ., electronic' within bounding box '[76.0, 676.0], [440.0, 676.0], [440.0, 693.0], [76.0, 693.0]'

...Line # 26 has text content 'consent, telehealth and remote patient monitoring). The aim' within bounding box '[77.0, 695.0], [427.0, 694.0], [427.0, 710.0], [77.0, 711.0]'

...Line # 27 has text content 'to use technology to improve the patient experience and' within bounding box '[76.0, 713.0], [405.0, 713.0], [405.0, 729.0], [76.0, 729.0]'

...Line # 28 has text content 'convenience has also broadened trial access to reach a broader,' within bounding box '[77.0, 730.0], [440.0, 730.0], [440.0, 746.0], [77.0, 746.0]'

...Line # 29 has text content 'more diverse patient population.' within bounding box '[77.0, 748.0], [264.0, 748.0], [264.0, 765.0], [77.0, 764.0]'

...Line # 30 has text content '"It's an interesting and exciting time right now," said Keren' within bounding box '[73.0, 780.0], [406.0, 780.0], [406.0, 797.0], [73.0, 797.0]'

...Line # 31 has text content 'Priyadarshini, Regional Business Lead - Asia, Worldwide' within bounding box '[76.0, 797.0], [401.0, 797.0], [401.0, 814.0], [76.0, 814.0]'

...Line # 32 has text content 'Health, Microsoft. "It used to be that physicians were key.' within bounding box '[76.0, 815.0], [407.0, 817.0], [406.0, 834.0], [76.0, 831.0]'

...Line # 33 has text content 'Now, suddenly, patients are feeling empowered by technology.' within bounding box '[77.0, 833.0], [437.0, 835.0], [437.0, 851.0], [77.0, 850.0]'

...Line # 34 has text content 'Pharmaceutical companies and other life sciences companies' within bounding box '[77.0, 852.0], [428.0, 853.0], [428.0, 869.0], [77.0, 868.0]'

...Line # 35 has text content 'are realizing they have to pay attention to the patient' within bounding box '[76.0, 870.0], [383.0, 870.0], [383.0, 887.0], [76.0, 887.0]'

...Line # 36 has text content 'experience in addition to the physician experience.' within bounding box '[77.0, 889.0], [370.0, 888.0], [370.0, 904.0], [77.0, 904.0]'

...Line # 37 has text content 'Enhanced patient experiences can be delivered in many different' within bounding box '[75.0, 919.0], [442.0, 919.0], [442.0, 935.0], [75.0, 935.0]'

...Line # 38 has text content 'ways. One example of a life sciences product that leverages the' within bounding box '[75.0, 937.0], [433.0, 937.0], [433.0, 954.0], [75.0, 954.0]'

...Line # 39 has text content 'intelligent cloud to directly affect the patient experience is the' within bounding box '[76.0, 955.0], [423.0, 955.0], [423.0, 972.0], [76.0, 972.0]'

...Line # 40 has text content 'Tandem® Diabetes Care insulin pump. The Tandem® t:slim X2' within bounding box '[74.0, 972.0], [423.0, 971.0], [423.0, 989.0], [74.0, 990.0]'

...Line # 41 has text content 'insulin pump with Basal-IQ technology enables patients with' within bounding box '[75.0, 991.0], [417.0, 991.0], [417.0, 1008.0], [75.0, 1008.0]'

...Line # 42 has text content 'Type 1 diabetes to predict and prevent the low levels of blood' within bounding box '[75.0, 1008.0], [419.0, 1008.0], [419.0, 1025.0], [75.0, 1026.0]'

...Line # 43 has text content 'sugar that cause hypoglycemia.2 The algorithm-driven, software-' within bounding box '[75.0, 1027.0], [439.0, 1026.0], [439.0, 1044.0], [75.0, 1045.0]'

...Line # 44 has text content 'updatable pump improves the patient experience by automating' within bounding box '[77.0, 1046.0], [439.0, 1046.0], [439.0, 1062.0], [77.0, 1062.0]'

...Line # 45 has text content 'chronic disease management and eliminating the need for' within bounding box '[76.0, 1063.0], [402.0, 1063.0], [402.0, 1080.0], [76.0, 1080.0]'

...Line # 46 has text content 'constant finger pricks to check glucose levels.' within bounding box '[77.0, 1081.0], [332.0, 1081.0], [332.0, 1098.0], [77.0, 1099.0]'

...Line # 47 has text content 'Tandem was able to create and deploy this innovation by' within bounding box '[466.0, 241.0], [793.0, 241.0], [793.0, 258.0], [466.0, 257.0]'

...Line # 48 has text content 'leveraging the Al and machine learning capabilities of the' within bounding box '[469.0, 260.0], [801.0, 260.0], [801.0, 276.0], [469.0, 277.0]'

...Line # 49 has text content 'intelligent cloud. As Al and other technologies continue to' within bounding box '[468.0, 278.0], [808.0, 278.0], [808.0, 295.0], [468.0, 294.0]'

...Line # 50 has text content 'advance, potential use cases will multiply. "Speed to value is' within bounding box '[468.0, 296.0], [811.0, 296.0], [811.0, 313.0], [468.0, 313.0]'

...Line # 51 has text content 'going to continue to accelerate," said Lawry.' within bounding box '[468.0, 316.0], [722.0, 315.0], [722.0, 330.0], [468.0, 332.0]'

...Line # 52 has text content 'In addition to enhancing the patient experience,' within bounding box '[467.0, 344.0], [746.0, 346.0], [746.0, 362.0], [467.0, 360.0]'

...Line # 53 has text content 'pharmaceutical and other life sciences companies can' within bounding box '[469.0, 363.0], [778.0, 364.0], [778.0, 380.0], [469.0, 379.0]'

...Line # 54 has text content 'leverage advanced technologies to improve relationships with' within bounding box '[466.0, 381.0], [824.0, 381.0], [824.0, 398.0], [466.0, 398.0]'

...Line # 55 has text content 'providers. For example, COVID-19 is driving changes in the' within bounding box '[467.0, 399.0], [814.0, 398.0], [814.0, 416.0], [467.0, 416.0]'

...Line # 56 has text content 'way companies interact with clinicians. Prior to COVID-19,' within bounding box '[468.0, 418.0], [806.0, 416.0], [806.0, 433.0], [468.0, 435.0]'

...Line # 57 has text content '75 percent of physicians preferred in-person sales visits from' within bounding box '[467.0, 435.0], [815.0, 435.0], [815.0, 452.0], [467.0, 452.0]'

...Line # 58 has text content 'medtech reps; likewise, 77 percent of physicians preferred in-' within bounding box '[468.0, 453.0], [818.0, 453.0], [818.0, 470.0], [468.0, 470.0]'

...Line # 59 has text content 'person sales visits from pharma reps.3' within bounding box '[468.0, 472.0], [685.0, 472.0], [685.0, 487.0], [468.0, 488.0]'

...Line # 60 has text content 'Since the advent of COVID-19, however, physician' within bounding box '[468.0, 502.0], [770.0, 503.0], [770.0, 520.0], [468.0, 519.0]'

...Line # 61 has text content 'preferences are moving toward virtual visits. Only 53 percent' within bounding box '[468.0, 521.0], [828.0, 521.0], [828.0, 537.0], [468.0, 538.0]'

...Line # 62 has text content 'of physicians now express a preference for in-person visits' within bounding box '[468.0, 539.0], [811.0, 539.0], [811.0, 556.0], [468.0, 556.0]'

...Line # 63 has text content 'from medtech reps and only 40 percent prefer in-person visits' within bounding box '[468.0, 557.0], [834.0, 557.0], [834.0, 575.0], [468.0, 574.0]'

...Line # 64 has text content 'from pharma reps." That puts the onus on pharmaceutical and' within bounding box '[469.0, 575.0], [835.0, 575.0], [835.0, 592.0], [469.0, 592.0]'

...Line # 65 has text content 'life sciences organizations to deliver valuable and engaging' within bounding box '[469.0, 593.0], [818.0, 594.0], [818.0, 610.0], [469.0, 609.0]'

...Line # 66 has text content 'virtual visits to providers.' within bounding box '[468.0, 611.0], [615.0, 612.0], [615.0, 627.0], [468.0, 626.0]'

...Line # 67 has text content 'One way to do that is to leverage text analytics capabilities to' within bounding box '[468.0, 642.0], [822.0, 643.0], [822.0, 659.0], [468.0, 658.0]'

...Line # 68 has text content 'enhance the provider information stored in the organization's' within bounding box '[469.0, 661.0], [820.0, 660.0], [820.0, 676.0], [469.0, 676.0]'

...Line # 69 has text content 'customer relationship management (CRM) system. For' within bounding box '[469.0, 679.0], [788.0, 678.0], [788.0, 695.0], [469.0, 696.0]'

...Line # 70 has text content 'example, a rep setting up a visit with 'Dr. X' could run text' within bounding box '[470.0, 697.0], [802.0, 696.0], [802.0, 712.0], [470.0, 714.0]'

...Line # 71 has text content 'analytics on publicly available resources on the web to identify' within bounding box '[468.0, 715.0], [825.0, 714.0], [825.0, 730.0], [468.0, 731.0]'

...Line # 72 has text content 'on which specific topics Dr. X has been writing about and' within bounding box '[468.0, 732.0], [800.0, 731.0], [800.0, 749.0], [468.0, 749.0]'

...Line # 73 has text content 'commenting. "All kinds of publicly available information can' within bounding box '[468.0, 749.0], [813.0, 749.0], [813.0, 767.0], [468.0, 768.0]'

...Line # 74 has text content 'All kinds of publicly' within bounding box '[467.0, 827.0], [645.0, 829.0], [645.0, 851.0], [467.0, 849.0]'

...Line # 75 has text content 'available information' within bounding box '[469.0, 857.0], [661.0, 857.0], [661.0, 877.0], [469.0, 877.0]'

...Line # 76 has text content 'can be mined with text' within bounding box '[468.0, 886.0], [674.0, 886.0], [674.0, 906.0], [468.0, 906.0]'

...Line # 77 has text content 'analytics technology,' within bounding box '[468.0, 915.0], [660.0, 915.0], [660.0, 938.0], [468.0, 937.0]'

...Line # 78 has text content 'which can be used to arm the' within bounding box '[468.0, 942.0], [731.0, 943.0], [731.0, 964.0], [468.0, 963.0]'

...Line # 79 has text content 'sales rep with relevant information even' within bounding box '[468.0, 971.0], [830.0, 971.0], [830.0, 993.0], [468.0, 992.0]'

...Line # 80 has text content 'before he or she meets the doctor. It's a' within bounding box '[469.0, 999.0], [823.0, 1000.0], [823.0, 1021.0], [469.0, 1020.0]'

...Line # 81 has text content 'totally different, digital game now."' within bounding box '[469.0, 1028.0], [788.0, 1028.0], [788.0, 1052.0], [469.0, 1052.0]'

...Line # 82 has text content 'KEREN PRIYADARSHINI | Regional Business Lead - Asia,' within bounding box '[469.0, 1068.0], [760.0, 1069.0], [760.0, 1084.0], [469.0, 1083.0]'

...Line # 83 has text content 'Worldwide Health | Microsoft' within bounding box '[468.0, 1089.0], [615.0, 1089.0], [615.0, 1104.0], [468.0, 1103.0]'

...Line # 84 has text content 'EMBRACING DIGITAL TRANSFORMATION IN LIFE SCIENCES ORGANIZATIONS' within bounding box '[77.0, 1140.0], [451.0, 1140.0], [451.0, 1153.0], [77.0, 1153.0]'

...Line # 85 has text content '2' within bounding box '[812.0, 1139.0], [824.0, 1140.0], [821.0, 1154.0], [810.0, 1152.0]'

...Word 'While' has a confidence of 0.999

...Word 'healthcare' has a confidence of 0.995

...Word 'is' has a confidence of 0.997

...Word 'still' has a confidence of 0.999

...Word 'in' has a confidence of 0.998

...Word 'the' has a confidence of 0.998

...Word 'early' has a confidence of 0.999

...Word 'stages' has a confidence of 0.994

...Word 'of' has a confidence of 0.998

...Word 'its' has a confidence of 0.998

...Word 'Al' has a confidence of 0.879

...Word 'journey,' has a confidence of 0.994

...Word 'we' has a confidence of 0.994

...Word 'are' has a confidence of 0.997

...Word 'seeing' has a confidence of 0.995

...Word 'pharmaceutical' has a confidence of 0.985

...Word 'and' has a confidence of 0.998

...Word 'other' has a confidence of 0.999

...Word 'life' has a confidence of 0.994

...Word 'sciences' has a confidence of 0.996

...Word 'organizations' has a confidence of 0.994

...Word 'making' has a confidence of 0.999

...Word 'major' has a confidence of 0.999

...Word 'investments' has a confidence of 0.994

...Word 'in' has a confidence of 0.999

...Word 'Al' has a confidence of 0.922

...Word 'and' has a confidence of 0.998

...Word 'related' has a confidence of 0.998

...Word 'technologies."' has a confidence of 0.994

...Word 'TOM' has a confidence of 0.998

...Word 'LAWRY' has a confidence of 0.999

...Word '|' has a confidence of 0.972

...Word 'National' has a confidence of 0.994

...Word 'Director' has a confidence of 0.995

...Word 'for' has a confidence of 0.997

...Word 'Al,' has a confidence of 0.815

...Word 'Health' has a confidence of 0.994

...Word 'and' has a confidence of 0.998

...Word 'Life' has a confidence of 0.993

...Word 'Sciences' has a confidence of 0.995

...Word '|' has a confidence of 0.97

...Word 'Microsoft' has a confidence of 0.995

...Word 'As' has a confidence of 0.994

...Word 'pharmaceutical' has a confidence of 0.994

...Word 'and' has a confidence of 0.998

...Word 'other' has a confidence of 0.998

...Word 'life' has a confidence of 0.993

...Word 'sciences' has a confidence of 0.995

...Word 'organizations' has a confidence of 0.994

...Word 'invest' has a confidence of 0.994

...Word 'in' has a confidence of 0.997

...Word 'and' has a confidence of 0.998

...Word 'deploy' has a confidence of 0.999

...Word 'advanced' has a confidence of 0.996

...Word 'technologies,' has a confidence of 0.994

...Word 'they' has a confidence of 0.99

...Word 'are' has a confidence of 0.998

...Word 'beginning' has a confidence of 0.996

...Word 'to' has a confidence of 0.998

...Word 'see' has a confidence of 0.997

...Word 'benefits' has a confidence of 0.995

...Word 'in' has a confidence of 0.997

...Word 'diverse' has a confidence of 0.994

...Word 'areas' has a confidence of 0.999

...Word 'across' has a confidence of 0.998

...Word 'their' has a confidence of 0.998

...Word 'organizations.' has a confidence of 0.994

...Word 'Companies' has a confidence of 0.993

...Word 'are' has a confidence of 0.998

...Word 'looking' has a confidence of 0.997

...Word 'to' has a confidence of 0.997

...Word 'incorporate' has a confidence of 0.994

...Word 'automation' has a confidence of 0.995

...Word 'and' has a confidence of 0.998

...Word 'continuing' has a confidence of 0.995

...Word 'smart' has a confidence of 0.999

...Word 'factory' has a confidence of 0.997

...Word 'investments' has a confidence of 0.979

...Word 'to' has a confidence of 0.997

...Word 'reduce' has a confidence of 0.992

...Word 'costs' has a confidence of 0.998

...Word 'in' has a confidence of 0.998

...Word 'drug' has a confidence of 0.987

...Word 'discovery,' has a confidence of 0.99

...Word 'research' has a confidence of 0.997

...Word 'and' has a confidence of 0.994

...Word 'development,' has a confidence of 0.994

...Word 'and' has a confidence of 0.998

...Word 'manufacturing' has a confidence of 0.994

...Word 'and' has a confidence of 0.998

...Word 'supply' has a confidence of 0.994

...Word 'chain' has a confidence of 0.999

...Word 'management.' has a confidence of 0.994

...Word 'Many' has a confidence of 0.994

...Word 'life' has a confidence of 0.994

...Word 'sciences' has a confidence of 0.995

...Word 'organizations' has a confidence of 0.994

...Word 'are' has a confidence of 0.998

...Word 'also' has a confidence of 0.993

...Word 'choosing' has a confidence of 0.997

...Word 'to' has a confidence of 0.997

...Word 'stay' has a confidence of 0.994

...Word 'with' has a confidence of 0.994

...Word 'more' has a confidence of 0.994

...Word 'virtual' has a confidence of 0.995

...Word 'approaches' has a confidence of 0.995

...Word 'in' has a confidence of 0.997

...Word 'the' has a confidence of 0.997

...Word '"new' has a confidence of 0.969

...Word 'normal"' has a confidence of 0.985

...Word '-' has a confidence of 0.895

...Word 'particularly' has a confidence of 0.994

...Word 'in' has a confidence of 0.997

...Word 'clinical' has a confidence of 0.986

...Word 'trials' has a confidence of 0.986

...Word 'and' has a confidence of 0.998

...Word 'sales' has a confidence of 0.999

...Word 'and' has a confidence of 0.998

...Word 'marketing' has a confidence of 0.994

...Word 'areas.' has a confidence of 0.994

...Word 'Enhancing' has a confidence of 0.994

...Word 'the' has a confidence of 0.999

...Word 'patient' has a confidence of 0.998

...Word 'and' has a confidence of 0.998

...Word 'provider' has a confidence of 0.996

...Word 'experience' has a confidence of 0.996

...Word 'Clinical' has a confidence of 0.996

...Word 'trial' has a confidence of 0.998

...Word 'sponsors' has a confidence of 0.995

...Word 'are' has a confidence of 0.998

...Word 'continually' has a confidence of 0.994

...Word 'seeking' has a confidence of 0.997

...Word 'to' has a confidence of 0.998

...Word 'make' has a confidence of 0.994

...Word 'clinical' has a confidence of 0.996

...Word 'trials' has a confidence of 0.994

...Word 'faster' has a confidence of 0.999

...Word 'and' has a confidence of 0.998

...Word 'to' has a confidence of 0.997

...Word 'improve' has a confidence of 0.997

...Word 'the' has a confidence of 0.994

...Word 'experience' has a confidence of 0.995

...Word 'for' has a confidence of 0.999

...Word 'patients' has a confidence of 0.996

...Word 'and' has a confidence of 0.998

...Word 'physicians.' has a confidence of 0.994

...Word 'The' has a confidence of 0.994

...Word 'COVID-19' has a confidence of 0.995

...Word 'pandemic' has a confidence of 0.994

...Word 'has' has a confidence of 0.997

...Word 'accelerated' has a confidence of 0.995

...Word 'the' has a confidence of 0.998

...Word 'adoption' has a confidence of 0.996

...Word 'of' has a confidence of 0.998

...Word 'decentralized' has a confidence of 0.987

...Word 'clinical' has a confidence of 0.973

...Word 'trials,' has a confidence of 0.994

...Word 'with' has a confidence of 0.994

...Word 'an' has a confidence of 0.997

...Word 'increase' has a confidence of 0.997

...Word 'in' has a confidence of 0.998

...Word 'trial' has a confidence of 0.994

...Word 'activities' has a confidence of 0.994

...Word 'conducted' has a confidence of 0.995

...Word 'remotely' has a confidence of 0.996

...Word 'and' has a confidence of 0.998

...Word 'in' has a confidence of 0.998

...Word 'participants'' has a confidence of 0.985

...Word 'homes.' has a confidence of 0.994

...Word 'In' has a confidence of 0.98

...Word 'a' has a confidence of 0.994

...Word 'Mckinsey' has a confidence of 0.993

...Word 'survey,'' has a confidence of 0.12

...Word 'up' has a confidence of 0.996

...Word 'to' has a confidence of 0.997

...Word '98' has a confidence of 0.998

...Word 'percent' has a confidence of 0.996

...Word 'of' has a confidence of 0.997

...Word 'patients' has a confidence of 0.996

...Word 'reported' has a confidence of 0.997

...Word 'satisfaction' has a confidence of 0.993

...Word 'with' has a confidence of 0.994

...Word 'telemedicine.' has a confidence of 0.994

...Word 'In' has a confidence of 0.986

...Word 'the' has a confidence of 0.997

...Word 'same' has a confidence of 0.994

...Word 'report,' has a confidence of 0.996

...Word '72' has a confidence of 0.996

...Word 'percent' has a confidence of 0.998

...Word 'of' has a confidence of 0.98

...Word 'physicians' has a confidence of 0.994

...Word 'surveyed' has a confidence of 0.995

...Word 'reported' has a confidence of 0.996

...Word 'similar' has a confidence of 0.996

...Word 'or' has a confidence of 0.996

...Word 'better' has a confidence of 0.999

...Word 'experiences' has a confidence of 0.995

...Word 'with' has a confidence of 0.994

...Word 'remote' has a confidence of 0.994

...Word 'engagement' has a confidence of 0.995

...Word 'compared' has a confidence of 0.997

...Word 'with' has a confidence of 0.994

...Word 'in-person' has a confidence of 0.996

...Word 'visits.' has a confidence of 0.994

...Word 'The' has a confidence of 0.994

...Word 'shift' has a confidence of 0.999

...Word 'of' has a confidence of 0.994

...Word 'trial' has a confidence of 0.967

...Word 'activities' has a confidence of 0.994

...Word 'closer' has a confidence of 0.994

...Word 'to' has a confidence of 0.997

...Word 'patients' has a confidence of 0.994

...Word 'has' has a confidence of 0.997

...Word 'been' has a confidence of 0.994

...Word 'enabled' has a confidence of 0.997

...Word 'by' has a confidence of 0.998

...Word 'a' has a confidence of 0.994

...Word 'myriad' has a confidence of 0.997

...Word 'of' has a confidence of 0.998

...Word 'evolving' has a confidence of 0.996

...Word 'technologies' has a confidence of 0.994

...Word 'and' has a confidence of 0.998

...Word 'services' has a confidence of 0.996

...Word '(e.g' has a confidence of 0.955

...Word '.,' has a confidence of 0.967

...Word 'electronic' has a confidence of 0.995

...Word 'consent,' has a confidence of 0.994

...Word 'telehealth' has a confidence of 0.994

...Word 'and' has a confidence of 0.998

...Word 'remote' has a confidence of 0.994

...Word 'patient' has a confidence of 0.997

...Word 'monitoring).' has a confidence of 0.994

...Word 'The' has a confidence of 0.998

...Word 'aim' has a confidence of 0.994

...Word 'to' has a confidence of 0.997

...Word 'use' has a confidence of 0.998

...Word 'technology' has a confidence of 0.995

...Word 'to' has a confidence of 0.997

...Word 'improve' has a confidence of 0.997

...Word 'the' has a confidence of 0.997

...Word 'patient' has a confidence of 0.97

...Word 'experience' has a confidence of 0.995

...Word 'and' has a confidence of 0.998

...Word 'convenience' has a confidence of 0.992

...Word 'has' has a confidence of 0.997

...Word 'also' has a confidence of 0.994

...Word 'broadened' has a confidence of 0.995

...Word 'trial' has a confidence of 0.997

...Word 'access' has a confidence of 0.999

...Word 'to' has a confidence of 0.998

...Word 'reach' has a confidence of 0.999

...Word 'a' has a confidence of 0.994

...Word 'broader,' has a confidence of 0.97

...Word 'more' has a confidence of 0.994

...Word 'diverse' has a confidence of 0.997

...Word 'patient' has a confidence of 0.997

...Word 'population.' has a confidence of 0.985

...Word '"It's' has a confidence of 0.94

...Word 'an' has a confidence of 0.996

...Word 'interesting' has a confidence of 0.994

...Word 'and' has a confidence of 0.998

...Word 'exciting' has a confidence of 0.996

...Word 'time' has a confidence of 0.994

...Word 'right' has a confidence of 0.999

...Word 'now,"' has a confidence of 0.907

...Word 'said' has a confidence of 0.991

...Word 'Keren' has a confidence of 0.999

...Word 'Priyadarshini,' has a confidence of 0.982

...Word 'Regional' has a confidence of 0.973

...Word 'Business' has a confidence of 0.995

...Word 'Lead' has a confidence of 0.994

...Word '-' has a confidence of 0.994

...Word 'Asia,' has a confidence of 0.997

...Word 'Worldwide' has a confidence of 0.991

...Word 'Health,' has a confidence of 0.989

...Word 'Microsoft.' has a confidence of 0.99

...Word '"It' has a confidence of 0.894

...Word 'used' has a confidence of 0.994

...Word 'to' has a confidence of 0.997

...Word 'be' has a confidence of 0.996

...Word 'that' has a confidence of 0.994

...Word 'physicians' has a confidence of 0.994

...Word 'were' has a confidence of 0.994

...Word 'key.' has a confidence of 0.973

...Word 'Now,' has a confidence of 0.992

...Word 'suddenly,' has a confidence of 0.995

...Word 'patients' has a confidence of 0.995

...Word 'are' has a confidence of 0.999

...Word 'feeling' has a confidence of 0.997

...Word 'empowered' has a confidence of 0.996

...Word 'by' has a confidence of 0.998

...Word 'technology.' has a confidence of 0.988

...Word 'Pharmaceutical' has a confidence of 0.994

...Word 'companies' has a confidence of 0.995

...Word 'and' has a confidence of 0.998

...Word 'other' has a confidence of 0.997

...Word 'life' has a confidence of 0.992

...Word 'sciences' has a confidence of 0.993

...Word 'companies' has a confidence of 0.996

...Word 'are' has a confidence of 0.998

...Word 'realizing' has a confidence of 0.995

...Word 'they' has a confidence of 0.994

...Word 'have' has a confidence of 0.994

...Word 'to' has a confidence of 0.997

...Word 'pay' has a confidence of 0.998

...Word 'attention' has a confidence of 0.996

...Word 'to' has a confidence of 0.988

...Word 'the' has a confidence of 0.998

...Word 'patient' has a confidence of 0.994

...Word 'experience' has a confidence of 0.985

...Word 'in' has a confidence of 0.997

...Word 'addition' has a confidence of 0.995

...Word 'to' has a confidence of 0.997

...Word 'the' has a confidence of 0.994

...Word 'physician' has a confidence of 0.995

...Word 'experience.' has a confidence of 0.994

...Word 'Enhanced' has a confidence of 0.994

...Word 'patient' has a confidence of 0.996

...Word 'experiences' has a confidence of 0.994

...Word 'can' has a confidence of 0.998

...Word 'be' has a confidence of 0.995

...Word 'delivered' has a confidence of 0.995

...Word 'in' has a confidence of 0.958

...Word 'many' has a confidence of 0.994

...Word 'different' has a confidence of 0.967

...Word 'ways.' has a confidence of 0.996

...Word 'One' has a confidence of 0.997

...Word 'example' has a confidence of 0.997

...Word 'of' has a confidence of 0.998

...Word 'a' has a confidence of 0.994

...Word 'life' has a confidence of 0.994

...Word 'sciences' has a confidence of 0.996

...Word 'product' has a confidence of 0.997

...Word 'that' has a confidence of 0.994

...Word 'leverages' has a confidence of 0.996

...Word 'the' has a confidence of 0.999

...Word 'intelligent' has a confidence of 0.99

...Word 'cloud' has a confidence of 0.998

...Word 'to' has a confidence of 0.997

...Word 'directly' has a confidence of 0.996

...Word 'affect' has a confidence of 0.999

...Word 'the' has a confidence of 0.997

...Word 'patient' has a confidence of 0.994

...Word 'experience' has a confidence of 0.996

...Word 'is' has a confidence of 0.997

...Word 'the' has a confidence of 0.998

...Word 'Tandem®' has a confidence of 0.694

...Word 'Diabetes' has a confidence of 0.978

...Word 'Care' has a confidence of 0.993

...Word 'insulin' has a confidence of 0.996

...Word 'pump.' has a confidence of 0.994

...Word 'The' has a confidence of 0.997

...Word 'Tandem®' has a confidence of 0.635

...Word 't:slim' has a confidence of 0.97

...Word 'X2' has a confidence of 0.979

...Word 'insulin' has a confidence of 0.997

...Word 'pump' has a confidence of 0.993

...Word 'with' has a confidence of 0.994

...Word 'Basal-IQ' has a confidence of 0.93

...Word 'technology' has a confidence of 0.994

...Word 'enables' has a confidence of 0.997

...Word 'patients' has a confidence of 0.996

...Word 'with' has a confidence of 0.994

...Word 'Type' has a confidence of 0.993

...Word '1' has a confidence of 0.994

...Word 'diabetes' has a confidence of 0.989

...Word 'to' has a confidence of 0.997

...Word 'predict' has a confidence of 0.996

...Word 'and' has a confidence of 0.998

...Word 'prevent' has a confidence of 0.998

...Word 'the' has a confidence of 0.998

...Word 'low' has a confidence of 0.998

...Word 'levels' has a confidence of 0.999

...Word 'of' has a confidence of 0.963

...Word 'blood' has a confidence of 0.999

...Word 'sugar' has a confidence of 0.999

...Word 'that' has a confidence of 0.994

...Word 'cause' has a confidence of 0.999

...Word 'hypoglycemia.2' has a confidence of 0.98

...Word 'The' has a confidence of 0.998

...Word 'algorithm-driven,' has a confidence of 0.994

...Word 'software-' has a confidence of 0.994

...Word 'updatable' has a confidence of 0.992

...Word 'pump' has a confidence of 0.993

...Word 'improves' has a confidence of 0.995

...Word 'the' has a confidence of 0.997

...Word 'patient' has a confidence of 0.996

...Word 'experience' has a confidence of 0.992

...Word 'by' has a confidence of 0.997

...Word 'automating' has a confidence of 0.994

...Word 'chronic' has a confidence of 0.993

...Word 'disease' has a confidence of 0.997

...Word 'management' has a confidence of 0.995

...Word 'and' has a confidence of 0.998

...Word 'eliminating' has a confidence of 0.994

...Word 'the' has a confidence of 0.998

...Word 'need' has a confidence of 0.994

...Word 'for' has a confidence of 0.998

...Word 'constant' has a confidence of 0.994

...Word 'finger' has a confidence of 1.0

...Word 'pricks' has a confidence of 0.999

...Word 'to' has a confidence of 0.998

...Word 'check' has a confidence of 0.999

...Word 'glucose' has a confidence of 0.998

...Word 'levels.' has a confidence of 0.994

...Word 'Tandem' has a confidence of 0.998

...Word 'was' has a confidence of 0.998

...Word 'able' has a confidence of 0.991

...Word 'to' has a confidence of 0.997

...Word 'create' has a confidence of 0.975

...Word 'and' has a confidence of 0.998

...Word 'deploy' has a confidence of 0.999

...Word 'this' has a confidence of 0.993

...Word 'innovation' has a confidence of 0.965

...Word 'by' has a confidence of 0.998

...Word 'leveraging' has a confidence of 0.994

...Word 'the' has a confidence of 0.997

...Word 'Al' has a confidence of 0.949

...Word 'and' has a confidence of 0.999

...Word 'machine' has a confidence of 0.993

...Word 'learning' has a confidence of 0.944

...Word 'capabilities' has a confidence of 0.994

...Word 'of' has a confidence of 0.994

...Word 'the' has a confidence of 0.998

...Word 'intelligent' has a confidence of 0.994

...Word 'cloud.' has a confidence of 0.97

...Word 'As' has a confidence of 0.989

...Word 'Al' has a confidence of 0.948

...Word 'and' has a confidence of 0.999

...Word 'other' has a confidence of 0.999

...Word 'technologies' has a confidence of 0.994

...Word 'continue' has a confidence of 0.996

...Word 'to' has a confidence of 0.998

...Word 'advance,' has a confidence of 0.996

...Word 'potential' has a confidence of 0.995

...Word 'use' has a confidence of 0.946

...Word 'cases' has a confidence of 0.998

...Word 'will' has a confidence of 0.994

...Word 'multiply.' has a confidence of 0.994

...Word '"Speed' has a confidence of 0.984

...Word 'to' has a confidence of 0.997

...Word 'value' has a confidence of 0.999

...Word 'is' has a confidence of 0.997

...Word 'going' has a confidence of 0.998

...Word 'to' has a confidence of 0.998

...Word 'continue' has a confidence of 0.996

...Word 'to' has a confidence of 0.998

...Word 'accelerate,"' has a confidence of 0.984

...Word 'said' has a confidence of 0.992

...Word 'Lawry.' has a confidence of 0.967

...Word 'In' has a confidence of 0.995

...Word 'addition' has a confidence of 0.996

...Word 'to' has a confidence of 0.997

...Word 'enhancing' has a confidence of 0.995

...Word 'the' has a confidence of 0.997

...Word 'patient' has a confidence of 0.997

...Word 'experience,' has a confidence of 0.994

...Word 'pharmaceutical' has a confidence of 0.994

...Word 'and' has a confidence of 0.998

...Word 'other' has a confidence of 0.997

...Word 'life' has a confidence of 0.993

...Word 'sciences' has a confidence of 0.995

...Word 'companies' has a confidence of 0.996

...Word 'can' has a confidence of 0.998

...Word 'leverage' has a confidence of 0.994

...Word 'advanced' has a confidence of 0.996

...Word 'technologies' has a confidence of 0.994

...Word 'to' has a confidence of 0.997

...Word 'improve' has a confidence of 0.997

...Word 'relationships' has a confidence of 0.994

...Word 'with' has a confidence of 0.994

...Word 'providers.' has a confidence of 0.994

...Word 'For' has a confidence of 0.998

...Word 'example,' has a confidence of 0.995

...Word 'COVID-19' has a confidence of 0.994

...Word 'is' has a confidence of 0.997

...Word 'driving' has a confidence of 0.996

...Word 'changes' has a confidence of 0.998

...Word 'in' has a confidence of 0.997

...Word 'the' has a confidence of 0.998

...Word 'way' has a confidence of 0.998

...Word 'companies' has a confidence of 0.995

...Word 'interact' has a confidence of 0.996

...Word 'with' has a confidence of 0.994

...Word 'clinicians.' has a confidence of 0.93

...Word 'Prior' has a confidence of 0.997

...Word 'to' has a confidence of 0.997

...Word 'COVID-19,' has a confidence of 0.968

...Word '75' has a confidence of 0.996

...Word 'percent' has a confidence of 0.997

...Word 'of' has a confidence of 0.994

...Word 'physicians' has a confidence of 0.994

...Word 'preferred' has a confidence of 0.995

...Word 'in-person' has a confidence of 0.995

...Word 'sales' has a confidence of 0.999

...Word 'visits' has a confidence of 0.999

...Word 'from' has a confidence of 0.994

...Word 'medtech' has a confidence of 0.998

...Word 'reps;' has a confidence of 0.979

...Word 'likewise,' has a confidence of 0.996

...Word '77' has a confidence of 0.997

...Word 'percent' has a confidence of 0.998

...Word 'of' has a confidence of 0.991

...Word 'physicians' has a confidence of 0.994

...Word 'preferred' has a confidence of 0.996

...Word 'in-' has a confidence of 0.997

...Word 'person' has a confidence of 0.999

...Word 'sales' has a confidence of 0.998

...Word 'visits' has a confidence of 0.997

...Word 'from' has a confidence of 0.992

...Word 'pharma' has a confidence of 0.999

...Word 'reps.3' has a confidence of 0.726

...Word 'Since' has a confidence of 0.999

...Word 'the' has a confidence of 0.987

...Word 'advent' has a confidence of 0.98

...Word 'of' has a confidence of 0.998

...Word 'COVID-19,' has a confidence of 0.993

...Word 'however,' has a confidence of 0.996

...Word 'physician' has a confidence of 0.993

...Word 'preferences' has a confidence of 0.994

...Word 'are' has a confidence of 0.998

...Word 'moving' has a confidence of 0.999

...Word 'toward' has a confidence of 0.999

...Word 'virtual' has a confidence of 0.996

...Word 'visits.' has a confidence of 0.996

...Word 'Only' has a confidence of 0.994

...Word '53' has a confidence of 0.996

...Word 'percent' has a confidence of 0.998

...Word 'of' has a confidence of 0.994

...Word 'physicians' has a confidence of 0.955

...Word 'now' has a confidence of 0.998

...Word 'express' has a confidence of 0.997

...Word 'a' has a confidence of 0.994

...Word 'preference' has a confidence of 0.995

...Word 'for' has a confidence of 0.998

...Word 'in-person' has a confidence of 0.996

...Word 'visits' has a confidence of 0.994

...Word 'from' has a confidence of 0.991

...Word 'medtech' has a confidence of 0.993

...Word 'reps' has a confidence of 0.994

...Word 'and' has a confidence of 0.998

...Word 'only' has a confidence of 0.994

...Word '40' has a confidence of 0.998

...Word 'percent' has a confidence of 0.991

...Word 'prefer' has a confidence of 0.999

...Word 'in-person' has a confidence of 0.996

...Word 'visits' has a confidence of 0.999

...Word 'from' has a confidence of 0.994

...Word 'pharma' has a confidence of 0.994

...Word 'reps."' has a confidence of 0.416

...Word 'That' has a confidence of 0.994

...Word 'puts' has a confidence of 0.991

...Word 'the' has a confidence of 0.997

...Word 'onus' has a confidence of 0.993

...Word 'on' has a confidence of 0.997

...Word 'pharmaceutical' has a confidence of 0.994

...Word 'and' has a confidence of 0.999

...Word 'life' has a confidence of 0.99

...Word 'sciences' has a confidence of 0.995

...Word 'organizations' has a confidence of 0.994

...Word 'to' has a confidence of 0.997

...Word 'deliver' has a confidence of 0.997

...Word 'valuable' has a confidence of 0.996

...Word 'and' has a confidence of 0.998

...Word 'engaging' has a confidence of 0.994

...Word 'virtual' has a confidence of 0.997

...Word 'visits' has a confidence of 0.999

...Word 'to' has a confidence of 0.994

...Word 'providers.' has a confidence of 0.986

...Word 'One' has a confidence of 0.998

...Word 'way' has a confidence of 0.998

...Word 'to' has a confidence of 0.998

...Word 'do' has a confidence of 0.997

...Word 'that' has a confidence of 0.994

...Word 'is' has a confidence of 0.997

...Word 'to' has a confidence of 0.997

...Word 'leverage' has a confidence of 0.996

...Word 'text' has a confidence of 0.994

...Word 'analytics' has a confidence of 0.995

...Word 'capabilities' has a confidence of 0.994

...Word 'to' has a confidence of 0.998

...Word 'enhance' has a confidence of 0.996

...Word 'the' has a confidence of 0.997

...Word 'provider' has a confidence of 0.996

...Word 'information' has a confidence of 0.995

...Word 'stored' has a confidence of 0.999

...Word 'in' has a confidence of 0.997

...Word 'the' has a confidence of 0.998

...Word 'organization's' has a confidence of 0.978

...Word 'customer' has a confidence of 0.996

...Word 'relationship' has a confidence of 0.994

...Word 'management' has a confidence of 0.995

...Word '(CRM)' has a confidence of 0.994

...Word 'system.' has a confidence of 0.995

...Word 'For' has a confidence of 0.994

...Word 'example,' has a confidence of 0.993

...Word 'a' has a confidence of 0.991

...Word 'rep' has a confidence of 0.998

...Word 'setting' has a confidence of 0.997

...Word 'up' has a confidence of 0.997

...Word 'a' has a confidence of 0.994

...Word 'visit' has a confidence of 0.998

...Word 'with' has a confidence of 0.994

...Word ''Dr.' has a confidence of 0.923

...Word 'X'' has a confidence of 0.899

...Word 'could' has a confidence of 0.999

...Word 'run' has a confidence of 0.996

...Word 'text' has a confidence of 0.994

...Word 'analytics' has a confidence of 0.994

...Word 'on' has a confidence of 0.997

...Word 'publicly' has a confidence of 0.985

...Word 'available' has a confidence of 0.995

...Word 'resources' has a confidence of 0.996

...Word 'on' has a confidence of 0.998

...Word 'the' has a confidence of 0.998

...Word 'web' has a confidence of 0.996

...Word 'to' has a confidence of 0.998

...Word 'identify' has a confidence of 0.996

...Word 'on' has a confidence of 0.994

...Word 'which' has a confidence of 0.998

...Word 'specific' has a confidence of 0.994

...Word 'topics' has a confidence of 0.993

...Word 'Dr.' has a confidence of 0.996

...Word 'X' has a confidence of 0.935

...Word 'has' has a confidence of 0.997

...Word 'been' has a confidence of 0.993

...Word 'writing' has a confidence of 0.996

...Word 'about' has a confidence of 0.999

...Word 'and' has a confidence of 0.999

...Word 'commenting.' has a confidence of 0.994

...Word '"All' has a confidence of 0.964

...Word 'kinds' has a confidence of 0.997

...Word 'of' has a confidence of 0.994

...Word 'publicly' has a confidence of 0.994

...Word 'available' has a confidence of 0.995

...Word 'information' has a confidence of 0.994

...Word 'can' has a confidence of 0.998

...Word 'All' has a confidence of 0.996

...Word 'kinds' has a confidence of 0.999

...Word 'of' has a confidence of 0.998

...Word 'publicly' has a confidence of 0.994

...Word 'available' has a confidence of 0.996

...Word 'information' has a confidence of 0.995

...Word 'can' has a confidence of 0.998

...Word 'be' has a confidence of 0.997

...Word 'mined' has a confidence of 0.999

...Word 'with' has a confidence of 0.994

...Word 'text' has a confidence of 0.994

...Word 'analytics' has a confidence of 0.996

...Word 'technology,' has a confidence of 0.994

...Word 'which' has a confidence of 0.994

...Word 'can' has a confidence of 0.998

...Word 'be' has a confidence of 0.996

...Word 'used' has a confidence of 0.994

...Word 'to' has a confidence of 0.998

...Word 'arm' has a confidence of 0.997

...Word 'the' has a confidence of 0.998

...Word 'sales' has a confidence of 0.999

...Word 'rep' has a confidence of 0.998

...Word 'with' has a confidence of 0.994

...Word 'relevant' has a confidence of 0.996

...Word 'information' has a confidence of 0.995

...Word 'even' has a confidence of 0.994

...Word 'before' has a confidence of 0.999

...Word 'he' has a confidence of 0.998

...Word 'or' has a confidence of 0.998

...Word 'she' has a confidence of 0.998

...Word 'meets' has a confidence of 0.999

...Word 'the' has a confidence of 0.999

...Word 'doctor.' has a confidence of 0.997

...Word 'It's' has a confidence of 0.982

...Word 'a' has a confidence of 0.994

...Word 'totally' has a confidence of 0.994

...Word 'different,' has a confidence of 0.995

...Word 'digital' has a confidence of 0.998

...Word 'game' has a confidence of 0.994

...Word 'now."' has a confidence of 0.985

...Word 'KEREN' has a confidence of 0.994

...Word 'PRIYADARSHINI' has a confidence of 0.983

...Word '|' has a confidence of 0.986

...Word 'Regional' has a confidence of 0.995

...Word 'Business' has a confidence of 0.994

...Word 'Lead' has a confidence of 0.994

...Word '-' has a confidence of 0.994

...Word 'Asia,' has a confidence of 0.968

...Word 'Worldwide' has a confidence of 0.994

...Word 'Health' has a confidence of 0.999

...Word '|' has a confidence of 0.958

...Word 'Microsoft' has a confidence of 0.995

...Word 'EMBRACING' has a confidence of 0.98

...Word 'DIGITAL' has a confidence of 0.985

...Word 'TRANSFORMATION' has a confidence of 0.994

...Word 'IN' has a confidence of 0.997

...Word 'LIFE' has a confidence of 0.975

...Word 'SCIENCES' has a confidence of 0.994

...Word 'ORGANIZATIONS' has a confidence of 0.994

...Word '2' has a confidence of 0.994

----------------------------------------
