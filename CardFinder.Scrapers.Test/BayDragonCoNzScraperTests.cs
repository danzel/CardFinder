using CardFinder.Scrapers.Helpers;
using CardFinder.Scrapers.SingleSite;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace CardFinder.Scrapers.Test;

public class BayDragonCoNzScraperTests
{
	[Fact]
	public async Task AridMesa()
	{
		var client = new Mock<ICachingHttpClient>();

		var scraper = new BayDragonCoNzScraper(NullLogger<BayDragonCoNzScraper>.Instance, client.Object, new DefaultConditionParser(), new DefaultTreatmentParser());

		client.SetupHttpGet(scraper.GetUrlForCardName("Arid Mesa"), Resources.ReadResource("CardFinder.Scrapers.Test.Resources.BayDragonCoNz.search_aridmesa.txt"));
		var cards = await scraper.Scrape("Arid Mesa", CancellationToken.None);

		Output.PrintResult(cards);
		Assert.Equal(10, cards.Length);

		var c = cards[4];
		Assert.Equal("Arid Mesa", c.CardName);
		Assert.Equal(Condition.NearMint, c.Condition);
		Assert.Equal(32.27m, c.Price);
		Assert.Equal("Magic Modern Horizons 2 Draft Booster", c.Set);
		Assert.Equal(6, c.Stock);
		Assert.Equal(Treatment.Normal, c.Treatment);
	}

	[Fact]
	public async Task LightningBolt()
	{
		var client = new Mock<ICachingHttpClient>();

		var scraper = new BayDragonCoNzScraper(NullLogger<BayDragonCoNzScraper>.Instance, client.Object, new DefaultConditionParser(), new DefaultTreatmentParser());

		client.SetupHttpGet(scraper.GetUrlForCardName("Lightning Bolt"), Resources.ReadResource("CardFinder.Scrapers.Test.Resources.BayDragonCoNz.search_lightningbolt.txt"));
		var cards = await scraper.Scrape("Lightning Bolt", CancellationToken.None);

		Output.PrintResult(cards);
		Assert.Equal(24, cards.Length);

		var c = cards[14];
		Assert.Equal("Lightning Bolt", c.CardName);
		Assert.Equal(Condition.NearMint, c.Condition);
		Assert.Equal(1.76m, c.Price);
		Assert.Equal("The List A25", c.Set);
		Assert.Equal(0, c.Stock);
		Assert.Equal(Treatment.Normal, c.Treatment);
	}
}