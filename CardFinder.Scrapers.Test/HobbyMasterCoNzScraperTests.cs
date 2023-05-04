using CardFinder.Scrapers.Helpers;
using CardFinder.Scrapers.SingleSite;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace CardFinder.Scrapers.Test;
public class HobbyMasterCoNzScraperTests
{
	[Fact]
	public async Task Test1()
	{
		var client = new Mock<ICachingHttpClient>();

		var scraper = new HobbyMasterCoNzScraper(NullLogger<HobbyMasterCoNzScraper>.Instance, client.Object, new DefaultConditionParser(), new HobbyMasterTreatmentParser());

		client.SetupHttpGet(scraper.GetUrlForCardName("Arid Mesa"), Resources.ReadResource("CardFinder.Scrapers.Test.Resources.HobbyMasterCoNz.getcards_aridmesa.txt"));
		var cards = await scraper.Scrape("Arid Mesa", CancellationToken.None);

		Output.PrintResult(cards);
		Assert.Equal(18, cards.Length);

		var c = cards[0];
		Assert.Equal("Arid Mesa", c.CardName);
		Assert.Equal(Condition.NearMint, c.Condition);
		Assert.Equal(29.30m, c.Price);
		Assert.Equal("Modern Horizons 2", c.Set);
		Assert.Equal(4, c.Stock);
		Assert.Equal(Treatment.Normal, c.Treatment);

		c = cards[2];
		Assert.Equal("Arid Mesa", c.CardName);
		Assert.Equal(Condition.NearMint, c.Condition);
		Assert.Equal(44.70m, c.Price);
		Assert.Equal("Modern Horizons 2", c.Set);
		Assert.Equal(1, c.Stock);
		Assert.Equal(Treatment.RetroFrame, c.Treatment);
	}

	[Fact]
	public async Task Test2()
	{
		var client = new Mock<ICachingHttpClient>();

		var scraper = new HobbyMasterCoNzScraper(NullLogger<HobbyMasterCoNzScraper>.Instance, client.Object, new DefaultConditionParser(), new HobbyMasterTreatmentParser());

		client.SetupHttpGet(scraper.GetUrlForCardName("Lightning Bolt"), Resources.ReadResource("CardFinder.Scrapers.Test.Resources.HobbyMasterCoNz.getcards_lightningbolt.txt"));
		var cards = await scraper.Scrape("Lightning Bolt", CancellationToken.None);

		Output.PrintResult(cards);
		Assert.Equal(50, cards.Length);

		//Art Card
		var c = cards[7];
		Assert.Equal("Lightning Bolt", c.CardName);
		Assert.Equal("Art Series: Strixhaven", c.Set);
		Assert.Equal(Treatment.ArtCard, c.Treatment);

		//Secret Lair
		c = cards[12];
		Assert.Equal("Lightning Bolt", c.CardName);
		Assert.Equal("Secret Lair Drop Series", c.Set);
		Assert.Equal(Treatment.Foil, c.Treatment);

		//World Championship Decks
		c = cards[33];
		Assert.Equal("Lightning Bolt", c.CardName);
		Assert.Equal("World Championship Decks", c.Set);
		Assert.Equal(Treatment.Normal, c.Treatment);
	}
}
