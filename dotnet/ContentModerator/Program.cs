// <snippet_using>
using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.Azure.CognitiveServices.ContentModerator.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
// </snippet_using>

/*
 * Content Moderator SDK Quickstart
 * 
 * Examples included:
 * - Image moderation - provide URL or image files for moderation
 * - Text moderation - provide text in a file for moderations
 * - Create human reviews for images - create a human review to be processed on the Content Moderator website.
 * 
 * Prerequisites:
 *	- Visual Studio 2019 (or 2017, but this is a .NET Core console app, not .NET Framework)
 *	- Create a C# console app in Visual Studio, then cut/paste this Program.cs file over your own (make sure namespaces match)
 *	- NuGet packages needed: Microsoft.Azure.CognitiveServices.ContentModerator & Newtonsoft.Json
 *	- Add the TextFile.txt and ImageFiles.txt included with this example to your bin/Debug/netcoreapp2.2 folder of your project.
 *	
 * How to Run:
 *  - Select start in Visual Studio
 *  - Image and text moderation results will appear, then the human reviews for images will start
 *  - Wait for the console command to tell you to perform a human review on https://{YOUR REGION}.contentmoderator.cognitive.microsoft.com
 *  - Perform the review on the website under Review-->Image. Select the "a" or "r" to test choosing the adult or racy markers, then choose "next".
 *  - Go back to the console and press enter to process the reviews.
 *  - The JSON returned will have review details in the console.
 *  - Check your output files for the image and text reviews. These files get created, only after the quickstart has run. 
 *    If testing multiple times, the output files will be overwritten with the latest moderation results.
 *    
 * References:
 *  - .NET SDK: https://docs.microsoft.com/en-us/dotnet/api/overview/azure/cognitiveservices/client/contentmoderator?view=azure-dotnet
 *  - API (testing console): https://docs.microsoft.com/en-us/azure/cognitive-services/content-moderator/api-reference
 *  - Content Moderator documentation: https://docs.microsoft.com/en-us/azure/cognitive-services/content-moderator/
 */

namespace ContentModeratorQuickstart
{
	class Program
	{
		// AUTHENTICATION - ALL EXAMPLES
		// <snippet_creds>
		// Your Content Moderator subscription key is found in your Azure portal resource on the 'Keys' page.
		private static readonly string SubscriptionKey = "CONTENT_MODERATOR_SUBSCRIPTION_KEY";
		// Base endpoint URL. Found on 'Overview' page in Azure resource. For example: https://westus.api.cognitive.microsoft.com
		private static readonly string Endpoint = "CONTENT_MODERATOR_ENDPOINT";
		// </snippet_creds>

		// <snippet_image_vars>
		// IMAGE MODERATION
		//The name of the file that contains the image URLs to evaluate.
		private static readonly string ImageUrlFile = "ImageFiles.txt";
		// The name of the file to contain the output from the evaluation.
		private static string ImageOutputFile = "ImageModerationOutput.json";
		// </snippet_image_vars>

		// <snippet_text_vars>
		// TEXT MODERATION
		// Name of the file that contains text
		private static readonly string TextFile = "TextFile.txt";
		// The name of the file to contain the output from the evaluation.
		private static string TextOutputFile = "TextModerationOutput.txt";
		// </snippet_text_vars>

		// CREATE HUMAN REVIEWS FOR IMAGES
		// <snippet_review_urls>
		// The list of URLs of the images to create review jobs for.
		private static readonly string[] IMAGE_URLS_FOR_REVIEW = new string[] { "https://moderatorsampleimages.blob.core.windows.net/samples/sample5.png" };
		// </snippet_review_urls>
		// <snippet_review_vars>
		// The name of the team to assign the review to. Must be the team name used to create your Content Moderator website account. 
		// If you do not yet have an account, follow this: https://docs.microsoft.com/en-us/azure/cognitive-services/content-moderator/quick-start
		// Select the gear symbol (settings)-->Credentials to retrieve it. Your team name is the Id associated with your subscription.
		private static readonly string TEAM_NAME = "CONTENT_MODERATOR_TEAM_NAME";
		// The callback endpoint for completed human reviews.
		// For example: https://westus.api.cognitive.microsoft.com/contentmoderator/review/v1.0
		// As reviewers complete reviews, results are sent using an HTTP POST request.
		private static readonly string ReviewsEndpoint = "CONTENT_MODERATOR_REVIEWS_ENDPOINT";
		// </snippet_review_vars>

