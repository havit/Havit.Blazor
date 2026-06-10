namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxContextMenuTests : BunitTestBase
{
	[Fact]
	public void HxContextMenu_Render_HasContextMenuStructure()
	{
		// Act
		var cut = RenderComponent<HxContextMenu>(p => p
			.AddChildContent<HxContextMenuItem>(item => item
				.Add(i => i.Text, "Action 1")));

		// Assert
		var contextMenu = cut.Find("div.hx-context-menu");
		Assert.NotNull(contextMenu);

		var triggerButton = cut.Find("div.hx-context-menu-btn");
		Assert.NotNull(triggerButton);
		Assert.Equal("menu", triggerButton.GetAttribute("data-bs-toggle"));
		Assert.Equal("button", triggerButton.GetAttribute("role"));
	}

	[Fact]
	public void HxContextMenu_RendersMenu()
	{
		// Act
		var cut = RenderComponent<HxContextMenu>(p => p
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
		var cut = RenderComponent<HxContextMenu>(p => p
			.AddChildContent<HxContextMenuItem>(item => item
				.Add(i => i.Text, "Edit")));

		// Assert
		var a = cut.Find("a.menu-item.hx-context-menu-item");
		Assert.NotNull(a);
		Assert.Contains("Edit", a.TextContent);
	}

	[Fact]
	public void HxContextMenuItem_Disabled_HasDisabledCssClass()
	{
		// Act
		var cut = RenderComponent<HxContextMenu>(p => p
			.AddChildContent<HxContextMenuItem>(item => item
				.Add(i => i.Text, "Disabled action")
				.Add(i => i.Enabled, false)));

		// Assert
		var a = cut.Find("a.menu-item");
		Assert.True(a.ClassList.Contains("disabled"), "Disabled menu item should have 'disabled' CSS class.");
	}

	[Fact]
	public void HxContextMenuItem_Enabled_NoDisabledCssClass()
	{
		// Act
		var cut = RenderComponent<HxContextMenu>(p => p
			.AddChildContent<HxContextMenuItem>(item => item
				.Add(i => i.Text, "Active action")
				.Add(i => i.Enabled, true)));

		// Assert
		var a = cut.Find("a.menu-item");
		Assert.False(a.ClassList.Contains("disabled"), "Enabled menu item should not have 'disabled' CSS class.");
	}

	[Fact]
	public void HxContextMenu_MultipleItems_RendersAll()
	{
		// Act
		var cut = RenderComponent<HxContextMenu>(p => p
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
		var items = cut.FindAll("a.hx-context-menu-item");
		Assert.Equal(3, items.Count());
	}
}
