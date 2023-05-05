using CardFinder.Scrapers.Helpers;
using CardFinder.Scrapers.SingleSite;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace CardFinder.Scrapers.Test;
public class CardKingdomComScraperTests
{
	[Fact]
	public async Task LightningBolt()
	{
		var client = new Mock<ICachingHttpClient>();

		var scraper = new CardKingdomComScraper(NullLogger<CardKingdomComScraper>.Instance, client.Object, new CardKingdomComConditionParser(), new CardKingdomComTreatmentParser());

		client.SetupHttpGet(scraper.GetUrlForCardName("Lightning Bolt"), Resources.ReadResource("CardFinder.Scrapers.Test.Resources.CardKingdomCom.search_lightningbolt.txt"));
		var cards = await scraper.Scrape("Lightning Bolt", CancellationToken.None);

		Output.PrintResult(cards);
		Assert.Equal(140, cards.Length);

		var c = cards[8];
		Assert.Equal("Lightning Bolt", c.CardName);
		Assert.Equal(Condition.NearMint, c.Condition);
		Assert.Equal(1.79m, c.Price);
		Assert.Equal("Commander Legends: Battle for Baldur's Gate Variants", c.Set);
		Assert.Equal(20, c.Stock);
		Assert.Equal(Treatment.Showcase, c.Treatment);

		c = cards[36];
		Assert.Equal("Lightning Bolt", c.CardName);
		Assert.Equal(Condition.NearMint, c.Condition);
		Assert.Equal(3.49m, c.Price);
		Assert.Equal("Double Masters 2022 Variants", c.Set);
		Assert.Equal(20, c.Stock);
		Assert.Equal(Treatment.Borderless, c.Treatment);
	}
}
