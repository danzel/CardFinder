using CardFinder.Scrapers.Helpers;
using CardFinder.Scrapers.SingleSite;
using Microsoft.Extensions.Logging;

namespace CardFinder.Scrapers.Test.Live;
public class MtgMintCardComScraperLiveTests : LiveTestsBase
{
	[Fact]
	public async Task GetLightningBolt()
	{
		var scraper = new MtgMintCardComScraper(Logger.CreateLogger<MtgMintCardComScraper>(), DirectHttpClient, new DefaultConditionParser(), new DefaultTreatmentParser());

		var cards = await scraper.Scrape("Lightning Bolt");

		Assert.NotEmpty(cards);
		Console.WriteLine($"Found {cards.Length} cards");
	}
}
