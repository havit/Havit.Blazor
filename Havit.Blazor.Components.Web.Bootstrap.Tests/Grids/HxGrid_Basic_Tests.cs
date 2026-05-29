using System.Linq.Expressions;
using Havit.Collections;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Grids;

public class HxGrid_Basic_Tests : BunitTestBase
{
	private record TestItem(int Id, string Name);

	[Fact]
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
		Assert.Equal(5, dataRows.Count());
	}

	[Fact]
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
		Assert.NotEmpty(cut.FindAll("th.hx-grid-sorted"));
		Assert.Equal(SortDirection.Ascending, lastSortDirection);
	}

	[Fact]
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
		Assert.Equal(SortDirection.Descending, lastSortDirection);
	}

	[Fact]
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
		Assert.Single(cut.FindAll("tr.hx-grid-empty-data-row"));
	}

	[Fact]
	public async Task HxGrid_EmptyData_RendersCustomEmptyDataTemplate()
	{
		// Arrange — regression for #623: custom EmptyDataTemplate should render when data is empty
		GridDataProviderDelegate<TestItem> dataProvider = (GridDataProviderRequest<TestItem> request) =>
			Task.FromResult(new GridDataProviderResult<TestItem> { Data = [], TotalCount = 0 });

		var cut = RenderComponent<HxGrid<TestItem>>(parameters => parameters
			.Add(p => p.DataProvider, dataProvider)
			.Add(p => p.EmptyDataTemplate, (RenderFragment)(builder =>
			{
				builder.OpenElement(0, "div");
				builder.AddAttribute(1, "class", "custom-empty");
				builder.AddContent(2, "No records found");
				builder.CloseElement();
			}))
			.Add<HxGridColumn<TestItem>>(p => p.Columns, column => column
				.Add(c => c.HeaderText, "Name")
				.Add(c => c.ItemTextSelector, item => item.Name)));

		// Act
		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert — custom empty template is rendered inside the empty data row
		var customEmpty = cut.Find("div.custom-empty");
		Assert.Contains("No records found", customEmpty.TextContent);
	}

	[Fact]
	public async Task HxGrid_NonSortableColumnHeader_IsNotInteractive()
	{
		// Arrange — regression for #1166: non-sortable column header should not be clickable
		var items = Enumerable.Range(1, 3).Select(i => new TestItem(i, $"Item {i}")).ToList();
		GridDataProviderDelegate<TestItem> dataProvider = (GridDataProviderRequest<TestItem> request) =>
			Task.FromResult(request.ApplyTo(items));

		var cut = RenderComponent<HxGrid<TestItem>>(parameters => parameters
			.Add(p => p.DataProvider, dataProvider)
			.Add<HxGridColumn<TestItem>>(p => p.Columns, column => column
				.Add(c => c.HeaderText, "Name")
				.Add(c => c.ItemTextSelector, item => item.Name)));

		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert — non-sortable header should not be interactive (no sortable class, no role)
		var headers = cut.FindAll("th");
		Assert.NotEmpty(headers);
		var header = headers[0];
		Assert.False(header.ClassList.Contains("hx-grid-sortable"), "Non-sortable column should not have sortable CSS class.");
		Assert.Null(header.GetAttribute("role"));
	}

}
