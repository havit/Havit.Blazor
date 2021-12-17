using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace Havit.Blazor.GoogleTagManager
{
	public class HxGoogleTagManagerPageViewTracker : ComponentBase, IDisposable
	{
		[Inject] protected NavigationManager NavigationManager { get; set; }
		[Inject] protected IHxGoogleTagManager HxGoogleTagManager { get; set; }

		private LocationChangedEventArgs locationChangedEventArgsToReportOnAfterRenderAsync;

		protected override void OnInitialized()
		{
			base.OnInitialized();

			NavigationManager.LocationChanged += OnLocationChanged;
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender || (locationChangedEventArgsToReportOnAfterRenderAsync is not null))
			{
				var argsToReport = locationChangedEventArgsToReportOnAfterRenderAsync;
				locationChangedEventArgsToReportOnAfterRenderAsync = null;
				await HxGoogleTagManager.PushPageViewAsync(argsToReport);
			}

			await base.OnAfterRenderAsync(firstRender);
		}

		private void OnLocationChanged(object sender, LocationChangedEventArgs args)
		{
			locationChangedEventArgsToReportOnAfterRenderAsync = args;
			StateHasChanged();
		}

		public virtual void Dispose()
		{
			NavigationManager.LocationChanged -= OnLocationChanged;
		}
	}
}
