namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://getbootstrap.com/docs/5.3/components/card/">Bootstrap card</see> component.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxCard">https://havit.blazor.eu/components/HxCard</see>
/// </summary>
public partial class HxCard
{
	/// <summary>
	/// Application-wide defaults for <see cref="HxCard"/> and derived components.
	/// </summary>
	public static CardSettings Defaults { get; set; }

	static HxCard()
	{
		Defaults = new CardSettings();
	}

	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use separate set of defaults).
	/// </summary>
	protected virtual CardSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public CardSettings Settings { get; set; }

	/// <summary>
	/// Returns optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in components descendants (by returning a derived settings class).
	/// </remarks>
	protected virtual CardSettings GetSettings() => this.Settings;


	/// <summary>
	/// Image to be placed in the card. For image position see <see cref="ImagePlacement"/>.
	/// </summary>
	[Parameter] public string ImageSrc { get; set; }

	/// <summary>
	/// Placement of the image. Default is <see cref="CardImagePlacement.Top"/>.
	/// </summary>
	[Parameter] public CardImagePlacement ImagePlacement { get; set; }

	/// <summary>
	/// Image <c>alt</c> attribute value.
	/// </summary>
	[Parameter] public string ImageAlt { get; set; }

	/// <summary>
	/// Image <c>width</c> attribute value.
	/// </summary>
	[Parameter] public int? ImageWidth { get; set; }

	/// <summary>
	/// Image <c>height</c> attribute value.
	/// </summary>
	[Parameter] public int? ImageHeight { get; set; }

	/// <summary>
	/// Header content.
	/// </summary>
	[Parameter] public RenderFragment HeaderTemplate { get; set; }

	/// <summary>
	/// Body content.
	/// </summary>
	[Parameter] public RenderFragment BodyTemplate { get; set; }

	/// <summary>
	/// Footer content.
	/// </summary>
	[Parameter] public RenderFragment FooterTemplate { get; set; }

	/// <summary>
	/// Generic card content (outside <c>.card-body</c>).
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// Additional CSS classes for the card-container.
	/// </summary>
	[Parameter] public string CssClass { get; set; }
	protected string CssClassEffective => this.CssClass ?? this.GetSettings()?.CssClass ?? GetDefaults().CssClass;

	/// <summary>
	/// Additional CSS class for the header.
	/// </summary>
	[Parameter] public string HeaderCssClass { get; set; }
	protected string HeaderCssClassEffective => this.HeaderCssClass ?? this.GetSettings()?.HeaderCssClass ?? GetDefaults().HeaderCssClass;

	/// <summary>
	/// Additional CSS class for the body.
	/// </summary>
	[Parameter] public string BodyCssClass { get; set; }
	protected string BodyCssClassEffective => this.BodyCssClass ?? this.GetSettings()?.BodyCssClass ?? GetDefaults().BodyCssClass;

	/// <summary>
	/// Additional CSS class for the footer.
	/// </summary>
	[Parameter] public string FooterCssClass { get; set; }
	protected string FooterCssClassEffective => this.FooterCssClass ?? this.GetSettings()?.FooterCssClass ?? GetDefaults().FooterCssClass;

	/// <summary>
	/// Additional attributes to be splatted onto an underlying HTML element.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }
}
