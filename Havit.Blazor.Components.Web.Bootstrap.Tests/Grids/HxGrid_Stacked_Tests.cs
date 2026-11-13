using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Grids;

public class HxGrid_Stacked_Tests : BunitTestBase
{
	private record TestItem(int Id, string Name, int Age);

	private static readonly List<TestItem> s_items = Enumerable.Range(1, 3).Select(i => new TestItem(i, $"Item {i}", 20 + i)).ToList();

	[Fact]
	public async Task HxGrid_StackedNotSet_DoesNotRenderStackedMarkup()
	{
		// Arrange
		var cut = RenderGrid(parameters => { });

		// Act
		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert — default (GridStacked.Never): no stacked class, no data-cell labels, no responsive wrapper
		var table = cut.Find("table");
		Assert.DoesNotContain(table.ClassList, cssClass => cssClass.Contains("table-stacked"));
		Assert.Empty(cut.FindAll("td[data-cell]"));
		Assert.False(cut.Find(".hx-grid-container").ClassList.Contains("table-responsive"));
		Assert.False(cut.Find(".hx-grid-container").ClassList.Contains("hx-grid-container-stacked"));
		Assert.Empty(cut.FindAll(".hx-grid-stacked-title"));
	}

	[Theory]
	[InlineData(GridStacked.Always, "table-stacked")]
	[InlineData(GridStacked.BelowSmall, "sm:table-stacked")]
	[InlineData(GridStacked.BelowMedium, "md:table-stacked")]
	[InlineData(GridStacked.BelowLarge, "lg:table-stacked")]
	[InlineData(GridStacked.BelowExtraLarge, "xl:table-stacked")]
	[InlineData(GridStacked.BelowXxl, "2xl:table-stacked")]
	public async Task HxGrid_Stacked_RendersBootstrapStackedTableCssClass(GridStacked stacked, string expectedCssClass)
	{
		// Arrange
		var cut = RenderGrid(parameters => parameters.Add(p => p.Stacked, stacked));

		// Act
		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert
		var table = cut.Find("table");
		Assert.Contains(expectedCssClass, table.ClassList);
		Assert.Single(table.ClassList, cssClass => cssClass.Contains("table-stacked"));
	}

	[Fact]
	public async Task HxGrid_StackedFromSettings_RendersBootstrapStackedTableCssClass()
	{
		// Arrange
		var cut = RenderGrid(parameters => parameters.Add(p => p.Settings, new GridSettings { Stacked = GridStacked.BelowLarge }));

		// Act
		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert
		Assert.Contains("lg:table-stacked", cut.Find("table").ClassList);
	}

	[Fact]
	public async Task HxGrid_StackedParameter_OverridesSettings()
	{
		// Arrange
		var cut = RenderGrid(parameters => parameters
			.Add(p => p.Settings, new GridSettings { Stacked = GridStacked.BelowLarge })
			.Add(p => p.Stacked, GridStacked.Never));

		// Act
		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert
		Assert.DoesNotContain(cut.Find("table").ClassList, cssClass => cssClass.Contains("table-stacked"));
		Assert.Empty(cut.FindAll("td[data-cell]"));
	}

	[Theory]
	[InlineData(GridStacked.BelowSmall)]
	[InlineData(GridStacked.BelowMedium)]
	[InlineData(GridStacked.BelowXxl)]
	public async Task HxGrid_StackedBelowBreakpoint_RendersQueryContainerWithoutResponsiveOverflow(GridStacked stacked)
	{
		// Arrange — the container is the query container of the stacked table container queries,
		// but it must not get the table-responsive overflow (it would become the scroll root of the infinite scroll virtualization)
		var cut = RenderGrid(parameters => parameters
			.Add(p => p.Stacked, stacked)
			.Add(p => p.Responsive, false));

		// Act
		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert
		var container = cut.Find(".hx-grid-container");
		Assert.Contains("hx-grid-container-stacked", container.ClassList);
		Assert.DoesNotContain("table-responsive", container.ClassList);
	}

