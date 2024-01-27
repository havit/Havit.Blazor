namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Displays the content of the component as "in progress".<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxProgressIndicator">https://havit.blazor.eu/components/HxProgressIndicator</see>
/// </summary>
public partial class HxProgressIndicator : IDisposable
{
	/// <summary>
	/// Application-wide defaults for the <see cref="HxProgressIndicator"/>.
	/// </summary>
	public static ProgressIndicatorSettings Defaults { get; set; }

	static HxProgressIndicator()
	{
		Defaults = new ProgressIndicatorSettings()
		{
			Delay = 300,
		};
	}

	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use a separate set of defaults).
	/// </summary>
	protected virtual ProgressIndicatorSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public ProgressIndicatorSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in component descendants (by returning a derived settings class).
	/// </remarks>
	protected virtual ProgressIndicatorSettings GetSettings() => Settings;

	/// <summary>
	/// Indicates whether the content should be displayed as "in progress".
	/// </summary>
	[Parameter] public bool InProgress { get; set; }

	/// <summary>
	/// Debounce delay in milliseconds. The default is <c>300 ms</c>.
	/// </summary>
	[Parameter] public int? Delay { get; set; }
	protected int DelayEffective => Delay ?? GetSettings()?.Delay ?? GetDefaults()?.Delay ?? throw new InvalidOperationException(nameof(Delay) + " default for " + nameof(HxProgressIndicator) + " has to be set.");

	/// <summary>
	/// Wrapped content.
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	protected EventCallback<bool> ProgressIndicatorVisibleChanged { get; set; }

	private bool _progressIndicatorVisible;
	private System.Timers.Timer _timer;

	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync();
		bool shouldBeProgressIndicatorVisible = InProgress;

		if (shouldBeProgressIndicatorVisible && !_progressIndicatorVisible)
		{
			// start showing the progress indicator (when not already started)
			if (DelayEffective == 0)
			{
				await StartInProgressAsync();
			}
			else
			{
				StartInProgressWithDelay();
			}
		}
		else if (!shouldBeProgressIndicatorVisible)
		{
			_timer?.Stop();
			if (_progressIndicatorVisible)
			{
				// hide the progress indicator if visible					
				_progressIndicatorVisible = false;
				await ProgressIndicatorVisibleChanged.InvokeAsync(_progressIndicatorVisible);
			}
		}
	}

	private async Task StartInProgressAsync()
	{
		_progressIndicatorVisible = true;
		await ProgressIndicatorVisibleChanged.InvokeAsync(_progressIndicatorVisible);
	}

	private void StartInProgressWithDelay()
	{
		if (_timer == null)
		{
			_timer = new System.Timers.Timer();
			_timer.AutoReset = false; // run once
			_timer.Elapsed += (_, _) =>
			{
				_ = InvokeAsync(async () =>
				{
					// condition InProgress:
					// Critical for times around TimerDelay - the timer can rise elapsed and InvokeAsync "plan" the action to run.
					// But before it is run, OnParametersSetAsync can be called and switch off the progress bar.
					if (InProgress)
					{
						await StartInProgressAsync();
						StateHasChanged();
					}
				});
			};
		}
		_timer.Interval = DelayEffective;
		_timer.Start(); // does nothing when already started (that's what we need here)
	}

	public void Dispose()
	{
		Dispose(true);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			_timer?.Dispose();
		}
	}
}
