// <dependencies>
"use strict";

/* To run this sample, install:
npm install microsoft-cognitiveservices-speech-sdk
*/
var sdk = require("microsoft-cognitiveservices-speech-sdk");
var fs = require("fs");

// Note: Change the locale if desired.
const profile_locale = "en-us";

/* Note: passphrase_files and verify_file should contain paths to audio files that contain \"My voice is my passport, verify me.\"
You can obtain these files from:
https://github.com/Azure-Samples/cognitive-services-speech-sdk/tree/fa6428a0837779cbeae172688e0286625e340942/quickstart/javascript/node/speaker-recognition/verification
*/ 
const passphrase_files = ["myVoiceIsMyPassportVerifyMe01.wav", "myVoiceIsMyPassportVerifyMe02.wav", "myVoiceIsMyPassportVerifyMe03.wav"];
const verify_file = "myVoiceIsMyPassportVerifyMe04.wav";
/* Note: identify_file should contain a path to an audio file that uses the same voice as the other files, but contains different speech. You can obtain this file from:
https://github.com/Azure-Samples/cognitive-services-speech-sdk/tree/fa6428a0837779cbeae172688e0286625e340942/quickstart/javascript/node/speaker-recognition/identification
*/
const identify_file = "aboutSpeechSdk.wav";

var subscription_key = 'PASTE_YOUR_SPEECH_SUBSCRIPTION_KEY_HERE';
var region = 'PASTE_YOUR_SPEECH_ENDPOINT_REGION_HERE';

const ticks_per_second = 10000000;
// </dependencies>

// <helpers>
/* From: https://github.com/Azure-Samples/cognitive-services-speech-sdk/blob/fa6428a0837779cbeae172688e0286625e340942/quickstart/javascript/node/speaker-recognition/verification/dependent-verification.js#L8
*/
function GetAudioConfigFromFile (file)
{
	return sdk.AudioConfig.fromWavFileInput(fs.readFileSync(file));
}
// </helpers>

// <get_activation_phrases>
async function GetActivationPhrases(client, type, locale)
{
	try {
		console.log ("Getting Activation Phrases for enrollment");
		const result = await client.getActivationPhrasesAsync(type, locale);
		if (!!result.phrases && result.phrases.length > 0) {
			for (const phrase of result.phrases) {
				console.log (`Activation phrase received: ${phrase}\n`);
			}
		} else {
			console.log ("No activation phrases received.\n");
		}
	} catch (error) {
		console.log ("Error getting activation phrases: " + error);
	}
}
// </get_activation_phrases>

// <add_enrollments_dependent>
async function AddEnrollmentsToTextDependentProfile(client, profile, audio_files)
{
	try {
		for (const file of audio_files) {
			console.log ("Adding enrollment to text dependent profile...");
			const audio_config = GetAudioConfigFromFile(file);
			const result = await client.enrollProfileAsync(profile, audio_config);
			if (result.reason === sdk.ResultReason.Canceled) {
				throw(JSON.stringify(sdk.VoiceProfileEnrollmentCancellationDetails.fromResult(result)));
			}
			else {
				console.log ("Remaining enrollments needed: " + result.privDetails["remainingEnrollmentsCount"] + ".");
			}
		};
		console.log ("Enrollment completed.\n");
	} catch (error) {
		console.log ("Error adding enrollments: " + error);
	}
}
// </add_enrollments_dependent>

// <add_enrollments_independent>
async function AddEnrollmentsToTextIndependentProfile(client, profile, audio_files)
{
	try {
		for (const file of audio_files) {
			console.log ("Adding enrollment to text independent profile...");
			const audio_config = GetAudioConfigFromFile(file);
			const result = await client.enrollProfileAsync (profile, audio_config);
			if (result.reason === sdk.ResultReason.Canceled) {
				throw(JSON.stringify(sdk.VoiceProfileEnrollmentCancellationDetails.fromResult(result)));
			}
			else {
				console.log ("Remaining audio time needed: " + (result.privDetails["remainingEnrollmentsSpeechLength"] / ticks_per_second) + " seconds.");
			}
		}
		console.log ("Enrollment completed.\n");
	} catch (error) {
		console.log ("Error adding enrollments: " + error);
	}
}
// </add_enrollments_independent>

