using CardFinder.Scrapers.BinderPos;
using CardFinder.Scrapers.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace CardFinder.Scrapers.Test.BinderPos;
public class MtgMagpieComTests
{
	[Fact]
	public async Task LightningBolt()
	{
		var client = new Mock<ICachingHttpClient>();

		var scraper = new BinderPosScraper(NullLogger<BinderPosScraper>.Instance, client.Object, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.MtgMagpieCom);

		client.SetupHttpGet(scraper.GetUrlForCardName("Arid Mesa"), Resources.ReadResource("CardFinder.Scrapers.Test.Resources.BinderPos.MtgMagpieCom_AridMesa.txt"));
		var cards = await scraper.Scrape("Arid Mesa", CancellationToken.None);

		Output.PrintResult(cards);
		Assert.Equal(10, cards.Length);

		var c = cards[0];
		Assert.Equal("Arid Mesa", c.CardName);
		Assert.Equal(Condition.LightlyPlayed, c.Condition);
		Assert.Equal(254.85m, c.Price);
		Assert.Equal("Zendikar Expeditions", c.Set);
		Assert.Equal(1, c.Stock);
		Assert.Equal(Treatment.Foil, c.Treatment);
	}
}
