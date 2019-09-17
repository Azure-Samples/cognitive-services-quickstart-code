
# FaceAPI Quickstart for Java

This quickstart contains many examples that can be run all at once.
  - Detect Faces: detect a face or faces in an image and URL
  - Find Similar: find face similar to the single-faced image in the group image
  - Verify: with 2 images, check if they are the same person or different people
  - Identify: with grouped images of the same person (in a person group), use group to find similar faces in another image
  - Group Faces: groups images into sub-groups based on same person images
  - Face Lists: adds images to a face list, then retrieves them
  - Delete: deletes the person group and face list to enable repeated testing
  
If cutting / pasting specific examples only, for instance say you only want Identify, be sure to include any helper methods it uses. For instance, Identify uses the Detect Faces example as a helper method.
 
## Prerequisites
  - Create a Face subscription in the Azure portal.
  - Download the faceAPI library (ms-azure-cs-faceapi.jar) from this repo.
  - Download the dependency jars (choose one of these ways):
        - Option 1: obtain jars in the dependencies.txt file from Maven (mvnrepository.com)
        - Option 2: download the faceapi-dependencies.jar file from this repo, open (expand) to find all the dependency jars.
                    Refer to dependencies.txt for URLs of the libraries to update if needed.
  - Create a **lib** folder in the root directory of your project, then add the FaceAPI library and dependency jars.
  - Add your Face subscription key to your environment variables with the name FACE_SUBSCRIPTION_KEY.
  - Replace the "REGION" variable in the authenticate section with your region. <br>
    The "westus" is used, otherwise, as the default. NOTE: this quickstart does **not** need your Face endpoint.
 
## Compile and run
Enter the following at a command prompt from your root folder: <br>
  `javac FaceQuickstart.java -cp .;lib\*` <br>
  `java -cp .;lib\* FaceQuickstart`

NOTE: If you run this sample with JRE 9+, you may encounter the following issue: 
https://github.com/Azure/autorest-clientruntime-for-java/issues/569 which results in the following output: <br>
`WARNING: An illegal reflective access operation has occurred` ... (plus several more warnings) <br>
This should not prevent the quickstart from running correctly, so this can be ignored.
 
## References
  - Face Documentation: https://docs.microsoft.com/en-us/azure/cognitive-services/face/
  - Face Java SDK: https://docs.microsoft.com/en-us/java/api/overview/azure/cognitiveservices/client/faceapi?view=azure-java-stable
  - Face SDK source code: https://github.com/Azure/azure-sdk-for-java/tree/master/sdk/cognitiveservices/ms-azure-cs-faceapi
  - API Reference: https://docs.microsoft.com/en-us/azure/cognitive-services/face/apireference
 
