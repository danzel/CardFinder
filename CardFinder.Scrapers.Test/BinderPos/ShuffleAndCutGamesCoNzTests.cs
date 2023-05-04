using CardFinder.Scrapers.BinderPos;
using CardFinder.Scrapers.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace CardFinder.Scrapers.Test.BinderPos;
public class ShuffleAndCutGamesCoNzTests
{
	[Fact]
	public async Task LightningBolt()
	{
		var client = new Mock<ICachingHttpClient>();

		var scraper = new BinderPosScraper(NullLogger<BinderPosScraper>.Instance, client.Object, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.ShuffleAndCutGameCoNz);

		client.SetupHttpGet(scraper.GetUrlForCardName("Lightning Bolt"), Resources.ReadResource("CardFinder.Scrapers.Test.Resources.BinderPos.ShuffleAndCutGamesCoNz_LightningBolt.txt"));
		var cards = await scraper.Scrape("Lightning Bolt", CancellationToken.None);

		Output.PrintResult(cards);
		Assert.Equal(20, cards.Length);

		var c = cards[0];
		Assert.Equal("Lightning Bolt", c.CardName);
		Assert.Equal(Condition.NearMint, c.Condition);
		Assert.Equal(2.00m, c.Price);
		Assert.Equal("Magic 2011", c.Set);
		Assert.Equal(2, c.Stock);
		Assert.Equal(Treatment.Normal, c.Treatment);

		c = cards[11];
		Assert.Equal("Lightning Bolt", c.CardName);
		Assert.Equal(Condition.NearMint, c.Condition);
		Assert.Equal(21.70m, c.Price);
		Assert.Equal("Strixhaven Mystical Archive", c.Set);
		Assert.Equal(1, c.Stock);
		Assert.Equal(Treatment.JapaneseAlternateArt, c.Treatment);
	}

	[Fact]
	public async Task AridMesa()
	{
		var client = new Mock<ICachingHttpClient>();

		var scraper = new BinderPosScraper(NullLogger<BinderPosScraper>.Instance, client.Object, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.ShuffleAndCutGameCoNz);

		client.SetupHttpGet(scraper.GetUrlForCardName("Arid Mesa"), Resources.ReadResource("CardFinder.Scrapers.Test.Resources.BinderPos.ShuffleAndCutGamesCoNz_AridMesa.txt"));
		var cards = await scraper.Scrape("Arid Mesa", CancellationToken.None);

		Output.PrintResult(cards);
		Assert.Equal(12, cards.Length);

		var c = cards[8];
		Assert.Equal("Arid Mesa", c.CardName);
		Assert.Equal(Condition.Unspecified, c.Condition);
		Assert.Equal(999.99m, c.Price);
		Assert.Equal("Modern Masters 2017", c.Set);
		Assert.Equal(0, c.Stock);
		Assert.Equal(Treatment.Normal, c.Treatment);
	}
}
