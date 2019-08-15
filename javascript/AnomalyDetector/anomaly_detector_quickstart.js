'use strict';

const fs = require('fs');
const readline = require('readline');

const AnomalyDetector = require('@azure/cognitiveservices-anomalydetector')
const msRest = require('@azure/ms-rest-js')

/**
 * This quickstart will detect anomolies in spreadsheet data
 * 
 * Prerequisites:
 *   - Install the following modules: 
 *       npm install @azure/ms-rest-js
 *       npm install @azure/cognitiveservices-face
 *   - Add your Anomaly Detector subscription key and endpoint to your environment variables
 *   - Add the request-data.csv file to your local root folder
 * 
 * Reference:
 *   Anomaly Detector documentation: https://docs.microsoft.com/en-us/azure/cognitive-services/anomaly-detector/
 *   SDK: https://docs.microsoft.com/en-us/javascript/api/overview/azure/cognitiveservices/anomalydetector?view=azure-node-latest
 *   API: https://westus2.dev.cognitive.microsoft.com/docs/services/AnomalyDetector/operations/post-timeseries-entire-detect
 */

// Authenticate
let key = process.env['ANOMALY_DETECTOR_SUBSCRIPTION_KEY']
let endpoint = process.env['ANOMALY_DETECTOR_ENDPOINT']
let credentials = new msRest.ApiKeyCredentials({ inHeader: { 'Ocp-Apim-Subscription-Key': key } });
let anomalyDetectorClient = new AnomalyDetector.AnomalyDetectorClient(credentials, endpoint)

async function main() {
    // Spreadsheet with 2 columns and n rows.
    let CSV_FILE = './request-data.csv'

    // Points[] for the request body
    var points = []

    // Read the .csv file
    await readline.createInterface({
        input: fs.createReadStream(CSV_FILE),
        terminal: false
    }).on('line', function(line) {
        var row = line.split(",")
        // Create points from each row of data
        let point = {
            timestamp: new Date(row[0]), 
            value: parseFloat(row[1])
        };
        points.push(point)
    }).on('close', function() {
         // Create request body for API call
        let body = { series: points, granularity: 'daily' }

        // Make the call to detect anomalies
        anomalyDetectorClient.entireDetect(body)
            .then((response) => {
                for (let item = 0; item < response.isAnomaly.length; item++) {
                    if (response.isAnomaly[item]) {
                        console.log("An anomaly was detected from the series, at row " + item)
                    } 
                }
            }).catch((error) => {
                console.log(error)
            })  
    }).on('error', function(err) {
        console.log(err);    
    });
}
main()
