using CardFinder.Scrapers.Helpers;
using CardFinder.Scrapers.SingleSite;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace CardFinder.Scrapers.Test;
public class StarCityGamesComScraperTests
{
	[Fact]
	public async Task Abrade()
	{
		var client = new Mock<ICachingHttpClient>();

		var scraper = new StarCityGamesComScraper(NullLogger<StarCityGamesComScraper>.Instance, client.Object, new DefaultConditionParser(), new StarCityGamesComTreatmentParser());

		client.SetupHttpPost(scraper.GetUrlForCardName("Abrade"), Resources.ReadResource("CardFinder.Scrapers.Test.Resources.StarCityGamesCom.search_abrade.txt"));
		var cards = await scraper.Scrape("Abrade", CancellationToken.None);

		Output.PrintResult(cards);
		Assert.Equal(21, cards.Length);

		var c = cards[1];
		Assert.Equal("Abrade", c.CardName);
		Assert.Equal(Condition.Played, c.Condition);
		Assert.Equal(0.25m, c.Price);
		Assert.Equal("Double Masters", c.Set);
		Assert.Equal(34, c.Stock);
		Assert.Equal(Treatment.Normal, c.Treatment);

		c = cards[9];
		Assert.Equal("Abrade", c.CardName);
		Assert.Equal(Condition.NearMint, c.Condition);
		Assert.Equal(2.54m, c.Price);
		Assert.Equal("Promo: General", c.Set);
		Assert.Equal(3, c.Stock);
		Assert.Equal(Treatment.FullArt, c.Treatment);
	}

	[Fact]
	public async Task AridMesa()
	{
		var client = new Mock<ICachingHttpClient>();

		var scraper = new StarCityGamesComScraper(NullLogger<StarCityGamesComScraper>.Instance, client.Object, new DefaultConditionParser(), new StarCityGamesComTreatmentParser());

		client.SetupHttpPost(scraper.GetUrlForCardName("Arid Mesa"), Resources.ReadResource("CardFinder.Scrapers.Test.Resources.StarCityGamesCom.search_aridmesa.txt"));
		var cards = await scraper.Scrape("Arid Mesa", CancellationToken.None);

		Output.PrintResult(cards);
		Assert.Equal(17, cards.Length);

		var c = cards[2];
		Assert.Equal("Arid Mesa", c.CardName);
		Assert.Equal(Condition.NearMint, c.Condition);
		Assert.Equal(22.99m, c.Price);
		Assert.Equal("Modern Horizons 2 - Retro Frame", c.Set);
		Assert.Equal(0, c.Stock);
		Assert.Equal(Treatment.RetroFrame, c.Treatment);

		c = cards[12];
		Assert.Equal("Arid Mesa", c.CardName);
		Assert.Equal(Condition.NearMint, c.Condition);
		Assert.Equal(17.99m, c.Price);
		Assert.Equal("Modern Horizons 2 - Variants", c.Set);
		Assert.Equal(1, c.Stock);
		Assert.Equal(Treatment.ExtendedArt, c.Treatment);
	}

	[Fact]
	public async Task LightningBolt()
	{
		var client = new Mock<ICachingHttpClient>();

		var scraper = new StarCityGamesComScraper(NullLogger<StarCityGamesComScraper>.Instance, client.Object, new DefaultConditionParser(), new StarCityGamesComTreatmentParser());

		client.SetupHttpPost(scraper.GetUrlForCardName("Lightning Bolt"), Resources.ReadResource("CardFinder.Scrapers.Test.Resources.StarCityGamesCom.search_lightningbolt.txt"));
		var cards = await scraper.Scrape("Lightning Bolt", CancellationToken.None);

		Output.PrintResult(cards);
		Assert.Equal(72, cards.Length);

		var c = cards[0];
		Assert.Equal("Lightning Bolt", c.CardName);
		Assert.Equal(Condition.NearMint, c.Condition);
		Assert.Equal(3.99m, c.Price);
		Assert.Equal("Double Masters 2022 - Variants", c.Set);
		Assert.Equal(0, c.Stock);
		Assert.Equal(Treatment.Foil | Treatment.Borderless, c.Treatment);

		c = cards[3];
		Assert.Equal("Lightning Bolt", c.CardName);
		Assert.Equal(Condition.NearMint, c.Condition);
		Assert.Equal(3.39m, c.Price); //Special
		Assert.Equal("Secret Lair (Secret Lair) (#084)", c.Set);
		Assert.Equal(0, c.Stock);
		Assert.Equal(Treatment.Foil | Treatment.FullArt, c.Treatment);

		c = cards[4];
		Assert.Equal("Lightning Bolt", c.CardName);
		Assert.Equal(Condition.NearMint, c.Condition);
		Assert.Equal(1.49m, c.Price); //Special
		Assert.Equal("Commander Legends: Battle for Baldur's Gate - Variants", c.Set);
		Assert.Equal(46, c.Stock);
		Assert.Equal(Treatment.Showcase, c.Treatment);
	}
}