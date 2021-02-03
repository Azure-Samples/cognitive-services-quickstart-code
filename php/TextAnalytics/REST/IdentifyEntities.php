<?php

// NOTE: Be sure to uncomment the following line in your php.ini file.
// ;extension=php_openssl.dll
// You might need to set the full path, for example:
// extension="C:\Program Files\Php\ext\php_openssl.dll"
$subscription_key = "<paste-your-text-analytics-key-here>";
$endpoint = "<paste-your-text-analytics-endpoint-here>";

$path = '/text/analytics/v3.0/entities/recognition/general';

function GetEntities ($host, $path, $key, $data) {

    $headers = "Content-type: text/json\r\n" .
        "Content-Length: " . strlen($data) . "\r\n" .
        "Ocp-Apim-Subscription-Key: $key\r\n";
    $data = json_encode ($data);

    // NOTE: Use the key 'http' even if you are making an HTTPS request. See:
    // https://php.net/manual/en/function.stream-context-create.php
    $options = array (
        'http' => array (
            'header' => $headers,
            'method' => 'POST',
            'content' => $data
        )
    );
    $context  = stream_context_create ($options);
    $result = file_get_contents ($host . $path, false, $context);
    return $result;
}

$data = array (
    'documents' => array (
        array ( 'id' => '1', 'language' => 'en', 'text' => 'Microsoft is and It company.' ),
    )
);

print "Please wait a moment for the results to appear.";

$result = GetEntities($endpoint, $path, $subscription_key, $data);

echo json_encode (json_decode ($result), JSON_PRETTY_PRINT);
?>