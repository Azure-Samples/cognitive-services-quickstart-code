'use strict';

const msRest = require("@azure/ms-rest-js");
const Face = require("@azure/cognitiveservices-face");
const uuid = require("uuid/v4");

key = "<paste-your-[product-name]-key-here>"
endpoint = "<paste-your-[product-name]-endpoint-here>"

// <credentials>
const credentials = new msRest.ApiKeyCredentials({ inHeader: { 'Ocp-Apim-Subscription-Key': key } });
const client = new Face.FaceClient(credentials, endpoint);
// </credentials>

// <globals>
const image_base_url = "https://csdx.blob.core.windows.net/resources/Face/Images/";
const person_group_id = uuid();
// </globals>

// <helpers>
function sleep(ms) {
	return new Promise(resolve => setTimeout(resolve, ms));
}
// </helpers>

// <detect>
async function DetectFaceExtract() {
    console.log("========DETECT FACES========");
    console.log();

    // Create a list of images
	const image_file_names = [
		"detection1.jpg",    // single female with glasses
		// "detection2.jpg", // (optional: single man)
		// "detection3.jpg", // (optional: single male construction worker)
		// "detection4.jpg", // (optional: 3 people at cafe, 1 is blurred)
		"detection5.jpg",    // family, woman child man
		"detection6.jpg"     // elderly couple, male female
	];

// NOTE await does not work properly in for, forEach, and while loops. Use Array.map and Promise.all instead.
	await Promise.all (image_file_names.map (async function (image_file_name) {
        let detected_faces = await client.face.detectWithUrl(image_base_url + image_file_name,
			{
				returnFaceAttributes: ["Accessories","Age","Blur","Emotion","Exposure","FacialHair","Gender","Glasses","Hair","HeadPose","Makeup","Noise","Occlusion","Smile"],
				// We specify detection model 1 because we are retrieving attributes.
				detectionModel: "detection_01"
			});
        console.log (detected_faces.length + " face(s) detected from image " + image_file_name + ".");
		console.log("Face attributes for face(s) in " + image_file_name + ":");

// Parse and print all attributes of each detected face.
		detected_faces.forEach (async function (face) {
			// Get the bounding box of the face
			console.log("Bounding box:\n  Left: " + face.faceRectangle.left + "\n  Top: " + face.faceRectangle.top + "\n  Width: " + face.faceRectangle.width + "\n  Height: " + face.faceRectangle.height);

			// Get the accessories of the face
			let accessories = face.faceAttributes.accessories.join();
			if (0 === accessories.length) {
				console.log ("No accessories detected.");
			}
			else {
				console.log ("Accessories: " + accessories);
			}

			// Get face other attributes
			console.log("Age: " + face.faceAttributes.age);
			console.log("Blur: " + face.faceAttributes.blur.blurLevel);

			// Get emotion on the face
			let emotions = "";
			let emotion_threshold = 0.0;
			if (face.faceAttributes.emotion.anger > emotion_threshold) { emotions += "anger, "; }
			if (face.faceAttributes.emotion.contempt > emotion_threshold) { emotions += "contempt, "; }
			if (face.faceAttributes.emotion.disgust > emotion_threshold) { emotions +=  "disgust, "; }
			if (face.faceAttributes.emotion.fear > emotion_threshold) { emotions +=  "fear, "; }
			if (face.faceAttributes.emotion.happiness > emotion_threshold) { emotions +=  "happiness, "; }
			if (face.faceAttributes.emotion.neutral > emotion_threshold) { emotions +=  "neutral, "; }
			if (face.faceAttributes.emotion.sadness > emotion_threshold) { emotions +=  "sadness, "; }
			if (face.faceAttributes.emotion.surprise > emotion_threshold) { emotions +=  "surprise, "; }
			if (emotions.length > 0) {
				console.log ("Emotions: " + emotions.slice (0, -2));
			}
			else {
				console.log ("No emotions detected.");
			}
			
			// Get more face attributes
			console.log("Exposure: " + face.faceAttributes.exposure.exposureLevel);
			if (face.faceAttributes.facialHair.moustache + face.faceAttributes.facialHair.beard + face.faceAttributes.facialHair.sideburns > 0) {
				console.log("FacialHair: Yes");
			}
			else {
				console.log("FacialHair: No");
			}
			console.log("Gender: " + face.faceAttributes.gender);
			console.log("Glasses: " + face.faceAttributes.glasses);

			// Get hair color
			var color = "";
			if (face.faceAttributes.hair.hairColor.length === 0) {
				if (face.faceAttributes.hair.invisible) { color = "Invisible"; } else { color = "Bald"; }
			}
			else {
				color = "Unknown";
				var highest_confidence = 0.0;
				face.faceAttributes.hair.hairColor.forEach (function (hair_color) {
					if (hair_color.confidence > highest_confidence) {
						highest_confidence = hair_color.confidence;
						color = hair_color.color;
					}
				});
			}
			console.log("Hair: " + color);

			// Get more attributes
			console.log("Head pose:");
			console.log("  Pitch: " + face.faceAttributes.headPose.pitch);
			console.log("  Roll: " + face.faceAttributes.headPose.roll);
			console.log("  Yaw: " + face.faceAttributes.headPose.yaw);
 
			console.log("Makeup: " + ((face.faceAttributes.makeup.eyeMakeup || face.faceAttributes.makeup.lipMakeup) ? "Yes" : "No"));
			console.log("Noise: " + face.faceAttributes.noise.noiseLevel);

			console.log("Occlusion:");
			console.log("  Eye occluded: " + (face.faceAttributes.occlusion.eyeOccluded ? "Yes" : "No"));
			console.log("  Forehead occluded: " + (face.faceAttributes.occlusion.foreheadOccluded ? "Yes" : "No"));
			console.log("  Mouth occluded: " + (face.faceAttributes.occlusion.mouthOccluded ? "Yes" : "No"));

			console.log("Smile: " + face.faceAttributes.smile);
			console.log();
		});
	}));
}
// </detect>

