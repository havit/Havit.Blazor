namespace Havit.Blazor.ApplicationInsights.Telemetry;

/// <summary>
/// Represents the lifetime of a timed Application Insights event.
/// Created by <see cref="BlazorApplicationInsightsExtensions.EnterTrackEventScopeAsync"/>,
/// which calls <see cref="IBlazorApplicationInsights"/>.<c>StartTrackEventAsync</c> upon creation
/// and <see cref="IBlazorApplicationInsights"/>.<c>StopTrackEventAsync</c> when the scope is disposed.
/// </summary>
/// <example>
/// <code>
/// await using var scope = await blazorApplicationInsights.EnterTrackEventScopeAsync("my-operation");
/// scope.Properties["result"] = "ok";
/// </code>
/// </example>
public class TrackedEventScope : IAsyncDisposable
{
	private readonly IBlazorApplicationInsights _blazorApplicationInsights;
	private readonly string _name;

	private Dictionary<string, string> _properties;
	private Dictionary<string, double> _measurements;

	/// <summary>
	/// Custom string properties attached to the event when <see cref="IBlazorApplicationInsights"/>.<c>StopTrackEventAsync</c> is called.
	/// Can be set any time before the scope is disposed.
	/// The dictionary is created lazily on first access.
	/// </summary>
	public Dictionary<string, string> Properties => _properties ??= new Dictionary<string, string>();

	/// <summary>
	/// Custom numeric measurements attached to the event when <see cref="IBlazorApplicationInsights"/>.<c>StopTrackEventAsync</c> is called.
	/// Can be set any time before the scope is disposed.
	/// The dictionary is created lazily on first access.
	/// </summary>
	public Dictionary<string, double> Measurements => _measurements ??= new Dictionary<string, double>();

	internal TrackedEventScope(IBlazorApplicationInsights blazorApplicationInsights, string name)
	{
		_blazorApplicationInsights = blazorApplicationInsights;
		_name = name;
	}

	/// <inheritdoc/>
	public async ValueTask DisposeAsync()
		// Use backing fields directly to avoid triggering lazy initialization for unused dictionaries.
		=> await _blazorApplicationInsights.StopTrackEventAsync(_name, _properties, _measurements);
}