		static void Main(string[] args)
		{
			// CLIENTS - each API call needs its own client
			// <snippet_client>
			// Create an image review client
			ContentModeratorClient clientImage = Authenticate(SubscriptionKey, Endpoint);
			// Create a text review client
			ContentModeratorClient clientText = Authenticate(SubscriptionKey, Endpoint);
			// Create a human reviews client
			ContentModeratorClient clientReviews = Authenticate(SubscriptionKey, Endpoint);
			// </snippet_client>

			// <snippet_imagemod_call>
			// Moderate images from list of image URLs
			ModerateImages(clientImage, ImageUrlFile, ImageOutputFile);
			// </snippet_imagemod_call>

			// <snippet_textmod_call>
			// Moderate text from text in a file
			ModerateText(clientText, TextFile, TextOutputFile);
			// </snippet_textmod_call>

			// <snippet_review_call>
			// Create image reviews for human reviewers
			CreateReviews(clientReviews, IMAGE_URLS_FOR_REVIEW, TEAM_NAME, ReviewsEndpoint);
			// </snippet_review_call>

			Console.WriteLine();
			Console.WriteLine("End of the quickstart.");
		}
		/*
		 * END - MAIN
		 */

		/*
		 * AUTHENTICATE
		 * Creates a new client with a validated subscription key and endpoint.
		 */
		// <snippet_auth>
		public static ContentModeratorClient Authenticate(string key, string endpoint)
		{
			ContentModeratorClient client = new ContentModeratorClient(new ApiKeyServiceClientCredentials(key));
			client.Endpoint = endpoint;

			return client;
		}
		// </snippet_auth>

		// <snippet_imagemod_iterate>
		/*
		 * IMAGE MODERATION
		 * This example moderates images from URLs.
		 */
		public static void ModerateImages(ContentModeratorClient client, string urlFile, string outputFile)
		{
			Console.WriteLine("--------------------------------------------------------------");
			Console.WriteLine();
			Console.WriteLine("IMAGE MODERATION");
			Console.WriteLine();
			// Create an object to store the image moderation results.
			List<EvaluationData> evaluationData = new List<EvaluationData>();

			using (client)
			{
				// Read image URLs from the input file and evaluate each one.
				using (StreamReader inputReader = new StreamReader(urlFile))
				{
					while (!inputReader.EndOfStream)
					{
						string line = inputReader.ReadLine().Trim();
						if (line != String.Empty)
						{
							Console.WriteLine("Evaluating {0}...", Path.GetFileName(line));
							var imageUrl = new BodyModel("URL", line.Trim());
							// </snippet_imagemod_iterate>
							// <snippet_imagemod_analyze>
							var imageData = new EvaluationData
							{
								ImageUrl = imageUrl.Value,

								// Evaluate for adult and racy content.
								ImageModeration =
								client.ImageModeration.EvaluateUrlInput("application/json", imageUrl, true)
							};
							Thread.Sleep(1000);

							// Detect and extract text.
							imageData.TextDetection =
								client.ImageModeration.OCRUrlInput("eng", "application/json", imageUrl, true);
							Thread.Sleep(1000);

							// Detect faces.
							imageData.FaceDetection =
								client.ImageModeration.FindFacesUrlInput("application/json", imageUrl, true);
							Thread.Sleep(1000);

							// Add results to Evaluation object
							evaluationData.Add(imageData);
						}
					}
				}
				// </snippet_imagemod_analyze>
				// <snippet_imagemod_save>
				// Save the moderation results to a file.
				using (StreamWriter outputWriter = new StreamWriter(outputFile, false))
				{
					outputWriter.WriteLine(JsonConvert.SerializeObject(
						evaluationData, Formatting.Indented));

					outputWriter.Flush();
					outputWriter.Close();
				}
				Console.WriteLine();
				Console.WriteLine("Image moderation results written to output file: " + outputFile);
				Console.WriteLine();
			}
		}
		// </snippet_imagemod_save>

		// <snippet_dataclass>
		// Contains the image moderation results for an image, 
		// including text and face detection results.
		public class EvaluationData
		{
			// The URL of the evaluated image.
			public string ImageUrl;

			// The image moderation results.
			public Evaluate ImageModeration;

			// The text detection results.
			public OCR TextDetection;

			// The face detection results;
			public FoundFaces FaceDetection;
		}
		// </snippet_dataclass>
		/*
		 * END - IMAGE MODERATION
		 */

