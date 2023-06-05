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
	/// Enables overriding defaults in descendants (use separate set of defaults).
	/// </summary>
	protected virtual ProgressIndicatorSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public ProgressIndicatorSettings Settings { get; set; }

	/// <summary>
	/// Returns optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in components descendants (by returning a derived settings class).
	/// </remarks>
	protected virtual ProgressIndicatorSettings GetSettings() => this.Settings;

	/// <summary>
	/// Indicates whether the content should be displayed as "in progress".
	/// </summary>
	[Parameter] public bool InProgress { get; set; }

	/// <summary>
	/// Debounce delay in milliseconds. Default is <c>300 ms</c>.
	/// </summary>
	[Parameter] public int? Delay { get; set; }
	protected int DelayEffective => this.Delay ?? GetSettings()?.Delay ?? GetDefaults()?.Delay ?? throw new InvalidOperationException(nameof(Delay) + " default for " + nameof(HxProgressIndicator) + " has to be set.");

	/// <summary>
	/// Wrapped content.
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	protected EventCallback<bool> ProgressIndicatorVisibleChanged { get; set; }

	private bool progressIndicatorVisible;
	private System.Timers.Timer timer;

	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync();
		bool shouldBeProgressIndicatorVisible = InProgress;

		if (shouldBeProgressIndicatorVisible && !progressIndicatorVisible)
		{
			// start showing progress indicator (when not already started)
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
			timer?.Stop();
			if (progressIndicatorVisible)
			{
				// hide progress indicator if visible					
				progressIndicatorVisible = false;
				await ProgressIndicatorVisibleChanged.InvokeAsync(progressIndicatorVisible);
			}
		}
	}

	private async Task StartInProgressAsync()
	{
		progressIndicatorVisible = true;
		await ProgressIndicatorVisibleChanged.InvokeAsync(progressIndicatorVisible);
	}

	private void StartInProgressWithDelay()
	{
		if (timer == null)
		{
			timer = new System.Timers.Timer();
			timer.AutoReset = false; // run once
			timer.Elapsed += (_, _) =>
			{
				_ = InvokeAsync(async () =>
				{
					// condition InProgress:
					// Critical for times around TimerDelay - timer can rise elapsed and InvokeAsync "plan" the action to run.
					// But before it is run, OnParametersSetAsync can be called and switch of the progress bar.
					if (InProgress)
					{
						await StartInProgressAsync();
						StateHasChanged();
					}
				});
			};
		}
		timer.Interval = DelayEffective;
		timer.Start(); // does nothing when already started (that's what we need here)
	}

	public void Dispose()
	{
		Dispose(true);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			timer?.Dispose();
		}
	}
}
