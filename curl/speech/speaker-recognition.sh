# Text-dependent verification

# <tdv_list_profiles>
curl --location --request GET 'INSERT_ENDPOINT_HERE/speaker-recognition/verification/text-dependent/profiles?api-version=2021-09-05' \
--header 'Ocp-Apim-Subscription-Key: INSERT_SUBSCRIPTION_KEY_HERE'
# </tdv_list_profiles>

# <tdv_create_profile>
# Note Change locale if needed.
curl --location --request POST 'INSERT_ENDPOINT_HERE/speaker-recognition/verification/text-dependent/profiles?api-version=2021-09-05' \
--header 'Ocp-Apim-Subscription-Key: INSERT_SUBSCRIPTION_KEY_HERE' \
--header 'Content-Type: application/json' \
--data-raw '{
    '\''locale'\'':'\''en-us'\''
}'
# </tdv_create_profile>

# <tdv_create_profile_response>
{
    "remainingEnrollmentsCount": 3,
    "locale": "en-us",
    "createdDateTime": "2020-09-29T14:54:29.683Z",
    "enrollmentStatus": "Enrolling",
    "modelVersion": null,
    "profileId": "714ce523-de76-4220-b93f-7c1cc1882d6e",
    "lastUpdatedDateTime": null,
    "enrollmentsCount": 0,
    "enrollmentsLength": 0.0,
    "enrollmentSpeechLength": 0.0
}
# </tdv_create_profile_response>

# <tdv_enroll>
curl --location --request POST 'INSERT_ENDPOINT_HERE/speaker-recognition/verification/text-dependent/profiles/INSERT_PROFILE_ID_HERE/enrollments?api-version=2021-09-05' \
--header 'Ocp-Apim-Subscription-Key: INSERT_SUBSCRIPTION_KEY_HERE' \
--header 'Content-Type: audio/wav' \
--data-binary @'INSERT_FILE_PATH_HERE'
# </tdv_enroll>

# <tdv_enroll_response_1>
{
    "remainingEnrollmentsCount": 2,
    "passPhrase": "my voice is my passport verify me",
    "profileId": "714ce523-de76-4220-b93f-7c1cc1882d6e",
    "enrollmentStatus": "Enrolling",
    "enrollmentsCount": 1,
    "enrollmentsLength": 3.5,
    "enrollmentsSpeechLength": 2.88,
    "audioLength": 3.5,
    "audioSpeechLength": 2.88
}
# </tdv_enroll_response_1>

# Just send the same request two more times.
# <tdv_enroll_response_2>
{
    "remainingEnrollmentsCount": 0,
    "passPhrase": "my voice is my passport verify me",
    "profileId": "714ce523-de76-4220-b93f-7c1cc1882d6e",
    "enrollmentStatus": "Enrolled",
    "enrollmentsCount": 3,
    "enrollmentsLength": 10.5,
    "enrollmentsSpeechLength": 8.64,
    "audioLength": 3.5,
    "audioSpeechLength": 2.88
}
# </tdv_enroll_response_2>

# <tdv_verify>
curl --location --request POST 'INSERT_ENDPOINT_HERE/speaker-recognition/verification/text-dependent/profiles/INSERT_PROFILE_ID_HERE:verify?api-version=2021-09-05' \
--header 'Ocp-Apim-Subscription-Key: INSERT_SUBSCRIPTION_KEY_HERE' \
--header 'Content-Type: audio/wav' \
--data-binary @'INSERT_FILE_PATH_HERE'
# </tdv_verify>

# <tdv_verify_response>
{
    "recognitionResult": "Accept",
    "score": 1.0
}
# </tdv_verify_response>

# <tdv_delete_profile>
curl --location --request DELETE \
'INSERT_ENDPOINT_HERE/speaker-recognition/verification/text-dependent/profiles/INSERT_PROFILE_ID_HERE?api-version=2021-09-05' \
--header 'Ocp-Apim-Subscription-Key: INSERT_SUBSCRIPTION_KEY_HERE'
# </tdv_delete_profile>
# 204 (No Content)

# Text-independent verification

# <tdv_list_profiles>
curl --location --request GET 'INSERT_ENDPOINT_HERE/speaker-recognition/verification/text-independent/profiles?api-version=2021-09-05' \
--header 'Ocp-Apim-Subscription-Key: INSERT_SUBSCRIPTION_KEY_HERE'
# </tdv_list_profiles>

