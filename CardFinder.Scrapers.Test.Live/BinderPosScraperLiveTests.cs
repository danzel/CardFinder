using CardFinder.Scrapers.BinderPos;
using CardFinder.Scrapers.Helpers;
using CardFinder.Scrapers.SingleSite;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFinder.Scrapers.Test.Live;
public class BinderPosScraperLiveTests : LiveTestsBase
{
	[Fact]
	public async Task BeaDndGamesCoNz()
	{
		var scraper = new BinderPosScraper(Logger.CreateLogger<BinderPosScraper>(), DirectHttpClient, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.BeaDndGamesCoNz);

		var cards = await scraper.Scrape("Arid Mesa");

		Assert.NotEmpty(cards);
		Console.WriteLine($"Found {cards.Length} cards");
	}

	[Fact]
	public async Task CalicoKeepCoNz()
	{
		var scraper = new BinderPosScraper(Logger.CreateLogger<BinderPosScraper>(), DirectHttpClient, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.CalicoKeepCoNz);

		var cards = await scraper.Scrape("Arid Mesa");

		Assert.NotEmpty(cards);
		Console.WriteLine($"Found {cards.Length} cards");
	}

	[Fact]
	public async Task CardMerchantCoNz()
	{
		var scraper = new BinderPosScraper(Logger.CreateLogger<BinderPosScraper>(), DirectHttpClient, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.CardMerchantCoNz);

		var cards = await scraper.Scrape("Arid Mesa");

		Assert.NotEmpty(cards);
		Console.WriteLine($"Found {cards.Length} cards");
	}

	[Fact]
	public async Task CardMerchantHamiltonCoNz()
	{
		var scraper = new BinderPosScraper(Logger.CreateLogger<BinderPosScraper>(), DirectHttpClient, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.CardMerchantHamiltonCoNz);

		var cards = await scraper.Scrape("Arid Mesa");

		Assert.NotEmpty(cards);
		Console.WriteLine($"Found {cards.Length} cards");
	}

	[Fact]
	public async Task CardMerchantNelsonCoNz()
	{
		var scraper = new BinderPosScraper(Logger.CreateLogger<BinderPosScraper>(), DirectHttpClient, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.CardMerchantNelsonCoNz);

		var cards = await scraper.Scrape("Arid Mesa");

		Assert.NotEmpty(cards);
		Console.WriteLine($"Found {cards.Length} cards");
	}

	[Fact]
	public async Task CardMerchantTakapunaCoNz()
	{
		var scraper = new BinderPosScraper(Logger.CreateLogger<BinderPosScraper>(), DirectHttpClient, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.CardMerchantTakapunaCoNz);

		var cards = await scraper.Scrape("Arid Mesa");

		Assert.NotEmpty(cards);
		Console.WriteLine($"Found {cards.Length} cards");
	}

	[Fact]
	public async Task GamingDnaCoNz()
	{
		var scraper = new BinderPosScraper(Logger.CreateLogger<BinderPosScraper>(), DirectHttpClient, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.GamingDnaCoNz);

		var cards = await scraper.Scrape("Arid Mesa");

		Assert.NotEmpty(cards);
		Console.WriteLine($"Found {cards.Length} cards");
	}

	[Fact]
	public async Task GoblinGamesNz()
	{
		var scraper = new BinderPosScraper(Logger.CreateLogger<BinderPosScraper>(), DirectHttpClient, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.GoblinGamesNz);

		var cards = await scraper.Scrape("Arid Mesa");

		Assert.NotEmpty(cards);
		Console.WriteLine($"Found {cards.Length} cards");
	}

	[Fact]
	public async Task IronKnightGamingCoNz()
	{
		var scraper = new BinderPosScraper(Logger.CreateLogger<BinderPosScraper>(), DirectHttpClient, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.IronKnightGamingCoNz);

		var cards = await scraper.Scrape("Arid Mesa");

		Assert.NotEmpty(cards);
		Console.WriteLine($"Found {cards.Length} cards");
	}

	[Fact]
	public async Task MadLootGamesCoNz()
	{
		var scraper = new BinderPosScraper(Logger.CreateLogger<BinderPosScraper>(), DirectHttpClient, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.MadLootGamesCoNz);

		var cards = await scraper.Scrape("Arid Mesa");

		Assert.NotEmpty(cards);
		Console.WriteLine($"Found {cards.Length} cards");
	}

	[Fact]
	public async Task MagicAtWillisCoNz()
	{
		var scraper = new BinderPosScraper(Logger.CreateLogger<BinderPosScraper>(), DirectHttpClient, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.MagicAtWillisCoNz);

		var cards = await scraper.Scrape("Arid Mesa");

		Assert.NotEmpty(cards);
		Console.WriteLine($"Found {cards.Length} cards");
	}

	[Fact]
	public async Task ShuffleAndCutGameCoNz()
	{
		var scraper = new BinderPosScraper(Logger.CreateLogger<BinderPosScraper>(), DirectHttpClient, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.ShuffleAndCutGameCoNz);

		var cards = await scraper.Scrape("Arid Mesa");

		Assert.NotEmpty(cards);
		Console.WriteLine($"Found {cards.Length} cards");
	}

	[Fact]
	public async Task SpellboundGamesCoNz()
	{
		var scraper = new BinderPosScraper(Logger.CreateLogger<BinderPosScraper>(), DirectHttpClient, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.SpellboundGamesCoNz);

		var cards = await scraper.Scrape("Arid Mesa");

		Assert.NotEmpty(cards);
		Console.WriteLine($"Found {cards.Length} cards");
	}
}
