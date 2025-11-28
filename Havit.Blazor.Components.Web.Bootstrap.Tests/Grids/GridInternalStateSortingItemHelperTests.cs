using Havit.Blazor.Components.Web.Bootstrap;
using Havit.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Grids;

[TestClass]
public class GridInternalStateSortingItemHelperTests
{
	/// <summary>
	/// Verifies the new sorted column is added to the sorting.
	/// Also verifies method ApplyColumnToSorting is accepting null argument as a currentSorting parameter.
	/// </summary>
	[TestMethod]
	public void GridInternalStateSortingItemHelper_ApplyColumnToSorting_OnEmptySorting()
	{
		// Arrange
		Mock<IHxGridColumn<string>> mockColumn = new Mock<IHxGridColumn<string>>(MockBehavior.Strict);

		// Act
		var newSorting = GridInternalStateSortingItemHelper.ApplyColumnToSorting(null, mockColumn.Object);

		// Assert
		Assert.HasCount(1, newSorting);
		Assert.AreSame(mockColumn.Object, newSorting.Single().Column);
		Assert.IsFalse(newSorting.Single().ReverseDirection);
	}

	/// <summary>
	/// Verifies the new sorting column is "moved to the top position" when it is already in the currentSorting (and not on the top position).
	/// Also verifies the ReverseDirection which shoult be false after the "move to the top position".
	/// </summary>
	[TestMethod]
	public void GridInternalStateSortingItemHelper_ApplyColumnToSorting_TakesColumnToTheFirstPosition()
	{
		// Arrange
		Mock<IHxGridColumn<string>> mockColumn1 = new Mock<IHxGridColumn<string>>(MockBehavior.Strict);
		Mock<IHxGridColumn<string>> mockColumn2 = new Mock<IHxGridColumn<string>>(MockBehavior.Strict);
		var currentSorting = new List<GridInternalStateSortingItem<string>>
		{
			new GridInternalStateSortingItem<string> { Column = mockColumn1.Object, ReverseDirection = false },
			new GridInternalStateSortingItem<string> { Column = mockColumn2.Object, ReverseDirection = true }
		};

		// Act
		var newSorting = GridInternalStateSortingItemHelper.ApplyColumnToSorting(currentSorting, mockColumn2.Object);

		// Assert
		Assert.HasCount(2, newSorting);

		Assert.AreSame(mockColumn2.Object, newSorting[0].Column);
		Assert.IsFalse(newSorting[0].ReverseDirection);

		Assert.AreSame(mockColumn1.Object, newSorting[1].Column);
		Assert.IsFalse(newSorting[1].ReverseDirection);
	}

	/// <summary>
	/// Verifies the new sorting column is reversed when it is on the top position.
	/// </summary>
	[TestMethod]
	public void GridInternalStateSortingItemHelper_ApplyColumnToSorting_TogglesReverseOnFirstColumn()
	{
		// Arrange
		Mock<IHxGridColumn<string>> mockColumn1 = new Mock<IHxGridColumn<string>>(MockBehavior.Strict);
		var currentSorting = new List<GridInternalStateSortingItem<string>>
		{
			new GridInternalStateSortingItem<string> { Column = mockColumn1.Object, ReverseDirection = false }
		};

		// Act
		var newSorting = GridInternalStateSortingItemHelper.ApplyColumnToSorting(currentSorting, mockColumn1.Object);

		// Assert
		Assert.HasCount(1, newSorting);

		Assert.AreSame(mockColumn1.Object, newSorting[0].Column);
		Assert.IsTrue(newSorting[0].ReverseDirection);
	}

