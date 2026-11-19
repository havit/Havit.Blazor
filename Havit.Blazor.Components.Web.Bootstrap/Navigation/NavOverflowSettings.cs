namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for <see cref="HxNavOverflow"/>.
/// </summary>
public record NavOverflowSettings
{
	/// <summary>
	/// Text of the "More" toggle button.
	/// </summary>
	public string MoreText { get; set; }

	/// <summary>
	/// Icon of the "More" toggle button.
	/// </summary>
	public IconBase MoreIcon { get; set; }

	/// <summary>
	/// Position of the icon relative to the text in the "More" toggle button.
	/// </summary>
	public NavOverflowIconPlacement? IconPlacement { get; set; }

	/// <summary>
	/// Placement of the overflow menu relative to the "More" toggle button.
	/// </summary>
	public MenuPlacement? MenuPlacement { get; set; }

	/// <summary>
	/// Minimum number of items to keep visible before the remaining items overflow into the menu.
	/// </summary>
	public int? MinimumVisibleItems { get; set; }

	/// <summary>
	/// Container width threshold below which all items collapse into the overflow menu.
	/// A breakpoint name (e.g. <c>md</c>, resolved from the <c>--bs-breakpoint-{name}</c> CSS variable) or a pixel value (e.g. <c>768</c>).
	/// </summary>
	public string CollapseBelow { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the nav.
	/// </summary>
	public string CssClass { get; set; }
}
