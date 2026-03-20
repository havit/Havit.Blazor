using Havit.Blazor.ApplicationInsights.Telemetry;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace Havit.Blazor.ApplicationInsights.Logging;

/// <summary>
/// ASP.NET Core <see cref="ILoggerProvider"/> that forwards log entries to Application Insights via <see cref="IBlazorApplicationInsights"/>.
/// Use provider alias <c>"BlazorApplicationInsights"</c> in <c>appsettings.json</c> to configure log levels.
/// </summary>
/// <remarks>
/// Designed primarily for Interactive WebAssembly. In other render modes (SSR, Interactive Server)
/// log entries are silently discarded — no JS runtime is available server-side.
/// Register via <c>builder.Logging.AddBlazorApplicationInsights()</c>.
/// </remarks>
[ProviderAlias(BlazorApplicationInsightsLoggerProvider.ProviderName)]
public sealed class BlazorApplicationInsightsLoggerProvider : ILoggerProvider, IAsyncDisposable, ISupportExternalScope
{
	public const string ProviderName = "BlazorApplicationInsights";

	private readonly Channel<LogEntry> _channel;
	private readonly Task _processTask;
	private IExternalScopeProvider _scopeProvider = new LoggerExternalScopeProvider();

	public BlazorApplicationInsightsLoggerProvider(IBlazorApplicationInsights insights)
	{
		_channel = Channel.CreateBounded<LogEntry>(new BoundedChannelOptions(1024)
		{
			FullMode = BoundedChannelFullMode.DropOldest,
			SingleReader = true
		});
		_processTask = ProcessLogEntriesAsync(insights, _channel.Reader);
	}

	/// <inheritdoc />
	void ISupportExternalScope.SetScopeProvider(IExternalScopeProvider scopeProvider)
		=> _scopeProvider = scopeProvider;

	/// <inheritdoc />
	public ILogger CreateLogger(string categoryName)
		=> new BlazorApplicationInsightsLogger(categoryName, _channel.Writer, _scopeProvider);

	/// <inheritdoc />
	public void Dispose()
	{
		_channel.Writer.TryComplete();
	}

	/// <inheritdoc />
	public async ValueTask DisposeAsync()
	{
		_channel.Writer.TryComplete();
		await _processTask;
	}

	private static async Task ProcessLogEntriesAsync(IBlazorApplicationInsights insights, ChannelReader<LogEntry> reader)
	{
		await foreach (var entry in reader.ReadAllAsync())
		{
			try
			{
				if (entry.Exception != null)
				{
					await insights.TrackExceptionAsync(entry.Exception, entry.SeverityLevel, entry.Properties);
				}
				else
				{
					await insights.TrackTraceAsync(new TraceTelemetry { Message = entry.Message, SeverityLevel = entry.SeverityLevel }, entry.Properties);
				}
			}
			catch
			{
				// silently ignore telemetry errors
			}
		}
	}
}
