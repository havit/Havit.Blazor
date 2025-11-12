using Havit.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Grids;

[TestClass]
public class SortingItemTests
{
	/// <summary>
	/// Verifies that EqualsIgnoringSortDirection handles null SortKeySelector correctly
	/// when comparing items with mixed null states (regression test for NullReferenceException).
	/// </summary>
	[TestMethod]
	public void SortingItem_EqualsIgnoringSortDirection_HandlesNullSortKeySelector()
	{
		// Arrange - Create sorting items with null SortKeySelector (SortString-based)
		var item1 = new SortingItem<string>("Name", null, SortDirection.Ascending);
		var item2 = new SortingItem<string>("Name", null, SortDirection.Descending);

		// Act & Assert - Should not throw NullReferenceException
		Assert.IsTrue(item1.EqualsIgnoringSortDirection(item2));
	}

	/// <summary>
	/// Verifies that EqualsIgnoringSortDirection returns false when comparing
	/// a SortString-based item with a SortKeySelector-based item.
	/// </summary>
	[TestMethod]
	public void SortingItem_EqualsIgnoringSortDirection_ReturnsFalse_WhenOnlyOneHasSortKeySelector()
	{
		// Arrange
		Expression<Func<string, IComparable>> keySelector = x => x;
		var itemWithSortString = new SortingItem<string>("Name", null, SortDirection.Ascending);
		var itemWithSortKeySelector = new SortingItem<string>(null, keySelector, SortDirection.Ascending);

		// Act
		var result1 = itemWithSortString.EqualsIgnoringSortDirection(itemWithSortKeySelector);
		var result2 = itemWithSortKeySelector.EqualsIgnoringSortDirection(itemWithSortString);

		// Assert
		Assert.IsFalse(result1);
		Assert.IsFalse(result2);
	}

	/// <summary>
	/// Verifies that EqualsIgnoringSortDirection returns true when both items
	/// have the same non-null SortKeySelector.
	/// </summary>
	[TestMethod]
	public void SortingItem_EqualsIgnoringSortDirection_ReturnsTrue_WhenBothHaveSameSortKeySelector()
	{
		// Arrange
		Expression<Func<string, IComparable>> keySelector = x => x.Length;
		var item1 = new SortingItem<string>(null, keySelector, SortDirection.Ascending);
		var item2 = new SortingItem<string>(null, keySelector, SortDirection.Descending);

		// Act
		var result = item1.EqualsIgnoringSortDirection(item2);

		// Assert
		Assert.IsTrue(result);
	}

	/// <summary>
	/// Verifies that EqualsIgnoringSortDirection returns false when both items
	/// have different SortKeySelectors.
	/// </summary>
	[TestMethod]
	public void SortingItem_EqualsIgnoringSortDirection_ReturnsFalse_WhenDifferentSortKeySelectors()
	{
		// Arrange
		Expression<Func<string, IComparable>> keySelector1 = x => x.Length;
		Expression<Func<string, IComparable>> keySelector2 = x => x.ToLower();
		var item1 = new SortingItem<string>(null, keySelector1, SortDirection.Ascending);
		var item2 = new SortingItem<string>(null, keySelector2, SortDirection.Ascending);

		// Act
		var result = item1.EqualsIgnoringSortDirection(item2);

		// Assert
		Assert.IsFalse(result);
	}

	/// <summary>
	/// Verifies that EqualsIgnoringSortDirection ignores the sort direction
	/// when comparing items.
	/// </summary>
	[TestMethod]
	public void SortingItem_EqualsIgnoringSortDirection_IgnoresSortDirection()
	{
		// Arrange
		var item1 = new SortingItem<string>("Name", null, SortDirection.Ascending);
		var item2 = new SortingItem<string>("Name", null, SortDirection.Descending);

		// Act
		var result = item1.EqualsIgnoringSortDirection(item2);

		// Assert
		Assert.IsTrue(result);
	}

	/// <summary>
	/// Verifies that EqualsIgnoringSortDirection returns false when SortStrings differ.
	/// </summary>
	[TestMethod]
	public void SortingItem_EqualsIgnoringSortDirection_ReturnsFalse_WhenDifferentSortStrings()
	{
		// Arrange
		var item1 = new SortingItem<string>("Name", null, SortDirection.Ascending);
		var item2 = new SortingItem<string>("DisplayName", null, SortDirection.Ascending);

		// Act
		var result = item1.EqualsIgnoringSortDirection(item2);

		// Assert
		Assert.IsFalse(result);
	}

	/// <summary>
	/// Verifies that EqualsIgnoringSortDirection handles null argument gracefully.
	/// </summary>
	[TestMethod]
	public void SortingItem_EqualsIgnoringSortDirection_ReturnsFalse_WhenArgumentIsNull()
	{
		// Arrange
		var item = new SortingItem<string>("Name", null, SortDirection.Ascending);

		// Act
		var result = item.EqualsIgnoringSortDirection<string>(null);

		// Assert
		Assert.IsFalse(result);
	}
}