// <speaker_verify>
async function SpeakerVerify(profile, recognizer)
{
	try {
		const model = sdk.SpeakerVerificationModel.fromProfile(profile);
		const result = await recognizer.recognizeOnceAsync(model);
		console.log ("Verified voice profile for speaker: " + result.profileId + ". Score is: " + result.score + ".\n");
	} catch (error) {
		console.log ("Error verifying speaker: " + error);
	}
}
// </speaker_verify>

// <speaker_identify>
async function SpeakerIdentify(profile, recognizer)
{
	try {
		const model = sdk.SpeakerIdentificationModel.fromProfiles([profile]);
		const result = await recognizer.recognizeOnceAsync(model);
		console.log ("The most similar voice profile is: " + result.profileId + " with similarity score: " + result.score + ".\n");
	} catch (error) {
		console.log ("Error identifying speaker: " + error);
	}
}
// </speaker_identify>

// <text_dependent_verification>
async function TextDependentVerification(client, speech_config)
{
	console.log ("Text Dependent Verification:\n");
	var profile = null;
	try {
		const type = sdk.VoiceProfileType.TextDependentVerification;
		// Create the profile.
		profile = await client.createProfileAsync(type, profile_locale);
		console.log ("Created profile ID: " + profile.profileId);
		// Get the activation phrases
		await GetActivationPhrases(type, profile_locale);
		await AddEnrollmentsToTextDependentProfile(client, profile, passphrase_files);
		const audio_config = GetAudioConfigFromFile(verify_file);
		const recognizer = new sdk.SpeakerRecognizer(speech_config, audio_config);
		await SpeakerVerify(profile, recognizer);
	}
	catch (error) {
		console.log ("Error:\n" + error);
	}
	finally {
		if (profile !== null) {
			console.log ("Deleting profile ID: " + profile.profileId);
			const deleteResult = await client.deleteProfileAsync (profile);
		}
	}
}
// </text_dependent_verification>

// <text_independent_verification>
async function TextIndependentVerification(client, speech_config)
{
	console.log ("Text Independent Verification:\n");
	var profile = null;
	try {
		const type = sdk.VoiceProfileType.TextIndependentVerification;
		// Create the profile.
		profile = await client.createProfileAsync(type, profile_locale);
		console.log ("Created profile ID: " + profile.profileId);
		// Get the activation phrases
		await GetActivationPhrases(type, profile_locale);
		await AddEnrollmentsToTextIndependentProfile(client, profile, [identify_file]);
		const audio_config = GetAudioConfigFromFile(passphrase_files[0]);
		const recognizer = new sdk.SpeakerRecognizer(speech_config, audio_config);
		await SpeakerVerify(profile, recognizer);
	}
	catch (error) {
		console.log ("Error:\n" + error);
	}
	finally {
		if (profile !== null) {
			console.log ("Deleting profile ID: " + profile.profileId);
			const deleteResult = await client.deleteProfileAsync (profile);
		}
	}
}
// </text_independent_verification>

// <text_independent_indentification>
async function TextIndependentIdentification(client, speech_config)
{
	console.log ("Text Independent Identification:\n");
	var profile = null;
	try {
		const type = sdk.VoiceProfileType.TextIndependentIdentification;
		// Create the profile.
		profile = await client.createProfileAsync(type, profile_locale);
		console.log ("Created profile ID: " + profile.profileId);
		// Get the activation phrases
		await GetActivationPhrases(type, profile_locale);
		await AddEnrollmentsToTextIndependentProfile(client, profile, [identify_file]);
		const audio_config = GetAudioConfigFromFile(passphrase_files[0]);
		const recognizer = new sdk.SpeakerRecognizer(speech_config, audio_config);
		await SpeakerIdentify(profile, recognizer);
	}
	catch (error) {
		console.log ("Error:\n" + error);
	}
	finally {
		if (profile !== null) {
			console.log ("Deleting profile ID: " + profile.profileId);
			const deleteResult = await client.deleteProfileAsync (profile);
		}
	}
}
// </text_independent_indentification>

// <main>
async function main() {
	const speech_config = sdk.SpeechConfig.fromSubscription(subscription_key, region);
	const client = new sdk.VoiceProfileClient(speech_config);

	await TextDependentVerification(client, speech_config);
	await TextIndependentVerification(client, speech_config);
	await TextIndependentIdentification(client, speech_config);
	console.log ("End of quickstart.");
}
main();
// </main>
