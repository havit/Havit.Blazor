using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.ApplicationInsights.Components;

public class BlazorApplicationInsightsException : ComponentBase
{
	[Inject] protected IBlazorApplicationInsights BlazorApplicationInsights { get; set; }

	/// <summary>
	/// The exception to track. Typically bound to the <c>Context</c> of an <c>ErrorBoundary</c>'s
	/// <c>ErrorContent</c>. A new telemetry item is sent each time the value changes.
	/// </summary>
	[Parameter, EditorRequired]
	public Exception Exception { get; set; } = default!;

	private Exception _lastTrackedException;

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if ((Exception != null) && !ReferenceEquals(Exception, _lastTrackedException))
		{
			_lastTrackedException = Exception;
			await BlazorApplicationInsights.TrackExceptionAsync(Exception);
		}
	}
}
