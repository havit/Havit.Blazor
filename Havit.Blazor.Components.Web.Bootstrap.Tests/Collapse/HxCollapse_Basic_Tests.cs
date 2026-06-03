namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Collapse;

public class HxCollapse_Basic_Tests : BunitTestBase
{
	[Fact]
	public void HxCollapse_Render_DoesNotSetAriaExpandedOnContentContainer()
	{
		// Arrange & Act
		var cut = RenderComponent<HxCollapse>(parameters => parameters
			.AddChildContent("Test content"));

		// Assert
		var collapse = cut.Find("div.collapse");
		Assert.Null(collapse.GetAttribute("aria-expanded"));
	}
}
