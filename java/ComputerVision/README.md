---
services: cognitive-services, computer-vision
platforms: java
author: wiazur
---

# Computer Vision SDK Quickstart

This quickstart uses image classification and object detection on an image with the Computer Vision Cognitive Service. It will retrieve and print information (include text) from the image. Maven is used.

## Getting Started

### Prerequisites
- If you don't have a Microsoft Azure subscription you can get a FREE trial account [here](http://go.microsoft.com/fwlink/?LinkId=330212).
- Get an [Azure Computer Vision](https://azure.microsoft.com/en-us/services/cognitive-services/computer-vision/) account to get your subscription key and endpoint.
- Add COMPUTER_VISION_SUBSCRIPTION_KEY and COMPUTER_VISION_ENDPOINT to your environment variables with your key and endpoint as values.
- Create a 'resources' folder in your 'src/main/' folder. 
- Add images **landmark.jpg** and **printed_text.jpg** to it, downloaded locally from here:
  https://github.com/Azure-Samples/cognitive-services-sample-data-files/tree/master/ComputerVision/Images

### Clone and run

Execute from the command line:

1. `git clone https://github.com/Azure-Samples/cognitive-services-quickstart-code.git`
1. `cd cognitive-services-quickstart-code/java/ComputerVision`
1. `mvn compile exec:java -Dexec.cleanupDaemonThreads=false`

## More information 

- [Build and deploy Java apps on Azure](http://azure.com/java)
- The [Computer Vision documentation](https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/index)

---

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
