using Moq;
using System.Net.Http.Headers;

namespace CardFinder.Scrapers.Test;
internal static class MockHelpers
{
	public static void SetupHttpGet(this Mock<ICachingHttpClient> mock, string url, string result)
	{
		mock.Setup(m => m.Get(url, It.IsAny<CancellationToken>())).ReturnsAsync(result);
	}

	public static void SetupHttpPost(this Mock<ICachingHttpClient> mock, string url, string result)
	{
		mock.Setup(m => m.Post(url, It.IsAny<HttpContent?>(), It.IsAny<Action<HttpRequestHeaders>?>(), It.IsAny<CancellationToken>())).ReturnsAsync(result);
	}
}
