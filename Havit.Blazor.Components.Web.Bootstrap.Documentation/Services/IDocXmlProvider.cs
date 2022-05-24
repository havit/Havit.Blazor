using LoxSmoke.DocXml;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Services;
public interface IDocXmlProvider
{
	DocXmlReader GetDocXmlReaderFor(string docXmlResourceName);
}