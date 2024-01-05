namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/card/#titles-text-and-links">card-text</see> component.
/// </summary>
public partial class HxCardText
{
	/// <summary>
	/// Text content.
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the card-text element.
	/// </summary>
	[Parameter] public string CssClass { get; set; }
}
