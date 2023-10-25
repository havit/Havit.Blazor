using LoxSmoke.DocXml;

namespace Havit.Blazor.Documentation.Services;
public interface IDocXmlProvider
{
	DocXmlReader GetDocXmlReaderFor(string docXmlResourceName);
}