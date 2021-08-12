// <environment>
'use strict';

const axios = require('axios').default;

let subscriptionKey = 'PASTE_YOUR_FACE_SUBSCRIPTION_KEY_HERE'
let endpoint = 'PASTE_YOUR_FACE_ENDPOINT_HERE' + '/face/v1.0/detect'

// Optionally, replace with your own image URL (for example a .jpg or .png URL).
let imageUrl = 'https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/faces.jpg'
// </environment>

// <main>
// Send a POST request
axios({
    method: 'post',
    url: endpoint,
    params : {
		detectionModel: 'detection_03',
        returnFaceId: true
    },
    data: {
        url: imageUrl,
    },
    headers: { 'Ocp-Apim-Subscription-Key': subscriptionKey }
}).then(function (response) {
    console.log('Status text: ' + response.status)
    console.log('Status text: ' + response.statusText)
    console.log()
    console.log(response.data)
}).catch(function (error) {
    console.log(error)
});
// </main>
