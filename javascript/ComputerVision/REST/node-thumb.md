
# Quickstart: Generate a thumbnail using the Computer Vision REST API and Node.js

In this quickstart, you'll generate a thumbnail from an image using the Computer Vision REST API. With the [Get Thumbnail](https://westcentralus.dev.cognitive.microsoft.com/docs/services/computer-vision-v3-1-ga/operations/56f91f2e778daf14a499f20c) method, you can generate a thumbnail of an image. You specify the height and width, which can differ from the aspect ratio of the input image. Computer Vision uses smart cropping to intelligently identify the area of interest and generate cropping coordinates based on that region.

## Prerequisites

* An Azure subscription - [Create one for free](https://azure.microsoft.com/free/cognitive-services/)
* [Node.js](https://nodejs.org) 4.x or later 
* [npm](https://www.npmjs.com/) 
* Once you have your Azure subscription, <a href="https://portal.azure.com/#create/Microsoft.CognitiveServicesComputerVision"  title="Create a Computer Vision resource"  target="_blank">create a Computer Vision resource <span class="docon docon-navigate-external x-hidden-focus"></span></a> in the Azure portal to get your key and endpoint. After it deploys, click **Go to resource**.
    * You will need the key and endpoint from the resource you create to connect your application to the Computer Vision service. You'll paste your key and endpoint into the code below later in the quickstart.
    * You can use the free pricing tier (`F0`) to try the service, and upgrade later to a paid tier for production.

## Create and run the sample

To create and run the sample, do the following steps:

1. Install the npm [`request`](https://www.npmjs.com/package/request) package.
   1. Open a command prompt window as an administrator.
   1. Run the following command:

      ```console
      npm install request
      ```

   1. After the package is successfully installed, close the command prompt window.

1. Copy the following code into a text editor.
1. Replace the values of `key` and `endpoint` with your Computer Vision key and endpoint.
1. Optionally, replace the value of `imageUrl` with the URL of a different image that you want to analyze.
1. Save the code as a file with a `.js` extension. For example, `get-thumbnail.js`.
1. Open a command prompt window.
1. At the prompt, use the `node` command to run the file. For example, `node get-thumbnail.js`.

```javascript
'use strict';

const fs = require('fs');
const request = require('request').defaults({ encoding: null });

let key = 'PASTE_YOUR_COMPUTER_VISION_KEY_HERE';
let endpoint = 'PASTE_YOUR_COMPUTER_VISION_ENDPOINT_HERE';

var uriBase = endpoint + 'vision/v3.1/generateThumbnail';

const imageUrl = 'https://upload.wikimedia.org/wikipedia/commons/9/94/Bloodhound_Puppy.jpg';

// Request parameters.
const params = {
    'width': '100',
    'height': '100',
    'smartCropping': 'true'
};

// Construct the request
const options = {
    uri: uriBase,
    qs: params,
    body: '{"url": ' + '"' + imageUrl + '"}',
    headers: {
        'Content-Type': 'application/json',
        'Ocp-Apim-Subscription-Key' : key
    }
}

// Post the request and get the response (an image stream)
request.post(options, (error, response, body) => {
    // Write the stream to file
    var buf = Buffer.from(body, 'base64');
    fs.writeFile('thumbnail.png', buf, function (err) {
        if (err) throw err;
    });

    console.log('Image saved')
});
```

## Examine the response

A popup of the thumbnail image will display.
A successful response is returned as binary data, which represents the image data for the thumbnail. If the request fails, the response is displayed in the console window. The response for the failed request contains an error code and a message to help determine what went wrong.

## Next steps

Next, explore the Computer Vision API used to analyze an image, detect celebrities and landmarks, create a thumbnail, and extract printed and handwritten text.

* Explore the Computer Vision API](https://westcentralus.dev.cognitive.microsoft.com/docs/services/computer-vision-v3-1-ga/operations/56f91f2e778daf14a499f20d)
