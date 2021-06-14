/*  This Quickstart for the Azure Cognitive Services Computer Vision API shows how to analyze
 *  an image and recognize text for both a local and remote (URL) image.
 *
 *  - Extract Text (OCR) with the new Read API to find text in an image or document.
 */

// <snippet_imports_and_vars>
// <snippet_imports>
import com.microsoft.azure.cognitiveservices.vision.computervision.*;
import com.microsoft.azure.cognitiveservices.vision.computervision.implementation.ComputerVisionImpl;
import com.microsoft.azure.cognitiveservices.vision.computervision.models.*;

import java.io.*;
import java.nio.file.Files;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;
// </snippet_imports>

// <snippet_classdef_1>
public class ComputerVisionQuickstart {
    // </snippet_classdef_1>

    // <snippet_creds>
    static String subscriptionKey = "PASTE_YOUR_COMPUTER_VISION_SUBSCRIPTION_KEY_HERE";
    static String endpoint = "PASTE_YOUR_COMPUTER_VISION_ENDPOINT_HERE";
    // </snippet_creds>
    // </snippet_imports_and_vars>

    // <snippet_beginmain>
    public static void main(String[] args) {
        
        System.out.println("\nAzure Cognitive Services Computer Vision - Java Quickstart Sample");
        // </snippet_beginmain>

        // <snippet_authinmain>
        // Create an authenticated Computer Vision client.
        ComputerVisionClient compVisClient = Authenticate(subscriptionKey, endpoint); 
        // </snippet_authinmain>

        // <snippet_readinmain>
        // Read from local file
        ReadFromFile(compVisClient);
        // </snippet_readinmain>
    // <snippet_endmain>
    }
    // </snippet_endmain>

    // <snippet_auth>
    public static ComputerVisionClient Authenticate(String subscriptionKey, String endpoint){
        return ComputerVisionManager.authenticate(subscriptionKey).withEndpoint(endpoint);
    }
    // </snippet_auth>
    
    /**
     * OCR with READ : Performs a Read Operation on a remote image
     * @param client instantiated vision client
     * @param remoteTextImageURL public url from which to perform the read operation against
     */
    
    private static void ReadFromUrl(ComputerVisionClient client) {
        System.out.println("-----------------------------------------------");
        String remoteTextImageURL = "https://intelligentkioskstore.blob.core.windows.net/visionapi/suggestedphotos/3.png";
        System.out.println("Read with URL: " + remoteTextImageURL);
        try {
            // Cast Computer Vision to its implementation to expose the required methods
            ComputerVisionImpl vision = (ComputerVisionImpl) client.computerVision();

            // Read in remote image and response header
            ReadHeaders responseHeader = vision.readWithServiceResponseAsync(remoteTextImageURL, null, null,null)
                    .toBlocking()
                    .single()
                    .headers();

            // Extract the operation Id from the operationLocation header
            String operationLocation = responseHeader.operationLocation();
            System.out.println("Operation Location:" + operationLocation);

            getAndPrintReadResult(vision, operationLocation);

        } catch (Exception e) {
            System.out.println(e.getMessage());
            e.printStackTrace();
        }
    }

    // <snippet_read_setup>
    /**
     * OCR with READ : Performs a Read Operation on a local image
     * @param client instantiated vision client
     * @param localFilePath local file path from which to perform the read operation against
     */
    private static void ReadFromFile(ComputerVisionClient client) {
        System.out.println("-----------------------------------------------");
        
        String localFilePath = "src\\main\\resources\\myImage.png";
        System.out.println("Read with local file: " + localFilePath);
        // </snippet_read_setup>
        // <snippet_read_call>

        try {
            File rawImage = new File(localFilePath);
            byte[] localImageBytes = Files.readAllBytes(rawImage.toPath());

            // Cast Computer Vision to its implementation to expose the required methods
            ComputerVisionImpl vision = (ComputerVisionImpl) client.computerVision();

            // Read in remote image and response header
            ReadInStreamHeaders responseHeader =
                    vision.readInStreamWithServiceResponseAsync(localImageBytes, null, null)
                        .toBlocking()
                        .single()
                        .headers();
            // </snippet_read_call>
    // <snippet_read_response>
            // Extract the operationLocation from the response header
            String operationLocation = responseHeader.operationLocation();
            System.out.println("Operation Location:" + operationLocation);

            getAndPrintReadResult(vision, operationLocation);
    // </snippet_read_response>
            // <snippet_read_catch>

        } catch (Exception e) {
            System.out.println(e.getMessage());
            e.printStackTrace();
        }
    }
    // </snippet_read_catch>

    // <snippet_opid_extract>
    /**
     * Extracts the OperationId from a Operation-Location returned by the POST Read operation
     * @param operationLocation
     * @return operationId
     */
    private static String extractOperationIdFromOpLocation(String operationLocation) {
        if (operationLocation != null && !operationLocation.isEmpty()) {
            String[] splits = operationLocation.split("/");

            if (splits != null && splits.length > 0) {
                return splits[splits.length - 1];
            }
        }
        throw new IllegalStateException("Something went wrong: Couldn't extract the operation id from the operation location");
    }
    // </snippet_opid_extract>

    // <snippet_read_result_helper_call>
    /**
     * Polls for Read result and prints results to console
     * @param vision Computer Vision instance
     * @return operationLocation returned in the POST Read response header
     */
    private static void getAndPrintReadResult(ComputerVision vision, String operationLocation) throws InterruptedException {
        System.out.println("Polling for Read results ...");

        // Extract OperationId from Operation Location
        String operationId = extractOperationIdFromOpLocation(operationLocation);

        boolean pollForResult = true;
        ReadOperationResult readResults = null;

        while (pollForResult) {
            // Poll for result every second
            Thread.sleep(1000);
            readResults = vision.getReadResult(UUID.fromString(operationId));

            // The results will no longer be null when the service has finished processing the request.
            if (readResults != null) {
                // Get request status
                OperationStatusCodes status = readResults.status();

                if (status == OperationStatusCodes.FAILED || status == OperationStatusCodes.SUCCEEDED) {
                    pollForResult = false;
                }
            }
        }
        // </snippet_read_result_helper_call>
        
        // <snippet_read_result_helper_print>
        // Print read results, page per page
        for (ReadResult pageResult : readResults.analyzeResult().readResults()) {
            System.out.println("");
            System.out.println("Printing Read results for page " + pageResult.page());
            StringBuilder builder = new StringBuilder();

            for (Line line : pageResult.lines()) {
                builder.append(line.text());
                builder.append("\n");
            }

            System.out.println(builder.toString());
        }
    }
    // </snippet_read_result_helper_print>
    // <snippet_classdef_2>
}
// </snippet_classdef_2>
