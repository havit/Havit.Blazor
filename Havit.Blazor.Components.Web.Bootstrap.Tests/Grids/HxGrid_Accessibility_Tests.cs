using System.Linq.Expressions;
using Havit.Collections;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Grids;

public class HxGrid_Accessibility_Tests : BunitTestBase
{
	private record TestItem(int Id, string Name);

	[Fact]
	public async Task HxGrid_NoFilterHeaderTemplate_DoesNotRenderFilterHeaderRow()
	{
		// Arrange
		var items = Enumerable.Range(1, 3).Select(i => new TestItem(i, $"Item {i}")).ToList();
		Task<GridDataProviderResult<TestItem>> dataProvider(GridDataProviderRequest<TestItem> request) => Task.FromResult(request.ApplyTo(items));

		var cut = RenderComponent<HxGrid<TestItem>>(parameters => parameters
			.Add(p => p.DataProvider, dataProvider)
			.Add<HxGridColumn<TestItem>>(p => p.Columns, column => column
				.Add(c => c.HeaderText, "Name")
				.Add(c => c.ItemTextSelector, item => item.Name)));

		// Act
		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert — only the main header row is rendered, no extra filter header row
		Assert.Single(cut.FindAll("thead tr"));
	}

	[Fact]
	public async Task HxGrid_WithFilterHeaderTemplate_RendersFilterHeaderRow()
	{
		// Arrange
		var items = Enumerable.Range(1, 3).Select(i => new TestItem(i, $"Item {i}")).ToList();
		Task<GridDataProviderResult<TestItem>> dataProvider(GridDataProviderRequest<TestItem> request) => Task.FromResult(request.ApplyTo(items));

		var cut = RenderComponent<HxGrid<TestItem>>(parameters => parameters
			.Add(p => p.DataProvider, dataProvider)
			.Add<HxGridColumn<TestItem>>(p => p.Columns, column => column
				.Add(c => c.HeaderText, "Name")
				.Add(c => c.ItemTextSelector, item => item.Name)
				.Add(c => c.HeaderFilterTemplate, (RenderFragment)(builder =>
				{
					builder.OpenElement(0, "input");
					builder.AddAttribute(1, "class", "custom-filter");
					builder.CloseElement();
				}))));

		// Act
		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert — a second header row is rendered containing the filter template content
		Assert.Equal(2, cut.FindAll("thead tr").Count);
		Assert.Single(cut.FindAll("thead input.custom-filter"));
	}

	[Fact]
	public async Task HxGrid_FilterHeaderRow_AppliesHeaderCssClass()
	{
		// Arrange — regression: the filter header cell should carry the column's HeaderCssClass
		var items = Enumerable.Range(1, 3).Select(i => new TestItem(i, $"Item {i}")).ToList();
		Task<GridDataProviderResult<TestItem>> dataProvider(GridDataProviderRequest<TestItem> request) => Task.FromResult(request.ApplyTo(items));

		var cut = RenderComponent<HxGrid<TestItem>>(parameters => parameters
			.Add(p => p.DataProvider, dataProvider)
			.Add<HxGridColumn<TestItem>>(p => p.Columns, column => column
				.Add(c => c.HeaderText, "Name")
				.Add(c => c.ItemTextSelector, item => item.Name)
				.Add(c => c.HeaderCssClass, "my-header-css")
				.Add(c => c.HeaderFilterTemplate, (RenderFragment)(builder =>
				{
					builder.OpenElement(0, "input");
					builder.AddAttribute(1, "class", "custom-filter");
					builder.CloseElement();
				}))));

		// Act
		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert — the filter header row's <th> uses the configured HeaderCssClass
		var filterRow = cut.FindAll("thead tr")[1];
		var filterCell = filterRow.QuerySelector("th");
		Assert.NotNull(filterCell);
		Assert.True(filterCell.ClassList.Contains("my-header-css"), "Filter header cell should carry the column's HeaderCssClass.");
	}

	[Fact]
	public async Task HxGrid_SortableColumnWithTabIndex_RendersTabIndexAttribute()
	{
		// Arrange
		var items = Enumerable.Range(1, 3).Select(i => new TestItem(i, $"Item {i}")).ToList();
		Task<GridDataProviderResult<TestItem>> dataProvider(GridDataProviderRequest<TestItem> request) => Task.FromResult(request.ApplyTo(items));

		Expression<Func<TestItem, IComparable>> sortKeySelector = item => item.Name;

		var cut = RenderComponent<HxGrid<TestItem>>(parameters => parameters
			.Add(p => p.DataProvider, dataProvider)
			.Add<HxGridColumn<TestItem>>(p => p.Columns, column => column
				.Add(c => c.HeaderText, "Name")
				.Add(c => c.ItemTextSelector, item => item.Name)
				.Add(c => c.SortKeySelector, sortKeySelector)
				.Add(c => c.TabIndex, 5)));

		// Act
		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert — sortable header is focusable via the configured tabindex
		var header = cut.Find("th.hx-grid-sortable");
		Assert.Equal("5", header.GetAttribute("tabindex"));
		Assert.Equal("button", header.GetAttribute("role"));
	}

	[Fact]
	public async Task HxGrid_NonSortableColumn_DoesNotRenderTabIndexAttribute()
	{
		// Arrange
		var items = Enumerable.Range(1, 3).Select(i => new TestItem(i, $"Item {i}")).ToList();
		Task<GridDataProviderResult<TestItem>> dataProvider(GridDataProviderRequest<TestItem> request) => Task.FromResult(request.ApplyTo(items));

		var cut = RenderComponent<HxGrid<TestItem>>(parameters => parameters
			.Add(p => p.DataProvider, dataProvider)
			.Add<HxGridColumn<TestItem>>(p => p.Columns, column => column
				.Add(c => c.HeaderText, "Name")
				.Add(c => c.ItemTextSelector, item => item.Name)
				.Add(c => c.TabIndex, 5)));

		// Act
		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert — non-sortable header is not focusable (no tabindex emitted)
		var header = cut.FindAll("th")[0];
		Assert.Null(header.GetAttribute("tabindex"));
	}

	[Fact]
	public async Task HxGrid_PressEnterKeyOnSortableHeader_SortsColumn()
	{
		// Arrange
		var items = Enumerable.Range(1, 5).Select(i => new TestItem(i, $"Item {i}")).ToList();
		SortDirection? lastSortDirection = null;

		Task<GridDataProviderResult<TestItem>> dataProvider(GridDataProviderRequest<TestItem> request)
		{
			if (request.Sorting != null && request.Sorting.Any())
			{
				lastSortDirection = request.Sorting[0].SortDirection;
			}
			return Task.FromResult(request.ApplyTo(items));
		}

		Expression<Func<TestItem, IComparable>> sortKeySelector = item => item.Name;

		var cut = RenderComponent<HxGrid<TestItem>>(parameters => parameters
			.Add(p => p.DataProvider, dataProvider)
			.Add<HxGridColumn<TestItem>>(p => p.Columns, column => column
				.Add(c => c.HeaderText, "Name")
				.Add(c => c.ItemTextSelector, item => item.Name)
				.Add(c => c.SortKeySelector, sortKeySelector)));

		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Act — activate sorting via keyboard (Enter), as a keyboard user would
		var header = cut.Find("th.hx-grid-sortable");
		await cut.InvokeAsync(() => header.KeyPress(new KeyboardEventArgs { Key = "Enter" }));

		// Assert — column is now sorted ascending
		Assert.NotEmpty(cut.FindAll("th.hx-grid-sorted"));
		Assert.Equal(SortDirection.Ascending, lastSortDirection);
	}
}
