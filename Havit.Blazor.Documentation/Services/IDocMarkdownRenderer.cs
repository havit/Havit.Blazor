using Havit.Blazor.Documentation.Model;

namespace Havit.Blazor.Documentation.Services;

/// <summary>
/// Renders an <see cref="ApiDocModel"/> into a Markdown string.
/// </summary>
public interface IDocMarkdownRenderer
{
	/// <summary>
	/// Renders the type API documentation as markdown (parameters, properties, events, methods).
	/// </summary>
	string RenderTypeDoc(ApiDocModel model);

	/// <summary>
	/// Renders the component API documentation as markdown, including a list of available demo samples.
	/// </summary>
	string RenderComponentDoc(ApiDocModel model, IReadOnlyList<string> sampleNames, bool includeMcpToolHint = true);
}
