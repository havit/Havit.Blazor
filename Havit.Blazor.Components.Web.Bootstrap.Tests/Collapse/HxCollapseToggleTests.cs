namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Collapse;

public class HxCollapseToggleTests : BunitTestBase
{
	[Fact]
	public void HxCollapseToggleButton_Render_RendersAriaExpandedFalseAndAriaControls()
	{
		// Act
		var cut = Render<HxCollapseToggleButton>(parameters => parameters
			.Add(p => p.CollapseTarget, "#myCollapse")
			.Add(p => p.Text, "Toggle"));

		// Assert
		var button = cut.Find("button");
		Assert.Equal("collapse", button.GetAttribute("data-bs-toggle"));
		Assert.Equal("#myCollapse", button.GetAttribute("data-bs-target"));
		Assert.Equal("myCollapse", button.GetAttribute("aria-controls"));
		Assert.Equal("false", button.GetAttribute("aria-expanded"));
	}

	[Fact]
	public void HxCollapseToggleElement_Render_RendersAriaExpandedFalseAndAriaControls()
	{
		// Act
		var cut = Render<HxCollapseToggleElement>(parameters => parameters
			.Add(p => p.CollapseTarget, "#myCollapse")
			.AddChildContent("Toggle"));

		// Assert
		var element = cut.Find("span");
		Assert.Equal("collapse", element.GetAttribute("data-bs-toggle"));
		Assert.Equal("#myCollapse", element.GetAttribute("data-bs-target"));
		Assert.Equal("myCollapse", element.GetAttribute("aria-controls"));
		Assert.Equal("false", element.GetAttribute("aria-expanded"));
	}
}
