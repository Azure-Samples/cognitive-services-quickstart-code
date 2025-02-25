using System.Drawing;

using Azure;
using Azure.AI.Vision.Face;

namespace FaceQuickstart
{
    class Program
    {
        static string SubscriptionKey = "PASTE_YOUR_FACE_SUBSCRIPTION_KEY_HERE";
        static string Endpoint = "PASTE_YOUR_FACE_ENDPOINT_HERE";

        async static void Quickstart()
        {
            FaceClient faceClient = new FaceClient(new Uri(Endpoint), new AzureKeyCredential(SubscriptionKey));

            var imageUrl = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/faces.jpg";

            // <basic1>
            var response = await faceClient.DetectAsync(new Uri(imageUrl), FaceDetectionModel.Detection03, FaceRecognitionModel.Recognition04, returnFaceId: false);
            IReadOnlyList<FaceDetectionResult> faces = response.Value;
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
            var response2 = await faceClient.DetectAsync(new Uri(imageUrl), FaceDetectionModel.Detection03, FaceRecognitionModel.Recognition04, returnFaceId: false, returnFaceLandmarks: true);
            IReadOnlyList<FaceDetectionResult> faces2 = response2.Value;
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
                var upperLipBottom = landmarks.UpperLipBottom;
                var underLipTop = landmarks.UnderLipTop;

                var centerOfMouth = new Point(
                    (int)((upperLipBottom.X + underLipTop.X) / 2),
                    (int)((upperLipBottom.Y + underLipTop.Y) / 2));

                var eyeLeftInner = landmarks.EyeLeftInner;
                var eyeRightInner = landmarks.EyeRightInner;

                var centerOfTwoEyes = new Point(
                    (int)((eyeLeftInner.X + eyeRightInner.X) / 2),
                    (int)((eyeLeftInner.Y + eyeRightInner.Y) / 2));

                // </landmarks2>

                // <direction>
                var faceDirectionVectorX = centerOfTwoEyes.X - centerOfMouth.X;
                var faceDirectionVectorY = centerOfTwoEyes.Y - centerOfMouth.Y;
            }
            // </direction>

            // <attributes1>
            var requiredFaceAttributes = new FaceAttributeType[] {
                FaceAttributeType.Detection03.Blur,
                FaceAttributeType.Detection03.HeadPose,
                FaceAttributeType.Detection03.Mask,
                FaceAttributeType.Recognition04.QualityForRecognition
            };
            // Note DetectionModel.Detection02 cannot be used with returnFaceAttributes.
            var response3 = await faceClient.DetectAsync(new Uri(imageUrl), FaceDetectionModel.Detection03, FaceRecognitionModel.Recognition04, returnFaceId: false, returnFaceAttributes: requiredFaceAttributes);
            IReadOnlyList<FaceDetectionResult> faces3 = response3.Value;
            // </attributes1>

            // <attributes2>
            foreach (var face in faces3)
            {
                var attributes = face.FaceAttributes;
                var blur = attributes.Blur;
                var headPose = attributes.HeadPose;
                var mask = attributes.Mask;
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