'use strict'

const fs = require('fs')
const readline = require('readline')

const AnomalyDetector = require('@azure/cognitiveservices-anomalydetector')
const msRest = require('@azure/ms-rest-js')

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

// Spreadsheet with 2 columns and n rows.
let CSV_FILE = './request-data.csv'

// Authenticate
let key = process.env['ANOMALY_DETECTOR_SUBSCRIPTION_KEY']
let endpoint = process.env['ANOMALY_DETECTOR_ENDPOINT']
let credentials = new msRest.ApiKeyCredentials({ inHeader: { 'Ocp-Apim-Subscription-Key': key } })
let anomalyDetectorClient = new AnomalyDetector.AnomalyDetectorClient(credentials, endpoint)

async function main() {

    /*
    ANOMALY DETECTOR - BATCH
    */
    // Points array for the request body
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
        }
        points.push(point)
    }).on('close', async function() {
        //console.log(points)

         // Create request body for API call
        let body = { series: points, granularity: 'daily' }
        // Make the call to detect anomalies in whole series of points
        await anomalyDetectorClient.entireDetect(body)
            .then((response) => {
                console.log("Batch:")
                for (let item = 0; item < response.isAnomaly.length; item++) {
                    if (response.isAnomaly[item]) {
                        console.log("An anomaly was detected from the series, at row " + item)
                    } 
                }
            }).catch((error) => {
                console.log(error)
            })  
    }).on('error', function(err) {
        console.log(err)    
    })
    /*
    END - ANOMALY DETECTOR - BATCH
    */

    /*
    ANOMALY DETECTOR - STREAM
    */
    // Points array for the request body
    var points_stream = []

    await readline.createInterface({
        input: fs.createReadStream(CSV_FILE),
        terminal: false
    }).on('line', function(line) {
        var row = line.split(",")
        // Create points from each row of data
        let point_stream = {
            timestamp: new Date(row[0]), 
            value: parseFloat(row[1])
        }
        points_stream.push(point_stream)
    }).on('close', async function() {
        // Create request body for API call
        let body = { series: points_stream, granularity: 'daily' }
        // Make the call to detect anomalies in the lastest point of a series
        await anomalyDetectorClient.lastDetect(body)
            .then((response) => {
                console.log("Stream:")
                if (response.isAnomaly) {
                    console.log("The latest point, in row " + points_stream.length + ", is detected as an anomaly.")
                } else {
                    console.log("The latest point, in row " + points_stream.length + ", is not detected as an anomaly.")
                }
            }).catch((error) => {
                console.log(error)
            })  
    }).on('error', function(err) {
        console.log(err)    
    })
    console.log()
    /*
    END - ANOMALY DETECTOR - STREAM
    */
}
main()
