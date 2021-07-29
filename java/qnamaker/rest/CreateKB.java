// <dependencies>
import java.io.*;
import java.lang.reflect.Type;
import java.net.*;
import java.util.*;
import javax.net.ssl.HttpsURLConnection;

/**
 * Gson: https://github.com/google/gson
 * Maven info:
 *    <dependency>
 *      <groupId>com.google.code.gson</groupId>
 *      <artifactId>gson</artifactId>
 *      <version>2.8.5</version>
 *    </dependency>
 */
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import com.google.gson.reflect.TypeToken;
// </dependencies>

// Compile with: javac -cp ".;lib/*" CreateKB.java
// Run with: java -cp ".;lib/*" CreateKB

public class CreateKB {
/*
* Set the `authoring_key` and `authoring_endpoint` variables to your
* QnA Maker authoring subscription key and endpoint.
*
* These values can be found in the Azure portal (ms.portal.azure.com/).
* Look up your QnA Maker resource. Then, in the "Resource management"
* section, find the "Keys and Endpoint" page.
*
* The value of `authoring_endpoint` has the format https://YOUR-RESOURCE-NAME.cognitiveservices.azure.com.
*/
// <constants>
    private static String authoring_key = "PASTE_YOUR_QNA_MAKER_AUTHORING_SUBSCRIPTION_KEY_HERE";
	private static String authoring_endpoint = "PASTE_YOUR_QNA_MAKER_AUTHORING_ENDPOINT_HERE";

    static String service = "/qnamaker/v4.0";
    static String method = "/knowledgebases/create";
// </constants>

// <model>
    public static class KB {
        String name;
        Question[] qnaList;
        String[] urls;
        File[] files;
    }

    public static class Question {
        Integer id;
        String answer;
        String source;
        String[] questions;
        Metadata[] metadata;
    }

    public static class Metadata {
        String name;
        String value;
    }

    public static class File {
        String fileName;
        String fileUri;
    }

    public static KB GetKB () {
        KB kb = new KB ();
        kb.name = "Example Knowledge Base";

        Question q = new Question();
        q.id = 0;
        q.answer = "You can use our REST APIs to manage your Knowledge Base. See here for details: https://westus.dev.cognitive.microsoft.com/docs/services/58994a073d9e04097c7ba6fe/operations/58994a073d9e041ad42d9baa";
        q.source = "Custom Editorial";
        q.questions = new String[]{"How do I programmatically update my Knowledge Base?"};

        Metadata md = new Metadata();
        md.name = "category";
        md.value = "api";
        q.metadata = new Metadata[]{md};

        kb.qnaList = new Question[]{q};
        kb.urls = new String[]{"https://docs.microsoft.com/en-in/azure/cognitive-services/QnAMaker/troubleshooting"};

        return kb;
    }
// </model>

// <pretty>
    public static String PrettyPrint (String json_text) {
        JsonParser parser = new JsonParser();
        JsonElement json = parser.parse(json_text);
        Gson gson = new GsonBuilder().setPrettyPrinting().create();
        return gson.toJson(json);
    }
// </pretty>

// <response>
    public static class Response {
        Map<String, List<String>> Headers;
        String Response;

        public Response(Map<String, List<String>> headers, String response) {
            this.Headers = headers;
            this.Response = response;
        }
    }
// </response>

// <post>
    public static Response Post (URL url, String content) throws Exception{
        HttpsURLConnection connection = (HttpsURLConnection) url.openConnection();
        connection.setRequestMethod("POST");
        connection.setRequestProperty("Content-Type", "application/json");
        connection.setRequestProperty("Content-Length", content.length() + "");
        connection.setRequestProperty("Ocp-Apim-Subscription-Key", authoring_key);
        connection.setDoOutput(true);

        DataOutputStream wr = new DataOutputStream(connection.getOutputStream());
        byte[] encoded_content = content.getBytes("UTF-8");
        wr.write(encoded_content, 0, encoded_content.length);
        wr.flush();
        wr.close();

        StringBuilder response = new StringBuilder ();
        BufferedReader in = new BufferedReader(new InputStreamReader(connection.getInputStream(), "UTF-8"));
        String line;
        while ((line = in.readLine()) != null) {
            response.append(line);
        }
        in.close();
        return new Response (connection.getHeaderFields(), response.toString());
    }
// </post>

// <get>
    public static Response Get (URL url) throws Exception{
        HttpsURLConnection connection = (HttpsURLConnection) url.openConnection();
            connection.setRequestMethod("GET");
            connection.setRequestProperty("Ocp-Apim-Subscription-Key", authoring_key);
            connection.setDoOutput(true);
        StringBuilder response = new StringBuilder ();
        BufferedReader in = new BufferedReader(new InputStreamReader(connection.getInputStream(), "UTF-8"));

        String line;
        while ((line = in.readLine()) != null) {
            response.append(line);
        }
        in.close();
        return new Response (connection.getHeaderFields(), response.toString());
    }
// </get>

// <create_kb>
    public static Response CreateKB (KB kb) throws Exception {
        URL url = new URL (authoring_endpoint + service + method);
        System.out.println ("Calling " + url.toString() + ".");
        String content = new Gson().toJson(kb);
        return Post(url, content);
    }
// </create_kb>

// <get_status>
    public static Response GetStatus (String operation) throws Exception {
        URL url = new URL (authoring_endpoint + service + operation);
        System.out.println ("Calling " + url.toString() + ".");
        return Get(url);
    }
// </get_status>

// <main>
    public static void main(String[] args) {
        try {
            // Send the request to create the knowledge base.
            Response response = CreateKB (GetKB ());

            // Get operation ID
            String operation = response.Headers.get("Location").get(0);

            System.out.println (PrettyPrint (response.Response));

            // Loop until the request is completed.
            Boolean done = false;
            while (!done) {
                // Check on the status of the request.
                response = GetStatus (operation);
                System.out.println (PrettyPrint (response.Response));
                
                Type type = new TypeToken<Map<String, String>>(){}.getType();

                Map<String, String> fields = new Gson().fromJson(response.Response, type);

                String state = fields.get ("operationState");

                // If the request is still running, wait and then check the status again.
                if (state.equals("Running") || state.equals("NotStarted")) {
                    System.out.println ("Waiting 10 seconds...");
                    Thread.sleep (10000);
                }
                else {
                    done = true;
                }
            }
        } catch (Exception e) {
            System.out.println (e);
        }
    }
// </main>
}
