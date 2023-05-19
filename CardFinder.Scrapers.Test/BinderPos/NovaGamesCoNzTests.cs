using CardFinder.Scrapers.BinderPos;
using CardFinder.Scrapers.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace CardFinder.Scrapers.Test.BinderPos;
public class NovaGamesCoNzTests
{
	[Fact]
	public async Task AridMesa()
	{
		var client = new Mock<ICachingHttpClient>();

		var scraper = new BinderPosScraper(NullLogger<BinderPosScraper>.Instance, client.Object, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.NovaGamesCoNz);

		client.SetupHttpGet(scraper.GetUrlForCardName("Arid Mesa"), Resources.ReadResource("CardFinder.Scrapers.Test.Resources.BinderPos.NovaGamesCoNz_AridMesa.txt"));
		var cards = await scraper.Scrape("Arid Mesa", CancellationToken.None);

		Output.PrintResult(cards);
		Assert.Equal(20, cards.Length);

		var c = cards[0];
		Assert.Equal("Arid Mesa", c.CardName);
		Assert.Equal(Condition.NearMint, c.Condition);
		Assert.Equal(25.4m, c.Price);
		Assert.Equal("Modern Horizons 2", c.Set);
		Assert.Equal(1, c.Stock);
		Assert.Equal(Treatment.Normal, c.Treatment);
	}
}
