
# Quickstart: Analyze a remote image using the Computer Vision REST API and PHP

In this quickstart, you analyze a remotely stored image to extract visual features by using Computer Vision's REST API. With the [Analyze Image](https://westcentralus.dev.cognitive.microsoft.com/docs/services/5adf991815e1060e6355ad44/operations/56f91f2e778daf14a499e1fa) method, you can extract visual features based on image content.

If you don't have an Azure subscription, create a [free account](https://azure.microsoft.com/free/ai/?ref=microsoft.com&utm_source=microsoft.com&utm_medium=docs&utm_campaign=cognitive-services) before you begin.

## Prerequisites

- You must have [PHP](https://secure.php.net/downloads.php) installed.
- You must have [Pear](https://pear.php.net) installed.
- You must have a key for Computer Vision. You can visit [the Microsoft Cognitive Services Web site](https://azure.microsoft.com/free/cognitive-services/), create a new Azure account, and try Cognitive Services for free. Or, follow the instructions in [Create a Cognitive Services account](https://docs.microsoft.com/azure/cognitive-services/cognitive-services-apis-create-account) to subscribe to Computer Vision and get your key.

## Create and run the sample

To create and run the sample, do the following steps:

1. Install the PHP5 [`HTTP_Request2`](https://pear.php.net/package/HTTP_Request2) package.
   1. Open a command prompt window as an administrator.
   1. Run the following command:

      ```console
      pear install HTTP_Request2
      ```

   1. After the package is successfully installed, close the command prompt window.

1. Copy the following code into a text editor.
1. Make the following changes in code where needed:
    1. Replace the value of `key` with your key.
    1. Replace the value of `uriBase` with the endpoint URL for the [Analyze Image](https://westcentralus.dev.cognitive.microsoft.com/docs/services/5adf991815e1060e6355ad44/operations/56f91f2e778daf14a499e1fa) method from the Azure region where you obtained your keys, if necessary.
    1. Optionally, replace the value of `imageUrl` with the URL of a different image that you want to analyze.
    1. Optionally, replace the value of the `language` request parameter with a different language.
1. Save the code as a file with a `.php` extension. For example, `analyze-image.php`.
1. Open a browser window with PHP support.
1. Drag and drop the file into the browser window.

```php
<html>
<head>
    <title>Analyze Image Sample</title>
</head>
<body>
<?php
// Replace <key> with a valid key.
$ocpApimkey = '<key>';

// You must use the same location in your REST call as you used to obtain
// your keys. For example, if you obtained your keys
// from westus, replace "westcentralus" in the URL below with "westus".
$uriBase = 'https://westcentralus.api.cognitive.microsoft.com/vision/v2.0/';

$imageUrl = 'https://upload.wikimedia.org/wikipedia/commons/3/3c/Shaki_waterfall.jpg';

require_once 'HTTP/Request2.php';

$request = new Http_Request2($uriBase . '/analyze');
$url = $request->getUrl();

$headers = array(
    // Request headers
    'Content-Type' => 'application/json',
    'Ocp-Apim-Subscription-Key' => $ocpApimkey
);
$request->setHeader($headers);

$parameters = array(
    // Request parameters
    'visualFeatures' => 'Categories,Description',
    'details' => '',
    'language' => 'en'
);
$url->setQueryVariables($parameters);

$request->setMethod(HTTP_Request2::METHOD_POST);

// Request body parameters
$body = json_encode(array('url' => $imageUrl));

// Request body
$request->setBody($body);

try
{
    $response = $request->send();
    echo "<pre>" .
        json_encode(json_decode($response->getBody()), JSON_PRETTY_PRINT) . "</pre>";
}
catch (HttpException $ex)
{
    echo "<pre>" . $ex . "</pre>";
}
?>
</body>
</html>
```

## Examine the response

A successful response is returned in JSON. The sample website parses and displays a successful response in the browser window, similar to the following example:

```json
{
  "categories": [
    {
      "name": "outdoor_water",
      "score": 0.9921875,
      "detail": {
        "landmarks": []
      }
    }
  ],
  "description": {
    "tags": [
      "nature",
      "water",
      "waterfall",
      "outdoor",
      "rock",
      "mountain",
      "rocky",
      "grass",
      "hill",
      "covered",
      "hillside",
      "standing",
      "side",
      "group",
      "walking",
      "white",
      "man",
      "large",
      "snow",
      "grazing",
      "forest",
      "slope",
      "herd",
      "river",
      "giraffe",
      "field"
    ],
    "captions": [
      {
        "text": "a large waterfall over a rocky cliff",
        "confidence": 0.916458423253597
      }
    ]
  },
  "requestId": "ebf5a1bc-3ba2-4c56-99b4-bbd20ba28705",
  "metadata": {
    "height": 959,
    "width": 1280,
    "format": "Jpeg"
  }
}
```

## Clean up resources

When no longer needed, delete the file, and then uninstall the PHP5 `HTTP_Request2` package. To uninstall the package, do the following steps:

1. Open a command prompt window as an administrator.
2. Run the following command:

   ```console
   pear uninstall HTTP_Request2
   ```

3. After the package is successfully uninstalled, close the command prompt window.
