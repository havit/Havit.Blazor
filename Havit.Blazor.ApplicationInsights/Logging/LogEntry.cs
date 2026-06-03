using Havit.Blazor.ApplicationInsights.Telemetry;

namespace Havit.Blazor.ApplicationInsights.Logging;

internal readonly record struct LogEntry(
	string Message,
	SeverityLevel SeverityLevel,
	Exception Exception,
	Dictionary<string, object> Properties
);
