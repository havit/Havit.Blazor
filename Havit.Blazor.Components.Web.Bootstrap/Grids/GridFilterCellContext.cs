namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Grid filter cell context.
/// </summary>
public record GridFilterCellContext
{
	/// <summary>
	/// Total count of items in the grid (includes all pages).
	/// </summary>
	public int? TotalCount { get; init; }
}