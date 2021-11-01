namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Grid footer cell context.
	/// </summary>
	public record GridFooterCellContext
	{
		/// <summary>
		/// Total count of items in the grid (includes all pages).
		/// </summary>
		public int? TotalCount { get; init; }
	}
}