	[Fact]
	public async Task HxGrid_StackedAlways_DoesNotRenderQueryContainer()
	{
		// Arrange — .table-stacked does not use container queries
		var cut = RenderGrid(parameters => parameters.Add(p => p.Stacked, GridStacked.Always));

		// Act
		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert
		Assert.DoesNotContain("hx-grid-container-stacked", cut.Find(".hx-grid-container").ClassList);
	}

	[Fact]
	public async Task HxGrid_StackedAndResponsive_RendersBothContainerCssClasses()
	{
		// Arrange
		var cut = RenderGrid(parameters => parameters
			.Add(p => p.Stacked, GridStacked.BelowMedium)
			.Add(p => p.Responsive, true));

		// Act
		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert
		var container = cut.Find(".hx-grid-container");
		Assert.Contains("hx-grid-container-stacked", container.ClassList);
		Assert.Contains("table-responsive", container.ClassList);
	}

	[Fact]
	public async Task HxGrid_Stacked_DataCellsHaveDataCellLabelFromHeaderText()
	{
		// Arrange
		var cut = RenderGrid(parameters => parameters.Add(p => p.Stacked, GridStacked.BelowMedium));

		// Act
		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert — every data cell carries the plain-text header of its column
		var rows = cut.FindAll("tbody tr");
		Assert.Equal(s_items.Count, rows.Count);
		Assert.All(rows, row =>
		{
			var cells = row.QuerySelectorAll("td");
			Assert.Equal(["Name", "Age"], cells.Select(cell => cell.GetAttribute("data-cell")));
		});
		Assert.Equal("Item 1", cut.Find("tbody tr td[data-cell=Name]").TextContent.Trim());
	}

	[Fact]
	public async Task HxGrid_Stacked_StackedLabelOverridesHeaderText()
	{
		// Arrange
		var cut = RenderGrid(
			parameters => parameters.Add(p => p.Stacked, GridStacked.Always),
			ageColumn: column => column
				.Add(c => c.HeaderText, "Age (years)")
				.Add(c => c.StackedLabel, "Age"));

		// Act
		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert
		Assert.Equal(s_items.Count, cut.FindAll("tbody td[data-cell=Age]").Count);
		Assert.Empty(cut.FindAll("tbody td[data-cell='Age (years)']"));
	}

	[Fact]
	public async Task HxGrid_Stacked_StackedLabelProvidesLabelForHeaderTemplate()
	{
		// Arrange — a header template cannot be used as a plain-text label
		var cut = RenderGrid(
			parameters => parameters.Add(p => p.Stacked, GridStacked.Always),
			ageColumn: column => column
				.Add(c => c.HeaderTemplate, (RenderFragment<GridHeaderCellContext>)(context => builder =>
				{
					builder.OpenElement(0, "strong");
					builder.AddContent(1, "Age");
					builder.CloseElement();
				}))
				.Add(c => c.StackedLabel, "Age"));

		// Act
		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert
		Assert.Equal(s_items.Count, cut.FindAll("tbody td[data-cell=Age]").Count);
	}

	[Fact]
	public async Task HxGrid_Stacked_HeaderTemplateWithoutStackedLabel_RendersCellsWithoutDataCell()
	{
		// Arrange
		var cut = RenderGrid(
			parameters => parameters.Add(p => p.Stacked, GridStacked.Always),
			ageColumn: column => column
				.Add(c => c.HeaderTemplate, (RenderFragment<GridHeaderCellContext>)(context => builder => builder.AddContent(0, "Age"))));

		// Act
		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert — Name cells are labeled, Age cells (template header, no StackedLabel) are not
		Assert.Equal(s_items.Count, cut.FindAll("tbody td[data-cell]").Count);
		Assert.All(cut.FindAll("tbody td[data-cell]"), cell => Assert.Equal("Name", cell.GetAttribute("data-cell")));
	}

