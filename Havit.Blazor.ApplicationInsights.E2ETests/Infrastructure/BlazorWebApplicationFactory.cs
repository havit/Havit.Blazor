using Havit.Blazor.ApplicationInsights.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;

public class BlazorWebApplicationFactory : WebApplicationFactory<TestApp.Program>
{
	private readonly Action<BlazorApplicationInsightsOptions> _optionsOverride;

	private IHost _host;

	public BlazorWebApplicationFactory(Action<BlazorApplicationInsightsOptions> optionsOverride = null)
	{
		_optionsOverride = optionsOverride;
	}

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.UseEnvironment("Development");
		if (_optionsOverride != null)
		{
			builder.ConfigureServices(services =>
				services.Configure<BlazorApplicationInsightsOptions>(_optionsOverride));
		}
	}

	protected override IHost CreateHost(IHostBuilder builder)
	{
		var testHost = builder.Build();
		testHost.Start(); // (solves: System.InvalidOperationException: The server has not been started or no web application was configured.)

		// Configure Kestrel to use a dynamic port
		builder.ConfigureWebHost(webHostBuilder =>
		{
			webHostBuilder.UseKestrel();
			webHostBuilder.UseUrls("http://127.0.0.1:0");
		});

		_host = builder.Build();
		_host.Start();

		// wait for kestrel start (solves: System.InvalidOperationException: The server has not been started or no web application was configured.)
		var lifetime = _host.Services.GetRequiredService<IHostApplicationLifetime>();
		lifetime.ApplicationStarted.WaitHandle.WaitOne();

		return testHost;
	}

	public string GetServerAddress()
	{
		if (_host == null)
		{
			throw new InvalidOperationException("Host has not been created yet.");
		}

		var server = _host.Services.GetRequiredService<IServer>();
		var addressFeature = server.Features.Get<IServerAddressesFeature>();
		return addressFeature?.Addresses.FirstOrDefault() ?? "http://localhost";
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_host?.Dispose();
		}
		base.Dispose(disposing);
	}
}
