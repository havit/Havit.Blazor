namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxMarkdown"/> and derived components.
/// </summary>
public record MarkdownSettings
{
	/// <summary>
	/// When <c>true</c> (default), HTML tags in the input are escaped.
	/// When <c>false</c>, raw HTML in the input passes through to the output (use only with trusted content).
	/// </summary>
	public bool? SanitizeHtml { get; set; }

	/// <summary>
	/// CSS class for the wrapper <c>&lt;div&gt;</c> element.
	/// </summary>
	public string CssClass { get; set; }

	/// <summary>
	/// CSS class for <c>&lt;table&gt;</c> elements rendered from markdown tables.
	/// Default is <c>"table"</c> (Bootstrap table).
	/// </summary>
	public string TableCssClass { get; set; }

	/// <summary>
	/// CSS class for <c>&lt;blockquote&gt;</c> elements rendered from markdown blockquotes.
	/// Default is <c>"blockquote"</c> (Bootstrap blockquote).
	/// </summary>
	public string BlockquoteCssClass { get; set; }

	/// <summary>
	/// CSS class for <c>&lt;img&gt;</c> elements rendered from markdown images.
	/// Default is <c>"img-fluid"</c> (Bootstrap responsive image).
	/// </summary>
	public string ImageCssClass { get; set; }
}
