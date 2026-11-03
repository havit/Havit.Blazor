using System.Text.RegularExpressions;

namespace Havit.Blazor.E2ETests.HxScrollspyTests;

/// <summary>
/// HxScrollspyNavLink scrolls the scroll containers itself (navigateToFragment in HxScrollspy.js),
/// the destination must respect scroll-padding-top of the containers (e.g. a sticky header), as element.scrollIntoView() does.
/// </summary>
public class HxScrollspyTests : TestAppTestBase
{
	private const double Tolerance = 1.5;

	[Fact]
	public async Task HxScrollspyNavLink_StickyHeaderWithScrollPadding_SectionNotObscured()
	{
		// Arrange
		await Page.SetViewportSizeAsync(1200, 800);
		await NavigateToTestAppAsync("/HxScrollspyTests");

		// Act
		await Page.Locator("[data-testid='padding-link-3']").ClickAsync();

		// Assert - the section is right below the sticky header (scroll-padding-top: 50px), not under it
		var section = Page.Locator("#padding-section3");
		await AssertTopEventuallyAsync(section, "padding-section3", expectedOffset: 50);

		var sectionBox = await section.BoundingBoxAsync();
		var headerBox = await Page.Locator("[data-testid='padding-header']").BoundingBoxAsync();
		Assert.True(sectionBox.Y >= headerBox.Y + headerBox.Height - Tolerance, $"The section (top {sectionBox.Y}) is obscured by the sticky header (bottom {headerBox.Y + headerBox.Height}).");
		await Expect(Page).ToHaveURLAsync(new Regex("#padding-section3$"));
		await Expect(section).ToBeFocusedAsync();

		// Act - back to the first section (clamped at the start of the content)
		await Page.Locator("[data-testid='padding-link-1']").ClickAsync();

		// Assert
		await Expect(Page).ToHaveURLAsync(new Regex("#padding-section1$"));
		Assert.Equal(0, await Page.Locator("#padding-section1").EvaluateAsync<double>("e => e.closest('.hx-scrollspy').scrollTop"));
	}

	[Fact]
	public async Task HxScrollspyNavLink_PercentageScrollPaddingAndScrollMargin_BothApplied()
	{
		// Arrange
		await Page.SetViewportSizeAsync(1200, 800);
		await NavigateToTestAppAsync("/HxScrollspyTests");

		// Act
		await Page.Locator("[data-testid='percent-link-3']").ClickAsync();

		// Assert - scroll-padding-top: 20% of the 300px scrollport (60px) + scroll-margin-top: 10px
		await AssertTopEventuallyAsync(Page.Locator("#percent-section3"), "percent-section3", expectedOffset: 70);
	}

	[Fact]
	public async Task HxScrollspyNavLink_NestedScrollContainers_ScrollPaddingOfEachContainerApplied()
	{
		// Arrange
		await Page.SetViewportSizeAsync(1200, 800);
		await NavigateToTestAppAsync("/HxScrollspyTests");

		// Act
		await Page.Locator("[data-testid='nested-link-3']").ClickAsync();

		// Assert - the section is 30px (scroll-padding-top of the inner container) below the top of the inner scrollport...
		var section = Page.Locator("#nested-section3");
		await Expect(Page).ToHaveURLAsync(new Regex("#nested-section3$"));
		await AssertTopEventuallyAsync(section, "nested-section3", expectedOffset: 30, relativeTo: "e.parentElement");

		// ... and 50px (scroll-padding-top of the HxScrollspy container, the sticky header) below the top of the HxScrollspy scrollport
		await AssertTopEventuallyAsync(section, "nested-section3", expectedOffset: 50);
	}

	/// <summary>
	/// Asserts the distance between the top of the element and the top of the scrollport (inside the border) of the container.
	/// </summary>
	private async Task AssertTopEventuallyAsync(ILocator element, string description, double expectedOffset, string relativeTo = "e.closest('.hx-scrollspy')")
	{
		double actualOffset = double.NaN;
		for (int i = 0; i < 50; i++)
		{
			actualOffset = await element.EvaluateAsync<double>($@"e => {{
				const c = {relativeTo};
				return e.getBoundingClientRect().top - c.getBoundingClientRect().top - c.clientTop;
			}}");
			if (Math.Abs(actualOffset - expectedOffset) <= Tolerance)
			{
				return;
			}
			await Task.Delay(100);
		}
		Assert.Fail($"{description}: expected {expectedOffset}px below the top of the scroll container, actual {actualOffset}px.");
	}
}
