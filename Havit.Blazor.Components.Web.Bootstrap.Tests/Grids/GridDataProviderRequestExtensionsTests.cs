using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Grids;

[TestClass]
public class GridDataProviderRequestExtensionsTests
{
	[TestMethod]
	public void GridDataProviderRequestExtensions_ApplyGridDataProviderRequest_InMemory_AppliesSortingAndPaging()
	{
		// Arrange
		var source = new List<Item>
		{
			new Item{ A = "2", B = 3 },
			new Item{ A = "2", B = 2 },
			new Item{ A = "2", B = 1 },
			new Item{ A = "1", B = 1 },
			new Item{ A = "1", B = 2 },
			new Item{ A = "1", B = 3 }
		}.AsQueryable();

		var gridDataProviderRequest = new GridDataProviderRequest<Item>
		{
			Sorting = new List<SortingItem<Item>>
			{
				new SortingItem<Item>(null, item => item.A, Collections.SortDirection.Ascending),
				new SortingItem<Item>(null, item => item.B, Collections.SortDirection.Descending),
			},
			StartIndex = 1, // zero based (skipping first item), to verify paging
			Count = 3 // to verify paging
		};

		// Act
		var result = source.ApplyGridDataProviderRequest(gridDataProviderRequest).ToList();

		// Assert
		Assert.HasCount(3, result);
		Assert.AreEqual(new Item { A = "1", B = 2 }, result[0]);
		Assert.AreEqual(new Item { A = "1", B = 1 }, result[1]);
		Assert.AreEqual(new Item { A = "2", B = 3 }, result[2]);
	}

	private record Item // record: for comparison purposes
	{
		public string A { get; set; }
		public int B { get; set; }
	}
}
