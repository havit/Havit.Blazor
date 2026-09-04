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
		await Page.GetByRole(AriaRole.Link, new() { Name = "Interactive Server" }).ClickAsync();
		await Page.WaitForURLAsync("**/HxGoogleTagManagerTests/Server");
		await WaitForVirtualPageViewCountAsync(expectedTotalVirtualPageViewCount);

		var trackedUrls = await GetTrackedPageUrlsAsync();
		Assert.Equal(expectedTotalVirtualPageViewCount, trackedUrls.Count);
		Assert.EndsWith("/HxGoogleTagManagerTests/StaticSsr2", trackedUrls[^2]);
		Assert.EndsWith("/HxGoogleTagManagerTests/Server", trackedUrls[^1]);
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
