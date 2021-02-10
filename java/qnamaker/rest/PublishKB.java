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

// Compile with: javac -cp ".;lib/*" PublishKB.java
// Run with: java -cp ".;lib/*" PublishKB

public class PublishKB {
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
* Set the `knowledge_base_id` variable to the ID of a knowledge base you have
* previously created.
*/
// <constants>
    private static String authoring_key = "PASTE_YOUR_QNA_MAKER_SUBSCRIPTION_KEY_HERE";
	private static String authoring_endpoint = "PASTE_YOUR_QNA_MAKER_AUTHORING_ENDPOINT_HERE";
	private static String knowledge_base_id = "PASTE_YOUR_QNA_MAKER_KB_ID_HERE";

    static String service = "/qnamaker/v4.0";
    static String method = "/knowledgebases/" + knowledge_base_id;
// </constants>

// <post>
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
// </post>
}
