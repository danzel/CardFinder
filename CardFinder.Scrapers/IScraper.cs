namespace CardFinder.Scrapers;

public interface IScraper
{
	public Task<CardDetails[]> Scrape(string searchCardName, CancellationToken cancellationToken = default);
}
