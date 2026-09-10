using Havit.Blazor.GoogleTagManager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Havit.Blazor.E2ETests.HxGoogleTagManagerTests;

public class HxGoogleTagManager_InitialPageViewTracking_Tests : PageTest
{
	[Fact]
	public async Task EnableInitialPageViewTracking_True_TracksInitialAndEnhancedNavigationWithoutDuplicates()
	{
		await TestInitialAndEnhancedNavigationAsync(enableInitialPageViewTracking: true, expectedTotalVirtualPageViewCount: 3);
	}

	[Fact]
	public async Task EnableInitialPageViewTracking_False_SkipsInitialAndTracksEnhancedNavigationWithoutDuplicates()
	{
		await TestInitialAndEnhancedNavigationAsync(enableInitialPageViewTracking: false, expectedTotalVirtualPageViewCount: 2);
	}

	[Fact]
	public async Task InitializedWithoutInlineSnippet_TracksEnhancedNavigationBetweenStaticSsrPages()
	{
		// Arrange
		await using var factory = new TestAppWebApplicationFactory();

		factory.CreateClient();
		var baseUrl = factory.GetServerAddress();

		// Act + Assert - the page is interactive without prerendering and its layout renders no tracker, so GTM
		// gets initialized from JS interop and no inline snippet ever runs
		await Page.GotoAsync(baseUrl + "/HxGoogleTagManagerTests/ServerNoPrerender");
		await Page.GetByText("interactive: True").WaitForAsync();
		await WaitForVirtualPageViewCountAsync(1);

		// Act + Assert - enhanced navigation away from the interactive page
		await Page.GetByRole(AriaRole.Link, new() { Name = "Static SSR (1)" }).ClickAsync();
		await Page.WaitForURLAsync("**/HxGoogleTagManagerTests/StaticSsr");
		await WaitForVirtualPageViewCountAsync(2);

		// Act + Assert - enhanced navigation between static SSR pages, where there is no interactive tracker left
		// and the initializer is the only one tracking. It needs the configuration the JS interop initialization
		// left behind, because the inline snippet that comes with an enhanced page update is not executed.
		await Page.GetByRole(AriaRole.Link, new() { Name = "Static SSR (2)" }).ClickAsync();
		await Page.WaitForURLAsync("**/HxGoogleTagManagerTests/StaticSsr2");
		await WaitForVirtualPageViewCountAsync(3);

		var trackedUrls = await GetTrackedPageUrlsAsync();
		Assert.Equal(3, trackedUrls.Count);
		Assert.EndsWith("/HxGoogleTagManagerTests/ServerNoPrerender", trackedUrls[0]);
		Assert.EndsWith("/HxGoogleTagManagerTests/StaticSsr", trackedUrls[1]);
		Assert.EndsWith("/HxGoogleTagManagerTests/StaticSsr2", trackedUrls[2]);
	}

	private async Task TestInitialAndEnhancedNavigationAsync(bool enableInitialPageViewTracking, int expectedTotalVirtualPageViewCount)
	{
		// Arrange
		await using var factory = new TestAppWebApplicationFactory(services =>
		{
			services.PostConfigure<HxGoogleTagManagerOptions>(options =>
			{
				options.EnableInitialPageViewTracking = enableInitialPageViewTracking;
			});
		});

		factory.CreateClient();
		var baseUrl = factory.GetServerAddress();

		// Act + Assert - initial load path in initializer
		await Page.GotoAsync(baseUrl + "/HxGoogleTagManagerTests/StaticSsr");
		await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
		await Page.WaitForSelectorAsync("#blazor-ready-for-tests", new() { State = WaitForSelectorState.Attached });
		await WaitForVirtualPageViewCountAsync(enableInitialPageViewTracking ? 1 : 0);

		// Act + Assert - enhancedload path in initializer
		await Page.GetByRole(AriaRole.Link, new() { Name = "Static SSR (2)" }).ClickAsync();
		await Page.WaitForURLAsync("**/HxGoogleTagManagerTests/StaticSsr2");
		await WaitForVirtualPageViewCountAsync(enableInitialPageViewTracking ? 2 : 1);

		// Act + Assert - overlap with interactive tracker
		await Page.GetByRole(AriaRole.Link, new() { Name = "Interactive Server (1)" }).ClickAsync();
		await Page.WaitForURLAsync("**/HxGoogleTagManagerTests/Server");
		await Page.GetByText("interactive: True").WaitForAsync();

		// Act + Assert - interactive server navigation
		await Page.GetByRole(AriaRole.Link, new() { Name = "Interactive Server (2)" }).ClickAsync();
		await Page.WaitForURLAsync("**/HxGoogleTagManagerTests/Server2");
		await WaitForVirtualPageViewCountAsync(expectedTotalVirtualPageViewCount + 1);

		var trackedUrls = await GetTrackedPageUrlsAsync();
		Assert.Equal(expectedTotalVirtualPageViewCount + 1, trackedUrls.Count);
		Assert.EndsWith("/HxGoogleTagManagerTests/StaticSsr2", trackedUrls[^3]);
		Assert.EndsWith("/HxGoogleTagManagerTests/Server", trackedUrls[^2]);
		Assert.EndsWith("/HxGoogleTagManagerTests/Server2", trackedUrls[^1]);
	}

	private async Task WaitForVirtualPageViewCountAsync(int expectedCount)
	{
		await Page.WaitForFunctionAsync(
			"""(expectedCount) => (window.dataLayer || []).filter(item => item.event === "virtualPageView").length === expectedCount""",
			expectedCount);
	}

	private async Task<IReadOnlyList<string>> GetTrackedPageUrlsAsync()
	{
		return await Page.EvaluateAsync<string[]>(
			"""() => (window.dataLayer || []).filter(item => item.event === "virtualPageView").map(item => item.pageUrl)""");
	}
}
