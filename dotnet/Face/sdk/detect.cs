using System;
using System.Collections.Generic;
using System.Linq;
/* Add a reference to the WindowsBase .NET Framework assembly.
* Note: Currently, that means this project must be created to
* target the .NET Framework and not .NET Core.
*/
using System.Windows;
// Install NuGet package Microsoft.Azure.CognitiveServices.Vision.Face.
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace ConsoleApp1
{
    class Program
    {
        static string SUBSCRIPTION_KEY = "PASTE_YOUR_FACE_SUBSCRIPTION_KEY_HERE";
        static string ENDPOINT = "PASTE_YOUR_FACE_ENDPOINT_HERE";

        async static void Quickstart()
        {
            IFaceClient faceClient = new FaceClient(new ApiKeyServiceClientCredentials(SUBSCRIPTION_KEY)) { Endpoint = ENDPOINT };

            var imageUrl = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/faces.jpg";

            // <basic1>
            IList<DetectedFace> faces = await faceClient.Face.DetectWithUrlAsync(url: imageUrl, returnFaceId: true, detectionModel: DetectionModel.Detection03);
            // </basic1>

            // <basic2>
            foreach (var face in faces)
            {
                string id = face.FaceId.ToString();
                FaceRectangle rect = face.FaceRectangle;
            }
            // </basic2>

            // <landmarks1>
            // Note DetectionModel.Detection02 cannot be used with returnFaceLandmarks.
            IList<DetectedFace> faces2 = await faceClient.Face.DetectWithUrlAsync(url: imageUrl, returnFaceId: true, returnFaceLandmarks: true, detectionModel: DetectionModel.Detection01);
            // </landmarks1>

            // <landmarks2>
            foreach (var face in faces2)
            {
                var landmarks = face.FaceLandmarks;

                double noseX = landmarks.NoseTip.X;
                double noseY = landmarks.NoseTip.Y;

                double leftPupilX = landmarks.PupilLeft.X;
                double leftPupilY = landmarks.PupilLeft.Y;

                double rightPupilX = landmarks.PupilRight.X;
                double rightPupilY = landmarks.PupilRight.Y;
                // </landmarks2>

                // <direction>
                var upperLipBottom = landmarks.UpperLipBottom;
                var underLipTop = landmarks.UnderLipTop;

                var centerOfMouth = new Point(
                    (upperLipBottom.X + underLipTop.X) / 2,
                    (upperLipBottom.Y + underLipTop.Y) / 2);

                var eyeLeftInner = landmarks.EyeLeftInner;
                var eyeRightInner = landmarks.EyeRightInner;

                var centerOfTwoEyes = new Point(
                    (eyeLeftInner.X + eyeRightInner.X) / 2,
                    (eyeLeftInner.Y + eyeRightInner.Y) / 2);

                Vector faceDirection = new Vector(
                    centerOfTwoEyes.X - centerOfMouth.X,
                    centerOfTwoEyes.Y - centerOfMouth.Y);
            }
            // </direction>

            // <attributes1>
            var requiredFaceAttributes = new FaceAttributeType?[] {
                FaceAttributeType.FacialHair,
                FaceAttributeType.HeadPose,
                FaceAttributeType.Glasses,
                FaceAttributeType.QualityForRecognition
            };
            // Note DetectionModel.Detection02 cannot be used with returnFaceAttributes.
            var faces3 = await faceClient.Face.DetectWithUrlAsync(url: imageUrl, returnFaceId: true, returnFaceAttributes: requiredFaceAttributes, detectionModel: DetectionModel.Detection01, recognitionModel: RecognitionModel.Recognition04);
            // </attributes1>

            // <attributes2>
            foreach (var face in faces3)
            {
                var attributes = face.FaceAttributes;
                var facialHair = attributes.FacialHair;
                var headPose = attributes.HeadPose;
                var glasses = attributes.Glasses;
                var qualityForRecognition = attributes.QualityForRecognition;
            }
            // </attributes2>
        }

        static void Main(string[] args)
        {
            Quickstart();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
