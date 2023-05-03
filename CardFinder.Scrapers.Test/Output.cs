namespace CardFinder.Scrapers.Test;
internal static class Output
{
	public static void PrintResult(CardDetails[] cards)
	{
		foreach (var card in cards)
		{
			Console.WriteLine($"{card.CardName}|{card.Treatment}|{card.Condition}|{card.Currency}|{card.Price}|{card.Set}|{card.Stock}|{card.ProductUrl}|{card.ImageUrl}");
		}
	}
}
