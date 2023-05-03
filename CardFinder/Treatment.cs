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
	Borderless = 1 << 6,
	JapaneseAlternateArt = 1 << 7,
	Textless = 1 << 8,
	Timeshifted = 1 << 9,
	AlternateArt = 1 << 10,

	NotTournamentLegal = 1 << 11,
	Oversized = 1 << 12,
	ArtCard = 1 << 13,

	/// <summary>
	/// Different for each set, usually a fancy border or alt art, or sketch art (mh2)
	/// </summary>
	Showcase = 1 << 14,

	Expeditions = 1 << 15,
	Phyrexian = 1 << 16,
	Schematic = 1 << 17,
}