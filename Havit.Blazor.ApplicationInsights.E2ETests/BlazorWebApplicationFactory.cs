using Havit.Blazor.ApplicationInsights.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

public class BlazorWebApplicationFactory : WebApplicationFactory<TestApp.Program>
{
	private readonly Action<BlazorApplicationInsightsOptions> _optionsOverride;
	private IHost _kestrelHost;
	public string ServerAddress { get; private set; } = string.Empty;

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
		var testHost = builder.Build(); // dummy host pro DI

		builder.ConfigureWebHost(b =>
			b.UseKestrel(opts => opts.Listen(IPAddress.Loopback, 0)));

		_kestrelHost = builder.Build();
		_kestrelHost.Start();

		ServerAddress = _kestrelHost.Services
			.GetRequiredService<IServer>()
			.Features.Get<IServerAddressesFeature>()!
			.Addresses.First();

		return testHost;
	}

	protected override void Dispose(bool disposing)
	{
		_kestrelHost?.StopAsync().GetAwaiter().GetResult();
		_kestrelHost?.Dispose();
		base.Dispose(disposing);
	}
}
