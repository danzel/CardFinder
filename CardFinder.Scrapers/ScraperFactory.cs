using CardFinder.Scrapers.BinderPos;
using CardFinder.Scrapers.Helpers;
using CardFinder.Scrapers.SingleSite;
using Microsoft.Extensions.Logging;

namespace CardFinder.Scrapers;
public class ScraperFactory
{
	private readonly ILoggerFactory _loggerFactory;
	private readonly ICachingHttpClient _httpClient;

	public ScraperFactory(ILoggerFactory loggerFactory, ICachingHttpClient httpClient)
	{
		_loggerFactory = loggerFactory;
		_httpClient = httpClient;
	}

	#region NZ
	public BayDragonCoNzScraper BayDragonCoNz => new(_loggerFactory.CreateLogger<BayDragonCoNzScraper>(), _httpClient, new DefaultConditionParser(), new DefaultTreatmentParser());
	public HobbyMasterCoNzScraper HobbyMasterCoNz => new(_loggerFactory.CreateLogger<HobbyMasterCoNzScraper>(), _httpClient, new DefaultConditionParser(), new HobbyMasterTreatmentParser());

	public BinderPosScraper BeaDndGamesCoNz => new(_loggerFactory.CreateLogger<BinderPosScraper>(), _httpClient, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.BeaDndGamesCoNz);
	public BinderPosScraper CalicoKeepCoNz => new(_loggerFactory.CreateLogger<BinderPosScraper>(), _httpClient, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.CalicoKeepCoNz);
	public BinderPosScraper CardMerchantCoNz => new(_loggerFactory.CreateLogger<BinderPosScraper>(), _httpClient, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.CardMerchantCoNz);
	public BinderPosScraper CardMerchantHamiltonCoNz => new(_loggerFactory.CreateLogger<BinderPosScraper>(), _httpClient, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.CardMerchantHamiltonCoNz);
	public BinderPosScraper CardMerchantNelsonCoNz => new(_loggerFactory.CreateLogger<BinderPosScraper>(), _httpClient, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.CardMerchantNelsonCoNz);
	public BinderPosScraper CardMerchantTakapunaCoNz => new(_loggerFactory.CreateLogger<BinderPosScraper>(), _httpClient, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.CardMerchantTakapunaCoNz);
	public BinderPosScraper GamingDnaCoNz => new(_loggerFactory.CreateLogger<BinderPosScraper>(), _httpClient, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.GamingDnaCoNz);
	public BinderPosScraper GoblinGamesNz => new(_loggerFactory.CreateLogger<BinderPosScraper>(), _httpClient, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.GoblinGamesNz);
	public BinderPosScraper HobbyLordsCoNz => new(_loggerFactory.CreateLogger<BinderPosScraper>(), _httpClient, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.HobbyLordsCoNz);
	public BinderPosScraper IronKnightGamingCoNz => new(_loggerFactory.CreateLogger<BinderPosScraper>(), _httpClient, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.IronKnightGamingCoNz);
	public BinderPosScraper MadLootGamesCoNz => new(_loggerFactory.CreateLogger<BinderPosScraper>(), _httpClient, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.MadLootGamesCoNz);
	public BinderPosScraper MagicAtWillisCoNz => new(_loggerFactory.CreateLogger<BinderPosScraper>(), _httpClient, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.MagicAtWillisCoNz);
	public BinderPosScraper MtgMagpieCom => new(_loggerFactory.CreateLogger<BinderPosScraper>(), _httpClient, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.MtgMagpieCom);
	public BinderPosScraper NovaGamesCoNz => new(_loggerFactory.CreateLogger<BinderPosScraper>(), _httpClient, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.NovaGamesCoNz);
	public BinderPosScraper ShuffleAndCutGamesCoNz => new(_loggerFactory.CreateLogger<BinderPosScraper>(), _httpClient, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.ShuffleAndCutGameCoNz);
	public BinderPosScraper SpellboundGamesCoNz => new(_loggerFactory.CreateLogger<BinderPosScraper>(), _httpClient, new BinderPosConditionParser(), new BinderPosTreatmentParser(), BinderPosConfiguration.SpellboundGamesCoNz);
	#endregion

	#region USA
	public CardKingdomComScraper CardKingdomCom => new(_loggerFactory.CreateLogger<CardKingdomComScraper>(), _httpClient, new CardKingdomComConditionParser(), new CardKingdomComTreatmentParser());
	public StarCityGamesComScraper StarCityGamesCom => new(_loggerFactory.CreateLogger<StarCityGamesComScraper>(), _httpClient, new DefaultConditionParser(), new StarCityGamesComTreatmentParser());
	#endregion

	#region Hong Kong
	public MtgMintCardComScraper MtgMintCardCom => new(_loggerFactory.CreateLogger<MtgMintCardComScraper>(), _httpClient, new DefaultConditionParser(), new DefaultTreatmentParser());
	#endregion
}
