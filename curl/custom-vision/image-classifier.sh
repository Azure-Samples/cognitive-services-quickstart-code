# <createproject>
curl -v -X POST -H "Training-key: {subscription key}" "https://{endpoint}/customvision/v3.3/Training/projects?name={name}"
# </createproject>

# <createtag>
curl -v -X POST -H "Training-key: {subscription key}" "https://{endpoint}/customvision/v3.3/Training/projects/{projectId}/tags?name={name}"
# </createtag>

# <uploadimages>
curl -v -X POST -H "Content-Type: multipart/form-data" -H "Training-key: {subscription key}" "https://{endpoint}/customvision/v3.3/Training/projects/{projectId}/images?tagIds={tagArray}"
--data-ascii "{binary data}" 
# </uploadimages>

# <trainproject>
curl -v -X POST -H "Content-Type: application/json" -H "Training-key: {subscription key}" "https://{endpoint}/customvision/v3.3/Training/projects/{projectId}/train"
# </trainproject>

# <publish>
curl -v -X POST -H "Training-key: {subscription key}" "https://{endpoint}/customvision/v3.3/Training/projects/{projectId}/iterations/{iterationId}/publish?publishName={publishName}&predictionId={predictionId}"
# </publish>

# <predict>
curl -v -X POST -H "Content-Type: application/octet-stream" -H "Prediction-key: {subscription key}" "https://{endpoint}/customvision/v3.1/Prediction/{projectId}/classify/iterations/{publishedName}/image"
--data-ascii "{binary data}" 
# </predict>

