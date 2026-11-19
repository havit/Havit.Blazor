namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Progress;

public class HxProgressIndicatorTests : BunitTestBase
{
	[Fact]
	public void HxProgressIndicator_InProgressTrue_ShowsOverlay()
	{
		// Arrange & Act
		var cut = Render<HxProgressIndicator>(parameters => parameters
			.Add(p => p.InProgress, true)
			.Add(p => p.Delay, 0)
		);

		// Assert
		var overlay = cut.Find(".hx-progress-indicator");
		Assert.NotNull(overlay);
	}

	[Fact]
	public void HxProgressIndicator_InProgressFalse_HidesOverlay()
	{
		// Arrange & Act
		var cut = Render<HxProgressIndicator>(parameters => parameters
			.Add(p => p.InProgress, false)
		);

		// Assert
		var overlayElements = cut.FindAll(".hx-progress-indicator");
		Assert.Empty(overlayElements);
	}

	[Fact]
	public void HxProgressIndicator_Content_RemainsVisibleUnderOverlay()
	{
		// Arrange & Act
		var cut = Render<HxProgressIndicator>(parameters => parameters
			.Add(p => p.InProgress, true)
			.Add(p => p.Delay, 0)
			.Add(p => p.ChildContent, builder => builder.AddMarkupContent(0, "<p id=\"test-content\">Content</p>"))
		);

		// Assert - overlay is visible
		var overlay = cut.Find(".hx-progress-indicator");
		Assert.NotNull(overlay);

		// Assert - content is also visible
		var content = cut.Find("#test-content");
		Assert.NotNull(content);
	}

	[Fact]
	public void HxProgressIndicator_Spinner_RendersStatusWithLocalizedText()
	{
		// Arrange & Act
		var cut = Render<HxProgressIndicator>(parameters => parameters
			.Add(p => p.InProgress, true)
			.Add(p => p.Delay, 0)
		);

		// Assert - the status must have an accessible text (HxSpinner's localized HiddenText)
		var spinner = cut.Find(".hx-progress-indicator .hx-progress-indicator-spinner.spinner-border");
		Assert.Equal("status", spinner.GetAttribute("role"));
		Assert.False(String.IsNullOrWhiteSpace(spinner.QuerySelector(".visually-hidden")?.TextContent));
	}
}
