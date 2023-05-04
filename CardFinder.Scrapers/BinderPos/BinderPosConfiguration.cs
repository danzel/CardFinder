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
		ImageSelector = ".productCard__img"
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