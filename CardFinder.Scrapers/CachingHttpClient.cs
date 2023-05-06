using Microsoft.Extensions.Caching.Distributed;
using System.Net.Http.Headers;

namespace CardFinder.Scrapers;
public class CachingHttpClient : ICachingHttpClient
{
	private readonly IDistributedCache _cache;
	private readonly DirectHttpClient _client;

	public CachingHttpClient(IDistributedCache cache, HttpClient httpClient)
	{
		_cache = cache;
		_client = new DirectHttpClient(httpClient);
	}

	public async Task<string> Get(string uri, CancellationToken cancellationToken = default)
	{
		var key = $"GET: {uri}";
		var res = await _cache.GetStringAsync(key, cancellationToken);

		if (res == null)
		{
			res = await _client.Get(uri, cancellationToken);
			await _cache.SetStringAsync(key, res, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1) }, cancellationToken);
		}

		return res;
	}

	public async Task<string> Post(string uri, HttpContent? payload, Action<HttpRequestHeaders>? headerModifier, CancellationToken cancellationToken = default)
	{
		string key;
		if (payload == null)
			key = $"POST: {uri}";
		else
			key = $"POST {uri} {await payload.ReadAsStringAsync()}";


		var res = await _cache.GetStringAsync(key, cancellationToken);

		if (res == null)
		{
			res = await _client.Get(uri, cancellationToken);
			await _cache.SetStringAsync(key, res, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1) }, cancellationToken);
		}

		return res;
	}
}