	[Fact]
	public async Task HxGrid_Stacked_EmptyStackedLabel_RendersCellsWithoutDataCell()
	{
		// Arrange
		var cut = RenderGrid(
			parameters => parameters.Add(p => p.Stacked, GridStacked.Always),
			ageColumn: column => column
				.Add(c => c.HeaderText, "Age")
				.Add(c => c.StackedLabel, String.Empty));

		// Act
		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert
		Assert.Empty(cut.FindAll("tbody td[data-cell=Age]"));
		Assert.Empty(cut.FindAll("tbody td[data-cell='']"));
	}

	[Fact]
	public async Task HxGrid_Stacked_MultiSelectAndContextMenuCellsHaveNoDataCell()
	{
		// Arrange
		var cut = Render<HxGrid<TestItem>>(parameters => parameters
			.Add(p => p.DataProvider, GetDataProvider())
			.Add(p => p.Stacked, GridStacked.BelowMedium)
			.Add(p => p.MultiSelectionEnabled, true)
			.Add<HxGridColumn<TestItem>>(p => p.Columns, column => column
				.Add(c => c.HeaderText, "Name")
				.Add(c => c.ItemTextSelector, item => item.Name))
			.Add<HxContextMenuGridColumn<TestItem>>(p => p.Columns, column => column
				.Add(c => c.ChildContent, item => "menu")));

		// Act
		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert
		Assert.Equal(s_items.Count, cut.FindAll("tbody td.hx-grid-multiselect-cell").Count);
		Assert.Empty(cut.FindAll("tbody td.hx-grid-multiselect-cell[data-cell]"));
		Assert.Equal(s_items.Count, cut.FindAll("tbody td.hx-context-menu-grid-column").Count);
		Assert.Empty(cut.FindAll("tbody td.hx-context-menu-grid-column[data-cell]"));
		Assert.Equal(s_items.Count, cut.FindAll("tbody td[data-cell=Name]").Count);
	}

	[Fact]
	public void HxGrid_Stacked_PlaceholderCellsHaveDataCell()
	{
		// Arrange — the data provider never completes, so the placeholders stay rendered
		var dataProviderTcs = new TaskCompletionSource<GridDataProviderResult<TestItem>>();

		// Act
		var cut = RenderGrid(
			parameters => parameters
				.Add(p => p.Stacked, GridStacked.Always)
				.Add(p => p.MultiSelectionEnabled, true)
				.Add(p => p.PlaceholdersRowCount, 2),
			dataProvider: request => dataProviderTcs.Task);

		// Assert
		var rows = cut.FindAll("tbody tr");
		Assert.Equal(2, rows.Count);
		Assert.All(rows, row =>
		{
			var cells = row.QuerySelectorAll("td");
			Assert.Equal([null, "Name", "Age"], cells.Select(cell => cell.GetAttribute("data-cell")));
			Assert.Contains("hx-grid-multiselect-cell", cells[0].ClassList); // the next cell is the title of the stacked row
		});
	}

	[Fact]
	public void HxGrid_Stacked_ContextMenuPlaceholderCellsHaveContextMenuColumnCssClass()
	{
		// Arrange — the data provider never completes, so the placeholders stay rendered.
		// The stacked layout takes the context menu cell out of the flow (top-end corner of the row) by its CSS class;
		// without the class, the placeholder rows would be taller than the data rows (skews the infinite scroll item size).
		var dataProviderTcs = new TaskCompletionSource<GridDataProviderResult<TestItem>>();

		// Act
		var cut = Render<HxGrid<TestItem>>(parameters => parameters
			.Add(p => p.DataProvider, request => dataProviderTcs.Task)
			.Add(p => p.Stacked, GridStacked.Always)
			.Add(p => p.ContentNavigationMode, GridContentNavigationMode.InfiniteScroll)
			.Add(p => p.PlaceholdersRowCount, 2)
			.Add<HxGridColumn<TestItem>>(p => p.Columns, column => column
				.Add(c => c.HeaderText, "Name")
				.Add(c => c.ItemTextSelector, item => item.Name))
			.Add<HxContextMenuGridColumn<TestItem>>(p => p.Columns, column => column
				.Add(c => c.ItemCssClass, "custom-cell")
				.Add(c => c.ChildContent, item => "menu")));

		// Assert — the rows with cells (the empty rows are the Virtualize spacers)
		var rows = cut.FindAll("tbody tr").Where(row => row.QuerySelectorAll("td").Length > 0).ToList();
		Assert.Equal(2, rows.Count);
		Assert.All(rows, row =>
		{
			var contextMenuCell = row.QuerySelectorAll("td").Last();
			Assert.Contains("hx-context-menu-grid-column", contextMenuCell.ClassList);
			Assert.Contains("custom-cell", contextMenuCell.ClassList);
			Assert.Equal("", contextMenuCell.InnerHtml);
			Assert.Null(contextMenuCell.GetAttribute("data-cell"));
		});
	}

