namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Grid header cell context.
/// </summary>
public record GridHeaderCellContext
{
	/// <summary>
	/// Total count of items in the grid (includes all pages).
	/// </summary>
	public int? TotalCount { get; init; }
}