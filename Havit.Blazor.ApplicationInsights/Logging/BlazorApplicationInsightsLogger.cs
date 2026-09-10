using System.Threading.Channels;
using Havit.Blazor.ApplicationInsights.Telemetry;
using Microsoft.Extensions.Logging;

namespace Havit.Blazor.ApplicationInsights.Logging;

internal sealed class BlazorApplicationInsightsLogger : ILogger
{
	private readonly string _categoryName;
	private readonly ChannelWriter<LogEntry> _channelWriter;
	private readonly IExternalScopeProvider _scopeProvider;

	public BlazorApplicationInsightsLogger(string categoryName, ChannelWriter<LogEntry> channelWriter, IExternalScopeProvider scopeProvider)
	{
		_categoryName = categoryName;
		_channelWriter = channelWriter;
		_scopeProvider = scopeProvider;
	}

	/// <inheritdoc />
	public IDisposable BeginScope<TState>(TState state) => _scopeProvider.Push(state);

	/// <inheritdoc />
	public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

	/// <inheritdoc />
	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
	{
		if (!IsEnabled(logLevel))
		{
			return;
		}

		var message = formatter(state, exception);
		var severityLevel = MapSeverityLevel(logLevel);
		var properties = new Dictionary<string, object> { ["CategoryName"] = _categoryName };
		if (eventId.Id != 0)
		{
			properties["EventId"] = eventId.Id;
		}
		if (!string.IsNullOrEmpty(eventId.Name))
		{
			properties["EventName"] = eventId.Name;
		}
		if (exception != null && !string.IsNullOrEmpty(message))
		{
			properties["Message"] = message;
		}

		_scopeProvider.ForEachScope((scopeState, props) =>
		{
			if (scopeState is IEnumerable<KeyValuePair<string, object>> kvPairs)
			{
				foreach (var kv in kvPairs)
				{
					props.TryAdd(kv.Key, kv.Value);
				}
			}
			else if (scopeState is string s && !string.IsNullOrEmpty(s))
			{
				props.TryAdd("Scope", s);
			}
		}, properties);

		_channelWriter.TryWrite(new LogEntry(
			Message: exception == null ? message : null,
			SeverityLevel: severityLevel,
			Exception: exception,
			Properties: properties
		));
	}

	private static SeverityLevel MapSeverityLevel(LogLevel logLevel) => logLevel switch
	{
		LogLevel.Trace => SeverityLevel.Verbose,
		LogLevel.Debug => SeverityLevel.Verbose,
		LogLevel.Information => SeverityLevel.Information,
		LogLevel.Warning => SeverityLevel.Warning,
		LogLevel.Error => SeverityLevel.Error,
		LogLevel.Critical => SeverityLevel.Critical,
		_ => throw new InvalidOperationException($"Unsupported log level: {logLevel}.")
	};
}
