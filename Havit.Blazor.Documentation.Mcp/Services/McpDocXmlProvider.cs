using System.Reflection;
using System.Xml.XPath;
using Havit.Blazor.Documentation.Services;
using LoxSmoke.DocXml;

namespace Havit.Blazor.Documentation.Mcp.Services;

/// <summary>
/// Provides DocXml readers by loading XML documentation from embedded resources
/// of the Havit.Blazor.Documentation assembly.
/// </summary>
internal class McpDocXmlProvider : IDocXmlProvider
{
	private readonly Dictionary<string, DocXmlReader> _docXmlReaders = new();

	public DocXmlReader GetDocXmlReaderFor(string docXmlResourceName)
	{
		if (_docXmlReaders.TryGetValue(docXmlResourceName, out DocXmlReader result))
		{
			return result;
		}

		DocXmlReader reader = LoadDocXmlReader(docXmlResourceName);
		_docXmlReaders.Add(docXmlResourceName, reader);

		return reader;
	}

	private static DocXmlReader LoadDocXmlReader(string resourceName)
	{
		// The XML docs are embedded in the Havit.Blazor.Documentation assembly.
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
