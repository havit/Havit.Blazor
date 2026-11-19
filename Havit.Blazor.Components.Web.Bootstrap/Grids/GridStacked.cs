namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Stacked layout of the <see cref="HxGrid{TItem}"/> (Bootstrap stacked tables).
/// In the stacked layout, each row is rendered as a vertical block: the first cell acts as a bold title,
/// the remaining cells are displayed as label–value pairs (labels are taken from the column headers, see <see cref="HxGridColumn{TItem}.StackedLabel"/>).
/// </summary>
/// <remarks>
/// The breakpoints are evaluated against the width of the grid container (CSS container queries), not the viewport.
/// </remarks>
public enum GridStacked
{
	/// <summary>
	/// Never stacked, the grid is always rendered as a regular table.
	/// </summary>
	Never = 0,

	/// <summary>
	/// Always stacked (<c>table-stacked</c>).
	/// </summary>
	Always,

	/// <summary>
	/// Stacked when the grid container is narrower than the "small" breakpoint (<c>576px</c>, exclusive; <c>sm:table-stacked</c>).
	/// </summary>
	BelowSmall,

	/// <summary>
	/// Stacked when the grid container is narrower than the "medium" breakpoint (<c>768px</c>, exclusive; <c>md:table-stacked</c>).
	/// </summary>
	BelowMedium,

	/// <summary>
	/// Stacked when the grid container is narrower than the "large" breakpoint (<c>1024px</c>, exclusive; <c>lg:table-stacked</c>).
	/// </summary>
	BelowLarge,

	/// <summary>
	/// Stacked when the grid container is narrower than the "extra-large" breakpoint (<c>1280px</c>, exclusive; <c>xl:table-stacked</c>).
	/// </summary>
	BelowExtraLarge,

	/// <summary>
	/// Stacked when the grid container is narrower than the "2xl" breakpoint (<c>1536px</c>, exclusive; <c>2xl:table-stacked</c>).
	/// </summary>
	BelowXxl
}
