using System.Text.RegularExpressions;

namespace Havit.Blazor.E2ETests.HxCarouselTests;

public class HxCarousel_Tests : TestAppTestBase
{
	private static readonly Regex ActiveClassRegex = new Regex("\\bactive\\b");
	private const int CarouselTransitionTimeout = 5_000;

	[Fact]
	public async Task HxCarousel_Render_DisplaysCorrectSlideCount()
	{
		// Arrange & Act
		await NavigateToTestAppAsync("/HxCarousel");

		// Assert - there should be exactly 3 carousel items
		var slides = Page.Locator(".carousel-item");
		await Expect(slides).ToHaveCountAsync(3);
	}

	[Fact]
	public async Task HxCarousel_ClickNext_AdvancesToNextSlide()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxCarousel");

		// Verify slide 1 is initially active
		var firstSlide = Page.Locator(".carousel-item").First;
		await Expect(firstSlide).ToHaveClassAsync(ActiveClassRegex);

		// Act - click the next control
		var nextButton = Page.Locator("[data-bs-slide='next']");
		await nextButton.ClickAsync();

		// Assert - slide 2 (index 1) should become active
		var secondSlide = Page.Locator(".carousel-item").Nth(1);
		await Expect(secondSlide).ToHaveClassAsync(ActiveClassRegex, new() { Timeout = CarouselTransitionTimeout });
	}

	[Fact]
	public async Task HxCarousel_ClickPrevious_GoesToPreviousSlide()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxCarousel");

		// First advance to slide 2
		var nextButton = Page.Locator("[data-bs-slide='next']");
		await nextButton.ClickAsync();

		var secondSlide = Page.Locator(".carousel-item").Nth(1);
		await Expect(secondSlide).ToHaveClassAsync(ActiveClassRegex, new() { Timeout = CarouselTransitionTimeout });

		// Act - click the previous control
		var prevButton = Page.Locator("[data-bs-slide='prev']");
		await prevButton.ClickAsync();

		// Assert - slide 1 (index 0) should be active again
		var firstSlide = Page.Locator(".carousel-item").First;
		await Expect(firstSlide).ToHaveClassAsync(ActiveClassRegex, new() { Timeout = CarouselTransitionTimeout });
	}

	[Fact]
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
		var nextButton = Page.Locator("[data-bs-slide='next']");
		await nextButton.ClickAsync();

		// Assert - the second indicator should now be active
		var secondIndicator = indicators.Nth(1);
		await Expect(secondIndicator).ToHaveClassAsync(ActiveClassRegex, new() { Timeout = CarouselTransitionTimeout });

		// And the first indicator should no longer be active
		await Expect(firstIndicator).Not.ToHaveClassAsync(ActiveClassRegex, new() { Timeout = CarouselTransitionTimeout });
	}

	[Fact]
	public async Task HxCarousel_ItemAddedAfterInitialization_IsManagedByCarousel()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxCarousel_DynamicItems");
		var slides = Page.Locator(".carousel-item");
		var indicators = Page.Locator(".carousel-indicators button");
		await Expect(slides).ToHaveCountAsync(2);

		// Act - add a third slide after the Bootstrap carousel has been initialized
		await Page.GetByTestId("add-slide").ClickAsync();
		await Expect(slides).ToHaveCountAsync(3);
		await Expect(indicators).ToHaveCountAsync(3);

		// navigate to the added slide using its indicator
		await indicators.Nth(2).ClickAsync();

		// Assert - the added slide becomes active, the indicator is updated, the slid event is raised (scrolling may pass through slide 1 first)
		// and the next control is disabled at the end (Ends="CarouselEnds.Stop")
		await Expect(slides.Nth(2)).ToHaveClassAsync(ActiveClassRegex, new() { Timeout = CarouselTransitionTimeout });
		await Expect(indicators.Nth(2)).ToHaveClassAsync(ActiveClassRegex, new() { Timeout = CarouselTransitionTimeout });
		await Expect(Page.GetByTestId("last-slid")).ToHaveTextAsync(new Regex("->2$"), new() { Timeout = CarouselTransitionTimeout });
		await Expect(Page.Locator("[data-bs-slide='next']")).ToBeDisabledAsync(new() { Timeout = CarouselTransitionTimeout });
	}

	[Fact]
	public async Task HxCarousel_OverlayToggledDuringAutoplayWithTopControls_KeepsCycling()
	{
		// Arrange - autoplaying carousel with top controls
		await NavigateToTestAppAsync("/HxCarousel_ControlsLayoutChange");
		var carousel = Page.Locator(".carousel");
		var slides = Page.Locator(".carousel-item");
		await Expect(carousel).ToHaveClassAsync(new Regex("\\bcarousel-playing\\b"));

		// Act - Overlay moves the controls below the slides (re-creates the Bootstrap carousel) and rewrites the root class attribute
		await Page.GetByTestId("toggle-overlay").ClickAsync();
		await Expect(carousel).ToHaveClassAsync(new Regex("\\bcarousel-overlay\\b"));

		// Assert - the re-created carousel keeps cycling (advances from whichever slide is active now)
		await Expect(carousel).ToHaveClassAsync(new Regex("\\bcarousel-playing\\b"), new() { Timeout = CarouselTransitionTimeout });
		int activeIndex = await slides.EvaluateAllAsync<int>("items => items.findIndex(item => item.classList.contains('active'))");
		await Expect(slides.Nth((activeIndex + 1) % 3)).ToHaveClassAsync(ActiveClassRegex, new() { Timeout = CarouselTransitionTimeout });
	}

	[Fact]
	public async Task HxCarousel_ControlsPlacementChangedWhileCyclingViaCycleAsync_KeepsCycling()
	{
		// Arrange - carousel without Autoplay, cycling started via CycleAsync()
		await NavigateToTestAppAsync("/HxCarousel_CycleLayoutChange");
		var carousel = Page.Locator(".carousel");
		var slides = Page.Locator(".carousel-item");
		await Page.GetByTestId("start-cycle").ClickAsync();
		await Expect(carousel).ToHaveClassAsync(new Regex("\\bcarousel-playing\\b"));

		// Act - moving the controls above the slides re-creates the Bootstrap carousel
		await Page.GetByTestId("toggle-placement").ClickAsync();
		await Expect(carousel.Locator("> :first-child [data-bs-slide='prev']")).ToHaveCountAsync(1);

		// Assert - the re-created carousel keeps cycling (advances from whichever slide is active now)
		await Expect(carousel).ToHaveClassAsync(new Regex("\\bcarousel-playing\\b"), new() { Timeout = CarouselTransitionTimeout });
		int activeIndex = await slides.EvaluateAllAsync<int>("items => items.findIndex(item => item.classList.contains('active'))");
		await Expect(slides.Nth((activeIndex + 1) % 3)).ToHaveClassAsync(ActiveClassRegex, new() { Timeout = CarouselTransitionTimeout });
	}
}
