using System.Globalization;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Navigation;

public class HxNavbarDrawerTests : BunitTestBase
{
	[Fact]
	public void HxNavbarDrawer_WithoutTitle_HasLocalizedAriaLabel()
	{
		using var culture = CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo("en-US"));

		// Act
		var cut = Render<HxNavbar>(parameters => parameters
			.AddChildContent<HxNavbarDrawer>(drawer => drawer
				.AddChildContent("Content")));

		// Assert
		var dialog = cut.Find("dialog.drawer");
		Assert.Equal("Navigation", dialog.GetAttribute("aria-label"));
		Assert.Null(dialog.GetAttribute("aria-labelledby"));
		Assert.Equal("Close", cut.Find(".drawer-header .btn-close").GetAttribute("aria-label"));
	}

	[Fact]
	public void HxNavbarDrawer_WithTitle_IsLabelledByTitle()
	{
		// Act
		var cut = Render<HxNavbar>(parameters => parameters
			.AddChildContent<HxNavbarDrawer>(drawer => drawer
				.Add(d => d.Title, "Menu")
				.AddChildContent("Content")));

		// Assert
		var dialog = cut.Find("dialog.drawer");
		Assert.Null(dialog.GetAttribute("aria-label"));
		var title = cut.Find(".drawer-title");
		Assert.Equal(title.Id, dialog.GetAttribute("aria-labelledby"));
		Assert.Equal("Menu", title.TextContent);
	}

	[Fact]
	public void HxNavbarDrawer_CloseButtonLabel_IsLocalized()
	{
		using var culture = CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo("cs-CZ"));

		// Act
		var cut = Render<HxNavbar>(parameters => parameters
			.AddChildContent<HxNavbarDrawer>(drawer => drawer
				.AddChildContent("Content")));

		// Assert
		Assert.Equal("Zavřít", cut.Find(".drawer-header .btn-close").GetAttribute("aria-label"));
		Assert.Equal("Navigace", cut.Find("dialog.drawer").GetAttribute("aria-label"));
	}
}
