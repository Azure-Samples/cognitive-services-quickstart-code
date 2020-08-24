// httpclient-4.5.3.jar
// httpcore-4.4.6.jar
// commons-logging-1.2.jar

import java.net.URI;
import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.client.utils.URIBuilder;
import org.apache.http.impl.client.HttpClients;
import org.apache.http.util.EntityUtils;
import org.apache.http.entity.StringEntity;

// 1. Replace variable values with your own from QnA Maker Publish page
// 2. Compile with: javac -cp "lib/*" GetAnswer.java
// 3. Execute with: java -cp ".;lib/*" GetAnswer

public class GetAnswer {

    public static void main(String[] args) 
    {

        try
        {
            // Represents the various elements used to create HTTP request URIs
            // for QnA Maker operations.
            // From Publish Page: HOST
            // Example: https://YOUR-RESOURCE-NAME.azurewebsites.net/qnamaker
            String host = "YOUR-KB-HOST";

            // Management APIs postpend the version to the route
            // From Publish Page, value after POST
            // Example: /knowledgebases/ZZZ15f8c-d01b-4698-a2de-85b0dbf3358c/generateAnswer
            String service = "YOUR-KB-ROUTE";

            // Authorization endpoint key
            // From Publish Page
            String endpointKey = "EndpointKey YOUR-KB-ENDPOINT";

            // JSON format for passing question to service
            String question = "{ 'question' : 'Is the QnA Maker Service free?', 'top' : 3 }";

            // Create http client
            HttpClient httpclient = HttpClients.createDefault();

            // Add host + service to get full URI
            String answer_uri = host + service;
            System.out.println(answer_uri);

            //
            HttpPost request = new HttpPost(answer_uri);

            // set question
            StringEntity entity = new StringEntity(question);
            request.setEntity(entity);
            request.setHeader("Content-type", "application/json");

            // set authorization
            request.setHeader("Authorization",endpointKey);
            System.out.println("authorizationHeader " + endpointKey);

            // Send request to Azure service, get response
            HttpResponse response = httpclient.execute(request);

            HttpEntity entityResponse = response.getEntity();

            if (entityResponse != null) 
            {
                System.out.println("response back!");
                System.out.println(EntityUtils.toString(entityResponse));
            }
        }

        catch (Exception e)
        {
            System.out.println(e.getMessage());
        }
    }
}
