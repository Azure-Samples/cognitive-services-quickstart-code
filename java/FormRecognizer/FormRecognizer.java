
// <snippet_imports>
import com.azure.ai.formrecognizer.*;
import com.azure.ai.formrecognizer.training.*;
import com.azure.ai.formrecognizer.models.*;
import com.azure.ai.formrecognizer.training.models.*;

import java.util.concurrent.atomic.AtomicReference;
import java.util.List;
import java.util.Map;
import java.time.LocalDate;

import com.azure.core.credential.AzureKeyCredential;
import com.azure.core.http.rest.PagedIterable;
import com.azure.core.util.Context;
import com.azure.core.util.polling.SyncPoller;
// <.snippet_imports>

public class FormRecognizer {

    // <snippet_creds>
    static final String key = "<replace-with-your-form-recognizer-key>";
    static final String endpoint = "<replace-with-your-form-recognizer-endpoint>";
    // </snippet_creds>

    public static void main(String[] args) {
        // <snippet_auth>
        FormRecognizerClient recognizerClient = new FormRecognizerClientBuilder()
                .credential(new AzureKeyCredential(key)).endpoint(endpoint).buildClient();

        FormTrainingClient trainingClient = new FormTrainingClientBuilder().credential(new AzureKeyCredential(key))
                .endpoint(endpoint).buildClient();
        // </snippet_auth>

        // <snippet_mainvars>
        String trainingDataUrl = "<SAS-URL-of-your-form-folder-in-blob-storage>";
        String formUrl = "<SAS-URL-of-a-form-in-blob-storage>";
        String receiptUrl = "https://docs.microsoft.com/azure/cognitive-services/form-recognizer/media"
                + "/contoso-allinone.jpg";
        // </snippet_mainvars>
        
        // <snippet_maincalls>
        // Call Form Recognizer scenarios:
        System.out.println("Get form content...");
        GetContent(recognizerClient, formUrl);

        System.out.println("Analyze receipt...");
        AnalyzeReceipt(recognizerClient, receiptUrl);

        System.out.println("Train Model with training data...");
        String modelId = TrainModel(trainingClient, trainingDataUrl);

        System.out.println("Analyze PDF form...");
        AnalyzePdfForm(recognizerClient, modelId, formUrl);

        System.out.println("Manage models...");
        ManageModels(trainingClient, trainingDataUrl);
        // </snippet_maincalls>

    }
}