namespace CardFinder.Scrapers;

/// <summary>
/// Directly fetches the url with no caching
/// </summary>
public class DirectHttpClient : ICachingHttpClient
{
	private readonly HttpClient _httpClient;

	public DirectHttpClient(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<string> Get(string uri, CancellationToken cancellationToken = default)
	{
		var response = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri), cancellationToken);
		response.EnsureSuccessStatusCode();
		return await response.Content.ReadAsStringAsync(cancellationToken);
	}
}
