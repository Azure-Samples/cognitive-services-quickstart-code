# <imagemod>
curl -v -X POST "https://westus.api.cognitive.microsoft.com/contentmoderator/moderate/v1.0/ProcessImage/Evaluate?CacheImage={boolean}" 
-H "Content-Type: application/json"
-H "Ocp-Apim-Subscription-Key: {subscription key}" 
--data-ascii "{\"DataRepresentation\":\"URL\", \"Value\":\"https://moderatorsampleimages.blob.core.windows.net/samples/sample.jpg\"}"
# </imagemod>

# <textmod>
curl -v -X POST "https://westus.api.cognitive.microsoft.com/contentmoderator/moderate/v1.0/ProcessText/Screen?autocorrect=True&PII=True&classify=True&language={string}"
-H "Content-Type: text/plain"
-H "Ocp-Apim-Subscription-Key: {subscription key}"
--data-ascii "Is this a crap email abcdef@abcd.com, phone: 6657789887, IP: 255.255.255.255, 1 Microsoft Way, Redmond, WA 98052" 
# </textmod>