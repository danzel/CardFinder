using CardFinder.Scrapers.BinderPos;
using CardFinder.Scrapers.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace CardFinder.Scrapers.Test.BinderPos;
public class BeaDndGamesCoNzTests
{
	[Fact]
	public async Task LightningBolt()
	{
		var client = new Mock<ICachingHttpClient>();

		var scraper = new BinderPosScraper(NullLogger<BinderPosScraper>.Instance, client.Object, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.BeaDndGamesCoNz);

		client.SetupHttpGet(scraper.GetUrlForCardName("Lightning Bolt"), Resources.ReadResource("CardFinder.Scrapers.Test.Resources.BinderPos.BeaDndGamesCoNz_LightningBolt.txt"));
		var cards = await scraper.Scrape("Lightning Bolt", CancellationToken.None);

		Output.PrintResult(cards);
		Assert.Equal(90, cards.Length);

		var c = cards[0];
		Assert.Equal("Lightning Bolt", c.CardName);
		Assert.Equal(Condition.NearMint, c.Condition);
		Assert.Equal(3.4m, c.Price);
		Assert.Equal("Strixhaven Mystical Archive", c.Set);
		Assert.Equal(2, c.Stock);
		Assert.Equal(Treatment.Normal, c.Treatment);
	}
}
