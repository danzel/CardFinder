namespace CardFinder;

[Flags]
public enum Treatment
{
	Normal = 0,

	Foil = 1 << 1,

	RetroFrame = 1 << 2,
	FullArt = 1 << 3,
	ExtendedArt = 1 << 4,
	Etched = 1 << 5,
	Galaxy = 1 << 6,
	Borderless = 1 << 7,
	JapaneseAlternateArt = 1 << 8,
	Textless = 1 << 9,
	Timeshifted = 1 << 10,
	AlternateArt = 1 << 11,

	/// <summary>
	/// Has the logo on the art
	/// </summary>
	PromoPack = 1 << 12,

	NotTournamentLegal = 1 << 13,
	Oversized = 1 << 14,
	ArtCard = 1 << 15,

	/// <summary>
	/// Used on BinderPos sites
	/// </summary>
	NonEnglish = 1 << 16,

	/// <summary>
	/// Different for each set, usually a fancy border or alt art, or sketch art (mh2)
	/// </summary>
	Showcase = 1 << 17,

	Expeditions = 1 << 18,
	Phyrexian = 1 << 19,
	Schematic = 1 << 20,
}