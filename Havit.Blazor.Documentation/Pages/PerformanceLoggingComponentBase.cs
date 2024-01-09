namespace Havit.Blazor.Documentation.Pages;

public class PerformanceLoggingComponentBase : ComponentBase
{
	[Inject] protected ILogger<PerformanceLoggingComponentBase> Logger { get; set; }

	private TimeSpan? timeStart;
	private TimeSpan? timeLast;

	public PerformanceLoggingComponentBase()
	{
		timeStart = DateTime.Now.TimeOfDay;
		timeLast = timeStart;
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
		timeStart = null;
		timeLast = null;
	}

	protected void LogTimeElapsed(string message)
	{
		TimeSpan now = DateTime.Now.TimeOfDay;
		timeStart ??= now;
		timeLast ??= now;
		Logger.LogDebug($"{this.GetType().Name}({this.GetHashCode()})_{message}: {(now - timeStart.Value).TotalMilliseconds} ms (+ {(now - timeLast.Value).TotalMilliseconds} ms)");
		timeLast = now;
	}
}
