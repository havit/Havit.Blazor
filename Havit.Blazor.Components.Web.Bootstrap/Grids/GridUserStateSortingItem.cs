using Havit.Collections;
using System.Linq.Expressions;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Sorting criteria for a <see cref="GridUserState"/>.
/// </summary>
public sealed record GridUserStateSortingItem
{
	/// <summary>
	/// Column identifier.
	/// </summary>
	public string ColumnIdentifier { get; init; }

	/// <summary>
	/// Indicates whether the sorting should be performed in the reverse direction.	
	/// </summary>
	public bool ReverseDirection { get; init; }
}