// <recognize>
async function DetectFaceRecognize(url) {
    // Detect faces from image URL. Since only recognizing, use the recognition model 4.
    // We use detection model 3 because we are not retrieving attributes.
    let detected_faces = await client.face.detectWithUrl(url,
		{
			detectionModel: "detection_03",
			recognitionModel: "recognition_04"
		});
    return detected_faces;
}
// </recognize>

// <find_similar>
async function FindSimilar() {
    console.log("========FIND SIMILAR========");
    console.log();

	const source_image_file_name = "findsimilar.jpg";
    const target_image_file_names = [
		"Family1-Dad1.jpg",
		"Family1-Daughter1.jpg",
		"Family1-Mom1.jpg",
		"Family1-Son1.jpg",
		"Family2-Lady1.jpg",
		"Family2-Man1.jpg",
		"Family3-Lady1.jpg",
		"Family3-Man1.jpg"
	];

	let target_face_ids = (await Promise.all (target_image_file_names.map (async function (target_image_file_name) {
        // Detect faces from target image url.
        var faces = await DetectFaceRecognize(image_base_url + target_image_file_name);
		console.log(faces.length + " face(s) detected from image: " +  target_image_file_name + ".");
        return faces.map (function (face) { return face.faceId });;
	}))).flat();

    // Detect faces from source image url.
	let detected_faces = await DetectFaceRecognize(image_base_url + source_image_file_name);

    // Find a similar face(s) in the list of IDs. Comapring only the first in list for testing purposes.
    let results = await client.face.findSimilar(detected_faces[0].faceId, { faceIds : target_face_ids });
	results.forEach (function (result) {
		console.log("Faces from: " + source_image_file_name + " and ID: " + result.faceId + " are similar with confidence: " + result.confidence + ".");
	});
	console.log();
}
// </find_similar>

