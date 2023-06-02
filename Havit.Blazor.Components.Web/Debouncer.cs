namespace Havit.Blazor.Components.Web;

/// <summary>
/// Debouncer helps you to debounce asynchronous actions.
/// You can use it in your callbacks to prevent multiple calls of the same action in a short period of time.
/// </summary>
public class Debouncer : IDisposable
{
	private CancellationTokenSource debounceCancellationTokenSource = null;
	private bool disposed;

	/// <summary>
	/// Starts the debouncing.
	/// </summary>
	/// <param name="millisecondsDelay">debouncing delay</param>
	/// <param name="actionAsync">work to be executed (<see cref="CancellationToken"/> gets canceled if the method is called again)</param>
	public async Task DebounceAsync(int millisecondsDelay, Func<CancellationToken, Task> actionAsync)
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
