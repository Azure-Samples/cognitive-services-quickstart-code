import com.microsoft.azure.cognitiveservices.language.luis.authoring.*;
import com.microsoft.azure.cognitiveservices.language.luis.authoring.models.*;
import com.microsoft.azure.cognitiveservices.language.luis.runtime.*;
import com.microsoft.azure.cognitiveservices.language.luis.runtime.models.*;

import java.io.*;
import java.lang.Object.*;
import java.time.format.DateTimeFormatter;  
import java.time.LocalDateTime; 
import java.util.*;
import java.net.*;

/* 
 * To collect the Maven libraries needed, enter this at the command prompt:
 * mvn clean dependency:copy-dependencies
 * 
 * To compile and run, enter the following at a command prompt:
 * javac Quickstart.java -cp .;lib\*
 * java -cp .;lib\* Quickstart
 * 
 * This presumes your libraries are stored in a folder named "lib"
 * directly under the current folder. If not, please adjust the
 * -classpath/-cp value accordingly.
 */

public class Quickstart {
	/* Configure the local environment:
	* Set the following environment variables on your local machine using the
	* appropriate method for your preferred shell (Bash, PowerShell, Command
	* Prompt, etc.).
	*
	* LUIS_AUTHORING_KEY
	* LUIS_AUTHORING_ENDPOINT
	* LUIS_RUNTIME_KEY
	* LUIS_RUNTIME_ENDPOINT
	*
	* If the environment variable is created after the application is launched in a console or with Visual
	* Studio, the shell (or Visual Studio) needs to be closed and reloaded to take the environment variable into account.
	*/
    private static String authoring_key = System.getenv("LUIS_AUTHORING_KEY");
	private static String s_authoring_endpoint = System.getenv("LUIS_AUTHORING_ENDPOINT");
    private static String runtime_key = System.getenv("LUIS_RUNTIME_KEY");
	private static String s_runtime_endpoint = System.getenv("LUIS_RUNTIME_ENDPOINT");

// Note EndpointAPI.fromString cannot parse a URL that begins with "https://", so we remove it.
	private static com.microsoft.azure.cognitiveservices.language.luis.authoring.EndpointAPI authoring_endpoint = com.microsoft.azure.cognitiveservices.language.luis.authoring.EndpointAPI.fromString(s_authoring_endpoint.replace ("https://", ""));
	private static com.microsoft.azure.cognitiveservices.language.luis.runtime.EndpointAPI runtime_endpoint = com.microsoft.azure.cognitiveservices.language.luis.runtime.EndpointAPI.fromString(s_runtime_endpoint.replace ("https://", ""));

	private class AppInfo {
		public String app_id;
		public String app_version;
		public AppInfo(String app_id, String app_version) {
			this.app_id = app_id;
			this.app_version = app_version;
		}
	}

	public static void list_applications() {
		LUISAuthoringClient client = LUISAuthoringManager.authenticate(authoring_endpoint, authoring_key);

		Apps.AppsListDefinitionStages.WithExecute stages = client.apps().list();
		List<ApplicationInfoResponse> result = stages.execute();

		for (ApplicationInfoResponse app : result) { 
			System.out.print(app.id().toString()); 
		} 
	}

	public AppInfo create_application() {
		LUISAuthoringClient client = LUISAuthoringManager.authenticate(authoring_endpoint, authoring_key);

		PrebuiltDomainCreateObject create_app_payload = new PrebuiltDomainCreateObject().withDomainName("HomeAutomation").withCulture("en-us");
		UUID app_id = client.apps().addCustomPrebuiltDomain(create_app_payload);
		System.out.println("Created LUIS app with ID " + app_id.toString());
		return new AppInfo(app_id.toString(), "0.1");
	}

	public void train_application(AppInfo app_info) throws InterruptedException {
		LUISAuthoringClient client = LUISAuthoringManager.authenticate(authoring_endpoint, authoring_key);

		client.trains().trainVersion(UUID.fromString(app_info.app_id), app_info.app_version);

		// Wait for the train operation to finish.
		System.out.println ("Waiting for train operation to finish...");

		Boolean waiting = true;
		while (true == waiting) {
			List<ModelTrainingInfo> result = client.trains().getStatus(UUID.fromString(app_info.app_id), app_info.app_version);
			// GetStatus returns a list of training statuses, one for each model.
			// Loop through them and make sure all are done.
			waiting = false;
			for (ModelTrainingInfo info : result) { 
				String status = info.details().status();
				if (status.equals("Queued") || status.equals("InProgress")) {
					waiting = true;
				}
				if (status.equals("Fail")) {
					System.out.println("Training operation failed. Reason: " + info.details().failureReason());
				}
			}
			if (true == waiting) {
				System.out.println("Waiting 10 seconds for training to complete...");
				Thread.sleep(10000);
			}
		}
	}

	public void publish_application(AppInfo app_info) {
		LUISAuthoringClient client = LUISAuthoringManager.authenticate(authoring_endpoint, authoring_key);

		/* NOTE: Make sure you use the same value for staging when you publish the app and when you query it.
		Otherwise, the query raises an exception that the app was not found. */
		 ProductionOrStagingEndpointInfo result = client.apps().publish(UUID.fromString(app_info.app_id), new ApplicationPublishObject().withVersionId(app_info.app_version).withIsStaging(true));
		
		System.out.println("Application published. Endpoint URL: " + result.endpointUrl());
	}

	public void query_application(AppInfo app_info) {
		LuisRuntimeAPI client = LuisRuntimeManager.authenticate(runtime_endpoint, runtime_key);
		String query = "turn on the lights";

		/* NOTE: Make sure you use the same value for staging when you publish the app and when you query it.
		Otherwise, the query raises an exception that the app was not found. */
		LuisResult result = client.predictions().resolve().withAppId(app_info.app_id).withQuery(query).withStaging(true).execute();
		System.out.println("Top scoring intent: " + result.topScoringIntent().intent());
		if (result.sentimentAnalysis() != null) {
			System.out.println("Sentiment label: " + result.sentimentAnalysis().label() + ". Score: " + result.sentimentAnalysis().score());
		}
		if (result.intents() != null) {
			System.out.println("Intents: ");
			for (IntentModel intent : result.intents()) { 
				System.out.println("\t" + intent.intent() + ". Score: " + intent.score());
			}
		}
		if (result.entities() != null) {
			System.out.println("Entities: ");
			for (EntityModel entity : result.entities()) { 
				System.out.println("\t" + entity.entity() + ". Type: " + entity.type());
			}
		}
	}

	public void delete_application(AppInfo app_info) {
		LUISAuthoringClient client = LUISAuthoringManager.authenticate(authoring_endpoint, authoring_key);

		OperationStatus status = client.apps().delete(UUID.fromString(app_info.app_id));
		OperationStatusType code = status.code();
		System.out.println("Deleted LUIS app with ID " + app_info.app_id + ". Operation status: " + code);
	}

    public static void main(String[] args) {
		try {
			Quickstart quickstart = new Quickstart();
			AppInfo app_info = quickstart.create_application();

			quickstart.train_application(app_info);
			quickstart.publish_application(app_info);
			quickstart.query_application(app_info);
			quickstart.delete_application(app_info);
        } catch (Exception e) {
            System.out.println(e.getMessage());
            e.printStackTrace();
        }
    }
}
