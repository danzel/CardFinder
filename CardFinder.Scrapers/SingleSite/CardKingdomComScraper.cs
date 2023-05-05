using AngleSharp.Html.Dom;
using AngleSharp;
using CardFinder.Scrapers.Helpers;
using Microsoft.Extensions.Logging;

namespace CardFinder.Scrapers.SingleSite;
public class CardKingdomComScraper : IScraper
{
	private readonly ILogger<CardKingdomComScraper> _logger;
	private readonly ICachingHttpClient _httpClient;
	private readonly CardKingdomComConditionParser _conditionParser;
	private readonly CardKingdomComTreatmentParser _treatmentParser;

	public CardKingdomComScraper(ILogger<CardKingdomComScraper> logger, ICachingHttpClient httpClient, CardKingdomComConditionParser conditionParser, CardKingdomComTreatmentParser treatmentParser)
	{
		_logger = logger;
		_httpClient = httpClient;
		_conditionParser = conditionParser;
		_treatmentParser = treatmentParser;
	}

	public string GetUrlForCardName(string searchCardName)
	{
		return "https://www.cardkingdom.com/catalog/search?search=header&perPage=100&filter%5Bname%5D=" + Uri.EscapeDataString(searchCardName);
	}

	public async Task<CardDetails[]> Scrape(string searchCardName, CancellationToken cancellationToken = default)
	{

		//Get the search results page
		string uri = GetUrlForCardName(searchCardName);
		var searchPage = await _httpClient.Get(uri, cancellationToken);

		var context = BrowsingContext.New();
		var document = await context.OpenAsync(req => req.Content(searchPage).Address(uri), cancellationToken);

		var results = new List<CardDetails>();

		foreach (var div in document.QuerySelectorAll(".productCardWrapper"))
		{
			var (cardName, treatment, empty) = CardNameHelpers.SplitCardNameAndBracketedText(div.QuerySelector(".productDetailTitle")!.TextContent);
			if (empty.Length != 0)
				throw new NotImplementedException("Found text in square brackets");

			var set = div.QuerySelector(".productDetailSet a")!.TextContent.Trim();

			if (!CardNameHelpers.CardNameMatches(searchCardName, cardName))
			{
				_logger.LogDebug("Skipping name not matching " + cardName);
				continue;
			}
			if (treatment.Contains("Not Tournament Legal"))
				continue;
			if (set.EndsWith("\n\nFOIL"))
			{
				treatment = treatment.Concat(new[] { "Foil" }).ToArray();
				set = set[..^6];
			}

			if (!new[] { "(S)", "(M)", "(R)", "(U)", "(C)" }.Any(set.EndsWith))
			{
				throw new NotImplementedException("Set doesn't end right " + set);
			}
			set = set[0..^3].Trim();

			if (set == "Promotional" || set == "Mystery Booster/The List")
			{
				set = set + " " + string.Join(", ", treatment);
				treatment = Array.Empty<string>();
			}
			treatment = treatment.SelectMany(x => x.Split(" - ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)).ToArray();

			var imageUrl = div.QuerySelector("mtg-card-image")!.Attributes["src"]!.Value;
			var productUrl = ((IHtmlAnchorElement)div.QuerySelector(".productDetailTitle a")!).Href;

			foreach (var conditionBox in div.QuerySelectorAll(".itemAddToCart"))
			{
				var condition =
					conditionBox.ClassList.Contains("NM") ? "NM" :
					conditionBox.ClassList.Contains("EX") ? "EX" :
					conditionBox.ClassList.Contains("VG") ? "VG" :
					conditionBox.ClassList.Contains("G") ? "G" :
					throw new NotImplementedException("Not sure what condition " + conditionBox.ClassName);

				int quantity;
				decimal price;

				if (conditionBox.QuerySelector(".outOfStockNotice") != null)
				{
					quantity = 0;
					price = decimal.Parse(((IHtmlInputElement)conditionBox.QuerySelector("input[name='price']")!).Value.Trim());
				}
				else
				{
					quantity = int.Parse(conditionBox.QuerySelector(".styleQty")!.TextContent);
					price = decimal.Parse(conditionBox.QuerySelector(".stylePrice")!.TextContent.Trim().Substring(1));
				}

				results.Add(new CardDetails
				{
					CardName = cardName,
					Condition = _conditionParser.Parse(condition),
					Currency = Currency.USD,
					ImageUrl = imageUrl,
					Price = price,
					ProductUrl = productUrl,
					Set = set,
					Stock = quantity,
					Treatment = _treatmentParser.Parse(treatment)
				});
			}
		}

		return results.ToArray();
	}
}