# <tiv_create_profile>
curl --location --request POST 'INSERT_ENDPOINT_HERE/speaker-recognition/verification/text-independent/profiles?api-version=2021-09-05' \
--header 'Ocp-Apim-Subscription-Key: INSERT_SUBSCRIPTION_KEY_HERE' \
--header 'Content-Type: application/json' \
--data-raw '{
    '\''locale'\'':'\''en-us'\''
}'
# </tiv_create_profile>

# <tiv_create_profile_response>
{
    "profileStatus": "Inactive",
    "remainingEnrollmentsSpeechLength": 20.0,
    "profileId": "3f85dca9-ffc9-4011-bf21-37fad2beb4d2",
    "locale": "en-us",
    "enrollmentStatus": "Enrolling",
    "createdDateTime": "2020-09-29T16:08:52.409Z",
    "lastUpdatedDateTime": null,
    "enrollmentsCount": 0,
    "enrollmentsLength": 0.0,
    "enrollmentSpeechLength": 0.0
    "modelVersion": null,
}
# </tiv_create_profile_response>

# <tiv_pass_phrase>
# Note Change locale if needed.
curl --location --request POST 'INSERT_ENDPOINT_HERE/speaker-recognition/verification/text-independent/phrases/en-us?api-version=2021-09-05' \
--header 'Ocp-Apim-Subscription-Key: INSERT_SUBSCRIPTION_KEY_HERE'
# </tiv_pass_phrase

# <tiv_pass_phrase_response>
# Must use phrase as the first enrollment phrase
{
    "value": [
        {
            "activationPhrase": "I'll talk for a few seconds so you can recognize my voice in the future."
        }
    ]
}
# </tiv_pass_phrase_response>

# <tiv_enroll>
curl --location --request POST 'INSERT_ENDPOINT_HERE/speaker-recognition/verification/text-independent/profiles/INSERT_PROFILE_ID_HERE/enrollments?api-version=2021-09-05' \
--header 'Ocp-Apim-Subscription-Key: INSERT_SUBSCRIPTION_KEY_HERE' \
--header 'Content-Type: audio/wav' \
--data-binary @'INSERT_FILE_PATH_HERE'
# </tiv_enroll>

# <tiv_enroll_response>
{
    "remainingEnrollmentsSpeechLength": 0.0,
    "profileId": "3f85dca9-ffc9-4011-bf21-37fad2beb4d2",
    "enrollmentStatus": "Enrolled",
    "enrollmentsCount": 1,
    "enrollmentsLength": 33.16,
    "enrollmentsSpeechLength": 29.21,
    "audioLength": 33.16,
    "audioSpeechLength": 29.21
}
# </tiv_enroll_response>

# <tiv_verify>
curl --location --request POST 'INSERT_ENDPOINT_HERE/speaker-recognition/verification/text-independent/profiles/INSERT_PROFILE_ID_HERE:verify?api-version=2021-09-05' \
--header 'Ocp-Apim-Subscription-Key: INSERT_SUBSCRIPTION_KEY_HERE' \
--header 'Content-Type: audio/wav' \
--data-binary @'INSERT_FILE_PATH_HERE'
# </tiv_verify>

# <tiv_verify_response>
{
    "recognitionResult": "Accept",
    "score": 0.9196669459342957
}
# </tiv_verify_response>

# <tiv_delete_profile>
curl --location --request DELETE 'INSERT_ENDPOINT_HERE/speaker-recognition/verification/text-independent/profiles/INSERT_PROFILE_ID_HERE?api-version=2021-09-05' \
--header 'Ocp-Apim-Subscription-Key: INSERT_SUBSCRIPTION_KEY_HERE'
# </tiv_delete_profile>
# 204 (No Content)

# Text-independent identification

# <tii_list_profiles>
curl --location --request GET 'INSERT_ENDPOINT_HERE/speaker-recognition/identification/text-independent/profiles?api-version=2021-09-05' \
--header 'Ocp-Apim-Subscription-Key: INSERT_SUBSCRIPTION_KEY_HERE'
# </tii_list_profiles>

# <tii_create_profile>
# Note Change locale if needed.
curl --location --request POST 'INSERT_ENDPOINT_HERE/speaker-recognition/identification/text-independent/profiles?api-version=2021-09-05' \
--header 'Ocp-Apim-Subscription-Key: INSERT_SUBSCRIPTION_KEY_HERE' \
--header 'Content-Type: application/json' \
--data-raw '{
    '\''locale'\'':'\''en-us'\''
}'
# </tii_create_profile>

