using Havit.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Grids;

[TestClass]
public class HxGrid_SortingIcons_Tests
{
	#region Showing icon "on hover", direction should be the same which will be used when a user clicks.

	[TestMethod]
	public void HxGrid_GetSortIconDisplayDirection_NotCurrentSorting_ColumnSortingAscending()
	{
		// Arrange
		List<GridInternalStateSortingItem<object>> currentSorting = new List<GridInternalStateSortingItem<object>>();
		SortingItem<object>[] columnSorting = new[] { new SortingItem<object>("A", null, SortDirection.Ascending) };

		// Act
		SortDirection result = HxGrid<object>.GetSortIconDisplayDirection(false, currentSorting, columnSorting);

		// Assert
		Assert.AreEqual(SortDirection.Ascending, result);
	}

	/// <summary>
	/// Showing icon "on hover", direction should be the same which will be used when click.
	/// </summary>
	[TestMethod]
	public void HxGrid_GetSortIconDisplayDirection_NotCurrentSorting_ColumnSortingDescending()
	{
		// Arrange
		List<GridInternalStateSortingItem<object>> currentSorting = new List<GridInternalStateSortingItem<object>>();
		SortingItem<object>[] columnSorting = new[] { new SortingItem<object>("A", null, SortDirection.Descending) };

		// Act
		SortDirection result = HxGrid<object>.GetSortIconDisplayDirection(false, currentSorting, columnSorting);

		// Assert
		Assert.AreEqual(SortDirection.Descending, result);
	}
	#endregion

	#region Showing icon as a current state, direction should be the same which is used right now.

	[TestMethod]
	public void HxGrid_GetSortIconDisplayDirection_CurrentSorting_ColumnSortingAscending()
	{
		// Arrange
		List<GridInternalStateSortingItem<object>> currentSorting = new List<GridInternalStateSortingItem<object>> { new GridInternalStateSortingItem<object> { ReverseDirection = false } };
		SortingItem<object>[] columnSorting = new[] { new SortingItem<object>("A", null, SortDirection.Ascending) };

		// Act
		SortDirection result = HxGrid<object>.GetSortIconDisplayDirection(true, currentSorting, columnSorting);

		// Assert
		Assert.AreEqual(SortDirection.Ascending, result);
	}

	[TestMethod]
	public void HxGrid_GetSortIconDisplayDirection_CurrentSorting_ColumnSortingDescending()
	{
		// Arrange
		List<GridInternalStateSortingItem<object>> currentSorting = new List<GridInternalStateSortingItem<object>> { new GridInternalStateSortingItem<object> { ReverseDirection = false } };
		SortingItem<object>[] columnSorting = new[] { new SortingItem<object>("A", null, SortDirection.Descending) };

		// Act
		SortDirection result = HxGrid<object>.GetSortIconDisplayDirection(true, currentSorting, columnSorting);

		// Assert
		Assert.AreEqual(SortDirection.Descending, result);
	}

	[TestMethod]
	public void HxGrid_GetSortIconDisplayDirection_CurrentSorting_ReverseDirection_ColumnSortingAscending()
	{
		// Arrange
		List<GridInternalStateSortingItem<object>> currentSorting = new List<GridInternalStateSortingItem<object>> { new GridInternalStateSortingItem<object> { ReverseDirection = true } };
		SortingItem<object>[] columnSorting = new[] { new SortingItem<object>("A", null, SortDirection.Ascending) };

		// Act
		SortDirection result = HxGrid<object>.GetSortIconDisplayDirection(true, currentSorting, columnSorting);

		// Assert
		Assert.AreEqual(SortDirection.Descending, result);
	}

	[TestMethod]
	public void HxGrid_GetSortIconDisplayDirection_CurrentSorting_ReverseDirection_ColumnSortingDescending()
	{
		// Arrange
		List<GridInternalStateSortingItem<object>> currentSorting = new List<GridInternalStateSortingItem<object>> { new GridInternalStateSortingItem<object> { ReverseDirection = true } };
		SortingItem<object>[] columnSorting = new[] { new SortingItem<object>("A", null, SortDirection.Descending) };

		// Act
		SortDirection result = HxGrid<object>.GetSortIconDisplayDirection(true, currentSorting, columnSorting);

		// Assert
		Assert.AreEqual(SortDirection.Ascending, result);
	}
	#endregion


}
