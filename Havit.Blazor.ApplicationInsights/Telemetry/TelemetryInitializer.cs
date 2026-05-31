namespace Havit.Blazor.ApplicationInsights.Telemetry;

/// <summary>
/// Defines static tag values applied to every telemetry envelope by the Application Insights
/// JavaScript SDK — including auto-collected telemetry (page views, XHR requests, unhandled exceptions).
/// Pass to <see cref="IBlazorApplicationInsights"/>.<c>AddTelemetryInitializerAsync</c> once after SDK initialization.
/// </summary>
/// <remarks>
/// Use the typed properties for common tags. For any other AI SDK tag, set <see cref="Tags"/> directly.
/// Typed properties are aliases over <see cref="Tags"/> — all values end up in the same dictionary.
/// </remarks>
/// <example>
/// <code>
/// await BlazorApplicationInsights.AddTelemetryInitializerAsync(new TelemetryInitializer
/// {
///     CloudRoleName = "MyBlazorApp",
///     ApplicationVersion = "1.2.3"
/// });
/// </code>
/// </example>
public class TelemetryInitializer
{
	private Dictionary<string, string> _tags = new Dictionary<string, string>();

	/// <summary>
	/// Raw AI SDK tags. All typed properties (<see cref="CloudRoleName"/>, <see cref="ApplicationVersion"/>)
	/// read and write directly into this dictionary.
	/// </summary>
	public Dictionary<string, string> Tags
	{
		get => _tags;
		set => _tags = value ?? new Dictionary<string, string>();
	}

	/// <summary>
	/// Sets the <c>ai.cloud.role</c> tag. Identifies the service or application component
	/// in Azure Monitor / Application Insights — use to distinguish multiple services
	/// sending to the same Application Insights resource.
	/// </summary>
	public string CloudRoleName
	{
		get => Tags.GetValueOrDefault("ai.cloud.role");
		set => Tags["ai.cloud.role"] = value;
	}

	/// <summary>
	/// Sets the <c>ai.application.ver</c> tag. The JS SDK does not detect the application version
	/// automatically — provide it here to enable filtering telemetry by deployment version.
	/// </summary>
	public string ApplicationVersion
	{
		get => Tags.GetValueOrDefault("ai.application.ver");
		set => Tags["ai.application.ver"] = value;
	}

	/// <summary>
	/// Returns the dictionary of AI SDK tags to be applied to all telemetry envelopes.
	/// </summary>
	internal IDictionary<string, string> GetTags() => Tags;
}
