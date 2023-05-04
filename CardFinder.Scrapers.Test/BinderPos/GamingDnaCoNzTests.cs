using CardFinder.Scrapers.BinderPos;
using CardFinder.Scrapers.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace CardFinder.Scrapers.Test.BinderPos;
public class GamingDnaCoNzTests
{
	[Fact]
	public async Task LightningBolt()
	{
		var client = new Mock<ICachingHttpClient>();

		var scraper = new BinderPosScraper(NullLogger<BinderPosScraper>.Instance, client.Object, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.GamingDnaCoNz);

		client.SetupHttpGet(scraper.GetUrlForCardName("Force Of Will"), Resources.ReadResource("CardFinder.Scrapers.Test.Resources.BinderPos.GamingDnaCoNz_ForceOfWill.txt"));
		var cards = await scraper.Scrape("Force Of Will", CancellationToken.None);

		Output.PrintResult(cards);
		Assert.Equal(20, cards.Length);

		var c = cards[0];
		Assert.Equal("Force of Will", c.CardName);
		Assert.Equal(Condition.NearMint, c.Condition);
		Assert.Equal(100.2m, c.Price);
		Assert.Equal("Dominaria Remastered", c.Set);
		Assert.Equal(1, c.Stock);
		Assert.Equal(Treatment.Normal, c.Treatment);
	}
}
