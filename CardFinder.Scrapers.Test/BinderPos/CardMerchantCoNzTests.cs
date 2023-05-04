using CardFinder.Scrapers.BinderPos;
using CardFinder.Scrapers.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace CardFinder.Scrapers.Test.BinderPos;
public class CardMerchantCoNzTests
{
	[Fact]
	public async Task OnakkeOgre()
	{
		var client = new Mock<ICachingHttpClient>();

		var scraper = new BinderPosScraper(NullLogger<BinderPosScraper>.Instance, client.Object, new BinderPosConditionParser(), new DefaultTreatmentParser(), BinderPosConfiguration.CardMerchantCoNz);

		client.SetupHttpGet(scraper.GetUrlForCardName("Onakke Ogre"), Resources.ReadResource("CardFinder.Scrapers.Test.Resources.BinderPos.CardMerchantCoNz_OnakkeOgre.txt"));
		var cards = await scraper.Scrape("Onakke Ogre", CancellationToken.None);

		Output.PrintResult(cards);
		Assert.Equal(16, cards.Length);

		var c = cards[0];
		Assert.Equal("Onakke Ogre", c.CardName);
		Assert.Equal(Condition.NearMint, c.Condition);
		Assert.Equal(0.3m, c.Price);
		Assert.Equal("Core Set 2021", c.Set);
		Assert.Equal(0, c.Stock);
		Assert.Equal(Treatment.Normal, c.Treatment);

		c = cards[4];
		Assert.Equal("Onakke Ogre", c.CardName);
		Assert.Equal(Condition.NearMint, c.Condition);
		Assert.Equal(0.3m, c.Price);
		Assert.Equal("Core Set 2021", c.Set);
		Assert.Equal(1, c.Stock);
		Assert.Equal(Treatment.Foil, c.Treatment);
	}
}
