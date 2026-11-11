namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Grids;

public class GridItemKeyEqualityComparerTests
{
	private static readonly Func<object, object> s_defaultSelectorForObject = x => x;

	[Fact]
	public void GridItemKeyEqualityComparer_DefaultSelector_PreservesTypedEquality()
	{
		// Arrange
		Func<EquatableItem, object> defaultSelector = x => x;
		var sut = new GridItemKeyEqualityComparer<EquatableItem>(() => defaultSelector, defaultSelector);
		var item1 = new EquatableItem(1);
		var item2 = new EquatableItem(1); // fresh instance, equal by IEquatable<T> only (Equals(object) is not overridden)

		// Precondition
		Assert.False(Object.Equals(item1, item2));

		// Act + Assert
		Assert.True(sut.Equals(item1, item2));
		Assert.Equal(sut.GetHashCode(item1), sut.GetHashCode(item2));
		Assert.False(sut.Equals(item1, new EquatableItem(2)));
	}

	[Fact]
	public void GridItemKeyEqualityComparer_DefaultSelector_PlainClass_UsesReferenceEquality()
	{
		// Arrange
		var sut = new GridItemKeyEqualityComparer<object>(() => s_defaultSelectorForObject, s_defaultSelectorForObject);
		var item = new object();

		// Act + Assert
		Assert.True(sut.Equals(item, item));
		Assert.False(sut.Equals(item, new object()));
	}

	[Fact]
	public void GridItemKeyEqualityComparer_CustomSelector_ComparesByKey()
	{
		// Arrange
		Func<KeyedItem, object> defaultSelector = x => x;
		Func<KeyedItem, object> customSelector = x => x.Id;
		var sut = new GridItemKeyEqualityComparer<KeyedItem>(() => customSelector, defaultSelector);

		// Act + Assert
		Assert.True(sut.Equals(new KeyedItem(1), new KeyedItem(1)));
		Assert.Equal(sut.GetHashCode(new KeyedItem(1)), sut.GetHashCode(new KeyedItem(1)));
		Assert.False(sut.Equals(new KeyedItem(1), new KeyedItem(2)));
	}

	[Fact]
	public void GridItemKeyEqualityComparer_CustomSelector_HandlesNullItems()
	{
		// Arrange
		Func<KeyedItem, object> defaultSelector = x => x;
		Func<KeyedItem, object> customSelector = x => x.Id; // would throw for null items
		var sut = new GridItemKeyEqualityComparer<KeyedItem>(() => customSelector, defaultSelector);

		// Act + Assert
		Assert.True(sut.Equals(null, null));
		Assert.False(sut.Equals(new KeyedItem(1), null));
		Assert.False(sut.Equals(null, new KeyedItem(1)));
		Assert.Equal(0, sut.GetHashCode(null));
	}

	[Fact]
	public void GridItemKeyEqualityComparer_UsesCurrentSelector()
	{
		// Arrange
		Func<KeyedItem, object> defaultSelector = x => x;
		Func<KeyedItem, object> currentSelector = defaultSelector;
		var sut = new GridItemKeyEqualityComparer<KeyedItem>(() => currentSelector, defaultSelector);
		var item1 = new KeyedItem(1);
		var item2 = new KeyedItem(1);

		// Act + Assert
		Assert.False(sut.Equals(item1, item2));

		currentSelector = x => x.Id;
		Assert.True(sut.Equals(item1, item2));
	}

	private class EquatableItem(int id) : IEquatable<EquatableItem>
	{
		public int Id { get; } = id;

		public bool Equals(EquatableItem other) => (other is not null) && (other.Id == Id);

		// Equals(object) intentionally not overridden - simulates a type with IEquatable<T> equality only.
		public override int GetHashCode() => Id;
	}

	private class KeyedItem(int id)
	{
		public int Id { get; } = id;
	}
}
