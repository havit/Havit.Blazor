namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

/// <summary>
/// Options for the markdown-to-HTML rendering.
/// </summary>
internal class MarkdownRenderOptions
{
	public bool SanitizeHtml { get; set; } = true;
	public string TableCssClass { get; set; }
	public string BlockquoteCssClass { get; set; }
	public string ImageCssClass { get; set; }
}
