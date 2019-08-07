# 1. BUILD IMAGE
# $ docker build --no-cache -t my-py3-image .
#
# 2. RUN CODE IMMEDIATELY THEN STOP CONTAINER
#
# WINDOWS BASH COMMAND 
# $ winpty docker run -it -e LUIS_SUBSCRIPTION_KEY='421a7b3ec59f4645a46b7dec7c18414f' -e LUIS_REGION='westus' --mount src="/c/Users/<YOUR-USER-NAME/repos/cognitive-services-quickstart-code/python/LUIS/",dst=/usr/src/app,type=bind --rm my-py3-image 
#
# NON-WINDOWS
# $ docker run -it --rm --name my-py3 my-py3 tail -f /dev/null

# winpty docker exec -it 08ca1a3b1a8d bash

FROM python:3

WORKDIR /usr/src/app

RUN pip install azure-cognitiveservices-language-luis
COPY . .

CMD [ "python", "application_quickstart.py" ]