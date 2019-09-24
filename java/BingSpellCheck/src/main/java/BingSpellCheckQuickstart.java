import com.microsoft.azure.cognitiveservices.language.spellcheck.BingSpellCheckAPI;
import com.microsoft.azure.cognitiveservices.language.spellcheck.BingSpellCheckManager;
import com.microsoft.azure.cognitiveservices.language.spellcheck.models.SpellCheck;
import com.microsoft.azure.cognitiveservices.language.spellcheck.models.SpellCheckerOptionalParameter;
import com.microsoft.azure.cognitiveservices.language.spellcheck.models.SpellingFlaggedToken;
import com.microsoft.azure.cognitiveservices.language.spellcheck.models.SpellingTokenSuggestion;

import java.util.List;

/**
 * Sample code for spell checking using Bing Spell Check, an Azure Cognitive Service.
 *  - Spell check "Bill Gatas" with market and mode settings and print out the flagged tokens and suggestions.
 */
public class BingSpellCheckQuickstart {

    public static void main(String[] args) {
        try {
            // Authenticate - create a client
            final String subscriptionKey = System.getenv("BING_SPELL_CHECK_SUBSCRIPTION_KEY");
            BingSpellCheckAPI client = BingSpellCheckManager.authenticate(subscriptionKey);

            spellCheck(client);

        } catch (Exception e) {
            System.out.println(e.getMessage());
            e.printStackTrace();
        }
    }

    public static void spellCheck(BingSpellCheckAPI client) {
        System.out.println();
        String query = "bill Gatas was ehre today";

        SpellCheck result = client.bingSpellCheckOperations().spellChecker()
            .withText(query)
            .withMode("proof")
            .withMarket("en-us")
            .execute();

        System.out.println("Original query: \n" + query);
        System.out.println();

        // SpellCheck Results
        if (result.flaggedTokens().size() > 0) {

            // Get all misspelled words (tokens)
            System.out.println("Misspelled words: ");
            for (SpellingFlaggedToken token : result.flaggedTokens()) {
                System.out.println(token.token());
            }
            System.out.println();

            // Get suggestions for correct spellings
            System.out.println("Suggested corrections: ");
            for (SpellingFlaggedToken token : result.flaggedTokens()) {
                for (int i = 0; i < token.suggestions().size(); i++ ) {
                    System.out.printf(token.suggestions().get(i).suggestion() 
                        + " with confidence score " + "%.2f%n", token.suggestions().get(i).score());
                }
            }
        }
        System.out.println();
    }
}
