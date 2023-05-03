namespace CardFinder.Scrapers.Helpers;
public static class CardNameHelpers
{
	/// <summary>
	/// Returns true if the given card names match. Has special handling for "//" split cards and transforming cards
	/// </summary>
	public static bool CardNameMatches(string expected, string actual)
	{
		static void Fix(ref string value)
		{
			var slashslash = value.IndexOf("//");
			if (slashslash >= 0)
				value = value[0..slashslash];
			value = value.Trim().ToLowerInvariant();
		}

		Fix(ref expected);
		Fix(ref actual);

		return expected == actual;
	}

	/// <summary>
	/// Finds all blocks of bracketed text and removes them, returning them separately
	/// </summary>
	public static (string CardName, string[] RoundBracketsText, string[] BoxBracketsText) SplitCardNameAndBracketedText(string cardName)
	{
		List<string>? roundBrackets = null;
		List<string>? boxBrackets = null;

		static void Process(ref List<string>? list, char start, char end, ref string cardName)
		{
			int startingBraceIndex;
			while ((startingBraceIndex = cardName.IndexOf(start)) >= 0)
			{
				var endingIndex = cardName.IndexOf(end, startingBraceIndex);
				if (endingIndex < 0)
					throw new InvalidDataException($"Couldn't find closing brace '{end}' in '{cardName}'");

				var inBrackets = cardName[(startingBraceIndex + 1)..endingIndex];
				cardName = cardName.Remove(startingBraceIndex, endingIndex - startingBraceIndex + 1);

				list ??= new List<string>();
				list.Add(inBrackets);
			}
		}

		Process(ref roundBrackets, '(', ')', ref cardName);
		Process(ref boxBrackets, '[', ']', ref cardName);

		return (
			cardName.Trim(), 
			roundBrackets?.ToArray() ?? Array.Empty<string>(),
			boxBrackets?.ToArray() ?? Array.Empty<string>()
		);
	}
}
