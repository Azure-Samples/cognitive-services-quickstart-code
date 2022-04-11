# Output: REST API v3.0 prebuilt-read model (beta)

| [Form Recognizer REST API](https://westcentralus.dev.cognitive.microsoft.com/docs/services/form-recognizer-api-v3-0-preview-2/operations/AnalyzeDocument) | [Azure SDKS](https://azure.github.io/azure-sdk/releases/latest/index.html) |

The prebuilt-read model extracts printed and handwritten textual elements including lines, words, locations, and detected languages from documents (PDF and TIFF) and images (JPG, PNG, and BMP). The read model is the foundation for all Form Recognizer models. Here is the expected outcome from the prebuilt-read how-to article:

```json
{
    "status": "succeeded",
    "createdDateTime": "2022-04-08T15:35:32Z",
    "lastUpdatedDateTime": "2022-04-08T15:35:35Z",
    "analyzeResult": {
        "apiVersion": "2022-01-30-preview",
        "modelId": "prebuilt-read",
        "stringIndexType": "textElements",
        "content": "While healthcare is still in the early stages of its Al journey, we\nare seeing pharmaceutical and other life sciences organizations\nmaking major investments in Al and related technologies.\"\nTOM LAWRY | National Director for Al, Health and Life Sciences | Microsoft\nAs pharmaceutical and other life sciences organizations invest\nin and deploy advanced technologies, they are beginning to see\nbenefits in diverse areas across their organizations. Companies\nare looking to incorporate automation and continuing smart\nfactory investments to reduce costs in drug discovery, research\nand development, and manufacturing and supply chain\nmanagement. Many life sciences organizations are also choosing\nto stay with more virtual approaches in the \"new normal\" -\nparticularly in clinical trials and sales and marketing areas.\nEnhancing the patient\nand provider experience\nClinical trial sponsors are continually seeking to make clinical\ntrials faster and to improve the experience for patients and\nphysicians. The COVID-19 pandemic has accelerated the\nadoption of decentralized clinical trials, with an increase in\ntrial activities conducted remotely and in participants' homes.\nIn a Mckinsey survey,' up to 98 percent of patients reported\nsatisfaction with telemedicine. In the same report, 72 percent of\nphysicians surveyed reported similar or better experiences with\nremote engagement compared with in-person visits.\nThe shift of trial activities closer to patients has been enabled by\na myriad of evolving technologies and services (e.g ., electronic\nconsent, telehealth and remote patient monitoring). The aim\nto use technology to improve the patient experience and\nconvenience has also broadened trial access to reach a broader,\nmore diverse patient population.\n\"It's an interesting and exciting time right now,\" said Keren\nPriyadarshini, Regional Business Lead - Asia, Worldwide\nHealth, Microsoft. \"It used to be that physicians were key.\nNow, suddenly, patients are feeling empowered by technology.\nPharmaceutical companies and other life sciences companies\nare realizing they have to pay attention to the patient\nexperience in addition to the physician experience.\nEnhanced patient experiences can be delivered in many different\nways. One example of a life sciences product that leverages the\nintelligent cloud to directly affect the patient experience is the\nTandem&reg; Diabetes Care insulin pump. The Tandem&reg; t:slim X2\ninsulin pump with Basal-IQ technology enables patients with\nType 1 diabetes to predict and prevent the low levels of blood\nsugar that cause hypoglycemia.2 The algorithm-driven, software-\nupdatable pump improves the patient experience by automating\nchronic disease management and eliminating the need for\nconstant finger pricks to check glucose levels.\nTandem was able to create and deploy this innovation by\nleveraging the Al and machine learning capabilities of the\nintelligent cloud. As Al and other technologies continue to\nadvance, potential use cases will multiply. \"Speed to value is\ngoing to continue to accelerate,\" said Lawry.\nIn addition to enhancing the patient experience,\npharmaceutical and other life sciences companies can\nleverage advanced technologies to improve relationships with\nproviders. For example, COVID-19 is driving changes in the\nway companies interact with clinicians. Prior to COVID-19,\n75 percent of physicians preferred in-person sales visits from\nmedtech reps; likewise, 77 percent of physicians preferred in-\nperson sales visits from pharma reps.3\nSince the advent of COVID-19, however, physician\npreferences are moving toward virtual visits. Only 53 percent\nof physicians now express a preference for in-person visits\nfrom medtech reps and only 40 percent prefer in-person visits\nfrom pharma reps.\" That puts the onus on pharmaceutical and\nlife sciences organizations to deliver valuable and engaging\nvirtual visits to providers.\nOne way to do that is to leverage text analytics capabilities to\nenhance the provider information stored in the organization's\ncustomer relationship management (CRM) system. For\nexample, a rep setting up a visit with 'Dr. X' could run text\nanalytics on publicly available resources on the web to identify\non which specific topics Dr. X has been writing about and\ncommenting. \"All kinds of publicly available information can\nAll kinds of publicly\navailable information\ncan be mined with text\nanalytics technology,\nwhich can be used to arm the\nsales rep with relevant information even\nbefore he or she meets the doctor. It's a\ntotally different, digital game now.\"\nKEREN PRIYADARSHINI | Regional Business Lead - Asia,\nWorldwide Health | Microsoft\nEMBRACING DIGITAL TRANSFORMATION IN LIFE SCIENCES ORGANIZATIONS\n2",
        "pages": [
            {
                "pageNumber": 1,
                "angle": 0,
                "width": 915,
                "height": 1190,
                "unit": "pixel",
                "words": [
                    {
                        "content": "While",
                        "boundingBox": [
                            260,
                            56,
                            307,
                            56,
                            306,
                            76,
                            260,
                            76
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 0,
                            "length": 5
                        }
                    },
                    {
                        "content": "healthcare",
                        "boundingBox": [
                            311,
                            56,
                            407,
                            56,
                            407,
                            77,
                            311,
                            76
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 6,
                            "length": 10
                        }
                    },
                    {
                        "content": "is",
                        "boundingBox": [
                            412,
                            56,
                            426,
                            56,
                            426,
                            78,
                            411,
                            78
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 17,
                            "length": 2
                        }
                    },
                    {
                        "content": "still",
                        "boundingBox": [
                            431,
                            56,
                            463,
                            56,
                            462,
                            78,
                            430,
                            78
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 20,
                            "length": 5
                        }
                    },
                    {
                        "content": "in",
                        "boundingBox": [
                            467,
                            56,
                            483,
                            56,
                            483,
                            78,
                            467,
                            78
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 26,
                            "length": 2
                        }
                    },
                    {
                        "content": "the",
                        "boundingBox": [
                            488,
                            56,
                            518,
                            56,
                            518,
                            79,
                            487,
                            78
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 29,
                            "length": 3
                        }
                    },
                    {
                        "content": "early",
                        "boundingBox": [
                            522,
                            56,
                            568,
                            56,
                            567,
                            79,
                            522,
                            79
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 33,
                            "length": 5
                        }
                    },
                    {
                        "content": "stages",
                        "boundingBox": [
                            572,
                            56,
                            632,
                            56,
                            631,
                            79,
                            572,
                            79
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 39,
                            "length": 6
                        }
                    },
                    {
                        "content": "of",
                        "boundingBox": [
                            636,
                            56,
                            655,
                            56,
                            655,
                            79,
                            636,
                            79
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 46,
                            "length": 2
                        }
                    },
                    {
                        "content": "its",
                        "boundingBox": [
                            659,
                            56,
                            683,
                            56,
                            682,
                            80,
                            659,
                            79
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 49,
                            "length": 3
                        }
                    },
                    {
                        "content": "Al",
                        "boundingBox": [
                            687,
                            56,
                            705,
                            56,
                            704,
                            80,
                            687,
                            80
                        ],
                        "confidence": 0.879,
                        "span": {
                            "offset": 53,
                            "length": 2
                        }
                    },
                    {
                        "content": "journey,",
                        "boundingBox": [
                            709,
                            56,
                            783,
                            57,
                            783,
                            80,
                            709,
                            80
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 56,
                            "length": 8
                        }
                    },
                    {
                        "content": "we",
                        "boundingBox": [
                            788,
                            57,
                            816,
                            57,
                            816,
                            80,
                            787,
                            80
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 65,
                            "length": 2
                        }
                    },
                    {
                        "content": "are",
                        "boundingBox": [
                            259,
                            84,
                            287,
                            84,
                            288,
                            107,
                            260,
                            107
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 68,
                            "length": 3
                        }
                    },
                    {
                        "content": "seeing",
                        "boundingBox": [
                            292,
                            84,
                            350,
                            84,
                            350,
                            107,
                            292,
                            107
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 72,
                            "length": 6
                        }
                    },
                    {
                        "content": "pharmaceutical",
                        "boundingBox": [
                            354,
                            84,
                            493,
                            84,
                            493,
                            106,
                            355,
                            107
                        ],
                        "confidence": 0.985,
                        "span": {
                            "offset": 79,
                            "length": 14
                        }
                    },
                    {
                        "content": "and",
                        "boundingBox": [
                            497,
                            84,
                            530,
                            84,
                            530,
                            106,
                            497,
                            106
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 94,
                            "length": 3
                        }
                    },
                    {
                        "content": "other",
                        "boundingBox": [
                            534,
                            84,
                            583,
                            84,
                            583,
                            106,
                            534,
                            106
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 98,
                            "length": 5
                        }
                    },
                    {
                        "content": "life",
                        "boundingBox": [
                            588,
                            84,
                            616,
                            84,
                            616,
                            106,
                            588,
                            106
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 104,
                            "length": 4
                        }
                    },
                    {
                        "content": "sciences",
                        "boundingBox": [
                            621,
                            84,
                            695,
                            84,
                            695,
                            106,
                            620,
                            106
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 109,
                            "length": 8
                        }
                    },
                    {
                        "content": "organizations",
                        "boundingBox": [
                            699,
                            84,
                            825,
                            84,
                            825,
                            107,
                            699,
                            106
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 118,
                            "length": 13
                        }
                    },
                    {
                        "content": "making",
                        "boundingBox": [
                            260,
                            112,
                            324,
                            113,
                            325,
                            136,
                            261,
                            137
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 132,
                            "length": 6
                        }
                    },
                    {
                        "content": "major",
                        "boundingBox": [
                            329,
                            113,
                            381,
                            113,
                            382,
                            136,
                            330,
                            136
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 139,
                            "length": 5
                        }
                    },
                    {
                        "content": "investments",
                        "boundingBox": [
                            386,
                            113,
                            497,
                            113,
                            497,
                            135,
                            386,
                            135
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 145,
                            "length": 11
                        }
                    },
                    {
                        "content": "in",
                        "boundingBox": [
                            501,
                            113,
                            519,
                            113,
                            519,
                            135,
                            501,
                            135
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 157,
                            "length": 2
                        }
                    },
                    {
                        "content": "Al",
                        "boundingBox": [
                            523,
                            113,
                            541,
                            113,
                            542,
                            135,
                            524,
                            135
                        ],
                        "confidence": 0.922,
                        "span": {
                            "offset": 160,
                            "length": 2
                        }
                    },
                    {
                        "content": "and",
                        "boundingBox": [
                            546,
                            113,
                            580,
                            113,
                            581,
                            135,
                            546,
                            135
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 163,
                            "length": 3
                        }
                    },
                    {
                        "content": "related",
                        "boundingBox": [
                            585,
                            113,
                            648,
                            113,
                            648,
                            135,
                            585,
                            135
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 167,
                            "length": 7
                        }
                    },
                    {
                        "content": "technologies.\"",
                        "boundingBox": [
                            652,
                            113,
                            785,
                            112,
                            785,
                            137,
                            653,
                            135
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 175,
                            "length": 14
                        }
                    },
                    {
                        "content": "TOM",
                        "boundingBox": [
                            259,
                            151,
                            283,
                            152,
                            284,
                            167,
                            260,
                            167
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 190,
                            "length": 3
                        }
                    },
                    {
                        "content": "LAWRY",
                        "boundingBox": [
                            288,
                            152,
                            331,
                            152,
                            332,
                            167,
                            289,
                            167
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 194,
                            "length": 5
                        }
                    },
                    {
                        "content": "|",
                        "boundingBox": [
                            335,
                            152,
                            341,
                            152,
                            342,
                            168,
                            335,
                            168
                        ],
                        "confidence": 0.972,
                        "span": {
                            "offset": 200,
                            "length": 1
                        }
                    },
                    {
                        "content": "National",
                        "boundingBox": [
                            344,
                            152,
                            384,
                            153,
                            385,
                            168,
                            345,
                            168
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 202,
                            "length": 8
                        }
                    },
                    {
                        "content": "Director",
                        "boundingBox": [
                            387,
                            153,
                            425,
                            153,
                            425,
                            168,
                            388,
                            168
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 211,
                            "length": 8
                        }
                    },
                    {
                        "content": "for",
                        "boundingBox": [
                            428,
                            153,
                            441,
                            153,
                            442,
                            168,
                            428,
                            168
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 220,
                            "length": 3
                        }
                    },
                    {
                        "content": "Al,",
                        "boundingBox": [
                            444,
                            153,
                            457,
                            153,
                            457,
                            168,
                            445,
                            168
                        ],
                        "confidence": 0.815,
                        "span": {
                            "offset": 224,
                            "length": 3
                        }
                    },
                    {
                        "content": "Health",
                        "boundingBox": [
                            460,
                            153,
                            491,
                            153,
                            491,
                            168,
                            460,
                            168
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 228,
                            "length": 6
                        }
                    },
                    {
                        "content": "and",
                        "boundingBox": [
                            494,
                            153,
                            511,
                            153,
                            511,
                            168,
                            494,
                            168
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 235,
                            "length": 3
                        }
                    },
                    {
                        "content": "Life",
                        "boundingBox": [
                            514,
                            153,
                            532,
                            153,
                            532,
                            168,
                            514,
                            168
                        ],
                        "confidence": 0.993,
                        "span": {
                            "offset": 239,
                            "length": 4
                        }
                    },
                    {
                        "content": "Sciences",
                        "boundingBox": [
                            535,
                            153,
                            576,
                            153,
                            576,
                            168,
                            535,
                            168
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 244,
                            "length": 8
                        }
                    },
                    {
                        "content": "|",
                        "boundingBox": [
                            579,
                            153,
                            586,
                            153,
                            586,
                            168,
                            579,
                            168
                        ],
                        "confidence": 0.97,
                        "span": {
                            "offset": 253,
                            "length": 1
                        }
                    },
                    {
                        "content": "Microsoft",
                        "boundingBox": [
                            589,
                            153,
                            638,
                            153,
                            638,
                            167,
                            589,
                            168
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 255,
                            "length": 9
                        }
                    },
                    {
                        "content": "As",
                        "boundingBox": [
                            76,
                            242,
                            90,
                            241,
                            90,
                            258,
                            76,
                            258
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 265,
                            "length": 2
                        }
                    },
                    {
                        "content": "pharmaceutical",
                        "boundingBox": [
                            93,
                            241,
                            179,
                            241,
                            179,
                            258,
                            93,
                            258
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 268,
                            "length": 14
                        }
                    },
                    {
                        "content": "and",
                        "boundingBox": [
                            182,
                            241,
                            203,
                            241,
                            203,
                            258,
                            182,
                            258
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 283,
                            "length": 3
                        }
                    },
                    {
                        "content": "other",
                        "boundingBox": [
                            207,
                            241,
                            235,
                            241,
                            235,
                            258,
                            206,
                            258
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 287,
                            "length": 5
                        }
                    },
                    {
                        "content": "life",
                        "boundingBox": [
                            238,
                            241,
                            256,
                            241,
                            256,
                            258,
                            238,
                            258
                        ],
                        "confidence": 0.993,
                        "span": {
                            "offset": 293,
                            "length": 4
                        }
                    },
                    {
                        "content": "sciences",
                        "boundingBox": [
                            260,
                            241,
                            306,
                            242,
                            306,
                            258,
                            259,
                            258
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 298,
                            "length": 8
                        }
                    },
                    {
                        "content": "organizations",
                        "boundingBox": [
                            309,
                            242,
                            383,
                            243,
                            383,
                            257,
                            309,
                            258
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 307,
                            "length": 13
                        }
                    },
                    {
                        "content": "invest",
                        "boundingBox": [
                            387,
                            243,
                            423,
                            243,
                            422,
                            257,
                            386,
                            257
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 321,
                            "length": 6
                        }
                    },
                    {
                        "content": "in",
                        "boundingBox": [
                            77,
                            261,
                            85,
                            260,
                            86,
                            276,
                            77,
                            276
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 328,
                            "length": 2
                        }
                    },
                    {
                        "content": "and",
                        "boundingBox": [
                            88,
                            260,
                            110,
                            260,
                            111,
                            277,
                            89,
                            276
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 331,
                            "length": 3
                        }
                    },
                    {
                        "content": "deploy",
                        "boundingBox": [
                            113,
                            260,
                            151,
                            260,
                            152,
                            277,
                            114,
                            277
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 335,
                            "length": 6
                        }
                    },
                    {
                        "content": "advanced",
                        "boundingBox": [
                            155,
                            260,
                            209,
                            260,
                            209,
                            277,
                            155,
                            277
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 342,
                            "length": 8
                        }
                    },
                    {
                        "content": "technologies,",
                        "boundingBox": [
                            212,
                            260,
                            287,
                            260,
                            287,
                            278,
                            212,
                            277
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 351,
                            "length": 13
                        }
                    },
                    {
                        "content": "they",
                        "boundingBox": [
                            290,
                            260,
                            315,
                            260,
                            315,
                            278,
                            290,
                            278
                        ],
                        "confidence": 0.99,
                        "span": {
                            "offset": 365,
                            "length": 4
                        }
                    },
                    {
                        "content": "are",
                        "boundingBox": [
                            318,
                            261,
                            335,
                            261,
                            336,
                            278,
                            318,
                            278
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 370,
                            "length": 3
                        }
                    },
                    {
                        "content": "beginning",
                        "boundingBox": [
                            339,
                            261,
                            395,
                            262,
                            395,
                            277,
                            339,
                            278
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 374,
                            "length": 9
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            398,
                            262,
                            410,
                            262,
                            410,
                            277,
                            398,
                            277
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 384,
                            "length": 2
                        }
                    },
                    {
                        "content": "see",
                        "boundingBox": [
                            413,
                            262,
                            435,
                            262,
                            435,
                            277,
                            413,
                            277
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 387,
                            "length": 3
                        }
                    },
                    {
                        "content": "benefits",
                        "boundingBox": [
                            76,
                            279,
                            120,
                            279,
                            120,
                            294,
                            76,
                            293
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 391,
                            "length": 8
                        }
                    },
                    {
                        "content": "in",
                        "boundingBox": [
                            123,
                            279,
                            134,
                            279,
                            134,
                            294,
                            123,
                            294
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 400,
                            "length": 2
                        }
                    },
                    {
                        "content": "diverse",
                        "boundingBox": [
                            138,
                            279,
                            177,
                            279,
                            177,
                            295,
                            137,
                            294
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 403,
                            "length": 7
                        }
                    },
                    {
                        "content": "areas",
                        "boundingBox": [
                            180,
                            279,
                            209,
                            279,
                            209,
                            295,
                            180,
                            295
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 411,
                            "length": 5
                        }
                    },
                    {
                        "content": "across",
                        "boundingBox": [
                            212,
                            279,
                            247,
                            279,
                            247,
                            295,
                            212,
                            295
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 417,
                            "length": 6
                        }
                    },
                    {
                        "content": "their",
                        "boundingBox": [
                            251,
                            279,
                            275,
                            279,
                            275,
                            295,
                            250,
                            295
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 424,
                            "length": 5
                        }
                    },
                    {
                        "content": "organizations.",
                        "boundingBox": [
                            279,
                            279,
                            356,
                            279,
                            356,
                            295,
                            278,
                            295
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 430,
                            "length": 14
                        }
                    },
                    {
                        "content": "Companies",
                        "boundingBox": [
                            359,
                            279,
                            427,
                            279,
                            427,
                            295,
                            359,
                            295
                        ],
                        "confidence": 0.993,
                        "span": {
                            "offset": 445,
                            "length": 9
                        }
                    },
                    {
                        "content": "are",
                        "boundingBox": [
                            78,
                            297,
                            93,
                            297,
                            93,
                            313,
                            78,
                            313
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 455,
                            "length": 3
                        }
                    },
                    {
                        "content": "looking",
                        "boundingBox": [
                            96,
                            297,
                            138,
                            297,
                            138,
                            313,
                            96,
                            313
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 459,
                            "length": 7
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            141,
                            297,
                            152,
                            297,
                            152,
                            313,
                            141,
                            313
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 467,
                            "length": 2
                        }
                    },
                    {
                        "content": "incorporate",
                        "boundingBox": [
                            155,
                            297,
                            221,
                            297,
                            221,
                            313,
                            155,
                            313
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 470,
                            "length": 11
                        }
                    },
                    {
                        "content": "automation",
                        "boundingBox": [
                            224,
                            297,
                            288,
                            298,
                            288,
                            313,
                            224,
                            313
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 482,
                            "length": 10
                        }
                    },
                    {
                        "content": "and",
                        "boundingBox": [
                            291,
                            298,
                            313,
                            298,
                            313,
                            313,
                            291,
                            313
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 493,
                            "length": 3
                        }
                    },
                    {
                        "content": "continuing",
                        "boundingBox": [
                            316,
                            298,
                            375,
                            298,
                            375,
                            313,
                            316,
                            313
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 497,
                            "length": 10
                        }
                    },
                    {
                        "content": "smart",
                        "boundingBox": [
                            378,
                            298,
                            414,
                            298,
                            413,
                            313,
                            378,
                            313
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 508,
                            "length": 5
                        }
                    },
                    {
                        "content": "factory",
                        "boundingBox": [
                            77,
                            316,
                            115,
                            315,
                            115,
                            330,
                            77,
                            330
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 514,
                            "length": 7
                        }
                    },
                    {
                        "content": "investments",
                        "boundingBox": [
                            118,
                            315,
                            185,
                            315,
                            185,
                            331,
                            118,
                            330
                        ],
                        "confidence": 0.979,
                        "span": {
                            "offset": 522,
                            "length": 11
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            188,
                            315,
                            199,
                            315,
                            199,
                            331,
                            188,
                            331
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 534,
                            "length": 2
                        }
                    },
                    {
                        "content": "reduce",
                        "boundingBox": [
                            202,
                            315,
                            242,
                            315,
                            243,
                            331,
                            203,
                            331
                        ],
                        "confidence": 0.992,
                        "span": {
                            "offset": 537,
                            "length": 6
                        }
                    },
                    {
                        "content": "costs",
                        "boundingBox": [
                            246,
                            315,
                            273,
                            315,
                            273,
                            331,
                            246,
                            331
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 544,
                            "length": 5
                        }
                    },
                    {
                        "content": "in",
                        "boundingBox": [
                            276,
                            315,
                            287,
                            315,
                            287,
                            331,
                            277,
                            331
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 550,
                            "length": 2
                        }
                    },
                    {
                        "content": "drug",
                        "boundingBox": [
                            290,
                            315,
                            316,
                            315,
                            317,
                            331,
                            290,
                            331
                        ],
                        "confidence": 0.987,
                        "span": {
                            "offset": 553,
                            "length": 4
                        }
                    },
                    {
                        "content": "discovery,",
                        "boundingBox": [
                            319,
                            315,
                            375,
                            315,
                            375,
                            331,
                            320,
                            331
                        ],
                        "confidence": 0.99,
                        "span": {
                            "offset": 558,
                            "length": 10
                        }
                    },
                    {
                        "content": "research",
                        "boundingBox": [
                            378,
                            315,
                            426,
                            315,
                            426,
                            330,
                            378,
                            331
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 569,
                            "length": 8
                        }
                    },
                    {
                        "content": "and",
                        "boundingBox": [
                            77,
                            333,
                            97,
                            333,
                            97,
                            349,
                            78,
                            349
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 578,
                            "length": 3
                        }
                    },
                    {
                        "content": "development,",
                        "boundingBox": [
                            100,
                            333,
                            177,
                            333,
                            177,
                            350,
                            100,
                            349
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 582,
                            "length": 12
                        }
                    },
                    {
                        "content": "and",
                        "boundingBox": [
                            180,
                            333,
                            202,
                            333,
                            202,
                            350,
                            180,
                            350
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 595,
                            "length": 3
                        }
                    },
                    {
                        "content": "manufacturing",
                        "boundingBox": [
                            205,
                            333,
                            286,
                            332,
                            286,
                            350,
                            205,
                            350
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 599,
                            "length": 13
                        }
                    },
                    {
                        "content": "and",
                        "boundingBox": [
                            290,
                            332,
                            311,
                            332,
                            311,
                            350,
                            290,
                            350
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 613,
                            "length": 3
                        }
                    },
                    {
                        "content": "supply",
                        "boundingBox": [
                            314,
                            332,
                            350,
                            332,
                            350,
                            350,
                            314,
                            350
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 617,
                            "length": 6
                        }
                    },
                    {
                        "content": "chain",
                        "boundingBox": [
                            353,
                            332,
                            386,
                            332,
                            386,
                            350,
                            353,
                            350
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 624,
                            "length": 5
                        }
                    },
                    {
                        "content": "management.",
                        "boundingBox": [
                            77,
                            352,
                            153,
                            351,
                            154,
                            368,
                            77,
                            368
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 630,
                            "length": 11
                        }
                    },
                    {
                        "content": "Many",
                        "boundingBox": [
                            157,
                            351,
                            188,
                            351,
                            188,
                            367,
                            157,
                            368
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 642,
                            "length": 4
                        }
                    },
                    {
                        "content": "life",
                        "boundingBox": [
                            192,
                            351,
                            210,
                            351,
                            210,
                            367,
                            192,
                            367
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 647,
                            "length": 4
                        }
                    },
                    {
                        "content": "sciences",
                        "boundingBox": [
                            213,
                            351,
                            258,
                            351,
                            258,
                            367,
                            213,
                            367
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 652,
                            "length": 8
                        }
                    },
                    {
                        "content": "organizations",
                        "boundingBox": [
                            262,
                            351,
                            338,
                            351,
                            337,
                            367,
                            261,
                            367
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 661,
                            "length": 13
                        }
                    },
                    {
                        "content": "are",
                        "boundingBox": [
                            341,
                            351,
                            358,
                            351,
                            357,
                            367,
                            341,
                            367
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 675,
                            "length": 3
                        }
                    },
                    {
                        "content": "also",
                        "boundingBox": [
                            361,
                            351,
                            382,
                            351,
                            382,
                            367,
                            361,
                            367
                        ],
                        "confidence": 0.993,
                        "span": {
                            "offset": 679,
                            "length": 4
                        }
                    },
                    {
                        "content": "choosing",
                        "boundingBox": [
                            386,
                            351,
                            439,
                            352,
                            438,
                            367,
                            385,
                            367
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 684,
                            "length": 8
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            77,
                            369,
                            88,
                            369,
                            88,
                            385,
                            77,
                            385
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 693,
                            "length": 2
                        }
                    },
                    {
                        "content": "stay",
                        "boundingBox": [
                            91,
                            369,
                            113,
                            369,
                            113,
                            385,
                            91,
                            385
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 696,
                            "length": 4
                        }
                    },
                    {
                        "content": "with",
                        "boundingBox": [
                            116,
                            369,
                            140,
                            369,
                            140,
                            386,
                            116,
                            385
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 701,
                            "length": 4
                        }
                    },
                    {
                        "content": "more",
                        "boundingBox": [
                            143,
                            369,
                            172,
                            369,
                            172,
                            386,
                            143,
                            386
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 706,
                            "length": 4
                        }
                    },
                    {
                        "content": "virtual",
                        "boundingBox": [
                            175,
                            369,
                            209,
                            369,
                            209,
                            386,
                            175,
                            386
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 711,
                            "length": 7
                        }
                    },
                    {
                        "content": "approaches",
                        "boundingBox": [
                            212,
                            369,
                            277,
                            369,
                            277,
                            385,
                            212,
                            386
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 719,
                            "length": 10
                        }
                    },
                    {
                        "content": "in",
                        "boundingBox": [
                            280,
                            369,
                            290,
                            369,
                            291,
                            385,
                            280,
                            385
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 730,
                            "length": 2
                        }
                    },
                    {
                        "content": "the",
                        "boundingBox": [
                            294,
                            369,
                            311,
                            369,
                            312,
                            385,
                            294,
                            385
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 733,
                            "length": 3
                        }
                    },
                    {
                        "content": "\"new",
                        "boundingBox": [
                            315,
                            369,
                            342,
                            369,
                            342,
                            384,
                            315,
                            385
                        ],
                        "confidence": 0.969,
                        "span": {
                            "offset": 737,
                            "length": 4
                        }
                    },
                    {
                        "content": "normal\"",
                        "boundingBox": [
                            345,
                            369,
                            389,
                            369,
                            390,
                            383,
                            345,
                            384
                        ],
                        "confidence": 0.985,
                        "span": {
                            "offset": 742,
                            "length": 7
                        }
                    },
                    {
                        "content": "-",
                        "boundingBox": [
                            393,
                            369,
                            402,
                            369,
                            402,
                            383,
                            393,
                            383
                        ],
                        "confidence": 0.895,
                        "span": {
                            "offset": 750,
                            "length": 1
                        }
                    },
                    {
                        "content": "particularly",
                        "boundingBox": [
                            77,
                            387,
                            137,
                            387,
                            137,
                            403,
                            77,
                            404
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 752,
                            "length": 12
                        }
                    },
                    {
                        "content": "in",
                        "boundingBox": [
                            140,
                            387,
                            151,
                            387,
                            151,
                            403,
                            140,
                            403
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 765,
                            "length": 2
                        }
                    },
                    {
                        "content": "clinical",
                        "boundingBox": [
                            154,
                            387,
                            191,
                            387,
                            191,
                            403,
                            154,
                            403
                        ],
                        "confidence": 0.986,
                        "span": {
                            "offset": 768,
                            "length": 8
                        }
                    },
                    {
                        "content": "trials",
                        "boundingBox": [
                            194,
                            387,
                            221,
                            387,
                            220,
                            403,
                            194,
                            403
                        ],
                        "confidence": 0.986,
                        "span": {
                            "offset": 777,
                            "length": 6
                        }
                    },
                    {
                        "content": "and",
                        "boundingBox": [
                            224,
                            387,
                            245,
                            387,
                            245,
                            403,
                            224,
                            403
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 784,
                            "length": 3
                        }
                    },
                    {
                        "content": "sales",
                        "boundingBox": [
                            248,
                            387,
                            275,
                            387,
                            274,
                            403,
                            248,
                            403
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 788,
                            "length": 5
                        }
                    },
                    {
                        "content": "and",
                        "boundingBox": [
                            278,
                            387,
                            299,
                            387,
                            299,
                            403,
                            277,
                            403
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 794,
                            "length": 3
                        }
                    },
                    {
                        "content": "marketing",
                        "boundingBox": [
                            302,
                            387,
                            358,
                            387,
                            358,
                            403,
                            302,
                            403
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 798,
                            "length": 9
                        }
                    },
                    {
                        "content": "areas.",
                        "boundingBox": [
                            361,
                            387,
                            396,
                            388,
                            395,
                            403,
                            361,
                            403
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 808,
                            "length": 6
                        }
                    },
                    {
                        "content": "Enhancing",
                        "boundingBox": [
                            77,
                            422,
                            166,
                            425,
                            166,
                            447,
                            77,
                            445
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 815,
                            "length": 9
                        }
                    },
                    {
                        "content": "the",
                        "boundingBox": [
                            171,
                            425,
                            198,
                            425,
                            198,
                            447,
                            171,
                            447
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 825,
                            "length": 3
                        }
                    },
                    {
                        "content": "patient",
                        "boundingBox": [
                            203,
                            425,
                            267,
                            424,
                            267,
                            446,
                            203,
                            447
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 829,
                            "length": 7
                        }
                    },
                    {
                        "content": "and",
                        "boundingBox": [
                            78,
                            447,
                            107,
                            448,
                            107,
                            469,
                            78,
                            469
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 837,
                            "length": 3
                        }
                    },
                    {
                        "content": "provider",
                        "boundingBox": [
                            111,
                            448,
                            183,
                            448,
                            182,
                            470,
                            111,
                            469
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 841,
                            "length": 8
                        }
                    },
                    {
                        "content": "experience",
                        "boundingBox": [
                            187,
                            448,
                            283,
                            449,
                            282,
                            470,
                            186,
                            470
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 850,
                            "length": 10
                        }
                    },
                    {
                        "content": "Clinical",
                        "boundingBox": [
                            77,
                            482,
                            118,
                            483,
                            118,
                            499,
                            76,
                            498
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 861,
                            "length": 8
                        }
                    },
                    {
                        "content": "trial",
                        "boundingBox": [
                            121,
                            483,
                            143,
                            483,
                            143,
                            499,
                            121,
                            499
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 870,
                            "length": 5
                        }
                    },
                    {
                        "content": "sponsors",
                        "boundingBox": [
                            146,
                            483,
                            197,
                            483,
                            197,
                            500,
                            146,
                            499
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 876,
                            "length": 8
                        }
                    },
                    {
                        "content": "are",
                        "boundingBox": [
                            200,
                            483,
                            218,
                            483,
                            217,
                            500,
                            200,
                            500
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 885,
                            "length": 3
                        }
                    },
                    {
                        "content": "continually",
                        "boundingBox": [
                            221,
                            483,
                            284,
                            483,
                            283,
                            500,
                            221,
                            500
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 889,
                            "length": 11
                        }
                    },
                    {
                        "content": "seeking",
                        "boundingBox": [
                            287,
                            483,
                            330,
                            483,
                            330,
                            499,
                            287,
                            500
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 901,
                            "length": 7
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            333,
                            483,
                            345,
                            483,
                            345,
                            499,
                            333,
                            499
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 909,
                            "length": 2
                        }
                    },
                    {
                        "content": "make",
                        "boundingBox": [
                            349,
                            483,
                            380,
                            482,
                            380,
                            499,
                            348,
                            499
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 912,
                            "length": 4
                        }
                    },
                    {
                        "content": "clinical",
                        "boundingBox": [
                            383,
                            482,
                            426,
                            482,
                            426,
                            498,
                            383,
                            499
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 917,
                            "length": 8
                        }
                    },
                    {
                        "content": "trials",
                        "boundingBox": [
                            77,
                            501,
                            102,
                            501,
                            103,
                            516,
                            78,
                            516
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 926,
                            "length": 6
                        }
                    },
                    {
                        "content": "faster",
                        "boundingBox": [
                            105,
                            501,
                            138,
                            501,
                            138,
                            517,
                            106,
                            516
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 933,
                            "length": 6
                        }
                    },
                    {
                        "content": "and",
                        "boundingBox": [
                            141,
                            501,
                            162,
                            501,
                            163,
                            517,
                            141,
                            517
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 940,
                            "length": 3
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            166,
                            501,
                            177,
                            501,
                            177,
                            517,
                            166,
                            517
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 944,
                            "length": 2
                        }
                    },
                    {
                        "content": "improve",
                        "boundingBox": [
                            180,
                            501,
                            228,
                            501,
                            228,
                            517,
                            180,
                            517
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 947,
                            "length": 7
                        }
                    },
                    {
                        "content": "the",
                        "boundingBox": [
                            231,
                            501,
                            251,
                            501,
                            251,
                            518,
                            231,
                            518
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 955,
                            "length": 3
                        }
                    },
                    {
                        "content": "experience",
                        "boundingBox": [
                            254,
                            501,
                            317,
                            501,
                            317,
                            517,
                            254,
                            518
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 959,
                            "length": 10
                        }
                    },
                    {
                        "content": "for",
                        "boundingBox": [
                            320,
                            501,
                            337,
                            501,
                            337,
                            517,
                            320,
                            517
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 970,
                            "length": 3
                        }
                    },
                    {
                        "content": "patients",
                        "boundingBox": [
                            340,
                            501,
                            386,
                            501,
                            385,
                            517,
                            340,
                            517
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 974,
                            "length": 8
                        }
                    },
                    {
                        "content": "and",
                        "boundingBox": [
                            389,
                            501,
                            412,
                            501,
                            412,
                            517,
                            388,
                            517
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 983,
                            "length": 3
                        }
                    },
                    {
                        "content": "physicians.",
                        "boundingBox": [
                            77,
                            518,
                            138,
                            518,
                            139,
                            536,
                            78,
                            536
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 987,
                            "length": 11
                        }
                    },
                    {
                        "content": "The",
                        "boundingBox": [
                            142,
                            518,
                            164,
                            518,
                            165,
                            536,
                            142,
                            536
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 999,
                            "length": 3
                        }
                    },
                    {
                        "content": "COVID-19",
                        "boundingBox": [
                            168,
                            518,
                            231,
                            518,
                            231,
                            535,
                            168,
                            536
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 1003,
                            "length": 8
                        }
                    },
                    {
                        "content": "pandemic",
                        "boundingBox": [
                            235,
                            518,
                            292,
                            518,
                            292,
                            535,
                            235,
                            535
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1012,
                            "length": 8
                        }
                    },
                    {
                        "content": "has",
                        "boundingBox": [
                            296,
                            518,
                            315,
                            518,
                            315,
                            535,
                            295,
                            535
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 1021,
                            "length": 3
                        }
                    },
                    {
                        "content": "accelerated",
                        "boundingBox": [
                            318,
                            518,
                            385,
                            519,
                            384,
                            535,
                            318,
                            535
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 1025,
                            "length": 11
                        }
                    },
                    {
                        "content": "the",
                        "boundingBox": [
                            388,
                            519,
                            409,
                            519,
                            409,
                            534,
                            388,
                            535
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 1037,
                            "length": 3
                        }
                    },
                    {
                        "content": "adoption",
                        "boundingBox": [
                            77,
                            537,
                            128,
                            536,
                            128,
                            553,
                            78,
                            554
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 1041,
                            "length": 8
                        }
                    },
                    {
                        "content": "of",
                        "boundingBox": [
                            131,
                            536,
                            143,
                            536,
                            143,
                            553,
                            131,
                            553
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 1050,
                            "length": 2
                        }
                    },
                    {
                        "content": "decentralized",
                        "boundingBox": [
                            146,
                            536,
                            224,
                            536,
                            224,
                            553,
                            146,
                            553
                        ],
                        "confidence": 0.987,
                        "span": {
                            "offset": 1053,
                            "length": 13
                        }
                    },
                    {
                        "content": "clinical",
                        "boundingBox": [
                            228,
                            536,
                            266,
                            536,
                            266,
                            553,
                            227,
                            553
                        ],
                        "confidence": 0.973,
                        "span": {
                            "offset": 1067,
                            "length": 8
                        }
                    },
                    {
                        "content": "trials,",
                        "boundingBox": [
                            269,
                            536,
                            299,
                            536,
                            299,
                            553,
                            269,
                            553
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1076,
                            "length": 7
                        }
                    },
                    {
                        "content": "with",
                        "boundingBox": [
                            303,
                            536,
                            326,
                            537,
                            326,
                            553,
                            302,
                            553
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1084,
                            "length": 4
                        }
                    },
                    {
                        "content": "an",
                        "boundingBox": [
                            329,
                            537,
                            343,
                            537,
                            343,
                            553,
                            329,
                            553
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 1089,
                            "length": 2
                        }
                    },
                    {
                        "content": "increase",
                        "boundingBox": [
                            347,
                            537,
                            393,
                            537,
                            392,
                            553,
                            346,
                            553
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 1092,
                            "length": 8
                        }
                    },
                    {
                        "content": "in",
                        "boundingBox": [
                            396,
                            537,
                            409,
                            538,
                            409,
                            553,
                            396,
                            553
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 1101,
                            "length": 2
                        }
                    },
                    {
                        "content": "trial",
                        "boundingBox": [
                            77,
                            556,
                            98,
                            556,
                            98,
                            571,
                            77,
                            571
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1104,
                            "length": 5
                        }
                    },
                    {
                        "content": "activities",
                        "boundingBox": [
                            101,
                            556,
                            150,
                            556,
                            150,
                            572,
                            101,
                            571
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1110,
                            "length": 10
                        }
                    },
                    {
                        "content": "conducted",
                        "boundingBox": [
                            153,
                            556,
                            215,
                            556,
                            215,
                            572,
                            153,
                            572
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 1121,
                            "length": 9
                        }
                    },
                    {
                        "content": "remotely",
                        "boundingBox": [
                            218,
                            556,
                            270,
                            556,
                            269,
                            573,
                            218,
                            572
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 1131,
                            "length": 8
                        }
                    },
                    {
                        "content": "and",
                        "boundingBox": [
                            273,
                            556,
                            294,
                            556,
                            294,
                            573,
                            273,
                            573
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 1140,
                            "length": 3
                        }
                    },
                    {
                        "content": "in",
                        "boundingBox": [
                            298,
                            556,
                            308,
                            556,
                            308,
                            573,
                            297,
                            573
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 1144,
                            "length": 2
                        }
                    },
                    {
                        "content": "participants'",
                        "boundingBox": [
                            311,
                            556,
                            381,
                            556,
                            380,
                            573,
                            311,
                            573
                        ],
                        "confidence": 0.985,
                        "span": {
                            "offset": 1147,
                            "length": 13
                        }
                    },
                    {
                        "content": "homes.",
                        "boundingBox": [
                            384,
                            556,
                            429,
                            556,
                            429,
                            572,
                            383,
                            572
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1161,
                            "length": 6
                        }
                    },
                    {
                        "content": "In",
                        "boundingBox": [
                            77,
                            573,
                            87,
                            573,
                            87,
                            589,
                            78,
                            589
                        ],
                        "confidence": 0.98,
                        "span": {
                            "offset": 1168,
                            "length": 2
                        }
                    },
                    {
                        "content": "a",
                        "boundingBox": [
                            90,
                            573,
                            97,
                            573,
                            98,
                            590,
                            90,
                            589
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1171,
                            "length": 1
                        }
                    },
                    {
                        "content": "Mckinsey",
                        "boundingBox": [
                            101,
                            573,
                            158,
                            574,
                            158,
                            590,
                            101,
                            590
                        ],
                        "confidence": 0.993,
                        "span": {
                            "offset": 1173,
                            "length": 8
                        }
                    },
                    {
                        "content": "survey,'",
                        "boundingBox": [
                            161,
                            574,
                            201,
                            574,
                            201,
                            590,
                            161,
                            590
                        ],
                        "confidence": 0.12,
                        "span": {
                            "offset": 1182,
                            "length": 8
                        }
                    },
                    {
                        "content": "up",
                        "boundingBox": [
                            205,
                            574,
                            220,
                            574,
                            220,
                            591,
                            205,
                            590
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 1191,
                            "length": 2
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            223,
                            574,
                            236,
                            574,
                            236,
                            591,
                            223,
                            591
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 1194,
                            "length": 2
                        }
                    },
                    {
                        "content": "98",
                        "boundingBox": [
                            239,
                            574,
                            253,
                            574,
                            253,
                            591,
                            239,
                            591
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 1197,
                            "length": 2
                        }
                    },
                    {
                        "content": "percent",
                        "boundingBox": [
                            257,
                            574,
                            300,
                            574,
                            300,
                            591,
                            257,
                            591
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 1200,
                            "length": 7
                        }
                    },
                    {
                        "content": "of",
                        "boundingBox": [
                            303,
                            574,
                            316,
                            574,
                            316,
                            591,
                            303,
                            591
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 1208,
                            "length": 2
                        }
                    },
                    {
                        "content": "patients",
                        "boundingBox": [
                            320,
                            574,
                            365,
                            574,
                            365,
                            590,
                            319,
                            591
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 1211,
                            "length": 8
                        }
                    },
                    {
                        "content": "reported",
                        "boundingBox": [
                            368,
                            574,
                            421,
                            573,
                            421,
                            590,
                            368,
                            590
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 1220,
                            "length": 8
                        }
                    },
                    {
                        "content": "satisfaction",
                        "boundingBox": [
                            77,
                            592,
                            139,
                            592,
                            140,
                            607,
                            78,
                            607
                        ],
                        "confidence": 0.993,
                        "span": {
                            "offset": 1229,
                            "length": 12
                        }
                    },
                    {
                        "content": "with",
                        "boundingBox": [
                            143,
                            592,
                            167,
                            592,
                            167,
                            608,
                            143,
                            608
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1242,
                            "length": 4
                        }
                    },
                    {
                        "content": "telemedicine.",
                        "boundingBox": [
                            170,
                            592,
                            247,
                            592,
                            247,
                            608,
                            170,
                            608
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1247,
                            "length": 13
                        }
                    },
                    {
                        "content": "In",
                        "boundingBox": [
                            250,
                            592,
                            262,
                            592,
                            261,
                            608,
                            250,
                            608
                        ],
                        "confidence": 0.986,
                        "span": {
                            "offset": 1261,
                            "length": 2
                        }
                    },
                    {
                        "content": "the",
                        "boundingBox": [
                            265,
                            592,
                            283,
                            592,
                            283,
                            608,
                            265,
                            608
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 1264,
                            "length": 3
                        }
                    },
                    {
                        "content": "same",
                        "boundingBox": [
                            287,
                            592,
                            317,
                            592,
                            317,
                            608,
                            286,
                            608
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1268,
                            "length": 4
                        }
                    },
                    {
                        "content": "report,",
                        "boundingBox": [
                            320,
                            592,
                            360,
                            592,
                            359,
                            608,
                            320,
                            608
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 1273,
                            "length": 7
                        }
                    },
                    {
                        "content": "72",
                        "boundingBox": [
                            363,
                            592,
                            375,
                            592,
                            375,
                            608,
                            362,
                            608
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 1281,
                            "length": 2
                        }
                    },
                    {
                        "content": "percent",
                        "boundingBox": [
                            378,
                            592,
                            422,
                            592,
                            422,
                            608,
                            378,
                            608
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 1284,
                            "length": 7
                        }
                    },
                    {
                        "content": "of",
                        "boundingBox": [
                            425,
                            592,
                            441,
                            592,
                            441,
                            608,
                            425,
                            608
                        ],
                        "confidence": 0.98,
                        "span": {
                            "offset": 1292,
                            "length": 2
                        }
                    },
                    {
                        "content": "physicians",
                        "boundingBox": [
                            77,
                            611,
                            134,
                            610,
                            134,
                            627,
                            78,
                            627
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1295,
                            "length": 10
                        }
                    },
                    {
                        "content": "surveyed",
                        "boundingBox": [
                            137,
                            610,
                            189,
                            610,
                            189,
                            627,
                            137,
                            627
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 1306,
                            "length": 8
                        }
                    },
                    {
                        "content": "reported",
                        "boundingBox": [
                            192,
                            610,
                            243,
                            610,
                            244,
                            627,
                            193,
                            627
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 1315,
                            "length": 8
                        }
                    },
                    {
                        "content": "similar",
                        "boundingBox": [
                            246,
                            610,
                            282,
                            610,
                            283,
                            626,
                            247,
                            626
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 1324,
                            "length": 7
                        }
                    },
                    {
                        "content": "or",
                        "boundingBox": [
                            286,
                            610,
                            297,
                            610,
                            298,
                            626,
                            286,
                            626
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 1332,
                            "length": 2
                        }
                    },
                    {
                        "content": "better",
                        "boundingBox": [
                            301,
                            610,
                            336,
                            610,
                            336,
                            626,
                            301,
                            626
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 1335,
                            "length": 6
                        }
                    },
                    {
                        "content": "experiences",
                        "boundingBox": [
                            339,
                            610,
                            408,
                            610,
                            408,
                            626,
                            339,
                            626
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 1342,
                            "length": 11
                        }
                    },
                    {
                        "content": "with",
                        "boundingBox": [
                            411,
                            610,
                            437,
                            610,
                            437,
                            626,
                            411,
                            626
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1354,
                            "length": 4
                        }
                    },
                    {
                        "content": "remote",
                        "boundingBox": [
                            77,
                            630,
                            117,
                            630,
                            118,
                            644,
                            77,
                            643
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1359,
                            "length": 6
                        }
                    },
                    {
                        "content": "engagement",
                        "boundingBox": [
                            120,
                            630,
                            194,
                            629,
                            194,
                            644,
                            121,
                            644
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 1366,
                            "length": 10
                        }
                    },
                    {
                        "content": "compared",
                        "boundingBox": [
                            197,
                            629,
                            255,
                            628,
                            255,
                            644,
                            197,
                            644
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 1377,
                            "length": 8
                        }
                    },
                    {
                        "content": "with",
                        "boundingBox": [
                            258,
                            628,
                            283,
                            628,
                            282,
                            644,
                            258,
                            644
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1386,
                            "length": 4
                        }
                    },
                    {
                        "content": "in-person",
                        "boundingBox": [
                            286,
                            628,
                            342,
                            628,
                            341,
                            644,
                            285,
                            644
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 1391,
                            "length": 9
                        }
                    },
                    {
                        "content": "visits.",
                        "boundingBox": [
                            344,
                            628,
                            376,
                            628,
                            375,
                            643,
                            344,
                            644
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1401,
                            "length": 7
                        }
                    },
                    {
                        "content": "The",
                        "boundingBox": [
                            75,
                            657,
                            97,
                            657,
                            97,
                            674,
                            75,
                            674
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1409,
                            "length": 3
                        }
                    },
                    {
                        "content": "shift",
                        "boundingBox": [
                            100,
                            657,
                            124,
                            658,
                            124,
                            675,
                            100,
                            674
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 1413,
                            "length": 5
                        }
                    },
                    {
                        "content": "of",
                        "boundingBox": [
                            127,
                            658,
                            139,
                            658,
                            140,
                            675,
                            128,
                            675
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1419,
                            "length": 2
                        }
                    },
                    {
                        "content": "trial",
                        "boundingBox": [
                            143,
                            658,
                            165,
                            658,
                            165,
                            675,
                            143,
                            675
                        ],
                        "confidence": 0.967,
                        "span": {
                            "offset": 1422,
                            "length": 5
                        }
                    },
                    {
                        "content": "activities",
                        "boundingBox": [
                            168,
                            658,
                            217,
                            658,
                            217,
                            675,
                            168,
                            675
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1428,
                            "length": 10
                        }
                    },
                    {
                        "content": "closer",
                        "boundingBox": [
                            221,
                            658,
                            255,
                            659,
                            255,
                            675,
                            221,
                            675
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1439,
                            "length": 6
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            258,
                            659,
                            270,
                            659,
                            270,
                            675,
                            258,
                            675
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 1446,
                            "length": 2
                        }
                    },
                    {
                        "content": "patients",
                        "boundingBox": [
                            273,
                            659,
                            318,
                            659,
                            318,
                            675,
                            273,
                            675
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1449,
                            "length": 8
                        }
                    },
                    {
                        "content": "has",
                        "boundingBox": [
                            322,
                            659,
                            341,
                            659,
                            341,
                            675,
                            321,
                            675
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 1458,
                            "length": 3
                        }
                    },
                    {
                        "content": "been",
                        "boundingBox": [
                            345,
                            659,
                            374,
                            659,
                            374,
                            675,
                            344,
                            675
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1462,
                            "length": 4
                        }
                    },
                    {
                        "content": "enabled",
                        "boundingBox": [
                            378,
                            659,
                            424,
                            658,
                            423,
                            675,
                            377,
                            675
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 1467,
                            "length": 7
                        }
                    },
                    {
                        "content": "by",
                        "boundingBox": [
                            427,
                            658,
                            443,
                            658,
                            443,
                            675,
                            427,
                            675
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 1475,
                            "length": 2
                        }
                    },
                    {
                        "content": "a",
                        "boundingBox": [
                            77,
                            677,
                            82,
                            677,
                            83,
                            693,
                            77,
                            693
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1478,
                            "length": 1
                        }
                    },
                    {
                        "content": "myriad",
                        "boundingBox": [
                            86,
                            677,
                            125,
                            677,
                            125,
                            693,
                            86,
                            693
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 1480,
                            "length": 6
                        }
                    },
                    {
                        "content": "of",
                        "boundingBox": [
                            128,
                            677,
                            140,
                            677,
                            141,
                            694,
                            129,
                            694
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 1487,
                            "length": 2
                        }
                    },
                    {
                        "content": "evolving",
                        "boundingBox": [
                            144,
                            677,
                            192,
                            677,
                            192,
                            694,
                            144,
                            694
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 1490,
                            "length": 8
                        }
                    },
                    {
                        "content": "technologies",
                        "boundingBox": [
                            195,
                            677,
                            270,
                            677,
                            270,
                            694,
                            195,
                            694
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1499,
                            "length": 12
                        }
                    },
                    {
                        "content": "and",
                        "boundingBox": [
                            273,
                            677,
                            294,
                            677,
                            294,
                            694,
                            273,
                            694
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 1512,
                            "length": 3
                        }
                    },
                    {
                        "content": "services",
                        "boundingBox": [
                            297,
                            677,
                            342,
                            677,
                            342,
                            694,
                            297,
                            694
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 1516,
                            "length": 8
                        }
                    },
                    {
                        "content": "(e.g",
                        "boundingBox": [
                            346,
                            677,
                            366,
                            677,
                            366,
                            694,
                            345,
                            694
                        ],
                        "confidence": 0.955,
                        "span": {
                            "offset": 1525,
                            "length": 4
                        }
                    },
                    {
                        "content": ".,",
                        "boundingBox": [
                            370,
                            677,
                            375,
                            677,
                            375,
                            694,
                            369,
                            694
                        ],
                        "confidence": 0.967,
                        "span": {
                            "offset": 1530,
                            "length": 2
                        }
                    },
                    {
                        "content": "electronic",
                        "boundingBox": [
                            378,
                            677,
                            438,
                            677,
                            437,
                            693,
                            378,
                            694
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 1533,
                            "length": 10
                        }
                    },
                    {
                        "content": "consent,",
                        "boundingBox": [
                            78,
                            696,
                            124,
                            696,
                            124,
                            711,
                            78,
                            710
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1544,
                            "length": 8
                        }
                    },
                    {
                        "content": "telehealth",
                        "boundingBox": [
                            127,
                            696,
                            184,
                            696,
                            184,
                            711,
                            127,
                            711
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1553,
                            "length": 10
                        }
                    },
                    {
                        "content": "and",
                        "boundingBox": [
                            187,
                            696,
                            209,
                            696,
                            209,
                            711,
                            187,
                            711
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 1564,
                            "length": 3
                        }
                    },
                    {
                        "content": "remote",
                        "boundingBox": [
                            212,
                            696,
                            254,
                            695,
                            254,
                            711,
                            212,
                            711
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1568,
                            "length": 6
                        }
                    },
                    {
                        "content": "patient",
                        "boundingBox": [
                            257,
                            695,
                            298,
                            695,
                            298,
                            711,
                            257,
                            712
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 1575,
                            "length": 7
                        }
                    },
                    {
                        "content": "monitoring).",
                        "boundingBox": [
                            301,
                            695,
                            375,
                            695,
                            375,
                            711,
                            301,
                            711
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1583,
                            "length": 12
                        }
                    },
                    {
                        "content": "The",
                        "boundingBox": [
                            378,
                            695,
                            400,
                            695,
                            400,
                            711,
                            378,
                            711
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 1596,
                            "length": 3
                        }
                    },
                    {
                        "content": "aim",
                        "boundingBox": [
                            403,
                            695,
                            422,
                            694,
                            422,
                            711,
                            403,
                            711
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1600,
                            "length": 3
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            77,
                            713,
                            87,
                            713,
                            87,
                            729,
                            77,
                            729
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 1604,
                            "length": 2
                        }
                    },
                    {
                        "content": "use",
                        "boundingBox": [
                            90,
                            713,
                            110,
                            713,
                            110,
                            729,
                            91,
                            729
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 1607,
                            "length": 3
                        }
                    },
                    {
                        "content": "technology",
                        "boundingBox": [
                            113,
                            713,
                            178,
                            713,
                            178,
                            730,
                            113,
                            729
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 1611,
                            "length": 10
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            182,
                            713,
                            194,
                            713,
                            194,
                            730,
                            182,
                            730
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 1622,
                            "length": 2
                        }
                    },
                    {
                        "content": "improve",
                        "boundingBox": [
                            197,
                            713,
                            245,
                            713,
                            245,
                            730,
                            197,
                            730
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 1625,
                            "length": 7
                        }
                    },
                    {
                        "content": "the",
                        "boundingBox": [
                            248,
                            713,
                            267,
                            713,
                            266,
                            730,
                            248,
                            730
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 1633,
                            "length": 3
                        }
                    },
                    {
                        "content": "patient",
                        "boundingBox": [
                            270,
                            713,
                            311,
                            713,
                            311,
                            729,
                            269,
                            730
                        ],
                        "confidence": 0.97,
                        "span": {
                            "offset": 1637,
                            "length": 7
                        }
                    },
                    {
                        "content": "experience",
                        "boundingBox": [
                            314,
                            713,
                            377,
                            713,
                            377,
                            728,
                            314,
                            729
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 1645,
                            "length": 10
                        }
                    },
                    {
                        "content": "and",
                        "boundingBox": [
                            380,
                            713,
                            404,
                            713,
                            403,
                            728,
                            380,
                            728
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 1656,
                            "length": 3
                        }
                    },
                    {
                        "content": "convenience",
                        "boundingBox": [
                            78,
                            731,
                            148,
                            731,
                            148,
                            747,
                            78,
                            747
                        ],
                        "confidence": 0.992,
                        "span": {
                            "offset": 1660,
                            "length": 11
                        }
                    },
                    {
                        "content": "has",
                        "boundingBox": [
                            151,
                            731,
                            170,
                            731,
                            170,
                            747,
                            151,
                            747
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 1672,
                            "length": 3
                        }
                    },
                    {
                        "content": "also",
                        "boundingBox": [
                            173,
                            731,
                            197,
                            730,
                            196,
                            747,
                            173,
                            747
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1676,
                            "length": 4
                        }
                    },
                    {
                        "content": "broadened",
                        "boundingBox": [
                            200,
                            730,
                            262,
                            730,
                            262,
                            747,
                            200,
                            747
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 1681,
                            "length": 9
                        }
                    },
                    {
                        "content": "trial",
                        "boundingBox": [
                            265,
                            730,
                            288,
                            730,
                            287,
                            747,
                            265,
                            747
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 1691,
                            "length": 5
                        }
                    },
                    {
                        "content": "access",
                        "boundingBox": [
                            291,
                            730,
                            328,
                            730,
                            327,
                            747,
                            290,
                            747
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 1697,
                            "length": 6
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            331,
                            730,
                            343,
                            730,
                            342,
                            747,
                            331,
                            747
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 1704,
                            "length": 2
                        }
                    },
                    {
                        "content": "reach",
                        "boundingBox": [
                            346,
                            730,
                            379,
                            730,
                            378,
                            747,
                            345,
                            747
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 1707,
                            "length": 5
                        }
                    },
                    {
                        "content": "a",
                        "boundingBox": [
                            382,
                            730,
                            388,
                            730,
                            388,
                            747,
                            381,
                            747
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1713,
                            "length": 1
                        }
                    },
                    {
                        "content": "broader,",
                        "boundingBox": [
                            391,
                            730,
                            440,
                            731,
                            440,
                            747,
                            391,
                            747
                        ],
                        "confidence": 0.97,
                        "span": {
                            "offset": 1715,
                            "length": 8
                        }
                    },
                    {
                        "content": "more",
                        "boundingBox": [
                            78,
                            748,
                            106,
                            749,
                            106,
                            765,
                            77,
                            765
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1724,
                            "length": 4
                        }
                    },
                    {
                        "content": "diverse",
                        "boundingBox": [
                            109,
                            749,
                            150,
                            749,
                            150,
                            765,
                            109,
                            765
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 1729,
                            "length": 7
                        }
                    },
                    {
                        "content": "patient",
                        "boundingBox": [
                            154,
                            749,
                            194,
                            749,
                            194,
                            765,
                            153,
                            765
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 1737,
                            "length": 7
                        }
                    },
                    {
                        "content": "population.",
                        "boundingBox": [
                            197,
                            749,
                            264,
                            749,
                            264,
                            765,
                            197,
                            765
                        ],
                        "confidence": 0.985,
                        "span": {
                            "offset": 1745,
                            "length": 11
                        }
                    },
                    {
                        "content": "\"It's",
                        "boundingBox": [
                            74,
                            780,
                            92,
                            780,
                            92,
                            797,
                            74,
                            796
                        ],
                        "confidence": 0.94,
                        "span": {
                            "offset": 1757,
                            "length": 5
                        }
                    },
                    {
                        "content": "an",
                        "boundingBox": [
                            95,
                            780,
                            108,
                            781,
                            108,
                            797,
                            95,
                            797
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 1763,
                            "length": 2
                        }
                    },
                    {
                        "content": "interesting",
                        "boundingBox": [
                            111,
                            781,
                            173,
                            781,
                            173,
                            797,
                            111,
                            797
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1766,
                            "length": 11
                        }
                    },
                    {
                        "content": "and",
                        "boundingBox": [
                            176,
                            781,
                            198,
                            781,
                            198,
                            798,
                            176,
                            797
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 1778,
                            "length": 3
                        }
                    },
                    {
                        "content": "exciting",
                        "boundingBox": [
                            201,
                            781,
                            245,
                            781,
                            245,
                            798,
                            201,
                            798
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 1782,
                            "length": 8
                        }
                    },
                    {
                        "content": "time",
                        "boundingBox": [
                            249,
                            781,
                            275,
                            781,
                            275,
                            798,
                            249,
                            798
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1791,
                            "length": 4
                        }
                    },
                    {
                        "content": "right",
                        "boundingBox": [
                            278,
                            781,
                            305,
                            781,
                            305,
                            798,
                            278,
                            798
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 1796,
                            "length": 5
                        }
                    },
                    {
                        "content": "now,\"",
                        "boundingBox": [
                            308,
                            781,
                            340,
                            781,
                            339,
                            797,
                            308,
                            798
                        ],
                        "confidence": 0.907,
                        "span": {
                            "offset": 1802,
                            "length": 5
                        }
                    },
                    {
                        "content": "said",
                        "boundingBox": [
                            343,
                            781,
                            365,
                            780,
                            365,
                            797,
                            343,
                            797
                        ],
                        "confidence": 0.991,
                        "span": {
                            "offset": 1808,
                            "length": 4
                        }
                    },
                    {
                        "content": "Keren",
                        "boundingBox": [
                            369,
                            780,
                            404,
                            780,
                            404,
                            796,
                            369,
                            797
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 1813,
                            "length": 5
                        }
                    },
                    {
                        "content": "Priyadarshini,",
                        "boundingBox": [
                            77,
                            798,
                            152,
                            798,
                            152,
                            815,
                            77,
                            814
                        ],
                        "confidence": 0.982,
                        "span": {
                            "offset": 1819,
                            "length": 14
                        }
                    },
                    {
                        "content": "Regional",
                        "boundingBox": [
                            155,
                            798,
                            206,
                            798,
                            206,
                            815,
                            155,
                            815
                        ],
                        "confidence": 0.973,
                        "span": {
                            "offset": 1834,
                            "length": 8
                        }
                    },
                    {
                        "content": "Business",
                        "boundingBox": [
                            209,
                            798,
                            258,
                            798,
                            258,
                            815,
                            209,
                            815
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 1843,
                            "length": 8
                        }
                    },
                    {
                        "content": "Lead",
                        "boundingBox": [
                            261,
                            798,
                            292,
                            798,
                            292,
                            815,
                            261,
                            815
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1852,
                            "length": 4
                        }
                    },
                    {
                        "content": "-",
                        "boundingBox": [
                            295,
                            798,
                            302,
                            798,
                            302,
                            814,
                            295,
                            815
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1857,
                            "length": 1
                        }
                    },
                    {
                        "content": "Asia,",
                        "boundingBox": [
                            306,
                            798,
                            333,
                            798,
                            333,
                            814,
                            306,
                            814
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 1859,
                            "length": 5
                        }
                    },
                    {
                        "content": "Worldwide",
                        "boundingBox": [
                            336,
                            798,
                            402,
                            798,
                            402,
                            814,
                            336,
                            814
                        ],
                        "confidence": 0.991,
                        "span": {
                            "offset": 1865,
                            "length": 9
                        }
                    },
                    {
                        "content": "Health,",
                        "boundingBox": [
                            77,
                            815,
                            117,
                            815,
                            117,
                            832,
                            77,
                            831
                        ],
                        "confidence": 0.989,
                        "span": {
                            "offset": 1875,
                            "length": 7
                        }
                    },
                    {
                        "content": "Microsoft.",
                        "boundingBox": [
                            120,
                            815,
                            180,
                            816,
                            180,
                            833,
                            121,
                            832
                        ],
                        "confidence": 0.99,
                        "span": {
                            "offset": 1883,
                            "length": 10
                        }
                    },
                    {
                        "content": "\"It",
                        "boundingBox": [
                            183,
                            816,
                            196,
                            816,
                            196,
                            833,
                            183,
                            833
                        ],
                        "confidence": 0.894,
                        "span": {
                            "offset": 1894,
                            "length": 3
                        }
                    },
                    {
                        "content": "used",
                        "boundingBox": [
                            199,
                            816,
                            227,
                            816,
                            227,
                            833,
                            199,
                            833
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1898,
                            "length": 4
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            231,
                            816,
                            243,
                            816,
                            242,
                            833,
                            231,
                            833
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 1903,
                            "length": 2
                        }
                    },
                    {
                        "content": "be",
                        "boundingBox": [
                            246,
                            816,
                            261,
                            816,
                            261,
                            833,
                            246,
                            833
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 1906,
                            "length": 2
                        }
                    },
                    {
                        "content": "that",
                        "boundingBox": [
                            264,
                            816,
                            287,
                            816,
                            287,
                            834,
                            264,
                            833
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1909,
                            "length": 4
                        }
                    },
                    {
                        "content": "physicians",
                        "boundingBox": [
                            290,
                            817,
                            348,
                            817,
                            348,
                            833,
                            290,
                            834
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1914,
                            "length": 10
                        }
                    },
                    {
                        "content": "were",
                        "boundingBox": [
                            352,
                            817,
                            379,
                            818,
                            378,
                            833,
                            351,
                            833
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1925,
                            "length": 4
                        }
                    },
                    {
                        "content": "key.",
                        "boundingBox": [
                            382,
                            818,
                            407,
                            818,
                            407,
                            833,
                            381,
                            833
                        ],
                        "confidence": 0.973,
                        "span": {
                            "offset": 1930,
                            "length": 4
                        }
                    },
                    {
                        "content": "Now,",
                        "boundingBox": [
                            78,
                            834,
                            106,
                            834,
                            106,
                            850,
                            78,
                            850
                        ],
                        "confidence": 0.992,
                        "span": {
                            "offset": 1935,
                            "length": 4
                        }
                    },
                    {
                        "content": "suddenly,",
                        "boundingBox": [
                            109,
                            834,
                            163,
                            834,
                            162,
                            851,
                            109,
                            850
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 1940,
                            "length": 9
                        }
                    },
                    {
                        "content": "patients",
                        "boundingBox": [
                            166,
                            834,
                            212,
                            834,
                            211,
                            851,
                            166,
                            851
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 1950,
                            "length": 8
                        }
                    },
                    {
                        "content": "are",
                        "boundingBox": [
                            215,
                            834,
                            233,
                            834,
                            232,
                            851,
                            215,
                            851
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 1959,
                            "length": 3
                        }
                    },
                    {
                        "content": "feeling",
                        "boundingBox": [
                            236,
                            834,
                            275,
                            835,
                            275,
                            851,
                            235,
                            851
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 1963,
                            "length": 7
                        }
                    },
                    {
                        "content": "empowered",
                        "boundingBox": [
                            279,
                            835,
                            346,
                            835,
                            346,
                            852,
                            278,
                            851
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 1971,
                            "length": 9
                        }
                    },
                    {
                        "content": "by",
                        "boundingBox": [
                            349,
                            835,
                            364,
                            835,
                            363,
                            852,
                            349,
                            852
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 1981,
                            "length": 2
                        }
                    },
                    {
                        "content": "technology.",
                        "boundingBox": [
                            367,
                            835,
                            437,
                            835,
                            437,
                            852,
                            366,
                            852
                        ],
                        "confidence": 0.988,
                        "span": {
                            "offset": 1984,
                            "length": 11
                        }
                    },
                    {
                        "content": "Pharmaceutical",
                        "boundingBox": [
                            78,
                            853,
                            165,
                            852,
                            165,
                            869,
                            77,
                            868
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 1996,
                            "length": 14
                        }
                    },
                    {
                        "content": "companies",
                        "boundingBox": [
                            168,
                            852,
                            230,
                            853,
                            230,
                            869,
                            168,
                            869
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 2011,
                            "length": 9
                        }
                    },
                    {
                        "content": "and",
                        "boundingBox": [
                            233,
                            853,
                            255,
                            853,
                            255,
                            869,
                            233,
                            869
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 2021,
                            "length": 3
                        }
                    },
                    {
                        "content": "other",
                        "boundingBox": [
                            258,
                            853,
                            288,
                            853,
                            288,
                            869,
                            258,
                            869
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 2025,
                            "length": 5
                        }
                    },
                    {
                        "content": "life",
                        "boundingBox": [
                            291,
                            853,
                            309,
                            853,
                            309,
                            869,
                            291,
                            869
                        ],
                        "confidence": 0.992,
                        "span": {
                            "offset": 2031,
                            "length": 4
                        }
                    },
                    {
                        "content": "sciences",
                        "boundingBox": [
                            312,
                            853,
                            361,
                            853,
                            360,
                            869,
                            312,
                            869
                        ],
                        "confidence": 0.993,
                        "span": {
                            "offset": 2036,
                            "length": 8
                        }
                    },
                    {
                        "content": "companies",
                        "boundingBox": [
                            364,
                            853,
                            429,
                            854,
                            428,
                            869,
                            364,
                            869
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 2045,
                            "length": 9
                        }
                    },
                    {
                        "content": "are",
                        "boundingBox": [
                            77,
                            871,
                            93,
                            871,
                            93,
                            887,
                            77,
                            887
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 2055,
                            "length": 3
                        }
                    },
                    {
                        "content": "realizing",
                        "boundingBox": [
                            96,
                            871,
                            145,
                            871,
                            145,
                            888,
                            96,
                            887
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 2059,
                            "length": 9
                        }
                    },
                    {
                        "content": "they",
                        "boundingBox": [
                            149,
                            871,
                            173,
                            871,
                            173,
                            888,
                            149,
                            888
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2069,
                            "length": 4
                        }
                    },
                    {
                        "content": "have",
                        "boundingBox": [
                            177,
                            871,
                            204,
                            871,
                            204,
                            888,
                            176,
                            888
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2074,
                            "length": 4
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            208,
                            871,
                            219,
                            871,
                            219,
                            888,
                            207,
                            888
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 2079,
                            "length": 2
                        }
                    },
                    {
                        "content": "pay",
                        "boundingBox": [
                            223,
                            871,
                            244,
                            871,
                            244,
                            888,
                            222,
                            888
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 2082,
                            "length": 3
                        }
                    },
                    {
                        "content": "attention",
                        "boundingBox": [
                            247,
                            871,
                            299,
                            871,
                            298,
                            887,
                            247,
                            888
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 2086,
                            "length": 9
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            302,
                            871,
                            315,
                            871,
                            314,
                            887,
                            301,
                            887
                        ],
                        "confidence": 0.988,
                        "span": {
                            "offset": 2096,
                            "length": 2
                        }
                    },
                    {
                        "content": "the",
                        "boundingBox": [
                            318,
                            871,
                            336,
                            872,
                            336,
                            887,
                            317,
                            887
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 2099,
                            "length": 3
                        }
                    },
                    {
                        "content": "patient",
                        "boundingBox": [
                            339,
                            872,
                            384,
                            872,
                            383,
                            886,
                            339,
                            887
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2103,
                            "length": 7
                        }
                    },
                    {
                        "content": "experience",
                        "boundingBox": [
                            78,
                            892,
                            139,
                            890,
                            139,
                            905,
                            78,
                            905
                        ],
                        "confidence": 0.985,
                        "span": {
                            "offset": 2111,
                            "length": 10
                        }
                    },
                    {
                        "content": "in",
                        "boundingBox": [
                            142,
                            890,
                            153,
                            890,
                            153,
                            905,
                            142,
                            905
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 2122,
                            "length": 2
                        }
                    },
                    {
                        "content": "addition",
                        "boundingBox": [
                            156,
                            890,
                            203,
                            889,
                            203,
                            905,
                            155,
                            905
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 2125,
                            "length": 8
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            206,
                            889,
                            218,
                            889,
                            218,
                            905,
                            206,
                            905
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 2134,
                            "length": 2
                        }
                    },
                    {
                        "content": "the",
                        "boundingBox": [
                            221,
                            889,
                            241,
                            889,
                            241,
                            905,
                            221,
                            905
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2137,
                            "length": 3
                        }
                    },
                    {
                        "content": "physician",
                        "boundingBox": [
                            244,
                            889,
                            297,
                            889,
                            297,
                            905,
                            244,
                            905
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 2141,
                            "length": 9
                        }
                    },
                    {
                        "content": "experience.",
                        "boundingBox": [
                            300,
                            889,
                            371,
                            890,
                            371,
                            904,
                            300,
                            905
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2151,
                            "length": 11
                        }
                    },
                    {
                        "content": "Enhanced",
                        "boundingBox": [
                            76,
                            920,
                            132,
                            920,
                            132,
                            936,
                            76,
                            936
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2163,
                            "length": 8
                        }
                    },
                    {
                        "content": "patient",
                        "boundingBox": [
                            135,
                            920,
                            174,
                            920,
                            174,
                            936,
                            135,
                            936
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 2172,
                            "length": 7
                        }
                    },
                    {
                        "content": "experiences",
                        "boundingBox": [
                            177,
                            920,
                            244,
                            921,
                            244,
                            936,
                            177,
                            936
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2180,
                            "length": 11
                        }
                    },
                    {
                        "content": "can",
                        "boundingBox": [
                            247,
                            921,
                            268,
                            921,
                            268,
                            936,
                            247,
                            936
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 2192,
                            "length": 3
                        }
                    },
                    {
                        "content": "be",
                        "boundingBox": [
                            271,
                            921,
                            285,
                            921,
                            285,
                            936,
                            271,
                            936
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 2196,
                            "length": 2
                        }
                    },
                    {
                        "content": "delivered",
                        "boundingBox": [
                            288,
                            921,
                            339,
                            920,
                            339,
                            936,
                            288,
                            936
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 2199,
                            "length": 9
                        }
                    },
                    {
                        "content": "in",
                        "boundingBox": [
                            342,
                            920,
                            353,
                            920,
                            353,
                            936,
                            342,
                            936
                        ],
                        "confidence": 0.958,
                        "span": {
                            "offset": 2209,
                            "length": 2
                        }
                    },
                    {
                        "content": "many",
                        "boundingBox": [
                            356,
                            920,
                            387,
                            920,
                            387,
                            936,
                            356,
                            936
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2212,
                            "length": 4
                        }
                    },
                    {
                        "content": "different",
                        "boundingBox": [
                            390,
                            920,
                            443,
                            919,
                            442,
                            936,
                            390,
                            936
                        ],
                        "confidence": 0.967,
                        "span": {
                            "offset": 2217,
                            "length": 9
                        }
                    },
                    {
                        "content": "ways.",
                        "boundingBox": [
                            76,
                            938,
                            105,
                            938,
                            106,
                            955,
                            77,
                            955
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 2227,
                            "length": 5
                        }
                    },
                    {
                        "content": "One",
                        "boundingBox": [
                            109,
                            938,
                            134,
                            938,
                            135,
                            954,
                            109,
                            955
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 2233,
                            "length": 3
                        }
                    },
                    {
                        "content": "example",
                        "boundingBox": [
                            138,
                            938,
                            185,
                            938,
                            185,
                            954,
                            138,
                            954
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 2237,
                            "length": 7
                        }
                    },
                    {
                        "content": "of",
                        "boundingBox": [
                            188,
                            938,
                            201,
                            938,
                            201,
                            954,
                            188,
                            954
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 2245,
                            "length": 2
                        }
                    },
                    {
                        "content": "a",
                        "boundingBox": [
                            204,
                            938,
                            210,
                            938,
                            210,
                            954,
                            204,
                            954
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2248,
                            "length": 1
                        }
                    },
                    {
                        "content": "life",
                        "boundingBox": [
                            213,
                            938,
                            230,
                            938,
                            230,
                            954,
                            213,
                            954
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2250,
                            "length": 4
                        }
                    },
                    {
                        "content": "sciences",
                        "boundingBox": [
                            233,
                            938,
                            279,
                            938,
                            279,
                            954,
                            233,
                            954
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 2255,
                            "length": 8
                        }
                    },
                    {
                        "content": "product",
                        "boundingBox": [
                            283,
                            938,
                            328,
                            938,
                            327,
                            954,
                            282,
                            954
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 2264,
                            "length": 7
                        }
                    },
                    {
                        "content": "that",
                        "boundingBox": [
                            331,
                            938,
                            352,
                            938,
                            352,
                            954,
                            331,
                            954
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2272,
                            "length": 4
                        }
                    },
                    {
                        "content": "leverages",
                        "boundingBox": [
                            356,
                            938,
                            408,
                            938,
                            408,
                            954,
                            355,
                            954
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 2277,
                            "length": 9
                        }
                    },
                    {
                        "content": "the",
                        "boundingBox": [
                            411,
                            938,
                            433,
                            938,
                            433,
                            954,
                            411,
                            954
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 2287,
                            "length": 3
                        }
                    },
                    {
                        "content": "intelligent",
                        "boundingBox": [
                            76,
                            956,
                            131,
                            955,
                            131,
                            972,
                            77,
                            972
                        ],
                        "confidence": 0.99,
                        "span": {
                            "offset": 2291,
                            "length": 11
                        }
                    },
                    {
                        "content": "cloud",
                        "boundingBox": [
                            134,
                            955,
                            166,
                            955,
                            166,
                            972,
                            135,
                            972
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 2303,
                            "length": 5
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            169,
                            955,
                            180,
                            955,
                            180,
                            972,
                            169,
                            972
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 2309,
                            "length": 2
                        }
                    },
                    {
                        "content": "directly",
                        "boundingBox": [
                            183,
                            955,
                            225,
                            955,
                            226,
                            973,
                            183,
                            972
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 2312,
                            "length": 8
                        }
                    },
                    {
                        "content": "affect",
                        "boundingBox": [
                            229,
                            955,
                            260,
                            955,
                            260,
                            972,
                            229,
                            973
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 2321,
                            "length": 6
                        }
                    },
                    {
                        "content": "the",
                        "boundingBox": [
                            264,
                            955,
                            282,
                            955,
                            282,
                            972,
                            264,
                            972
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 2328,
                            "length": 3
                        }
                    },
                    {
                        "content": "patient",
                        "boundingBox": [
                            285,
                            955,
                            324,
                            956,
                            324,
                            972,
                            285,
                            972
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2332,
                            "length": 7
                        }
                    },
                    {
                        "content": "experience",
                        "boundingBox": [
                            327,
                            956,
                            389,
                            956,
                            388,
                            971,
                            327,
                            972
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 2340,
                            "length": 10
                        }
                    },
                    {
                        "content": "is",
                        "boundingBox": [
                            392,
                            956,
                            400,
                            957,
                            400,
                            971,
                            392,
                            971
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 2351,
                            "length": 2
                        }
                    },
                    {
                        "content": "the",
                        "boundingBox": [
                            404,
                            957,
                            423,
                            957,
                            423,
                            971,
                            403,
                            971
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 2354,
                            "length": 3
                        }
                    },
                    {
                        "content": "Tandem&reg;",
                        "boundingBox": [
                            75,
                            973,
                            124,
                            973,
                            124,
                            989,
                            75,
                            988
                        ],
                        "confidence": 0.694,
                        "span": {
                            "offset": 2358,
                            "length": 7
                        }
                    },
                    {
                        "content": "Diabetes",
                        "boundingBox": [
                            127,
                            973,
                            178,
                            973,
                            178,
                            990,
                            127,
                            989
                        ],
                        "confidence": 0.978,
                        "span": {
                            "offset": 2366,
                            "length": 8
                        }
                    },
                    {
                        "content": "Care",
                        "boundingBox": [
                            182,
                            973,
                            209,
                            973,
                            208,
                            990,
                            181,
                            990
                        ],
                        "confidence": 0.993,
                        "span": {
                            "offset": 2375,
                            "length": 4
                        }
                    },
                    {
                        "content": "insulin",
                        "boundingBox": [
                            212,
                            973,
                            248,
                            973,
                            247,
                            990,
                            212,
                            990
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 2380,
                            "length": 7
                        }
                    },
                    {
                        "content": "pump.",
                        "boundingBox": [
                            251,
                            973,
                            289,
                            973,
                            289,
                            990,
                            251,
                            990
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2388,
                            "length": 5
                        }
                    },
                    {
                        "content": "The",
                        "boundingBox": [
                            292,
                            973,
                            315,
                            973,
                            315,
                            990,
                            292,
                            990
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 2394,
                            "length": 3
                        }
                    },
                    {
                        "content": "Tandem&reg;",
                        "boundingBox": [
                            318,
                            973,
                            367,
                            973,
                            367,
                            990,
                            318,
                            990
                        ],
                        "confidence": 0.635,
                        "span": {
                            "offset": 2398,
                            "length": 7
                        }
                    },
                    {
                        "content": "t:slim",
                        "boundingBox": [
                            371,
                            973,
                            399,
                            972,
                            399,
                            989,
                            370,
                            990
                        ],
                        "confidence": 0.97,
                        "span": {
                            "offset": 2406,
                            "length": 6
                        }
                    },
                    {
                        "content": "X2",
                        "boundingBox": [
                            404,
                            972,
                            423,
                            972,
                            423,
                            989,
                            404,
                            989
                        ],
                        "confidence": 0.979,
                        "span": {
                            "offset": 2413,
                            "length": 2
                        }
                    },
                    {
                        "content": "insulin",
                        "boundingBox": [
                            77,
                            993,
                            111,
                            992,
                            111,
                            1008,
                            76,
                            1008
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 2416,
                            "length": 7
                        }
                    },
                    {
                        "content": "pump",
                        "boundingBox": [
                            115,
                            992,
                            148,
                            992,
                            147,
                            1008,
                            114,
                            1008
                        ],
                        "confidence": 0.993,
                        "span": {
                            "offset": 2424,
                            "length": 4
                        }
                    },
                    {
                        "content": "with",
                        "boundingBox": [
                            151,
                            992,
                            174,
                            992,
                            173,
                            1009,
                            151,
                            1008
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2429,
                            "length": 4
                        }
                    },
                    {
                        "content": "Basal-IQ",
                        "boundingBox": [
                            177,
                            992,
                            225,
                            992,
                            225,
                            1009,
                            176,
                            1009
                        ],
                        "confidence": 0.93,
                        "span": {
                            "offset": 2434,
                            "length": 8
                        }
                    },
                    {
                        "content": "technology",
                        "boundingBox": [
                            231,
                            992,
                            294,
                            991,
                            293,
                            1009,
                            230,
                            1009
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2443,
                            "length": 10
                        }
                    },
                    {
                        "content": "enables",
                        "boundingBox": [
                            297,
                            991,
                            340,
                            992,
                            340,
                            1008,
                            296,
                            1009
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 2454,
                            "length": 7
                        }
                    },
                    {
                        "content": "patients",
                        "boundingBox": [
                            343,
                            992,
                            387,
                            992,
                            387,
                            1008,
                            343,
                            1008
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 2462,
                            "length": 8
                        }
                    },
                    {
                        "content": "with",
                        "boundingBox": [
                            391,
                            992,
                            416,
                            992,
                            416,
                            1008,
                            390,
                            1008
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2471,
                            "length": 4
                        }
                    },
                    {
                        "content": "Type",
                        "boundingBox": [
                            76,
                            1009,
                            101,
                            1009,
                            101,
                            1026,
                            76,
                            1026
                        ],
                        "confidence": 0.993,
                        "span": {
                            "offset": 2476,
                            "length": 4
                        }
                    },
                    {
                        "content": "1",
                        "boundingBox": [
                            104,
                            1009,
                            109,
                            1009,
                            109,
                            1026,
                            105,
                            1026
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2481,
                            "length": 1
                        }
                    },
                    {
                        "content": "diabetes",
                        "boundingBox": [
                            112,
                            1009,
                            159,
                            1009,
                            160,
                            1026,
                            112,
                            1026
                        ],
                        "confidence": 0.989,
                        "span": {
                            "offset": 2483,
                            "length": 8
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            163,
                            1009,
                            175,
                            1009,
                            175,
                            1026,
                            163,
                            1026
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 2492,
                            "length": 2
                        }
                    },
                    {
                        "content": "predict",
                        "boundingBox": [
                            178,
                            1010,
                            217,
                            1010,
                            217,
                            1027,
                            178,
                            1026
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 2495,
                            "length": 7
                        }
                    },
                    {
                        "content": "and",
                        "boundingBox": [
                            220,
                            1010,
                            241,
                            1010,
                            241,
                            1026,
                            220,
                            1027
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 2503,
                            "length": 3
                        }
                    },
                    {
                        "content": "prevent",
                        "boundingBox": [
                            244,
                            1010,
                            287,
                            1009,
                            287,
                            1026,
                            244,
                            1026
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 2507,
                            "length": 7
                        }
                    },
                    {
                        "content": "the",
                        "boundingBox": [
                            291,
                            1009,
                            308,
                            1009,
                            308,
                            1026,
                            290,
                            1026
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 2515,
                            "length": 3
                        }
                    },
                    {
                        "content": "low",
                        "boundingBox": [
                            311,
                            1009,
                            330,
                            1009,
                            330,
                            1026,
                            311,
                            1026
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 2519,
                            "length": 3
                        }
                    },
                    {
                        "content": "levels",
                        "boundingBox": [
                            334,
                            1009,
                            365,
                            1009,
                            365,
                            1026,
                            333,
                            1026
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 2523,
                            "length": 6
                        }
                    },
                    {
                        "content": "of",
                        "boundingBox": [
                            369,
                            1009,
                            380,
                            1009,
                            379,
                            1025,
                            368,
                            1026
                        ],
                        "confidence": 0.963,
                        "span": {
                            "offset": 2530,
                            "length": 2
                        }
                    },
                    {
                        "content": "blood",
                        "boundingBox": [
                            383,
                            1009,
                            419,
                            1008,
                            418,
                            1025,
                            383,
                            1025
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 2533,
                            "length": 5
                        }
                    },
                    {
                        "content": "sugar",
                        "boundingBox": [
                            76,
                            1029,
                            106,
                            1029,
                            106,
                            1045,
                            76,
                            1044
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 2539,
                            "length": 5
                        }
                    },
                    {
                        "content": "that",
                        "boundingBox": [
                            109,
                            1029,
                            132,
                            1028,
                            131,
                            1045,
                            109,
                            1045
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2545,
                            "length": 4
                        }
                    },
                    {
                        "content": "cause",
                        "boundingBox": [
                            135,
                            1028,
                            166,
                            1028,
                            166,
                            1045,
                            135,
                            1045
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 2550,
                            "length": 5
                        }
                    },
                    {
                        "content": "hypoglycemia.2",
                        "boundingBox": [
                            169,
                            1027,
                            258,
                            1027,
                            257,
                            1045,
                            169,
                            1045
                        ],
                        "confidence": 0.98,
                        "span": {
                            "offset": 2556,
                            "length": 14
                        }
                    },
                    {
                        "content": "The",
                        "boundingBox": [
                            261,
                            1027,
                            282,
                            1027,
                            282,
                            1045,
                            261,
                            1045
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 2571,
                            "length": 3
                        }
                    },
                    {
                        "content": "algorithm-driven,",
                        "boundingBox": [
                            286,
                            1027,
                            382,
                            1028,
                            381,
                            1044,
                            285,
                            1045
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2575,
                            "length": 17
                        }
                    },
                    {
                        "content": "software-",
                        "boundingBox": [
                            385,
                            1028,
                            440,
                            1029,
                            439,
                            1043,
                            385,
                            1044
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2593,
                            "length": 9
                        }
                    },
                    {
                        "content": "updatable",
                        "boundingBox": [
                            78,
                            1046,
                            132,
                            1046,
                            132,
                            1063,
                            77,
                            1062
                        ],
                        "confidence": 0.992,
                        "span": {
                            "offset": 2603,
                            "length": 9
                        }
                    },
                    {
                        "content": "pump",
                        "boundingBox": [
                            136,
                            1046,
                            169,
                            1046,
                            168,
                            1063,
                            135,
                            1063
                        ],
                        "confidence": 0.993,
                        "span": {
                            "offset": 2613,
                            "length": 4
                        }
                    },
                    {
                        "content": "improves",
                        "boundingBox": [
                            172,
                            1046,
                            224,
                            1046,
                            224,
                            1063,
                            172,
                            1063
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 2618,
                            "length": 8
                        }
                    },
                    {
                        "content": "the",
                        "boundingBox": [
                            227,
                            1046,
                            245,
                            1046,
                            245,
                            1063,
                            227,
                            1063
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 2627,
                            "length": 3
                        }
                    },
                    {
                        "content": "patient",
                        "boundingBox": [
                            248,
                            1046,
                            287,
                            1046,
                            286,
                            1063,
                            248,
                            1063
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 2631,
                            "length": 7
                        }
                    },
                    {
                        "content": "experience",
                        "boundingBox": [
                            290,
                            1046,
                            352,
                            1047,
                            351,
                            1063,
                            289,
                            1063
                        ],
                        "confidence": 0.992,
                        "span": {
                            "offset": 2639,
                            "length": 10
                        }
                    },
                    {
                        "content": "by",
                        "boundingBox": [
                            355,
                            1047,
                            369,
                            1047,
                            368,
                            1063,
                            354,
                            1063
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 2650,
                            "length": 2
                        }
                    },
                    {
                        "content": "automating",
                        "boundingBox": [
                            372,
                            1047,
                            439,
                            1047,
                            439,
                            1062,
                            371,
                            1063
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2653,
                            "length": 10
                        }
                    },
                    {
                        "content": "chronic",
                        "boundingBox": [
                            77,
                            1064,
                            117,
                            1064,
                            117,
                            1079,
                            77,
                            1079
                        ],
                        "confidence": 0.993,
                        "span": {
                            "offset": 2664,
                            "length": 7
                        }
                    },
                    {
                        "content": "disease",
                        "boundingBox": [
                            120,
                            1064,
                            161,
                            1064,
                            161,
                            1080,
                            120,
                            1080
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 2672,
                            "length": 7
                        }
                    },
                    {
                        "content": "management",
                        "boundingBox": [
                            164,
                            1064,
                            238,
                            1064,
                            238,
                            1080,
                            164,
                            1080
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 2680,
                            "length": 10
                        }
                    },
                    {
                        "content": "and",
                        "boundingBox": [
                            241,
                            1064,
                            263,
                            1064,
                            263,
                            1080,
                            241,
                            1080
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 2691,
                            "length": 3
                        }
                    },
                    {
                        "content": "eliminating",
                        "boundingBox": [
                            266,
                            1064,
                            328,
                            1064,
                            328,
                            1080,
                            266,
                            1080
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2695,
                            "length": 11
                        }
                    },
                    {
                        "content": "the",
                        "boundingBox": [
                            331,
                            1064,
                            349,
                            1064,
                            348,
                            1080,
                            331,
                            1080
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 2707,
                            "length": 3
                        }
                    },
                    {
                        "content": "need",
                        "boundingBox": [
                            352,
                            1064,
                            381,
                            1064,
                            381,
                            1080,
                            352,
                            1080
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2711,
                            "length": 4
                        }
                    },
                    {
                        "content": "for",
                        "boundingBox": [
                            384,
                            1064,
                            402,
                            1063,
                            402,
                            1080,
                            384,
                            1080
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 2716,
                            "length": 3
                        }
                    },
                    {
                        "content": "constant",
                        "boundingBox": [
                            78,
                            1083,
                            124,
                            1082,
                            124,
                            1098,
                            78,
                            1098
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2720,
                            "length": 8
                        }
                    },
                    {
                        "content": "finger",
                        "boundingBox": [
                            127,
                            1082,
                            160,
                            1082,
                            159,
                            1099,
                            127,
                            1098
                        ],
                        "confidence": 1,
                        "span": {
                            "offset": 2729,
                            "length": 6
                        }
                    },
                    {
                        "content": "pricks",
                        "boundingBox": [
                            163,
                            1082,
                            195,
                            1081,
                            195,
                            1099,
                            163,
                            1099
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 2736,
                            "length": 6
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            198,
                            1081,
                            210,
                            1081,
                            210,
                            1099,
                            198,
                            1099
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 2743,
                            "length": 2
                        }
                    },
                    {
                        "content": "check",
                        "boundingBox": [
                            214,
                            1081,
                            246,
                            1081,
                            246,
                            1099,
                            213,
                            1099
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 2746,
                            "length": 5
                        }
                    },
                    {
                        "content": "glucose",
                        "boundingBox": [
                            249,
                            1081,
                            292,
                            1081,
                            292,
                            1098,
                            249,
                            1099
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 2752,
                            "length": 7
                        }
                    },
                    {
                        "content": "levels.",
                        "boundingBox": [
                            296,
                            1081,
                            332,
                            1082,
                            332,
                            1097,
                            296,
                            1098
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2760,
                            "length": 7
                        }
                    },
                    {
                        "content": "Tandem",
                        "boundingBox": [
                            468,
                            241,
                            509,
                            241,
                            509,
                            257,
                            468,
                            257
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 2768,
                            "length": 6
                        }
                    },
                    {
                        "content": "was",
                        "boundingBox": [
                            514,
                            241,
                            535,
                            241,
                            535,
                            258,
                            514,
                            257
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 2775,
                            "length": 3
                        }
                    },
                    {
                        "content": "able",
                        "boundingBox": [
                            538,
                            241,
                            563,
                            241,
                            563,
                            258,
                            538,
                            258
                        ],
                        "confidence": 0.991,
                        "span": {
                            "offset": 2779,
                            "length": 4
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            566,
                            241,
                            578,
                            241,
                            578,
                            258,
                            566,
                            258
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 2784,
                            "length": 2
                        }
                    },
                    {
                        "content": "create",
                        "boundingBox": [
                            582,
                            241,
                            618,
                            241,
                            618,
                            259,
                            581,
                            258
                        ],
                        "confidence": 0.975,
                        "span": {
                            "offset": 2787,
                            "length": 6
                        }
                    },
                    {
                        "content": "and",
                        "boundingBox": [
                            621,
                            241,
                            642,
                            241,
                            642,
                            259,
                            621,
                            259
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 2794,
                            "length": 3
                        }
                    },
                    {
                        "content": "deploy",
                        "boundingBox": [
                            646,
                            241,
                            686,
                            241,
                            685,
                            259,
                            645,
                            259
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 2798,
                            "length": 6
                        }
                    },
                    {
                        "content": "this",
                        "boundingBox": [
                            689,
                            241,
                            708,
                            241,
                            707,
                            259,
                            688,
                            259
                        ],
                        "confidence": 0.993,
                        "span": {
                            "offset": 2805,
                            "length": 4
                        }
                    },
                    {
                        "content": "innovation",
                        "boundingBox": [
                            711,
                            241,
                            772,
                            242,
                            771,
                            259,
                            710,
                            259
                        ],
                        "confidence": 0.965,
                        "span": {
                            "offset": 2810,
                            "length": 10
                        }
                    },
                    {
                        "content": "by",
                        "boundingBox": [
                            775,
                            242,
                            792,
                            242,
                            791,
                            259,
                            774,
                            259
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 2821,
                            "length": 2
                        }
                    },
                    {
                        "content": "leveraging",
                        "boundingBox": [
                            470,
                            261,
                            528,
                            261,
                            528,
                            277,
                            470,
                            276
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2824,
                            "length": 10
                        }
                    },
                    {
                        "content": "the",
                        "boundingBox": [
                            532,
                            261,
                            551,
                            261,
                            551,
                            277,
                            532,
                            277
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 2835,
                            "length": 3
                        }
                    },
                    {
                        "content": "Al",
                        "boundingBox": [
                            554,
                            261,
                            568,
                            261,
                            568,
                            277,
                            554,
                            277
                        ],
                        "confidence": 0.949,
                        "span": {
                            "offset": 2839,
                            "length": 2
                        }
                    },
                    {
                        "content": "and",
                        "boundingBox": [
                            571,
                            261,
                            592,
                            261,
                            592,
                            277,
                            571,
                            277
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 2842,
                            "length": 3
                        }
                    },
                    {
                        "content": "machine",
                        "boundingBox": [
                            595,
                            261,
                            644,
                            261,
                            644,
                            277,
                            595,
                            277
                        ],
                        "confidence": 0.993,
                        "span": {
                            "offset": 2846,
                            "length": 7
                        }
                    },
                    {
                        "content": "learning",
                        "boundingBox": [
                            647,
                            261,
                            694,
                            260,
                            694,
                            277,
                            647,
                            277
                        ],
                        "confidence": 0.944,
                        "span": {
                            "offset": 2854,
                            "length": 8
                        }
                    },
                    {
                        "content": "capabilities",
                        "boundingBox": [
                            698,
                            260,
                            761,
                            260,
                            761,
                            277,
                            697,
                            277
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2863,
                            "length": 12
                        }
                    },
                    {
                        "content": "of",
                        "boundingBox": [
                            764,
                            260,
                            776,
                            260,
                            776,
                            277,
                            764,
                            277
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2876,
                            "length": 2
                        }
                    },
                    {
                        "content": "the",
                        "boundingBox": [
                            779,
                            260,
                            801,
                            260,
                            801,
                            276,
                            779,
                            277
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 2879,
                            "length": 3
                        }
                    },
                    {
                        "content": "intelligent",
                        "boundingBox": [
                            469,
                            279,
                            525,
                            278,
                            525,
                            295,
                            469,
                            295
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2883,
                            "length": 11
                        }
                    },
                    {
                        "content": "cloud.",
                        "boundingBox": [
                            528,
                            278,
                            564,
                            278,
                            565,
                            295,
                            529,
                            295
                        ],
                        "confidence": 0.97,
                        "span": {
                            "offset": 2895,
                            "length": 6
                        }
                    },
                    {
                        "content": "As",
                        "boundingBox": [
                            567,
                            278,
                            583,
                            278,
                            584,
                            295,
                            568,
                            295
                        ],
                        "confidence": 0.989,
                        "span": {
                            "offset": 2902,
                            "length": 2
                        }
                    },
                    {
                        "content": "Al",
                        "boundingBox": [
                            587,
                            278,
                            600,
                            278,
                            601,
                            295,
                            587,
                            295
                        ],
                        "confidence": 0.948,
                        "span": {
                            "offset": 2905,
                            "length": 2
                        }
                    },
                    {
                        "content": "and",
                        "boundingBox": [
                            604,
                            278,
                            625,
                            278,
                            625,
                            295,
                            604,
                            295
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 2908,
                            "length": 3
                        }
                    },
                    {
                        "content": "other",
                        "boundingBox": [
                            628,
                            278,
                            659,
                            278,
                            659,
                            295,
                            628,
                            295
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 2912,
                            "length": 5
                        }
                    },
                    {
                        "content": "technologies",
                        "boundingBox": [
                            662,
                            278,
                            735,
                            279,
                            735,
                            295,
                            662,
                            295
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2918,
                            "length": 12
                        }
                    },
                    {
                        "content": "continue",
                        "boundingBox": [
                            739,
                            279,
                            790,
                            280,
                            789,
                            295,
                            738,
                            295
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 2931,
                            "length": 8
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            793,
                            280,
                            807,
                            280,
                            807,
                            295,
                            792,
                            295
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 2940,
                            "length": 2
                        }
                    },
                    {
                        "content": "advance,",
                        "boundingBox": [
                            469,
                            298,
                            519,
                            298,
                            519,
                            313,
                            469,
                            312
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 2943,
                            "length": 8
                        }
                    },
                    {
                        "content": "potential",
                        "boundingBox": [
                            522,
                            298,
                            572,
                            297,
                            572,
                            313,
                            522,
                            313
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 2952,
                            "length": 9
                        }
                    },
                    {
                        "content": "use",
                        "boundingBox": [
                            576,
                            297,
                            596,
                            297,
                            596,
                            314,
                            575,
                            313
                        ],
                        "confidence": 0.946,
                        "span": {
                            "offset": 2962,
                            "length": 3
                        }
                    },
                    {
                        "content": "cases",
                        "boundingBox": [
                            599,
                            297,
                            630,
                            297,
                            630,
                            314,
                            599,
                            314
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 2966,
                            "length": 5
                        }
                    },
                    {
                        "content": "will",
                        "boundingBox": [
                            633,
                            297,
                            651,
                            297,
                            651,
                            314,
                            633,
                            314
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2972,
                            "length": 4
                        }
                    },
                    {
                        "content": "multiply.",
                        "boundingBox": [
                            654,
                            297,
                            702,
                            296,
                            702,
                            314,
                            654,
                            314
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 2977,
                            "length": 9
                        }
                    },
                    {
                        "content": "\"Speed",
                        "boundingBox": [
                            705,
                            296,
                            747,
                            296,
                            747,
                            313,
                            705,
                            314
                        ],
                        "confidence": 0.984,
                        "span": {
                            "offset": 2987,
                            "length": 6
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            750,
                            296,
                            763,
                            296,
                            763,
                            313,
                            750,
                            313
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 2994,
                            "length": 2
                        }
                    },
                    {
                        "content": "value",
                        "boundingBox": [
                            766,
                            296,
                            796,
                            296,
                            796,
                            313,
                            766,
                            313
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 2997,
                            "length": 5
                        }
                    },
                    {
                        "content": "is",
                        "boundingBox": [
                            799,
                            296,
                            811,
                            296,
                            811,
                            312,
                            799,
                            313
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 3003,
                            "length": 2
                        }
                    },
                    {
                        "content": "going",
                        "boundingBox": [
                            469,
                            317,
                            502,
                            317,
                            502,
                            331,
                            469,
                            332
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 3006,
                            "length": 5
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            505,
                            317,
                            516,
                            317,
                            516,
                            331,
                            505,
                            331
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 3012,
                            "length": 2
                        }
                    },
                    {
                        "content": "continue",
                        "boundingBox": [
                            519,
                            317,
                            570,
                            316,
                            570,
                            331,
                            519,
                            331
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 3015,
                            "length": 8
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            573,
                            316,
                            586,
                            316,
                            586,
                            331,
                            573,
                            331
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 3024,
                            "length": 2
                        }
                    },
                    {
                        "content": "accelerate,\"",
                        "boundingBox": [
                            589,
                            316,
                            655,
                            315,
                            655,
                            331,
                            589,
                            331
                        ],
                        "confidence": 0.984,
                        "span": {
                            "offset": 3027,
                            "length": 12
                        }
                    },
                    {
                        "content": "said",
                        "boundingBox": [
                            658,
                            315,
                            682,
                            315,
                            681,
                            331,
                            658,
                            331
                        ],
                        "confidence": 0.992,
                        "span": {
                            "offset": 3040,
                            "length": 4
                        }
                    },
                    {
                        "content": "Lawry.",
                        "boundingBox": [
                            685,
                            315,
                            722,
                            315,
                            722,
                            331,
                            684,
                            331
                        ],
                        "confidence": 0.967,
                        "span": {
                            "offset": 3045,
                            "length": 6
                        }
                    },
                    {
                        "content": "In",
                        "boundingBox": [
                            468,
                            345,
                            479,
                            345,
                            479,
                            361,
                            468,
                            361
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 3052,
                            "length": 2
                        }
                    },
                    {
                        "content": "addition",
                        "boundingBox": [
                            482,
                            345,
                            529,
                            346,
                            529,
                            361,
                            482,
                            361
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 3055,
                            "length": 8
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            532,
                            346,
                            544,
                            346,
                            544,
                            362,
                            532,
                            361
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 3064,
                            "length": 2
                        }
                    },
                    {
                        "content": "enhancing",
                        "boundingBox": [
                            547,
                            346,
                            607,
                            347,
                            608,
                            362,
                            547,
                            362
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 3067,
                            "length": 9
                        }
                    },
                    {
                        "content": "the",
                        "boundingBox": [
                            610,
                            347,
                            629,
                            347,
                            630,
                            362,
                            611,
                            362
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 3077,
                            "length": 3
                        }
                    },
                    {
                        "content": "patient",
                        "boundingBox": [
                            632,
                            347,
                            674,
                            347,
                            674,
                            362,
                            633,
                            362
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 3081,
                            "length": 7
                        }
                    },
                    {
                        "content": "experience,",
                        "boundingBox": [
                            677,
                            347,
                            746,
                            347,
                            746,
                            362,
                            677,
                            362
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 3089,
                            "length": 11
                        }
                    },
                    {
                        "content": "pharmaceutical",
                        "boundingBox": [
                            470,
                            364,
                            557,
                            364,
                            557,
                            380,
                            470,
                            380
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 3101,
                            "length": 14
                        }
                    },
                    {
                        "content": "and",
                        "boundingBox": [
                            560,
                            364,
                            581,
                            364,
                            581,
                            379,
                            560,
                            380
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 3116,
                            "length": 3
                        }
                    },
                    {
                        "content": "other",
                        "boundingBox": [
                            584,
                            364,
                            615,
                            364,
                            615,
                            380,
                            584,
                            379
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 3120,
                            "length": 5
                        }
                    },
                    {
                        "content": "life",
                        "boundingBox": [
                            618,
                            364,
                            636,
                            364,
                            636,
                            380,
                            618,
                            380
                        ],
                        "confidence": 0.993,
                        "span": {
                            "offset": 3126,
                            "length": 4
                        }
                    },
                    {
                        "content": "sciences",
                        "boundingBox": [
                            639,
                            364,
                            686,
                            365,
                            686,
                            380,
                            638,
                            380
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 3131,
                            "length": 8
                        }
                    },
                    {
                        "content": "companies",
                        "boundingBox": [
                            689,
                            365,
                            752,
                            365,
                            752,
                            380,
                            689,
                            380
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 3140,
                            "length": 9
                        }
                    },
                    {
                        "content": "can",
                        "boundingBox": [
                            755,
                            365,
                            778,
                            366,
                            777,
                            380,
                            755,
                            380
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 3150,
                            "length": 3
                        }
                    },
                    {
                        "content": "leverage",
                        "boundingBox": [
                            467,
                            382,
                            517,
                            382,
                            518,
                            398,
                            468,
                            398
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 3154,
                            "length": 8
                        }
                    },
                    {
                        "content": "advanced",
                        "boundingBox": [
                            520,
                            382,
                            575,
                            382,
                            576,
                            398,
                            521,
                            398
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 3163,
                            "length": 8
                        }
                    },
                    {
                        "content": "technologies",
                        "boundingBox": [
                            578,
                            382,
                            652,
                            382,
                            652,
                            398,
                            579,
                            398
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 3172,
                            "length": 12
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            655,
                            382,
                            667,
                            382,
                            667,
                            399,
                            656,
                            398
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 3185,
                            "length": 2
                        }
                    },
                    {
                        "content": "improve",
                        "boundingBox": [
                            670,
                            382,
                            718,
                            382,
                            718,
                            398,
                            670,
                            399
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 3188,
                            "length": 7
                        }
                    },
                    {
                        "content": "relationships",
                        "boundingBox": [
                            721,
                            382,
                            793,
                            382,
                            793,
                            398,
                            721,
                            398
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 3196,
                            "length": 13
                        }
                    },
                    {
                        "content": "with",
                        "boundingBox": [
                            796,
                            382,
                            822,
                            382,
                            822,
                            398,
                            796,
                            398
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 3210,
                            "length": 4
                        }
                    },
                    {
                        "content": "providers.",
                        "boundingBox": [
                            468,
                            400,
                            524,
                            400,
                            524,
                            417,
                            468,
                            416
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 3215,
                            "length": 10
                        }
                    },
                    {
                        "content": "For",
                        "boundingBox": [
                            528,
                            400,
                            547,
                            399,
                            547,
                            417,
                            528,
                            417
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 3226,
                            "length": 3
                        }
                    },
                    {
                        "content": "example,",
                        "boundingBox": [
                            551,
                            399,
                            602,
                            399,
                            602,
                            417,
                            551,
                            417
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 3230,
                            "length": 8
                        }
                    },
                    {
                        "content": "COVID-19",
                        "boundingBox": [
                            606,
                            399,
                            670,
                            399,
                            669,
                            417,
                            606,
                            417
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 3239,
                            "length": 8
                        }
                    },
                    {
                        "content": "is",
                        "boundingBox": [
                            673,
                            399,
                            682,
                            399,
                            682,
                            417,
                            673,
                            417
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 3248,
                            "length": 2
                        }
                    },
                    {
                        "content": "driving",
                        "boundingBox": [
                            685,
                            399,
                            725,
                            399,
                            724,
                            416,
                            685,
                            417
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 3251,
                            "length": 7
                        }
                    },
                    {
                        "content": "changes",
                        "boundingBox": [
                            728,
                            399,
                            774,
                            400,
                            774,
                            416,
                            728,
                            416
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 3259,
                            "length": 7
                        }
                    },
                    {
                        "content": "in",
                        "boundingBox": [
                            777,
                            400,
                            788,
                            400,
                            788,
                            416,
                            777,
                            416
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 3267,
                            "length": 2
                        }
                    },
                    {
                        "content": "the",
                        "boundingBox": [
                            792,
                            400,
                            814,
                            401,
                            814,
                            416,
                            792,
                            416
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 3270,
                            "length": 3
                        }
                    },
                    {
                        "content": "way",
                        "boundingBox": [
                            469,
                            420,
                            489,
                            420,
                            490,
                            435,
                            470,
                            435
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 3274,
                            "length": 3
                        }
                    },
                    {
                        "content": "companies",
                        "boundingBox": [
                            493,
                            420,
                            555,
                            419,
                            555,
                            435,
                            493,
                            435
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 3278,
                            "length": 9
                        }
                    },
                    {
                        "content": "interact",
                        "boundingBox": [
                            558,
                            419,
                            602,
                            418,
                            602,
                            434,
                            558,
                            435
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 3288,
                            "length": 8
                        }
                    },
                    {
                        "content": "with",
                        "boundingBox": [
                            605,
                            418,
                            628,
                            418,
                            629,
                            434,
                            605,
                            434
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 3297,
                            "length": 4
                        }
                    },
                    {
                        "content": "clinicians.",
                        "boundingBox": [
                            632,
                            418,
                            686,
                            417,
                            686,
                            434,
                            632,
                            434
                        ],
                        "confidence": 0.93,
                        "span": {
                            "offset": 3302,
                            "length": 11
                        }
                    },
                    {
                        "content": "Prior",
                        "boundingBox": [
                            689,
                            417,
                            717,
                            417,
                            717,
                            434,
                            689,
                            434
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 3314,
                            "length": 5
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            720,
                            417,
                            733,
                            417,
                            733,
                            434,
                            720,
                            434
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 3320,
                            "length": 2
                        }
                    },
                    {
                        "content": "COVID-19,",
                        "boundingBox": [
                            736,
                            417,
                            806,
                            417,
                            806,
                            434,
                            736,
                            434
                        ],
                        "confidence": 0.968,
                        "span": {
                            "offset": 3323,
                            "length": 9
                        }
                    },
                    {
                        "content": "75",
                        "boundingBox": [
                            468,
                            437,
                            480,
                            437,
                            481,
                            452,
                            469,
                            452
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 3333,
                            "length": 2
                        }
                    },
                    {
                        "content": "percent",
                        "boundingBox": [
                            483,
                            436,
                            527,
                            436,
                            528,
                            453,
                            484,
                            452
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 3336,
                            "length": 7
                        }
                    },
                    {
                        "content": "of",
                        "boundingBox": [
                            530,
                            436,
                            542,
                            436,
                            543,
                            453,
                            531,
                            453
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 3344,
                            "length": 2
                        }
                    },
                    {
                        "content": "physicians",
                        "boundingBox": [
                            546,
                            436,
                            604,
                            435,
                            604,
                            453,
                            546,
                            453
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 3347,
                            "length": 10
                        }
                    },
                    {
                        "content": "preferred",
                        "boundingBox": [
                            607,
                            435,
                            661,
                            435,
                            661,
                            453,
                            608,
                            453
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 3358,
                            "length": 9
                        }
                    },
                    {
                        "content": "in-person",
                        "boundingBox": [
                            664,
                            435,
                            719,
                            435,
                            719,
                            453,
                            664,
                            453
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 3368,
                            "length": 9
                        }
                    },
                    {
                        "content": "sales",
                        "boundingBox": [
                            722,
                            435,
                            750,
                            436,
                            750,
                            452,
                            722,
                            453
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 3378,
                            "length": 5
                        }
                    },
                    {
                        "content": "visits",
                        "boundingBox": [
                            753,
                            436,
                            780,
                            436,
                            780,
                            452,
                            753,
                            452
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 3384,
                            "length": 6
                        }
                    },
                    {
                        "content": "from",
                        "boundingBox": [
                            784,
                            436,
                            809,
                            436,
                            809,
                            452,
                            784,
                            452
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 3391,
                            "length": 4
                        }
                    },
                    {
                        "content": "medtech",
                        "boundingBox": [
                            469,
                            455,
                            518,
                            454,
                            519,
                            470,
                            469,
                            469
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 3396,
                            "length": 7
                        }
                    },
                    {
                        "content": "reps;",
                        "boundingBox": [
                            522,
                            454,
                            549,
                            454,
                            549,
                            470,
                            522,
                            470
                        ],
                        "confidence": 0.979,
                        "span": {
                            "offset": 3404,
                            "length": 5
                        }
                    },
                    {
                        "content": "likewise,",
                        "boundingBox": [
                            553,
                            454,
                            600,
                            454,
                            600,
                            471,
                            553,
                            470
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 3410,
                            "length": 9
                        }
                    },
                    {
                        "content": "77",
                        "boundingBox": [
                            603,
                            454,
                            616,
                            453,
                            616,
                            471,
                            603,
                            471
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 3420,
                            "length": 2
                        }
                    },
                    {
                        "content": "percent",
                        "boundingBox": [
                            619,
                            453,
                            663,
                            453,
                            663,
                            471,
                            619,
                            471
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 3423,
                            "length": 7
                        }
                    },
                    {
                        "content": "of",
                        "boundingBox": [
                            666,
                            453,
                            679,
                            453,
                            678,
                            471,
                            666,
                            471
                        ],
                        "confidence": 0.991,
                        "span": {
                            "offset": 3431,
                            "length": 2
                        }
                    },
                    {
                        "content": "physicians",
                        "boundingBox": [
                            682,
                            453,
                            740,
                            453,
                            740,
                            471,
                            682,
                            471
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 3434,
                            "length": 10
                        }
                    },
                    {
                        "content": "preferred",
                        "boundingBox": [
                            744,
                            453,
                            798,
                            453,
                            797,
                            471,
                            743,
                            471
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 3445,
                            "length": 9
                        }
                    },
                    {
                        "content": "in-",
                        "boundingBox": [
                            801,
                            453,
                            818,
                            453,
                            818,
                            470,
                            801,
                            471
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 3455,
                            "length": 3
                        }
                    },
                    {
                        "content": "person",
                        "boundingBox": [
                            469,
                            474,
                            508,
                            473,
                            508,
                            488,
                            469,
                            488
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 3459,
                            "length": 6
                        }
                    },
                    {
                        "content": "sales",
                        "boundingBox": [
                            511,
                            473,
                            538,
                            473,
                            538,
                            488,
                            511,
                            488
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 3466,
                            "length": 5
                        }
                    },
                    {
                        "content": "visits",
                        "boundingBox": [
                            541,
                            473,
                            570,
                            472,
                            569,
                            488,
                            541,
                            488
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 3472,
                            "length": 6
                        }
                    },
                    {
                        "content": "from",
                        "boundingBox": [
                            573,
                            472,
                            597,
                            472,
                            597,
                            488,
                            572,
                            488
                        ],
                        "confidence": 0.992,
                        "span": {
                            "offset": 3479,
                            "length": 4
                        }
                    },
                    {
                        "content": "pharma",
                        "boundingBox": [
                            603,
                            472,
                            647,
                            472,
                            647,
                            488,
                            602,
                            488
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 3484,
                            "length": 6
                        }
                    },
                    {
                        "content": "reps.3",
                        "boundingBox": [
                            650,
                            472,
                            685,
                            472,
                            685,
                            487,
                            650,
                            488
                        ],
                        "confidence": 0.726,
                        "span": {
                            "offset": 3491,
                            "length": 6
                        }
                    },
                    {
                        "content": "Since",
                        "boundingBox": [
                            469,
                            504,
                            498,
                            503,
                            499,
                            519,
                            469,
                            519
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 3498,
                            "length": 5
                        }
                    },
                    {
                        "content": "the",
                        "boundingBox": [
                            502,
                            503,
                            521,
                            503,
                            522,
                            519,
                            502,
                            519
                        ],
                        "confidence": 0.987,
                        "span": {
                            "offset": 3504,
                            "length": 3
                        }
                    },
                    {
                        "content": "advent",
                        "boundingBox": [
                            524,
                            503,
                            566,
                            503,
                            566,
                            520,
                            525,
                            519
                        ],
                        "confidence": 0.98,
                        "span": {
                            "offset": 3508,
                            "length": 6
                        }
                    },
                    {
                        "content": "of",
                        "boundingBox": [
                            569,
                            503,
                            582,
                            503,
                            582,
                            520,
                            569,
                            520
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 3515,
                            "length": 2
                        }
                    },
                    {
                        "content": "COVID-19,",
                        "boundingBox": [
                            585,
                            503,
                            653,
                            503,
                            653,
                            520,
                            585,
                            520
                        ],
                        "confidence": 0.993,
                        "span": {
                            "offset": 3518,
                            "length": 9
                        }
                    },
                    {
                        "content": "however,",
                        "boundingBox": [
                            656,
                            503,
                            709,
                            504,
                            709,
                            520,
                            657,
                            520
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 3528,
                            "length": 8
                        }
                    },
                    {
                        "content": "physician",
                        "boundingBox": [
                            713,
                            504,
                            768,
                            505,
                            768,
                            520,
                            713,
                            520
                        ],
                        "confidence": 0.993,
                        "span": {
                            "offset": 3537,
                            "length": 9
                        }
                    },
                    {
                        "content": "preferences",
                        "boundingBox": [
                            469,
                            524,
                            537,
                            523,
                            537,
                            538,
                            469,
                            537
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 3547,
                            "length": 11
                        }
                    },
                    {
                        "content": "are",
                        "boundingBox": [
                            540,
                            523,
                            559,
                            523,
                            559,
                            538,
                            540,
                            538
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 3559,
                            "length": 3
                        }
                    },
                    {
                        "content": "moving",
                        "boundingBox": [
                            562,
                            522,
                            607,
                            522,
                            607,
                            538,
                            562,
                            538
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 3563,
                            "length": 6
                        }
                    },
                    {
                        "content": "toward",
                        "boundingBox": [
                            610,
                            522,
                            651,
                            522,
                            651,
                            538,
                            610,
                            538
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 3570,
                            "length": 6
                        }
                    },
                    {
                        "content": "virtual",
                        "boundingBox": [
                            654,
                            522,
                            691,
                            522,
                            691,
                            538,
                            655,
                            538
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 3577,
                            "length": 7
                        }
                    },
                    {
                        "content": "visits.",
                        "boundingBox": [
                            694,
                            522,
                            727,
                            522,
                            727,
                            538,
                            694,
                            538
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 3585,
                            "length": 7
                        }
                    },
                    {
                        "content": "Only",
                        "boundingBox": [
                            730,
                            522,
                            760,
                            522,
                            760,
                            538,
                            730,
                            538
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 3593,
                            "length": 4
                        }
                    },
                    {
                        "content": "53",
                        "boundingBox": [
                            763,
                            522,
                            776,
                            522,
                            776,
                            538,
                            763,
                            538
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 3598,
                            "length": 2
                        }
                    },
                    {
                        "content": "percent",
                        "boundingBox": [
                            779,
                            522,
                            828,
                            522,
                            828,
                            538,
                            779,
                            538
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 3601,
                            "length": 7
                        }
                    },
                    {
                        "content": "of",
                        "boundingBox": [
                            469,
                            540,
                            480,
                            540,
                            480,
                            556,
                            469,
                            556
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 3609,
                            "length": 2
                        }
                    },
                    {
                        "content": "physicians",
                        "boundingBox": [
                            483,
                            540,
                            543,
                            540,
                            543,
                            556,
                            483,
                            556
                        ],
                        "confidence": 0.955,
                        "span": {
                            "offset": 3612,
                            "length": 10
                        }
                    },
                    {
                        "content": "now",
                        "boundingBox": [
                            547,
                            540,
                            571,
                            540,
                            571,
                            557,
                            546,
                            556
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 3623,
                            "length": 3
                        }
                    },
                    {
                        "content": "express",
                        "boundingBox": [
                            574,
                            540,
                            619,
                            540,
                            618,
                            557,
                            574,
                            557
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 3627,
                            "length": 7
                        }
                    },
                    {
                        "content": "a",
                        "boundingBox": [
                            622,
                            540,
                            629,
                            540,
                            629,
                            557,
                            622,
                            557
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 3635,
                            "length": 1
                        }
                    },
                    {
                        "content": "preference",
                        "boundingBox": [
                            633,
                            540,
                            696,
                            540,
                            696,
                            557,
                            632,
                            557
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 3637,
                            "length": 10
                        }
                    },
                    {
                        "content": "for",
                        "boundingBox": [
                            699,
                            540,
                            715,
                            540,
                            715,
                            557,
                            699,
                            557
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 3648,
                            "length": 3
                        }
                    },
                    {
                        "content": "in-person",
                        "boundingBox": [
                            719,
                            540,
                            776,
                            540,
                            775,
                            556,
                            718,
                            557
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 3652,
                            "length": 9
                        }
                    },
                    {
                        "content": "visits",
                        "boundingBox": [
                            779,
                            540,
                            812,
                            540,
                            811,
                            556,
                            779,
                            556
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 3662,
                            "length": 6
                        }
                    },
                    {
                        "content": "from",
                        "boundingBox": [
                            470,
                            557,
                            494,
                            557,
                            493,
                            573,
                            469,
                            573
                        ],
                        "confidence": 0.991,
                        "span": {
                            "offset": 3669,
                            "length": 4
                        }
                    },
                    {
                        "content": "medtech",
                        "boundingBox": [
                            500,
                            557,
                            551,
                            557,
                            550,
                            574,
                            499,
                            573
                        ],
                        "confidence": 0.993,
                        "span": {
                            "offset": 3674,
                            "length": 7
                        }
                    },
                    {
                        "content": "reps",
                        "boundingBox": [
                            554,
                            557,
                            580,
                            557,
                            580,
                            575,
                            553,
                            574
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 3682,
                            "length": 4
                        }
                    },
                    {
                        "content": "and",
                        "boundingBox": [
                            584,
                            557,
                            606,
                            557,
                            605,
                            575,
                            583,
                            575
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 3687,
                            "length": 3
                        }
                    },
                    {
                        "content": "only",
                        "boundingBox": [
                            609,
                            557,
                            635,
                            557,
                            634,
                            575,
                            608,
                            575
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 3691,
                            "length": 4
                        }
                    },
                    {
                        "content": "40",
                        "boundingBox": [
                            638,
                            557,
                            652,
                            557,
                            652,
                            575,
                            638,
                            575
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 3696,
                            "length": 2
                        }
                    },
                    {
                        "content": "percent",
                        "boundingBox": [
                            656,
                            557,
                            702,
                            558,
                            701,
                            575,
                            655,
                            575
                        ],
                        "confidence": 0.991,
                        "span": {
                            "offset": 3699,
                            "length": 7
                        }
                    },
                    {
                        "content": "prefer",
                        "boundingBox": [
                            705,
                            558,
                            740,
                            558,
                            739,
                            575,
                            704,
                            575
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 3707,
                            "length": 6
                        }
                    },
                    {
                        "content": "in-person",
                        "boundingBox": [
                            743,
                            558,
                            800,
                            558,
                            799,
                            574,
                            742,
                            575
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 3714,
                            "length": 9
                        }
                    },
                    {
                        "content": "visits",
                        "boundingBox": [
                            803,
                            558,
                            834,
                            559,
                            833,
                            573,
                            802,
                            574
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 3724,
                            "length": 6
                        }
                    },
                    {
                        "content": "from",
                        "boundingBox": [
                            470,
                            576,
                            493,
                            576,
                            493,
                            591,
                            470,
                            591
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 3731,
                            "length": 4
                        }
                    },
                    {
                        "content": "pharma",
                        "boundingBox": [
                            499,
                            576,
                            544,
                            576,
                            544,
                            592,
                            499,
                            591
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 3736,
                            "length": 6
                        }
                    },
                    {
                        "content": "reps.\"",
                        "boundingBox": [
                            547,
                            576,
                            582,
                            576,
                            582,
                            592,
                            548,
                            592
                        ],
                        "confidence": 0.416,
                        "span": {
                            "offset": 3743,
                            "length": 6
                        }
                    },
                    {
                        "content": "That",
                        "boundingBox": [
                            585,
                            576,
                            612,
                            576,
                            612,
                            592,
                            586,
                            592
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 3750,
                            "length": 4
                        }
                    },
                    {
                        "content": "puts",
                        "boundingBox": [
                            615,
                            576,
                            641,
                            576,
                            641,
                            593,
                            615,
                            593
                        ],
                        "confidence": 0.991,
                        "span": {
                            "offset": 3755,
                            "length": 4
                        }
                    },
                    {
                        "content": "the",
                        "boundingBox": [
                            644,
                            576,
                            663,
                            576,
                            663,
                            593,
                            644,
                            593
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 3760,
                            "length": 3
                        }
                    },
                    {
                        "content": "onus",
                        "boundingBox": [
                            666,
                            576,
                            695,
                            576,
                            694,
                            593,
                            666,
                            593
                        ],
                        "confidence": 0.993,
                        "span": {
                            "offset": 3764,
                            "length": 4
                        }
                    },
                    {
                        "content": "on",
                        "boundingBox": [
                            698,
                            576,
                            714,
                            576,
                            713,
                            593,
                            698,
                            593
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 3769,
                            "length": 2
                        }
                    },
                    {
                        "content": "pharmaceutical",
                        "boundingBox": [
                            717,
                            576,
                            807,
                            576,
                            806,
                            592,
                            717,
                            593
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 3772,
                            "length": 14
                        }
                    },
                    {
                        "content": "and",
                        "boundingBox": [
                            810,
                            576,
                            834,
                            576,
                            833,
                            592,
                            810,
                            592
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 3787,
                            "length": 3
                        }
                    },
                    {
                        "content": "life",
                        "boundingBox": [
                            469,
                            594,
                            486,
                            594,
                            486,
                            609,
                            469,
                            609
                        ],
                        "confidence": 0.99,
                        "span": {
                            "offset": 3791,
                            "length": 4
                        }
                    },
                    {
                        "content": "sciences",
                        "boundingBox": [
                            489,
                            594,
                            539,
                            594,
                            539,
                            610,
                            489,
                            609
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 3796,
                            "length": 8
                        }
                    },
                    {
                        "content": "organizations",
                        "boundingBox": [
                            542,
                            594,
                            621,
                            594,
                            620,
                            610,
                            542,
                            610
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 3805,
                            "length": 13
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            624,
                            594,
                            637,
                            594,
                            637,
                            611,
                            624,
                            610
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 3819,
                            "length": 2
                        }
                    },
                    {
                        "content": "deliver",
                        "boundingBox": [
                            640,
                            594,
                            680,
                            594,
                            680,
                            611,
                            640,
                            611
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 3822,
                            "length": 7
                        }
                    },
                    {
                        "content": "valuable",
                        "boundingBox": [
                            683,
                            594,
                            732,
                            594,
                            731,
                            611,
                            683,
                            611
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 3830,
                            "length": 8
                        }
                    },
                    {
                        "content": "and",
                        "boundingBox": [
                            735,
                            594,
                            758,
                            594,
                            757,
                            611,
                            735,
                            611
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 3839,
                            "length": 3
                        }
                    },
                    {
                        "content": "engaging",
                        "boundingBox": [
                            761,
                            594,
                            819,
                            594,
                            818,
                            611,
                            760,
                            611
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 3843,
                            "length": 8
                        }
                    },
                    {
                        "content": "virtual",
                        "boundingBox": [
                            469,
                            612,
                            504,
                            612,
                            504,
                            627,
                            469,
                            627
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 3852,
                            "length": 7
                        }
                    },
                    {
                        "content": "visits",
                        "boundingBox": [
                            508,
                            612,
                            536,
                            612,
                            536,
                            627,
                            507,
                            627
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 3860,
                            "length": 6
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            539,
                            612,
                            552,
                            612,
                            552,
                            628,
                            539,
                            627
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 3867,
                            "length": 2
                        }
                    },
                    {
                        "content": "providers.",
                        "boundingBox": [
                            555,
                            612,
                            615,
                            612,
                            615,
                            628,
                            555,
                            628
                        ],
                        "confidence": 0.986,
                        "span": {
                            "offset": 3870,
                            "length": 10
                        }
                    },
                    {
                        "content": "One",
                        "boundingBox": [
                            469,
                            642,
                            494,
                            643,
                            494,
                            659,
                            469,
                            658
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 3881,
                            "length": 3
                        }
                    },
                    {
                        "content": "way",
                        "boundingBox": [
                            497,
                            643,
                            520,
                            643,
                            520,
                            659,
                            497,
                            659
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 3885,
                            "length": 3
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            523,
                            643,
                            535,
                            643,
                            535,
                            659,
                            523,
                            659
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 3889,
                            "length": 2
                        }
                    },
                    {
                        "content": "do",
                        "boundingBox": [
                            538,
                            643,
                            554,
                            643,
                            554,
                            659,
                            538,
                            659
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 3892,
                            "length": 2
                        }
                    },
                    {
                        "content": "that",
                        "boundingBox": [
                            557,
                            643,
                            579,
                            643,
                            579,
                            659,
                            557,
                            659
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 3895,
                            "length": 4
                        }
                    },
                    {
                        "content": "is",
                        "boundingBox": [
                            583,
                            643,
                            591,
                            643,
                            591,
                            659,
                            583,
                            659
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 3900,
                            "length": 2
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            594,
                            643,
                            606,
                            643,
                            606,
                            659,
                            594,
                            659
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 3903,
                            "length": 2
                        }
                    },
                    {
                        "content": "leverage",
                        "boundingBox": [
                            609,
                            643,
                            659,
                            643,
                            658,
                            660,
                            609,
                            659
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 3906,
                            "length": 8
                        }
                    },
                    {
                        "content": "text",
                        "boundingBox": [
                            662,
                            643,
                            685,
                            643,
                            685,
                            660,
                            662,
                            660
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 3915,
                            "length": 4
                        }
                    },
                    {
                        "content": "analytics",
                        "boundingBox": [
                            689,
                            643,
                            738,
                            643,
                            737,
                            659,
                            688,
                            660
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 3920,
                            "length": 9
                        }
                    },
                    {
                        "content": "capabilities",
                        "boundingBox": [
                            741,
                            643,
                            804,
                            643,
                            804,
                            659,
                            741,
                            659
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 3930,
                            "length": 12
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            807,
                            643,
                            822,
                            643,
                            821,
                            659,
                            807,
                            659
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 3943,
                            "length": 2
                        }
                    },
                    {
                        "content": "enhance",
                        "boundingBox": [
                            469,
                            662,
                            516,
                            661,
                            517,
                            677,
                            470,
                            677
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 3946,
                            "length": 7
                        }
                    },
                    {
                        "content": "the",
                        "boundingBox": [
                            519,
                            661,
                            538,
                            661,
                            538,
                            677,
                            520,
                            677
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 3954,
                            "length": 3
                        }
                    },
                    {
                        "content": "provider",
                        "boundingBox": [
                            541,
                            661,
                            590,
                            661,
                            590,
                            677,
                            541,
                            677
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 3958,
                            "length": 8
                        }
                    },
                    {
                        "content": "information",
                        "boundingBox": [
                            593,
                            661,
                            661,
                            661,
                            661,
                            677,
                            593,
                            677
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 3967,
                            "length": 11
                        }
                    },
                    {
                        "content": "stored",
                        "boundingBox": [
                            664,
                            661,
                            700,
                            661,
                            700,
                            677,
                            664,
                            677
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 3979,
                            "length": 6
                        }
                    },
                    {
                        "content": "in",
                        "boundingBox": [
                            703,
                            661,
                            713,
                            661,
                            713,
                            677,
                            703,
                            677
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 3986,
                            "length": 2
                        }
                    },
                    {
                        "content": "the",
                        "boundingBox": [
                            716,
                            661,
                            735,
                            661,
                            735,
                            677,
                            716,
                            677
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 3989,
                            "length": 3
                        }
                    },
                    {
                        "content": "organization's",
                        "boundingBox": [
                            738,
                            661,
                            820,
                            662,
                            820,
                            677,
                            738,
                            677
                        ],
                        "confidence": 0.978,
                        "span": {
                            "offset": 3993,
                            "length": 14
                        }
                    },
                    {
                        "content": "customer",
                        "boundingBox": [
                            469,
                            680,
                            521,
                            680,
                            521,
                            695,
                            470,
                            694
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 4008,
                            "length": 8
                        }
                    },
                    {
                        "content": "relationship",
                        "boundingBox": [
                            524,
                            680,
                            591,
                            679,
                            591,
                            696,
                            525,
                            695
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4017,
                            "length": 12
                        }
                    },
                    {
                        "content": "management",
                        "boundingBox": [
                            594,
                            679,
                            671,
                            679,
                            671,
                            696,
                            594,
                            696
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 4030,
                            "length": 10
                        }
                    },
                    {
                        "content": "(CRM)",
                        "boundingBox": [
                            674,
                            679,
                            717,
                            679,
                            717,
                            696,
                            674,
                            696
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4041,
                            "length": 5
                        }
                    },
                    {
                        "content": "system.",
                        "boundingBox": [
                            720,
                            679,
                            763,
                            678,
                            763,
                            695,
                            720,
                            696
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 4047,
                            "length": 7
                        }
                    },
                    {
                        "content": "For",
                        "boundingBox": [
                            766,
                            678,
                            789,
                            678,
                            789,
                            695,
                            766,
                            695
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4055,
                            "length": 3
                        }
                    },
                    {
                        "content": "example,",
                        "boundingBox": [
                            470,
                            698,
                            520,
                            698,
                            520,
                            714,
                            470,
                            713
                        ],
                        "confidence": 0.993,
                        "span": {
                            "offset": 4059,
                            "length": 8
                        }
                    },
                    {
                        "content": "a",
                        "boundingBox": [
                            523,
                            698,
                            528,
                            697,
                            528,
                            714,
                            523,
                            714
                        ],
                        "confidence": 0.991,
                        "span": {
                            "offset": 4068,
                            "length": 1
                        }
                    },
                    {
                        "content": "rep",
                        "boundingBox": [
                            532,
                            697,
                            551,
                            697,
                            551,
                            714,
                            532,
                            714
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 4070,
                            "length": 3
                        }
                    },
                    {
                        "content": "setting",
                        "boundingBox": [
                            554,
                            697,
                            594,
                            697,
                            594,
                            714,
                            555,
                            714
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 4074,
                            "length": 7
                        }
                    },
                    {
                        "content": "up",
                        "boundingBox": [
                            597,
                            697,
                            612,
                            696,
                            612,
                            714,
                            597,
                            714
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 4082,
                            "length": 2
                        }
                    },
                    {
                        "content": "a",
                        "boundingBox": [
                            615,
                            696,
                            622,
                            696,
                            622,
                            714,
                            615,
                            714
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4085,
                            "length": 1
                        }
                    },
                    {
                        "content": "visit",
                        "boundingBox": [
                            625,
                            696,
                            648,
                            696,
                            648,
                            714,
                            625,
                            714
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 4087,
                            "length": 5
                        }
                    },
                    {
                        "content": "with",
                        "boundingBox": [
                            651,
                            696,
                            675,
                            696,
                            675,
                            714,
                            651,
                            714
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4093,
                            "length": 4
                        }
                    },
                    {
                        "content": "'Dr.",
                        "boundingBox": [
                            678,
                            696,
                            700,
                            696,
                            700,
                            713,
                            678,
                            714
                        ],
                        "confidence": 0.923,
                        "span": {
                            "offset": 4098,
                            "length": 4
                        }
                    },
                    {
                        "content": "X'",
                        "boundingBox": [
                            703,
                            696,
                            714,
                            696,
                            714,
                            713,
                            703,
                            713
                        ],
                        "confidence": 0.899,
                        "span": {
                            "offset": 4103,
                            "length": 2
                        }
                    },
                    {
                        "content": "could",
                        "boundingBox": [
                            718,
                            696,
                            750,
                            696,
                            750,
                            713,
                            718,
                            713
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 4106,
                            "length": 5
                        }
                    },
                    {
                        "content": "run",
                        "boundingBox": [
                            753,
                            696,
                            772,
                            696,
                            772,
                            712,
                            753,
                            713
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 4112,
                            "length": 3
                        }
                    },
                    {
                        "content": "text",
                        "boundingBox": [
                            775,
                            696,
                            802,
                            696,
                            802,
                            712,
                            775,
                            712
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4116,
                            "length": 4
                        }
                    },
                    {
                        "content": "analytics",
                        "boundingBox": [
                            469,
                            715,
                            516,
                            715,
                            517,
                            732,
                            470,
                            732
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4121,
                            "length": 9
                        }
                    },
                    {
                        "content": "on",
                        "boundingBox": [
                            520,
                            715,
                            535,
                            715,
                            535,
                            732,
                            520,
                            732
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 4131,
                            "length": 2
                        }
                    },
                    {
                        "content": "publicly",
                        "boundingBox": [
                            538,
                            715,
                            583,
                            715,
                            583,
                            732,
                            538,
                            732
                        ],
                        "confidence": 0.985,
                        "span": {
                            "offset": 4134,
                            "length": 8
                        }
                    },
                    {
                        "content": "available",
                        "boundingBox": [
                            586,
                            715,
                            635,
                            715,
                            635,
                            731,
                            586,
                            732
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 4143,
                            "length": 9
                        }
                    },
                    {
                        "content": "resources",
                        "boundingBox": [
                            639,
                            715,
                            693,
                            715,
                            693,
                            731,
                            639,
                            731
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 4153,
                            "length": 9
                        }
                    },
                    {
                        "content": "on",
                        "boundingBox": [
                            697,
                            715,
                            712,
                            715,
                            712,
                            731,
                            697,
                            731
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 4163,
                            "length": 2
                        }
                    },
                    {
                        "content": "the",
                        "boundingBox": [
                            715,
                            715,
                            734,
                            715,
                            734,
                            731,
                            715,
                            731
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 4166,
                            "length": 3
                        }
                    },
                    {
                        "content": "web",
                        "boundingBox": [
                            737,
                            715,
                            761,
                            715,
                            761,
                            731,
                            737,
                            731
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 4170,
                            "length": 3
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            764,
                            715,
                            775,
                            715,
                            775,
                            731,
                            764,
                            731
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 4174,
                            "length": 2
                        }
                    },
                    {
                        "content": "identify",
                        "boundingBox": [
                            778,
                            715,
                            825,
                            715,
                            825,
                            730,
                            778,
                            731
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 4177,
                            "length": 8
                        }
                    },
                    {
                        "content": "on",
                        "boundingBox": [
                            469,
                            733,
                            483,
                            733,
                            483,
                            749,
                            469,
                            749
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4186,
                            "length": 2
                        }
                    },
                    {
                        "content": "which",
                        "boundingBox": [
                            486,
                            733,
                            518,
                            733,
                            518,
                            750,
                            486,
                            749
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 4189,
                            "length": 5
                        }
                    },
                    {
                        "content": "specific",
                        "boundingBox": [
                            522,
                            733,
                            565,
                            732,
                            565,
                            750,
                            522,
                            750
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4195,
                            "length": 8
                        }
                    },
                    {
                        "content": "topics",
                        "boundingBox": [
                            568,
                            732,
                            604,
                            732,
                            604,
                            750,
                            568,
                            750
                        ],
                        "confidence": 0.993,
                        "span": {
                            "offset": 4204,
                            "length": 6
                        }
                    },
                    {
                        "content": "Dr.",
                        "boundingBox": [
                            607,
                            732,
                            626,
                            732,
                            626,
                            750,
                            607,
                            750
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 4211,
                            "length": 3
                        }
                    },
                    {
                        "content": "X",
                        "boundingBox": [
                            630,
                            732,
                            637,
                            732,
                            637,
                            750,
                            629,
                            750
                        ],
                        "confidence": 0.935,
                        "span": {
                            "offset": 4215,
                            "length": 1
                        }
                    },
                    {
                        "content": "has",
                        "boundingBox": [
                            641,
                            732,
                            661,
                            732,
                            660,
                            750,
                            640,
                            750
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 4217,
                            "length": 3
                        }
                    },
                    {
                        "content": "been",
                        "boundingBox": [
                            664,
                            732,
                            693,
                            732,
                            693,
                            750,
                            664,
                            750
                        ],
                        "confidence": 0.993,
                        "span": {
                            "offset": 4221,
                            "length": 4
                        }
                    },
                    {
                        "content": "writing",
                        "boundingBox": [
                            696,
                            732,
                            735,
                            732,
                            735,
                            749,
                            696,
                            750
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 4226,
                            "length": 7
                        }
                    },
                    {
                        "content": "about",
                        "boundingBox": [
                            738,
                            732,
                            772,
                            733,
                            771,
                            749,
                            738,
                            749
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 4234,
                            "length": 5
                        }
                    },
                    {
                        "content": "and",
                        "boundingBox": [
                            775,
                            733,
                            799,
                            733,
                            799,
                            749,
                            775,
                            749
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 4240,
                            "length": 3
                        }
                    },
                    {
                        "content": "commenting.",
                        "boundingBox": [
                            469,
                            752,
                            545,
                            751,
                            545,
                            768,
                            469,
                            766
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4244,
                            "length": 11
                        }
                    },
                    {
                        "content": "\"All",
                        "boundingBox": [
                            548,
                            751,
                            566,
                            750,
                            566,
                            768,
                            548,
                            768
                        ],
                        "confidence": 0.964,
                        "span": {
                            "offset": 4256,
                            "length": 4
                        }
                    },
                    {
                        "content": "kinds",
                        "boundingBox": [
                            569,
                            750,
                            599,
                            750,
                            600,
                            768,
                            569,
                            768
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 4261,
                            "length": 5
                        }
                    },
                    {
                        "content": "of",
                        "boundingBox": [
                            603,
                            750,
                            614,
                            750,
                            615,
                            768,
                            603,
                            768
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4267,
                            "length": 2
                        }
                    },
                    {
                        "content": "publicly",
                        "boundingBox": [
                            618,
                            750,
                            663,
                            750,
                            663,
                            768,
                            618,
                            768
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4270,
                            "length": 8
                        }
                    },
                    {
                        "content": "available",
                        "boundingBox": [
                            666,
                            750,
                            716,
                            750,
                            715,
                            768,
                            666,
                            768
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 4279,
                            "length": 9
                        }
                    },
                    {
                        "content": "information",
                        "boundingBox": [
                            719,
                            750,
                            787,
                            752,
                            786,
                            766,
                            719,
                            768
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4289,
                            "length": 11
                        }
                    },
                    {
                        "content": "can",
                        "boundingBox": [
                            790,
                            752,
                            812,
                            752,
                            811,
                            766,
                            789,
                            766
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 4301,
                            "length": 3
                        }
                    },
                    {
                        "content": "All",
                        "boundingBox": [
                            468,
                            828,
                            490,
                            828,
                            490,
                            850,
                            468,
                            850
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 4305,
                            "length": 3
                        }
                    },
                    {
                        "content": "kinds",
                        "boundingBox": [
                            494,
                            828,
                            544,
                            828,
                            544,
                            851,
                            494,
                            850
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 4309,
                            "length": 5
                        }
                    },
                    {
                        "content": "of",
                        "boundingBox": [
                            548,
                            828,
                            567,
                            829,
                            567,
                            851,
                            548,
                            851
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 4315,
                            "length": 2
                        }
                    },
                    {
                        "content": "publicly",
                        "boundingBox": [
                            571,
                            829,
                            643,
                            829,
                            643,
                            851,
                            571,
                            851
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4318,
                            "length": 8
                        }
                    },
                    {
                        "content": "available",
                        "boundingBox": [
                            470,
                            857,
                            548,
                            857,
                            548,
                            878,
                            470,
                            878
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 4327,
                            "length": 9
                        }
                    },
                    {
                        "content": "information",
                        "boundingBox": [
                            552,
                            857,
                            658,
                            857,
                            658,
                            878,
                            552,
                            878
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 4337,
                            "length": 11
                        }
                    },
                    {
                        "content": "can",
                        "boundingBox": [
                            470,
                            886,
                            499,
                            886,
                            499,
                            907,
                            470,
                            907
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 4349,
                            "length": 3
                        }
                    },
                    {
                        "content": "be",
                        "boundingBox": [
                            503,
                            886,
                            526,
                            886,
                            527,
                            907,
                            504,
                            907
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 4353,
                            "length": 2
                        }
                    },
                    {
                        "content": "mined",
                        "boundingBox": [
                            531,
                            886,
                            585,
                            886,
                            586,
                            907,
                            531,
                            907
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 4356,
                            "length": 5
                        }
                    },
                    {
                        "content": "with",
                        "boundingBox": [
                            590,
                            886,
                            629,
                            886,
                            630,
                            907,
                            590,
                            907
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4362,
                            "length": 4
                        }
                    },
                    {
                        "content": "text",
                        "boundingBox": [
                            633,
                            886,
                            673,
                            886,
                            673,
                            906,
                            634,
                            907
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4367,
                            "length": 4
                        }
                    },
                    {
                        "content": "analytics",
                        "boundingBox": [
                            470,
                            916,
                            549,
                            916,
                            549,
                            938,
                            470,
                            937
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 4372,
                            "length": 9
                        }
                    },
                    {
                        "content": "technology,",
                        "boundingBox": [
                            554,
                            916,
                            660,
                            916,
                            660,
                            939,
                            554,
                            938
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4382,
                            "length": 11
                        }
                    },
                    {
                        "content": "which",
                        "boundingBox": [
                            469,
                            943,
                            519,
                            943,
                            519,
                            964,
                            469,
                            964
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4394,
                            "length": 5
                        }
                    },
                    {
                        "content": "can",
                        "boundingBox": [
                            523,
                            943,
                            555,
                            943,
                            555,
                            964,
                            523,
                            964
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 4400,
                            "length": 3
                        }
                    },
                    {
                        "content": "be",
                        "boundingBox": [
                            560,
                            943,
                            582,
                            943,
                            582,
                            964,
                            560,
                            964
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 4404,
                            "length": 2
                        }
                    },
                    {
                        "content": "used",
                        "boundingBox": [
                            586,
                            943,
                            629,
                            943,
                            628,
                            964,
                            586,
                            964
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4407,
                            "length": 4
                        }
                    },
                    {
                        "content": "to",
                        "boundingBox": [
                            633,
                            943,
                            652,
                            943,
                            652,
                            964,
                            632,
                            964
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 4412,
                            "length": 2
                        }
                    },
                    {
                        "content": "arm",
                        "boundingBox": [
                            656,
                            943,
                            687,
                            944,
                            687,
                            964,
                            656,
                            964
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 4415,
                            "length": 3
                        }
                    },
                    {
                        "content": "the",
                        "boundingBox": [
                            696,
                            944,
                            728,
                            944,
                            728,
                            964,
                            695,
                            964
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 4419,
                            "length": 3
                        }
                    },
                    {
                        "content": "sales",
                        "boundingBox": [
                            469,
                            973,
                            514,
                            973,
                            514,
                            993,
                            469,
                            993
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 4423,
                            "length": 5
                        }
                    },
                    {
                        "content": "rep",
                        "boundingBox": [
                            518,
                            972,
                            547,
                            972,
                            547,
                            993,
                            518,
                            993
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 4429,
                            "length": 3
                        }
                    },
                    {
                        "content": "with",
                        "boundingBox": [
                            552,
                            972,
                            592,
                            972,
                            591,
                            993,
                            552,
                            993
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4433,
                            "length": 4
                        }
                    },
                    {
                        "content": "relevant",
                        "boundingBox": [
                            596,
                            972,
                            670,
                            972,
                            670,
                            993,
                            595,
                            993
                        ],
                        "confidence": 0.996,
                        "span": {
                            "offset": 4438,
                            "length": 8
                        }
                    },
                    {
                        "content": "information",
                        "boundingBox": [
                            674,
                            972,
                            780,
                            973,
                            780,
                            993,
                            674,
                            993
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 4447,
                            "length": 11
                        }
                    },
                    {
                        "content": "even",
                        "boundingBox": [
                            785,
                            973,
                            827,
                            974,
                            827,
                            993,
                            784,
                            993
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4459,
                            "length": 4
                        }
                    },
                    {
                        "content": "before",
                        "boundingBox": [
                            469,
                            1000,
                            526,
                            1000,
                            527,
                            1021,
                            470,
                            1021
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 4464,
                            "length": 6
                        }
                    },
                    {
                        "content": "he",
                        "boundingBox": [
                            531,
                            1000,
                            553,
                            1000,
                            553,
                            1021,
                            531,
                            1021
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 4471,
                            "length": 2
                        }
                    },
                    {
                        "content": "or",
                        "boundingBox": [
                            557,
                            1000,
                            576,
                            1000,
                            576,
                            1022,
                            557,
                            1022
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 4474,
                            "length": 2
                        }
                    },
                    {
                        "content": "she",
                        "boundingBox": [
                            580,
                            1000,
                            612,
                            1000,
                            612,
                            1022,
                            580,
                            1022
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 4477,
                            "length": 3
                        }
                    },
                    {
                        "content": "meets",
                        "boundingBox": [
                            616,
                            1000,
                            673,
                            1000,
                            673,
                            1022,
                            617,
                            1022
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 4481,
                            "length": 5
                        }
                    },
                    {
                        "content": "the",
                        "boundingBox": [
                            677,
                            1000,
                            707,
                            1000,
                            706,
                            1022,
                            677,
                            1022
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 4487,
                            "length": 3
                        }
                    },
                    {
                        "content": "doctor.",
                        "boundingBox": [
                            711,
                            1000,
                            773,
                            1001,
                            773,
                            1022,
                            711,
                            1022
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 4491,
                            "length": 7
                        }
                    },
                    {
                        "content": "It's",
                        "boundingBox": [
                            777,
                            1001,
                            804,
                            1001,
                            803,
                            1022,
                            777,
                            1022
                        ],
                        "confidence": 0.982,
                        "span": {
                            "offset": 4499,
                            "length": 4
                        }
                    },
                    {
                        "content": "a",
                        "boundingBox": [
                            808,
                            1001,
                            820,
                            1001,
                            820,
                            1022,
                            808,
                            1022
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4504,
                            "length": 1
                        }
                    },
                    {
                        "content": "totally",
                        "boundingBox": [
                            470,
                            1028,
                            525,
                            1028,
                            526,
                            1052,
                            470,
                            1051
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4506,
                            "length": 7
                        }
                    },
                    {
                        "content": "different,",
                        "boundingBox": [
                            530,
                            1028,
                            613,
                            1029,
                            613,
                            1053,
                            531,
                            1052
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 4514,
                            "length": 10
                        }
                    },
                    {
                        "content": "digital",
                        "boundingBox": [
                            618,
                            1029,
                            676,
                            1029,
                            676,
                            1053,
                            618,
                            1053
                        ],
                        "confidence": 0.998,
                        "span": {
                            "offset": 4525,
                            "length": 7
                        }
                    },
                    {
                        "content": "game",
                        "boundingBox": [
                            680,
                            1029,
                            729,
                            1029,
                            730,
                            1052,
                            681,
                            1053
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4533,
                            "length": 4
                        }
                    },
                    {
                        "content": "now.\"",
                        "boundingBox": [
                            734,
                            1029,
                            788,
                            1029,
                            788,
                            1051,
                            734,
                            1052
                        ],
                        "confidence": 0.985,
                        "span": {
                            "offset": 4538,
                            "length": 5
                        }
                    },
                    {
                        "content": "KEREN",
                        "boundingBox": [
                            470,
                            1068,
                            507,
                            1068,
                            506,
                            1083,
                            469,
                            1082
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4544,
                            "length": 5
                        }
                    },
                    {
                        "content": "PRIYADARSHINI",
                        "boundingBox": [
                            510,
                            1068,
                            603,
                            1069,
                            603,
                            1084,
                            509,
                            1083
                        ],
                        "confidence": 0.983,
                        "span": {
                            "offset": 4550,
                            "length": 13
                        }
                    },
                    {
                        "content": "|",
                        "boundingBox": [
                            606,
                            1069,
                            612,
                            1069,
                            611,
                            1084,
                            606,
                            1084
                        ],
                        "confidence": 0.986,
                        "span": {
                            "offset": 4564,
                            "length": 1
                        }
                    },
                    {
                        "content": "Regional",
                        "boundingBox": [
                            615,
                            1069,
                            656,
                            1069,
                            656,
                            1084,
                            614,
                            1084
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 4566,
                            "length": 8
                        }
                    },
                    {
                        "content": "Business",
                        "boundingBox": [
                            659,
                            1069,
                            698,
                            1069,
                            697,
                            1084,
                            658,
                            1084
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4575,
                            "length": 8
                        }
                    },
                    {
                        "content": "Lead",
                        "boundingBox": [
                            700,
                            1069,
                            724,
                            1069,
                            724,
                            1084,
                            700,
                            1084
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4584,
                            "length": 4
                        }
                    },
                    {
                        "content": "-",
                        "boundingBox": [
                            727,
                            1069,
                            733,
                            1069,
                            733,
                            1083,
                            727,
                            1084
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4589,
                            "length": 1
                        }
                    },
                    {
                        "content": "Asia,",
                        "boundingBox": [
                            736,
                            1069,
                            761,
                            1070,
                            760,
                            1083,
                            735,
                            1083
                        ],
                        "confidence": 0.968,
                        "span": {
                            "offset": 4591,
                            "length": 5
                        }
                    },
                    {
                        "content": "Worldwide",
                        "boundingBox": [
                            469,
                            1090,
                            518,
                            1090,
                            518,
                            1104,
                            469,
                            1104
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4597,
                            "length": 9
                        }
                    },
                    {
                        "content": "Health",
                        "boundingBox": [
                            521,
                            1090,
                            554,
                            1090,
                            553,
                            1104,
                            521,
                            1104
                        ],
                        "confidence": 0.999,
                        "span": {
                            "offset": 4607,
                            "length": 6
                        }
                    },
                    {
                        "content": "|",
                        "boundingBox": [
                            557,
                            1090,
                            563,
                            1090,
                            563,
                            1104,
                            557,
                            1104
                        ],
                        "confidence": 0.958,
                        "span": {
                            "offset": 4614,
                            "length": 1
                        }
                    },
                    {
                        "content": "Microsoft",
                        "boundingBox": [
                            566,
                            1090,
                            616,
                            1090,
                            615,
                            1104,
                            566,
                            1104
                        ],
                        "confidence": 0.995,
                        "span": {
                            "offset": 4616,
                            "length": 9
                        }
                    },
                    {
                        "content": "EMBRACING",
                        "boundingBox": [
                            77,
                            1141,
                            136,
                            1141,
                            136,
                            1153,
                            78,
                            1154
                        ],
                        "confidence": 0.98,
                        "span": {
                            "offset": 4626,
                            "length": 9
                        }
                    },
                    {
                        "content": "DIGITAL",
                        "boundingBox": [
                            138,
                            1141,
                            179,
                            1140,
                            180,
                            1153,
                            138,
                            1153
                        ],
                        "confidence": 0.985,
                        "span": {
                            "offset": 4636,
                            "length": 7
                        }
                    },
                    {
                        "content": "TRANSFORMATION",
                        "boundingBox": [
                            182,
                            1140,
                            275,
                            1140,
                            275,
                            1153,
                            182,
                            1153
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4644,
                            "length": 14
                        }
                    },
                    {
                        "content": "IN",
                        "boundingBox": [
                            278,
                            1140,
                            288,
                            1140,
                            288,
                            1153,
                            278,
                            1153
                        ],
                        "confidence": 0.997,
                        "span": {
                            "offset": 4659,
                            "length": 2
                        }
                    },
                    {
                        "content": "LIFE",
                        "boundingBox": [
                            290,
                            1140,
                            310,
                            1140,
                            310,
                            1153,
                            290,
                            1153
                        ],
                        "confidence": 0.975,
                        "span": {
                            "offset": 4662,
                            "length": 4
                        }
                    },
                    {
                        "content": "SCIENCES",
                        "boundingBox": [
                            312,
                            1140,
                            360,
                            1140,
                            360,
                            1153,
                            312,
                            1153
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4667,
                            "length": 8
                        }
                    },
                    {
                        "content": "ORGANIZATIONS",
                        "boundingBox": [
                            363,
                            1140,
                            451,
                            1141,
                            450,
                            1153,
                            362,
                            1153
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4676,
                            "length": 13
                        }
                    },
                    {
                        "content": "2",
                        "boundingBox": [
                            814,
                            1139,
                            822,
                            1140,
                            820,
                            1153,
                            812,
                            1152
                        ],
                        "confidence": 0.994,
                        "span": {
                            "offset": 4690,
                            "length": 1
                        }
                    }
                ],
                "lines": [
                    {
                        "content": "While healthcare is still in the early stages of its Al journey, we",
                        "boundingBox": [
                            259,
                            55,
                            816,
                            56,
                            816,
                            79,
                            259,
                            77
                        ],
                        "spans": [
                            {
                                "offset": 0,
                                "length": 67
                            }
                        ]
                    },
                    {
                        "content": "are seeing pharmaceutical and other life sciences organizations",
                        "boundingBox": [
                            258,
                            83,
                            825,
                            83,
                            825,
                            106,
                            258,
                            106
                        ],
                        "spans": [
                            {
                                "offset": 68,
                                "length": 63
                            }
                        ]
                    },
                    {
                        "content": "making major investments in Al and related technologies.\"",
                        "boundingBox": [
                            259,
                            112,
                            784,
                            112,
                            784,
                            136,
                            259,
                            136
                        ],
                        "spans": [
                            {
                                "offset": 132,
                                "length": 57
                            }
                        ]
                    },
                    {
                        "content": "TOM LAWRY | National Director for Al, Health and Life Sciences | Microsoft",
                        "boundingBox": [
                            257,
                            151,
                            638,
                            152,
                            638,
                            168,
                            257,
                            167
                        ],
                        "spans": [
                            {
                                "offset": 190,
                                "length": 74
                            }
                        ]
                    },
                    {
                        "content": "As pharmaceutical and other life sciences organizations invest",
                        "boundingBox": [
                            75,
                            241,
                            423,
                            241,
                            423,
                            258,
                            75,
                            257
                        ],
                        "spans": [
                            {
                                "offset": 265,
                                "length": 62
                            }
                        ]
                    },
                    {
                        "content": "in and deploy advanced technologies, they are beginning to see",
                        "boundingBox": [
                            76,
                            259,
                            435,
                            260,
                            435,
                            277,
                            76,
                            276
                        ],
                        "spans": [
                            {
                                "offset": 328,
                                "length": 62
                            }
                        ]
                    },
                    {
                        "content": "benefits in diverse areas across their organizations. Companies",
                        "boundingBox": [
                            75,
                            278,
                            427,
                            279,
                            427,
                            295,
                            75,
                            294
                        ],
                        "spans": [
                            {
                                "offset": 391,
                                "length": 63
                            }
                        ]
                    },
                    {
                        "content": "are looking to incorporate automation and continuing smart",
                        "boundingBox": [
                            77,
                            297,
                            413,
                            297,
                            413,
                            313,
                            77,
                            312
                        ],
                        "spans": [
                            {
                                "offset": 455,
                                "length": 58
                            }
                        ]
                    },
                    {
                        "content": "factory investments to reduce costs in drug discovery, research",
                        "boundingBox": [
                            76,
                            314,
                            427,
                            314,
                            427,
                            330,
                            76,
                            330
                        ],
                        "spans": [
                            {
                                "offset": 514,
                                "length": 63
                            }
                        ]
                    },
                    {
                        "content": "and development, and manufacturing and supply chain",
                        "boundingBox": [
                            77,
                            332,
                            388,
                            332,
                            388,
                            349,
                            77,
                            349
                        ],
                        "spans": [
                            {
                                "offset": 578,
                                "length": 51
                            }
                        ]
                    },
                    {
                        "content": "management. Many life sciences organizations are also choosing",
                        "boundingBox": [
                            76,
                            350,
                            440,
                            350,
                            440,
                            366,
                            76,
                            367
                        ],
                        "spans": [
                            {
                                "offset": 630,
                                "length": 62
                            }
                        ]
                    },
                    {
                        "content": "to stay with more virtual approaches in the \"new normal\" -",
                        "boundingBox": [
                            76,
                            369,
                            404,
                            368,
                            404,
                            384,
                            76,
                            385
                        ],
                        "spans": [
                            {
                                "offset": 693,
                                "length": 58
                            }
                        ]
                    },
                    {
                        "content": "particularly in clinical trials and sales and marketing areas.",
                        "boundingBox": [
                            76,
                            386,
                            396,
                            386,
                            396,
                            403,
                            76,
                            403
                        ],
                        "spans": [
                            {
                                "offset": 752,
                                "length": 62
                            }
                        ]
                    },
                    {
                        "content": "Enhancing the patient",
                        "boundingBox": [
                            77,
                            422,
                            267,
                            423,
                            267,
                            447,
                            77,
                            445
                        ],
                        "spans": [
                            {
                                "offset": 815,
                                "length": 21
                            }
                        ]
                    },
                    {
                        "content": "and provider experience",
                        "boundingBox": [
                            76,
                            447,
                            285,
                            448,
                            285,
                            469,
                            76,
                            468
                        ],
                        "spans": [
                            {
                                "offset": 837,
                                "length": 23
                            }
                        ]
                    },
                    {
                        "content": "Clinical trial sponsors are continually seeking to make clinical",
                        "boundingBox": [
                            76,
                            482,
                            425,
                            481,
                            425,
                            499,
                            76,
                            499
                        ],
                        "spans": [
                            {
                                "offset": 861,
                                "length": 64
                            }
                        ]
                    },
                    {
                        "content": "trials faster and to improve the experience for patients and",
                        "boundingBox": [
                            76,
                            501,
                            414,
                            501,
                            414,
                            517,
                            76,
                            517
                        ],
                        "spans": [
                            {
                                "offset": 926,
                                "length": 60
                            }
                        ]
                    },
                    {
                        "content": "physicians. The COVID-19 pandemic has accelerated the",
                        "boundingBox": [
                            76,
                            517,
                            409,
                            517,
                            409,
                            535,
                            76,
                            535
                        ],
                        "spans": [
                            {
                                "offset": 987,
                                "length": 53
                            }
                        ]
                    },
                    {
                        "content": "adoption of decentralized clinical trials, with an increase in",
                        "boundingBox": [
                            76,
                            536,
                            409,
                            536,
                            409,
                            553,
                            76,
                            553
                        ],
                        "spans": [
                            {
                                "offset": 1041,
                                "length": 62
                            }
                        ]
                    },
                    {
                        "content": "trial activities conducted remotely and in participants' homes.",
                        "boundingBox": [
                            76,
                            555,
                            429,
                            555,
                            429,
                            572,
                            76,
                            571
                        ],
                        "spans": [
                            {
                                "offset": 1104,
                                "length": 63
                            }
                        ]
                    },
                    {
                        "content": "In a Mckinsey survey,' up to 98 percent of patients reported",
                        "boundingBox": [
                            77,
                            572,
                            421,
                            573,
                            421,
                            590,
                            77,
                            589
                        ],
                        "spans": [
                            {
                                "offset": 1168,
                                "length": 60
                            }
                        ]
                    },
                    {
                        "content": "satisfaction with telemedicine. In the same report, 72 percent of",
                        "boundingBox": [
                            77,
                            591,
                            441,
                            592,
                            441,
                            608,
                            77,
                            607
                        ],
                        "spans": [
                            {
                                "offset": 1229,
                                "length": 65
                            }
                        ]
                    },
                    {
                        "content": "physicians surveyed reported similar or better experiences with",
                        "boundingBox": [
                            76,
                            610,
                            438,
                            609,
                            438,
                            625,
                            76,
                            626
                        ],
                        "spans": [
                            {
                                "offset": 1295,
                                "length": 63
                            }
                        ]
                    },
                    {
                        "content": "remote engagement compared with in-person visits.",
                        "boundingBox": [
                            76,
                            629,
                            377,
                            628,
                            377,
                            643,
                            76,
                            644
                        ],
                        "spans": [
                            {
                                "offset": 1359,
                                "length": 49
                            }
                        ]
                    },
                    {
                        "content": "The shift of trial activities closer to patients has been enabled by",
                        "boundingBox": [
                            74,
                            657,
                            443,
                            658,
                            443,
                            675,
                            74,
                            674
                        ],
                        "spans": [
                            {
                                "offset": 1409,
                                "length": 68
                            }
                        ]
                    },
                    {
                        "content": "a myriad of evolving technologies and services (e.g ., electronic",
                        "boundingBox": [
                            76,
                            676,
                            440,
                            676,
                            440,
                            693,
                            76,
                            693
                        ],
                        "spans": [
                            {
                                "offset": 1478,
                                "length": 65
                            }
                        ]
                    },
                    {
                        "content": "consent, telehealth and remote patient monitoring). The aim",
                        "boundingBox": [
                            77,
                            695,
                            427,
                            694,
                            427,
                            710,
                            77,
                            711
                        ],
                        "spans": [
                            {
                                "offset": 1544,
                                "length": 59
                            }
                        ]
                    },
                    {
                        "content": "to use technology to improve the patient experience and",
                        "boundingBox": [
                            76,
                            713,
                            405,
                            713,
                            405,
                            729,
                            76,
                            729
                        ],
                        "spans": [
                            {
                                "offset": 1604,
                                "length": 55
                            }
                        ]
                    },
                    {
                        "content": "convenience has also broadened trial access to reach a broader,",
                        "boundingBox": [
                            77,
                            730,
                            440,
                            730,
                            440,
                            746,
                            77,
                            746
                        ],
                        "spans": [
                            {
                                "offset": 1660,
                                "length": 63
                            }
                        ]
                    },
                    {
                        "content": "more diverse patient population.",
                        "boundingBox": [
                            77,
                            748,
                            264,
                            748,
                            264,
                            765,
                            77,
                            764
                        ],
                        "spans": [
                            {
                                "offset": 1724,
                                "length": 32
                            }
                        ]
                    },
                    {
                        "content": "\"It's an interesting and exciting time right now,\" said Keren",
                        "boundingBox": [
                            73,
                            780,
                            406,
                            780,
                            406,
                            797,
                            73,
                            797
                        ],
                        "spans": [
                            {
                                "offset": 1757,
                                "length": 61
                            }
                        ]
                    },
                    {
                        "content": "Priyadarshini, Regional Business Lead - Asia, Worldwide",
                        "boundingBox": [
                            76,
                            797,
                            401,
                            797,
                            401,
                            814,
                            76,
                            814
                        ],
                        "spans": [
                            {
                                "offset": 1819,
                                "length": 55
                            }
                        ]
                    },
                    {
                        "content": "Health, Microsoft. \"It used to be that physicians were key.",
                        "boundingBox": [
                            76,
                            815,
                            407,
                            817,
                            406,
                            834,
                            76,
                            831
                        ],
                        "spans": [
                            {
                                "offset": 1875,
                                "length": 59
                            }
                        ]
                    },
                    {
                        "content": "Now, suddenly, patients are feeling empowered by technology.",
                        "boundingBox": [
                            77,
                            833,
                            437,
                            835,
                            437,
                            851,
                            77,
                            850
                        ],
                        "spans": [
                            {
                                "offset": 1935,
                                "length": 60
                            }
                        ]
                    },
                    {
                        "content": "Pharmaceutical companies and other life sciences companies",
                        "boundingBox": [
                            77,
                            852,
                            428,
                            853,
                            428,
                            869,
                            77,
                            868
                        ],
                        "spans": [
                            {
                                "offset": 1996,
                                "length": 58
                            }
                        ]
                    },
                    {
                        "content": "are realizing they have to pay attention to the patient",
                        "boundingBox": [
                            76,
                            870,
                            383,
                            870,
                            383,
                            887,
                            76,
                            887
                        ],
                        "spans": [
                            {
                                "offset": 2055,
                                "length": 55
                            }
                        ]
                    },
                    {
                        "content": "experience in addition to the physician experience.",
                        "boundingBox": [
                            77,
                            889,
                            370,
                            888,
                            370,
                            904,
                            77,
                            904
                        ],
                        "spans": [
                            {
                                "offset": 2111,
                                "length": 51
                            }
                        ]
                    },
                    {
                        "content": "Enhanced patient experiences can be delivered in many different",
                        "boundingBox": [
                            75,
                            919,
                            442,
                            919,
                            442,
                            935,
                            75,
                            935
                        ],
                        "spans": [
                            {
                                "offset": 2163,
                                "length": 63
                            }
                        ]
                    },
                    {
                        "content": "ways. One example of a life sciences product that leverages the",
                        "boundingBox": [
                            75,
                            937,
                            433,
                            937,
                            433,
                            954,
                            75,
                            954
                        ],
                        "spans": [
                            {
                                "offset": 2227,
                                "length": 63
                            }
                        ]
                    },
                    {
                        "content": "intelligent cloud to directly affect the patient experience is the",
                        "boundingBox": [
                            76,
                            955,
                            423,
                            955,
                            423,
                            972,
                            76,
                            972
                        ],
                        "spans": [
                            {
                                "offset": 2291,
                                "length": 66
                            }
                        ]
                    },
                    {
                        "content": "Tandem&reg; Diabetes Care insulin pump. The Tandem&reg; t:slim X2",
                        "boundingBox": [
                            74,
                            972,
                            423,
                            971,
                            423,
                            989,
                            74,
                            990
                        ],
                        "spans": [
                            {
                                "offset": 2358,
                                "length": 57
                            }
                        ]
                    },
                    {
                        "content": "insulin pump with Basal-IQ technology enables patients with",
                        "boundingBox": [
                            75,
                            991,
                            417,
                            991,
                            417,
                            1008,
                            75,
                            1008
                        ],
                        "spans": [
                            {
                                "offset": 2416,
                                "length": 59
                            }
                        ]
                    },
                    {
                        "content": "Type 1 diabetes to predict and prevent the low levels of blood",
                        "boundingBox": [
                            75,
                            1008,
                            419,
                            1008,
                            419,
                            1025,
                            75,
                            1026
                        ],
                        "spans": [
                            {
                                "offset": 2476,
                                "length": 62
                            }
                        ]
                    },
                    {
                        "content": "sugar that cause hypoglycemia.2 The algorithm-driven, software-",
                        "boundingBox": [
                            75,
                            1027,
                            439,
                            1026,
                            439,
                            1044,
                            75,
                            1045
                        ],
                        "spans": [
                            {
                                "offset": 2539,
                                "length": 63
                            }
                        ]
                    },
                    {
                        "content": "updatable pump improves the patient experience by automating",
                        "boundingBox": [
                            77,
                            1046,
                            439,
                            1046,
                            439,
                            1062,
                            77,
                            1062
                        ],
                        "spans": [
                            {
                                "offset": 2603,
                                "length": 60
                            }
                        ]
                    },
                    {
                        "content": "chronic disease management and eliminating the need for",
                        "boundingBox": [
                            76,
                            1063,
                            402,
                            1063,
                            402,
                            1080,
                            76,
                            1080
                        ],
                        "spans": [
                            {
                                "offset": 2664,
                                "length": 55
                            }
                        ]
                    },
                    {
                        "content": "constant finger pricks to check glucose levels.",
                        "boundingBox": [
                            77,
                            1081,
                            332,
                            1081,
                            332,
                            1098,
                            77,
                            1099
                        ],
                        "spans": [
                            {
                                "offset": 2720,
                                "length": 47
                            }
                        ]
                    },
                    {
                        "content": "Tandem was able to create and deploy this innovation by",
                        "boundingBox": [
                            466,
                            241,
                            793,
                            241,
                            793,
                            258,
                            466,
                            257
                        ],
                        "spans": [
                            {
                                "offset": 2768,
                                "length": 55
                            }
                        ]
                    },
                    {
                        "content": "leveraging the Al and machine learning capabilities of the",
                        "boundingBox": [
                            469,
                            260,
                            801,
                            260,
                            801,
                            276,
                            469,
                            277
                        ],
                        "spans": [
                            {
                                "offset": 2824,
                                "length": 58
                            }
                        ]
                    },
                    {
                        "content": "intelligent cloud. As Al and other technologies continue to",
                        "boundingBox": [
                            468,
                            278,
                            808,
                            278,
                            808,
                            295,
                            468,
                            294
                        ],
                        "spans": [
                            {
                                "offset": 2883,
                                "length": 59
                            }
                        ]
                    },
                    {
                        "content": "advance, potential use cases will multiply. \"Speed to value is",
                        "boundingBox": [
                            468,
                            296,
                            811,
                            296,
                            811,
                            313,
                            468,
                            313
                        ],
                        "spans": [
                            {
                                "offset": 2943,
                                "length": 62
                            }
                        ]
                    },
                    {
                        "content": "going to continue to accelerate,\" said Lawry.",
                        "boundingBox": [
                            468,
                            316,
                            722,
                            315,
                            722,
                            330,
                            468,
                            332
                        ],
                        "spans": [
                            {
                                "offset": 3006,
                                "length": 45
                            }
                        ]
                    },
                    {
                        "content": "In addition to enhancing the patient experience,",
                        "boundingBox": [
                            467,
                            344,
                            746,
                            346,
                            746,
                            362,
                            467,
                            360
                        ],
                        "spans": [
                            {
                                "offset": 3052,
                                "length": 48
                            }
                        ]
                    },
                    {
                        "content": "pharmaceutical and other life sciences companies can",
                        "boundingBox": [
                            469,
                            363,
                            778,
                            364,
                            778,
                            380,
                            469,
                            379
                        ],
                        "spans": [
                            {
                                "offset": 3101,
                                "length": 52
                            }
                        ]
                    },
                    {
                        "content": "leverage advanced technologies to improve relationships with",
                        "boundingBox": [
                            466,
                            381,
                            824,
                            381,
                            824,
                            398,
                            466,
                            398
                        ],
                        "spans": [
                            {
                                "offset": 3154,
                                "length": 60
                            }
                        ]
                    },
                    {
                        "content": "providers. For example, COVID-19 is driving changes in the",
                        "boundingBox": [
                            467,
                            399,
                            814,
                            398,
                            814,
                            416,
                            467,
                            416
                        ],
                        "spans": [
                            {
                                "offset": 3215,
                                "length": 58
                            }
                        ]
                    },
                    {
                        "content": "way companies interact with clinicians. Prior to COVID-19,",
                        "boundingBox": [
                            468,
                            418,
                            806,
                            416,
                            806,
                            433,
                            468,
                            435
                        ],
                        "spans": [
                            {
                                "offset": 3274,
                                "length": 58
                            }
                        ]
                    },
                    {
                        "content": "75 percent of physicians preferred in-person sales visits from",
                        "boundingBox": [
                            467,
                            435,
                            815,
                            435,
                            815,
                            452,
                            467,
                            452
                        ],
                        "spans": [
                            {
                                "offset": 3333,
                                "length": 62
                            }
                        ]
                    },
                    {
                        "content": "medtech reps; likewise, 77 percent of physicians preferred in-",
                        "boundingBox": [
                            468,
                            453,
                            818,
                            453,
                            818,
                            470,
                            468,
                            470
                        ],
                        "spans": [
                            {
                                "offset": 3396,
                                "length": 62
                            }
                        ]
                    },
                    {
                        "content": "person sales visits from pharma reps.3",
                        "boundingBox": [
                            468,
                            472,
                            685,
                            472,
                            685,
                            487,
                            468,
                            488
                        ],
                        "spans": [
                            {
                                "offset": 3459,
                                "length": 38
                            }
                        ]
                    },
                    {
                        "content": "Since the advent of COVID-19, however, physician",
                        "boundingBox": [
                            468,
                            502,
                            770,
                            503,
                            770,
                            520,
                            468,
                            519
                        ],
                        "spans": [
                            {
                                "offset": 3498,
                                "length": 48
                            }
                        ]
                    },
                    {
                        "content": "preferences are moving toward virtual visits. Only 53 percent",
                        "boundingBox": [
                            468,
                            521,
                            828,
                            521,
                            828,
                            537,
                            468,
                            538
                        ],
                        "spans": [
                            {
                                "offset": 3547,
                                "length": 61
                            }
                        ]
                    },
                    {
                        "content": "of physicians now express a preference for in-person visits",
                        "boundingBox": [
                            468,
                            539,
                            811,
                            539,
                            811,
                            556,
                            468,
                            556
                        ],
                        "spans": [
                            {
                                "offset": 3609,
                                "length": 59
                            }
                        ]
                    },
                    {
                        "content": "from medtech reps and only 40 percent prefer in-person visits",
                        "boundingBox": [
                            468,
                            557,
                            834,
                            557,
                            834,
                            575,
                            468,
                            574
                        ],
                        "spans": [
                            {
                                "offset": 3669,
                                "length": 61
                            }
                        ]
                    },
                    {
                        "content": "from pharma reps.\" That puts the onus on pharmaceutical and",
                        "boundingBox": [
                            469,
                            575,
                            835,
                            575,
                            835,
                            592,
                            469,
                            592
                        ],
                        "spans": [
                            {
                                "offset": 3731,
                                "length": 59
                            }
                        ]
                    },
                    {
                        "content": "life sciences organizations to deliver valuable and engaging",
                        "boundingBox": [
                            469,
                            593,
                            818,
                            594,
                            818,
                            610,
                            469,
                            609
                        ],
                        "spans": [
                            {
                                "offset": 3791,
                                "length": 60
                            }
                        ]
                    },
                    {
                        "content": "virtual visits to providers.",
                        "boundingBox": [
                            468,
                            611,
                            615,
                            612,
                            615,
                            627,
                            468,
                            626
                        ],
                        "spans": [
                            {
                                "offset": 3852,
                                "length": 28
                            }
                        ]
                    },
                    {
                        "content": "One way to do that is to leverage text analytics capabilities to",
                        "boundingBox": [
                            468,
                            642,
                            822,
                            643,
                            822,
                            659,
                            468,
                            658
                        ],
                        "spans": [
                            {
                                "offset": 3881,
                                "length": 64
                            }
                        ]
                    },
                    {
                        "content": "enhance the provider information stored in the organization's",
                        "boundingBox": [
                            469,
                            661,
                            820,
                            660,
                            820,
                            676,
                            469,
                            676
                        ],
                        "spans": [
                            {
                                "offset": 3946,
                                "length": 61
                            }
                        ]
                    },
                    {
                        "content": "customer relationship management (CRM) system. For",
                        "boundingBox": [
                            469,
                            679,
                            788,
                            678,
                            788,
                            695,
                            469,
                            696
                        ],
                        "spans": [
                            {
                                "offset": 4008,
                                "length": 50
                            }
                        ]
                    },
                    {
                        "content": "example, a rep setting up a visit with 'Dr. X' could run text",
                        "boundingBox": [
                            470,
                            697,
                            802,
                            696,
                            802,
                            712,
                            470,
                            714
                        ],
                        "spans": [
                            {
                                "offset": 4059,
                                "length": 61
                            }
                        ]
                    },
                    {
                        "content": "analytics on publicly available resources on the web to identify",
                        "boundingBox": [
                            468,
                            715,
                            825,
                            714,
                            825,
                            730,
                            468,
                            731
                        ],
                        "spans": [
                            {
                                "offset": 4121,
                                "length": 64
                            }
                        ]
                    },
                    {
                        "content": "on which specific topics Dr. X has been writing about and",
                        "boundingBox": [
                            468,
                            732,
                            800,
                            731,
                            800,
                            749,
                            468,
                            749
                        ],
                        "spans": [
                            {
                                "offset": 4186,
                                "length": 57
                            }
                        ]
                    },
                    {
                        "content": "commenting. \"All kinds of publicly available information can",
                        "boundingBox": [
                            468,
                            749,
                            813,
                            749,
                            813,
                            767,
                            468,
                            768
                        ],
                        "spans": [
                            {
                                "offset": 4244,
                                "length": 60
                            }
                        ]
                    },
                    {
                        "content": "All kinds of publicly",
                        "boundingBox": [
                            467,
                            827,
                            645,
                            829,
                            645,
                            851,
                            467,
                            849
                        ],
                        "spans": [
                            {
                                "offset": 4305,
                                "length": 21
                            }
                        ]
                    },
                    {
                        "content": "available information",
                        "boundingBox": [
                            469,
                            857,
                            661,
                            857,
                            661,
                            877,
                            469,
                            877
                        ],
                        "spans": [
                            {
                                "offset": 4327,
                                "length": 21
                            }
                        ]
                    },
                    {
                        "content": "can be mined with text",
                        "boundingBox": [
                            468,
                            886,
                            674,
                            886,
                            674,
                            906,
                            468,
                            906
                        ],
                        "spans": [
                            {
                                "offset": 4349,
                                "length": 22
                            }
                        ]
                    },
                    {
                        "content": "analytics technology,",
                        "boundingBox": [
                            468,
                            915,
                            660,
                            915,
                            660,
                            938,
                            468,
                            937
                        ],
                        "spans": [
                            {
                                "offset": 4372,
                                "length": 21
                            }
                        ]
                    },
                    {
                        "content": "which can be used to arm the",
                        "boundingBox": [
                            468,
                            942,
                            731,
                            943,
                            731,
                            964,
                            468,
                            963
                        ],
                        "spans": [
                            {
                                "offset": 4394,
                                "length": 28
                            }
                        ]
                    },
                    {
                        "content": "sales rep with relevant information even",
                        "boundingBox": [
                            468,
                            971,
                            830,
                            971,
                            830,
                            993,
                            468,
                            992
                        ],
                        "spans": [
                            {
                                "offset": 4423,
                                "length": 40
                            }
                        ]
                    },
                    {
                        "content": "before he or she meets the doctor. It's a",
                        "boundingBox": [
                            469,
                            999,
                            823,
                            1000,
                            823,
                            1021,
                            469,
                            1020
                        ],
                        "spans": [
                            {
                                "offset": 4464,
                                "length": 41
                            }
                        ]
                    },
                    {
                        "content": "totally different, digital game now.\"",
                        "boundingBox": [
                            469,
                            1028,
                            788,
                            1028,
                            788,
                            1052,
                            469,
                            1052
                        ],
                        "spans": [
                            {
                                "offset": 4506,
                                "length": 37
                            }
                        ]
                    },
                    {
                        "content": "KEREN PRIYADARSHINI | Regional Business Lead - Asia,",
                        "boundingBox": [
                            469,
                            1068,
                            760,
                            1069,
                            760,
                            1084,
                            469,
                            1083
                        ],
                        "spans": [
                            {
                                "offset": 4544,
                                "length": 52
                            }
                        ]
                    },
                    {
                        "content": "Worldwide Health | Microsoft",
                        "boundingBox": [
                            468,
                            1089,
                            615,
                            1089,
                            615,
                            1104,
                            468,
                            1103
                        ],
                        "spans": [
                            {
                                "offset": 4597,
                                "length": 28
                            }
                        ]
                    },
                    {
                        "content": "EMBRACING DIGITAL TRANSFORMATION IN LIFE SCIENCES ORGANIZATIONS",
                        "boundingBox": [
                            77,
                            1140,
                            451,
                            1140,
                            451,
                            1153,
                            77,
                            1153
                        ],
                        "spans": [
                            {
                                "offset": 4626,
                                "length": 63
                            }
                        ]
                    },
                    {
                        "content": "2",
                        "boundingBox": [
                            812,
                            1139,
                            824,
                            1140,
                            821,
                            1154,
                            810,
                            1152
                        ],
                        "spans": [
                            {
                                "offset": 4690,
                                "length": 1
                            }
                        ]
                    }
                ],
                "spans": [
                    {
                        "offset": 0,
                        "length": 4691
                    }
                ]
            }
        ],
        "languages": [
            {
                "spans": [
                    {
                        "offset": 0,
                        "length": 131
                    },
                    {
                        "offset": 265,
                        "length": 125
                    },
                    {
                        "offset": 455,
                        "length": 58
                    },
                    {
                        "offset": 578,
                        "length": 51
                    },
                    {
                        "offset": 837,
                        "length": 88
                    },
                    {
                        "offset": 1041,
                        "length": 62
                    },
                    {
                        "offset": 1229,
                        "length": 65
                    },
                    {
                        "offset": 1660,
                        "length": 63
                    },
                    {
                        "offset": 1875,
                        "length": 120
                    },
                    {
                        "offset": 2476,
                        "length": 62
                    },
                    {
                        "offset": 2664,
                        "length": 55
                    },
                    {
                        "offset": 2768,
                        "length": 114
                    },
                    {
                        "offset": 2943,
                        "length": 62
                    },
                    {
                        "offset": 3052,
                        "length": 48
                    },
                    {
                        "offset": 3154,
                        "length": 178
                    },
                    {
                        "offset": 3396,
                        "length": 62
                    },
                    {
                        "offset": 3547,
                        "length": 61
                    },
                    {
                        "offset": 3731,
                        "length": 149
                    },
                    {
                        "offset": 3946,
                        "length": 61
                    },
                    {
                        "offset": 4423,
                        "length": 40
                    },
                    {
                        "offset": 4506,
                        "length": 37
                    }
                ],
                "languageCode": "en",
                "confidence": 0.95
            },
            {
                "spans": [
                    {
                        "offset": 132,
                        "length": 57
                    },
                    {
                        "offset": 514,
                        "length": 63
                    },
                    {
                        "offset": 1724,
                        "length": 32
                    },
                    {
                        "offset": 2416,
                        "length": 59
                    },
                    {
                        "offset": 2603,
                        "length": 60
                    },
                    {
                        "offset": 2883,
                        "length": 59
                    },
                    {
                        "offset": 3609,
                        "length": 59
                    },
                    {
                        "offset": 3881,
                        "length": 64
                    }
                ],
                "languageCode": "en",
                "confidence": 0.9
            },
            {
                "spans": [
                    {
                        "offset": 190,
                        "length": 74
                    },
                    {
                        "offset": 815,
                        "length": 21
                    },
                    {
                        "offset": 987,
                        "length": 53
                    },
                    {
                        "offset": 1478,
                        "length": 125
                    },
                    {
                        "offset": 2720,
                        "length": 47
                    },
                    {
                        "offset": 4008,
                        "length": 50
                    }
                ],
                "languageCode": "en",
                "confidence": 0.8
            },
            {
                "spans": [
                    {
                        "offset": 391,
                        "length": 63
                    },
                    {
                        "offset": 926,
                        "length": 60
                    },
                    {
                        "offset": 1104,
                        "length": 63
                    },
                    {
                        "offset": 1409,
                        "length": 68
                    },
                    {
                        "offset": 1604,
                        "length": 55
                    },
                    {
                        "offset": 1757,
                        "length": 61
                    },
                    {
                        "offset": 2163,
                        "length": 63
                    },
                    {
                        "offset": 2291,
                        "length": 66
                    },
                    {
                        "offset": 3459,
                        "length": 87
                    },
                    {
                        "offset": 4059,
                        "length": 61
                    },
                    {
                        "offset": 4186,
                        "length": 57
                    },
                    {
                        "offset": 4327,
                        "length": 21
                    },
                    {
                        "offset": 4394,
                        "length": 28
                    },
                    {
                        "offset": 4464,
                        "length": 41
                    }
                ],
                "languageCode": "en",
                "confidence": 1
            },
            {
                "spans": [
                    {
                        "offset": 630,
                        "length": 184
                    },
                    {
                        "offset": 1168,
                        "length": 60
                    },
                    {
                        "offset": 1295,
                        "length": 113
                    },
                    {
                        "offset": 1996,
                        "length": 166
                    },
                    {
                        "offset": 2227,
                        "length": 63
                    },
                    {
                        "offset": 3006,
                        "length": 45
                    },
                    {
                        "offset": 3101,
                        "length": 52
                    },
                    {
                        "offset": 3333,
                        "length": 62
                    },
                    {
                        "offset": 3669,
                        "length": 61
                    },
                    {
                        "offset": 4121,
                        "length": 64
                    },
                    {
                        "offset": 4244,
                        "length": 82
                    },
                    {
                        "offset": 4349,
                        "length": 22
                    }
                ],
                "languageCode": "en",
                "confidence": 0.99
            },
            {
                "spans": [
                    {
                        "offset": 1819,
                        "length": 55
                    },
                    {
                        "offset": 2539,
                        "length": 63
                    },
                    {
                        "offset": 4597,
                        "length": 28
                    }
                ],
                "languageCode": "en",
                "confidence": 0.6
            },
            {
                "spans": [
                    {
                        "offset": 2358,
                        "length": 57
                    }
                ],
                "languageCode": "en",
                "confidence": 0.4
            },
            {
                "spans": [
                    {
                        "offset": 4372,
                        "length": 21
                    }
                ],
                "languageCode": "en",
                "confidence": 0.5
            },
            {
                "spans": [
                    {
                        "offset": 4544,
                        "length": 52
                    }
                ],
                "languageCode": "en",
                "confidence": 0.2
            },
            {
                "spans": [
                    {
                        "offset": 4626,
                        "length": 63
                    }
                ],
                "languageCode": "en",
                "confidence": 0.7
            }
        ],
        "styles": []
    }
}
```
