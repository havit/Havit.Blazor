namespace Havit.Blazor.ApplicationInsights.Telemetry;

/// <summary>
/// Represents the lifetime of a timed Application Insights page view.
/// Created by <see cref="BlazorApplicationInsightsExtensions.EnterTrackPageScopeAsync"/>,
/// which calls <see cref="IBlazorApplicationInsights"/>.<c>StartTrackPageAsync</c> upon creation
/// and <see cref="IBlazorApplicationInsights"/>.<c>StopTrackPageAsync</c> when the scope is disposed.
/// </summary>
/// <example>
/// <code>
/// await using var scope = await blazorApplicationInsights.EnterTrackPageScopeAsync("my-page");
/// scope.Url = "https://example.com/my-page";
/// </code>
/// </example>
public class TrackedPageScope : IAsyncDisposable
{
	private readonly IBlazorApplicationInsights _blazorApplicationInsights;
	private readonly string _name;

	/// <summary>
	/// URL of the page to include when <see cref="IBlazorApplicationInsights"/>.<c>StopTrackPageAsync</c> is called.
	/// Can be set any time before the scope is disposed.
	/// </summary>
	public string Url { get; set; }

	private Dictionary<string, string> _properties;
	private Dictionary<string, double> _measurements;

	/// <summary>
	/// Custom string properties attached to the page view when <see cref="IBlazorApplicationInsights"/>.<c>StopTrackPageAsync</c> is called.
	/// Can be set any time before the scope is disposed.
	/// The dictionary is created lazily on first access.
	/// </summary>
	public Dictionary<string, string> Properties => _properties ??= new Dictionary<string, string>();

	/// <summary>
	/// Custom numeric measurements attached to the page view when <see cref="IBlazorApplicationInsights"/>.<c>StopTrackPageAsync</c> is called.
	/// Can be set any time before the scope is disposed.
	/// The dictionary is created lazily on first access.
	/// </summary>
	public Dictionary<string, double> Measurements => _measurements ??= new Dictionary<string, double>();

	internal TrackedPageScope(IBlazorApplicationInsights blazorApplicationInsights, string name)
	{
		_blazorApplicationInsights = blazorApplicationInsights;
		_name = name;
	}

	/// <inheritdoc/>
	public async ValueTask DisposeAsync()
		// Use backing fields directly to avoid triggering lazy initialization for unused dictionaries.
		=> await _blazorApplicationInsights.StopTrackPageAsync(_name, Url, _properties, _measurements);
}
