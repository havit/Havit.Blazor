namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Toasts;

public class HxToastTests : BunitTestBase
{
	[Fact]
	public void HxToast_Color_RendersThemeClassOnToastOnly()
	{
		// Act
		var cut = Render<HxToast>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Success)
			.Add(p => p.HeaderText, "Header")
			.Add(p => p.ContentText, "Content"));

		// Assert - Bootstrap 6 toast variant: theme-* on the toast, the header picks up the tokens (subtle header)
		var toast = cut.Find("div.toast");
		Assert.Contains("theme-success", toast.ClassList);
		Assert.DoesNotContain("theme-contrast", toast.ClassList);
		Assert.Equal("toast-header", cut.Find(".toast-header").ClassName);
	}

	[Theory]
	[InlineData(ThemeColor.Success)]
	[InlineData(ThemeColor.Primary)]
	[InlineData(ThemeColor.Warning)]
	public void HxToast_CloseButton_HasAccessibleNameAndNoLegacyThemeWrapper(ThemeColor color)
	{
		// Act
		var cut = Render<HxToast>(parameters => parameters
			.Add(p => p.Color, color)
			.Add(p => p.HeaderText, "Header")
			.Add(p => p.ContentText, "Content"));

		// Assert
		var closeButton = cut.Find("button.btn-close");
		Assert.Equal("Close", closeButton.GetAttribute("aria-label"));
		Assert.Equal("toast-header", closeButton.ParentElement.ClassName);
		Assert.Empty(cut.FindAll("[data-bs-theme]"));
	}

	[Fact]
	public void HxToast_CloseButtonWithoutHeader_HasAccessibleName()
	{
		// Act
		var cut = Render<HxToast>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Primary)
			.Add(p => p.ContentText, "Content"));

		// Assert
		Assert.Equal("Close", cut.Find("button.btn-close").GetAttribute("aria-label"));
		Assert.Empty(cut.FindAll("[data-bs-theme]"));
	}

	[Theory]
	[InlineData(null, "status", "polite")]
	[InlineData(ThemeColor.Success, "status", "polite")]
	[InlineData(ThemeColor.Info, "status", "polite")]
	[InlineData(ThemeColor.Warning, "alert", "assertive")]
	[InlineData(ThemeColor.Danger, "alert", "assertive")]
	public void HxToast_LiveRegion_DependsOnColor(ThemeColor? color, string expectedRole, string expectedAriaLive)
	{
		// Act
		var cut = Render<HxToast>(parameters => parameters
			.Add(p => p.Color, color)
			.Add(p => p.ContentText, "Content"));

		// Assert
		var toast = cut.Find("div.toast");
		Assert.Equal(expectedRole, toast.GetAttribute("role"));
		Assert.Equal(expectedAriaLive, toast.GetAttribute("aria-live"));
	}

	[Fact]
	public void HxToast_Instant_RendersToastInstant()
	{
		// Act
		var cut = Render<HxToast>(parameters => parameters
			.Add(p => p.Instant, true)
			.Add(p => p.ContentText, "Content"));

		// Assert
		Assert.Contains("toast-instant", cut.Find("div.toast").ClassList);
	}

	[Theory]
	[InlineData(ToastContainerPosition.TopStart, "hx-toast-container-top", "hx-toast-container-start")]
	[InlineData(ToastContainerPosition.TopCenter, "hx-toast-container-top", "hx-toast-container-center")]
	[InlineData(ToastContainerPosition.MiddleEnd, "hx-toast-container-middle", "hx-toast-container-end")]
	[InlineData(ToastContainerPosition.BottomCenter, "hx-toast-container-bottom", "hx-toast-container-center")]
	public void HxToastContainer_Position_RendersToastContainer(ToastContainerPosition position, string expectedVerticalCss, string expectedHorizontalCss)
	{
		// Act
		var cut = Render<HxToastContainer>(parameters => parameters
			.Add(p => p.Position, position));

		// Assert
		var container = cut.Find("div.hx-toast-container");
		Assert.Contains("toast-container", container.ClassList);
		Assert.Contains("hx-toast-container-fixed", container.ClassList);
		Assert.Contains(expectedVerticalCss, container.ClassList);
		Assert.Contains(expectedHorizontalCss, container.ClassList);
	}

	[Fact]
	public void HxToastContainer_PositionNone_RendersInDocumentFlow()
	{
		// Act
		var cut = Render<HxToastContainer>();

		// Assert
		var container = cut.Find("div.hx-toast-container");
		Assert.Contains("toast-container", container.ClassList);
		Assert.Contains("position-static", container.ClassList);
		Assert.DoesNotContain("hx-toast-container-fixed", container.ClassList);
	}

	[Fact]
	public void HxMessenger_RendersPoliteLiveRegion()
	{
		// Act
		var cut = Render<HxMessenger>();

		// Assert
		Assert.Equal("polite", cut.Find("div.hx-messenger").GetAttribute("aria-live"));
	}

	[Theory]
	[InlineData(ThemeColor.Primary, "theme-primary theme-contrast")]
	[InlineData(ThemeColor.Warning, "theme-warning theme-contrast")]
	[InlineData(ThemeColor.None, null)]
	public void ThemeColorExtensions_ToTextBackgroundColorCss_PaintsBackgroundAndContrastColor(ThemeColor color, string expected)
	{
		Assert.Equal(expected, color.ToTextBackgroundColorCss());
	}
}
