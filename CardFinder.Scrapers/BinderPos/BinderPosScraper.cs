using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using CardFinder.Scrapers.Helpers;
using Microsoft.Extensions.Logging;

namespace CardFinder.Scrapers.BinderPos;
public class BinderPosScraper : IScraper
{
	private readonly ILogger<BinderPosScraper> _logger;
	private readonly ICachingHttpClient _httpClient;
	private readonly DefaultConditionParser _conditionParser;
	private readonly BinderPosTreatmentParser _treatmentParser;
	private readonly BinderPosConfiguration _configuration;

	public BinderPosScraper(ILogger<BinderPosScraper> logger, ICachingHttpClient httpClient, BinderPosConditionParser conditionParser, BinderPosTreatmentParser treatmentParser, BinderPosConfiguration configuration)
	{
		_logger = logger;
		_httpClient = httpClient;
		_conditionParser = conditionParser;
		_treatmentParser = treatmentParser;
		_configuration = configuration;
	}

	public string GetUrlForCardName(string searchCardName)
	{
		return _configuration.UriRoot + "/search?page=1&q=" + Uri.EscapeDataString("*" + searchCardName + "*") + _configuration.AdditionalQueryText;
	}

	public async Task<CardDetails[]> Scrape(string searchCardName, CancellationToken cancellationToken = default)
	{
		//Get the search results page
		var uri = GetUrlForCardName(searchCardName);
		var searchPage = await _httpClient.Get(uri, cancellationToken);

		var context = BrowsingContext.New();
		var document = await context.OpenAsync(req => req.Content(searchPage).Address(uri), cancellationToken);

		var results = new List<CardDetails>();

		var productDivs = document.QuerySelectorAll(_configuration.CardContainerSelector);
		if (productDivs.Length == 0)
			_logger.LogWarning("Didn't find any products?");

		foreach (var div in productDivs)
		{
			var (cardName, treatment, set) = CardNameHelpers.SplitCardNameAndBracketedText(div.QuerySelector(_configuration.ProductTitleSelector)!.TextContent.Trim());

			if (!CardNameHelpers.CardNameMatches(searchCardName, cardName))
			{
				_logger.LogDebug("Skipping name not matching " + cardName);
				continue;
			}

			var defaultPriceStr = div.QuerySelector(_configuration.PriceSelector)!.TextContent.Trim();
			Currency currency;

			if (_configuration.Currency.HasValue)
			{
				currency = _configuration.Currency.Value;
				defaultPriceStr = defaultPriceStr.Substring(1);
			}
			else if (defaultPriceStr.EndsWith("NZD"))
			{
				currency = Currency.NZD;
				defaultPriceStr = defaultPriceStr.Substring(1, defaultPriceStr.IndexOf(' ') - 1);
			}
			else
				throw new NotImplementedException("Currency");

			switch (_configuration.ParseMode)
			{
				case BinderPosParseMode.StockInChipsDataSet:
					if (set.Length != 0)
						throw new NotImplementedException($"Found text in square brackets: '{set[0]}'");
					ParseStockInChipsDataset(ref results, cardName, div, treatment, currency);
					break;
				case BinderPosParseMode.StockInOnClickJs:
					if (set.Length != 1)
						throw new NotImplementedException($"Found more than one text in square brackets: '{string.Join(',', set)}'");
					ParseStockInOnClickJs(ref results, cardName, div, treatment, set[0], currency);
					break;
				case BinderPosParseMode.StockInOptionsDropdown:
					throw new NotImplementedException();
					//ParseStockInOptionsDropdown(ref results, cardName, div, treatment, set[0], currency);
					break;
				default:
					throw new NotImplementedException(_configuration.ParseMode.ToString());
			}
		}

		return results.ToArray();
	}

