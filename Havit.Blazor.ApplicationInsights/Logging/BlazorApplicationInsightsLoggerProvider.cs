using System.Collections.Concurrent;
using System.Threading.Channels;
using Havit.Blazor.ApplicationInsights.Telemetry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
	/// <summary>
	/// Provider name. Use this name in <c>appsettings.json</c> to configure log levels for this provider.
	/// </summary>
	public const string ProviderName = "BlazorApplicationInsights";

	private readonly Channel<LogEntry> _channel;
	private readonly Task _processTask;
	private readonly ConcurrentDictionary<string, BlazorApplicationInsightsLogger> _loggers = new();
	private IExternalScopeProvider _scopeProvider = new LoggerExternalScopeProvider();

	/// <summary>
	/// Constructs a new instance of the <see cref="BlazorApplicationInsightsLoggerProvider"/> class.
	/// </summary>
	public BlazorApplicationInsightsLoggerProvider(IServiceScopeFactory scopeFactory)
	{
		_channel = Channel.CreateBounded<LogEntry>(new BoundedChannelOptions(1024)
		{
			FullMode = BoundedChannelFullMode.DropOldest,
			SingleReader = true
		});
		_processTask = ProcessLogEntriesAsync(scopeFactory, _channel.Reader);
	}

	/// <inheritdoc />
	void ISupportExternalScope.SetScopeProvider(IExternalScopeProvider scopeProvider)
		=> _scopeProvider = scopeProvider;

	/// <inheritdoc />
	public ILogger CreateLogger(string categoryName)
		=> _loggers.GetOrAdd(categoryName, name => new BlazorApplicationInsightsLogger(name, _channel.Writer, _scopeProvider));

	/// <inheritdoc />
	public void Dispose()
	{
		_channel.Writer.TryComplete();
		_loggers.Clear();
	}

	/// <inheritdoc />
	public async ValueTask DisposeAsync()
	{
		_channel.Writer.TryComplete();
#pragma warning disable VSTHRD003 // background processing task — intentional drain on dispose
		await _processTask.ConfigureAwait(false);
#pragma warning restore VSTHRD003
		_loggers.Clear();
	}

	private static async Task ProcessLogEntriesAsync(IServiceScopeFactory scopeFactory, ChannelReader<LogEntry> reader)
	{
		// BlazorApplicationInsightsLoggerProvider is a singleton,
		// but IBlazorApplicationInsights is a scoped dependency.
		using var scope = scopeFactory.CreateScope();
		var insights = scope.ServiceProvider.GetRequiredService<IBlazorApplicationInsights>();

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
