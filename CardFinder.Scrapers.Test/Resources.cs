using System.Reflection;

namespace CardFinder.Scrapers.Test;

static class Resources
{
	public static string ReadResource(string resourceName)
	{
		var assembly = Assembly.GetExecutingAssembly();

		using var memory = new MemoryStream();
		using var stream = assembly.GetManifestResourceStream(resourceName)!;

		if (stream == null)
			throw new InvalidDataException($"Couldn't find resource '{resourceName}'");

		using var reader = new StreamReader(stream);
		return reader.ReadToEnd();
	}
}