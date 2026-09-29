using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure.Model;

namespace Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;

/// <summary>
/// Collects the telemetry items intercepted on the Application Insights track endpoint and lets a test
/// wait for specific items to arrive. A wait completes the moment the matching request is on the wire,
/// so tests do not have to approximate delivery with <c>NetworkIdle</c> or fixed delays.
/// </summary>
public sealed class AiTelemetryCollector
{
	/// <summary>
	/// Default time to wait for an item. Telemetry arrives within a second once flushed;
	/// the reserve covers a cold CDN download of the SDK on CI.
	/// </summary>
	public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(15);

	private readonly object _lock = new object();
	private readonly List<AiTelemetryItem> _items = new List<AiTelemetryItem>();
	private readonly List<Waiter> _waiters = new List<Waiter>();

	/// <summary>
	/// Snapshot of the items captured so far.
	/// </summary>
	public IReadOnlyList<AiTelemetryItem> Items
	{
		get
		{
			lock (_lock)
			{
				return _items.ToArray();
			}
		}
	}

	/// <summary>
	/// Waits until an item matching <paramref name="predicate" /> has been captured and returns it.
	/// Returns immediately when such an item is already present.
	/// </summary>
	/// <param name="description">Human readable description of the expected item, used in the timeout message.</param>
	public async Task<AiTelemetryItem> WaitForItemAsync(Func<AiTelemetryItem, bool> predicate, string description, TimeSpan? timeout = null)
	{
		var items = await WaitForItemsAsync(predicate, 1, description, timeout);
		return items[0];
	}

	/// <summary>
	/// Waits until at least <paramref name="count" /> items matching <paramref name="predicate" /> have been captured
	/// and returns all matching items.
	/// </summary>
	/// <param name="description">Human readable description of the expected items, used in the timeout message.</param>
	public async Task<IReadOnlyList<AiTelemetryItem>> WaitForItemsAsync(Func<AiTelemetryItem, bool> predicate, int count, string description, TimeSpan? timeout = null)
	{
		await WaitUntilAsync(items => items.Count(predicate) >= count, description, timeout ?? DefaultTimeout);
		return Items.Where(predicate).ToArray();
	}

	/// <summary>
	/// Adds intercepted items and wakes up the waiters whose condition is now satisfied.
	/// Called from the Playwright route handler.
	/// </summary>
	internal void Add(IEnumerable<AiTelemetryItem> items)
	{
		List<Waiter> satisfiedWaiters;

		lock (_lock)
		{
			_items.AddRange(items);

			satisfiedWaiters = _waiters.Where(waiter => waiter.Condition(_items)).ToList();
			_waiters.RemoveAll(satisfiedWaiters.Contains);
		}

		// completed outside the lock; RunContinuationsAsynchronously keeps the test continuation off the Playwright dispatcher thread
		foreach (var waiter in satisfiedWaiters)
		{
			waiter.Completion.TrySetResult();
		}
	}

	private async Task WaitUntilAsync(Func<IReadOnlyList<AiTelemetryItem>, bool> condition, string description, TimeSpan timeout)
	{
		Waiter waiter;

		lock (_lock)
		{
			if (condition(_items))
			{
				return;
			}

			waiter = new Waiter(condition);
			_waiters.Add(waiter);
		}

		var testCancellationToken = TestContext.Current.CancellationToken;
		using var timeoutCancellation = CancellationTokenSource.CreateLinkedTokenSource(testCancellationToken);
		timeoutCancellation.CancelAfter(timeout);

		try
		{
			await waiter.Completion.Task.WaitAsync(timeoutCancellation.Token);
		}
		catch (OperationCanceledException) when (!testCancellationToken.IsCancellationRequested)
		{
			lock (_lock)
			{
				_waiters.Remove(waiter);
			}

			var captured = Items;
			var capturedDescription = (captured.Count == 0)
				? "nothing"
				: string.Join(Environment.NewLine, captured.Select(item => "  - " + Describe(item)));

			throw new TimeoutException($"Telemetry '{description}' did not arrive within {timeout.TotalSeconds:0.#} s. Captured so far ({captured.Count}):{Environment.NewLine}{capturedDescription}");
		}
	}

	private static string Describe(AiTelemetryItem item)
	{
		var baseData = item.Data?.BaseData;
		var identifier = baseData?.Name
			?? baseData?.Message
			?? baseData?.Metrics?.FirstOrDefault()?.Name
			?? baseData?.Exceptions?.FirstOrDefault()?.TypeName
			?? baseData?.Url;

		return (identifier == null) ? item.BaseType ?? "(unknown)" : $"{item.BaseType} '{identifier}'";
	}

	private sealed class Waiter(Func<IReadOnlyList<AiTelemetryItem>, bool> condition)
	{
		public Func<IReadOnlyList<AiTelemetryItem>, bool> Condition { get; } = condition;
		public TaskCompletionSource Completion { get; } = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
	}
}
