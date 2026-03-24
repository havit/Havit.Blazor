using System.Text.RegularExpressions;

namespace Havit.Blazor.E2ETests.HxCarouselTests;

[TestClass]
public class HxCarousel_Tests : TestAppTestBase
{
	private static readonly Regex ActiveClassRegex = new Regex("\\bactive\\b");
	private const int CarouselTransitionTimeout = 5_000;

	[TestMethod]
	public async Task HxCarousel_Render_DisplaysCorrectSlideCount()
	{
		// Arrange & Act
		await NavigateToTestAppAsync("/HxCarousel");

		// Assert - there should be exactly 3 carousel items
		var slides = Page.Locator(".carousel-item");
		await Expect(slides).ToHaveCountAsync(3);
	}

	[TestMethod]
	public async Task HxCarousel_ClickNext_AdvancesToNextSlide()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxCarousel");

		// Verify slide 1 is initially active
		var firstSlide = Page.Locator(".carousel-item").First;
		await Expect(firstSlide).ToHaveClassAsync(ActiveClassRegex);

		// Act - click the next control
		var nextButton = Page.Locator(".carousel-control-next");
		await nextButton.ClickAsync();

		// Assert - slide 2 (index 1) should become active
		var secondSlide = Page.Locator(".carousel-item").Nth(1);
		await Expect(secondSlide).ToHaveClassAsync(ActiveClassRegex, new() { Timeout = CarouselTransitionTimeout });
	}

	[TestMethod]
	public async Task HxCarousel_ClickPrevious_GoesToPreviousSlide()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxCarousel");

		// First advance to slide 2
		var nextButton = Page.Locator(".carousel-control-next");
		await nextButton.ClickAsync();

		var secondSlide = Page.Locator(".carousel-item").Nth(1);
		await Expect(secondSlide).ToHaveClassAsync(ActiveClassRegex, new() { Timeout = CarouselTransitionTimeout });

		// Act - click the previous control
		var prevButton = Page.Locator(".carousel-control-prev");
		await prevButton.ClickAsync();

		// Assert - slide 1 (index 0) should be active again
		var firstSlide = Page.Locator(".carousel-item").First;
		await Expect(firstSlide).ToHaveClassAsync(ActiveClassRegex, new() { Timeout = CarouselTransitionTimeout });
	}

	[TestMethod]
	public async Task HxCarousel_Indicators_ReflectCurrentSlide()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxCarousel");

		// Assert - the first indicator should be active initially
		var indicators = Page.Locator(".carousel-indicators button");
		await Expect(indicators).ToHaveCountAsync(3);

		var firstIndicator = indicators.First;
		await Expect(firstIndicator).ToHaveClassAsync(ActiveClassRegex);

		// Act - advance to the next slide
		var nextButton = Page.Locator(".carousel-control-next");
		await nextButton.ClickAsync();

		// Assert - the second indicator should now be active
		var secondIndicator = indicators.Nth(1);
		await Expect(secondIndicator).ToHaveClassAsync(ActiveClassRegex, new() { Timeout = CarouselTransitionTimeout });

		// And the first indicator should no longer be active
		await Expect(firstIndicator).Not.ToHaveClassAsync(ActiveClassRegex, new() { Timeout = CarouselTransitionTimeout });
	}
}
