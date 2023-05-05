using Microsoft.Extensions.Logging;

namespace CardFinder.Scrapers.Test.Live;

public abstract class LiveTestsBase
{
	protected ILoggerFactory Logger { get; }
	protected ICachingHttpClient DirectHttpClient { get; }
	public LiveTestsBase()
	{
		Logger = LoggerFactory.Create(c => c.AddConsole());

		var httpClient = new HttpClient();
		httpClient.DefaultRequestHeaders.UserAgent.Clear();
		httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/113.0");
		DirectHttpClient = new DirectHttpClient(httpClient);
	}
}
