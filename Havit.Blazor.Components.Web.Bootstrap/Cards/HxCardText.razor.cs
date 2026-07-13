namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Paragraph of body text for a <see cref="HxCard"/>, rendered as Bootstrap's <see href="https://getbootstrap.com/docs/5.3/components/card/#titles-text-and-links">card-text</see>.
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
