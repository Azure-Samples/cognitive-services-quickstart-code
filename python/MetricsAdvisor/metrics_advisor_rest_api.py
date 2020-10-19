import requests
import json
import time


def add_data_feed(endpoint, subscription_key, api_key):
    url = endpoint + '/dataFeeds'
    data_feed_body = {
        "dataSourceType": "SqlServer",
        "dataFeedName": "test_data_feed_00000001",
        "dataFeedDescription": "",
        "dataSourceParameter": {
            "connectionString": "Server=<server>.database.windows.net,1433;Initial Catalog=<catalog>;Persist Security Info=False;User ID=<user-name>;Password=<password>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;",
            "query": "select * from adsample3 where Timestamp = @StartTime"
        },
        "granularityName": "Daily",
        "granularityAmount": 0,
        "metrics": [
            {
                "metricName": "revenue",
                "metricDisplayName": "revenue",
                "metricDescription": ""
            },
            {
                "metricName": "cost",
                "metricDisplayName": "cost",
                "metricDescription": ""
            }
        ],
        "dimension": [
            {
                "dimensionName": "city",
                "dimensionDisplayName": "city"
            },
            {
                "dimensionName": "category",
                "dimensionDisplayName": "category"
            }
        ],
        "timestampColumn": "timestamp",
        "dataStartFrom": "2020-06-01T00:00:00.000Z",
        "startOffsetInSeconds": 0,
        "maxConcurrency": -1,
        "minRetryIntervalInSeconds": -1,
        "stopRetryAfterInSeconds": -1,
        "allUpIdentification": "__SUM__",
        "needRollup": "AlreadyRollup",
        "fillMissingPointType": "SmartFilling",
        "fillMissingPointValue": 0,
        "viewMode": "Private",
        "admins": [
            "yongw@microsoft.com"
        ],
        "viewers": [
        ],
        "actionLinkTemplate": ""
    }
    res = requests.post(url, data=json.dumps(data_feed_body),
                        headers={'Ocp-Apim-Subscription-Key': subscription_key,
                                 'x-api-key': api_key})
    if res.status_code != 201:
        raise RuntimeError("add_data_feed failed " + res.text)
    else:
        print("add_data_feed success " + res.text)
    return res.headers['Location']


def check_ingestion_latest_status(endpoint, subscription_key, api_key, datafeed_id):
    url = endpoint + '/dataFeeds/{}/ingestionProgress'.format(datafeed_id)
    res = requests.get(url, headers={'Ocp-Apim-Subscription-Key': subscription_key,
                                     'x-api-key': api_key})
    if res.status_code != 200:
        raise RuntimeError("check_ingestion_latest_status failed " + res.text)
    else:
        print("check_ingestion_latest_status success " + res.text)


def check_ingestion_detail_status(endpoint, subscription_key, api_key, datafeed_id, start_time, end_time):
    url = endpoint + '/dataFeeds/{}/ingestionStatus/query'.format(datafeed_id)
    ingestion_detail_status_body = {
      "startTime": start_time,
      "endTime": end_time
    }
    res = requests.post(url, data=json.dumps(ingestion_detail_status_body),
                        headers={'Ocp-Apim-Subscription-Key': subscription_key,
                                 'x-api-key': api_key})
    if res.status_code != 200:
        raise RuntimeError("check_ingestion_detail_status failed " + res.text)
    else:
        print("check_ingestion_detail_status success " + res.text)


def create_detection_config(endpoint, subscription_key, api_key, metric_id):
    url = endpoint + '/enrichment/anomalyDetection/configurations'
    detection_config_body = {
        "name": "test_detection_config0000000001",
        "description": "string",
        "metricId": metric_id,
        "wholeMetricConfiguration": {
            "smartDetectionCondition": {
                "sensitivity": 100,
                "anomalyDetectorDirection": "Both",
                "suppressCondition": {
                    "minNumber": 1,
                    "minRatio": 1
                }
            }
        },
        "dimensionGroupOverrideConfigurations": [
        ],
        "seriesOverrideConfigurations": [
        ]
    }
    res = requests.post(url, data=json.dumps(detection_config_body),
                        headers={'Ocp-Apim-Subscription-Key': subscription_key,
                                 'x-api-key': api_key})
    if res.status_code != 201:
        raise RuntimeError("create_detection_config failed " + res.text)
    else:

        print("create_detection_config success " + res.text)
    return res.headers['Location']


def create_web_hook(endpoint, subscription_key, api_key):
    url = endpoint + '/hooks'
    web_hook_body = {
        "hookType": "Webhook",
        "hookName": "test_web_hook000001",
        "description": "",
        "externalLink": "",
        "hookParameter": {
            "endpoint": "https://www.xxx.com",
            "username": "",
            "password": ""
        }
    }
    res = requests.post(url, data=json.dumps(web_hook_body),
                        headers={'Ocp-Apim-Subscription-Key': subscription_key,
                                 'x-api-key': api_key})
    if res.status_code != 201:
        raise RuntimeError("create_web_hook failed " + res.text)
    else:
        print("create_web_hook success " + res.text)
    return res.headers['Location']


