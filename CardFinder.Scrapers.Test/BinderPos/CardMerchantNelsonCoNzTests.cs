using CardFinder.Scrapers.BinderPos;
using CardFinder.Scrapers.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace CardFinder.Scrapers.Test.BinderPos;
public class CardMerchantNelsonCoNzTests
{
	[Fact]
	public async Task ForceOfNegation()
	{
		var client = new Mock<ICachingHttpClient>();

		var scraper = new BinderPosScraper(NullLogger<BinderPosScraper>.Instance, client.Object, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.CardMerchantNelsonCoNz);

		client.SetupHttpGet(scraper.GetUrlForCardName("Force Of Negation"), Resources.ReadResource("CardFinder.Scrapers.Test.Resources.BinderPos.CardMerchantNelsonCoNz_ForceOfNegation.txt"));
		var cards = await scraper.Scrape("Force Of Negation", CancellationToken.None);

		Output.PrintResult(cards);
		Assert.Equal(45, cards.Length);

		var c = cards[0];
		Assert.Equal("Force of Negation", c.CardName);
		Assert.Equal(Condition.NearMint, c.Condition);
		Assert.Equal(49.90m, c.Price);
		Assert.Equal("Modern Horizons", c.Set);
		Assert.Equal(1, c.Stock);
		Assert.Equal(Treatment.Normal, c.Treatment);
	}
}
