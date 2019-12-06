import com.microsoft.azure.cognitiveservices.language.luis.authoring.*;
import com.microsoft.azure.cognitiveservices.language.luis.authoring.models.*;

import java.io.*;
import java.lang.Object.*;
import java.time.format.DateTimeFormatter;  
import java.time.LocalDateTime; 
import java.util.*;
import java.net.*;

/* This sample for the Azure Cognitive Services LUIS API shows how to:
 * - Create an application.
 * - Add intents to an application.
 * - Add entities to an application.
 * - Add utterances to an application.
 * - Train an application.
 * - Publish an application.
 * - Delete an application.
 * - List all applications.
 */

/* To collect the Maven libraries needed, enter this at the command prompt:
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

/* Note If you run this sample with JRE 9+, you may encounter the following issue:
 * https://github.com/Azure/autorest-clientruntime-for-java/issues/569
 * which results in the following output:
 *
 * WARNING: An illegal reflective access operation has occurred
 * WARNING: Illegal reflective access by com.microsoft.rest.Validator (file:client-runtime-1.6.6.jar) to field java.util.ArrayList.serialVersionUID
 * WARNING: Please consider reporting this to the maintainers of com.microsoft.rest.Validator
 * WARNING: Use --illegal-access=warn to enable warnings of further illegal reflective access operations
 * WARNING: All illegal access operations will be denied in a future release
 *
 * This should not prevent the sample from running correctly.
 */

public class Quickstart {
	/* Configure the local environment:
	* Set the following environment variables on your local machine using the
	* appropriate method for your preferred shell (Bash, PowerShell, Command
	* Prompt, etc.).
	*
	* LUIS_AUTHORING_KEY
	* LUIS_AUTHORING_ENDPOINT
	*
	* If the environment variable is created after the application is launched in a console or with Visual
	* Studio, the shell (or Visual Studio) needs to be closed and reloaded to take the environment variable into account.
	*/
    private static String authoring_key = System.getenv("LUIS_AUTHORING_KEY");
	private static String s_authoring_endpoint = System.getenv("LUIS_AUTHORING_ENDPOINT");

// Note EndpointAPI.fromString cannot parse a URL that begins with "https://", so we remove it.
	private static com.microsoft.azure.cognitiveservices.language.luis.authoring.EndpointAPI authoring_endpoint = com.microsoft.azure.cognitiveservices.language.luis.authoring.EndpointAPI.fromString(s_authoring_endpoint.replace ("https://", ""));

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

		DateTimeFormatter dtf = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss");  
		String name = "Contoso " + dtf.format(LocalDateTime.now());
		String description = "Flight booking app built with Azure SDK for Java.";
		String version = "0.1";
		String locale = "en-us";
		ApplicationCreateObject create_app_payload = new ApplicationCreateObject().withName(name).withDescription(description).withInitialVersionId(version).withCulture(locale);
		UUID app_id = client.apps().add(create_app_payload);
		System.out.println("Created LUIS app " + name + " with ID " + app_id.toString());
		return new AppInfo(app_id.toString(), version);
	}

	public void add_entities(AppInfo app_info) {
		LUISAuthoringClient client = LUISAuthoringManager.authenticate(authoring_endpoint, authoring_key);

		String entity_name = "Destination";
		Models.ModelsAddEntityDefinitionStages.WithAppId stages = client.models().addEntity();
		stages.withAppId(UUID.fromString(app_info.app_id)).withVersionId(app_info.app_version).withName(entity_name).execute();

		String h_entity_name = "Class";
		List<String> h_entity_children = Arrays.asList("First", "Business", "Economy");
		client.models().addHierarchicalEntity(UUID.fromString(app_info.app_id), app_info.app_version, new HierarchicalEntityModel().withName(h_entity_name).withChildren(h_entity_children));

		String c_entity_name = "Flight";
		List<String> c_entity_children = Arrays.asList("Class", "Destination");
		client.models().addCompositeEntity(UUID.fromString(app_info.app_id), app_info.app_version, new CompositeEntityModel().withName(c_entity_name).withChildren(c_entity_children));

		System.out.println("Entities Destination, Class, Flight created.");
	}

	public void add_intents(AppInfo app_info) {
		LUISAuthoringClient client = LUISAuthoringManager.authenticate(authoring_endpoint, authoring_key);

		String intent_name = "FindFlights";
		Models.ModelsAddIntentDefinitionStages.WithAppId stages = client.models().addIntent();
		stages.withAppId(UUID.fromString(app_info.app_id)).withVersionId(app_info.app_version).withName(intent_name);

		System.out.println("Intent FindFlights added.");
	}

	public ExampleLabelObject create_utterance(String intent, String text, Map<String, String> labels) {
		text = text.toLowerCase();

		ArrayList<EntityLabelObject> entityLabels = new ArrayList<EntityLabelObject>();

		for (Map.Entry<String, String> entry : labels.entrySet()) {
			String key = entry.getKey();
			String value = entry.getValue().toLowerCase();
			
			int start_index = text.indexOf(value);
			int end_index = start_index + value.length();
			if (start_index > -1) {
				entityLabels.add(new EntityLabelObject().withEntityName(key).withStartCharIndex(start_index).withEndCharIndex(end_index));
			}
		}
	
		System.out.println("Created " + entityLabels.size() + " utterances.");

		return new ExampleLabelObject().withText(text).withEntityLabels(entityLabels).withIntentName(intent);
	}

	public void add_utterances(AppInfo app_info) {
		LUISAuthoringClient client = LUISAuthoringManager.authenticate(authoring_endpoint, authoring_key);

		// Each entity must have at least one utterance before we train the application.
		ExampleLabelObject utterance_1 = create_utterance("FindFlights", "find flights in economy to Madrid", Map.of("Flight", "economy to Madrid", "Destination", "Madrid", "Class", "economy"));
		ExampleLabelObject utterance_2 = create_utterance("FindFlights", "find flights to London in first class", Map.of ("Flight", "London in first class", "Destination", "London", "Class", "first"));
		List<ExampleLabelObject> utterances = Arrays.asList(utterance_1, utterance_2);

		client.examples().batch(UUID.fromString(app_info.app_id), app_info.app_version, utterances);

		System.out.println("Example utterances added.")	;
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

		 ProductionOrStagingEndpointInfo result = client.apps().publish(UUID.fromString(app_info.app_id), new ApplicationPublishObject().withVersionId(app_info.app_version).withIsStaging(true));
		
		System.out.println("Application published. Endpoint URL: " + result.endpointUrl());
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
			quickstart.add_entities(app_info);
			quickstart.add_intents(app_info);
			quickstart.add_utterances(app_info);
			quickstart.train_application(app_info);
			quickstart.publish_application(app_info);
			quickstart.delete_application(app_info);
        } catch (Exception e) {
            System.out.println(e.getMessage());
            e.printStackTrace();
        }
    }

}
