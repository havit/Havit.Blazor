namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Collapse;

[TestClass]
public class HxCollapse_Basic_Tests : BunitTestBase
{
	[TestMethod]
	public void HxCollapse_Render_DoesNotSetAriaExpandedOnContentContainer()
	{
		// Arrange & Act
		var cut = RenderComponent<HxCollapse>(parameters => parameters
			.AddChildContent("Test content"));

		// Assert
		var collapse = cut.Find("div.collapse");
		Assert.IsNull(collapse.GetAttribute("aria-expanded"));
	}
}
