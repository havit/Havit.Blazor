using Havit.Blazor.Components.Web.Services.DataStores;
using Moq;

namespace Havit.Blazor.Components.Web.Tests.DataStores;

public class DictionaryStaticDataStoreTests
{
	[Fact]
	public async Task DictionaryStaticDataStore_FirstLoad_Async()
	{
		// arrange
		var sut = new Mock<DictionaryStaticDataStore<char, string>>();
		sut.CallBase = true;
		sut.SetupGet(s => s.KeySelector).Returns(value => value[0]);
		sut.Setup(s => s.LoadDataAsync()).ReturnsAsync(new[] { "Adam", "Barbora", "Cyril" });
		sut.Setup(s => s.ShouldRefresh()).Returns(false);

		// act
		var result = await sut.Object.GetAllAsync();
		var isLoaded = sut.Object.IsLoaded;

		// assert
		Assert.Equal(new[] { "Adam", "Barbora", "Cyril" }, result.ToList());
		sut.Verify(s => s.LoadDataAsync(), Times.Once);
		Assert.True(isLoaded);
	}

	[Fact]
	public async Task DictionaryStaticDataStore_FirstLoad_Sync()
	{
		// arrange
		var sut = new Mock<DictionaryStaticDataStore<char, string>>();
		sut.CallBase = true;
		sut.SetupGet(s => s.KeySelector).Returns(value => value[0]);
		sut.Setup(s => s.LoadDataAsync()).ReturnsAsync(new[] { "Adam", "Barbora", "Cyril" });
		sut.Setup(s => s.ShouldRefresh()).Returns(false);

		// act
		await sut.Object.EnsureDataAsync();
		var result = sut.Object.GetAll();

		// assert
		Assert.Equal(new[] { "Adam", "Barbora", "Cyril" }, result.ToList());
		sut.Verify(s => s.LoadDataAsync(), Times.Once);
	}

	[Fact]
	public async Task DictionaryStaticDataStore_GetByKeyAsync()
	{
		// arrange
		var sut = new Mock<DictionaryStaticDataStore<char, string>>();
		sut.CallBase = true;
		sut.SetupGet(s => s.KeySelector).Returns(value => value[0]);
		sut.Setup(s => s.LoadDataAsync()).ReturnsAsync(new[] { "Adam", "Barbora", "Cyril" });
		sut.Setup(s => s.ShouldRefresh()).Returns(false);

		// act
		var result = await sut.Object.GetByKeyAsync('A');

		// assert
		Assert.Equal("Adam", result);
	}

	[Fact]
	public async Task DictionaryStaticDataStore_GetByKey()
	{
		// arrange
		var sut = new Mock<DictionaryStaticDataStore<char, string>>();
		sut.CallBase = true;
		sut.SetupGet(s => s.KeySelector).Returns(value => value[0]);
		sut.Setup(s => s.LoadDataAsync()).ReturnsAsync(new[] { "Adam", "Barbora", "Cyril" });
		sut.Setup(s => s.ShouldRefresh()).Returns(false);

		// act
		await sut.Object.EnsureDataAsync();
		var result = sut.Object.GetByKey('A');

		// assert
		Assert.Equal("Adam", result);
	}

	[Fact]
	public async Task DictionaryStaticDataStore_SecondCallShouldUseExistingData()
	{
		// arrange
		var sut = new Mock<DictionaryStaticDataStore<char, string>>();
		sut.CallBase = true;
		sut.SetupGet(s => s.KeySelector).Returns(value => value[0]);
		sut.Setup(s => s.LoadDataAsync()).ReturnsAsync(new[] { "Adam", "Barbora", "Cyril" });
		sut.Setup(s => s.ShouldRefresh()).Returns(false);
		_ = await sut.Object.GetAllAsync();

		// act
		var isLoaded = sut.Object.IsLoaded;
		var result = await sut.Object.GetAllAsync();

		// assert
		Assert.True(isLoaded);
		sut.Verify(s => s.LoadDataAsync(), Times.Once);
		Assert.Equal(new[] { "Adam", "Barbora", "Cyril" }, result.ToList());
	}

