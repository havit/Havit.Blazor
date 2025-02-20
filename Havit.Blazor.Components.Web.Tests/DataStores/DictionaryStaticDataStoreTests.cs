using Havit.Blazor.Components.Web.Services.DataStores;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Havit.Blazor.Components.Web.Tests.DataStores;

[TestClass]
public class DictionaryStaticDataStoreTests
{
	[TestMethod]
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
		CollectionAssert.AreEqual(new[] { "Adam", "Barbora", "Cyril" }, result.ToList());
		sut.Verify(s => s.LoadDataAsync(), Times.Once);
		Assert.IsTrue(isLoaded);
	}

	[TestMethod]
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
		CollectionAssert.AreEqual(new[] { "Adam", "Barbora", "Cyril" }, result.ToList());
		sut.Verify(s => s.LoadDataAsync(), Times.Once);
	}

	[TestMethod]
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
		Assert.AreEqual("Adam", result);
	}

	[TestMethod]
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
		Assert.AreEqual("Adam", result);
	}

	[TestMethod]
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
		Assert.IsTrue(isLoaded);
		sut.Verify(s => s.LoadDataAsync(), Times.Once);
		CollectionAssert.AreEqual(new[] { "Adam", "Barbora", "Cyril" }, result.ToList());
	}

	[TestMethod]
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
		Assert.IsFalse(isLoaded);
		sut.Verify(s => s.LoadDataAsync(), Times.Exactly(2));
		CollectionAssert.AreEqual(new[] { "Adam", "Barbora", "Cyril" }, result.ToList());
	}

	[TestMethod]
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
		Assert.IsFalse(isLoaded);
		sut.Verify(s => s.LoadDataAsync(), Times.Exactly(2));
		CollectionAssert.AreEqual(new[] { "Adam", "Barbora", "Cyril" }, result.ToList());
	}

	[TestMethod]
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

	[TestMethod]
	public void DictionaryStaticDataStore_SyncWithThrowIfNotLoaded_ShouldRaiseException()
	{
		// arrange
		var sut = new Mock<DictionaryStaticDataStore<char, string>>();
		sut.CallBase = true;
		sut.SetupGet(s => s.KeySelector).Returns(value => value[0]);
		sut.Setup(s => s.LoadDataAsync()).ReturnsAsync(new[] { "Adam", "Barbora", "Cyril" });
		sut.Setup(s => s.ShouldRefresh()).Returns(false);

		// act
		Assert.ThrowsExactly<InvalidOperationException>(() => sut.Object.GetAll(throwIfNotLoaded: true));

		// assert
		// expected exception
	}

	[TestMethod]
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
		Assert.IsNull(result);
	}


	[TestMethod]
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
		Assert.IsFalse(isLoaded);
	}
}
