using Havit.Blazor.Components.Web.Services.DataStores;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Havit.Blazor.Components.Web.Tests.DataStores;

[TestClass]
public class StaticDataStoreTests
{
	[TestMethod]
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
		Assert.AreEqual("Adam", result);
		sut.Verify(s => s.LoadDataAsync(), Times.Once);
		Assert.IsTrue(isLoaded);
	}

	[TestMethod]
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
		Assert.AreEqual("Adam", result);
		sut.Verify(s => s.LoadDataAsync(), Times.Once);
		Assert.IsTrue(isLoaded);
	}

	[TestMethod]
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
		Assert.IsTrue(isLoaded);
		sut.Verify(s => s.LoadDataAsync(), Times.Once);
		Assert.AreEqual("Adam", result);
	}

	[TestMethod]
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
		Assert.IsFalse(isLoaded);
		sut.Verify(s => s.LoadDataAsync(), Times.Exactly(2));
		Assert.AreEqual("Adam", result);
	}

	[TestMethod]
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
		Assert.IsFalse(isLoaded);
		sut.Verify(s => s.LoadDataAsync(), Times.Exactly(2));
		Assert.AreEqual("Adam", result);
	}

	[TestMethod]
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

	[TestMethod]
	public void StaticDataStore_SyncNotLoaded_ShouldReturnDefault()
	{
		// arrange
		var sut = new Mock<StaticDataStore<string>>();
		sut.CallBase = true;
		sut.Setup(s => s.LoadDataAsync()).ReturnsAsync("Adam");
		sut.Setup(s => s.ShouldRefresh()).Returns(false);

		// act
		var result = sut.Object.GetValue();

		Assert.IsNull(result);
	}

	[TestMethod]
	public void StaticDataStore_SyncThrowIfNotLoaded_ShouldRaiseException()
	{
		// arrange
		var sut = new Mock<StaticDataStore<string>>();
		sut.CallBase = true;
		sut.Setup(s => s.LoadDataAsync()).ReturnsAsync("Adam");
		sut.Setup(s => s.ShouldRefresh()).Returns(false);

		// act
		Assert.ThrowsExactly<InvalidOperationException>(() => sut.Object.GetValue(throwIfNotLoaded: true));

		// assert
		// expected exception
	}

	[TestMethod]
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
		Assert.IsFalse(isLoaded);
	}
}
