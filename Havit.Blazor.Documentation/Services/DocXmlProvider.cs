using System.Collections.Concurrent;
using System.Reflection;
using System.Xml.XPath;
using LoxSmoke.DocXml;

namespace Havit.Blazor.Documentation.Services;

/// <summary>
/// Provides DocXml readers by loading XML documentation from embedded resources
/// of the Havit.Blazor.Documentation assembly.
/// Uses <see cref="Lazy{T}"/> to prevent stampede (multiple concurrent builds for the same resource).
/// </summary>
public class DocXmlProvider : IDocXmlProvider
{
	private readonly ConcurrentDictionary<string, Lazy<DocXmlReader>> _docXmlReaders = new();

	public DocXmlReader GetDocXmlReaderFor(string docXmlResourceName)
	{
		Lazy<DocXmlReader> lazy = _docXmlReaders.GetOrAdd(
			docXmlResourceName,
			static resourceName => new Lazy<DocXmlReader>(() => LoadDocXmlReader(resourceName)));

		return lazy.Value;
	}

	private static DocXmlReader LoadDocXmlReader(string resourceName)
	{
		Assembly documentationAssembly = typeof(ApiDocModelBuilder).Assembly;

		string[] matchingResourceNames = documentationAssembly.GetManifestResourceNames()
			.Where(str => str.EndsWith(resourceName, StringComparison.Ordinal))
			.ToArray();

		if (matchingResourceNames.Length == 0)
		{
			throw new InvalidOperationException($"Embedded resource '{resourceName}' not found in assembly '{documentationAssembly.FullName}'. Available resources: {string.Join(", ", documentationAssembly.GetManifestResourceNames())}");
		}

		if (matchingResourceNames.Length > 1)
		{
			throw new InvalidOperationException($"Multiple embedded resources match '{resourceName}' in assembly '{documentationAssembly.FullName}': {string.Join(", ", matchingResourceNames)}");
		}

		string fullResourceName = matchingResourceNames[0];

		using Stream stream = documentationAssembly.GetManifestResourceStream(fullResourceName);
		using StreamReader reader = new StreamReader(stream);

		using TextReader textReader = new StringReader(reader.ReadToEnd());
		XPathDocument xPathDocument = new(textReader);

		return new DocXmlReader(xPathDocument);
	}
}
