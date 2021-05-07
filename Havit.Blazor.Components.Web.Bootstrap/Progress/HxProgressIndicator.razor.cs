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
	public partial class HxProgressIndicator
	{
		private bool progressIndicatorVisible;
		private CancellationTokenSource delayCancellationTokenSource;

		/// <summary>
		/// Default debounce delay in miliseconds to be used when <see cref="Delay"/> not set.
		/// Default DefaultDelay is 300 ms.
		/// </summary>
		public static int DefaultDelay { get; set; } = 300;

		[Parameter] public bool InProgress { get; set; }

		/// <summary>
		/// Debounce delay in miliseconds. If not set, uses the <see cref="DefaultDelay"/>.
		/// </summary>
		[Parameter] public int? Delay { get; set; }

		[Parameter] public RenderFragment ChildContent { get; set; }

		protected EventCallback<bool> ProgressIndicatorVisibleChanged { get; set; }

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
						await Task.Delay(Delay ?? HxProgressIndicator.DefaultDelay, cancellationToken);
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
