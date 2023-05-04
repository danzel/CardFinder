namespace CardFinder.Scrapers.Helpers;

/// <summary>
/// Parses a condition string in to the condition it represents.
/// </summary>
public interface IConditionParser
{
	/// <summary>
	/// Parses a condition string in to the condition it represents.
	/// </summary>
	Condition Parse(string conditionString);
}

/// <summary>
/// Parses a condition string in to the condition it represents.
/// Handles the standard condition text you can expect on most sites
/// </summary>
public class DefaultConditionParser : IConditionParser
{
	/// <inheritdoc />
	public Condition Parse(string conditionString)
	{
		switch (conditionString.Trim())
		{
			case "Near Mint":
			case "NM":
			case "NM-Mint":
				return Condition.NearMint;
			case "Lightly Played":
			case "SP":
				return Condition.LightlyPlayed;
			case "Moderately Played":
			case "MP":
				return Condition.ModeratelyPlayed;
			case "Heavily Played":
				return Condition.HeavilyPlayed;
			case "Damaged":
			case "Damage":
				return Condition.Damaged;
			default:
				return CustomParse(conditionString);
				throw new NotImplementedException(conditionString);
		}
	}

	/// <summary>
	/// Override to handle any custom strings this site uses.
	/// </summary>
	protected virtual Condition CustomParse(string conditionString)
	{
		throw new NotImplementedException($"Don't know this condition: '{conditionString}'");
	}
}

/// <summary>
/// Although all BinderPos sites use this, they have different sets of conditions they use on their site
/// </summary>
/// <example>BeaDndGamesCoNz - Near Mint, Lightly Played, Moderately Played, Heavily Played, Damaged</example>
/// <example>SpellboundGamesCoNz - NM / SP, NM / SP Non English</example>
public class BinderPosConditionParser : DefaultConditionParser
{
	protected override Condition CustomParse(string conditionString)
	{
		switch (conditionString.Trim())
		{
			case "Near Mint / Lightly Played": //Best quality
			case "NM / SP": //Best quality
				return Condition.NearMint;
			case "Played": //Second best quality
				return Condition.LightlyPlayed; //Maybe should be ModeratelyPlayed
			default:
				return base.CustomParse(conditionString);
		}
	}
}