	[Fact]
	public async Task HxGrid_Stacked_TitleCellIsTheFirstDataColumn_SkipsMultiSelectAndContextMenuColumns()
	{
		// Arrange — the context menu column ordered before the data columns, multi-select enabled
		var cut = Render<HxGrid<TestItem>>(parameters => parameters
			.Add(p => p.DataProvider, GetDataProvider())
			.Add(p => p.Stacked, GridStacked.BelowMedium)
			.Add(p => p.MultiSelectionEnabled, true)
			.Add<HxContextMenuGridColumn<TestItem>>(p => p.Columns, column => column
				.Add(c => c.Order, -1)
				.Add(c => c.ChildContent, item => "menu"))
			.Add<HxGridColumn<TestItem>>(p => p.Columns, column => column
				.Add(c => c.HeaderText, "Name")
				.Add(c => c.ItemTextSelector, item => item.Name)
				.Add(c => c.FooterText, "Total"))
			.Add<HxGridColumn<TestItem>>(p => p.Columns, column => column
				.Add(c => c.HeaderText, "Age")
				.Add(c => c.ItemTextSelector, item => item.Age.ToString())
				.Add(c => c.FooterText, "66")));

		// Act
		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert — in both the data rows and the footer, the title is the Name cell (not the first cell, not the context menu cell)
		var rows = cut.FindAll("tbody tr").Concat(cut.FindAll("tfoot tr")).ToList();
		Assert.Equal(s_items.Count + 1, rows.Count);
		Assert.All(rows, row =>
		{
			var cells = row.QuerySelectorAll("td");
			Assert.Equal(4, cells.Length);
			Assert.Contains("hx-grid-multiselect-cell", cells[0].ClassList);
			Assert.Equal(cells[2], Assert.Single(row.QuerySelectorAll("td.hx-grid-stacked-title")));
			Assert.Equal("Name", cells[2].GetAttribute("data-cell"));
		});
		Assert.All(cut.FindAll("tbody tr"), row => Assert.Contains("hx-context-menu-grid-column", row.QuerySelectorAll("td")[1].ClassList));
	}

	[Fact]
	public void HxGrid_Stacked_PlaceholderTitleCellIsTheFirstDataColumn()
	{
		// Arrange — the data provider never completes, so the placeholders stay rendered
		var dataProviderTcs = new TaskCompletionSource<GridDataProviderResult<TestItem>>();

		// Act
		var cut = Render<HxGrid<TestItem>>(parameters => parameters
			.Add(p => p.DataProvider, request => dataProviderTcs.Task)
			.Add(p => p.Stacked, GridStacked.Always)
			.Add(p => p.MultiSelectionEnabled, true)
			.Add(p => p.PlaceholdersRowCount, 2)
			.Add<HxContextMenuGridColumn<TestItem>>(p => p.Columns, column => column
				.Add(c => c.Order, -1)
				.Add(c => c.ChildContent, item => "menu"))
			.Add<HxGridColumn<TestItem>>(p => p.Columns, column => column
				.Add(c => c.HeaderText, "Name")
				.Add(c => c.ItemTextSelector, item => item.Name)));

		// Assert
		var rows = cut.FindAll("tbody tr");
		Assert.Equal(2, rows.Count);
		Assert.All(rows, row =>
		{
			var titleCell = Assert.Single(row.QuerySelectorAll("td.hx-grid-stacked-title"));
			Assert.Equal("Name", titleCell.GetAttribute("data-cell"));
		});
	}

