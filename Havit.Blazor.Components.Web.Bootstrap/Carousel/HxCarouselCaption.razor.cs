namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Represents a caption for a carousel slide.
/// </summary>
public partial class HxCarouselCaption
{
	/// <summary>
	/// The content of the caption.
	/// </summary>
	[Parameter]
	public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// CSS class for the caption.
	/// </summary>
	[Parameter]
	public string CssClass { get; set; }
}
