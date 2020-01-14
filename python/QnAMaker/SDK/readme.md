# Create, update, publish, delete a knowledge base

## To use this sample

1. Create QnA Maker resource in Azure portal.
1. Get resource's key and host and set these values in the environment variables `QNAMAKER_KEY` and `QNAMAKER_HOST`.
1. Install dependency with `py -m pip install azure-cognitiveservices-knowledge-qnamaker`.
1. Run sample with `py -3 knowledgebase_quickstart.py`.

## Sample output

```console
PS C:\samples\cognitive-services-qnamaker-python\documentation-samples\quickstarts\knowledgebase_quickstart> py -3 knowledgebase_quickstart.py
Waiting for operation: a423a0b5-9f85-4b2d-972b-abd95122fea8 to complete.
Waiting for operation: a423a0b5-9f85-4b2d-972b-abd95122fea8 to complete.
Created KB with ID: 8320348f-5896-4a98-b66d-f32787d02cb9
Waiting for operation: 0843cd2b-aa1b-4918-b9f2-b3318cc4d7fb to complete.
Updated.
Published.
Query knowledge base with prediction runtime key 8482830b-681e-400e-b8a3-4016278aba64.
Downloaded. It has 27 QnAs.
Deleted.
```