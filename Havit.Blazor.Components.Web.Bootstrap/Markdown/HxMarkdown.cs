using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Renders Markdown content as HTML using Bootstrap typography.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxMarkdown">https://havit.blazor.eu/components/HxMarkdown</see>
/// </summary>
public partial class HxMarkdown : ComponentBase
{
	/// <summary>
	/// Application-wide defaults for <see cref="HxMarkdown"/> and derived components.
	/// </summary>
	public static MarkdownSettings Defaults { get; set; }

	static HxMarkdown()
	{
		Defaults = new MarkdownSettings()
		{
			SanitizeHtml = true,
			CssClass = null,
			TableCssClass = "table",
			BlockquoteCssClass = "blockquote",
			ImageCssClass = "img-fluid"
		};
	}

	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use a separate set of defaults).
	/// </summary>
	protected virtual MarkdownSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public MarkdownSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in component descendants (by returning a derived settings class).
	/// </remarks>
	protected virtual MarkdownSettings GetSettings() => Settings;

	/// <summary>
	/// Markdown text to render as HTML.
	/// </summary>
	[Parameter, EditorRequired] public string Content { get; set; }

	/// <summary>
	/// When <c>true</c> (default), HTML tags in the input are escaped.
	/// When <c>false</c>, raw HTML in the input passes through to the output (use only with trusted content).
	/// </summary>
	[Parameter] public bool? SanitizeHtml { get; set; }
	protected bool SanitizeHtmlEffective => SanitizeHtml ?? GetSettings()?.SanitizeHtml ?? GetDefaults().SanitizeHtml ?? true;

	/// <summary>
	/// CSS class for <c>&lt;table&gt;</c> elements rendered from markdown tables.
	/// Default is <c>"table"</c> (Bootstrap table).
	/// </summary>
	[Parameter] public string TableCssClass { get; set; }
	protected string TableCssClassEffective => TableCssClass ?? GetSettings()?.TableCssClass ?? GetDefaults().TableCssClass;

	/// <summary>
	/// CSS class for <c>&lt;blockquote&gt;</c> elements rendered from markdown blockquotes.
	/// Default is <c>"blockquote"</c> (Bootstrap blockquote).
	/// </summary>
	[Parameter] public string BlockquoteCssClass { get; set; }
	protected string BlockquoteCssClassEffective => BlockquoteCssClass ?? GetSettings()?.BlockquoteCssClass ?? GetDefaults().BlockquoteCssClass;

	/// <summary>
	/// CSS class for <c>&lt;img&gt;</c> elements rendered from markdown images.
	/// Default is <c>"img-fluid"</c> (Bootstrap responsive image).
	/// </summary>
	[Parameter] public string ImageCssClass { get; set; }
	protected string ImageCssClassEffective => ImageCssClass ?? GetSettings()?.ImageCssClass ?? GetDefaults().ImageCssClass;

	/// <summary>
	/// Any additional CSS class to apply to the wrapper element.
	/// When set (or when <see cref="AdditionalAttributes"/> is used), the output is wrapped in a <c>&lt;div&gt;</c>.
	/// </summary>
	[Parameter] public string CssClass { get; set; }
	protected string CssClassEffective => CssClass ?? GetSettings()?.CssClass ?? GetDefaults().CssClass;

	/// <summary>
	/// Additional attributes to be splatted onto the wrapper <c>&lt;div&gt;</c> element.
	/// When set, the output is wrapped in a <c>&lt;div&gt;</c>.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

	private bool HasWrapper => !string.IsNullOrEmpty(CssClassEffective) || (AdditionalAttributes is not null && AdditionalAttributes.Count > 0);

	private string _renderedHtml;
	private string _previousContent;
	private bool _previousSanitizeHtml;
	private string _previousTableCssClass;
	private string _previousBlockquoteCssClass;
	private string _previousImageCssClass;

	protected override void OnParametersSet()
	{
		base.OnParametersSet();

		var sanitize = SanitizeHtmlEffective;
		var tableCss = TableCssClassEffective;
		var blockquoteCss = BlockquoteCssClassEffective;
		var imageCss = ImageCssClassEffective;

		if (Content == _previousContent
			&& sanitize == _previousSanitizeHtml
			&& tableCss == _previousTableCssClass
			&& blockquoteCss == _previousBlockquoteCssClass
			&& imageCss == _previousImageCssClass)
		{
			return;
		}

		_previousContent = Content;
		_previousSanitizeHtml = sanitize;
		_previousTableCssClass = tableCss;
		_previousBlockquoteCssClass = blockquoteCss;
		_previousImageCssClass = imageCss;

		var options = new MarkdownRenderOptions
		{
			SanitizeHtml = sanitize,
			TableCssClass = tableCss,
			BlockquoteCssClass = blockquoteCss,
			ImageCssClass = imageCss
		};

		_renderedHtml = MarkdownParser.ToHtml(Content, options);
	}

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		if (string.IsNullOrEmpty(_renderedHtml))
		{
			return;
		}

		if (HasWrapper)
		{
			builder.OpenElement(0, "div");
			builder.AddAttribute(1, "class", CssClassEffective);
			builder.AddMultipleAttributes(2, AdditionalAttributes);
			builder.AddMarkupContent(3, _renderedHtml);
			builder.CloseElement();
		}
		else
		{
			builder.AddMarkupContent(0, _renderedHtml);
		}
	}
}
