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