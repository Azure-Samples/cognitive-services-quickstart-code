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

// Compile with: javac -cp ".;lib/*" PublishKB.java
// Run with: java -cp ".;lib/*" PublishKB

public class PublishKB {

	/* Configure the local environment:
	* Set the following environment variables on your local machine using the
	* appropriate method for your preferred shell (Bash, PowerShell, Command
	* Prompt, etc.).
	*
	* QNA_MAKER_SUBSCRIPTION_KEY
	* QNA_MAKER_ENDPOINT
	* QNA_MAKER_KB_ID
	*
	* If the environment variable is created after the application is launched in a console or with Visual
	* Studio, the shell (or Visual Studio) needs to be closed and reloaded to take the environment variable into account.
	*/
    private static String authoring_key = System.getenv("QNA_MAKER_SUBSCRIPTION_KEY");
	private static String authoring_endpoint = System.getenv("QNA_MAKER_ENDPOINT");
	private static String knowledge_base_id = System.getenv("QNA_MAKER_KB_ID");

    static String service = "/qnamaker/v4.0";
    static String method = "/knowledgebases/" + knowledge_base_id;

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
        return new Response (connection.getHeaderFields(), connection.getResponseCode(), response.toString());
    }

    public static Response PublishKB (String knowledge_base_id) throws Exception {
        URL url = new URL (authoring_endpoint + service + method);
        System.out.println ("Calling " + url.toString() + ".");
        return Post(url, "");
    }

    public static void main(String[] args) 
    {
        try
        {
            // Send the request to publish the knowledge base.
            Response response = PublishKB (knowledge_base_id);

            // No returned content, 204 == success
            System.out.println("Response status code: " + response.Status);
        }

        catch (Exception e)
        {
            System.out.println(e.getMessage());
        }
    }
}
