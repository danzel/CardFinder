using CardFinder.Scrapers.Helpers;
using CardFinder.Scrapers.SingleSite;
using Microsoft.Extensions.Logging;

namespace CardFinder.Scrapers.Test.Live;

public class BayDragonCoNzScraperLiveTests : LiveTestsBase
{
	[Fact]
	public async Task GetAridMesa()
	{
		var scraper = new BayDragonCoNzScraper(Logger.CreateLogger<BayDragonCoNzScraper>(), DirectHttpClient, new DefaultConditionParser(), new DefaultTreatmentParser());

		var cards = await scraper.Scrape("Arid Mesa");

		Assert.NotEmpty(cards);
		Console.WriteLine($"Found {cards.Length} cards");
	}
}