	[Fact]
	public async Task DictionaryStaticDataStore_ShouldRefresh_SecondCallShouldReloadData()
	{
		// arrange
		var sut = new Mock<DictionaryStaticDataStore<char, string>>();
		sut.CallBase = true;
		sut.SetupGet(s => s.KeySelector).Returns(value => value[0]);
		sut.Setup(s => s.LoadDataAsync()).ReturnsAsync(new[] { "Adam", "Barbora", "Cyril" });
		sut.Setup(s => s.ShouldRefresh()).Returns(true);
		_ = await sut.Object.GetAllAsync();

		// act
		var isLoaded = sut.Object.IsLoaded;
		var result = await sut.Object.GetAllAsync();

		// assert
		Assert.False(isLoaded);
		sut.Verify(s => s.LoadDataAsync(), Times.Exactly(2));
		Assert.Equal(new[] { "Adam", "Barbora", "Cyril" }, result.ToList());
	}

	[Fact]
	public async Task DictionaryStaticDataStore_Clear_ShouldReloadData()
	{
		// arrange
		var sut = new Mock<DictionaryStaticDataStore<char, string>>();
		sut.CallBase = true;
		sut.SetupGet(s => s.KeySelector).Returns(value => value[0]);
		sut.Setup(s => s.LoadDataAsync()).ReturnsAsync(new[] { "Adam", "Barbora", "Cyril" });
		sut.Setup(s => s.ShouldRefresh()).Returns(false);
		_ = await sut.Object.GetAllAsync();

		// act
		sut.Object.Clear();
		var isLoaded = sut.Object.IsLoaded;
		var result = await sut.Object.GetAllAsync();

		// assert
		Assert.False(isLoaded);
		sut.Verify(s => s.LoadDataAsync(), Times.Exactly(2));
		Assert.Equal(new[] { "Adam", "Barbora", "Cyril" }, result.ToList());
	}

	[Fact]
	public void DictionaryStaticDataStore_ManyParallelCallsShouldNotReloadData()
	{
		// arrange
		var sut = new Mock<DictionaryStaticDataStore<char, string>>();
		sut.CallBase = true;
		sut.SetupGet(s => s.KeySelector).Returns(value => value[0]);
		sut.Setup(s => s.LoadDataAsync()).ReturnsAsync(new[] { "Adam", "Barbora", "Cyril" }, TimeSpan.FromMilliseconds(50));
		sut.Setup(s => s.ShouldRefresh()).Returns(false);

		var store = sut.Object;
		// act
		Parallel.For(0, 1000, async (int i) =>
		{
			_ = await store.GetAllAsync();
		});

		// assert
		sut.Verify(s => s.LoadDataAsync(), Times.Once);
	}

	[Fact]
	public void DictionaryStaticDataStore_SyncWithThrowIfNotLoaded_ShouldRaiseException()
	{
		// arrange
		var sut = new Mock<DictionaryStaticDataStore<char, string>>();
		sut.CallBase = true;
		sut.SetupGet(s => s.KeySelector).Returns(value => value[0]);
		sut.Setup(s => s.LoadDataAsync()).ReturnsAsync(new[] { "Adam", "Barbora", "Cyril" });
		sut.Setup(s => s.ShouldRefresh()).Returns(false);

		// act
		Assert.Throws<InvalidOperationException>(() => sut.Object.GetAll(throwIfNotLoaded: true));

		// assert
		// expected exception
	}

	[Fact]
	public void DictionaryStaticDataStore_SyncNotLoaded_ShouldReturnDefaultValue()
	{
		// arrange
		var sut = new Mock<DictionaryStaticDataStore<char, string>>();
		sut.CallBase = true;
		sut.SetupGet(s => s.KeySelector).Returns(value => value[0]);
		sut.Setup(s => s.LoadDataAsync()).ReturnsAsync(new[] { "Adam", "Barbora", "Cyril" });
		sut.Setup(s => s.ShouldRefresh()).Returns(false);

		// act
		var result = sut.Object.GetAll();

		// assert
		Assert.Null(result);
	}


	[Fact]
	public void DictionaryStaticDataStore_IsLoadedBeforeLoad_ShouldReturnFalse()
	{
		// arrange
		var sut = new Mock<DictionaryStaticDataStore<char, string>>();
		sut.CallBase = true;
		sut.SetupGet(s => s.KeySelector).Returns(value => value[0]);
		sut.Setup(s => s.LoadDataAsync()).ReturnsAsync(new[] { "Adam", "Barbora", "Cyril" });
		sut.Setup(s => s.ShouldRefresh()).Returns(false);

		// act
		var isLoaded = sut.Object.IsLoaded;

		// assert
		Assert.False(isLoaded);
	}
}
