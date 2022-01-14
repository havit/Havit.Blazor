using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

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

		public void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				NavigationManager.LocationChanged -= OnLocationChanged;
			}
		}
	}
}
