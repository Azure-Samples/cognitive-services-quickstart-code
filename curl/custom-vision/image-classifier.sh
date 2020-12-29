# <createproject>
curl -v -X POST "https://{endpoint}/customvision/v3.3/Training/projects?name={name}"
-H "Training-key: {subscription key}"
# </createproject>

# <createtag>
curl -v -X POST "https://{endpoint}/customvision/v3.3/Training/projects/{projectId}/tags?name={name}"
-H "Training-key: {subscription key}"
# </createtag>

# <uploadimages>
curl -v -X POST "https://{endpoint}/customvision/v3.3/Training/projects/{projectId}/images?tagIds={tagArray}"
-H "Content-Type: multipart/form-data"
-H "Training-key: {subscription key}"
--data-ascii "{binary data}" 
# </uploadimages>

# <trainproject>
curl -v -X POST "https://{endpoint}/customvision/v3.3/Training/projects/{projectId}/train"
-H "Content-Type: application/json"
-H "Training-key: {subscription key}"
# </trainproject>

# <publish>
curl -v -X POST "https://{endpoint}/customvision/v3.3/Training/projects/{projectId}/iterations/{iterationId}/publish?publishName={publishName}&predictionId={predictionId}"
-H "Training-key: {subscription key}"
# </publish>

# <predict>
curl -v -X POST "https://{endpoint}/customvision/v3.1/Prediction/{projectId}/classify/iterations/{publishedName}/image"
-H "Content-Type: application/octet-stream"
-H "Prediction-key: {subscription key}"
--data-ascii "{binary data}" 
# </predict>

