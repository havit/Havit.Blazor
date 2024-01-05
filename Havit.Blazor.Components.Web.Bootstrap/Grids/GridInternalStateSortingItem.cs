using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Collections;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Grid internal state item to persist sorting.
/// </summary>
internal class GridInternalStateSortingItem<TItem>
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
