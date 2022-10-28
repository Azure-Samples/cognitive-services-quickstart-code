import * as sdk from "microsoft-cognitiveservices-speech-sdk";

const key = "REPLACE-WITH-YOUR-KEY";
const region = "REPLACE-WITH-YOUR-REGION";

const fileFormat = 3; // audio-16khz-32kbitrate-mono-mp3
const fileName = "text-to-speech-file.mp3";

const text = "A simple test to write to a file.";

function synthesizeSpeechDocs() {
    const speechConfig = sdk.SpeechConfig.fromSubscription(key, region);
    speechConfig.speechSynthesisOutputFormat = fileFormat; 
    const audioConfig = sdk.AudioConfig.fromAudioFileOutput(fileName);

    const synthesizer = new sdk.SpeechSynthesizer(speechConfig, audioConfig);
    synthesizer.speakTextAsync(
        text,
        result => {
            if (result) {
                // result = {"privResultId":"BADBF73893EF4575AEB907E98EF36350","privReason":9,"privAudioData":{}}
                console.log(JSON.stringify(result));
            }
            synthesizer.close();
        },
        error => {
            console.log(error);
            synthesizer.close();
        });
}

synthesizeSpeechDocs();