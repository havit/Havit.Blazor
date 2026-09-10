using Havit.Blazor.Components.Web.Services.DataStores;
using Moq;

namespace Havit.Blazor.Components.Web.Tests.DataStores;

public class StaticDataStoreTests
{
	[Fact]
	public async Task StaticDataStore_FirstLoad_Async()
	{
		// arrange
		var sut = new Mock<StaticDataStore<string>>();
		sut.CallBase = true;
		sut.Setup(s => s.LoadDataAsync()).ReturnsAsync("Adam");
		sut.Setup(s => s.ShouldRefresh()).Returns(false);

		// act
		var result = await sut.Object.GetValueAsync();
		var isLoaded = sut.Object.IsLoaded;

		// assert
		Assert.Equal("Adam", result);
		sut.Verify(s => s.LoadDataAsync(), Times.Once);
		Assert.True(isLoaded);
	}

	[Fact]
	public async Task StaticDataStore_FirstLoad_Sync()
	{
		// arrange
		var sut = new Mock<StaticDataStore<string>>();
		sut.CallBase = true;
		sut.Setup(s => s.LoadDataAsync()).ReturnsAsync("Adam");
		sut.Setup(s => s.ShouldRefresh()).Returns(false);

		// act
		await sut.Object.EnsureDataAsync();
		var result = sut.Object.GetValue();
		var isLoaded = sut.Object.IsLoaded;

		// assert
		Assert.Equal("Adam", result);
		sut.Verify(s => s.LoadDataAsync(), Times.Once);
		Assert.True(isLoaded);
	}

	[Fact]
	public async Task StaticDataStore_SecondCallShouldUseExistingData()
	{
		// arrange
		var sut = new Mock<StaticDataStore<string>>();
		sut.CallBase = true;
		sut.Setup(s => s.LoadDataAsync()).ReturnsAsync("Adam");
		sut.Setup(s => s.ShouldRefresh()).Returns(false);
		_ = await sut.Object.GetValueAsync();

		// act
		var isLoaded = sut.Object.IsLoaded;
		var result = await sut.Object.GetValueAsync();

		// assert
		Assert.True(isLoaded);
		sut.Verify(s => s.LoadDataAsync(), Times.Once);
		Assert.Equal("Adam", result);
	}

	[Fact]
	public async Task StaticDataStore_ShouldRefresh_SecondCallShouldReloadData()
	{
		// arrange
		var sut = new Mock<StaticDataStore<string>>();
		sut.CallBase = true;
		sut.Setup(s => s.LoadDataAsync()).ReturnsAsync("Adam");
		sut.Setup(s => s.ShouldRefresh()).Returns(true);
		_ = await sut.Object.GetValueAsync();

		// act
		var isLoaded = sut.Object.IsLoaded;
		var result = await sut.Object.GetValueAsync();

		// assert
		Assert.False(isLoaded);
		sut.Verify(s => s.LoadDataAsync(), Times.Exactly(2));
		Assert.Equal("Adam", result);
	}

	[Fact]
	public async Task StaticDataStore_Clear_ShouldReloadData()
	{
		// arrange
		var sut = new Mock<StaticDataStore<string>>();
		sut.CallBase = true;
		sut.Setup(s => s.LoadDataAsync()).ReturnsAsync("Adam");
		sut.Setup(s => s.ShouldRefresh()).Returns(false);
		_ = await sut.Object.GetValueAsync();

		// act
		sut.Object.Clear();
		var isLoaded = sut.Object.IsLoaded;
		var result = await sut.Object.GetValueAsync();

		// assert
		Assert.False(isLoaded);
		sut.Verify(s => s.LoadDataAsync(), Times.Exactly(2));
		Assert.Equal("Adam", result);
	}

	[Fact]
	public void StaticDataStore_ManyParallelCallsShouldNotReloadData()
	{
		// arrange
		var sut = new Mock<StaticDataStore<string>>();
		sut.CallBase = true;
		sut.Setup(s => s.LoadDataAsync()).ReturnsAsync("Adam");
		sut.Setup(s => s.ShouldRefresh()).Returns(false);

		var store = sut.Object;
		// act
		Parallel.For(0, 1000, async (int i) =>
		{
			_ = await store.GetValueAsync();
		});

		// assert
		sut.Verify(s => s.LoadDataAsync(), Times.Once);
	}

	[Fact]
	public void StaticDataStore_SyncNotLoaded_ShouldReturnDefault()
	{
		// arrange
		var sut = new Mock<StaticDataStore<string>>();
		sut.CallBase = true;
		sut.Setup(s => s.LoadDataAsync()).ReturnsAsync("Adam");
		sut.Setup(s => s.ShouldRefresh()).Returns(false);

		// act
		var result = sut.Object.GetValue();

		Assert.Null(result);
	}

	[Fact]
	public void StaticDataStore_SyncThrowIfNotLoaded_ShouldRaiseException()
	{
		// arrange
		var sut = new Mock<StaticDataStore<string>>();
		sut.CallBase = true;
		sut.Setup(s => s.LoadDataAsync()).ReturnsAsync("Adam");
		sut.Setup(s => s.ShouldRefresh()).Returns(false);

		// act
		Assert.Throws<InvalidOperationException>(() => sut.Object.GetValue(throwIfNotLoaded: true));

		// assert
		// expected exception
	}

	[Fact]
	public void StaticDataStore_IsLoadedBeforeLoad_ShouldReturnFalse()
	{
		// arrange
		var sut = new Mock<StaticDataStore<string>>();
		sut.CallBase = true;
		sut.Setup(s => s.LoadDataAsync()).ReturnsAsync("Adam");
		sut.Setup(s => s.ShouldRefresh()).Returns(false);

		// act
		var isLoaded = sut.Object.IsLoaded;

		// assert
		Assert.False(isLoaded);
	}
}
