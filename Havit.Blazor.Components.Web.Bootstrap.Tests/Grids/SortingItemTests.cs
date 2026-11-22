using Havit.Collections;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Grids;

public class SortingItemTests
{
	[Fact]
	public void SortingItem_EqualsIgnoringSortDirection_SameSortStringAndSelector_ReturnsTrue()
	{
		// Arrange
		var sortingItem1 = new SortingItem<string>("Length", item => item.Length, SortDirection.Ascending);
		var sortingItem2 = new SortingItem<string>("Length", item => item.Length, SortDirection.Descending);

		// Act + Assert
		Assert.True(sortingItem1.EqualsIgnoringSortDirection(sortingItem2));
	}

	[Fact]
	public void SortingItem_EqualsIgnoringSortDirection_BothSelectorsNull_ReturnsTrue()
	{
		// Arrange
		var sortingItem1 = new SortingItem<string>("Length", null, SortDirection.Ascending);
		var sortingItem2 = new SortingItem<string>("Length", null, SortDirection.Descending);

		// Act + Assert
		Assert.True(sortingItem1.EqualsIgnoringSortDirection(sortingItem2));
	}

	[Fact]
	public void SortingItem_EqualsIgnoringSortDirection_OnlyOneSelectorSet_ReturnsFalse()
	{
		// Arrange
		var sortStringOnly = new SortingItem<string>("Length", null, SortDirection.Ascending);
		var withSelector = new SortingItem<string>("Length", item => item.Length, SortDirection.Ascending);

		// Act + Assert
		Assert.False(sortStringOnly.EqualsIgnoringSortDirection(withSelector));
		Assert.False(withSelector.EqualsIgnoringSortDirection(sortStringOnly));
	}
}
