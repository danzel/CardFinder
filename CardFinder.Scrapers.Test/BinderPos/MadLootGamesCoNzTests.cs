using CardFinder.Scrapers.BinderPos;
using CardFinder.Scrapers.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace CardFinder.Scrapers.Test.BinderPos;
public class MadLootGamesCoNzTests
{
	[Fact]
	public async Task OnakkeOgre()
	{
		var client = new Mock<ICachingHttpClient>();

		var scraper = new BinderPosScraper(NullLogger<BinderPosScraper>.Instance, client.Object, new BinderPosConditionParser(), new DefaultTreatmentParser(), BinderPosConfiguration.MadLootGamesCoNz);

		client.SetupHttpGet(scraper.GetUrlForCardName("Onakke Ogre"), Resources.ReadResource("CardFinder.Scrapers.Test.Resources.BinderPos.MadLootGamesCoNz_OnakkeOgre.txt"));
		var cards = await scraper.Scrape("Onakke Ogre", CancellationToken.None);

		Output.PrintResult(cards);
		Assert.Equal(20, cards.Length);

		var c = cards[0];
		Assert.Equal("Onakke Ogre", c.CardName);
		Assert.Equal(Condition.NearMint, c.Condition);
		Assert.Equal(0.3m, c.Price);
		Assert.Equal("Core Set 2021", c.Set);
		Assert.Equal(1, c.Stock);
		Assert.Equal(Treatment.Normal, c.Treatment);
	}
}
