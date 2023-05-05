using AngleSharp.Html.Dom;
using AngleSharp;
using CardFinder.Scrapers.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;

namespace CardFinder.Scrapers.SingleSite;
public class MtgMintCardComScraper : IScraper
{
	private readonly ILogger<MtgMintCardComScraper> _logger;
	private readonly ICachingHttpClient _httpClient;
	private readonly DefaultConditionParser _conditionParser;
	private readonly DefaultTreatmentParser _treatmentParser;

	public MtgMintCardComScraper(ILogger<MtgMintCardComScraper> logger, ICachingHttpClient httpClient, DefaultConditionParser conditionParser, DefaultTreatmentParser treatmentParser)
	{
		_logger = logger;
		_httpClient = httpClient;
		_conditionParser = conditionParser;
		_treatmentParser = treatmentParser;
	}

	/// <summary>
	/// Return the "Regular" url
	/// </summary>
	public string GetUrlForCardName(string searchCardName)
	{
		return "https://www.mtgmintcard.com/mtg/singles/search?ed=0&action=normal_search&keywords=" + Uri.EscapeDataString(searchCardName);
	}

	public string GetUrlForCardNameVariants(string searchCardName)
	{
		return "https://www.mtgmintcard.com/mtg/singles/search?ed=0&action=normal_search&fil_ml=15&keywords=" + Uri.EscapeDataString(searchCardName);
		throw new NotImplementedException("");
	}

	public async Task<CardDetails[]> Scrape(string searchCardName, CancellationToken cancellationToken = default)
	{
		var uris = new[]
		{
			GetUrlForCardName(searchCardName),
			GetUrlForCardNameVariants(searchCardName)
			//TODO: Foil, Foil Variants, Prerelease, other languages
		};


		var results = new List<CardDetails>();

		foreach (var uri in uris)
		{
			//Get the search results page
			var searchPage = await _httpClient.Get(uri, cancellationToken);

			var context = BrowsingContext.New();
			var document = await context.OpenAsync(req => req.Content(searchPage).Address(uri), cancellationToken);

			foreach (var tr in document.QuerySelectorAll("#product_list_content"))
			{
				var (cardName, treatment, empty) = CardNameHelpers.SplitCardNameAndBracketedText(tr.QuerySelector(".card-name")!.TextContent);
				if (empty.Length != 0)
					throw new NotImplementedException($"Found text in square brackets: '{empty[0]}'");

				var set = ((IHtmlImageElement)tr.Children[2].FirstElementChild!).Title!;

				if (set.Contains("Art Series"))
					continue;
				if (!CardNameHelpers.CardNameMatches(searchCardName, cardName))
				{
					_logger.LogDebug("Skipping name not matching '{cardName}'", cardName);
					continue;
				}

				if (set == "Mystery Booster/The List" || set.StartsWith("Promo: ") || set == "World Championships")
				{
					set += " " + string.Join(", ", treatment);
					treatment = Array.Empty<string>();
				}


				var conditions = tr.Children[4].Children.Select(c => c.TextContent).ToArray();
				var prices = tr.Children[5].Children.Select(c => c.TextContent == "" ? default(decimal?) : decimal.Parse(c.TextContent[1..])).ToArray();
				var stock = tr.Children[7].Children.Where(c => c is IHtmlDivElement).Select(c => FindStock(c)).ToArray();

				if (conditions.Length != prices.Length || conditions.Length != stock.Length)
					throw new Exception($"Length of things isn't matching {conditions.Length} {prices.Length} {stock.Length}");

				for (var i = 0; i < conditions.Length; i++)
				{
					if (conditions[i] == "" || !prices[i].HasValue)
						continue;

					results.Add(new CardDetails
					{
						CardName = cardName,
						Condition = _conditionParser.Parse(conditions[i]),
						Currency = Currency.USD,
						Price = prices[i]!.Value,
						Set = set,
						Stock = stock[i],
						Treatment = _treatmentParser.Parse(treatment),

						ImageUrl = ((IHtmlImageElement)tr.Children[0].FirstElementChild!.FirstElementChild!).Source,
						ProductUrl = ((IHtmlAnchorElement)tr.Children[1].FirstElementChild!.FirstElementChild!).Href
					});
				}
			}
		}
		return results.ToArray();
	}

	private int FindStock(IElement c)
	{
		if (!c.TextContent.Contains("Add to Cart"))
			return 0;

		return int.Parse(c.QuerySelector("#selectable")!.LastElementChild!.TextContent);
	}
}