	[Fact]
	public async Task HxGrid_Stacked_EmptyValueCellHasNoContent()
	{
		// Arrange — the stacked infinite scroll CSS keeps the value line of empty cells (td:empty), the cell must be really empty
		var cut = RenderGrid(
			parameters => parameters
				.Add(p => p.Stacked, GridStacked.BelowMedium)
				.Add(p => p.ContentNavigationMode, GridContentNavigationMode.InfiniteScroll),
			ageColumn: column => column
				.Add(c => c.HeaderText, "Age")
				.Add(c => c.ItemTextSelector, item => (item.Id == 1) ? null : (item.Id == 2) ? String.Empty : item.Age.ToString()));

		// Act
		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert
		var ageCells = cut.FindAll("tbody td[data-cell=Age]");
		Assert.Equal(s_items.Count, ageCells.Count);
		Assert.Equal("", ageCells[0].InnerHtml);
		Assert.Equal(0, ageCells[0].ChildNodes.Count(node => node.TextContent.Length > 0));
		Assert.Equal("", ageCells[1].InnerHtml);
		Assert.Equal("23", ageCells[2].TextContent);
	}

	[Fact]
	public async Task HxGrid_Stacked_ItemTemplateRenderingNothing_CellHasNoContent()
	{
		// Arrange — the stacked infinite scroll CSS keeps the value line of the plain text cells (td without elements),
		// an ItemTemplate rendering nothing must produce an empty cell as well
		var cut = RenderGrid(
			parameters => parameters
				.Add(p => p.Stacked, GridStacked.BelowMedium)
				.Add(p => p.ContentNavigationMode, GridContentNavigationMode.InfiniteScroll),
			ageColumn: column => column
				.Add(c => c.HeaderText, "Age")
				.Add(c => c.ItemTemplate, item => builder =>
				{
					if (item.Id != 1)
					{
						builder.OpenElement(0, "strong");
						builder.AddContent(1, item.Age);
						builder.CloseElement();
					}
				}));

		// Act
		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert
		var ageCells = cut.FindAll("tbody td[data-cell=Age]");
		Assert.Equal(s_items.Count, ageCells.Count);
		Assert.Empty(ageCells[0].ChildNodes);
		Assert.Single(ageCells[1].Children);
	}

	[Fact]
	public async Task HxGrid_Stacked_FooterCellsHaveDataCell()
	{
		// Arrange
		var cut = RenderGrid(
			parameters => parameters
				.Add(p => p.Stacked, GridStacked.Always)
				.Add(p => p.MultiSelectionEnabled, true),
			nameColumn: column => column
				.Add(c => c.HeaderText, "Name")
				.Add(c => c.ItemTextSelector, item => item.Name)
				.Add(c => c.FooterText, "Total"),
			ageColumn: column => column
				.Add(c => c.HeaderText, "Age")
				.Add(c => c.ItemTextSelector, item => item.Age.ToString())
				.Add(c => c.FooterText, "66"));

		// Act
		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert
		var footerCells = cut.FindAll("tfoot tr td");
		Assert.Equal([null, "Name", "Age"], footerCells.Select(cell => cell.GetAttribute("data-cell")));
		Assert.Contains("hx-grid-multiselect-cell", footerCells[0].ClassList); // the next cell is the title of the stacked footer row
		Assert.Equal("", footerCells[0].InnerHtml); // empty footer cells are hidden in the stacked layout (:empty)
		Assert.Equal("Total", footerCells[1].TextContent);
	}

	[Fact]
	public async Task HxGrid_NotStackedMultiSelect_FooterWithoutTemplates_IsNotRendered()
	{
		// Arrange — the CSS class of the (empty) multi-select footer cell must not make the grid render the footer
		var cut = RenderGrid(parameters => parameters.Add(p => p.MultiSelectionEnabled, true));

		// Act
		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert
		Assert.Empty(cut.FindAll("tfoot"));
	}

