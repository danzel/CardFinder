﻿using CardFinder.Scrapers.Helpers;

namespace CardFinder.Scrapers.BinderPos;

public enum BinderPosParseMode
{
	StockInChipsDataSet,
	StockInOnClickJs,
	StockInOptionsDropdown,
}
public record BinderPosConfiguration
{
	public required string UriRoot { get; init; }

	public required BinderPosParseMode ParseMode { get; init; }

	public required string AdditionalQueryText { get; init; }

	public required string CardContainerSelector { get; init; }
	public required string ProductTitleSelector { get; init; }
	public required string PriceSelector { get; init; }

	public required string ChipSelector { get; init; }
	public required string OutOfStockSelector { get; init; }
	public required string SetNameSelector { get; init; }
	public required string ImageSelector { get; init; }

	public Currency? Currency { get; init; }

	public Func<string, (string CardName, string[] RoundBracketsText, string[] BoxBracketsText)> SplitCardNameAndBracketedText { get; init; } =  CardNameHelpers.SplitCardNameAndBracketedText;

	private static BinderPosConfiguration NzDefaults { get; } = new BinderPosConfiguration
	{
		UriRoot = null!,
		ParseMode = BinderPosParseMode.StockInChipsDataSet,
		AdditionalQueryText = "+product_type%3Amtg",

		CardContainerSelector = ".productCard__card",
		ProductTitleSelector = ".productCard__title",
		PriceSelector = ".productCard__price",

		ChipSelector = ".productChip",
		OutOfStockSelector = ".productCard__button--outOfStock",
		SetNameSelector = ".productCard__setName",
		ImageSelector = ".productCard__img",
	};

	private static BinderPosConfiguration StockInOnClickJsNzDefaults { get; } = new BinderPosConfiguration
	{
		UriRoot = null!,
		ParseMode = BinderPosParseMode.StockInOnClickJs,
		AdditionalQueryText = "+product_type%3AMTG+Single",

		CardContainerSelector = ".product.Norm",
		ProductTitleSelector = ".productTitle",
		PriceSelector = ".productPrice",

		ChipSelector = ".addNow",
		OutOfStockSelector = ".soldout",
		SetNameSelector = null!,
		ImageSelector = ".items-even",
	};

	private static BinderPosConfiguration StockInOptionsDropdown { get; } = new BinderPosConfiguration
	{
		UriRoot = null!,
		ParseMode = BinderPosParseMode.StockInOptionsDropdown,
		AdditionalQueryText = "+product_type%3AMTG+Single", // this works, but I'm not sure it's really correct

		CardContainerSelector = ".list__item",
		ProductTitleSelector = ".grid-view-item__title",
		PriceSelector = ".product-price__price", //Unused in this mode

		ChipSelector = "[name=\"id\"].product-form__variants option",
		OutOfStockSelector = "TODO",
		SetNameSelector = ".product-desc",
		ImageSelector = ".grid-view-item__image"
	};

	public static BinderPosConfiguration BeaDndGamesCoNz { get; } = NzDefaults with
	{
		UriRoot = "https://www.beadndgames.co.nz",
		AdditionalQueryText = "+product_type%3AMTG+Single"
	};

	public static BinderPosConfiguration CalicoKeepCoNz { get; } = NzDefaults with
	{
		UriRoot = "https://www.calicokeep.co.nz"
	};

	public static BinderPosConfiguration CardMerchantCoNz { get; } = NzDefaults with
	{
		UriRoot = "https://cardmerchant.co.nz",
		AdditionalQueryText = "+product_type%3AMTG+Single"
	};

	public static BinderPosConfiguration CardMerchantHamiltonCoNz { get; } = StockInOptionsDropdown with
	{
		UriRoot = "https://www.cardmerchanthamilton.co.nz",
		Currency = CardFinder.Currency.NZD,

		PriceSelector = ".qv-regularprice", //Unused in this mode
	};

	public static BinderPosConfiguration CardMerchantNelsonCoNz { get; } = NzDefaults with
	{
		UriRoot = "https://cardmerchantnelson.co.nz",
		AdditionalQueryText = "+product_type%3AMTG+Single"
	};

	public static BinderPosConfiguration CardMerchantTakapunaCoNz { get; } = StockInOnClickJsNzDefaults with
	{
		UriRoot = "https://www.cardmerchanttakapuna.co.nz",
		Currency = CardFinder.Currency.NZD,
	};

	public static BinderPosConfiguration GamingDnaCoNz { get; } = NzDefaults with
	{
		UriRoot = "https://gamingdna.co.nz",
		AdditionalQueryText = "+product_type%3AMTG+Single"
	};

	public static BinderPosConfiguration GoblinGamesNz { get; } = StockInOptionsDropdown with
	{
		UriRoot = "https://goblingames.nz",
		Currency = CardFinder.Currency.NZD,
	};

	public static BinderPosConfiguration HobbyLordsCoNz { get; } = StockInOptionsDropdown with
	{
		UriRoot = "https://www.hobbylords.co.nz",
		Currency = CardFinder.Currency.NZD,

		AdditionalQueryText = "",

		SplitCardNameAndBracketedText = (cardName) =>
		{
			var split = cardName.Split(" - ");

			//Phyrexian Arena - Phyrexia: All Will Be One (ONE) - Foil - Coll # 283
			//Name in 0, treatment in 2, set in 1
			if (split.Length == 4)
				return (split[0], new[] { split[2] }, new[] { split[1] } ); 

			//Clearwater Pathway // Murkwater Pathway - Secret Lair: Ultimate Edition - (SLU) - Foil - Coll # 15
			//Name in 0, treatment in 3, set in 1 (set code in 2, ignored)
			if (split.Length == 5)
				return (split[0], new[] { split[3] }, new[] { split[1] });

			return (cardName, Array.Empty<string>(), Array.Empty<string>());
		}
	};

	public static BinderPosConfiguration IronKnightGamingCoNz { get; } = StockInOptionsDropdown with
	{
		UriRoot = "https://ironknightgaming.co.nz",
		Currency = CardFinder.Currency.NZD,
	};

	public static BinderPosConfiguration MadLootGamesCoNz { get; } = NzDefaults with
	{
		UriRoot = "https://www.madlootgames.co.nz"
	};

	public static BinderPosConfiguration MagicAtWillisCoNz { get; } = NzDefaults with
	{
		UriRoot = "https://magicatwillis.co.nz"
	};

	public static BinderPosConfiguration MtgMagpieCom { get; } = StockInOnClickJsNzDefaults with
	{
		UriRoot = "https://mtgmagpie.com",
		Currency = CardFinder.Currency.NZD,
	};

	public static BinderPosConfiguration NovaGamesCoNz { get; } = StockInOptionsDropdown with
	{
		UriRoot = "https://novagames.co.nz",
		Currency = CardFinder.Currency.NZD,
	};

	public static BinderPosConfiguration ShuffleAndCutGameCoNz { get; } = StockInOnClickJsNzDefaults with
	{
		UriRoot = "https://www.shuffleandcutgames.co.nz",
		Currency = CardFinder.Currency.NZD,
	};

	public static BinderPosConfiguration SpellboundGamesCoNz { get; } = NzDefaults with
	{
		UriRoot = "https://spellboundgames.co.nz",
	};
}