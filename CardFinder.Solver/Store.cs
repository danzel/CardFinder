using CardFinder.Scrapers;

namespace CardFinder.Solver;

/// <summary>
/// A store we can scrape and order cards from
/// </summary>
public record Store(string Name, IScraper Scraper, decimal ShippingCost, Currency ShippingCostCurrency, GstHandling GstHandling)
{
	public decimal? ShippingIsFreeIfYouSpend { get; init; }

	public decimal GstMultiplier => GstHandling switch
	{
		GstHandling.Included => 1,
		GstHandling.AddedToTotal => 1.15m,
		_ => throw new NotImplementedException(GstHandling.ToString())
	};
}
