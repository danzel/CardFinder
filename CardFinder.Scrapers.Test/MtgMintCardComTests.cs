using CardFinder.Scrapers.Helpers;
using CardFinder.Scrapers.SingleSite;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace CardFinder.Scrapers.Test;

public class MtgMintCardComTests
{
	[Fact]
	public async Task LightningBolt()
	{
		var client = new Mock<ICachingHttpClient>();

		var scraper = new MtgMintCardComScraper(NullLogger<MtgMintCardComScraper>.Instance, client.Object, new DefaultConditionParser(), new DefaultTreatmentParser());

		client.SetupHttpGet(scraper.GetUrlForCardName("Lightning Bolt"), Resources.ReadResource("CardFinder.Scrapers.Test.Resources.MtgMintCardCom.search_lightningbolt.txt"));
		client.SetupHttpGet(scraper.GetUrlForCardNameVariants("Lightning Bolt"), Resources.ReadResource("CardFinder.Scrapers.Test.Resources.MtgMintCardCom.search_lightningbolt_variants.txt"));
		var cards = await scraper.Scrape("Lightning Bolt", CancellationToken.None);

		Output.PrintResult(cards);
		Assert.Equal(29, cards.Length);

		var c = cards[3];
		Assert.Equal("Lightning Bolt", c.CardName);
		Assert.Equal(Condition.NearMint, c.Condition);
		Assert.Equal(39.99m, c.Price);
		Assert.Equal("Unlimited", c.Set);
		Assert.Equal(2, c.Stock);
		Assert.Equal(Treatment.Normal, c.Treatment);

		c = cards[28];
		Assert.Equal("Lightning Bolt", c.CardName);
		Assert.Equal(Condition.NearMint, c.Condition);
		Assert.Equal(1.79m, c.Price);
		Assert.Equal("Commander Legends: Battle for Baldur's Gate", c.Set);
		Assert.Equal(7, c.Stock);
		Assert.Equal(Treatment.Showcase, c.Treatment);
	}
}