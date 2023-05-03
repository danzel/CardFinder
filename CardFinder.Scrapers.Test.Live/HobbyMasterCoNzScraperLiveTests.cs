using CardFinder.Scrapers.Helpers;
using CardFinder.Scrapers.SingleSite;
using Microsoft.Extensions.Logging;

namespace CardFinder.Scrapers.Test.Live;
public class HobbyMasterCoNzScraperLiveTests : LiveTestsBase
{
	[Fact]
	public async Task GetAridMesa()
	{
		var scraper = new HobbyMasterCoNzScraper(Logger.CreateLogger<HobbyMasterCoNzScraper>(), DirectHttpClient, new DefaultConditionParser(), new HobbyMasterTreatmentParser());

		var cards = await scraper.Scrape("Arid Mesa");

		Assert.NotEmpty(cards);
		Console.WriteLine($"Found {cards.Length} cards");
	}
}
