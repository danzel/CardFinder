using CardFinder.Scrapers.BinderPos;
using CardFinder.Scrapers.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace CardFinder.Scrapers.Test.BinderPos;
public class IronKnightGamingCoNzTests
{
	[Fact]
	public async Task LightningBolt()
	{
		var client = new Mock<ICachingHttpClient>();

		var scraper = new BinderPosScraper(NullLogger<BinderPosScraper>.Instance, client.Object, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.IronKnightGamingCoNz);

		client.SetupHttpGet(scraper.GetUrlForCardName("Lightning Bolt"), Resources.ReadResource("CardFinder.Scrapers.Test.Resources.BinderPos.IronKnightGamingCoNz_LightningBolt.txt"));
		var cards = await scraper.Scrape("Lightning Bolt", CancellationToken.None);

		Output.PrintResult(cards);
		Assert.Equal(55, cards.Length);

		var c = cards[3];
		Assert.Equal("Lightning Bolt", c.CardName);
		Assert.Equal(Condition.HeavilyPlayed, c.Condition);
		Assert.Equal(1.10m, c.Price);
		Assert.Equal("Magic 2011", c.Set);
		Assert.Equal(1, c.Stock);
		Assert.Equal(Treatment.Normal, c.Treatment);
	}
}
