// <imports>
'use strict'

const fs = require('fs')
const parse = require("csv-parse/lib/sync");
const AnomalyDetector = require('@azure/cognitiveservices-anomalydetector')
const msRest = require('@azure/ms-rest-js')
// </imports>

/**
 * This quickstart will detect anomolies in spreadsheet (.csv) data in two different ways:
 *  - Using the batch method (detects anomalies in entire series of data points)
 *  - Using the stream method (detects the latest data point in a series)
 * 
 * Prerequisites:
 *   - Install the following modules: 
 *       npm install @azure/ms-rest-js
 *       npm install @azure/cognitiveservices-anomalydetector
 *   - Add your Anomaly Detector subscription key and endpoint to your environment variables
 *   - Add the request-data.csv file to your local root folder
 * 
 * Reference:
 *   Anomaly Detector documentation: https://docs.microsoft.com/en-us/azure/cognitive-services/anomaly-detector/
 *   SDK: https://docs.microsoft.com/en-us/javascript/api/overview/azure/cognitiveservices/anomalydetector?view=azure-node-latest
 *   API: https://westus2.dev.cognitive.microsoft.com/docs/services/AnomalyDetector/operations/post-timeseries-entire-detect
 */

//<vars>
// Spreadsheet with 2 columns and n rows.
let CSV_FILE = './request-data.csv'

// Authentication variables
// Add your Anomaly Detector subscription key and endpoint to your environment variables.
let key = process.env['ANOMALY_DETECTOR_KEY']
let endpoint = process.env['ANOMALY_DETECTOR_ENDPOINT']
let credentials = new msRest.ApiKeyCredentials({ inHeader: { 'Ocp-Apim-Subscription-Key': key } })
// Points array for the request body
let points = []
//</vars>

// <authentication>
let anomalyDetectorClient = new AnomalyDetector.AnomalyDetectorClient(credentials, endpoint)
// </authentication>

// <readFile>
function readFile() {
    let input = fs.readFileSync(CSV_FILE).toString();
    let parsed = parse(input, { skip_empty_lines: true });
    parsed.forEach(function (e) {
        points.push({ timestamp: new Date(e[0]), value: parseFloat(e[1]) });
    });
}
readFile()
// </readFile>

// <batchCall>
async function batchCall() {
    // Create request body for API call
    let body = { series: points, granularity: 'daily' }
    // Make the call to detect anomalies in whole series of points
    await anomalyDetectorClient.entireDetect(body)
        .then((response) => {
            console.log("Batch (entire) anomaly detection):")
            for (let item = 0; item < response.isAnomaly.length; item++) {
                if (response.isAnomaly[item]) {
                    console.log("An anomaly was detected from the series, at row " + item)
                }
            }
        }).catch((error) => {
            console.log(error)
        })

}
batchCall()
// </batchCall>

// <lastDetection>
async function lastDetection() {

    let body = { series: points, granularity: 'daily' }
    // Make the call to detect anomalies in the latest point of a series
    await anomalyDetectorClient.lastDetect(body)
        .then((response) => {
            console.log("Latest point anomaly detection:")
            if (response.isAnomaly) {
                console.log("The latest point, in row " + points.length + ", is detected as an anomaly.")
            } else {
                console.log("The latest point, in row " + points.length + ", is not detected as an anomaly.")
            }
        }).catch((error) => {
            console.log(error)
        })
}
lastDetection()
// </lastDetection>

// <changePointDetection>
async function changePointDetection() {

    let body = { series: points, granularity: 'daily' }
    // get change point detect results
    await anomalyDetectorClient.detectChangePoint(body)
        .then((response) => {
            if (
                response.isChangePoint.some(function (changePoint) {
                    return changePoint === true;
                })
            ) {
                console.log("Change points were detected from the series at index:");
                response.isChangePoint.forEach(function (changePoint, index) {
                    if (changePoint === true) {
                        console.log(index);
                    }
                });
            } else {
                console.log("There is no change point detected from the series.");
            }
        }).catch((error) => {
            console.log(error)
        })
}
// </changePointDetection>