	private void ParseStockInChipsDataset(ref List<CardDetails> results, string cardName, IElement div, string[] treatment, Currency currency)
	{
		var chips = div.QuerySelectorAll(_configuration.ChipSelector).Cast<IHtmlListItemElement>().ToArray();

		if (chips.Length == 0)
		{
			throw new NotImplementedException("Didn't find any chips, need to check if there is an OutOfStock element");
		}
		else
		{
			foreach (var chip in chips)
			{
				var price = decimal.Parse((chip.Dataset["price"] ?? chip.Dataset["variantprice"])!) / 100;
				var stock = int.Parse((chip.Dataset["variantquantity"] ?? chip.Dataset["variantqty"])!);

				var condition = chip.Dataset["varianttitle"]!;
				var ourTreatment = treatment;

				if (condition.Contains("foil", StringComparison.InvariantCultureIgnoreCase))
				{
					ourTreatment = ourTreatment.Concat(new[] { "Foil" }).ToArray();
					condition = condition.Replace("foil", "", StringComparison.InvariantCultureIgnoreCase).Trim();
				}
				if (condition.EndsWith(" non english", StringComparison.InvariantCultureIgnoreCase))
				{
					ourTreatment = ourTreatment.Concat(new[] { "non english" }).ToArray();
					condition = condition[..^12];
				}

				results.Add(new CardDetails
				{
					CardName = cardName,
					Treatment = _treatmentParser.Parse(ourTreatment),
					Set = div.QuerySelector(_configuration.SetNameSelector)!.TextContent.Trim(),
					ImageUrl = "https:" + ((IHtmlImageElement)div.QuerySelector(_configuration.ImageSelector)!).Dataset["src"],
					ProductUrl = ((IHtmlAnchorElement)div.QuerySelector("a")!).Href,
					Currency = currency,
					Price = price,
					Condition = _conditionParser.Parse(condition),
					Stock = stock
				});
			}
		}
	}

	private void ParseStockInOnClickJs(ref List<CardDetails> results, string cardName, IElement div, string[] treatment, string set, Currency currency)
	{
		var chips = div.QuerySelectorAll(_configuration.ChipSelector).Cast<IHtmlDivElement>().ToArray();

		if (chips.Length == 0)
		{
			//No stock
			if (div.QuerySelector(_configuration.OutOfStockSelector) == null)
				throw new NotImplementedException("Expected an out of stock element as there are no chips");

			results.Add(new CardDetails
			{
				CardName = cardName,
				Treatment = _treatmentParser.Parse(treatment),
				Set = set,
				ImageUrl = ((IHtmlImageElement)div.QuerySelector(_configuration.ImageSelector)!).Source,
				ProductUrl = ((IHtmlAnchorElement)div.QuerySelector("a")!).Href,
				Currency = currency,
				Price = 999.99M, //Doesn't list price for sold out things
				Condition = Condition.Unspecified,
				Stock = 0
			});
		}
		else
		{
			foreach (var chip in chips)
			{
				var conditionAndPrice = chip.QuerySelector("p")!.TextContent;
				var price = decimal.Parse(conditionAndPrice.Split('$', ')')[1].Trim());

				//addToCart('39462942900387','Barkchannel Pathway // Tidechannel Pathway (Extended Art) [Kaldheim] - Near Mint Foil','1',1)
				var stock = int.Parse(chip.Attributes["onclick"]!.Value.Split("',")[2].Trim('\''));

				var condition = conditionAndPrice.Split('-')[0].Trim();
				var ourTreatment = treatment;

				//"Near Mint Foil"
				if (condition.ToLower().Contains("foil"))
				{
					ourTreatment = ourTreatment.Concat(new[] { "Foil" }).ToArray();
					condition = condition.Replace("FOIL", "").Replace("Foil", "").Replace("foil", "").Trim();
				}

				results.Add(new CardDetails
				{
					CardName = cardName,
					Treatment = _treatmentParser.Parse(ourTreatment),
					Set = set,
					ImageUrl = "https:" + ((IHtmlImageElement)div.QuerySelector(_configuration.ImageSelector)!).Dataset["src"],
					ProductUrl = ((IHtmlAnchorElement)div.QuerySelector("a")!).Href,
					Currency = currency,
					Price = price,
					Condition = _conditionParser.Parse(condition),
					Stock = stock
				});
			}
		}
	}
}
