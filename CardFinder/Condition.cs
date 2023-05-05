namespace CardFinder;

public enum Condition
{
	Unspecified = 0,

	NearMint = 1,

	/// <summary>
	/// Lightly Played / SP / Slightly Played
	/// </summary>
	LightlyPlayed,

	/// <summary>
	/// Played (Usually used on sites that don't have Lightly Played)
	/// </summary>
	Played,

	/// <summary>
	/// Very Good / MP / Played
	/// </summary>
	ModeratelyPlayed,

	/// <summary>
	/// Heavily Played / HP
	/// </summary>
	HeavilyPlayed,

	/// <summary>
	/// Damaged, you probably can't play at a tournament
	/// </summary>
	Damaged,
}