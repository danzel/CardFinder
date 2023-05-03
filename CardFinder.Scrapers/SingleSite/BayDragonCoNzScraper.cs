using AngleSharp;
using AngleSharp.Html.Dom;
using CardFinder.Scrapers.Helpers;
using Microsoft.Extensions.Logging;

namespace CardFinder.Scrapers.SingleSite;

public class BayDragonCoNzScraper : IScraper
{
	public const string SearchPageTemplate = "https://baydragon.co.nz/searchsingle/category/01/brand/01?searchString=";

	private readonly ILogger<BayDragonCoNzScraper> _logger;
	private readonly ICachingHttpClient _httpClient;
	private readonly DefaultConditionParser _conditionParser;
	private readonly DefaultTreatmentParser _treatmentParser;

	public BayDragonCoNzScraper(ILogger<BayDragonCoNzScraper> logger, ICachingHttpClient httpClient, DefaultConditionParser conditionParser, DefaultTreatmentParser treatmentParser)
	{
		_logger = logger;
		_httpClient = httpClient;
		_conditionParser = conditionParser;
		_treatmentParser = treatmentParser;
	}

	public static string GetUrlForCardName(string searchCardName)
	{
		return SearchPageTemplate + Uri.EscapeDataString(searchCardName); ;
	}

	public async Task<CardDetails[]> Scrape(string searchCardName, CancellationToken cancellationToken = default)
	{
		//Get the list page
		string uri = GetUrlForCardName(searchCardName);
		var searchPage = await _httpClient.Get(uri, cancellationToken);

		//TODO: There can be multiple pages of results

		var context = BrowsingContext.New();
		var document = await context.OpenAsync(req => req.Content(searchPage).Address(uri), cancellationToken);

		//Rows in the results table
		var results = new List<CardDetails>();
		foreach (var tr in document.QuerySelectorAll("div#tcgSingles tr:not(:first-child)"))
		{
			var (cardName, treatments, bonusTreatments) = CardNameHelpers.SplitCardNameAndBracketedText(tr.Children[1].TextContent.Trim());

			var imageNode = tr.Children[0].FirstElementChild?.FirstElementChild;

			if (!CardNameHelpers.CardNameMatches(searchCardName, cardName))
			{
				_logger.LogDebug("Skipping name not matching '{cardName}'", cardName);
				continue;
			}


			results.Add(new CardDetails
			{
				CardName = cardName,
				Treatment = _treatmentParser.Parse(treatments.Concat(bonusTreatments)),

				Condition = _conditionParser.Parse(tr.Children[5].TextContent.Trim()),
				Price = decimal.Parse(tr.Children[6].TextContent.Trim().Replace("NZ$", "")),
				Currency = Currency.NZD,
				Set = tr.Children[2].TextContent.Trim(),
				Stock = int.Parse(tr.Children[7].TextContent.Trim()),

				ProductUrl = ((IHtmlAnchorElement)tr.Children[1].FirstElementChild!).Href,
				ImageUrl = imageNode != null ? ((IHtmlImageElement)imageNode).Source!.Replace("_small.", "_large.") : null
			});
		}

		return results.ToArray();
	}
}
