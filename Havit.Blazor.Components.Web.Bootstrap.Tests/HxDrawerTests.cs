using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxDrawerTests : BunitTestBase
{
	[Fact]
	public void HxDrawer_RenderModeAlways_RendersStructure()
	{
		// Act
		var cut = Render<HxDrawer>(p => p
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
		var cut = Render<HxDrawer>(p => p
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
		var cut = Render<HxDrawer>(p => p
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
		var cut = Render<HxDrawer>(p => p
			.Add(o => o.RenderMode, DrawerRenderMode.Always)
			.Add(o => o.ShowCloseButton, true)
			.Add(o => o.Title, "Title")
			.Add(o => o.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body"))));

		// Assert
		var closeBtn = cut.Find("button.btn-close");
		Assert.NotNull(closeBtn);
		// The close button is wired via @onclick (single close owner, transition tracking); it intentionally carries no data-bs-dismiss.
		Assert.Null(closeBtn.GetAttribute("data-bs-dismiss"));
		Assert.Null(closeBtn.GetAttribute("data-bs-target"));
	}

	[Fact]
	public void HxDrawer_CloseButtonIcon_RendersKeyboardAccessibleButton()
	{
		// Act
		var cut = Render<HxDrawer>(p => p
			.Add(o => o.RenderMode, DrawerRenderMode.Always)
			.Add(o => o.CloseButtonIcon, BootstrapIcon.XLg)
			.Add(o => o.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body"))));

		// Assert
		var closeBtn = cut.Find("div.drawer-header > button.btn-close");
		Assert.Equal("button", closeBtn.GetAttribute("type"));
		Assert.Equal("Close", closeBtn.GetAttribute("aria-label"));
		Assert.NotNull(closeBtn.QuerySelector("i, svg"));
		Assert.Empty(cut.FindAll("div[role=button]"));
	}

	[Fact]
	public void HxDrawer_Title_LabelsTheDrawer()
	{
		// Act
		var cut = Render<HxDrawer>(p => p
			.Add(o => o.RenderMode, DrawerRenderMode.Always)
			.Add(o => o.Title, "Filters"));

		// Assert
		var drawer = cut.Find("dialog.drawer");
		var title = cut.Find("h5.drawer-title");
		Assert.False(String.IsNullOrEmpty(title.Id));
		Assert.Equal(title.Id, drawer.GetAttribute("aria-labelledby"));
	}

	[Fact]
	public void HxDrawer_NoTitle_NoAriaLabelledBy()
	{
		// Act
		var cut = Render<HxDrawer>(p => p
			.Add(o => o.RenderMode, DrawerRenderMode.Always));

		// Assert
		Assert.Null(cut.Find("dialog.drawer").GetAttribute("aria-labelledby"));
	}

	[Fact]
	public void HxDrawer_AriaLabelledByFromAdditionalAttributes_PreservedWithoutTitle()
	{
		// Act - heading supplied via HeaderTemplate, labelled by the caller
		var cut = Render<HxDrawer>(p => p
			.Add(o => o.RenderMode, DrawerRenderMode.Always)
			.Add(o => o.HeaderTemplate, (RenderFragment)(b => b.AddMarkupContent(0, "<h2 id=\"custom-heading\">Custom</h2>")))
			.AddUnmatched("aria-labelledby", "custom-heading"));

		// Assert
		Assert.Equal("custom-heading", cut.Find("dialog.drawer").GetAttribute("aria-labelledby"));
	}

	[Fact]
	public void HxNavbarDrawer_PlacementFullscreen_RendersDrawerFullscreen()
	{
		// Act
		var cut = Render<HxNavbar>(p => p
			.Add(o => o.ChildContent, (RenderFragment)(b =>
			{
				b.OpenComponent<HxNavbarDrawer>(0);
				b.AddAttribute(1, nameof(HxNavbarDrawer.Placement), DrawerPlacement.Fullscreen);
				b.CloseComponent();
			})));

		// Assert
		Assert.Contains("drawer-fullscreen", cut.Find("dialog.drawer").ClassList);
	}

	[Fact]
	public void HxDrawer_Variants_RenderCssClasses()
	{
		// Act
		var cut = Render<HxDrawer>(p => p
			.Add(o => o.Placement, DrawerPlacement.Fullscreen)
			.Add(o => o.Instant, true)
			.Add(o => o.Translucent, true));

		// Assert
		var drawer = cut.Find("dialog.drawer");
		Assert.Contains("drawer-fullscreen", drawer.ClassList);
		Assert.Contains("drawer-instant", drawer.ClassList);
		Assert.Contains("drawer-translucent", drawer.ClassList);
	}

	[Fact]
	public void HxDrawer_AdditionalAttributes_SplattedOnDialog()
	{
		// Act
		var cut = Render<HxDrawer>(p => p
			.AddUnmatched("data-bs-theme", "dark")
			.AddUnmatched("id", "my-drawer"));

		// Assert
		var drawer = cut.Find("dialog.drawer");
		Assert.Equal("dark", drawer.GetAttribute("data-bs-theme"));
		Assert.Equal("my-drawer", drawer.Id);
	}

	[Fact]
	public async Task HxDrawer_ShowAsync_DoesNotRenderShowClass()
	{
		// Arrange
		var cut = Render<HxDrawer>(p => p
			.Add(o => o.Title, "Title"));

		// Act
		await cut.InvokeAsync(() => cut.Instance.ShowAsync());

		// Assert - Bootstrap 6 drawer state is the [open] attribute, there is no .show class
		Assert.DoesNotContain("show", cut.Find("dialog.drawer").ClassList);
	}

	[Fact]
	public async Task HxDrawer_HideAsyncDuringShowTransition_IsDeferredUntilShown()
	{
		// Arrange
		var module = JSInterop.SetupModule("./_content/Havit.Blazor.Components.Web.Bootstrap/HxDrawer.js");
		module.Mode = JSRuntimeMode.Loose;
		var cut = Render<HxDrawer>(p => p
			.Add(o => o.Title, "Title"));

		// Act
		await cut.InvokeAsync(() => cut.Instance.ShowAsync());
		await cut.InvokeAsync(() => cut.Instance.HideAsync()); // the show transition is still in progress

		// Assert - Bootstrap would silently drop hide() during the transition, so it must not be sent yet
		module.VerifyInvoke("show", 1);
		module.VerifyNotInvoke("hide");

		// Act - the show transition completes
		await cut.InvokeAsync(() => cut.Instance.HandleDrawerShown());

		// Assert
		module.VerifyInvoke("hide", 1);
	}

	[Fact]
	public async Task HxDrawer_CloseButtonClickDuringShowTransition_IsDeferredUntilShown()
	{
		// Arrange
		var module = JSInterop.SetupModule("./_content/Havit.Blazor.Components.Web.Bootstrap/HxDrawer.js");
		module.Mode = JSRuntimeMode.Loose;
		var cut = Render<HxDrawer>(p => p
			.Add(o => o.Title, "Title"));
		await cut.InvokeAsync(() => cut.Instance.ShowAsync());

		// Act
		await cut.Find("button.btn-close").ClickAsync(new());

		// Assert
		module.VerifyNotInvoke("hide");
		await cut.InvokeAsync(() => cut.Instance.HandleDrawerShown());
		module.VerifyInvoke("hide", 1);
	}

	[Fact]
	public async Task HxDrawer_ShowAsyncDuringHideTransition_IsDeferredUntilHidden()
	{
		// Arrange
		var module = JSInterop.SetupModule("./_content/Havit.Blazor.Components.Web.Bootstrap/HxDrawer.js");
		module.Mode = JSRuntimeMode.Loose;
		var cut = Render<HxDrawer>(p => p
			.Add(o => o.Title, "Title"));
		await cut.InvokeAsync(() => cut.Instance.ShowAsync());
		await cut.InvokeAsync(() => cut.Instance.HandleDrawerShown());

		// Act
		await cut.InvokeAsync(() => cut.Instance.HideAsync());
		await cut.InvokeAsync(() => cut.Instance.ShowAsync()); // the hide transition is still in progress

		// Assert
		module.VerifyInvoke("hide", 1);
		module.VerifyInvoke("show", 1);

		// Act - the hide transition completes
		await cut.InvokeAsync(() => cut.Instance.HandleDrawerHidden());

		// Assert - the deferred show is sent
		module.VerifyInvoke("show", 2);
	}

	[Fact]
	public void HxDrawer_ShowCloseButtonFalse_NoCloseButton()
	{
		// Act
		var cut = Render<HxDrawer>(p => p
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
		var cut = Render<HxDrawer>(p => p
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
		var cut = Render<HxDrawer>(p => p
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
		var cut = Render<HxDrawer>(p => p
			.Add(o => o.Title, "Title")
			.Add(o => o.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body"))));

		// Assert - the drawer div exists but header/body should not be rendered
		var drawer = cut.Find("dialog.drawer");
		Assert.NotNull(drawer);
		Assert.Empty(cut.FindAll("div.drawer-header"));
	}
}
