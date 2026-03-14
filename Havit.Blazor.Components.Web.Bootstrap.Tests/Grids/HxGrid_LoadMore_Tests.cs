namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Grids;

[TestClass]
public class HxGrid_LoadMore_Tests : BunitTestBase
{
	[TestMethod]
	public async Task HxGrid_LoadMore_ButtonShouldRemainVisibleDuringLoadingOnSecondToLastPage()
	{
		// Arrange: 20 items with page size 10
		// Clicking Load More once will load all remaining items (second-to-last page scenario).
		var items = Enumerable.Range(1, 20).Select(i => new { Id = i, Name = $"Item {i}" }).ToList();

		var dataProviderCallCount = 0;
		var dataProviderTCS = new TaskCompletionSource();

		GridDataProviderDelegate<object> dataProvider = async (GridDataProviderRequest<object> request) =>
		{
			dataProviderCallCount++;
			if (dataProviderCallCount > 1)
			{
				await dataProviderTCS.Task; // simulate slow loading on subsequent calls
			}
			return request.ApplyTo(items);
		};

		var cut = RenderComponent<HxGrid<object>>(parameters => parameters
			.Add(p => p.DataProvider, dataProvider)
			.Add(p => p.PageSize, 10)
			.Add(p => p.ContentNavigationMode, GridContentNavigationMode.LoadMore)
			.Add<HxGridColumn<object>>(p => p.Columns, column => column
				.Add(c => c.HeaderText, "Id")
				.Add(c => c.ItemTextSelector, item => item.ToString())));

		// Verify: Load More button should be visible initially (10 items shown out of 20 total)
		Assert.HasCount(1, cut.FindAll(".hx-grid-load-more-container"), "Load More button should be visible initially.");

		// Act: Start loading more data (data provider will block on the second call)
		var loadMoreTask = cut.InvokeAsync(() => cut.Instance.LoadMoreAsync());

		// Assert: Button should remain visible while loading is in progress
		// (Without the fix, the button would disappear because LoadMoreAdditionalItemsCount is already incremented)
		Assert.HasCount(1, cut.FindAll(".hx-grid-load-more-container"), "Load More button should remain visible during loading.");

		// Complete the data loading
		dataProviderTCS.SetResult();
		await loadMoreTask;

		// Assert: Button should disappear after all data is loaded (all 20 items are now displayed)
		Assert.IsEmpty(cut.FindAll(".hx-grid-load-more-container"), "Load More button should disappear after all data is loaded.");
	}
}
