// <dependencies>
/* Download the following files.
 * - https://repo1.maven.org/maven2/com/microsoft/azure/cognitiveservices/azure-cognitiveservices-qnamaker/1.0.0-beta.1/azure-cognitiveservices-qnamaker-1.0.0-beta.1.jar
 * - https://repo1.maven.org/maven2/com/microsoft/azure/cognitiveservices/azure-cognitiveservices-qnamaker/1.0.0-beta.1/azure-cognitiveservices-qnamaker-1.0.0-beta.1.pom
 * Move the downloaded .jar file to a folder named "lib" directly under the current folder.
 * Rename the downloaded file to pom.xml.
 * At the command line, run
 * mvn dependency:copy-dependencies
 * This will download the .jar files depended on by azure-cognitiveservices-qnamaker-1.0.0-beta.1.jar to the folder "target/dependency" under the current folder. Move these .jar files to the "lib" folder as well.
 */
import com.microsoft.azure.cognitiveservices.knowledge.qnamaker.*;
import com.microsoft.azure.cognitiveservices.knowledge.qnamaker.models.*;

import java.io.*;
import java.lang.Object.*;
import java.time.format.DateTimeFormatter;  
import java.time.LocalDateTime; 
import java.util.*;
import java.net.*;
// </dependencies>


/* This sample does the following tasks.
 * Create a knowledge base.
 * Publish a knowledge base.
 * Download a knowledge base.
 * Query a knowledge base.
 * Delete a knowledge base.
*/

/* To compile and run, enter the following at a command prompt:
 * javac Quickstart.java -cp .;lib\*
 * java -cp .;lib\* Quickstart
 * This presumes your libraries are stored in a folder named "lib"
 * directly under the current folder. If not, please adjust the
 * -classpath/-cp value accordingly.
 */

public class Quickstart {
	// <resourceKeys>
	/* Configure the local environment:
	* Set the following environment variables on your local machine using the
	* appropriate method for your preferred shell (Bash, PowerShell, Command
	* Prompt, etc.).
	*
	* QNA_MAKER_SUBSCRIPTION_KEY
	* QNA_MAKER_ENDPOINT
	* QNA_MAKER_RUNTIME_ENDPOINT
	*
	* If the environment variable is created after the application is launched in a console or with Visual
	* Studio, the shell (or Visual Studio) needs to be closed and reloaded to take the environment variable into account.
	*/
    private static String authoring_key = System.getenv("QNA_MAKER_SUBSCRIPTION_KEY");
	private static String authoring_endpoint = System.getenv("QNA_MAKER_ENDPOINT");
	private static String runtime_endpoint = System.getenv("QNA_MAKER_RUNTIME_ENDPOINT");
	// </resourceKeys>

	// <authenticate>
	/* Note QnAMakerManager.authenticate() does not set the baseUrl paramater value
	 * as the value for QnAMakerClient.endpoint, so we still need to call withEndpoint().
	 */
	QnAMakerClient authoring_client = QnAMakerManager.authenticate(authoring_key).withEndpoint(authoring_endpoint);
	Knowledgebases kb_client = authoring_client.knowledgebases();
	Operations ops_client = authoring_client.operations();
	EndpointKeys keys_client = authoring_client.endpointKeys();
	// </authenticate>

	// <listKbs>
	public void list_kbs() {
		System.out.println("Listing KBs...");
		var result = kb_client.listAllAsync();
		result.subscribe(kbs -> {
			for (var kb : kbs.knowledgebases()) {
				System.out.print(kb.id().toString());
			};
		});
		System.out.println();
	}
	// </listKbs>

	// <waitForOperation>
	public String wait_for_operation(Operation op) throws Exception {
		System.out.println ("Waiting for operation to finish...");
		Boolean waiting = true;
		String result = "";
		while (true == waiting) {
			var op_ = ops_client.getDetails(op.operationId());
			var state = op_.operationState();
			if (OperationStateType.FAILED == state) {
				throw new Exception("Operation failed.");
			}
			if (OperationStateType.SUCCEEDED == state) {
				waiting = false;
				// Remove "/knowledgebases/" from the resource location.
				result = op_.resourceLocation().replace("/knowledgebases/", "");
			}
			if (true == waiting) {
				System.out.println("Waiting 10 seconds for operation to complete...");
				Thread.sleep(10000);
			}
		}
		return result;
	}
	// </waitForOperation>

	// <createKb>
	public String create_kb () throws Exception {
		System.out.println("Creating KB...");

		String name = "QnA Maker FAQ from quickstart";

		var metadata = new MetadataDTO()
			.withName ("Category")
			.withValue ("api");

		List<MetadataDTO> metadata_list = Arrays.asList(new MetadataDTO[]{ metadata });

		var qna = new QnADTO()
			.withAnswer ("You can use our REST APIs to manage your knowledge base.")
			.withQuestions ( Arrays.asList(new String[]{ "How do I manage my knowledgebase?" }))
			.withMetadata (metadata_list);

		List<QnADTO> qna_list = Arrays.asList(new QnADTO[]{ qna });

		var urls = Arrays.asList(new String[]{ "https://docs.microsoft.com/en-in/azure/cognitive-services/qnamaker/faqs" });

		var payload = new CreateKbDTO().withName(name).withQnaList(qna_list).withUrls(urls);

		var result = kb_client.create(payload);
		var kb_id = wait_for_operation(result);

		System.out.println("Created KB with ID: " + kb_id + ".\n");
		return kb_id;
	}
	// </createKb>

	// <publishKb>
	public void publish_kb(String kb_id) {
		System.out.println("Publishing KB...");
		kb_client.publish(kb_id);
		System.out.println("KB published.\n");
	}
	// </publishKb>

	// <downloadKb>
	public void download_kb(String kb_id) {
		System.out.println("Downloading KB...");

		var kb_data = kb_client.download(kb_id, EnvironmentType.PROD);
		System.out.println("KB Downloaded. It has " + kb_data.qnaDocuments().size() + " question/answer sets.");

		System.out.println("Downloaded KB.\n");
	}
	// </downloadKb>

	// <queryKb>
	public void query_kb(String kb_id) {
		System.out.println("Sending query to KB...");
		
		var runtime_key = keys_client.getKeys().primaryEndpointKey();
		QnAMakerRuntimeClient runtime_client = QnAMakerRuntimeManager.authenticate(runtime_key).withRuntimeEndpoint(runtime_endpoint);
		var query = (new QueryDTO()).withQuestion("How do I manage my knowledgebase?");
		var result = runtime_client.runtimes().generateAnswer(kb_id, query);
		System.out.println("Answers:");
		for (var answer : result.answers()) {
			System.out.println(answer.answer().toString());
		};
		System.out.println();
	}
	// </queryKb>

	// <deleteKb>
	public void delete_kb(String kb_id) {
		System.out.println("Deleting KB...");
		kb_client.delete(kb_id);
		System.out.println("KB deleted.\n");
	}
	// </deleteKb>

	// <main>
    public static void main(String[] args) {
		try {
			Quickstart quickstart = new Quickstart();
			String kb_id = quickstart.create_kb();
//			quickstart.list_kbs();
			quickstart.publish_kb(kb_id);
			quickstart.download_kb(kb_id);
			quickstart.query_kb(kb_id);
			quickstart.delete_kb(kb_id);
        } catch (Exception e) {
            System.out.println(e.getMessage());
            e.printStackTrace();
        }
    }
	// </main>
}