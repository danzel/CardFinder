namespace CardFinder.Solver;
static class Helpers
{
	//https://stackoverflow.com/questions/7802822/all-possible-combinations-of-a-list-of-values
	public static IEnumerable<T[]> Combinations<T>(IEnumerable<T> source)
	{
		if (null == source)
			throw new ArgumentNullException(nameof(source));

		T[] data = source.ToArray();

		return Enumerable
		  .Range(1, 1 << (data.Length)) //Exclude the empty set
		  .Select(index => data
			 .Where((v, i) => (index & (1 << i)) != 0)
			 .ToArray());
	}
}
