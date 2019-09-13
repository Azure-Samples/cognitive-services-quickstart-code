
# FaceAPI Quickstart for Java

This quickstart contains many examples that can be run all at once.
  - Detect Faces: detect a face or faces in an image and URL
  - Find Similar: find face similar to the single-faced image in the group image
  - Identify: uses similar images of a person, grouped together, to learn how to identify that person in other images
  
If cutting / pasting specific examples only, for instance say you only want Identify, be sure to include any helper methods it uses. For instance, Identify uses the Detect Faces example as a helper method.
 
## Prerequisites
  - Create a **lib** folder in the root directory of your project, then add the jars from dependencies.txt list.
  - Download the ms-azure-cs-faceapi.jar in this repo and add to your lib folder.
  - Add your Face subscription key to your environment variables with the name FACE_SUBSCRIPTION_KEY.
  - Replace the "myRegion" variable in the authenticate section with your region. <br>
    The "westus" is used, otherwise, as the default. NOTE: this quickstart does **not** need your Face endpoint.
 
## Compile and run
Enter the following at a command prompt from your root folder: <br>
  javac FaceQuickstart.java -cp .;lib\* <br>
  java -cp .;lib\* FaceQuickstart

NOTE: If you run this sample with JRE 9+, you may encounter the following issue: 
https://github.com/Azure/autorest-clientruntime-for-java/issues/569 which results in the following output: <br>
`WARNING: An illegal reflective access operation has occurred` ... (plus several more warnings) <br>
This should not prevent the quickstart from running correctly, so this can be ignored.
 
## References
  - Face Documentation: https://docs.microsoft.com/en-us/azure/cognitive-services/face/
  - Face Java SDK: https://docs.microsoft.com/en-us/java/api/overview/azure/cognitiveservices/client/faceapi?view=azure-java-stable
  - API Reference: https://docs.microsoft.com/en-us/azure/cognitive-services/face/apireference
 
