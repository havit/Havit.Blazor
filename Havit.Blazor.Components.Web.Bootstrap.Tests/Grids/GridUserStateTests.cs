namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Grids;

public class GridUserStateTests
{
	/// <summary>
	/// The default state has null Sorting. It must still work as a hash-based key, e.g. when persisting the grid state.
	/// </summary>
	[Fact]
	public void GridUserState_GetHashCode_NullSorting_CanBeUsedInHashSet()
	{
		// Arrange
		var hashSet = new HashSet<GridUserState>();

		// Act
		hashSet.Add(new GridUserState());
		hashSet.Add(new GridUserState());

		// Assert
		Assert.Single(hashSet);
	}
}
