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
		Assembly documentationAssembly = typeof(ComponentApiDocModelBuilder).Assembly;

		string fullResourceName = documentationAssembly.GetManifestResourceNames()
			.SingleOrDefault(str => str.EndsWith(resourceName));

		if (fullResourceName is null)
		{
			throw new InvalidOperationException($"Embedded resource '{resourceName}' not found in assembly '{documentationAssembly.FullName}'. Available resources: {string.Join(", ", documentationAssembly.GetManifestResourceNames())}");
		}

		using Stream stream = documentationAssembly.GetManifestResourceStream(fullResourceName);
		using StreamReader reader = new StreamReader(stream);

		TextReader textReader = new StringReader(reader.ReadToEnd());
		XPathDocument xPathDocument = new(textReader);

		return new DocXmlReader(xPathDocument);
	}
}
