namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Grid internal state item to persist sorting.
/// </summary>
internal record GridInternalStateSortingItem<TItem> // record: For comparison purposes.
{
	/// <summary>
	/// Sorting column.
	/// </summary>
	public IHxGridColumn<TItem> Column { get; init; }

	/// <summary>
	/// Indicates whether the sorting should be performed in the reverse direction.
	/// </summary>
	public bool ReverseDirection { get; init; }
}
