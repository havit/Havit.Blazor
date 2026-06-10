namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxMenuTests : BunitTestBase
{
	[Fact]
	public void HxMenu_RendersMenuElementWithContent()
	{
		// Act
		var cut = RenderComponent<HxMenu>(p => p
			.Add(m => m.Content, "Content"));

		// Assert
		var menu = cut.Find("div.menu");
		Assert.Contains("Content", menu.TextContent);
	}

	[Fact]
	public void HxMenu_CssClass_IsAppliedToMenuElement()
	{
		// Act
		var cut = RenderComponent<HxMenu>(p => p
			.Add(m => m.CssClass, "my-menu")
			.Add(m => m.Content, "Content"));

		// Assert
		var menu = cut.Find("div.menu");
		Assert.True(menu.ClassList.Contains("my-menu"));
	}

	[Fact]
	public void HxMenu_ToggleAndMenu_AreRenderedAsSiblings()
	{
		// Act
		var cut = RenderComponent<HxMenu>(p => p
			.Add<HxMenuToggleElement>(m => m.Toggle, toggle => toggle
				.AddChildContent("Toggle"))
			.Add(m => m.Content, "Content"));

		// Assert
		var toggle = cut.Find("span[data-bs-toggle='menu']");
		var menu = cut.Find("div.menu");
		Assert.Equal(menu, toggle.NextElementSibling);
	}

	[Fact]
	public void HxMenuToggleElement_RendersDataBsToggle()
	{
		// Act
		var cut = RenderComponent<HxMenu>(p => p
			.Add<HxMenuToggleElement>(m => m.Toggle, toggle => toggle
				.AddChildContent("Toggle"))
			.Add(m => m.Content, "Content"));

		// Assert
		var toggle = cut.Find("span[data-bs-toggle='menu']");
		Assert.NotNull(toggle);
		Assert.Equal("false", toggle.GetAttribute("aria-expanded"));
	}

	[Theory]
	[InlineData(MenuPlacement.Top, "top")]
	[InlineData(MenuPlacement.BottomEnd, "bottom-end")]
	[InlineData(MenuPlacement.End, "end")]
	[InlineData(MenuPlacement.StartStart, "start-start")]
	public void HxMenuToggleElement_Placement_RendersDataBsPlacement(MenuPlacement placement, string expectedAttributeValue)
	{
		// Act
		var cut = RenderComponent<HxMenu>(p => p
			.Add(m => m.Placement, placement)
			.Add<HxMenuToggleElement>(m => m.Toggle, toggle => toggle
				.AddChildContent("Toggle"))
			.Add(m => m.Content, "Content"));

		// Assert
		var toggle = cut.Find("span[data-bs-toggle='menu']");
		Assert.Equal(expectedAttributeValue, toggle.GetAttribute("data-bs-placement"));
	}

	[Fact]
	public void HxMenuToggleElement_DefaultPlacement_DoesNotRenderDataBsPlacement()
	{
		// Act
		var cut = RenderComponent<HxMenu>(p => p
			.Add<HxMenuToggleElement>(m => m.Toggle, toggle => toggle
				.AddChildContent("Toggle"))
			.Add(m => m.Content, "Content"));

		// Assert
		var toggle = cut.Find("span[data-bs-toggle='menu']");
		Assert.False(toggle.HasAttribute("data-bs-placement"));
	}

	[Fact]
	public void HxMenuItem_RendersCorrectStructure()
	{
		// Act
		var cut = RenderComponent<HxMenu>(p => p
			.Add<HxMenuItem>(m => m.Content, item => item
				.AddChildContent("Item text")));

		// Assert
		var button = cut.Find("button.menu-item");
		Assert.Equal("button", button.GetAttribute("type"));
		Assert.Contains("Item text", button.TextContent);
	}

	[Fact]
	public void HxMenuItem_EnabledFalse_HasDisabledCssClass()
	{
		// Act
		var cut = RenderComponent<HxMenu>(p => p
			.Add<HxMenuItem>(m => m.Content, item => item
				.Add(i => i.Enabled, false)
				.AddChildContent("Disabled item")));

		// Assert
		var button = cut.Find("button.menu-item");
		Assert.True(button.ClassList.Contains("disabled"), "Disabled item should have 'disabled' CSS class.");
		Assert.True(button.HasAttribute("disabled"), "Disabled item should have the 'disabled' attribute.");
	}

	[Fact]
	public void HxMenuItem_Color_RendersThemeCssClass()
	{
		// Act
		var cut = RenderComponent<HxMenu>(p => p
			.Add<HxMenuItem>(m => m.Content, item => item
				.Add(i => i.Color, ThemeColor.Danger)
				.AddChildContent("Remove")));

		// Assert
		var button = cut.Find("button.menu-item");
		Assert.True(button.ClassList.Contains("theme-danger"));
	}

	[Fact]
	public void HxMenuDivider_RendersDividerStructure()
	{
		// Act
		var cut = RenderComponent<HxMenu>(p => p
			.Add<HxMenuDivider>(m => m.Content));

		// Assert
		var hr = cut.Find("hr.menu-divider");
		Assert.NotNull(hr);
	}

	[Fact]
	public void HxMenuHeader_RendersHeaderStructure()
	{
		// Act
		var cut = RenderComponent<HxMenu>(p => p
			.Add<HxMenuHeader>(m => m.Content, header => header
				.AddChildContent("Section title")));

		// Assert
		var h6 = cut.Find("h6.menu-header");
		Assert.Contains("Section title", h6.TextContent);
	}

	[Fact]
	public void HxMenuText_RendersMenuTextStructure()
	{
		// Act
		var cut = RenderComponent<HxMenu>(p => p
			.Add<HxMenuText>(m => m.Content, text => text
				.AddChildContent("Plain text")));

		// Assert
		var span = cut.Find("span.menu-text");
		Assert.Contains("Plain text", span.TextContent);
	}
}
