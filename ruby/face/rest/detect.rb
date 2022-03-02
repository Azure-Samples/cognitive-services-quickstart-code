require 'net/http'

subscription_key = 'PASTE_YOUR_FACE_SUBSCRIPTION_KEY_HERE'
endpoint = 'PASTE_YOUR_FACE_ENDPOINT_HERE'

uri = URI(endpoint + '/face/v1.0/detect')
uri.query = URI.encode_www_form({
    # Request parameters
	'detectionModel' => 'detection_03',
    'returnFaceId' => 'true'
})

request = Net::HTTP::Post.new(uri.request_uri)

# Request headers
request['Ocp-Apim-Subscription-Key'] = subscription_key
request['Content-Type'] = 'application/json'

imageUri = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/faces.jpg"
request.body = "{\"url\": \"" + imageUri + "\"}"

response = Net::HTTP.start(uri.host, uri.port, :use_ssl => uri.scheme == 'https') do |http|
    http.request(request)
end

puts response.body