		// <snippet_textmod>
		/*
		 * TEXT MODERATION
		 * This example moderates text from file.
		 */
		public static void ModerateText(ContentModeratorClient client, string inputFile, string outputFile)
		{
			Console.WriteLine("--------------------------------------------------------------");
			Console.WriteLine();
			Console.WriteLine("TEXT MODERATION");
			Console.WriteLine();
			// Load the input text.
			string text = File.ReadAllText(inputFile);

			// Remove carriage returns
			text = text.Replace(Environment.NewLine, " ");
			// Convert string to a byte[], then into a stream (for parameter in ScreenText()).
			byte[] textBytes = Encoding.UTF8.GetBytes(text);
			MemoryStream stream = new MemoryStream(textBytes);

			Console.WriteLine("Screening {0}...", inputFile);
			// Format text

			// Save the moderation results to a file.
			using (StreamWriter outputWriter = new StreamWriter(outputFile, false))
			{
				using (client)
				{
					// Screen the input text: check for profanity, classify the text into three categories,
					// do autocorrect text, and check for personally identifying information (PII)
					outputWriter.WriteLine("Autocorrect typos, check for matching terms, PII, and classify.");

					// Moderate the text
					var screenResult = client.TextModeration.ScreenText("text/plain", stream, "eng", true, true, null, true);
					outputWriter.WriteLine(JsonConvert.SerializeObject(screenResult, Formatting.Indented));
				}

				outputWriter.Flush();
				outputWriter.Close();
			}

			Console.WriteLine("Results written to {0}", outputFile);
			Console.WriteLine();
		}
		// </snippet_textmod>
		/*
		 * END - TEXT MODERATION
		 */

		/*
		 * CREATE HUMAN IMAGE REVIEWS
		 * This example shows how to create image reviews for humans to review on the Content Moderator website.
		 * Results of the review (once manually performed on the website) are then returned and written to OutputLog.txt.
		 * 
		 * Prerequisistes: 
		 * 1. In your Content Moderator resource go to Resource Management -> Properties, then copy your Resource ID. 
		 * 2. Go to the Content Moderator website.
		 * 3. Click the gear sign (Settings) and go to "Credentials". 
		 * 4. Under "Whitelisted Resource Id(s)" paste your Resource ID, then select the "+" button to add it.
		 *    This enables the website to receive programmatic reviews from your Content Moderator resource in Azure.
		 * 
		 * For more information about the steps:
		 * https://docs.microsoft.com/en-us/azure/cognitive-services/content-moderator/moderation-reviews-quickstart-dotnet
		 * Use your Azure account with the review APIs:
		 * https://docs.microsoft.com/en-us/azure/cognitive-services/content-moderator/review-tool-user-guide/configure
		 */
		// <snippet_review_item>
		// Associates the review ID (assigned by the service) to the internal.
		public class ReviewItem
		{
			// The media type for the item to review. 
			public string Type;
			// The URL of the item to review.
			public string Url;
			// The internal content ID for the item to review.
			public string ContentId;
			// The ID that the service assigned to the review.
			public string ReviewId;
		}
		// </snippet_review_item>