	[Fact]
	public async Task HxGrid_Stacked_EmptyData_RendersEmptyDataRowWithoutDataCell()
	{
		// Arrange
		var cut = RenderGrid(
			parameters => parameters.Add(p => p.Stacked, GridStacked.Always),
			dataProvider: request => Task.FromResult(new GridDataProviderResult<TestItem> { Data = [], TotalCount = 0 }));

		// Act
		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert
		var emptyDataCell = cut.Find("tr.hx-grid-empty-data-row td");
		Assert.Null(emptyDataCell.GetAttribute("data-cell"));
		Assert.Equal("2", emptyDataCell.GetAttribute("colspan"));
	}

	[Fact]
	public void HxGrid_Stacked_LoadingDataRow_HasCssClassAndNoDataCell()
	{
		// Arrange — no placeholders, the loading data row is rendered while the data provider is in progress
		var dataProviderTcs = new TaskCompletionSource<GridDataProviderResult<TestItem>>();

		// Act
		var cut = RenderGrid(
			parameters => parameters
				.Add(p => p.Stacked, GridStacked.Always)
				.Add(p => p.PlaceholdersRowCount, 0),
			dataProvider: request => dataProviderTcs.Task);

		// Assert
		var loadingDataCell = cut.Find("tr.hx-grid-loading-data-row td");
		Assert.Null(loadingDataCell.GetAttribute("data-cell"));
	}

	[Fact]
	public async Task HxGrid_StackedBelowBreakpoint_InfiniteScroll_RowsHaveFixedHeight()
	{
		// Arrange — above the breakpoint, the rows are rendered as a regular table with the fixed row height for the virtualization
		// (in the stacked layout, the CSS overrides the height)
		var cut = RenderGrid(parameters => parameters
			.Add(p => p.Stacked, GridStacked.BelowMedium)
			.Add(p => p.ContentNavigationMode, GridContentNavigationMode.InfiniteScroll)
			.Add(p => p.ItemRowHeight, 41f));

		// Act
		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert
		var rows = cut.FindAll("tbody tr[role=button]");
		Assert.Equal(s_items.Count, rows.Count);
		Assert.All(rows, row => Assert.Equal("41px", row.GetAttribute("height")));
		Assert.Equal(s_items.Count, cut.FindAll("tbody td[data-cell=Name]").Count);
		Assert.Contains("hx-grid-infinite-scroll", cut.Find("table").ClassList);
	}

	[Fact]
	public async Task HxGrid_NotStacked_InfiniteScroll_RowsHaveFixedHeight()
	{
		// Arrange
		var cut = RenderGrid(parameters => parameters
			.Add(p => p.ContentNavigationMode, GridContentNavigationMode.InfiniteScroll)
			.Add(p => p.ItemRowHeight, 40.5f));

		// Act
		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert
		var rows = cut.FindAll("tbody tr[role=button]");
		Assert.Equal(s_items.Count, rows.Count);
		Assert.All(rows, row => Assert.Equal("40.5px", row.GetAttribute("height")));
	}

	private IRenderedComponent<HxGrid<TestItem>> RenderGrid(
		Action<ComponentParameterCollectionBuilder<HxGrid<TestItem>>> parameters,
		Action<ComponentParameterCollectionBuilder<HxGridColumn<TestItem>>> nameColumn = null,
		Action<ComponentParameterCollectionBuilder<HxGridColumn<TestItem>>> ageColumn = null,
		GridDataProviderDelegate<TestItem> dataProvider = null)
	{
		nameColumn ??= column => column
			.Add(c => c.HeaderText, "Name")
			.Add(c => c.ItemTextSelector, item => item.Name);

		ageColumn ??= column => column
			.Add(c => c.HeaderText, "Age")
			.Add(c => c.ItemTextSelector, item => item.Age.ToString());

		return Render<HxGrid<TestItem>>(builder =>
		{
			builder
				.Add(p => p.DataProvider, dataProvider ?? GetDataProvider())
				.Add(p => p.Columns, nameColumn) // multiple render fragments for the same parameter are concatenated
				.Add(p => p.Columns, ageColumn);
			parameters(builder);
		});
	}

	private static GridDataProviderDelegate<TestItem> GetDataProvider() => request => Task.FromResult(request.ApplyTo(s_items));
}
