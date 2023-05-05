using CardFinder.Scrapers.BinderPos;
using CardFinder.Scrapers.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace CardFinder.Scrapers.Test.BinderPos;
public class CardMerchantHamiltonCoNzTests
{
	[Fact]
	public async Task GrindingStation()
	{
		var client = new Mock<ICachingHttpClient>();

		var scraper = new BinderPosScraper(NullLogger<BinderPosScraper>.Instance, client.Object, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.CardMerchantHamiltonCoNz);

		client.SetupHttpGet(scraper.GetUrlForCardName("Grinding Station"), Resources.ReadResource("CardFinder.Scrapers.Test.Resources.BinderPos.CardMerchantHamiltonCoNz_GrindingStation.txt"));
		var cards = await scraper.Scrape("Grinding Station", CancellationToken.None);

		Output.PrintResult(cards);
		Assert.Equal(10, cards.Length);

		var c = cards[2];
		Assert.Equal("Grinding Station", c.CardName);
		Assert.Equal(Condition.ModeratelyPlayed, c.Condition);
		Assert.Equal(26.30m, c.Price);
		Assert.Equal("Fifth Dawn", c.Set);
		Assert.Equal(1, c.Stock);
		Assert.Equal(Treatment.Normal, c.Treatment);
	}
}
