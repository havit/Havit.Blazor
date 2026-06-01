namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxPopoverTests : BunitTestBase
{
	[Fact]
	public void HxPopover_Title_RendersDataBsToggle()
	{
		// Act
		var cut = RenderComponent<HxPopover>(p => p
			.Add(po => po.Title, "Popover title")
			.AddChildContent("Click me"));

		// Assert
		var span = cut.Find("span");
		Assert.Equal("popover", span.GetAttribute("data-bs-toggle"));
	}

	[Fact]
	public void HxPopover_Title_RendersDataBsTitle()
	{
		// Act
		var cut = RenderComponent<HxPopover>(p => p
			.Add(po => po.Title, "My Title")
			.AddChildContent("Click me"));

		// Assert
		var span = cut.Find("span");
		Assert.Equal("My Title", span.GetAttribute("data-bs-title"));
	}

	[Fact]
	public void HxPopover_Content_RendersDataBsContent()
	{
		// Act
		var cut = RenderComponent<HxPopover>(p => p
			.Add(po => po.Title, "Title")
			.Add(po => po.Content, "Popover body content")
			.AddChildContent("Click me"));

		// Assert
		var span = cut.Find("span");
		Assert.Equal("Popover body content", span.GetAttribute("data-bs-content"));
	}

	[Fact]
	public void HxPopover_Placement_RendersDataBsPlacement()
	{
		// Act
		var cut = RenderComponent<HxPopover>(p => p
			.Add(po => po.Title, "Title")
			.Add(po => po.Placement, PopoverPlacement.Left)
			.AddChildContent("Click me"));

		// Assert
		var span = cut.Find("span");
		Assert.Equal("left", span.GetAttribute("data-bs-placement"));
	}

	[Fact]
	public void HxPopover_ContentOnly_RendersSpan()
	{
		// Act - popover with content but no title should still render span
		var cut = RenderComponent<HxPopover>(p => p
			.Add(po => po.Content, "Just content")
			.AddChildContent("Click me"));

		// Assert
		var span = cut.Find("span");
		Assert.Equal("popover", span.GetAttribute("data-bs-toggle"));
		Assert.Equal("Just content", span.GetAttribute("data-bs-content"));
	}

	[Fact]
	public void HxPopover_EmptyTitleAndContent_NoSpanWrapper()
	{
		// Act
		var cut = RenderComponent<HxPopover>(p => p
			.AddChildContent("Just content"));

		// Assert - no span wrapper
		var spans = cut.FindAll("span");
		Assert.Empty(spans);
		Assert.Contains("Just content", cut.Markup);
	}
}
