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

// Compile with: javac -cp ".;lib/*" QueryKB.java
// Run with: java -cp ".;lib/*" QueryKB

public class QueryKB {

/*
* Set the `authoring_key` and `authoring_endpoint` variables to your
* QnA Maker authoring subscription key and endpoint.
*
* These values can be found in the Azure portal (ms.portal.azure.com/).
* Look up your QnA Maker resource. Then, in the "Resource management"
* section, find the "Keys and Endpoint" page.
*
* The value of `authoring_endpoint` has the format https://YOUR-RESOURCE-NAME.cognitiveservices.azure.com.
*
* Set the `runtime_endpoint` variable to your QnA Maker runtime endpoint.
* The value of `runtime_endpoint` has the format https://YOUR-RESOURCE-NAME.azurewebsites.net.
*
* Set the `knowledge_base_id` variable to the ID of a knowledge base you have
* previously created.
*/
// <constants>
    private static String authoring_key = "PASTE_YOUR_QNA_MAKER_AUTHORING_SUBSCRIPTION_KEY_HERE";
	private static String authoring_endpoint = "PASTE_YOUR_QNA_MAKER_AUTHORING_ENDPOINT_HERE";
	private static String runtime_endpoint = "PASTE_YOUR_QNA_MAKER_RUNTIME_ENDPOINT_HERE";
	private static String knowledge_base_id = "PASTE_YOUR_QNA_MAKER_KB_ID_HERE";

    static String authoring_service = "/qnamaker/v4.0";
	static String get_runtime_key_method = "/endpointkeys";
	static String runtime_service = "/qnamaker/";
    static String query_kb_method = "/knowledgebases/" + knowledge_base_id + "/generateAnswer";
// </constants>

// <post>
    public static String PrettyPrint (String json_text) {
        JsonParser parser = new JsonParser();
        JsonElement json = parser.parse(json_text);
        Gson gson = new GsonBuilder().setPrettyPrinting().create();
        return gson.toJson(json);
    }

    public static class Response {
        Map<String, List<String>> Headers;
		int Status;
        String Response;

        public Response(Map<String, List<String>> headers, int status, String response) {
            this.Headers = headers;
			this.Status = status;
            this.Response = response;
        }
    }

    public static Response Post (URL url, String endpoint_key, String content) throws Exception{
        HttpsURLConnection connection = (HttpsURLConnection) url.openConnection();
        connection.setRequestMethod("POST");
        connection.setRequestProperty("Content-Type", "application/json");
        connection.setRequestProperty("Content-Length", content.length() + "");
// Note this differs from the "Ocp-Apim-Subscription-Key"/<subscription key> used by most Cognitive Services.
        connection.setRequestProperty("Authorization", "EndpointKey " + endpoint_key);
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
        return new Response (connection.getHeaderFields(), connection.getResponseCode(), response.toString());
    }

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
        return new Response (connection.getHeaderFields(), connection.getResponseCode(), response.toString());
    }

	public static Response GetRuntimeKey () throws Exception {
		URL url = new URL (authoring_endpoint + authoring_service + get_runtime_key_method);
        System.out.println ("Calling " + url.toString() + ".");
        return Get(url);
	}

    public static Response QueryKB (String content, String endpoint_key) throws Exception {
        URL url = new URL (runtime_endpoint + runtime_service + query_kb_method);
        System.out.println ("Calling " + url.toString() + ".");
        return Post(url, endpoint_key, content);
    }

    public static void main(String[] args) throws Exception
    {
        try
        {
			Response response = GetRuntimeKey();
			Type type = new TypeToken<Map<String, String>>(){}.getType();
			Map<String, String> fields = new Gson().fromJson(response.Response, type);
			String endpoint_key = fields.get ("primaryEndpointKey");

            // JSON format for passing question to service
            String question = "{ 'question' : 'Is the QnA Maker Service free?', 'top' : 3 }";

            // Send the request to publish the knowledge base.
            response = QueryKB (question, endpoint_key);

            System.out.println (PrettyPrint (response.Response));
        }

        catch (Exception e)
        {
            System.out.println(e.getMessage());
        }
    }
// </post>
}
