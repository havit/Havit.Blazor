using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Havit.Blazor.E2ETests;

/// <summary>
/// WebApplicationFactory for TestApp that manages the test server lifecycle.
/// Starts a real Kestrel server for Playwright to connect to.
/// </summary>
public class TestAppWebApplicationFactory : WebApplicationFactory<TestApp.Program>
{
	private IHost _host;

	protected override IHost CreateHost(IHostBuilder builder)
	{
		var testHost = builder.Build();

		// Configure Kestrel to use a dynamic port
		builder.ConfigureWebHost(webHostBuilder =>
		{
			webHostBuilder.UseKestrel();
			webHostBuilder.UseUrls("http://127.0.0.1:0");
		});

		_host = builder.Build();
		_host.Start();

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
