namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Grids;

public class HxGrid_ProgressIndicator_Tests : BunitTestBase
{
	private record TestItem(int Id, string Name);

	[Fact]
	public async Task HxGrid_ProgressIndicator_ShowsAfterDelayAndHidesWhenDataLoaded()
	{
		// Arrange
		var items = Enumerable.Range(1, 5).Select(i => new TestItem(i, $"Item {i}")).ToList();
		TaskCompletionSource dataProviderTCS = null;

		GridDataProviderDelegate<TestItem> dataProvider = async (GridDataProviderRequest<TestItem> request) =>
		{
			if (dataProviderTCS != null)
			{
				await dataProviderTCS.Task;
			}
			return request.ApplyTo(items);
		};

		var cut = Render<HxGrid<TestItem>>(parameters => parameters
			.Add(p => p.DataProvider, dataProvider)
			.Add(p => p.ProgressIndicatorDelay, 50)
			.Add<HxGridColumn<TestItem>>(p => p.Columns, column => column
				.Add(c => c.HeaderText, "Name")
				.Add(c => c.ItemTextSelector, item => item.Name)));
		await cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Act
		dataProviderTCS = new TaskCompletionSource();
		var refreshTask = cut.InvokeAsync(() => cut.Instance.RefreshDataAsync());

		// Assert
		cut.WaitForAssertion(() => Assert.NotNull(cut.Find(".hx-grid-container.hx-grid-progress-indicator")), TimeSpan.FromSeconds(5));

		dataProviderTCS.SetResult();
		await refreshTask;
		Assert.Empty(cut.FindAll(".hx-grid-progress-indicator"));
	}

	[Fact]
	public async Task HxGrid_ProgressIndicator_DisposingGridWithPendingDelayTimerDoesNotThrow()
	{
		// Arrange
		var dataProviderTCS = new TaskCompletionSource();

		GridDataProviderDelegate<TestItem> dataProvider = async (GridDataProviderRequest<TestItem> request) =>
		{
			await dataProviderTCS.Task;
			return request.ApplyTo(Enumerable.Empty<TestItem>());
		};

		Render<HxGrid<TestItem>>(parameters => parameters
			.Add(p => p.DataProvider, dataProvider)
			.Add(p => p.ProgressIndicatorDelay, 20)
			.Add<HxGridColumn<TestItem>>(p => p.Columns, column => column
				.Add(c => c.HeaderText, "Name")
				.Add(c => c.ItemTextSelector, item => item.Name)));

		// Act
		await DisposeComponentsAsync();
		await Task.Delay(100, Xunit.TestContext.Current.CancellationToken); // let the (already disposed) timer elapse, if it was not stopped
		dataProviderTCS.SetResult();

		// Assert
		// the main assertion is that no unhandled exception was thrown from the timer handler (#1817)
	}
}
