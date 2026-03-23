namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Progress;

[TestClass]
public class HxProgressIndicatorTests : BunitTestBase
{
	[TestMethod]
	public void HxProgressIndicator_InProgressTrue_ShowsOverlay()
	{
		// Arrange & Act
		var cut = RenderComponent<HxProgressIndicator>(parameters => parameters
			.Add(p => p.InProgress, true)
			.Add(p => p.Delay, 0)
		);

		// Assert
		var overlay = cut.Find(".hx-progress-indicator");
		Assert.IsNotNull(overlay);
	}

	[TestMethod]
	public void HxProgressIndicator_InProgressFalse_HidesOverlay()
	{
		// Arrange & Act
		var cut = RenderComponent<HxProgressIndicator>(parameters => parameters
			.Add(p => p.InProgress, false)
		);

		// Assert
		var overlayElements = cut.FindAll(".hx-progress-indicator");
		Assert.IsEmpty(overlayElements);
	}

	[TestMethod]
	public void HxProgressIndicator_Content_RemainsVisibleUnderOverlay()
	{
		// Arrange & Act
		var cut = RenderComponent<HxProgressIndicator>(parameters => parameters
			.Add(p => p.InProgress, true)
			.Add(p => p.Delay, 0)
			.Add(p => p.ChildContent, builder => builder.AddMarkupContent(0, "<p id=\"test-content\">Content</p>"))
		);

		// Assert - overlay is visible
		var overlay = cut.Find(".hx-progress-indicator");
		Assert.IsNotNull(overlay);

		// Assert - content is also visible
		var content = cut.Find("#test-content");
		Assert.IsNotNull(content);
	}
}
