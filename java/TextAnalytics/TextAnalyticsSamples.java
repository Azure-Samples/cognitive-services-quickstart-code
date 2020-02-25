import com.azure.ai.textanalytics.models.*;
import com.azure.ai.textanalytics.TextAnalyticsClientBuilder;
import com.azure.ai.textanalytics.TextAnalyticsClient;

public class TextAnalyticsSamples {

    private static String KEY = "<replace-with-your-text-analytics-key-here>";
    private static String ENDPOINT = "<replace-with-your-text-analytics-endpoint-here>";

    public static void main(String[] args) {

        TextAnalyticsClient client = authenticateClient(KEY, ENDPOINT);

        sentimentAnalysisExample(client);
        detectLanguageExample(client);
        recognizeEntitiesExample(client);
        recognizePIIEntitiesExample(client);
        recognizeLinkedEntitiesExample(client);
        extractKeyPhrasesExample(client);
    }

    static TextAnalyticsClient authenticateClient(String key, String endpoint) {
        return new TextAnalyticsClientBuilder()
                .apiKey(new TextAnalyticsApiKeyCredential(key))
                .endpoint(endpoint)
                .buildClient();
    }

    static void sentimentAnalysisExample(TextAnalyticsClient client)
    {
        // The text that need be analyzed.
        String text = "I had the best day of my life. I wish you were there with me.";

        DocumentSentiment documentSentiment = client.analyzeSentiment(text);
        System.out.printf(
                "Recognized document sentiment: %s, positive score: %.2f, neutral score: %.2f, negative score: %.2f.%n",
                documentSentiment.getSentiment(),
                documentSentiment.getSentimentScores().getPositive(),
                documentSentiment.getSentimentScores().getNeutral(),
                documentSentiment.getSentimentScores().getNegative());

        for (SentenceSentiment sentenceSentiment : documentSentiment.getSentences()) {
            System.out.printf(
                    "Recognized sentence sentiment: %s, positive score: %.2f, neutral score: %.2f, negative score: %.2f.%n",
                    sentenceSentiment.getSentiment(),
                    sentenceSentiment.getSentimentScores().getPositive(),
                    sentenceSentiment.getSentimentScores().getNeutral(),
                    sentenceSentiment.getSentimentScores().getNegative());
        }
    }

    static void detectLanguageExample(TextAnalyticsClient client)
    {
        // The text that need be analyzed.
        String text = "Ce document est rédigé en Français.";

        DetectedLanguage detectedLanguage = client.detectLanguage(text);
        System.out.printf("Detected primary language: %s, ISO 6391 name: %s, score: %.2f.%n",
                detectedLanguage.getName(),
                detectedLanguage.getIso6391Name(),
                detectedLanguage.getScore());
    }

    static void recognizeEntitiesExample(TextAnalyticsClient client)
    {
        // The text that need be analyzed.
        String text = "I had a wonderful trip to Seattle last week.";

        for (CategorizedEntity entity : client.recognizeEntities(text)) {
            System.out.printf(
                    "Recognized entity: %s, entity category: %s, entity sub-category: %s, offset: %s, length: %s, score: %.2f.%n",
                    entity.getText(),
                    entity.getCategory(),
                    entity.getSubCategory() == null || entity.getSubCategory().isEmpty() ? "N/A" : entity.getSubCategory(),
                    entity.getOffset(),
                    entity.getLength(),
                    entity.getScore());
        }
    }

    static void recognizePIIEntitiesExample(TextAnalyticsClient client)
    {
        // The text that need be analyzed.
        String text = "Insurance policy for SSN on file 123-12-1234 is here by approved.";

        for (PiiEntity entity : client.recognizePiiEntities(text)) {
            System.out.printf(
                    "Recognized personal identifiable information entity: %s, entity category: %s, entity sub-category: %s, offset: %s, length: %s, score: %.2f.%n",
                    entity.getText(),
                    entity.getCategory(),
                    entity.getSubCategory() == null || entity.getSubCategory().isEmpty() ? "N/A" : entity.getSubCategory(),
                    entity.getOffset(),
                    entity.getLength(),
                    entity.getScore());
        }
    }

    static void recognizeLinkedEntitiesExample(TextAnalyticsClient client)
    {
        // The text that need be analyzed.
        String text = "Microsoft was founded by Bill Gates and Paul Allen on April 4, 1975, " +
                "to develop and sell BASIC interpreters for the Altair 8800. " +
                "During his career at Microsoft, Gates held the positions of chairman, " +
                "chief executive officer, president and chief software architect, " +
                "while also being the largest individual shareholder until May 2014.";

        System.out.printf("Linked Entities:%n");
        for (LinkedEntity linkedEntity : client.recognizeLinkedEntities(text)) {
            System.out.printf("Name: %s, ID: %s, URL: %s, Data Source: %s.%n",
                    linkedEntity.getName(),
                    linkedEntity.getId(),
                    linkedEntity.getUrl(),
                    linkedEntity.getDataSource());
            System.out.printf("Matches:%n");
            for (LinkedEntityMatch linkedEntityMatch : linkedEntity.getLinkedEntityMatches()) {
                System.out.printf("Text: %s, Offset: %s, Length: %s, Score: %.2f.%n",
                        linkedEntityMatch.getText(),
                        linkedEntityMatch.getOffset(),
                        linkedEntityMatch.getLength(),
                        linkedEntityMatch.getScore());
            }
        }
    }

    static void extractKeyPhrasesExample(TextAnalyticsClient client)
    {
        // The text that need be analyzed.
        String text = "My cat might need to see a veterinarian.";

        System.out.printf("Recognized phrases: %n");
        for (String keyPhrase : client.extractKeyPhrases(text)) {
            System.out.printf("%s%n", keyPhrase);
        }
    }


}