// <add_faces>
async function AddFacesToPersonGroup(person_dictionary, person_group_id) {
	console.log ("Adding faces to person group...");
	// The similar faces will be grouped into a single person group person.
	
	await Promise.all (Object.keys(person_dictionary).map (async function (key) {
		const value = person_dictionary[key];

		// Wait briefly so we do not exceed rate limits.
		await sleep (1000);

		let person = await client.personGroupPerson.create(person_group_id, { name : key });
		console.log("Create a person group person: " + key + ".");

		// Add faces to the person group person.
		await Promise.all (value.map (async function (similar_image) {
			console.log("Add face to the person group person: (" + key + ") from image: " + similar_image + ".");
			await client.personGroupPerson.addFaceFromUrl(person_group_id, person.personId, image_base_url + similar_image);
		}));
	}));

	console.log ("Done adding faces to person group.");
}
// </add_faces>

// <wait_for_training>
async function WaitForPersonGroupTraining(person_group_id) {
	// Wait so we do not exceed rate limits.
	console.log ("Waiting 10 seconds...");
	await sleep (10000);
	let result = await client.personGroup.getTrainingStatus(person_group_id);
	console.log("Training status: " + result.status + ".");
	if (result.status !== "succeeded") {
		await WaitForPersonGroupTraining(person_group_id);
	}
}
// </wait_for_training>

/* NOTE This function might not work with the free tier of the Face service
because it might exceed the rate limits. If that happens, try inserting calls
to sleep() between calls to the Face service.
*/
// <identify>
async function IdentifyInPersonGroup() {
    console.log("========IDENTIFY FACES========");
    console.log();

// Create a dictionary for all your images, grouping similar ones under the same key.
	const person_dictionary = {
		"Family1-Dad" : ["Family1-Dad1.jpg", "Family1-Dad2.jpg"],
		"Family1-Mom" : ["Family1-Mom1.jpg", "Family1-Mom2.jpg"],
		"Family1-Son" : ["Family1-Son1.jpg", "Family1-Son2.jpg"],
		"Family1-Daughter" : ["Family1-Daughter1.jpg", "Family1-Daughter2.jpg"],
		"Family2-Lady" : ["Family2-Lady1.jpg", "Family2-Lady2.jpg"],
		"Family2-Man" : ["Family2-Man1.jpg", "Family2-Man2.jpg"]
	};

    // A group photo that includes some of the persons you seek to identify from your dictionary.
    let source_image_file_name = "identification1.jpg";

	// Create a person group. 
	console.log("Creating a person group with ID: " + person_group_id);
	await client.personGroup.create(person_group_id, { name : person_group_id, recognitionModel : "recognition_04" });

	await AddFacesToPersonGroup(person_dictionary, person_group_id);

	// Start to train the person group.
	console.log();
	console.log("Training person group: " + person_group_id + ".");
	await client.personGroup.train(person_group_id);

	await WaitForPersonGroupTraining(person_group_id);
	console.log();

	// Detect faces from source image url.
	let face_ids = (await DetectFaceRecognize(image_base_url + source_image_file_name)).map (face => face.faceId);

// Identify the faces in a person group.
    let results = await client.face.identify(face_ids, { personGroupId : person_group_id});
	await Promise.all (results.map (async function (result) {
        let person = await client.personGroupPerson.get(person_group_id, result.candidates[0].personId);
        console.log("Person: " + person.name + " is identified for face in: " + source_image_file_name + " with ID: " + result.faceId + ". Confidence: " + result.candidates[0].confidence + ".");
	}));
    console.log();
}
// </identify>

// <main>
async function main() {
	await DetectFaceExtract();
	await FindSimilar();
	await IdentifyInPersonGroup();
	console.log ("Done.");
}
main();
// </main>
