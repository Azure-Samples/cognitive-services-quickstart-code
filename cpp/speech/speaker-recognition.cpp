// <dependencies>
#include <iostream>
#include <stdexcept>
// Note: Install the NuGet package Microsoft.CognitiveServices.Speech.
#include <speechapi_cxx.h>

using namespace std;
using namespace Microsoft::CognitiveServices::Speech;

// Note: Change the locale if desired.
auto profile_locale = "en-us";
auto audio_config = Audio::AudioConfig::FromDefaultMicrophoneInput();
auto ticks_per_second = 10000000;
// </dependencies>

// <get_speech_config>
shared_ptr<SpeechConfig> GetSpeechConfig()
{
	auto subscription_key = 'PASTE_YOUR_SPEECH_SUBSCRIPTION_KEY_HERE';
	auto region = 'PASTE_YOUR_SPEECH_ENDPOINT_REGION_HERE';
	auto config = SpeechConfig::FromSubscription(subscription_key, region);
	return config;
}
// </get_speech_config>

// <add_enrollments_dependent>
void AddEnrollmentsToTextDependentProfile(shared_ptr<VoiceProfileClient> client, shared_ptr<VoiceProfile> profile)
{
	shared_ptr<VoiceProfileEnrollmentResult> enroll_result = nullptr;
	auto phraseResult = client->GetActivationPhrasesAsync(profile->GetType(), profile_locale).get();
    auto phrases = phraseResult->GetPhrases();
	while (enroll_result == nullptr || enroll_result->GetEnrollmentInfo(EnrollmentInfoType::RemainingEnrollmentsCount) > 0)
	{
		if (phrases != nullptr && phrases->size() > 0)
		{
			std::cout << "Please say the passphrase, \"" << phrases->at(0) << "\"\n";
			enroll_result = client->EnrollProfileAsync(profile, audio_config).get();
			std::cout << "Remaining enrollments needed: " << enroll_result->GetEnrollmentInfo(EnrollmentInfoType::RemainingEnrollmentsCount) << ".\n";
		}
		else
		{
			std::cout << "No passphrases received, enrollment not attempted.\n\n";
		}
	}
	std::cout << "Enrollment completed.\n\n";
}
// </add_enrollments_dependent>

// <add_enrollments_independent>
void AddEnrollmentsToTextIndependentProfile(shared_ptr<VoiceProfileClient> client, shared_ptr<VoiceProfile> profile)
{
	shared_ptr<VoiceProfileEnrollmentResult> enroll_result = nullptr;
	auto phraseResult = client->GetActivationPhrasesAsync(profile->GetType(), profile_locale).get();
    auto phrases = phraseResult->GetPhrases();
	while (enroll_result == nullptr || enroll_result->GetEnrollmentInfo(EnrollmentInfoType::RemainingEnrollmentsSpeechLength) > 0)
	{
		if (phrases != nullptr && phrases->size() > 0)
		{
			std::cout << "Please say the activation phrase, \"" << phrases->at(0) << "\"\n";
			enroll_result = client->EnrollProfileAsync(profile, audio_config).get();
			std::cout << "Remaining audio time needed: " << enroll_result->GetEnrollmentInfo(EnrollmentInfoType::RemainingEnrollmentsSpeechLength) / ticks_per_second << " seconds.\n";
		}
		else
		{
			std::cout << "No activation phrases received, enrollment not attempted.\n\n";
		}
	}
	std::cout << "Enrollment completed.\n\n";
}
// </add_enrollments_independent>

// <speaker_verify>
void SpeakerVerify(shared_ptr<VoiceProfile> profile, shared_ptr<SpeakerRecognizer> recognizer)
{
	shared_ptr<SpeakerVerificationModel> model = SpeakerVerificationModel::FromProfile(profile);
	std::cout << "Speak the passphrase to verify: \"My voice is my passport, verify me.\"\n";
	shared_ptr<SpeakerRecognitionResult> result = recognizer->RecognizeOnceAsync(model).get();
	std::cout << "Verified voice profile for speaker: " << result->ProfileId << ". Score is: " << result->GetScore() << ".\n\n";
}
// </speaker_verify>

// <speaker_identify>
void SpeakerIdentify(shared_ptr<VoiceProfile> profile, shared_ptr<SpeakerRecognizer> recognizer)
{
	shared_ptr<SpeakerIdentificationModel> model = SpeakerIdentificationModel::FromProfiles({ profile });
	// Note: We need at least four seconds of audio after pauses are subtracted.
	std::cout << "Please speak for at least ten seconds to identify who it is from your list of enrolled speakers.\n";
	shared_ptr<SpeakerRecognitionResult> result = recognizer->RecognizeOnceAsync(model).get();
	std::cout << "The most similar voice profile is: " << result->ProfileId << " with similarity score: " << result->GetScore() << ".\n\n";
}
// </speaker_identify>

// <text_dependent_verification>
void TextDependentVerification(shared_ptr<VoiceProfileClient> client, shared_ptr<SpeakerRecognizer> recognizer)
{
	std::cout << "Text Dependent Verification:\n\n";
	// Create the profile.
	auto profile = client->CreateProfileAsync(VoiceProfileType::TextDependentVerification, profile_locale).get();
	std::cout << "Created profile ID: " << profile->GetId() << "\n";
	AddEnrollmentsToTextDependentProfile(client, profile);
	SpeakerVerify(profile, recognizer);
	// Delete the profile.
	client->DeleteProfileAsync(profile);
}
// </text_dependent_verification>

// <text_independent_verification>
void TextIndependentVerification(shared_ptr<VoiceProfileClient> client, shared_ptr<SpeakerRecognizer> recognizer)
{
	std::cout << "Text Independent Verification:\n\n";
	// Create the profile.
	auto profile = client->CreateProfileAsync(VoiceProfileType::TextIndependentVerification, profile_locale).get();
	std::cout << "Created profile ID: " << profile->GetId() << "\n";
	AddEnrollmentsToTextIndependentProfile(client, profile);
	SpeakerVerify(profile, recognizer);
	// Delete the profile.
	client->DeleteProfileAsync(profile);
}
// </text_independent_verification>

// <text_independent_indentification>
void TextIndependentIdentification(shared_ptr<VoiceProfileClient> client, shared_ptr<SpeakerRecognizer> recognizer)
{
	std::cout << "Speaker Identification:\n\n";
	// Create the profile.
	auto profile = client->CreateProfileAsync(VoiceProfileType::TextIndependentIdentification, profile_locale).get();
	std::cout << "Created profile ID: " << profile->GetId() << "\n";
	AddEnrollmentsToTextIndependentProfile(client, profile);
	SpeakerIdentify(profile, recognizer);
	// Delete the profile.
	client->DeleteProfileAsync(profile);
}
// </text_independent_indentification>

// <main>
int main()
{
	auto speech_config = GetSpeechConfig();
	auto client = VoiceProfileClient::FromConfig(speech_config);
	auto recognizer = SpeakerRecognizer::FromConfig(speech_config, audio_config);
	TextDependentVerification(client, recognizer);
	TextIndependentVerification(client, recognizer);
	TextIndependentIdentification(client, recognizer);
    std::cout << "End of quickstart.\n";
}
// </main>
