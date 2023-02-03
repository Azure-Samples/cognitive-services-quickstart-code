
# Quickstart: Analyze a local image using the Computer Vision REST API and Python

In this quickstart, you'll analyze a locally stored image to extract visual features using the Computer Vision REST API. With the [Analyze Image](https://westcentralus.dev.cognitive.microsoft.com/docs/services/computer-vision-v3-1-ga/operations/56f91f2e778daf14a499f21b) method, you can extract visual features based on image content.

You can run this quickstart in a step-by step fashion using a Jupyter Notebook on [MyBinder](https://mybinder.org). To launch Binder, select the following button:

[![Binder](https://mybinder.org/badge.svg)](https://mybinder.org/v2/gh/Microsoft/cognitive-services-notebooks/master?filepath=VisionAPI.ipynb)

## Prerequisites

* An Azure subscription - [Create one for free](https://azure.microsoft.com/free/cognitive-services/)
* [Python](https://www.python.org/downloads/) and the following packages:
  * requests
  * [matplotlib](https://matplotlib.org/)
  * [pillow](https://python-pillow.org/)
* Once you have your Azure subscription, <a href="https://portal.azure.com/#create/Microsoft.CognitiveServicesComputerVision"  title="Create a Computer Vision resource"  target="_blank">create a Computer Vision resource <span class="docon docon-navigate-external x-hidden-focus"></span></a> in the Azure portal to get your key and endpoint. After it deploys, click **Go to resource**.
    * You will need the key and endpoint from the resource you create to connect your application to the Computer Vision service. You'll paste your key and endpoint into the code below later in the quickstart.
    * You can use the free pricing tier (`F0`) to try the service, and upgrade later to a paid tier for production.
* [Create environment variables](../../cognitive-services-apis-create-account.md#configure-an-environment-variable-for-authentication) for the key and endpoint URL, named `COMPUTER_VISION_KEY` and `COMPUTER_VISION_ENDPOINT`, respectively.

## Create and run the sample

To create and run the sample, do the following steps:

1. Copy the following code into a text editor.
1. Replace the value of `image_path` with the path and file name of a different image that you want to analyze.
1. Save the code as a file with an `.py` extension. For example, `analyze-local-image.py`.
1. Open a command prompt window.
1. At the prompt, use the `python` command to run the sample. For example, `python analyze-local-image.py`.

```python
import os
import sys
import requests
# If you are using a Jupyter Notebook, uncomment the following line.
# %matplotlib inline
import matplotlib.pyplot as plt
from PIL import Image
from io import BytesIO

# Add your Computer Vision key and endpoint to your environment variables.
if 'COMPUTER_VISION_KEY' in os.environ:
    subscription_key = os.environ['COMPUTER_VISION_KEY']
else:
    print("\nSet the COMPUTER_VISION_KEY environment variable.\n**Restart your shell or IDE for changes to take effect.**")
    sys.exit()

if 'COMPUTER_VISION_ENDPOINT' in os.environ:
    endpoint = os.environ['COMPUTER_VISION_ENDPOINT']

analyze_url = endpoint + "vision/v3.1/analyze"

# Set image_path to the local path of an image that you want to analyze.
# Sample images are here, if needed:
# https://github.com/Azure-Samples/cognitive-services-sample-data-files/tree/master/ComputerVision/Images
image_path = "C:/Documents/ImageToAnalyze.jpg"

# Read the image into a byte array
image_data = open(image_path, "rb").read()
headers = {'Ocp-Apim-Subscription-Key': subscription_key,
           'Content-Type': 'application/octet-stream'}
params = {'visualFeatures': 'Categories,Description,Color'}
response = requests.post(
    analyze_url, headers=headers, params=params, data=image_data)
response.raise_for_status()

# The 'analysis' object contains various fields that describe the image. The most
# relevant caption for the image is obtained from the 'description' property.
analysis = response.json()
print(analysis)
image_caption = analysis["description"]["captions"][0]["text"].capitalize()

# Display the image and overlay it with the caption.
image = Image.open(BytesIO(image_data))
plt.imshow(image)
plt.axis("off")
_ = plt.title(image_caption, size="x-large", y=-0.1)
plt.show()
```

## Examine the response

A successful response is returned in JSON. The sample webpage parses and displays a successful response in the command prompt window, similar to the following example:

```json
{
  "categories": [
    {
      "name": "outdoor_",
      "score": 0.00390625,
      "detail": {
        "landmarks": []
      }
    },
    {
      "name": "outdoor_street",
      "score": 0.33984375,
      "detail": {
        "landmarks": []
      }
    }
  ],
  "description": {
    "tags": [
      "building",
      "outdoor",
      "street",
      "city",
      "people",
      "busy",
      "table",
      "walking",
      "traffic",
      "filled",
      "large",
      "many",
      "group",
      "night",
      "light",
      "crowded",
      "bunch",
      "standing",
      "man",
      "sign",
      "crowd",
      "umbrella",
      "riding",
      "tall",
      "woman",
      "bus"
    ],
    "captions": [
      {
        "text": "a group of people on a city street at night",
        "confidence": 0.9122243847383961
      }
    ]
  },
  "color": {
    "dominantColorForeground": "Brown",
    "dominantColorBackground": "Brown",
    "dominantColors": [
      "Brown"
    ],
    "accentColor": "B54316",
    "isBwImg": false
  },
  "requestId": "c11894eb-de3e-451b-9257-7c8b168073d1",
  "metadata": {
    "height": 600,
    "width": 450,
    "format": "Jpeg"
  }
}
```

## Next steps

Next, explore a Python application that uses Computer Vision to perform optical character recognition (OCR); create smart-cropped thumbnails; and detect, categorize, tag, and describe visual features in images.

* [Computer Vision API Python Tutorial](https://github.com/Microsoft/Cognitive-Vision-Python)

* To rapidly experiment with the Computer Vision API, try the [Open API testing console](https://westcentralus.dev.cognitive.microsoft.com/docs/services/computer-vision-v3-1-ga/operations/56f91f2e778daf14a499f21b/console).
