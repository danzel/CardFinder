using Moq;

namespace CardFinder.Scrapers.Test;
internal static class MockHelpers
{
	public static void SetupHttpGet(this Mock<ICachingHttpClient> mock, string url, string result)
	{
		mock.Setup(m => m.Get(url, It.IsAny<CancellationToken>())).ReturnsAsync(result);
	}
}
