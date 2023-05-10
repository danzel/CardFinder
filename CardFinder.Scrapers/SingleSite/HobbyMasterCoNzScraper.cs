using CardFinder.Scrapers.Helpers;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CardFinder.Scrapers.SingleSite;
public class HobbyMasterCoNzScraper : IScraper
{
	private const string GetCardsTemplate = "https://hobbymaster.co.nz/cards/get-cards?lang=&game=1&foil=&_search=true&rows=100&page=1&sidx=stock&sord=desc&name=";

	private readonly ILogger<HobbyMasterCoNzScraper> _logger;
	private readonly ICachingHttpClient _httpClient;
	private readonly DefaultConditionParser _conditionParser;
	private readonly HobbyMasterTreatmentParser _treatmentParser;

	public HobbyMasterCoNzScraper(ILogger<HobbyMasterCoNzScraper> logger, ICachingHttpClient httpClient, DefaultConditionParser conditionParser, HobbyMasterTreatmentParser treatmentParser)
	{
		_logger = logger;
		_httpClient = httpClient;
		_conditionParser = conditionParser;
		_treatmentParser = treatmentParser;
	}

	public string GetUrlForCardName(string searchCardName)
	{
		return GetCardsTemplate + Uri.EscapeDataString(searchCardName);
	}

	public async Task<CardDetails[]> Scrape(string searchCardName, CancellationToken cancellationToken = default)
	{
		//Get the search results from their json endpoint
		var searchJson = await _httpClient.Get(GetUrlForCardName(searchCardName), cancellationToken);

		var response = JsonSerializer.Deserialize<GetCardsResponse>(searchJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true, })!;

		var result = new List<CardDetails>();

		foreach (var row in response.Rows)
		{
			//Strip condition off here as it has its own cell too
			var nameCell = row.Cell[0]!;
			if (nameCell.EndsWith(" (SP)") || nameCell.EndsWith(" (MP)"))
			{
				nameCell = nameCell[0..^5];
			}

			Treatment additionalTreatment = 0;
			if (nameCell.EndsWith(" Art Card"))
			{
				additionalTreatment |= Treatment.ArtCard;
				nameCell = nameCell[0..^9];
			}
			if (nameCell.EndsWith(" Art Card (Gold-Stamped Signature)"))
			{
				additionalTreatment |= Treatment.ArtCard; //TODO: Gold-Stamped Signature
				nameCell = nameCell[0..^34];
			}
			if (nameCell.Contains(" - ")) //"Lightning Bolt - 1996 Eric Tam (4ED)" (World Championship Decks)
			{
				nameCell = nameCell[0..nameCell.IndexOf(" - ")];
				//TODO: Could put this in the set
			}

			var (cardName, treatments, empty) = CardNameHelpers.SplitCardNameAndBracketedText(nameCell);
			if (empty.Length != 0)
				throw new NotImplementedException($"Found text in square brackets: '{empty[0]}'");

			if (!CardNameHelpers.CardNameMatches(searchCardName, cardName))
			{
				_logger.LogDebug("Skipping name not matching '{cardName}'", cardName);
				continue;
			}

			result.Add(new CardDetails
			{
				CardName = cardName,
				Condition = _conditionParser.Parse(row.Cell[9]!),
				Currency = Currency.NZD,
				ImageUrl = row.Cell[16],
				Price = decimal.Parse(row.Cell[10]!.Replace("$", "").Replace("!", "")), //! for special
				ProductUrl = null,
				Set = row.Cell[1]!,
				Stock = int.Parse(row.Cell[12]!.ToString().Replace("+", "")), //"8+" when they have that many
				Treatment = _treatmentParser.Parse(treatments) | additionalTreatment
			});
		}

		return result.ToArray();
	}

	class GetCardsResponse
	{
		public string Page { get; set; } = null!;
		public int Total { get; set; }
		public string Records { get; set; } = null!;

		public GetCardsRow[] Rows { get; set; } = null!;
	}

	class GetCardsRow
	{
		public string Id { get; set; } = null!;

		public GetCardsRowCell Cell { get; set; } = null!;
	}

	[JsonConverter(typeof(GetCardsRowCellConverter))]
	class GetCardsRowCell
	{
		public List<string?> Contents = new List<string?>();

		public string? this[int i]
		{
			get { return Contents[i]; }
			set { Contents[i] = value; }
		}
	}

	class GetCardsRowCellConverter : JsonConverter<GetCardsRowCell>
	{
		public override GetCardsRowCell Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var doc = JsonDocument.ParseValue(ref reader);

			var res = new GetCardsRowCell();
			foreach (var ele in doc.RootElement.EnumerateArray())
			{
				switch (ele.ValueKind)
				{
					case JsonValueKind.Number:
						res.Contents.Add(ele.GetInt32().ToString());
						break;
					case JsonValueKind.String:
						res.Contents.Add(ele.GetString()!);
						break;
					case JsonValueKind.Null:
						res.Contents.Add(null);
						break;
					default:
						throw new NotImplementedException();
				}
			}
			return res;
		}

		public override void Write(Utf8JsonWriter writer, GetCardsRowCell value, JsonSerializerOptions options)
		{
			throw new NotImplementedException();
		}
	}
}

public class HobbyMasterTreatmentParser : DefaultTreatmentParser
{
	public override Treatment CustomParse(string treatment)
	{
		return treatment switch
		{
			"a25" => 0, //Set for "The List" cards
			"ce" => 0, //Collector's Edition
			"ie" => 0, //International Edition
			_ when int.TryParse(treatment, out _) => 0, //Number for secret lair drop series
			_ => base.CustomParse(treatment),
		};
	}
}
