require 'net/http'

key_var_name = 'FACE_SUBSCRIPTION_KEY'
if nil == ENV[key_var_name]
    raise Exception("Please set/export the environment variable: #{key_var_name}\n")
end
subscription_key = ENV[key_var_name]

endpoint_var_name = 'FACE_ENDPOINT'
if nil == ENV[endpoint_var_name]
    raise Exception("Please set/export the environment variable: #{endpoint_var_name}\n")
end
endpoint = ENV[endpoint_var_name]

uri = URI(endpoint + '/face/v1.0/detect')
uri.query = URI.encode_www_form({
    # Request parameters
	'detectionModel' => 'detection_02',
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
