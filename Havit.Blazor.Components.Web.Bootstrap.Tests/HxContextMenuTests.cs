namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

[TestClass]
public class HxContextMenuTests : BunitTestBase
{
	[TestMethod]
	public void HxContextMenu_Render_HasContextMenuStructure()
	{
		// Act
		var cut = RenderComponent<HxContextMenu>(p => p
			.AddChildContent<HxContextMenuItem>(item => item
				.Add(i => i.Text, "Action 1")));

		// Assert
		var contextMenu = cut.Find("div.hx-context-menu");
		Assert.IsNotNull(contextMenu);

		var dropdown = cut.Find("div.dropdown");
		Assert.IsNotNull(dropdown);

		var triggerButton = cut.Find("div.hx-context-menu-btn");
		Assert.IsNotNull(triggerButton);
		Assert.AreEqual("dropdown", triggerButton.GetAttribute("data-bs-toggle"));
		Assert.AreEqual("button", triggerButton.GetAttribute("role"));
	}

	[TestMethod]
	public void HxContextMenu_RendersDropdownMenu()
	{
		// Act
		var cut = RenderComponent<HxContextMenu>(p => p
			.AddChildContent<HxContextMenuItem>(item => item
				.Add(i => i.Text, "Action 1")));

		// Assert
		var menu = cut.Find("ul.dropdown-menu");
		Assert.IsNotNull(menu);
	}

	[TestMethod]
	public void HxContextMenuItem_RendersCorrectStructure()
	{
		// Act
		var cut = RenderComponent<HxContextMenu>(p => p
			.AddChildContent<HxContextMenuItem>(item => item
				.Add(i => i.Text, "Edit")));

		// Assert
		var li = cut.Find("li.hx-context-menu-item");
		Assert.IsNotNull(li);

		var a = cut.Find("a.dropdown-item");
		Assert.IsNotNull(a);
		Assert.Contains("Edit", a.TextContent);
	}

	[TestMethod]
	public void HxContextMenuItem_Disabled_HasDisabledCssClass()
	{
		// Act
		var cut = RenderComponent<HxContextMenu>(p => p
			.AddChildContent<HxContextMenuItem>(item => item
				.Add(i => i.Text, "Disabled action")
				.Add(i => i.Enabled, false)));

		// Assert
		var a = cut.Find("a.dropdown-item");
		Assert.IsTrue(a.ClassList.Contains("disabled"), "Disabled menu item should have 'disabled' CSS class.");
	}

	[TestMethod]
	public void HxContextMenuItem_Enabled_NoDisabledCssClass()
	{
		// Act
		var cut = RenderComponent<HxContextMenu>(p => p
			.AddChildContent<HxContextMenuItem>(item => item
				.Add(i => i.Text, "Active action")
				.Add(i => i.Enabled, true)));

		// Assert
		var a = cut.Find("a.dropdown-item");
		Assert.IsFalse(a.ClassList.Contains("disabled"), "Enabled menu item should not have 'disabled' CSS class.");
	}

	[TestMethod]
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
		var items = cut.FindAll("li.hx-context-menu-item");
		Assert.HasCount(3, items);
	}
}