# <tii_create_profile_response>
{
    "profileStatus": "Inactive",
    "remainingEnrollmentsSpeechLengthInSec": 20.0,
    "profileId": "de99ab38-36c8-4b82-b137-510907c61fe8",
    "locale": "en-us",
    "enrollmentStatus": "Enrolling",
    "createdDateTime": "2020-09-22T17:25:48.642Z",
    "lastUpdatedDateTime": null,
    "enrollmentsCount": 0,
    "enrollmentsLengthInSec": 0.0,
    "enrollmentsSpeechLengthInSec": 0.0,
    "modelVersion": null
}
# </tii_create_profile_response>

# <tii_enroll>
curl --location --request POST 'INSERT_ENDPOINT_HERE/speaker-recognition/identification/text-independent/profiles/INSERT_PROFILE_ID_HERE/enrollments?api-version=2021-09-05' \
--header 'Ocp-Apim-Subscription-Key: INSERT_SUBSCRIPTION_KEY_HERE' \
--header 'Content-Type: audio/wav' \
--data-binary @'INSERT_FILE_PATH_HERE'
# </tii_enroll>

# <tii_enroll_response_1>
{
    "remainingEnrollmentsSpeechLength": 17.259999999999998,
    "profileId": "de99ab38-36c8-4b82-b137-510907c61fe8",
    "enrollmentStatus": "Enrolling",
    "enrollmentsCount": 1,
    "enrollmentsLength": 3.53,
    "enrollmentsSpeechLength": 2.74,
    "audioLength": 3.53,
    "audioSpeechLength": 2.74
}
# </tii_enroll_response_1>

# After calling get profiles again:
# <tii_list_profiles>
{
    "value": [
        {
            "remainingEnrollmentsSpeechLength": 17.259999999999998,
            "locale": "en-us",
            "createdDateTime": "2020-09-22T17:25:48.642Z",
            "enrollmentStatus": "Enrolling",
            "modelVersion": "2019-11-01",
            "profileId": "de99ab38-36c8-4b82-b137-510907c61fe8",
            "lastUpdatedDateTime": "09/22/2020 18:03:02",
            "enrollmentsCount": 0,
            "enrollmentsLength": 0.0,
            "enrollmentSpeechLength": 2.74
        }
    ],
    "@nextLink": ""
}
# </tii_list_profiles>

# After enrolling enough audio:
# <tii_enroll_response_2>
{
	"remainingEnrollmentsSpeechLength": 0.0,
	"profileId": "de99ab38-36c8-4b82-b137-510907c61fe8",
	"enrollmentStatus": "Enrolled",
	"enrollmentsCount": 2,
	"enrollmentsLength": 36.69,
	"enrollmentsSpeechLength": 31.95,
	"audioLength": 33.16,
	"audioSpeechLength": 29.21
}
# </tii_enroll_response_2>

# <tii_identify>
# Profile ids comma seperated list
curl --location --request POST 'INSERT_ENDPOINT_HERE/speaker-recognition/identification/text-independent/profiles:identifySingleSpeaker?api-version=2021-09-05&profileIds=INSERT_PROFILE_ID_HERE' \
--header 'Ocp-Apim-Subscription-Key: INSERT_SUBSCRIPTION_KEY_HERE' \
--header 'Content-Type: audio/wav' \
--data-binary @'INSERT_FILE_PATH_HERE'
# </tii_identify>

# <tii_identify_response_not_enrolled>
If we have not added enough speech samples:
{
    "error": {
        "code": "InvalidRequest",
        "message": "Profile is not enrolled."
    }
}
# </tii_identify_response_not_enrolled>

# <tii_identify_response_not_enough_audio>
If audio is not long enough:
{
    "error": {
        "code": "InvalidRequest",
        "message": "Invalid audio length. Minimum allowed length is 4 second(s)."
    }
}
# </tii_identify_response_not_enough_audio>

# <tii_identify_response>
Success:
{
    "identifiedProfile": {
        "profileId": "de99ab38-36c8-4b82-b137-510907c61fe8",
        "score": 0.9083486
    },
    "profilesRanking": [
        {
            "profileId": "de99ab38-36c8-4b82-b137-510907c61fe8",
            "score": 0.9083486
        }
    ]
}
# </tii_identify_response>

# <tii_delete_profile>
curl --location --request DELETE \
'INSERT_ENDPOINT_HERE/speaker-recognition/identification/text-independent/profiles/INSERT_PROFILE_ID_HERE?api-version=2021-09-05' \
--header 'Ocp-Apim-Subscription-Key: INSERT_SUBSCRIPTION_KEY_HERE'
# </tii_delete_profile>
# 204 (No Content)
