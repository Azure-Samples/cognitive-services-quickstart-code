package main

import (
	"bufio"
	"context"
	"encoding/csv"
	"fmt"
	"github.com/Azure/azure-sdk-for-go/services/preview/cognitiveservices/v1.0/anomalydetector"
	"github.com/Azure/go-autorest/autorest"
	"github.com/Azure/go-autorest/autorest/date"
	"io"
	"log"
	"os"
	"strconv"
	"time"
)

/*
This quickstart shows examples for:
	Detecting anomalies in a batch (in a whole series of points)
	Detecting anomalies in stream (latest data point)

Prerequisites:
	- Install libraries on the command line (or in IDE):
		go get github.com/Azure/azure-sdk-for-go/services/preview/cognitiveservices/v1.0/anomalydetector
		go get github.com/Azure/go-autorest/autorest
	- Set your Anomaly Dector subscription key and endpoint to your environment variables.
	- Add the request-data.csv to your local root folder

How to run (from command line):
	go run AnomalyDetectorQuickstart.go

References:
	Anomaly Detector documentation: https://docs.microsoft.com/en-us/azure/cognitive-services/anomaly-detector/
	SDK: https://godoc.org/github.com/Azure/azure-sdk-for-go/services/preview/cognitiveservices/v1.0/anomalydetector
	API: https://westus2.dev.cognitive.microsoft.com/docs/services/AnomalyDetector/operations/post-timeseries-entire-detect
*/

var key  = os.Getenv("ANOMALY_DETECTOR_SUBSCRIPTION_KEY")
var endpoint = os.Getenv("ANOMALY_DETECTOR_ENDPOINT")
// Spreadsheet data that will be converted to a series of points
var filePath = "./request-data.csv"

func main() {

	// Authenticate your client for all examples
	client := anomalydetector.New(endpoint)
	client.Authorizer = autorest.NewCognitiveServicesAuthorizer(key)

	// Get data from file , used for all examples
	var series []anomalydetector.Point
	csvFile, _ := os.Open(filePath)
	reader := csv.NewReader(bufio.NewReader(csvFile))
	for {
		line, err := reader.Read()
		if err == io.EOF { break } else if err != nil { log.Fatal(err) }

		timestamp, _ := time.Parse(time.RFC3339, line[0])
		value, _ := strconv.ParseFloat(line[1], 64)

		series = append(series, anomalydetector.Point{Timestamp: &date.Time{timestamp}, Value: &value})
	}

	// Request body for the API call in both batch and stream examples
	var request = anomalydetector.Request{Series: &series, Granularity: anomalydetector.Daily}

	/*
	ANOMALY DETECTOR - BATCH
	*/
	// Detect the anomalies
	fmt.Println()
	fmt.Println("Detecting anomalies in the entire series...")
	fmt.Println()

	// Make the call
	responseBatch, err := client.EntireDetect(context.Background(), request)
	if err != nil { log.Fatal("ERROR:", err) }

	var anomalies int
	for index, isAnomaly := range *responseBatch.IsAnomaly {
		if isAnomaly {
			fmt.Println("An anomaly is detected from the series at row:" + strconv.Itoa(index))
			anomalies++
		}
	}
	if (anomalies == 0) {
		fmt.Println("There are no anomalies detected from the series.")
	}
	/*
	END - ANOMALY DETECTOR - BATCH
	*/
	
	/*
	ANOMALY DETECTOR - STREAM
	*/
	fmt.Println()
	fmt.Println("Detecting the latest point of a series for anomalies...")
	fmt.Println()

	responseStream, err := client.LastDetect(context.Background(), request)
	if err != nil { log.Fatal("ERROR:", err) }

	if *responseStream.IsAnomaly {
		fmt.Println("The latest point, in row " + strconv.Itoa(len(series)) + ", is detected as an anomaly.")
	} else {
		fmt.Println("The latest point, in row " + strconv.Itoa(len(series)) + ", is not detected as an anomaly.")
	}
	/*
	END - ANOMALY DETECTOR - STREAM
	*/
}
