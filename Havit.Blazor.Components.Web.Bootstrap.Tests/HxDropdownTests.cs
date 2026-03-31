namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

[TestClass]
public class HxDropdownTests : BunitTestBase
{
	[TestMethod]
	public void HxDropdown_DefaultDirection_HasDropdownCssClass()
	{
		// Act
		var cut = RenderComponent<HxDropdown>(p => p
			.AddChildContent("Content"));

		// Assert
		var div = cut.Find("div");
		Assert.IsTrue(div.ClassList.Contains("dropdown"));
	}

	[TestMethod]
	[DataRow(DropdownDirection.Down, "dropdown")]
	[DataRow(DropdownDirection.Up, "dropup")]
	[DataRow(DropdownDirection.Start, "dropstart")]
	[DataRow(DropdownDirection.End, "dropend")]
	public void HxDropdown_Direction_CorrectCssClass(DropdownDirection direction, string expectedCss)
	{
		// Act
		var cut = RenderComponent<HxDropdown>(p => p
			.Add(d => d.Direction, direction)
			.AddChildContent("Content"));

		// Assert
		var div = cut.Find("div");
		Assert.IsTrue(div.ClassList.Contains(expectedCss), $"Expected CSS class '{expectedCss}' for direction {direction}.");
	}

	[TestMethod]
	public void HxDropdown_CssClass_IsApplied()
	{
		// Act
		var cut = RenderComponent<HxDropdown>(p => p
			.Add(d => d.CssClass, "my-dropdown")
			.AddChildContent("Content"));

		// Assert
		var div = cut.Find("div");
		Assert.IsTrue(div.ClassList.Contains("my-dropdown"));
	}

	[TestMethod]
	public void HxDropdownToggleElement_RendersDataBsToggle()
	{
		// Act
		var cut = RenderComponent<HxDropdown>(p => p
			.AddChildContent<HxDropdownToggleElement>(toggle => toggle
				.AddChildContent("Toggle")));

		// Assert
		var toggle = cut.Find("span[data-bs-toggle='dropdown']");
		Assert.IsNotNull(toggle);
		Assert.AreEqual("false", toggle.GetAttribute("aria-expanded"));
	}

	[TestMethod]
	public void HxDropdownMenu_RendersDropdownMenuCssClass()
	{
		// Act
		var cut = RenderComponent<HxDropdown>(p => p
			.AddChildContent<HxDropdownMenu>(menu => menu
				.AddChildContent("Menu content")));

		// Assert
		var menu = cut.Find("ul.dropdown-menu");
		Assert.IsNotNull(menu);
	}

	[TestMethod]
	public void HxDropdownItem_RendersCorrectStructure()
	{
		// Act
		var cut = RenderComponent<HxDropdown>(p => p
			.AddChildContent<HxDropdownMenu>(menu => menu
				.AddChildContent<HxDropdownItem>(item => item
					.AddChildContent("Item text"))));

		// Assert
		var li = cut.Find("li");
		Assert.IsNotNull(li);

		var span = cut.Find("span.dropdown-item");
		Assert.IsNotNull(span);
		Assert.AreEqual("button", span.GetAttribute("role"));
		Assert.Contains("Item text", span.TextContent);
	}

	[TestMethod]
	public void HxDropdownItem_DisabledFalse_HasDisabledCssClass()
	{
		// Act
		var cut = RenderComponent<HxDropdown>(p => p
			.AddChildContent<HxDropdownMenu>(menu => menu
				.AddChildContent<HxDropdownItem>(item => item
					.Add(i => i.Enabled, false)
					.AddChildContent("Disabled item"))));

		// Assert
		var span = cut.Find("span.dropdown-item");
		Assert.IsTrue(span.ClassList.Contains("disabled"), "Disabled item should have 'disabled' CSS class.");
	}

	[TestMethod]
	public void HxDropdownDivider_RendersDividerStructure()
	{
		// Act
		var cut = RenderComponent<HxDropdown>(p => p
			.AddChildContent<HxDropdownMenu>(menu => menu
				.AddChildContent<HxDropdownDivider>()));

		// Assert
		var hr = cut.Find("hr.dropdown-divider");
		Assert.IsNotNull(hr);
	}

	[TestMethod]
	public void HxDropdownHeader_RendersHeaderStructure()
	{
		// Act
		var cut = RenderComponent<HxDropdown>(p => p
			.AddChildContent<HxDropdownMenu>(menu => menu
				.AddChildContent<HxDropdownHeader>(header => header
					.AddChildContent("Section title"))));

		// Assert
		var h6 = cut.Find("h6.dropdown-header");
		Assert.IsNotNull(h6);
		Assert.Contains("Section title", h6.TextContent);
	}

	[TestMethod]
	public void HxDropdownButtonGroup_HasBtnGroupClass()
	{
		// Act
		var cut = RenderComponent<HxDropdownButtonGroup>(p => p
			.AddChildContent("Content"));

		// Assert
		var div = cut.Find("div");
		Assert.IsTrue(div.ClassList.Contains("btn-group"), "HxDropdownButtonGroup should have 'btn-group' CSS class.");
	}
}
