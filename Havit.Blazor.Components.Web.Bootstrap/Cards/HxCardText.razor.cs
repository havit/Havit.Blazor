namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Paragraph of body text for a <see cref="HxCard"/>, rendered as a plain <c>p</c> element
/// (Bootstrap 6 has no styling for the former <c>card-text</c> class, the card body spaces its children itself).
/// </summary>
public partial class HxCardText
{
	/// <summary>
	/// Text content.
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the paragraph element.
	/// </summary>
	[Parameter] public string CssClass { get; set; }
}
