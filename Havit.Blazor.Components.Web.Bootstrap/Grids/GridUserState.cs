using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap.Grids
{
	/// <summary>
	/// User state of the <see cref="HxGrid"/>.
	/// </summary>
	public class GridUserState<TItemType>
	{
		/// <summary>
		/// Current page index.
		/// </summary>
		public int CurrentPageIndex { get; }

		/// <summary>
		/// Current sorting.
		/// </summary>
		public SortingItem<TItemType>[] CurrentSorting { get; }

		/// <summary>
		/// Constructor.
		/// </summary>
		public GridUserState(int currentPageIndex, SortingItem<TItemType>[] currentSorting)
		{
			CurrentPageIndex = currentPageIndex;
			CurrentSorting = currentSorting;
		}
	}
}