def create_alert_config(endpoint, subscription_key, api_key, anomaly_detection_configuration_id, hook_id):
    url = endpoint + '/alert/anomaly/configurations'
    web_hook_body = {
        "name": "test_alert_config00000001",
        "description": "",
        "crossMetricsOperator": "AND",
        "hookIds": [
           hook_id
        ],
        "metricAlertingConfigurations": [
            {
                "anomalyDetectionConfigurationId": anomaly_detection_configuration_id,
                "anomalyScopeType": "All",
                "severityFilter": {
                    "minAlertSeverity": "Low",
                    "maxAlertSeverity": "High"
                },
                "snoozeFilter": {
                    "autoSnooze": 0,
                    "snoozeScope": "Metric",
                    "onlyForSuccessive": True
                },
            }
        ]
    }
    res = requests.post(url, data=json.dumps(web_hook_body),
                        headers={'Ocp-Apim-Subscription-Key': subscription_key,
                                 'x-api-key': api_key})
    if res.status_code != 201:
        raise RuntimeError("create_alert_config failed " + res.text)
    else:
        print("create_alert_config success " + res.text)
    return res.headers['Location']


def query_alert_by_alert_config(endpoint, subscription_key, api_key, alert_config_id, start_time, end_time):
    url = endpoint + '/alert/anomaly/configurations/{}/alerts/query'.format(alert_config_id)
    alerts_body = {
        "startTime": start_time,
        "endTime": end_time,
        "timeMode": "AnomalyTime"
    }
    res = requests.post(url, data=json.dumps(alerts_body),
                        headers={'Ocp-Apim-Subscription-Key': subscription_key,
                                 'x-api-key': api_key})
    if res.status_code != 200:
        raise RuntimeError("query_alert_by_alert_config failed " + res.text)
    else:
        print("query_alert_by_alert_config success " + res.text)
    return [item['alertId'] for item in json.loads(res.content)['value']]


def query_anomaly_by_alert(endpoint, subscription_key, api_key, alert_config_id, alert_id):
    url = endpoint + '/alert/anomaly/configurations/{}/alerts/{}/anomalies'.format(alert_config_id, alert_id)
    res = requests.get(url,
                       headers={'Ocp-Apim-Subscription-Key': subscription_key,
                                'x-api-key': api_key})
    if res.status_code != 200:
        raise RuntimeError("query_anomaly_by_alert failed " + res.text)
    else:
        print("query_anomaly_by_alert success " + res.text)
    return json.loads(res.content)


def query_incident_by_alert(endpoint, subscription_key, api_key, alert_config_id, alert_id):
    url = endpoint + '/alert/anomaly/configurations/{}/alerts/{}/incidents'.format(alert_config_id, alert_id)
    res = requests.get(url,
                       headers={'Ocp-Apim-Subscription-Key': subscription_key,
                                'x-api-key': api_key})
    if res.status_code != 200:
        raise RuntimeError("query_incident_by_alert failed " + res.text)
    else:
        print("query_incident_by_alert success " + res.text)
    return json.loads(res.content)


def query_root_cause_by_incident(endpoint, subscription_key, api_key, detection_config_id, incident_id):
    url = endpoint + '/enrichment/anomalyDetection/configurations/{}/incidents/{}/rootCause'.format(detection_config_id, incident_id)
    res = requests.get(url,
                       headers={'Ocp-Apim-Subscription-Key': subscription_key,
                                'x-api-key': api_key})
    if res.status_code != 200:
        raise RuntimeError("query_root_cause_by_incident failed " + res.text)
    else:
        print("query_root_cause_by_incident success " + res.text)
    return json.loads(res.content)


def query_anomaly_by_detection_config(endpoint, subscription_key, api_key, detection_config_id, start_time, end_time):
    url = endpoint + '/enrichment/anomalyDetection/configurations/{}/anomalies/query'.format(detection_config_id)
    body = {
        "startTime": start_time,
        "endTime": end_time,
        "filter": {
            "dimensionFilter": [
            ],
            "severityFilter": {
              "min": "Low",
              "max": "High"
            }
          }
    }
    res = requests.post(url, data=json.dumps(body),
                        headers={'Ocp-Apim-Subscription-Key': subscription_key,
                                 'x-api-key': api_key})
    if res.status_code != 200:
        raise RuntimeError("query_anomaly_by_detection_config failed " + res.text)
    else:
        print("query_anomaly_by_detection_config success " + res.text)
    return json.loads(res.content)


def query_incident_by_detection_config(endpoint, subscription_key, api_key, detection_config_id, start_time, end_time):
    url = endpoint + '/enrichment/anomalyDetection/configurations/{}/incidents/query'.format(detection_config_id)
    body = {
        "startTime": start_time,
        "endTime": end_time,
        "filter": {
            "dimensionFilter": [
            ],
        }
    }
    res = requests.post(url, data=json.dumps(body),
                        headers={'Ocp-Apim-Subscription-Key': subscription_key,
                                 'x-api-key': api_key})
    if res.status_code != 200:
        raise RuntimeError("query_incident_by_detection_config failed " + res.text)
    else:
        print("query_incident_by_detection_config success " + res.text)
    return json.loads(res.content)


