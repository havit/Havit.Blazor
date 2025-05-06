using Microsoft.Extensions.DependencyInjection;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Grids;

[TestClass]
public class HxGrid_Cancellation_Tests : BunitTestBase
{
	[TestMethod]
	public async Task HxGrid_Cancellation_ShouldRequestCancellationWhenDisposed()
	{
		// Arrange
		var items = Enumerable.Range(1, 100).Select(i => new { Id = i, Name = $"Item {i}" }).ToList();
		object selectedItem = items[7];

		var dataProviderTCS = new TaskCompletionSource<bool>();
		var disposedTCS = new TaskCompletionSource();

		GridDataProviderDelegate<object> dataProvider = async (GridDataProviderRequest<object> request) =>
		{
			try
			{
				await disposedTCS.Task; // wait for the component to be disposed
				dataProviderTCS.SetResult(request.CancellationToken.IsCancellationRequested);
				return request.ApplyTo(items);
			}
			catch (Exception ex)
			{
				// just to propagate eventual exception to the test
				dataProviderTCS.SetException(ex);
				throw;
			}
		};

		var cut = RenderComponent<HxGrid<object>>(parameters => parameters
			.Add(p => p.DataProvider, dataProvider));

		// Act
		DisposeComponents();
		disposedTCS.SetResult(); // signal that the component is disposed
		var wasCancellationRequested = await dataProviderTCS.Task;

		// Assert
		Assert.IsTrue(wasCancellationRequested);
	}

	// When we dispose CancellationTokenSource in HxGrid, the CancellationToken originated from it throws ObjectDisposedException in some scenarios.
	// QuickGrid, FluentUI-DataGrid and such reference implementations do not dispose the CancellationTokenSource to avoid this.
	// We should not dispose the CancellationTokenSource in HxGrid either.
	// https://stackoverflow.com/questions/6960520/when-to-dispose-cancellationtokensource#:~:text=One%20more%20comment%20by%20Stephen,it%27s%20used%20with%20a%20timeout
	// http://web.archive.org/web/20160203062224/http://blogs.msdn.com/b/pfxteam/archive/2012/03/25/10287435.aspx (see Steve Toube's comments bellow)
	[TestMethod]
	public async Task HxGrid_Cancellation_CancellationTokenUsageInDataProviderShouldNotThrowObjectDisposedExceptionWhenHxGridGetsDisposed()
	{
		// Arrange
		var items = Enumerable.Range(1, 100).Select(i => new { Id = i, Name = $"Item {i}" }).ToList();
		object selectedItem = items[7];

		var dataProviderTCS = new TaskCompletionSource<bool>();
		var disposedTCS = new TaskCompletionSource();

		GridDataProviderDelegate<object> dataProvider = async (GridDataProviderRequest<object> request) =>
		{
			try
			{
				await disposedTCS.Task; // wait for the component to be disposed

				// simulate more sophisticated CancellationToken access (e.g. HttpClient)
				_ = request.CancellationToken.WaitHandle; // throws ObjectDisposedException if the underlying CancellationTokenSource is disposed

				dataProviderTCS.SetResult(request.CancellationToken.IsCancellationRequested);
				return request.ApplyTo(items);
			}
			catch (Exception ex)
			{
				dataProviderTCS.SetException(ex);
				throw;
			}
		};

		var cut = RenderComponent<HxGrid<object>>(parameters => parameters
			.Add(p => p.DataProvider, dataProvider));

		// Act
		DisposeComponents();
		disposedTCS.SetResult(); // signal that the component is disposed
		var wasCancellationRequested = await dataProviderTCS.Task;

		// Assert
		// the main assertion is that no ObjectDisposedException was thrown
		Assert.IsTrue(wasCancellationRequested);
	}
}
