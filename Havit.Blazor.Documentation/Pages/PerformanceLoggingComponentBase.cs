namespace Havit.Blazor.Documentation.Pages;

public class PerformanceLoggingComponentBase : ComponentBase
{
	[Inject] protected ILogger<PerformanceLoggingComponentBase> Logger { get; set; }

	private TimeSpan? _timeStart;
	private TimeSpan? _timeLast;

	public PerformanceLoggingComponentBase()
	{
		_timeStart = DateTime.Now.TimeOfDay;
		_timeLast = _timeStart;
	}

	protected override void OnParametersSet()
	{
		LogTimeElapsed("OnParametersSet");
	}

	protected override void OnInitialized()
	{
		LogTimeElapsed("OnInitialized");
	}

	protected override void OnAfterRender(bool firstRender)
	{
		base.OnAfterRender(firstRender);

		LogTimeElapsed("OnAfterRender");
		_timeStart = null;
		_timeLast = null;
	}

	protected void LogTimeElapsed(string message)
	{
		TimeSpan now = DateTime.Now.TimeOfDay;
		_timeStart ??= now;
		_timeLast ??= now;
		Logger.LogDebug($"{GetType().Name}({GetHashCode()})_{message}: {(now - _timeStart.Value).TotalMilliseconds} ms (+ {(now - _timeLast.Value).TotalMilliseconds} ms)");
		_timeLast = now;
	}
}