if __name__ == '__main__':
    endpoint = "https://[placeholder].cognitiveservices.azure.com/metricsadvisor/v1.0"
    subscription_key = "[your subscription key]"
    api_key = "[your api key]"

    '''
    First part
    1.onboard datafeed
    2.check datafeed latest status
    3.check datafeed status details
    4.create detection config
    5.create webhook
    6.create alert config
    '''
    datafeed_resource_url = add_data_feed(endpoint, subscription_key, api_key)
    print(datafeed_resource_url)

    # datafeed_id and metrics_id can get from datafeed_resource_url
    datafeed_info = json.loads(requests.get(url=datafeed_resource_url,
                                            headers={'Ocp-Apim-Subscription-Key': subscription_key,
                                                     'x-api-key': api_key}).content)
    print(datafeed_info)
    datafeed_id = datafeed_info['dataFeedId']
    metrics_id = []
    for metrics in datafeed_info['metrics']:
        metrics_id.append(metrics['metricId'])
    time.sleep(60)

    check_ingestion_latest_status(endpoint, subscription_key, api_key, datafeed_id)

    check_ingestion_detail_status(endpoint, subscription_key, api_key, datafeed_id,
                                  "2020-06-01T00:00:00Z", "2020-07-01T00:00:00Z")

    detection_config_resource_url = create_detection_config(endpoint, subscription_key, api_key, metrics_id[0])
    print(detection_config_resource_url)

    # anomaly_detection_configuration_id can get from detection_config_resource_url
    detection_config = json.loads(requests.get(url=detection_config_resource_url,
                                               headers={'Ocp-Apim-Subscription-Key': subscription_key,
                                                        'x-api-key': api_key}).content)
    print(detection_config)
    anomaly_detection_configuration_id = detection_config['anomalyDetectionConfigurationId']

    webhook_resource_url = create_web_hook(endpoint, subscription_key, api_key)
    print(webhook_resource_url)

    # hook_id can get from webhook_resource_url
    webhook = json.loads(requests.get(url=webhook_resource_url,
                                      headers={'Ocp-Apim-Subscription-Key': subscription_key,
                                               'x-api-key': api_key}).content)
    print(webhook)
    hook_id = webhook['hookId']

    alert_config_resource_url = create_alert_config(endpoint, subscription_key, api_key,
                                                    anomaly_detection_configuration_id, hook_id)

    # anomaly_alerting_configuration_id can get from alert_config_resource_url
    alert_config = json.loads(requests.get(url=alert_config_resource_url,
                                           headers={'Ocp-Apim-Subscription-Key': subscription_key,
                                                    'x-api-key': api_key}).content)
    print(alert_config)
    anomaly_alerting_configuration_id = alert_config['anomalyAlertingConfigurationId']

    '''
    Second part
    after onboard everything, ingest data to pipeline and detect anomaly need a few minute, after that, 
    you may get alert from web hook callback or you can also query alert under alert config.
    1.query alert under alert config
    2.query anomalys by alert id
    3.query incidents by alert id
    4.query root cause by incident id
    '''

    # alert id can get from your webhook callback function
    # alert id also can get from query_alert_by_alert_config
    while True:
        alert_ids = query_alert_by_alert_config(endpoint, subscription_key, api_key, anomaly_alerting_configuration_id,
                                                "2020-08-01T00:00:00Z", "2020-09-01T00:00:00Z")
        if len(alert_ids) == 0:
            print("no alert, please wait for a minute.")
            time.sleep(60)
        else:
            break
    alert_id = alert_ids[0]
    anomalys = query_anomaly_by_alert(endpoint, subscription_key, api_key, anomaly_alerting_configuration_id,
                                      alert_id)

    incidents = query_incident_by_alert(endpoint, subscription_key, api_key, anomaly_alerting_configuration_id,
                                        alert_id)

    incident_ids = [item['incidentId'] for item in incidents['value']]

    root_cause = query_root_cause_by_incident(endpoint, subscription_key, api_key, anomaly_detection_configuration_id,
                                              incident_ids[0])

    '''
    Third part
    you can also query anomaly and incident under detection config
    1. query anomalys under anomaly detection config
    2. query incidents under anomaly detection config
    '''

    anomalys = query_anomaly_by_detection_config(endpoint, subscription_key, api_key,
                                                 anomaly_detection_configuration_id,
                                                 "2020-06-01T00:00:00Z", "2020-09-01T00:00:00Z")

    incidents = query_incident_by_detection_config(endpoint, subscription_key, api_key,
                                                   anomaly_detection_configuration_id,
                                                   "2020-06-01T00:00:00Z", "2020-09-01T00:00:00Z")
