using System.Net;

namespace CardFinder.Scrapers;
public interface ICachingHttpClient
{
	/// <summary>
	/// Perform an HTTP GET of the given URI and return the result.
	/// Will fetch from the cache first, and store any <see cref="HttpStatusCode.OK"/> result in the cache
	/// </summary>
	Task<string> Get(string uri, CancellationToken cancellationToken = default);
}
