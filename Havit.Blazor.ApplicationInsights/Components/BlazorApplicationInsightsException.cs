using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.ApplicationInsights.Components;

/// <summary>
/// Component which tracks exception whenever the <c>Exception</c> parameter is set (changed).
/// Intended for use within an <c>ErrorBoundary</c>'s <c>ErrorContent</c> to automatically track unhandled exceptions with Application Insights.
/// </summary>
public class BlazorApplicationInsightsException : ComponentBase
{
	/// <summary>
	/// Service used to interact with Application Insights in Blazor applications.
	/// </summary>
	[Inject] protected IBlazorApplicationInsights BlazorApplicationInsights { get; set; }

	/// <summary>
	/// The exception to track. Typically bound to the <c>Context</c> of an <c>ErrorBoundary</c>'s
	/// <c>ErrorContent</c>. A new telemetry item is sent each time the value changes.
	/// </summary>
	[Parameter, EditorRequired]
	public Exception Exception { get; set; } = default!;

	private Exception _lastTrackedException;

	/// <inheritdoc/>
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if ((Exception != null) && !ReferenceEquals(Exception, _lastTrackedException))
		{
			_lastTrackedException = Exception;
			await BlazorApplicationInsights.TrackExceptionAsync(Exception);
		}
	}
}
