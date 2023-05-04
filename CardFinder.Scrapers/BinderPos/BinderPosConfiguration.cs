namespace CardFinder.Scrapers.BinderPos;

public record BinderPosConfiguration
{
	public required string UriRoot { get; init; }

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
		AdditionalQueryText = "+product_type%3Amtg",
		CardContainerSelector = ".productCard__card",
		ProductTitleSelector = ".productCard__title",
		PriceSelector = ".productCard__price",
		
		ChipSelector = ".productChip",
		OutOfStockSelector = ".productCard__button--outOfStock",
		SetNameSelector = ".productCard__setName",
		ImageSelector = ".productCard__img"
	};

	public static BinderPosConfiguration SpellboundGamesCoNz { get; } = NzDefaults with
	{
		UriRoot = "https://spellboundgames.co.nz",
	};
}