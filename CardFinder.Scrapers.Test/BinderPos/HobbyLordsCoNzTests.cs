using CardFinder.Scrapers.BinderPos;
using CardFinder.Scrapers.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace CardFinder.Scrapers.Test.BinderPos;
public class HobbyLordsCoNzTests
{
	[Fact]
	public async Task PhyrexianArena()
	{
		var client = new Mock<ICachingHttpClient>();

		var scraper = new BinderPosScraper(NullLogger<BinderPosScraper>.Instance, client.Object, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.HobbyLordsCoNz);

		client.SetupHttpGet(scraper.GetUrlForCardName("Phyrexian Arena"), Resources.ReadResource("CardFinder.Scrapers.Test.Resources.BinderPos.HobbyLordsCoNz_PhyrexianArena.txt"));
		var cards = await scraper.Scrape("Phyrexian Arena", CancellationToken.None);

		Output.PrintResult(cards);
		Assert.Equal(2, cards.Length);

		var c = cards[0];
		Assert.Equal("Phyrexian Arena", c.CardName);
		Assert.Equal(Condition.NearMint, c.Condition);
		Assert.Equal(4.89m, c.Price);
		Assert.Equal("Phyrexia: All Will Be One (ONE)", c.Set);
		Assert.Equal(1, c.Stock);
		Assert.Equal(Treatment.Foil, c.Treatment);
	}
}
