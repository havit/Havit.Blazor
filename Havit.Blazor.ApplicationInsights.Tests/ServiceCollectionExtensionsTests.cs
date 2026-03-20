using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace Havit.Blazor.ApplicationInsights.Tests;

[TestClass]
public class ServiceCollectionExtensionsTests
{
	[TestMethod]
	public void ServiceCollectionExtensions_AddBlazorApplicationInsights_WithoutConfigureOptions_ServicesCanBeResolved()
	{
		// Arrange
		var services = new ServiceCollection();
		services.AddScoped<IJSRuntime>(_ => new StubJSRuntime());
		services.AddBlazorApplicationInsights();

		// Act — validateScopes: true mirrors the full host builder scope validation
		using var provider = services.BuildServiceProvider(validateScopes: true);
		using var scope = provider.CreateScope();
		var blazorApplicationInsights = scope.ServiceProvider.GetService<IBlazorApplicationInsights>();

		// Assert
		Assert.IsNotNull(blazorApplicationInsights, "BlazorApplicationInsights could not be resolved from the service provider.");
	}

	private class StubJSRuntime : IJSRuntime
	{
		public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object[] args)
			=> ValueTask.FromResult(default(TValue));

		public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object[] args)
			=> ValueTask.FromResult(default(TValue));
	}
}
