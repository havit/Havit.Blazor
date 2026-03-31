namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

[TestClass]
public class HxPopoverTests : BunitTestBase
{
	[TestMethod]
	public void HxPopover_Title_RendersDataBsToggle()
	{
		// Act
		var cut = RenderComponent<HxPopover>(p => p
			.Add(po => po.Title, "Popover title")
			.AddChildContent("Click me"));

		// Assert
		var span = cut.Find("span");
		Assert.AreEqual("popover", span.GetAttribute("data-bs-toggle"));
	}

	[TestMethod]
	public void HxPopover_Title_RendersDataBsTitle()
	{
		// Act
		var cut = RenderComponent<HxPopover>(p => p
			.Add(po => po.Title, "My Title")
			.AddChildContent("Click me"));

		// Assert
		var span = cut.Find("span");
		Assert.AreEqual("My Title", span.GetAttribute("data-bs-title"));
	}

	[TestMethod]
	public void HxPopover_Content_RendersDataBsContent()
	{
		// Act
		var cut = RenderComponent<HxPopover>(p => p
			.Add(po => po.Title, "Title")
			.Add(po => po.Content, "Popover body content")
			.AddChildContent("Click me"));

		// Assert
		var span = cut.Find("span");
		Assert.AreEqual("Popover body content", span.GetAttribute("data-bs-content"));
	}

	[TestMethod]
	public void HxPopover_Placement_RendersDataBsPlacement()
	{
		// Act
		var cut = RenderComponent<HxPopover>(p => p
			.Add(po => po.Title, "Title")
			.Add(po => po.Placement, PopoverPlacement.Left)
			.AddChildContent("Click me"));

		// Assert
		var span = cut.Find("span");
		Assert.AreEqual("left", span.GetAttribute("data-bs-placement"));
	}

	[TestMethod]
	public void HxPopover_ContentOnly_RendersSpan()
	{
		// Act - popover with content but no title should still render span
		var cut = RenderComponent<HxPopover>(p => p
			.Add(po => po.Content, "Just content")
			.AddChildContent("Click me"));

		// Assert
		var span = cut.Find("span");
		Assert.AreEqual("popover", span.GetAttribute("data-bs-toggle"));
		Assert.AreEqual("Just content", span.GetAttribute("data-bs-content"));
	}

	[TestMethod]
	public void HxPopover_EmptyTitleAndContent_NoSpanWrapper()
	{
		// Act
		var cut = RenderComponent<HxPopover>(p => p
			.AddChildContent("Just content"));

		// Assert - no span wrapper
		var spans = cut.FindAll("span");
		Assert.IsEmpty(spans, "No span should render when both Title and Content are empty.");
		Assert.Contains("Just content", cut.Markup);
	}
}
