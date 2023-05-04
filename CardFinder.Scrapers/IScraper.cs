namespace CardFinder.Scrapers;

public interface IScraper
{
	/// <summary>
	/// Get the Url that will be HTTP GET for the given card name.
	/// Implemented by most parsers, but not all
	/// </summary>
	string GetUrlForCardName(string searchCardName);

	/// <summary>
	/// Scrape the site for cards with the given name
	/// </summary>
	Task<CardDetails[]> Scrape(string searchCardName, CancellationToken cancellationToken = default);
}
