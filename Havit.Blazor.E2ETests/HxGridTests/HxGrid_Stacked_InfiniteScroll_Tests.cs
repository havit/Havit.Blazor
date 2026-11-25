namespace Havit.Blazor.E2ETests.HxGridTests;

/// <summary>
/// Browser validation of the stacked layout (<c>Stacked="GridStacked.BelowMedium"</c>) with <c>GridContentNavigationMode.InfiniteScroll</c>:
/// the row geometry the Virtualize component relies on (a single average item size) and the behavior when the grid crosses the breakpoint.
/// </summary>
public class HxGrid_Stacked_InfiniteScroll_Tests : TestAppTestBase
{
	private const int ItemsCount = 500; // HxGrid_Stacked_InfiniteScroll_Item.ItemsCount
	private const int StackedWidth = 400; // below the "medium" container breakpoint (768px)
	private const int TableWidth = 1000;

	// The virtual height of the grid (spacers + rendered rows) corresponds to the item count with the measured row height (within 1 %).
	// Virtualize starts with ItemRowHeight as the estimated item size and replaces it with the measured average
	// whenever a spacer becomes visible; with uniform row heights, the average is exact.
	private static readonly string s_spacersMatchRowHeightPredicate = $"(Math.abs((m.spacersHeight + m.dataRowsHeight) - ({ItemsCount} * m.averageRowHeight)) <= {ItemsCount} * m.averageRowHeight * 0.01)";

	// Measures the grid in the [data-testid=grid-wrapper] element.
	// Data rows are the rows with role=button (selection enabled), the Virtualize spacers are the rows without cells.
	// The row height is measured without the top border (the first row after the leading spacer intentionally has no separator).
	private const string MeasureGridScript = """
		() => {
			const container = document.querySelector("[data-testid='grid-wrapper'] .hx-grid-container");
			const tbody = container.querySelector("table > tbody");
			const rows = [...tbody.rows];
			const spacers = rows.filter(row => row.cells.length === 0);
			const dataRows = rows.filter(row => row.getAttribute("role") === "button");
			const containerRect = container.getBoundingClientRect();
			const viewportTop = Math.max(containerRect.top, 0);
			const viewportBottom = Math.min(containerRect.bottom, window.innerHeight);
			const rowHeights = dataRows.map(row => row.getBoundingClientRect().height - parseFloat(getComputedStyle(row).borderTopWidth));
			const visibleRows = dataRows.filter(row => {
				const rect = row.getBoundingClientRect();
				return (rect.bottom > viewportTop) && (rect.top < viewportBottom);
			});
			return {
				rowDisplay: dataRows.length ? getComputedStyle(dataRows[0]).display : null,
				dataRowCount: dataRows.length,
				minRowHeight: Math.min(...rowHeights),
				maxRowHeight: Math.max(...rowHeights),
				averageRowHeight: dataRows.length ? (dataRows.reduce((sum, row) => sum + row.getBoundingClientRect().height, 0) / dataRows.length) : 0,
				spacersHeight: spacers.reduce((sum, spacer) => sum + spacer.getBoundingClientRect().height, 0),
				dataRowsHeight: dataRows.reduce((sum, row) => sum + row.getBoundingClientRect().height, 0),
				// the rendered data rows cover the visible part of the grid (no blank region)
				viewportCovered: (dataRows.length > 0)
					&& (dataRows[0].getBoundingClientRect().top <= viewportTop + 1)
					&& (dataRows[dataRows.length - 1].getBoundingClientRect().bottom >= viewportBottom - 1),
				firstVisibleItem: visibleRows.length ? parseInt(visibleRows[0].cells[0].textContent.replace("Item ", ""), 10) : null,
				containerOverflowY: getComputedStyle(container).overflowY,
				scrollTop: container.scrollTop
			};
		}
		""";

	[Fact]
	public async Task HxGrid_StackedInfiniteScroll_RowsHaveUniformHeight_SpacersMatchTheRowHeight()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxGrid_Stacked_InfiniteScroll");

		await WaitForGridAsync("m => (m.rowDisplay === 'block') && m.viewportCovered");

		// Act - scroll to the end of the rendered rows: the trailing spacer becomes visible and Virtualize replaces
		// the estimated item size (ItemRowHeight, 41 px) with the measured height of the stacked rows
		await Page.EvaluateAsync("() => { const c = document.querySelector(\"[data-testid='grid-wrapper'] .hx-grid-container\"); c.scrollTop = [...c.querySelectorAll('tbody > tr[role=button]')].reduce((sum, row) => sum + row.offsetHeight, 0); }");

		// Assert - the spacers represent the non-rendered items with the height of the (uniform) stacked rows
		await WaitForGridAsync($"m => (m.rowDisplay === 'block') && m.viewportCovered && {s_spacersMatchRowHeightPredicate}");

