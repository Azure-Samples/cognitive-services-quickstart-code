//
// This quickstart shows how to predict the intent of an utterance by using the LUIS REST APIs.
//

import java.io.*;
import java.net.URI;
import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.utils.URIBuilder;
import org.apache.http.impl.client.HttpClients;
import org.apache.http.util.EntityUtils;

// To compile, execute this command at the console:
//      Windows: javac -cp ";lib/*" Predict.java
//      macOs: javac -cp ":lib/*" Predict.java
//      Linux: javac -cp ":lib/*" Predict.java

// To run, execute this command at the console:
//      Windows: java -cp ";lib/*" Predict
//      macOs: java -cp ":lib/*" Predict
//      Linux: java -cp ":lib/*" Predict

public class Predict {

    public static void main(String[] args)
    {
        HttpClient httpclient = HttpClients.createDefault();

        try
        {
            //////////
            // Values to modify.

            // YOUR-APP-ID: The App ID GUID found on the www.luis.ai Application Settings page.
            String AppId = "YOUR-APP-ID";

            // YOUR-PREDICTION-KEY: Your LUIS authoring key, 32 character value.
            String Key = "YOUR-PREDICTION-KEY";

            // YOUR-PREDICTION-ENDPOINT: Replace this with your authoring key endpoint.
            // For example, "https://westus.api.cognitive.microsoft.com/"
            String Endpoint = "https://YOUR-PREDICTION-ENDPOINT/";

            // The utterance you want to use.
            String Utterance = "I want two large pepperoni pizzas on thin crust please";
            //////////

            // Begin building the endpoint URL.
            URIBuilder endpointURLbuilder = new URIBuilder(Endpoint + "luis/prediction/v3.0/apps/" + AppId + "/slots/production/predict?");

            // Create the query string params.
            endpointURLbuilder.setParameter("query", Utterance);
            endpointURLbuilder.setParameter("subscription-key", Key);
            endpointURLbuilder.setParameter("show-all-intents", "true");
            endpointURLbuilder.setParameter("verbose", "true");

            // Create the prediction endpoint URL.
            URI endpointURL = endpointURLbuilder.build();

            // Create the HTTP object from the URL.
            HttpGet request = new HttpGet(endpointURL);

            // Access the LUIS endpoint to analyze the text utterance.
            HttpResponse response = httpclient.execute(request);

            // Get the response.
            HttpEntity entity = response.getEntity();

            // Print the response on the console.
            if (entity != null)
            {
                System.out.println(EntityUtils.toString(entity));
            }
        }

        // Display errors if they occur.
        catch (Exception e)
        {
            System.out.println(e.getMessage());
        }
    }
}
