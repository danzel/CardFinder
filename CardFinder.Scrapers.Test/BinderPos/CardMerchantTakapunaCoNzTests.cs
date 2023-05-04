using CardFinder.Scrapers.BinderPos;
using CardFinder.Scrapers.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace CardFinder.Scrapers.Test.BinderPos;
public class CardMerchantTakapunaCoNzTests
{
	[Fact]
	public async Task LightningBolt()
	{
		var client = new Mock<ICachingHttpClient>();

		var scraper = new BinderPosScraper(NullLogger<BinderPosScraper>.Instance, client.Object, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.CardMerchantTakapunaCoNz);

		client.SetupHttpGet(scraper.GetUrlForCardName("Lightning Bolt"), Resources.ReadResource("CardFinder.Scrapers.Test.Resources.BinderPos.CardMerchantTakapunaCoNz_LightningBolt.txt"));
		var cards = await scraper.Scrape("Lightning Bolt", CancellationToken.None);

		Output.PrintResult(cards);
		Assert.Equal(16, cards.Length);

		var c = cards[0];
		Assert.Equal("Lightning Bolt", c.CardName);
		Assert.Equal(Condition.NearMint, c.Condition);
		Assert.Equal(1.90m, c.Price);
		Assert.Equal("Modern Masters 2015", c.Set);
		Assert.Equal(1, c.Stock);
		Assert.Equal(Treatment.Normal, c.Treatment);

		c = cards[11];
		Assert.Equal("Lightning Bolt", c.CardName);
		Assert.Equal(Condition.Unspecified, c.Condition);
		Assert.Equal(999.99m, c.Price);
		Assert.Equal("Fourth Edition", c.Set);
		Assert.Equal(0, c.Stock);
		Assert.Equal(Treatment.Normal, c.Treatment);
	}
}
