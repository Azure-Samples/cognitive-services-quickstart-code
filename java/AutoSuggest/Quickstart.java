import com.microsoft.azure.cognitiveservices.search.autosuggest.*;
import com.microsoft.azure.cognitiveservices.search.autosuggest.models.*;

import java.io.*;
import java.lang.Object.*;
import java.util.*;
import java.net.*;

/**
 * This Azure Cognitive Services Bing Autosuggest API quickstart shows how to
 * get search suggestions for a given string query.
 * 
 * Download all Maven dependencies from command line into your project folder: 
 *     mvn clean dependency:copy-dependencies
 * 
 * To compile and run, enter the following at a command prompt: 
 *     javac Quickstart.java -cp .;lib\* 
 *     java -cp .;lib\* Quickstart 
 * This presumes your libraries are stored in a folder named "lib" in your project
 * folder. If not, please adjust the -classpath (-cp) value accordingly.
 */

public class Quickstart {
	/**
	 * Configure the local environment: Set the AUTOSUGGEST_SUBSCRIPTION_KEY 
	 * in your environment variables for your preferred shell (Bash, PowerShell, Command Prompt, etc.)
	 *
	 * If the environment variable is created after the application is launched in a
	 * console or with Visual Studio (Code), they need to be closed then reloaded 
	 * for the environment variable to work.
	 */
	private static String subscription_key = System.getenv("AUTOSUGGEST_SUBSCRIPTION_KEY");

	BingAutoSuggestSearchAPI client = BingAutoSuggestSearchManager.authenticate(subscription_key);

	public void get_suggestions() {
		Suggestions suggestions = client.bingAutoSuggestSearch().autoSuggest().withQuery("sail").execute();
		if (suggestions != null && suggestions.suggestionGroups() != null && suggestions.suggestionGroups().size() > 0) {
			SuggestionsSuggestionGroup group = suggestions.suggestionGroups().get(0);
			System.out.println("First suggestion group: " + group.name());
			System.out.println("Suggestions:");
			for (SearchAction suggestion: group.searchSuggestions()) {
				System.out.println("Query: " + suggestion.query());
				System.out.println("Text: " + suggestion.displayText());
				System.out.println("URL: " + suggestion.url());
				System.out.println("Kind: " + suggestion.searchKind());
				System.out.println();
			}
		} else {
			System.out.println("No suggestions found.");
		}
	}

	public static void main(String[] args) {
		try {
			Quickstart quickstart = new Quickstart();
			quickstart.get_suggestions();
		} catch (Exception e) {
			System.out.println(e.getMessage());
			e.printStackTrace();
		}
	}
}
