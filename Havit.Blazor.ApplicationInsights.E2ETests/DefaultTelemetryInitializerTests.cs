using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;
using Havit.Blazor.ApplicationInsights.TestApp.Client;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

public class DefaultTelemetryInitializerTests : BlazorApplicationInsightsPageTestBase
{
	[Fact]
	public async Task DefaultTelemetryInitializer_SSR_CloudRoleNameAppliedToInitialPageView()
		=> await TestDefaultTelemetryInitializerCloudRoleName(NavigationRoutes.DefaultTelemetryInitializerTests.ServerSideRendering);

	[Fact]
	public async Task DefaultTelemetryInitializer_InteractiveServer_CloudRoleNameAppliedToInitialPageView()
		=> await TestDefaultTelemetryInitializerCloudRoleName(NavigationRoutes.DefaultTelemetryInitializerTests.InteractiveServer);

	[Fact]
	public async Task DefaultTelemetryInitializer_InteractiveServerPrerendering_CloudRoleNameAppliedToInitialPageView()
		=> await TestDefaultTelemetryInitializerCloudRoleName(NavigationRoutes.DefaultTelemetryInitializerTests.InteractiveServerPrerendering);

	[Fact]
	public async Task DefaultTelemetryInitializer_InteractiveWebAssembly_CloudRoleNameAppliedToInitialPageView()
		=> await TestDefaultTelemetryInitializerCloudRoleName(NavigationRoutes.DefaultTelemetryInitializerTests.InteractiveWebAssembly);

	[Fact]
	public async Task DefaultTelemetryInitializer_InteractiveWebAssemblyPrerendering_CloudRoleNameAppliedToInitialPageView()
		=> await TestDefaultTelemetryInitializerCloudRoleName(NavigationRoutes.DefaultTelemetryInitializerTests.InteractiveWebAssemblyPrerendering);

	private async Task TestDefaultTelemetryInitializerCloudRoleName(string url)
	{
		// Arrange
		var telemetry = await Page.RouteApplicationInsightsTrackAsync();

		// Act
		// the page has no OnAfterRender[Async] (it has to work in SSR too), so the test flushes the SDK itself
		await Page.GotoAsync(url);
		await Page.WaitForApplicationInsightsReadyAsync();
		await Page.EvaluateAsync("() => window.havitBlazorAppInsights.flush()");

		// Assert
		var pageViewItem = await telemetry.WaitForItemAsync(i => i.BaseType == "PageviewData", "initial page view");
		Assert.Equal(TestDefaults.DefaultTelemetryInitializerCloudRoleName, pageViewItem.CloudRoleName);
	}
}
