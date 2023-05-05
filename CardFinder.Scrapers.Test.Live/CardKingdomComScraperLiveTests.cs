using CardFinder.Scrapers.Helpers;
using CardFinder.Scrapers.SingleSite;
using Microsoft.Extensions.Logging;

namespace CardFinder.Scrapers.Test.Live;
public class CardKingdomComScraperLiveTests : LiveTestsBase
{
	[Fact]
	public async Task GetLightningBolt()
	{
		var scraper = new CardKingdomComScraper(Logger.CreateLogger<CardKingdomComScraper>(), DirectHttpClient, new CardKingdomComConditionParser(), new CardKingdomComTreatmentParser());

		var cards = await scraper.Scrape("Lightning Bolt");

		Assert.NotEmpty(cards);
		Console.WriteLine($"Found {cards.Length} cards");
	}
}
