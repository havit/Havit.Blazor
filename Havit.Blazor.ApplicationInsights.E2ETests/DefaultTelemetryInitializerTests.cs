using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;
using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure.Model;
using Havit.Blazor.ApplicationInsights.TestApp.Client;
using Microsoft.Playwright;

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
		var capturedTelemetryItems = new List<AiTelemetryItem>();
		await Page.RouteApplicationInsightsTrackAsync(capturedTelemetryItems);

		// Act
		// (due SSR page does not have OnAfterRender[Async] with #done & BlazorApplicationInsights.FlushAsync())
		await Page.GotoAsync(url);
		await Page.WaitForLoadStateAsync(Microsoft.Playwright.LoadState.NetworkIdle);
		await Page.WaitForFunctionAsync("window.appInsights && window.appInsights.core");
		await Page.EvaluateAsync("window.appInsights.flush()");
		await Task.Delay(1000, TestContext.Current.CancellationToken);
		await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
		await Page.CloseAsync();

		// Assert
		var pageViewItem = capturedTelemetryItems.FirstOrDefault(i => i.BaseType == "PageviewData");
		Assert.NotNull(pageViewItem);
		Assert.Equal(TestDefaults.DefaultTelemetryInitializerCloudRoleName, pageViewItem.CloudRoleName);
	}
}
