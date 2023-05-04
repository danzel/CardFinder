using CardFinder.Scrapers.BinderPos;
using CardFinder.Scrapers.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace CardFinder.Scrapers.Test.BinderPos;
public class CalicoKeepCoNzTests
{
	[Fact]
	public async Task LightningBolt()
	{
		var client = new Mock<ICachingHttpClient>();

		var scraper = new BinderPosScraper(NullLogger<BinderPosScraper>.Instance, client.Object, new BinderPosConditionParser(), new DefaultTreatmentParser(), BinderPosConfiguration.CalicoKeepCoNz);

		client.SetupHttpGet(scraper.GetUrlForCardName("Lightning Bolt"), Resources.ReadResource("CardFinder.Scrapers.Test.Resources.BinderPos.CalicoKeepCoNz_LightningBolt.txt"));
		var cards = await scraper.Scrape("Lightning Bolt", CancellationToken.None);

		Output.PrintResult(cards);
		Assert.Equal(90, cards.Length);

		var c = cards[85];
		Assert.Equal("Lightning Bolt", c.CardName);
		Assert.Equal(Condition.NearMint, c.Condition);
		Assert.Equal(18.1m, c.Price);
		Assert.Equal("Strixhaven: School of Mages Mystical Archive", c.Set);
		Assert.Equal(1, c.Stock);
		Assert.Equal(Treatment.JapaneseAlternateArt | Treatment.Foil | Treatment.Etched, c.Treatment);
	}
}
