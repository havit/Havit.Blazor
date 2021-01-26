using Havit.Blazor.Components.Web.Infrastructure;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	// In Progress 
	public partial class HxProgressOverlay : ICascadeProgressComponent
	{
		private bool progressIndicatorVisible;
		private CancellationTokenSource delayCancellationTokenSource;

		[CascadingParameter] public ProgressState ProgressState { get; set; }

		[Parameter] public bool? InProgress { get; set; }

		[Parameter] public int Delay { get; set; } = 300;

		[Parameter] public RenderFragment ChildContent { get; set; }

		protected EventCallback<bool> ProgressIndicatorVisibleChanged { get; set; }

		protected override async Task OnParametersSetAsync()
		{
			await base.OnParametersSetAsync();

			bool shouldBeProgressIndicatorVisible = CascadeProgressComponent.InProgressEffective(this);

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
						await Task.Delay(Delay, cancellationToken);
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
