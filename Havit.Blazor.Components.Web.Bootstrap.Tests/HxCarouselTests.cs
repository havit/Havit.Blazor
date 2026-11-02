namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxCarouselTests : BunitTestBase
{
	[Fact]
	public void HxCarousel_ControlsPlacementDefault_RendersControlsBelowSlides()
	{
		// Act
		var cut = Render<HxCarousel>(parameters => parameters
			.Add(p => p.Controls, true)
		);

		// Assert
		var children = cut.Find("div.carousel").Children;
		Assert.Equal(2, children.Length);
		Assert.True(children[0].ClassList.Contains("carousel-inner"));
		Assert.NotNull(children[1].QuerySelector("button[data-bs-slide=prev]"));
	}

	[Fact]
	public void HxCarousel_ControlsPlacementTop_RendersControlsAboveSlides()
	{
		// Act
		var cut = Render<HxCarousel>(parameters => parameters
			.Add(p => p.Controls, true)
			.Add(p => p.ControlsPlacement, CarouselControlsPlacement.Top)
		);

		// Assert
		var children = cut.Find("div.carousel").Children;
		Assert.Equal(2, children.Length);
		Assert.NotNull(children[0].QuerySelector("button[data-bs-slide=prev]"));
		Assert.NotNull(children[0].QuerySelector("button[data-bs-slide=next]"));
		Assert.True(children[1].ClassList.Contains("carousel-inner"));
	}

	[Fact]
	public void HxCarousel_ControlsPlacementTopWithOverlay_RendersOverlayControlsAfterSlides()
	{
		// Act
		var cut = Render<HxCarousel>(parameters => parameters
			.Add(p => p.Controls, true)
			.Add(p => p.Overlay, true)
			.Add(p => p.ControlsPlacement, CarouselControlsPlacement.Top)
		);

		// Assert
		var children = cut.Find("div.carousel.carousel-overlay").Children;
		Assert.Equal(2, children.Length);
		Assert.True(children[0].ClassList.Contains("carousel-inner"));
		Assert.True(children[1].ClassList.Contains("carousel-overlay-controls"));
	}

	[Fact]
	public void HxCarousel_ControlsPlacementTopWithoutControls_RendersSlidesOnly()
	{
		// Act
		var cut = Render<HxCarousel>(parameters => parameters
			.Add(p => p.ControlsPlacement, CarouselControlsPlacement.Top)
		);

		// Assert
		var children = cut.Find("div.carousel").Children;
		Assert.Single(children);
		Assert.True(children[0].ClassList.Contains("carousel-inner"));
	}
}
