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
	Textured = 1 << 7,

	Borderless = 1 << 8,
	JapaneseAlternateArt = 1 << 9,
	Textless = 1 << 10,
	Timeshifted = 1 << 11,
	AlternateArt = 1 << 12,

	/// <summary>
	/// Has the logo on the art
	/// </summary>
	PromoPack = 1 << 13,

	NotTournamentLegal = 1 << 14,
	Oversized = 1 << 15,
	ArtCard = 1 << 16,

	/// <summary>
	/// Used on BinderPos sites
	/// </summary>
	NonEnglish = 1 << 17,

	/// <summary>
	/// Different for each set, usually a fancy border or alt art, or sketch art (mh2)
	/// </summary>
	Showcase = 1 << 18,

	Expeditions = 1 << 19,
	Phyrexian = 1 << 20,
	Schematic = 1 << 21,
	SerialNumbered = 1 << 22,
}