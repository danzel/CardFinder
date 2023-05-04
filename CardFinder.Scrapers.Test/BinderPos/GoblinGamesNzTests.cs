using CardFinder.Scrapers.BinderPos;
using CardFinder.Scrapers.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace CardFinder.Scrapers.Test.BinderPos;
public class GoblinGamesNzTests
{
	[Fact]
	public async Task LightningBolt()
	{
		var client = new Mock<ICachingHttpClient>();

		var scraper = new BinderPosScraper(NullLogger<BinderPosScraper>.Instance, client.Object, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.GoblinGamesNz);

		client.SetupHttpGet(scraper.GetUrlForCardName("Lightning Bolt"), Resources.ReadResource("CardFinder.Scrapers.Test.Resources.BinderPos.GoblinGamesNz_LightningBolt.txt"));
		var cards = await scraper.Scrape("Lightning Bolt", CancellationToken.None);

		Output.PrintResult(cards);
		Assert.Equal(40, cards.Length);

		var c = cards[5];
		Assert.Equal("Lightning Bolt", c.CardName);
		Assert.Equal(Condition.NearMint, c.Condition);
		Assert.Equal(2.00m, c.Price);
		Assert.Equal("Commander Legends: Battle for Baldur's Gate", c.Set);
		Assert.Equal(1, c.Stock);
		Assert.Equal(Treatment.Showcase | Treatment.Foil, c.Treatment);
	}
}
