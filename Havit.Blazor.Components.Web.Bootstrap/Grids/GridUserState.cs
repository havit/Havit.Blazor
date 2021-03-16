using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// User state of the <see cref="HxGrid"/>.
	/// </summary>
	public class GridUserState<TItem>
	{
		/// <summary>
		/// Current page index.
		/// </summary>
		public int PageIndex { get; }

		/// <summary>
		/// Current sorting.
		/// </summary>
		public IReadOnlyList<SortingItem<TItem>> Sorting { get; }

		/// <summary>
		/// Constructor.
		/// </summary>
		public GridUserState(int pageIndex, IReadOnlyList<SortingItem<TItem>> sorting)
		{
			PageIndex = pageIndex;
			Sorting = sorting;
		}
	}
}
