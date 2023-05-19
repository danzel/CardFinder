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
				"borderless concept praetors" => Treatment.Borderless | Treatment.Showcase,
				"borderless concept praetors step-and-compleat foil" => Treatment.Borderless | Treatment.Showcase | Treatment.Foil, //TODO: Needs another foil type
				"borderless ichor step-and-compleat foil" => Treatment.Borderless | Treatment.Showcase | Treatment.Foil, //TODO: Needs another foil type
				"borderless ichor" => Treatment.Borderless | Treatment.Showcase,
				"concept praetor" => Treatment.Showcase,
				"etched foil" => Treatment.Etched | Treatment.Foil,
				"expeditions" => Treatment.Expeditions,
				"extended" => Treatment.ExtendedArt,
				"extended art" => Treatment.ExtendedArt,
				"foil" => Treatment.Foil,
				"foil etched" => Treatment.Foil | Treatment.Etched,
				"full art" => Treatment.FullArt,
				"galaxy foil" => Treatment.Galaxy | Treatment.Foil,
				"japanese" => Treatment.JapaneseAlternateArt,
				"japanese etched foil" => Treatment.JapaneseAlternateArt | Treatment.Etched | Treatment.Foil,
				"japanese foil etched" => Treatment.JapaneseAlternateArt | Treatment.Foil | Treatment.Etched,
				"jpn alternate art" => Treatment.JapaneseAlternateArt,
				"jp alternate art" => Treatment.JapaneseAlternateArt,
				"non english" => Treatment.NonEnglish,
				"non foil" => 0,
				"not tournament legal" => Treatment.NotTournamentLegal,
				"oversized" => Treatment.Oversized,
				"phyrexian" => Treatment.Phyrexian,
				"promo pack" => Treatment.PromoPack,
				"retro" => Treatment.RetroFrame,
				"retro etched foil" => Treatment.RetroFrame | Treatment.Etched | Treatment.Foil,
				"retro foil etched" => Treatment.RetroFrame | Treatment.Foil | Treatment.Etched,
				"retro frame" => Treatment.RetroFrame,
				"showcase" => Treatment.Showcase,
				"showcase textured" => Treatment.Showcase | Treatment.Textured,
				"step-and-compleat foil" => Treatment.Foil, //TODO: Needs another foil type
				"step-and-complete foil" => Treatment.Foil, //TODO: Needs another foil type
				"textless" => Treatment.Textless,
				"textured foil" => Treatment.Textured | Treatment.Foil,
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

		return treatment switch
		{
			"spn" => Treatment.NonEnglish, //Spanish?
			_ => base.CustomParse(treatment)
		};
	}
}

public class CardKingdomComTreatmentParser : DefaultTreatmentParser
{
	public override Treatment CustomParse(string treatment)
	{
		return treatment switch
		{
			_ when int.TryParse(treatment, out _) => 0, //Collector number
			_ => base.CustomParse(treatment),
		};
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