		// <snippet_createreview_fields>
		// Create the reviews using the fixed list of images.
		private static void CreateReviews(ContentModeratorClient client, string[] ImageUrls, string teamName, string endpoint)
		{
			Console.WriteLine("--------------------------------------------------------------");
			Console.WriteLine();
			Console.WriteLine("CREATE HUMAN IMAGE REVIEWS");

			// The minimum amount of time, in milliseconds, to wait between calls to the Image List API.
			const int throttleRate = 2000;
			// The number of seconds to delay after a review has finished before getting the review results from the server.
			const int latencyDelay = 45;

			// The name of the log file to create. Relative paths are relative to the execution directory.
			const string OutputFile = "OutputLog.txt";

			// The optional name of the subteam to assign the review to. Not used for this example.
			const string Subteam = null;

			// The media type for the item to review. Valid values are "image", "text", and "video".
			const string MediaType = "image";

			// The metadata key to initially add to each review item. This is short for 'score'.
			// It will enable the keys to be 'a' (adult) and 'r' (racy) in the response,
			// with a value of true or false if the human reviewer marked them as adult and/or racy.
			const string MetadataKey = "sc";
			// The metadata value to initially add to each review item.
			const string MetadataValue = "true";

			// A static reference to the text writer to use for logging.
			TextWriter writer;

			// The cached review information, associating a local content ID to the created review ID for each item.
			List<ReviewItem> reviewItems = new List<ReviewItem>();
			// </snippet_createreview_fields>

			// <snippet_createreview_create>
			using (TextWriter outputWriter = new StreamWriter(OutputFile, false))
			{
				writer = outputWriter;
				WriteLine(writer, null, true);
				WriteLine(writer, "Creating reviews for the following images:", true);

				// Create the structure to hold the request body information.
				List<CreateReviewBodyItem> requestInfo = new List<CreateReviewBodyItem>();

				// Create some standard metadata to add to each item.
				List<CreateReviewBodyItemMetadataItem> metadata =
					new List<CreateReviewBodyItemMetadataItem>(new CreateReviewBodyItemMetadataItem[]
					{ new CreateReviewBodyItemMetadataItem(MetadataKey, MetadataValue) });

				// Populate the request body information and the initial cached review information.
				for (int i = 0; i < ImageUrls.Length; i++)
				{
					// Cache the local information with which to create the review.
					var itemInfo = new ReviewItem()
					{
						Type = MediaType,
						ContentId = i.ToString(),
						Url = ImageUrls[i],
						ReviewId = null
					};

					WriteLine(writer, $" {Path.GetFileName(itemInfo.Url)} with id = {itemInfo.ContentId}.", true);

					// Add the item informaton to the request information.
					requestInfo.Add(new CreateReviewBodyItem(itemInfo.Type, itemInfo.Url, itemInfo.ContentId, endpoint, metadata));

					// Cache the review creation information.
					reviewItems.Add(itemInfo);
				}

				var reviewResponse = client.Reviews.CreateReviewsWithHttpMessagesAsync("application/json", teamName, requestInfo);
				// </snippet_createreview_create>

				// <snippet_createreview_ids>
				// Update the local cache to associate the created review IDs with the associated content.
				var reviewIds = reviewResponse.Result.Body;
				for (int i = 0; i < reviewIds.Count; i++) { reviewItems[i].ReviewId = reviewIds[i]; }

				WriteLine(outputWriter, JsonConvert.SerializeObject(reviewIds, Formatting.Indented));
				Thread.Sleep(throttleRate);

				// Get details of the reviews created that were sent to the Content Moderator website.
				WriteLine(outputWriter, null, true);
				WriteLine(outputWriter, "Getting review details:", true);
				foreach (var item in reviewItems)
				{
					var reviewDetail = client.Reviews.GetReviewWithHttpMessagesAsync(teamName, item.ReviewId);
					WriteLine(outputWriter, $"Review {item.ReviewId} for item ID {item.ContentId} is " +
						$"{reviewDetail.Result.Body.Status}.", true);
					WriteLine(outputWriter, JsonConvert.SerializeObject(reviewDetail.Result.Body, Formatting.Indented));
					Thread.Sleep(throttleRate);
				}
				// </snippet_createreview_ids>

				// <snippet_createreview_results>
				Console.WriteLine();
				Console.WriteLine("Perform manual reviews on the Content Moderator site.");
				Console.WriteLine("Then, press any key to continue.");
				Console.ReadKey();

				// After the human reviews, the results are confirmed.
				Console.WriteLine();
				Console.WriteLine($"Waiting {latencyDelay} seconds for results to propagate.");
				Thread.Sleep(latencyDelay * 1000);

				// Get details from the human review.
				WriteLine(writer, null, true);
				WriteLine(writer, "Getting review details:", true);
				foreach (var item in reviewItems)
				{
					var reviewDetail = client.Reviews.GetReviewWithHttpMessagesAsync(teamName, item.ReviewId);
					WriteLine(writer, $"Review {item.ReviewId} for item ID {item.ContentId} is " + $"{reviewDetail.Result.Body.Status}.", true);
					WriteLine(outputWriter, JsonConvert.SerializeObject(reviewDetail.Result.Body, Formatting.Indented));

					Thread.Sleep(throttleRate);
				}

				Console.WriteLine();
				Console.WriteLine("Check the OutputLog.txt file for results of the review.");

				writer = null;
				outputWriter.Flush();
				outputWriter.Close();
			}
			Console.WriteLine("--------------------------------------------------------------");
		}
		// </snippet_createreview_results>

		// <snippet_writeline>
		// Helper function that writes a message to the log file, and optionally to the console.
		// If echo is set to true, details will be written to the console.
		private static void WriteLine(TextWriter writer, string message = null, bool echo = true)
		{
			writer.WriteLine(message ?? String.Empty);
			if (echo) { Console.WriteLine(message ?? String.Empty); }
		}
		// </snippet_writeline>
		/*
		 * END - CREATE HUMAN IMAGE REVIEWS
		 */
	}
}
