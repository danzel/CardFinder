using System.Text.RegularExpressions;

namespace CardFinder.Scrapers.Helpers;

interface ITreatmentParser
{
	Treatment Parse(IEnumerable<string> treatments);
}

public class DefaultTreatmentParser : ITreatmentParser
{
	public Treatment Parse(IEnumerable<string> treatments)
	{
		Treatment res = 0;

		foreach (var treatment in treatments)
		{
			res |= treatment.ToLowerInvariant() switch
			{
				"borderless" => Treatment.Borderless,
				"borderless alternate art" => Treatment.Borderless | Treatment.AlternateArt,
				"etched foil" => Treatment.Etched | Treatment.Foil,
				"expeditions" => Treatment.Expeditions,
				"extended" => Treatment.ExtendedArt,
				"extended art" => Treatment.ExtendedArt,
				"foil" => Treatment.Foil,
				"foil etched" => Treatment.Foil | Treatment.Etched,
				"full art" => Treatment.FullArt,
				"japanese" => Treatment.JapaneseAlternateArt,
				"japanese etched foil" => Treatment.JapaneseAlternateArt | Treatment.Etched | Treatment.Foil,
				"japanese foil etched" => Treatment.JapaneseAlternateArt | Treatment.Foil | Treatment.Etched,
				"jp alternate art" => Treatment.JapaneseAlternateArt,
				"non english" => Treatment.NonEnglish,
				"not tournament legal" => Treatment.NotTournamentLegal,
				"retro" => Treatment.RetroFrame,
				"retro etched foil" => Treatment.RetroFrame | Treatment.Etched | Treatment.Foil,
				"retro foil etched" => Treatment.RetroFrame | Treatment.Foil | Treatment.Etched,
				"retro frame" => Treatment.RetroFrame,
				"showcase" => Treatment.Showcase,
				"textless" => Treatment.Textless,
				_ => CustomParse(treatment.ToLowerInvariant())
			};
		}

		return res;
	}

	public virtual Treatment CustomParse(string treatment)
	{
		throw new NotImplementedException($"Don't know this treatment: '{treatment}'");
	}
}

public class BinderPosTreatmentParser : DefaultTreatmentParser
{
	public override Treatment CustomParse(string treatment)
	{
		//Lightning Bolt (141/249) [The List]
		if (Regex.IsMatch(treatment, "\\d\\d\\d/\\d\\d\\d"))
			return 0;

		return base.CustomParse(treatment);
	}
}

public class StarCityGamesComTreatmentParser : DefaultTreatmentParser
{
	public override Treatment CustomParse(string treatment)
	{
		return treatment switch
		{
			"archenemy: nicol bolas" => 0, //Mystery Booster
			"bb" => 0, //Black Border
			"commander deck" => 0,
			"game day" => 0,
			"judge" => 0,
			"magicfest" => 0,
			"non-foil" => 0,
			"oversized" => Treatment.NotTournamentLegal,
			"player rewards" => 0,

			_ => base.CustomParse(treatment),
		};
	}
}