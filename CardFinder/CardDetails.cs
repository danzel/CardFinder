namespace CardFinder;

public class CardDetails
{
	public string CardName { get; set; } = null!;

	public Treatment Treatment { get; set; }

	public Condition Condition { get; set; }

	public string Set { get; set; } = null!;

	public decimal Price { get; set; }

	public Currency Currency { get; set; }

	public int Stock { get; set; }

	public string? ProductUrl { get; set; }

	public string? ImageUrl { get; set; }
}