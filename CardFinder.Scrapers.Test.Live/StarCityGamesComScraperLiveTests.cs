using CardFinder.Scrapers.Helpers;
using CardFinder.Scrapers.SingleSite;
using Microsoft.Extensions.Logging;

namespace CardFinder.Scrapers.Test.Live;
public class StarCityGamesComScraperLiveTests : LiveTestsBase
{
	[Fact]
	public async Task GetLightningBolt()
	{
		var scraper = new StarCityGamesComScraper(Logger.CreateLogger<StarCityGamesComScraper>(), DirectHttpClient, new DefaultConditionParser(), new StarCityGamesComTreatmentParser());

		var cards = await scraper.Scrape("Lightning Bolt");

		Assert.NotEmpty(cards);
		Console.WriteLine($"Found {cards.Length} cards");
	}
}
