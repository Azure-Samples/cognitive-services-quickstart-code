using System;
/* See:
 * https://github.com/Microsoft/Cognitive-Samples-VideoFrameAnalysis
 * Compile and add reference to VideoFrameAnalyzer.dll.
 * Install NuGet package OpenCVSharp.
 */
using VideoFrameAnalyzer;
// Install NuGet package Microsoft.Azure.CognitiveServices.Vision.Face.
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VideoFrameConsoleApplication
{
    class Program
    {
        static string SUBSCRIPTION_KEY = Environment.GetEnvironmentVariable("FACE_SUBSCRIPTION_KEY");
        static string ENDPOINT = Environment.GetEnvironmentVariable("FACE_ENDPOINT");

        static void Main(string[] args)
        {
            IFaceClient client = new FaceClient(new ApiKeyServiceClientCredentials(SUBSCRIPTION_KEY)) { Endpoint = ENDPOINT };

            // Define this in Main so it is closed over the client.
            async Task<DetectedFace[]> Detect(VideoFrame frame)
            {
                return (DetectedFace[])await client.Face.DetectWithStreamAsync(frame.Image.ToMemoryStream(".jpg"), detectionModel:DetectionModel.Detection03);
            }

            // Create grabber, with analysis type Face[]. 
            FrameGrabber<DetectedFace[]> grabber = new FrameGrabber<DetectedFace[]>();

            // Set up our Face API call.
            grabber.AnalysisFunction = Detect;

            // Set up a listener for when we receive a new result from an API call. 
            grabber.NewResultAvailable += (s, e) =>
            {
                if (e.Analysis != null)
                    Console.WriteLine("New result received for frame acquired at {0}. {1} faces detected", e.Frame.Metadata.Timestamp, e.Analysis.Length);
            };

            // Tell grabber to call the Face API every 3 seconds.
            grabber.TriggerAnalysisOnInterval(TimeSpan.FromMilliseconds(3000));

            // Start running.
            grabber.StartProcessingCameraAsync().Wait();

            // Wait for keypress to stop
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();

            // Stop, blocking until done.
            grabber.StopProcessingAsync().Wait();
        }
    }
}