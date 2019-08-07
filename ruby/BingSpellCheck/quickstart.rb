# encoding: utf-8

# This sample does the following tasks:
# - Gets spell check suggestions.

# To run this sample, please install the required packages by running the following commands at an Administrator command prompt.
# gem install azure_cognitiveservices_spellcheck
#
# For more information about how to use the Azure SDK for Ruby, see:
# https://azure.microsoft.com/en-us/resources/samples/cognitive-services-ruby-sdk-samples/

require 'azure_cognitiveservices_spellcheck'

if (!ENV["SPELLCHECK_SUBSCRIPTION_KEY"])
	raise "Please set/export the following environment variable: SPELLCHECK_SUBSCRIPTION_KEY"
else
	subscription_key = ENV["SPELLCHECK_SUBSCRIPTION_KEY"]
end
if (!ENV["SPELLCHECK_ENDPOINT"])
	raise "Please set/export the following environment variable: SPELLCHECK_ENDPOINT"
else
	endpoint = ENV["SPELLCHECK_ENDPOINT"]
end

client = Azure::CognitiveServices::SpellCheck::V1_0::SpellCheckClient.new()
client.credentials = MsRestAzure::CognitiveServicesCredentials.new(subscription_key)
client.endpoint = endpoint

result = client.spell_checker("bill gtaes")
puts "Results:"
result.flagged_tokens.each do |token|
	puts "Offset: #{token.offset}\nToken: #{token.token }\nType: #{token.type}"
	token.suggestions.each do |suggestion|
		puts "Suggestion: " + suggestion.suggestion
		puts "Score: #{suggestion.score}"
	end
	puts ""
end
