namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxContextMenuTests : BunitTestBase
{
	[Fact]
	public void HxContextMenu_Render_HasContextMenuStructure()
	{
		// Act
		var cut = Render<HxContextMenu>(p => p
			.AddChildContent<HxContextMenuItem>(item => item
				.Add(i => i.Text, "Action 1")));

		// Assert
		var contextMenu = cut.Find("div.hx-context-menu");
		Assert.NotNull(contextMenu);

		var triggerButton = cut.Find("button.hx-context-menu-btn");
		Assert.NotNull(triggerButton);
		Assert.Equal("button", triggerButton.GetAttribute("type"));
		Assert.Equal("menu", triggerButton.GetAttribute("data-bs-toggle"));
		Assert.False(triggerButton.HasAttribute("data-bs-strategy"));
	}

	[Fact]
	public void HxContextMenu_RendersMenu()
	{
		// Act
		var cut = Render<HxContextMenu>(p => p
			.AddChildContent<HxContextMenuItem>(item => item
				.Add(i => i.Text, "Action 1")));

		// Assert
		var menu = cut.Find("div.menu");
		Assert.NotNull(menu);
	}

	[Fact]
	public void HxContextMenuItem_RendersCorrectStructure()
	{
		// Act
		var cut = Render<HxContextMenu>(p => p
			.AddChildContent<HxContextMenuItem>(item => item
				.Add(i => i.Text, "Edit")));

		// Assert
		var button = cut.Find("button.menu-item.hx-context-menu-item");
		Assert.Equal("button", button.GetAttribute("type"));
		Assert.Contains("Edit", button.TextContent);
	}

	[Fact]
	public void HxContextMenuItem_Disabled_HasDisabledCssClass()
	{
		// Act
		var cut = Render<HxContextMenu>(p => p
			.AddChildContent<HxContextMenuItem>(item => item
				.Add(i => i.Text, "Disabled action")
				.Add(i => i.Enabled, false)));

		// Assert
		var button = cut.Find("button.menu-item");
		Assert.True(button.ClassList.Contains("disabled"), "Disabled menu item should have 'disabled' CSS class.");
		Assert.True(button.HasAttribute("disabled"));
	}

	[Fact]
	public void HxContextMenuItem_Enabled_NoDisabledCssClass()
	{
		// Act
		var cut = Render<HxContextMenu>(p => p
			.AddChildContent<HxContextMenuItem>(item => item
				.Add(i => i.Text, "Active action")
				.Add(i => i.Enabled, true)));

		// Assert
		var button = cut.Find("button.menu-item");
		Assert.False(button.ClassList.Contains("disabled"), "Enabled menu item should not have 'disabled' CSS class.");
		Assert.False(button.HasAttribute("disabled"));
	}

	[Fact]
	public void HxContextMenu_MultipleItems_RendersAll()
	{
		// Act
		var cut = Render<HxContextMenu>(p => p
			.AddChildContent(builder =>
			{
				builder.OpenComponent<HxContextMenuItem>(0);
				builder.AddAttribute(1, nameof(HxContextMenuItem.Text), "Action 1");
				builder.CloseComponent();

				builder.OpenComponent<HxContextMenuItem>(10);
				builder.AddAttribute(11, nameof(HxContextMenuItem.Text), "Action 2");
				builder.CloseComponent();

				builder.OpenComponent<HxContextMenuItem>(20);
				builder.AddAttribute(21, nameof(HxContextMenuItem.Text), "Action 3");
				builder.CloseComponent();
			}));

		// Assert
		var items = cut.FindAll("button.hx-context-menu-item");
		Assert.Equal(3, items.Count());
	}

	[Fact]
	public void HxContextMenu_FloatingStrategyFixed_RendersDataBsStrategy()
	{
		// Act
		var cut = Render<HxContextMenu>(p => p
			.Add(cm => cm.FloatingStrategy, FloatingStrategy.Fixed)
			.AddChildContent<HxContextMenuItem>(item => item
				.Add(i => i.Text, "Action 1")));

		// Assert
		var triggerButton = cut.Find(".hx-context-menu-btn");
		Assert.Equal("fixed", triggerButton.GetAttribute("data-bs-strategy"));
		Assert.False(triggerButton.HasAttribute("data-bs-floating-config"));
	}

	[Fact]
	public void HxContextMenuItem_WithHref_RendersLink()
	{
		// Act
		var cut = Render<HxContextMenu>(p => p
			.AddChildContent<HxContextMenuItem>(item => item
				.Add(i => i.Text, "Detail")
				.Add(i => i.Href, "/detail")));

		// Assert
		var a = cut.Find("a.menu-item.hx-context-menu-item");
		Assert.Equal("/detail", a.GetAttribute("href"));
		Assert.False(a.HasAttribute("aria-disabled"));
		Assert.False(a.HasAttribute("tabindex"));
	}

	[Fact]
	public void HxContextMenuItem_WithHrefDisabled_HasAriaDisabled()
	{
		// Act
		var cut = Render<HxContextMenu>(p => p
			.AddChildContent<HxContextMenuItem>(item => item
				.Add(i => i.Text, "Detail")
				.Add(i => i.Href, "/detail")
				.Add(i => i.Enabled, false)));

		// Assert
		var a = cut.Find("a.menu-item");
		Assert.True(a.ClassList.Contains("disabled"));
		Assert.Equal("true", a.GetAttribute("aria-disabled"));
		Assert.Equal("-1", a.GetAttribute("tabindex"));
		Assert.False(a.HasAttribute("role"));
	}

	[Fact]
	public void HxContextMenuItem_Icon_RendersMenuItemIconSlot()
	{
		// Act
		var cut = Render<HxContextMenu>(p => p
			.AddChildContent<HxContextMenuItem>(item => item
				.Add(i => i.Text, "Edit")
				.Add(i => i.Icon, BootstrapIcon.Alarm)));

		// Assert
		var icon = cut.Find(".hx-context-menu-item > .menu-item-icon");
		Assert.NotNull(icon);
		Assert.Empty(cut.FindAll(".hx-context-menu-item-icon"));
	}
}
