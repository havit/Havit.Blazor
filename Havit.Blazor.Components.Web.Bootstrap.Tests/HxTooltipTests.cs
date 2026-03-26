namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

[TestClass]
public class HxTooltipTests : BunitTestBase
{
	[TestMethod]
	public void HxTooltip_Text_RendersDataBsToggle()
	{
		// Act
		var cut = RenderComponent<HxTooltip>(p => p
			.Add(t => t.Text, "Tooltip text")
			.AddChildContent("Hover me"));

		// Assert
		var span = cut.Find("span");
		Assert.AreEqual("tooltip", span.GetAttribute("data-bs-toggle"));
	}

	[TestMethod]
	public void HxTooltip_Text_RendersDataBsTitle()
	{
		// Act
		var cut = RenderComponent<HxTooltip>(p => p
			.Add(t => t.Text, "My tooltip text")
			.AddChildContent("Hover me"));

		// Assert
		var span = cut.Find("span");
		Assert.AreEqual("My tooltip text", span.GetAttribute("data-bs-title"));
	}

	[TestMethod]
	public void HxTooltip_Placement_RendersDataBsPlacement()
	{
		// Act
		var cut = RenderComponent<HxTooltip>(p => p
			.Add(t => t.Text, "Tooltip")
			.Add(t => t.Placement, TooltipPlacement.Bottom)
			.AddChildContent("Hover me"));

		// Assert
		var span = cut.Find("span");
		Assert.AreEqual("bottom", span.GetAttribute("data-bs-placement"));
	}

	[TestMethod]
	public void HxTooltip_EmptyText_DoesNotRenderSpanWrapper()
	{
		// Act
		var cut = RenderComponent<HxTooltip>(p => p
			.AddChildContent("Just content"));

		// Assert - no span wrapper should be rendered, only the child content
		var spans = cut.FindAll("span");
		Assert.IsEmpty(spans, "No span wrapper should be rendered when Text is empty.");
		Assert.Contains("Just content", cut.Markup);
	}

	[TestMethod]
	public void HxTooltip_ChildContent_IsRenderedInsideSpan()
	{
		// Act
		var cut = RenderComponent<HxTooltip>(p => p
			.Add(t => t.Text, "Tooltip text")
			.AddChildContent("<em>Styled content</em>"));

		// Assert
		var span = cut.Find("span");
		Assert.Contains("<em>Styled content</em>", span.InnerHtml);
	}

	[TestMethod]
	public void HxTooltip_SpanWrapper_HasInlineBlockClass()
	{
		// Act
		var cut = RenderComponent<HxTooltip>(p => p
			.Add(t => t.Text, "Tooltip")
			.AddChildContent("Content"));

		// Assert
		var span = cut.Find("span");
		Assert.IsTrue(span.ClassList.Contains("d-inline-block"));
	}

	[TestMethod]
	public void HxTooltip_WrapperCssClass_IsApplied()
	{
		// Act
		var cut = RenderComponent<HxTooltip>(p => p
			.Add(t => t.Text, "Tooltip")
			.Add(t => t.WrapperCssClass, "my-wrapper")
			.AddChildContent("Content"));

		// Assert
		var span = cut.Find("span");
		Assert.IsTrue(span.ClassList.Contains("my-wrapper"));
	}
}
