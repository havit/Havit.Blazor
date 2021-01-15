using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Services.DataStores;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Havit.Blazor.Components.Web.Tests.DataStores
{
	[TestClass]
	public class DictionaryStaticDataStoreTests
	{
		[TestMethod]
		public async Task DictionaryStaticDataStore_FirstLoad()
		{
			// arrange
			var sut = new Mock<DictionaryStaticDataStore<char, string>>();
			sut.CallBase = true;
			sut.SetupGet(s => s.KeySelector).Returns(value => value[0]);
			sut.Setup(s => s.LoadDataAsync()).ReturnsAsync(new[] { "Adam", "Barbora", "Cyril" });
			sut.Setup(s => s.ShouldRefresh()).Returns(false);

			// act
			var result = await sut.Object.GetAllAsync();

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
			var result = await sut.Object.GetAllAsync();

			// assert
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
			var result = await sut.Object.GetAllAsync();

			// assert
			sut.Verify(s => s.LoadDataAsync(), Times.Exactly(2));
			CollectionAssert.AreEqual(new[] { "Adam", "Barbora", "Cyril" }, result.ToList());
		}
	}
}
