using System.Threading;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Displays the content of the component as "in progress".
	/// </summary>
	public partial class HxProgressIndicator
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
		/// Enables overriding defaults in descandants (use separate set of defaults).
		/// </summary>
		protected virtual ProgressIndicatorSettings GetDefaults() => Defaults;

		/// <summary>
		/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overriden by individual parameters).
		/// </summary>
		[Parameter] public ProgressIndicatorSettings Settings { get; set; }

		/// <summary>
		/// Returns optional set of component settings.
		/// </summary>
		/// <remarks>
		/// Simmilar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in components descandants (by returning a derived settings class).
		/// </remarks>
		protected virtual ProgressIndicatorSettings GetSettings() => this.Settings;

		/// <summary>
		/// Indicates whether the content should be displayed as "in progress".
		/// </summary>
		[Parameter] public bool InProgress { get; set; }

		/// <summary>
		/// Debounce delay in miliseconds. Default is <c>300 ms</c>.
		/// </summary>
		[Parameter] public int? Delay { get; set; }
		protected int DelayEffective => this.Delay ?? GetSettings()?.Delay ?? GetDefaults()?.Delay ?? throw new InvalidOperationException(nameof(Delay) + " default for " + nameof(HxProgressIndicator) + " has to be set.");

		/// <summary>
		/// Wrapped content.
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }

		protected EventCallback<bool> ProgressIndicatorVisibleChanged { get; set; }  // TODO Needed?

		private bool progressIndicatorVisible;
		private CancellationTokenSource delayCancellationTokenSource;

		protected override async Task OnParametersSetAsync()
		{
			await base.OnParametersSetAsync();

			bool shouldBeProgressIndicatorVisible = InProgress;

			if (shouldBeProgressIndicatorVisible && !progressIndicatorVisible)
			{
				// start showing progress indicator (when not already started)
				StartInProgressWithDelay();
			}
			else if (!shouldBeProgressIndicatorVisible)
			{
				if (progressIndicatorVisible)
				{
					// hide progress indicator if visible
					progressIndicatorVisible = false;
					await ProgressIndicatorVisibleChanged.InvokeAsync(progressIndicatorVisible);
				}

				if (delayCancellationTokenSource != null)
				{
					// cancel showing progress indicator
					delayCancellationTokenSource.Cancel();
					delayCancellationTokenSource = null;
				}
			}
		}

		private void StartInProgressWithDelay()
		{
			if (delayCancellationTokenSource == null)
			{
				delayCancellationTokenSource = new CancellationTokenSource();
				CancellationToken cancellationToken = delayCancellationTokenSource.Token;

				Task.Run(async () =>
				{
					try
					{
						await Task.Delay(DelayEffective, cancellationToken);
					}
					catch (TaskCanceledException)
					{
						// NOOP
					}

					if (!cancellationToken.IsCancellationRequested)
					{
						await InvokeAsync(async () =>
						{
							progressIndicatorVisible = true;
							await ProgressIndicatorVisibleChanged.InvokeAsync(progressIndicatorVisible);
							StateHasChanged();
						});
					}
				});
			}
		}

	}
}
