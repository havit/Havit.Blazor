using Microsoft.Extensions.DependencyInjection;

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

	[Fact]
	public void HxToast_AdditionalAttributes_RenderedOnToast()
	{
		// Act
		var cut = Render<HxToast>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Success)
			.Add(p => p.ContentText, "Content")
			.AddUnmatched("data-bs-theme", "dark")
			.AddUnmatched("data-test", "toast"));

		// Assert - Bootstrap 6 dark toast: data-bs-theme="dark" on the .toast element
		var toast = cut.Find("div.toast");
		Assert.Equal("dark", toast.GetAttribute("data-bs-theme"));
		Assert.Equal("toast", toast.GetAttribute("data-test"));
		Assert.Contains("theme-success", toast.ClassList);
	}

	[Fact]
	public void HxToast_AdditionalAttributes_ClassDoesNotOverrideComponentCss()
	{
		// Act
		var cut = Render<HxToast>(parameters => parameters
			.Add(p => p.ContentText, "Content")
			.AddUnmatched("class", "foo"));

		// Assert
		Assert.Contains("toast", cut.Find("div.toast").ClassList);
	}

	[Fact]
	public void HxToastContainer_AdditionalAttributes_RenderedOnContainer()
	{
		// Act
		var cut = Render<HxToastContainer>(parameters => parameters
			.Add(p => p.Position, ToastContainerPosition.BottomEnd)
			.AddUnmatched("data-bs-theme", "dark"));

		// Assert
		var container = cut.Find("div.toast-container");
		Assert.Equal("dark", container.GetAttribute("data-bs-theme"));
		Assert.Contains("hx-toast-container-fixed", container.ClassList);
	}

	[Fact]
	public void HxMessenger_AdditionalAttributes_RenderedOnToastContainer()
	{
		// Act
		var cut = Render<HxMessenger>(parameters => parameters
			.AddUnmatched("data-bs-theme", "dark"));

		// Assert
		Assert.Equal("dark", cut.Find("div.toast-container").GetAttribute("data-bs-theme"));
		Assert.Null(cut.Find("div.hx-messenger").GetAttribute("data-bs-theme"));
	}

	[Fact]
	public void HxMessenger_MessageAdditionalAttributes_RenderedOnToast()
	{
		// Arrange
		var cut = Render<HxMessenger>();
		var messenger = Services.GetRequiredService<IHxMessengerService>();

		// Act
		messenger.AddMessage(new BootstrapMessengerMessage
		{
			Color = ThemeColor.Primary,
			Text = "Dark message",
			AdditionalAttributes = new Dictionary<string, object> { ["data-bs-theme"] = "dark" }
		});
		messenger.AddMessage(new BootstrapMessengerMessage
		{
			Color = ThemeColor.Primary,
			Text = "Regular message"
		});

		// Assert
		cut.WaitForAssertion(() => Assert.Equal(2, cut.FindAll("div.toast").Count));
		var toasts = cut.FindAll("div.toast");
		Assert.Equal("dark", toasts[0].GetAttribute("data-bs-theme"));
		Assert.Null(toasts[1].GetAttribute("data-bs-theme"));
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
