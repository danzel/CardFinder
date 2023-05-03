namespace CardFinder.Scrapers;

public interface IScraper
{
	static abstract string GetUrlForCardName(string searchCardName);

	Task<CardDetails[]> Scrape(string searchCardName, CancellationToken cancellationToken = default);
}
