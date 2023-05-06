using CardFinder.Solver;

namespace CardFinder.BlazorApp.Helpers;

public static class InputHelper
{
	public static CardAmount[] ParseCardsText(string cardsText)
	{
		var lines = cardsText.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToArray();

		var res = new List<CardAmount>();

		for (var i = 0; i < lines.Length; i++)
		{
			var line = lines[i];
			try
			{
				var firstSpace = line.IndexOf(' ');
				var amount = int.Parse(line[..firstSpace]);
				var cardName = line[(firstSpace + 1)..];

				res.Add(new CardAmount(amount, cardName));
			}
			catch (Exception ex)
			{
				throw new Exception($"Failed parsing on line {i} '{line}' {ex.Message}");
			}
		}

		return res.ToArray();
	}
}
