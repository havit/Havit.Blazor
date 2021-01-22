using Havit.Blazor.Components.Web.Forms;
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
	public partial class HxProgress : ICascadeProgressComponent
	{
		//private bool progressIndicatorVisible;
		//private bool debounceIntervalStarted;
		//private bool shouldRender;
		//private CancellationTokenSource delayCancellationTokenSource;

		[CascadingParameter] public ProgressState ProgressState { get; set; }

		[Parameter] public bool? InProgress { get; set; }

		[Parameter] public int Delay { get; set; } = 300;

		protected EventCallback<bool> ProgressIndicatorVisibleChanged { get; set; }

		//protected override async Task OnParametersSetAsync()
		//{
		//	await base.OnParametersSetAsync();

		//	bool shouldBeProgressIndicatorVisible = CascadeProgressComponent.InProgressEffective(this);

		//	if (shouldBeProgressIndicatorVisible && !progressIndicatorVisible)
		//	{
		//		StartInProgressWithDelay();				
		//	}
		//	else if (!shouldBeProgressIndicatorVisible && progressIndicatorVisible)
		//	{
		//		progressIndicatorVisible = false;
		//		await ProgressIndicatorVisibleChanged.InvokeAsync(false);
		//	}
		//	else if (!shouldBeProgressIndicatorVisible && (delayCancellationTokenSource != null))
		//	{
		//		delayCancellationTokenSource.Cancel();
		//		delayCancellationTokenSource = null;
		//	}
		//}

		//protected override bool ShouldRender()
		//{
		//	bool result = shouldRender;
		//	shouldRender = false;
		//	return shouldRender;
		//}

		//private void StartInProgressWithDelay()
		//{
		//	if (delayCancellationTokenSource == null)
		//	{
		//		delayCancellationTokenSource = new CancellationTokenSource();
		//		//Task.Run(() => xxx, delayCancellationTokenSource.Token);
		//	}
		//}
	}
}
