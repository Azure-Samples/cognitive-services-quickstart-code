## Face Quickstart

The FaceQuickstart backs the code snippets represented in the [Face API Documentation](https://docs.microsoft.com/en-us/azure/cognitive-services/face/). 

The samples included in this Go file are:

* **Authenticate**: authenticates using your Azure portal subscription key and endpoint to authorize a client.
* **Detect faces in two images**: detects faces from input images using the recognition model 01 for the purpose of extracting facial features. 
* **Find a similar face**: using a query image, it finds a similar face in a list of detected faces from other very similar face images.
* **Verify**: compares one face with another to check if they are from the same person.
* **PersonGroupOperations**: creates a person group from similar-faced images, then uses a group image for identifying who in the image is from an established person group.
* **Snapshot**: takes a snapshot of an existing person group in one region and copies it to another region.
* **Delete**: deletes a person group from a specific region.

### Prerequisites
* Your favorite IDE or text editor, such as Visual Studio Code
* Go 1.12+
* Install Face SDK in console: <br>
`go get github.com/Azure/azure-sdk-for-go/services/cognitiveservices/v1.0/face`
* Create an images folder in your root folder and copy images from here:<br>
  https://github.com/Azure-Samples/cognitive-services-sample-data-files/tree/master/Face/images

### How to run
* This sample can be run in its entirety from any text editor in the command line or from an IDE.
* Another way to use this sample is to view the GoQuickstart.go file and find all of the above samples. 
* Dependencies: 
    - There are 3 different **Authenticate** sections. The 1st is use by Detect Face and Find Similar, the 2nd one is special for a Person Group, and the 3rd is special for a Person Group Person.
    - The **Snapshot** needs a person group ID to be executed, so it uses the one created from the **Person Group Operations** sample. 
    - The **Delete Person Group** uses a person group ID, so it uses the one used in the **Snapshot**. For testing, it's helpful to delete the person group you create each time you run the samples, because your Azure account for Face will not accept a new person group with the same name to be created.

### References
* [Face recognition concepts](https://docs.microsoft.com/en-us/azure/cognitive-services/face/concepts/face-recognition)
* [Face detection and attributes](https://docs.microsoft.com/en-us/azure/cognitive-services/face/concepts/face-detection)
* [API Reference](https://docs.microsoft.com/en-us/azure/cognitive-services/face/apireference)
* [Go SDK](https://godoc.org/github.com/Azure/azure-sdk-for-go/services/cognitiveservices/v1.0/face)
