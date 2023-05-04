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
				"expeditions" => Treatment.Expeditions,
				"extended art" => Treatment.ExtendedArt,
				"foil" => Treatment.Foil,
				"foil etched" => Treatment.Foil | Treatment.Etched,
				"jp alternate art" => Treatment.JapaneseAlternateArt,
				"non english" => Treatment.NonEnglish,
				"retro" => Treatment.RetroFrame,
				"retro foil etched" => Treatment.RetroFrame | Treatment.Foil | Treatment.Etched,
				"retro frame" => Treatment.RetroFrame,
				"showcase" => Treatment.Showcase,
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
