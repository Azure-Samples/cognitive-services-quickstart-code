using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
// Install NuGet package Microsoft.Azure.CognitiveServices.Vision.Face.
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace ConsoleApp1
{
    class Program
    {
        async static void Quickstart()
        {
            // <authenticate>
            string SUBSCRIPTION_KEY = Environment.GetEnvironmentVariable("FACE_SUBSCRIPTION_KEY");
            string ENDPOINT = Environment.GetEnvironmentVariable("FACE_ENDPOINT");

            IFaceClient faceClient = new FaceClient(new ApiKeyServiceClientCredentials(SUBSCRIPTION_KEY)) { Endpoint = ENDPOINT };
            // </authenticate>

            // <person>
            // Create an empty PersonGroup
            string personGroupId = "myfriends";
            await faceClient.PersonGroup.CreateAsync(personGroupId, "My Friends");

            // Define Anna
            Person friend1 = await faceClient.PersonGroupPerson.CreateAsync(
                // Id of the PersonGroup that the person belonged to
                personGroupId,
                // Name of the person
                "Anna"
            );

            // Define Bill and Clare in the same way
            // </person>

            // <add_face>
            // Directory contains image files of Anna
            const string friend1ImageDir = @"D:\Pictures\MyFriends\Anna\";

            foreach (string imagePath in Directory.GetFiles(friend1ImageDir, "*.jpg"))
            {
                using (Stream s = File.OpenRead(imagePath))
                {
                    // Detect faces in the image and add to Anna
                    await faceClient.PersonGroupPerson.AddFaceFromStreamAsync(
                        personGroupId:personGroupId, personId:friend1.PersonId, image:s, detectionModel:DetectionModel.Detection02);
                }
            }
            // Do the same for Bill and Clare
            // </add_face>

            // <train1>
            await faceClient.PersonGroup.TrainAsync(personGroupId);
            // </train1>

            // <train2>
            TrainingStatus trainingStatus = null;
            while (true)
            {
                trainingStatus = await faceClient.PersonGroup.GetTrainingStatusAsync(personGroupId);

                if (trainingStatus.Status != TrainingStatusType.Running)
                {
                    break;
                }

                await Task.Delay(1000);
            }
            // </train2>

            // <main>
            string testImageFile = @"D:\Pictures\test_img1.jpg";

            using (Stream s = File.OpenRead(testImageFile))
            {
                var faces = await faceClient.Face.DetectWithStreamAsync(image:s, detectionModel:DetectionModel.Detection02);
                Guid?[] faceIds = faces.Select(face => face.FaceId).ToArray();

                var results = await faceClient.Face.IdentifyAsync(faceIds:faceIds, personGroupId:personGroupId);
                foreach (var identifyResult in results)
                {
                    Console.WriteLine("Result of face: {0}", identifyResult.FaceId);
                    if (identifyResult.Candidates.Count == 0)
                    {
                        Console.WriteLine("No one identified");
                    }
                    else
                    {
                        // Get top 1 among all candidates returned
                        var candidateId = identifyResult.Candidates[0].PersonId;
                        var person = await faceClient.PersonGroupPerson.GetAsync(personGroupId:personGroupId, personId:candidateId);
                        Console.WriteLine("Identified as {0}", person.Name);
                    }
                }
            }
            // </main>
        }

        static void Main(string[] args)
        {
            Quickstart();
        }
    }
}
