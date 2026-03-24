using System.Linq.Expressions;
using Havit.Collections;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Grids;

[TestClass]
public class HxGrid_Basic_Tests : BunitTestBase
{
	private record TestItem(int Id, string Name);

	[TestMethod]
	public async Task HxGrid_Render_DisplaysCorrectRowCount()
	{
		// Arrange
		var items = Enumerable.Range(1, 5).Select(i => new TestItem(i, $"Item {i}")).ToList();

		GridDataProviderDelegate<TestItem> dataProvider = (GridDataProviderRequest<TestItem> request) => Task.FromResult(request.ApplyTo(items));

		var cut = RenderComponent<HxGrid<TestItem>>(parameters => parameters
			.Add(p => p.DataProvider, dataProvider)
			.Add<HxGridColumn<TestItem>>(p => p.Columns, column => column
				.Add(c => c.HeaderText, "Name")
				.Add(c => c.ItemTextSelector, item => item.Name)));

		// Act: wait for data to load
		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert: 5 data rows rendered (excluding empty-data and placeholder rows)
		var dataRows = cut.FindAll("tbody tr:not(.hx-grid-empty-data-row)");
		Assert.HasCount(5, dataRows);
	}

	[TestMethod]
	public async Task HxGrid_ClickColumnHeader_SortsAscending()
	{
		// Arrange
		var items = Enumerable.Range(1, 5).Select(i => new TestItem(i, $"Item {i}")).ToList();
		SortDirection? lastSortDirection = null;

		GridDataProviderDelegate<TestItem> dataProvider = (GridDataProviderRequest<TestItem> request) =>
		{
			if (request.Sorting != null && request.Sorting.Any())
			{
				lastSortDirection = request.Sorting[0].SortDirection;
			}
			return Task.FromResult(request.ApplyTo(items));
		};

		Expression<Func<TestItem, IComparable>> sortKeySelector = item => item.Name;

		var cut = RenderComponent<HxGrid<TestItem>>(parameters => parameters
			.Add(p => p.DataProvider, dataProvider)
			.Add<HxGridColumn<TestItem>>(p => p.Columns, column => column
				.Add(c => c.HeaderText, "Name")
				.Add(c => c.ItemTextSelector, item => item.Name)
				.Add(c => c.SortKeySelector, sortKeySelector)));

		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Act: click the sortable column header
		var header = cut.Find("th.hx-grid-sortable");
		await cut.InvokeAsync(() => header.Click());

		// Assert: column is now marked as sorted and sort direction is ascending
		Assert.IsTrue(cut.FindAll("th.hx-grid-sorted").Any(), "Column header should have the sorted CSS class.");
		Assert.AreEqual(SortDirection.Ascending, lastSortDirection);
	}

	[TestMethod]
	public async Task HxGrid_ClickColumnHeaderTwice_SortsDescending()
	{
		// Arrange
		var items = Enumerable.Range(1, 5).Select(i => new TestItem(i, $"Item {i}")).ToList();
		SortDirection? lastSortDirection = null;

		GridDataProviderDelegate<TestItem> dataProvider = (GridDataProviderRequest<TestItem> request) =>
		{
			if (request.Sorting != null && request.Sorting.Any())
			{
				lastSortDirection = request.Sorting[0].SortDirection;
			}
			return Task.FromResult(request.ApplyTo(items));
		};

		Expression<Func<TestItem, IComparable>> sortKeySelector = item => item.Name;

		var cut = RenderComponent<HxGrid<TestItem>>(parameters => parameters
			.Add(p => p.DataProvider, dataProvider)
			.Add<HxGridColumn<TestItem>>(p => p.Columns, column => column
				.Add(c => c.HeaderText, "Name")
				.Add(c => c.ItemTextSelector, item => item.Name)
				.Add(c => c.SortKeySelector, sortKeySelector)));

		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Act: click the column header twice to sort descending
		var header = cut.Find("th.hx-grid-sortable");
		await cut.InvokeAsync(() => header.Click());
		await cut.InvokeAsync(() => cut.Find("th.hx-grid-sorted").Click());

		// Assert: sort direction is now descending
		Assert.AreEqual(SortDirection.Descending, lastSortDirection);
	}

	[TestMethod]
	public async Task HxGrid_EmptyData_ShowsEmptyMessage()
	{
		// Arrange
		GridDataProviderDelegate<TestItem> dataProvider = (GridDataProviderRequest<TestItem> request) =>
			Task.FromResult(new GridDataProviderResult<TestItem> { Data = [], TotalCount = 0 });

		var cut = RenderComponent<HxGrid<TestItem>>(parameters => parameters
			.Add(p => p.DataProvider, dataProvider)
			.Add<HxGridColumn<TestItem>>(p => p.Columns, column => column
				.Add(c => c.HeaderText, "Name")
				.Add(c => c.ItemTextSelector, item => item.Name)));

		// Act: wait for data to load
		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert: empty data row is displayed
		Assert.HasCount(1, cut.FindAll("tr.hx-grid-empty-data-row"), "Empty data row should be rendered when there are no items.");
	}

}
