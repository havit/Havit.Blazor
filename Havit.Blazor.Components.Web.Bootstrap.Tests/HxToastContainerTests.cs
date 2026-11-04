namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxToastContainerTests : BunitTestBase
{
	[Theory]
	[InlineData(ToastContainerPosition.TopStart, "top-0")]
	[InlineData(ToastContainerPosition.TopCenter, "top-0")]
	[InlineData(ToastContainerPosition.TopEnd, "top-0")]
	[InlineData(ToastContainerPosition.BottomStart, "bottom-0")]
	[InlineData(ToastContainerPosition.BottomCenter, "bottom-0")]
	[InlineData(ToastContainerPosition.BottomEnd, "bottom-0")]
	public void HxToastContainer_Position_PinsContainerToViewportEdge(ToastContainerPosition position, string expectedCssClass)
	{
		// Act
		var cut = Render<HxToastContainer>(parameters => parameters
			.Add(p => p.Position, position));

		// Assert
		var container = cut.Find(".hx-toast-container");
		Assert.True(container.ClassList.Contains("position-fixed"));
		Assert.True(container.ClassList.Contains(expectedCssClass));
	}
}