	/// <summary>
	/// Verifies the basic scenario.
	/// </summary>
	[TestMethod]
	public void GridInternalStateSortingItemHelper_ToSortingItems_TwoSimpleColumns()
	{
		// Arrange
		Mock<IHxGridColumn<string>> mockColumn1 = new Mock<IHxGridColumn<string>>(MockBehavior.Strict);
		mockColumn1.Setup(m => m.GetSorting()).Returns(new[] { new SortingItem<string>("A", null, SortDirection.Ascending) });

		Mock<IHxGridColumn<string>> mockColumn2 = new Mock<IHxGridColumn<string>>(MockBehavior.Strict);
		mockColumn2.Setup(m => m.GetSorting()).Returns(new[] { new SortingItem<string>("B", null, SortDirection.Ascending) });

		List<GridInternalStateSortingItem<string>> currentSorting = new List<GridInternalStateSortingItem<string>>
		{
			new GridInternalStateSortingItem<string> { Column = mockColumn1.Object, ReverseDirection = false },
			new GridInternalStateSortingItem<string> { Column = mockColumn2.Object, ReverseDirection = false },
		};

		// Act
		var sortingItems = GridInternalStateSortingItemHelper.ToSortingItems(currentSorting);

		// Assert
		Assert.HasCount(2, sortingItems);
		Assert.AreEqual("A", sortingItems[0].SortString);
		Assert.AreEqual(SortDirection.Ascending, sortingItems[0].SortDirection);
		Assert.AreEqual("B", sortingItems[1].SortString);
		Assert.AreEqual(SortDirection.Ascending, sortingItems[1].SortDirection);
	}

	/// <summary>
	/// Sorting by A (column1), B (column1), B (column2) should lead to the sorting by A, B (from column 1).
	/// </summary>
	[TestMethod]
	public void GridInternalStateSortingItemHelper_ToSortingItems_TwoOverlappingColumns()
	{
		// Arrange

		// this column sorts by A, then by B
		Mock<IHxGridColumn<string>> mockColumn1 = new Mock<IHxGridColumn<string>>(MockBehavior.Strict);
		mockColumn1.Setup(m => m.GetSorting()).Returns(new[] { new SortingItem<string>("A", null, SortDirection.Ascending), new SortingItem<string>("B", null, SortDirection.Ascending) });

		// this column sorts by B, then by A
		Mock<IHxGridColumn<string>> mockColumn2 = new Mock<IHxGridColumn<string>>(MockBehavior.Strict);
		mockColumn2.Setup(m => m.GetSorting()).Returns(new[] { new SortingItem<string>("B", null, SortDirection.Ascending) });

		List<GridInternalStateSortingItem<string>> currentSorting = new List<GridInternalStateSortingItem<string>>
		{
			new GridInternalStateSortingItem<string> { Column = mockColumn1.Object, ReverseDirection = false },
			new GridInternalStateSortingItem<string> { Column = mockColumn2.Object, ReverseDirection = false },
		};

		// Act
		var sortingItems = GridInternalStateSortingItemHelper.ToSortingItems(currentSorting);

		// Assert
		Assert.HasCount(2, sortingItems);
		Assert.AreEqual("A", sortingItems[0].SortString);
		Assert.AreEqual(SortDirection.Ascending, sortingItems[0].SortDirection);
		Assert.AreEqual("B", sortingItems[1].SortString);
		Assert.AreEqual(SortDirection.Ascending, sortingItems[1].SortDirection);
	}

	/// <summary>
	/// Sorting by A asc (column1), A asc (column2) should lead to the sorting by A asc.
	/// </summary>
	[TestMethod]
	public void GridInternalStateSortingItemHelper_ToSortingItems_TwoSameColumns()
	{
		// Arrange
		Mock<IHxGridColumn<string>> mockColumn1 = new Mock<IHxGridColumn<string>>(MockBehavior.Strict);
		mockColumn1.Setup(m => m.GetSorting()).Returns(new[] { new SortingItem<string>("A", null, SortDirection.Ascending) });

		Mock<IHxGridColumn<string>> mockColumn2 = new Mock<IHxGridColumn<string>>(MockBehavior.Strict);
		mockColumn2.Setup(m => m.GetSorting()).Returns(new[] { new SortingItem<string>("A", null, SortDirection.Ascending) });

		List<GridInternalStateSortingItem<string>> currentSorting = new List<GridInternalStateSortingItem<string>>
		{
			new GridInternalStateSortingItem<string> { Column = mockColumn1.Object, ReverseDirection = false },
			new GridInternalStateSortingItem<string> { Column = mockColumn2.Object, ReverseDirection = false },
		};

		// Act
		var sortingItems = GridInternalStateSortingItemHelper.ToSortingItems(currentSorting);

		// Assert
		Assert.HasCount(1, sortingItems);
		Assert.AreEqual("A", sortingItems[0].SortString);
		Assert.AreEqual(SortDirection.Ascending, sortingItems[0].SortDirection);
	}
}
