using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxDrawerTests : BunitTestBase
{
	[Fact]
	public void HxDrawer_RenderModeAlways_RendersStructure()
	{
		// Act
		var cut = RenderComponent<HxDrawer>(p => p
			.Add(o => o.RenderMode, DrawerRenderMode.Always)
			.Add(o => o.Title, "Test Title")
			.Add(o => o.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body content"))));

		// Assert
		var drawer = cut.Find("dialog.drawer");
		Assert.NotNull(drawer);

		var header = cut.Find("div.drawer-header");
		Assert.NotNull(header);

		var title = cut.Find("h5.drawer-title");
		Assert.NotNull(title);
		Assert.Equal("Test Title", title.TextContent);

		var body = cut.Find("div.drawer-body");
		Assert.NotNull(body);
		Assert.Contains("Body content", body.TextContent);
	}

	[Fact]
	public void HxDrawer_PlacementStart_HasCorrectCssClass()
	{
		// Act
		var cut = RenderComponent<HxDrawer>(p => p
			.Add(o => o.RenderMode, DrawerRenderMode.Always)
			.Add(o => o.Placement, DrawerPlacement.Start)
			.Add(o => o.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body"))));

		// Assert
		var drawer = cut.Find("dialog.drawer");
		Assert.True(drawer.ClassList.Contains("drawer-start"));
	}

	[Theory]
	[InlineData(DrawerPlacement.End, "drawer-end")]
	[InlineData(DrawerPlacement.Start, "drawer-start")]
	[InlineData(DrawerPlacement.Top, "drawer-top")]
	[InlineData(DrawerPlacement.Bottom, "drawer-bottom")]
	public void HxDrawer_Placement_AllDirections(DrawerPlacement placement, string expectedCss)
	{
		// Act
		var cut = RenderComponent<HxDrawer>(p => p
			.Add(o => o.RenderMode, DrawerRenderMode.Always)
			.Add(o => o.Placement, placement)
			.Add(o => o.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body"))));

		// Assert
		var drawer = cut.Find("dialog.drawer");
		Assert.True(drawer.ClassList.Contains(expectedCss), $"Expected CSS class '{expectedCss}' for placement {placement}.");
	}

	[Fact]
	public void HxDrawer_ShowCloseButtonTrue_RendersCloseButton()
	{
		// Act
		var cut = RenderComponent<HxDrawer>(p => p
			.Add(o => o.RenderMode, DrawerRenderMode.Always)
			.Add(o => o.ShowCloseButton, true)
			.Add(o => o.Title, "Title")
			.Add(o => o.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body"))));

		// Assert
		var closeBtn = cut.Find("button.btn-close");
		Assert.NotNull(closeBtn);
		Assert.Equal("drawer", closeBtn.GetAttribute("data-bs-dismiss"));
	}

	[Fact]
	public void HxDrawer_ShowCloseButtonFalse_NoCloseButton()
	{
		// Act
		var cut = RenderComponent<HxDrawer>(p => p
			.Add(o => o.RenderMode, DrawerRenderMode.Always)
			.Add(o => o.ShowCloseButton, false)
			.Add(o => o.Title, "Title")
			.Add(o => o.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body"))));

		// Assert
		Assert.Empty(cut.FindAll("button.btn-close"));
	}

	[Fact]
	public void HxDrawer_FooterTemplate_RendersFooter()
	{
		// Act
		var cut = RenderComponent<HxDrawer>(p => p
			.Add(o => o.RenderMode, DrawerRenderMode.Always)
			.Add(o => o.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body")))
			.Add(o => o.FooterTemplate, (RenderFragment)(b => b.AddContent(0, "Footer content"))));

		// Assert
		var footer = cut.Find("div.drawer-footer");
		Assert.NotNull(footer);
		Assert.Contains("Footer content", footer.TextContent);
	}

	[Fact]
	public void HxDrawer_SizeLarge_HasSizeCssClass()
	{
		// Act
		var cut = RenderComponent<HxDrawer>(p => p
			.Add(o => o.RenderMode, DrawerRenderMode.Always)
			.Add(o => o.Size, DrawerSize.Large)
			.Add(o => o.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body"))));

		// Assert
		var drawer = cut.Find("dialog.drawer");
		Assert.True(drawer.ClassList.Contains("hx-drawer-lg"));
	}

	[Fact]
	public void HxDrawer_RenderModeOpenOnly_DoesNotRenderContentWhenClosed()
	{
		// Act - default RenderMode is OpenOnly
		var cut = RenderComponent<HxDrawer>(p => p
			.Add(o => o.Title, "Title")
			.Add(o => o.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body"))));

		// Assert - the drawer div exists but header/body should not be rendered
		var drawer = cut.Find("dialog.drawer");
		Assert.NotNull(drawer);
		Assert.Empty(cut.FindAll("div.drawer-header"));
	}
}
