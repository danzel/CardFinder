using CardFinder.Scrapers;

namespace CardFinder.Solver;
public class StoreFactory
{
	private readonly ScraperFactory _scraperFactory;

	public Store[] Stores
	{
		get
		{
			return new[]
			{
				new Store("BayDragon", _scraperFactory.BayDragonCoNz, 4.08m, Currency.NZD, GstHandling.Included) { ShippingIsFreeIfYouSpend = 100m }, //Technically shipping is 50c if you spend $100
				new Store("Hobby Master", _scraperFactory.HobbyMasterCoNz, 4.90m, Currency.NZD, GstHandling.Included) { ShippingIsFreeIfYouSpend = 80m },

				new Store("Bea Dnd", _scraperFactory.BeaDndGamesCoNz, 8.00m, Currency.NZD, GstHandling.Included),
				new Store("Calico Keep", _scraperFactory.CalicoKeepCoNz, 5.00m, Currency.NZD, GstHandling.Included),
				new Store("Card Merchant", _scraperFactory.CardMerchantCoNz, 7.90m, Currency.NZD, GstHandling.Included),
				new Store("Card Merchant Hamilton", _scraperFactory.CardMerchantCoNz, 0.00m, Currency.NZD, GstHandling.Included), //5.50 if shipping
				new Store("Card Merchant Nelson", _scraperFactory.CardMerchantNelsonCoNz, 5.99m, Currency.NZD, GstHandling.Included) { ShippingIsFreeIfYouSpend = 200m },
				new Store("Card Merchant Takapuna", _scraperFactory.CardMerchantTakapunaCoNz, 5.00m, Currency.NZD, GstHandling.Included),
				new Store("Gaming DNA", _scraperFactory.GamingDnaCoNz, 0.00m, Currency.NZD, GstHandling.Included), //TODO: Shipping if not Hamilton
				new Store("Goblin Games", _scraperFactory.GoblinGamesNz, 8.00m, Currency.NZD, GstHandling.Included),
				new Store("Hobby Lords", _scraperFactory.HobbyLordsCoNz, 6.00m, Currency.NZD, GstHandling.Included),
				new Store("Iron Knight Gaming", _scraperFactory.IronKnightGamingCoNz, 12.99m, Currency.NZD, GstHandling.Included),
				new Store("Magic at Willis", _scraperFactory.MagicAtWillisCoNz, 7.50m, Currency.NZD, GstHandling.Included), //Courier Insured (can do Post 5.50)
				new Store("Mtg Magpie", _scraperFactory.MtgMagpieCom, 9.20m, Currency.NZD, GstHandling.Included) { ShippingIsFreeIfYouSpend = 70m }, //Tracked Courier (Can do Tracked shipping 6.70)
				new Store("Nova Games", _scraperFactory.NovaGamesCoNz, 10.00m, Currency.NZD, GstHandling.Included),
				new Store("Shuffle and Cut", _scraperFactory.ShuffleAndCutGamesCoNz, 6.85m, Currency.NZD, GstHandling.Included), //Insured to $80 (8.00 for $250)
				new Store("Spellbound Games", _scraperFactory.SpellboundGamesCoNz, 8.00m, Currency.NZD, GstHandling.Included),

				//new Store("Card Kingdom", scraperFactory.CardKingdomCom, 28.33m, Currency.USD, GstHandling.AddedToTotal), //UPS Worldwide Saver Express 2-3 days (8.44 for 5-21 days Asendia Fully Tracked)
				//new Store("Star City Games", scraperFactory.StarCityGamesCom, 34.49m, Currency.USD, GstHandling.Included), //GST Last tested 2022-09. UPS Worldwide Express Saver 11 days (6.68 for USPS 1 month)
				//new Store("MTG Mint Card", scraperFactory.MtgMintCardCom, 22.00m, Currency.USD, GstHandling.Included), //Express shipping + insurance, could do airmail for $2.50
			};
		}
	}

	public StoreFactory(ScraperFactory scraperFactory)
	{
		_scraperFactory = scraperFactory;
	}
}
