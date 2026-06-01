using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxOffcanvasTests : BunitTestBase
{
	[Fact]
	public void HxOffcanvas_RenderModeAlways_RendersStructure()
	{
		// Act
		var cut = RenderComponent<HxOffcanvas>(p => p
			.Add(o => o.RenderMode, OffcanvasRenderMode.Always)
			.Add(o => o.Title, "Test Title")
			.Add(o => o.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body content"))));

		// Assert
		var offcanvas = cut.Find("div.offcanvas");
		Assert.NotNull(offcanvas);

		var header = cut.Find("div.offcanvas-header");
		Assert.NotNull(header);

		var title = cut.Find("h5.offcanvas-title");
		Assert.NotNull(title);
		Assert.Equal("Test Title", title.TextContent);

		var body = cut.Find("div.offcanvas-body");
		Assert.NotNull(body);
		Assert.Contains("Body content", body.TextContent);
	}

	[Fact]
	public void HxOffcanvas_PlacementStart_HasCorrectCssClass()
	{
		// Act
		var cut = RenderComponent<HxOffcanvas>(p => p
			.Add(o => o.RenderMode, OffcanvasRenderMode.Always)
			.Add(o => o.Placement, OffcanvasPlacement.Start)
			.Add(o => o.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body"))));

		// Assert
		var offcanvas = cut.Find("div.offcanvas");
		Assert.True(offcanvas.ClassList.Contains("offcanvas-start"));
	}

	[Theory]
	[InlineData(OffcanvasPlacement.End, "offcanvas-end")]
	[InlineData(OffcanvasPlacement.Start, "offcanvas-start")]
	[InlineData(OffcanvasPlacement.Top, "offcanvas-top")]
	[InlineData(OffcanvasPlacement.Bottom, "offcanvas-bottom")]
	public void HxOffcanvas_Placement_AllDirections(OffcanvasPlacement placement, string expectedCss)
	{
		// Act
		var cut = RenderComponent<HxOffcanvas>(p => p
			.Add(o => o.RenderMode, OffcanvasRenderMode.Always)
			.Add(o => o.Placement, placement)
			.Add(o => o.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body"))));

		// Assert
		var offcanvas = cut.Find("div.offcanvas");
		Assert.True(offcanvas.ClassList.Contains(expectedCss), $"Expected CSS class '{expectedCss}' for placement {placement}.");
	}

	[Fact]
	public void HxOffcanvas_ShowCloseButtonTrue_RendersCloseButton()
	{
		// Act
		var cut = RenderComponent<HxOffcanvas>(p => p
			.Add(o => o.RenderMode, OffcanvasRenderMode.Always)
			.Add(o => o.ShowCloseButton, true)
			.Add(o => o.Title, "Title")
			.Add(o => o.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body"))));

		// Assert
		var closeBtn = cut.Find("button.btn-close");
		Assert.NotNull(closeBtn);
		Assert.Equal("offcanvas", closeBtn.GetAttribute("data-bs-dismiss"));
	}

	[Fact]
	public void HxOffcanvas_ShowCloseButtonFalse_NoCloseButton()
	{
		// Act
		var cut = RenderComponent<HxOffcanvas>(p => p
			.Add(o => o.RenderMode, OffcanvasRenderMode.Always)
			.Add(o => o.ShowCloseButton, false)
			.Add(o => o.Title, "Title")
			.Add(o => o.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body"))));

		// Assert
		Assert.Empty(cut.FindAll("button.btn-close"));
	}

	[Fact]
	public void HxOffcanvas_FooterTemplate_RendersFooter()
	{
		// Act
		var cut = RenderComponent<HxOffcanvas>(p => p
			.Add(o => o.RenderMode, OffcanvasRenderMode.Always)
			.Add(o => o.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body")))
			.Add(o => o.FooterTemplate, (RenderFragment)(b => b.AddContent(0, "Footer content"))));

		// Assert
		var footer = cut.Find("div.hx-offcanvas-footer");
		Assert.NotNull(footer);
		Assert.Contains("Footer content", footer.TextContent);
	}

	[Fact]
	public void HxOffcanvas_SizeLarge_HasSizeCssClass()
	{
		// Act
		var cut = RenderComponent<HxOffcanvas>(p => p
			.Add(o => o.RenderMode, OffcanvasRenderMode.Always)
			.Add(o => o.Size, OffcanvasSize.Large)
			.Add(o => o.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body"))));

		// Assert
		var offcanvas = cut.Find("div.offcanvas");
		Assert.True(offcanvas.ClassList.Contains("hx-offcanvas-lg"));
	}

	[Fact]
	public void HxOffcanvas_RenderModeOpenOnly_DoesNotRenderContentWhenClosed()
	{
		// Act - default RenderMode is OpenOnly
		var cut = RenderComponent<HxOffcanvas>(p => p
			.Add(o => o.Title, "Title")
			.Add(o => o.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body"))));

		// Assert - the offcanvas div exists but header/body should not be rendered
		var offcanvas = cut.Find("div.offcanvas");
		Assert.NotNull(offcanvas);
		Assert.Empty(cut.FindAll("div.offcanvas-header"));
	}
}