		// Assert
		var metrics = await MeasureGridAsync();
		Assert.Equal("block", metrics.RowDisplay);
		Assert.True(metrics.MinRowHeight > 100, $"The stacked rows are expected to be much taller than a table row, the measured height is {metrics.MinRowHeight}px.");
		// uniform height - including the items without an email (every third item) and the rows with a context menu
		Assert.True(metrics.MaxRowHeight - metrics.MinRowHeight < 0.5, $"The stacked rows have different heights ({metrics.MinRowHeight}px - {metrics.MaxRowHeight}px).");
		Assert.True(metrics.DataRowCount < ItemsCount / 2, $"The grid is expected to be virtualized, {metrics.DataRowCount} rows are rendered.");
	}

	[Fact]
	public async Task HxGrid_StackedInfiniteScroll_ScrolledGrid_RendersVisibleRowsWhenResizedAcrossBreakpoint()
	{
		// Arrange - a stacked grid scrolled to the middle of the items
		await NavigateToTestAppAsync("/HxGrid_Stacked_InfiniteScroll");
		await WaitForGridAsync("m => (m.rowDisplay === 'block') && m.viewportCovered");
		await Page.EvaluateAsync("() => { const c = document.querySelector(\"[data-testid='grid-wrapper'] .hx-grid-container\"); c.scrollTop = c.scrollHeight / 2; }");
		await WaitForGridAsync("m => (m.rowDisplay === 'block') && m.viewportCovered && (m.firstVisibleItem > 10)");
		var stackedMetrics = await MeasureGridAsync();

		// Act - widen the grid above the breakpoint (table layout)
		await SetGridWidthAsync(TableWidth);

		// Assert - the rows of the current scroll position are rendered (no blank region) and the grid stays scrolled.
		// (Not asserted: the spacer sizes. Virtualize measures the new row height only when a spacer becomes visible,
		// so the visible items can shift once - the same as with any change of the row height.)
		await WaitForGridAsync("m => (m.rowDisplay === 'table-row') && m.viewportCovered && (m.firstVisibleItem > 10)");
		var tableMetrics = await MeasureGridAsync();
		Assert.True(tableMetrics.ScrollTop > 0);
		Assert.True(tableMetrics.MaxRowHeight - tableMetrics.MinRowHeight < 0.5, $"The table rows have different heights ({tableMetrics.MinRowHeight}px - {tableMetrics.MaxRowHeight}px).");
		Assert.True(tableMetrics.MaxRowHeight < stackedMetrics.MinRowHeight);

		// Act - narrow the grid below the breakpoint again (stacked layout)
		await SetGridWidthAsync(StackedWidth);

		// Assert
		await WaitForGridAsync("m => (m.rowDisplay === 'block') && m.viewportCovered && (m.firstVisibleItem > 10)");
		var restackedMetrics = await MeasureGridAsync();
		Assert.True(restackedMetrics.ScrollTop > 0);
		Assert.True(restackedMetrics.MaxRowHeight - restackedMetrics.MinRowHeight < 0.5, $"The stacked rows have different heights ({restackedMetrics.MinRowHeight}px - {restackedMetrics.MaxRowHeight}px).");
	}

	[Fact]
	public async Task HxGrid_StackedInfiniteScroll_PageScrolling_IsVirtualized()
	{
		// Arrange - a stacked grid without a scrollable container (the page viewport is the scroll root of Virtualize)
		await NavigateToTestAppAsync("/HxGrid_Stacked_InfiniteScroll_PageScroll");
		await WaitForGridAsync("m => (m.rowDisplay === 'block') && (m.dataRowCount > 0)");

		// Act
		await Page.EvaluateAsync("() => window.scrollTo(0, document.documentElement.scrollHeight / 2)");

		// Assert - the rows in the viewport are rendered, but not all the items (the container is not a scroll container)
		await WaitForGridAsync("m => (m.rowDisplay === 'block') && m.viewportCovered && (m.firstVisibleItem > 10)");
		var metrics = await MeasureGridAsync();
		Assert.Equal("visible", metrics.ContainerOverflowY);
		Assert.True(metrics.DataRowCount < ItemsCount / 2, $"The grid is expected to be virtualized, {metrics.DataRowCount} rows are rendered.");
	}

	private async Task SetGridWidthAsync(int width)
	{
		await Page.EvaluateAsync($"() => document.querySelector(\"[data-testid='grid-wrapper']\").style.width = '{width}px'");
	}

	private async Task WaitForGridAsync(string predicate)
	{
		try
		{
			await Page.WaitForFunctionAsync($"() => ({predicate})(({MeasureGridScript})())", null, new() { Timeout = 10_000, PollingInterval = 100 });
		}
		catch (TimeoutException exception)
		{
			string metrics = await Page.EvaluateAsync<string>($"() => JSON.stringify(({MeasureGridScript})())");
			throw new TimeoutException($"The grid did not satisfy {predicate}, last measured: {metrics}", exception);
		}
	}

	private async Task<GridMetrics> MeasureGridAsync()
	{
		return await Page.EvaluateAsync<GridMetrics>(MeasureGridScript);
	}

	private record GridMetrics
	{
		public string RowDisplay { get; init; }
		public int DataRowCount { get; init; }
		public double MinRowHeight { get; init; }
		public double MaxRowHeight { get; init; }
		public double AverageRowHeight { get; init; }
		public double SpacersHeight { get; init; }
		public double DataRowsHeight { get; init; }
		public bool ViewportCovered { get; init; }
		public int? FirstVisibleItem { get; init; }
		public string ContainerOverflowY { get; init; }
		public double ScrollTop { get; init; }
	}
}
