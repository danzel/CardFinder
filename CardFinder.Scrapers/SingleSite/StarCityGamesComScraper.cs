using CardFinder.Scrapers.Helpers;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CardFinder.Scrapers.SingleSite;
public class StarCityGamesComScraper : IScraper
{
	private readonly ILogger<StarCityGamesComScraper> _logger;
	private readonly ICachingHttpClient _httpClient;
	private readonly DefaultConditionParser _conditionParser;
	private readonly StarCityGamesComTreatmentParser _treatmentParser;

	public StarCityGamesComScraper(ILogger<StarCityGamesComScraper> logger, ICachingHttpClient httpClient, DefaultConditionParser conditionParser, StarCityGamesComTreatmentParser treatmentParser)
	{
		_logger = logger;
		_httpClient = httpClient;
		_conditionParser = conditionParser;
		_treatmentParser = treatmentParser;
	}

	public string GetUrlForCardName(string searchCardName)
	{
		return "https://essearchapi-na.hawksearch.com/api/v2/search";
	}

	public async Task<CardDetails[]> Scrape(string searchCardName, CancellationToken cancellationToken = default)
	{
		var payload = JsonSerializer.Serialize(new RequestPayload
		{
			Keyword = searchCardName,
			Variant = new RequestPayloadVariant { MaxPerPage = 32 },
			SortBy = "score",
			MaxPerPage = "96",
			clientguid = "cc3be22005ef47d3969c3de28f09571b" //What the page was using
		});

		var responseJson = await _httpClient.Post(GetUrlForCardName(searchCardName), new StringContent(payload, null, "application/json"), ConfigureRequestHeaders, cancellationToken);
		var response = JsonSerializer.Deserialize<ResultPayload>(responseJson)!;

		var res = new List<CardDetails>();

		foreach (var card in response.Results.Select(x => x.Document))
		{
			var cardName = card.card_name.Single();
			var cardLanguage = card.language.Single();
			var finish = card.finish.Single();
			var subtitle = card.subtitle?.Single();
			var set = card.filter_set.Single();

			if (!CardNameHelpers.CardNameMatches(searchCardName, cardName))
			{
				_logger.LogDebug("Skipping name not matching '{cardName}'", cardName);
				continue;
			}

			if (set == "Promo: Date Stamped" || set == "The List")
			{
				set += " " + subtitle;
				subtitle = null;
			}
			if (set == "World Championship")
			{
				set += " " + subtitle;
				subtitle = "(Not Tournament Legal)";
			}
			if (set == "Secret Lair")
			{
				string? keepSubtitle = null;
				if (subtitle!.EndsWith(" (Full Art)"))
				{
					keepSubtitle += " (Full Art)";
					subtitle = subtitle[..^11];
				}
				set += " " + subtitle;
				subtitle = keepSubtitle;
			}

			var treatments = new[] { finish };
			if (subtitle != null)
			{
				var (empty, circle, square) = CardNameHelpers.SplitCardNameAndBracketedText(subtitle);
				if (square.Length != 0)
					throw new NotImplementedException("Found square brackets");

				treatments = treatments.Concat(circle).ToArray();

				if (cardLanguage == "Japanese")
				{
					var index = Array.FindIndex(treatments, x => x == "Alternate Art");
					if (index != -1)
						treatments[index] = "Jp Alternate Art";
				}
			}
			var treatment = _treatmentParser.Parse(treatments);

			foreach (var child in card.hawk_child_attributes.Where(c => c.price != null))
			{
				var childTreatment = treatment;

				var condition = _conditionParser.Parse(child.condition.Single());
				var image = card.image.Single();

				var language = child.variant_language.Single();
				if (language != "English")
					treatment |= Treatment.NonEnglish;

				decimal price;
				if (child.variant_price_sale.Single() == "0")
					price = decimal.Parse(child.price.Single());
				else
					price = decimal.Parse(child.variant_price_sale.Single());

				res.Add(new CardDetails
				{
					CardName = cardName,
					Condition = condition,
					Currency = Currency.USD,
					ImageUrl = image,
					Price = price,
					ProductUrl = "https://starcitygames.com" + card.url_detail.Single(),
					Set = set,
					Stock = int.Parse(child.qty.Single()),
					Treatment = childTreatment
				});
			}
		}

		return res.ToArray();
	}

	private static void ConfigureRequestHeaders(HttpRequestHeaders headers)
	{
		headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
		headers.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-US", 0.5));
		headers.Add("Origin", "https://starcitygames.com");
		headers.Add("Referer", "https://starcitygames.com/");
		headers.Add("DNT", "1");
		headers.Add("Sec-Fetch-Dest", "empty");
		headers.Add("Sec-Fetch-Mode", "cors");
		headers.Add("Sec-Fetch-Site", "cross-site");
		headers.Add("Sec-GPC", "1");
	}

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	class RequestPayload
	{
		public string Keyword { get; set; }
		public RequestPayloadVariant Variant { get; set; }
		public string SortBy { get; set; }
		public string MaxPerPage { get; set; }
		public string clientguid { get; set; }
	}
	class RequestPayloadVariant
	{
		public int MaxPerPage { get; set; }
	}

	class ResultPayload
	{
		public ResultItem[] Results { get; set; }
		//Heaps of fields we aren't including
	}
	class ResultItem
	{
		public ResultItemDocument Document { get; set; }
		//Heaps of fields we aren't including
	}
	class ResultItemDocument
	{
		public string[] card_name { get; set; }
		public string[] language { get; set; }
		public string[] image { get; set; }
		public string[] subtitle { get; set; } //(Borderless)
		public string[] url_detail { get; set; }
		public string[] finish { get; set; } //Foil
		public string[] filter_set { get; set; }
		public HawkChildAttributes[] hawk_child_attributes { get; set; }
	}
	class HawkChildAttributes
	{
		public string[] price { get; set; }
		public string[] qty { get; set; }
		public string[] variant_language { get; set; }
		public string[] variant_is_on_sale { get; set; }
		public string[] condition { get; set; }
		public string[] variant_price_sale { get; set; }
	}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
