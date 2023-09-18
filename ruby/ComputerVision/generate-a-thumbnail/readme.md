
# Quickstart: Generate a thumbnail using the Computer Vision REST API and Ruby

In this quickstart, you generate a thumbnail from an image by using Computer Vision's REST API. With the [Get Thumbnail](https://westcentralus.dev.cognitive.microsoft.com/docs/services/5adf991815e1060e6355ad44/operations/56f91f2e778daf14a499e1fb) method, you can generate a thumbnail of an image. You specify the height and width, which can differ from the aspect ratio of the input image. Computer Vision uses smart cropping to intelligently identify the area of interest and generate cropping coordinates based on that region.

If you don't have an Azure subscription, create a [free account](https://azure.microsoft.com/free/ai/?ref=microsoft.com&utm_source=microsoft.com&utm_medium=docs&utm_campaign=cognitive-services) before you begin.

## Prerequisites

- You must have [Ruby](https://www.ruby-lang.org/en/downloads/) 2.4.x or later installed.
- You must have a key for Computer Vision. You can visit [the Microsoft Cognitive Services Web site](https://azure.microsoft.com/free/cognitive-services/), create a new Azure account, and try Cognitive Services for free. Or, follow the instructions in [Create a Cognitive Services account](https://docs.microsoft.com/azure/cognitive-services/cognitive-services-apis-create-account) to subscribe to Computer Vision and get your key.

## Create and run the sample

To create and run the sample, do the following steps:

1. Copy the following code into a text editor.
1. Make the following changes in code where needed:
    1. Replace `<key>` with your key.
    1. Replace `https://westcentralus.api.cognitive.microsoft.com/vision/v2.0/analyze` with the endpoint URL for the [Get Thumbnail](https://westcentralus.dev.cognitive.microsoft.com/docs/services/5adf991815e1060e6355ad44/operations/56f91f2e778daf14a499e1fb) method in the Azure region where you obtained your keys, if necessary.
    1. Optionally, replace `https://upload.wikimedia.org/wikipedia/commons/thumb/5/56/Shorkie_Poo_Puppy.jpg/1280px-Shorkie_Poo_Puppy.jpg\` with the URL of a different image for which you want to generate a thumbnail.
1. Save the code as a file with an `.rb` extension. For example, `get-thumbnail.rb`.
1. Open a command prompt window.
1. At the prompt, use the `ruby` command to run the sample. For example, `ruby get-thumbnail.rb`.

```ruby
require 'net/http'

# You must use the same location in your REST call as you used to get your
# keys. For example, if you got your keys from westus,
# replace "westcentralus" in the URL below with "westus".
uri = URI('https://westcentralus.api.cognitive.microsoft.com/vision/v2.0/generateThumbnail')
uri.query = URI.encode_www_form({
    # Request parameters
    'width' => '100',
    'height' => '100',
    'smartCropping' => 'true'
})

request = Net::HTTP::Post.new(uri.request_uri)

# Request headers
# Replace <key> with your valid key.
request['Ocp-Apim-Subscription-Key'] = '<key>'
request['Content-Type'] = 'application/json'

request.body =
    "{\"url\": \"https://upload.wikimedia.org/wikipedia/commons/thumb/5/56/" +
    "Shorkie_Poo_Puppy.jpg/1280px-Shorkie_Poo_Puppy.jpg\"}"

response = Net::HTTP.start(uri.host, uri.port, :use_ssl => uri.scheme == 'https') do |http|
    http.request(request)
end

#puts response.body
```

## Examine the response

A successful response is returned as binary data, which represents the image data for the thumbnail. If the request fails, the response is displayed in the console window. The response for the failed request contains an error code and a message to help determine what went wrong.
