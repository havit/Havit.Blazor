using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

[TestClass]
public class HxOffcanvasTests : BunitTestBase
{
	[TestMethod]
	public void HxOffcanvas_RenderModeAlways_RendersStructure()
	{
		// Act
		var cut = RenderComponent<HxOffcanvas>(p => p
			.Add(o => o.RenderMode, OffcanvasRenderMode.Always)
			.Add(o => o.Title, "Test Title")
			.Add(o => o.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body content"))));

		// Assert
		var offcanvas = cut.Find("div.offcanvas");
		Assert.IsNotNull(offcanvas);

		var header = cut.Find("div.offcanvas-header");
		Assert.IsNotNull(header);

		var title = cut.Find("h5.offcanvas-title");
		Assert.IsNotNull(title);
		Assert.AreEqual("Test Title", title.TextContent);

		var body = cut.Find("div.offcanvas-body");
		Assert.IsNotNull(body);
		Assert.Contains("Body content", body.TextContent);
	}

	[TestMethod]
	public void HxOffcanvas_PlacementStart_HasCorrectCssClass()
	{
		// Act
		var cut = RenderComponent<HxOffcanvas>(p => p
			.Add(o => o.RenderMode, OffcanvasRenderMode.Always)
			.Add(o => o.Placement, OffcanvasPlacement.Start)
			.Add(o => o.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body"))));

		// Assert
		var offcanvas = cut.Find("div.offcanvas");
		Assert.IsTrue(offcanvas.ClassList.Contains("offcanvas-start"));
	}

	[TestMethod]
	[DataRow(OffcanvasPlacement.End, "offcanvas-end")]
	[DataRow(OffcanvasPlacement.Start, "offcanvas-start")]
	[DataRow(OffcanvasPlacement.Top, "offcanvas-top")]
	[DataRow(OffcanvasPlacement.Bottom, "offcanvas-bottom")]
	public void HxOffcanvas_Placement_AllDirections(OffcanvasPlacement placement, string expectedCss)
	{
		// Act
		var cut = RenderComponent<HxOffcanvas>(p => p
			.Add(o => o.RenderMode, OffcanvasRenderMode.Always)
			.Add(o => o.Placement, placement)
			.Add(o => o.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body"))));

		// Assert
		var offcanvas = cut.Find("div.offcanvas");
		Assert.IsTrue(offcanvas.ClassList.Contains(expectedCss), $"Expected CSS class '{expectedCss}' for placement {placement}.");
	}

	[TestMethod]
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
		Assert.IsNotNull(closeBtn);
		Assert.AreEqual("offcanvas", closeBtn.GetAttribute("data-bs-dismiss"));
	}

	[TestMethod]
	public void HxOffcanvas_ShowCloseButtonFalse_NoCloseButton()
	{
		// Act
		var cut = RenderComponent<HxOffcanvas>(p => p
			.Add(o => o.RenderMode, OffcanvasRenderMode.Always)
			.Add(o => o.ShowCloseButton, false)
			.Add(o => o.Title, "Title")
			.Add(o => o.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body"))));

		// Assert
		Assert.IsEmpty(cut.FindAll("button.btn-close"));
	}

	[TestMethod]
	public void HxOffcanvas_FooterTemplate_RendersFooter()
	{
		// Act
		var cut = RenderComponent<HxOffcanvas>(p => p
			.Add(o => o.RenderMode, OffcanvasRenderMode.Always)
			.Add(o => o.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body")))
			.Add(o => o.FooterTemplate, (RenderFragment)(b => b.AddContent(0, "Footer content"))));

		// Assert
		var footer = cut.Find("div.hx-offcanvas-footer");
		Assert.IsNotNull(footer);
		Assert.Contains("Footer content", footer.TextContent);
	}

	[TestMethod]
	public void HxOffcanvas_SizeLarge_HasSizeCssClass()
	{
		// Act
		var cut = RenderComponent<HxOffcanvas>(p => p
			.Add(o => o.RenderMode, OffcanvasRenderMode.Always)
			.Add(o => o.Size, OffcanvasSize.Large)
			.Add(o => o.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body"))));

		// Assert
		var offcanvas = cut.Find("div.offcanvas");
		Assert.IsTrue(offcanvas.ClassList.Contains("hx-offcanvas-lg"));
	}

	[TestMethod]
	public void HxOffcanvas_RenderModeOpenOnly_DoesNotRenderContentWhenClosed()
	{
		// Act - default RenderMode is OpenOnly
		var cut = RenderComponent<HxOffcanvas>(p => p
			.Add(o => o.Title, "Title")
			.Add(o => o.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body"))));

		// Assert - the offcanvas div exists but header/body should not be rendered
		var offcanvas = cut.Find("div.offcanvas");
		Assert.IsNotNull(offcanvas);
		Assert.IsEmpty(cut.FindAll("div.offcanvas-header"), "Header should not be rendered when closed and RenderMode is OpenOnly.");
	}
}
