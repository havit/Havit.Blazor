namespace Havit.Blazor.Components.Web;

public class Debouncer : IDisposable
{
	private CancellationTokenSource debounceCancellationTokenSource = null;
	private bool disposed;

	public async Task DebounceAsync(Func<CancellationToken, Task> actionAsync, int millisecondsDelay)
	{
		try
		{
			if (debounceCancellationTokenSource != null)
			{
				debounceCancellationTokenSource?.Cancel();
				debounceCancellationTokenSource?.Dispose();
			}

			debounceCancellationTokenSource = new CancellationTokenSource();

			await Task.Delay(millisecondsDelay, debounceCancellationTokenSource.Token);

			debounceCancellationTokenSource.Token.ThrowIfCancellationRequested();

			await actionAsync(debounceCancellationTokenSource.Token);
		}
		catch (TaskCanceledException)
		{
			// NOOP
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposed)
		{
			if (disposing)
			{
				if (debounceCancellationTokenSource != null)
				{
					debounceCancellationTokenSource?.Cancel();
					debounceCancellationTokenSource?.Dispose();
				}
			}

			disposed = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}
}
