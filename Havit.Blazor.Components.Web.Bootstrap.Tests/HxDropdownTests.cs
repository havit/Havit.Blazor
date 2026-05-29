namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxDropdownTests : BunitTestBase
{
	[Fact]
	public void HxDropdown_DefaultDirection_HasDropdownCssClass()
	{
		// Act
		var cut = RenderComponent<HxDropdown>(p => p
			.AddChildContent("Content"));

		// Assert
		var div = cut.Find("div");
		Assert.True(div.ClassList.Contains("dropdown"));
	}

	[Theory]
	[InlineData(DropdownDirection.Down, "dropdown")]
	[InlineData(DropdownDirection.Up, "dropup")]
	[InlineData(DropdownDirection.Start, "dropstart")]
	[InlineData(DropdownDirection.End, "dropend")]
	public void HxDropdown_Direction_CorrectCssClass(DropdownDirection direction, string expectedCss)
	{
		// Act
		var cut = RenderComponent<HxDropdown>(p => p
			.Add(d => d.Direction, direction)
			.AddChildContent("Content"));

		// Assert
		var div = cut.Find("div");
		Assert.True(div.ClassList.Contains(expectedCss), $"Expected CSS class '{expectedCss}' for direction {direction}.");
	}

	[Fact]
	public void HxDropdown_CssClass_IsApplied()
	{
		// Act
		var cut = RenderComponent<HxDropdown>(p => p
			.Add(d => d.CssClass, "my-dropdown")
			.AddChildContent("Content"));

		// Assert
		var div = cut.Find("div");
		Assert.True(div.ClassList.Contains("my-dropdown"));
	}

	[Fact]
	public void HxDropdownToggleElement_RendersDataBsToggle()
	{
		// Act
		var cut = RenderComponent<HxDropdown>(p => p
			.AddChildContent<HxDropdownToggleElement>(toggle => toggle
				.AddChildContent("Toggle")));

		// Assert
		var toggle = cut.Find("span[data-bs-toggle='dropdown']");
		Assert.NotNull(toggle);
		Assert.Equal("false", toggle.GetAttribute("aria-expanded"));
	}

	[Fact]
	public void HxDropdownMenu_RendersDropdownMenuCssClass()
	{
		// Act
		var cut = RenderComponent<HxDropdown>(p => p
			.AddChildContent<HxDropdownMenu>(menu => menu
				.AddChildContent("Menu content")));

		// Assert
		var menu = cut.Find("ul.dropdown-menu");
		Assert.NotNull(menu);
	}

	[Fact]
	public void HxDropdownItem_RendersCorrectStructure()
	{
		// Act
		var cut = RenderComponent<HxDropdown>(p => p
			.AddChildContent<HxDropdownMenu>(menu => menu
				.AddChildContent<HxDropdownItem>(item => item
					.AddChildContent("Item text"))));

		// Assert
		var li = cut.Find("li");
		Assert.NotNull(li);

		var span = cut.Find("span.dropdown-item");
		Assert.NotNull(span);
		Assert.Equal("button", span.GetAttribute("role"));
		Assert.Contains("Item text", span.TextContent);
	}

	[Fact]
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
		Assert.True(span.ClassList.Contains("disabled"), "Disabled item should have 'disabled' CSS class.");
	}

	[Fact]
	public void HxDropdownDivider_RendersDividerStructure()
	{
		// Act
		var cut = RenderComponent<HxDropdown>(p => p
			.AddChildContent<HxDropdownMenu>(menu => menu
				.AddChildContent<HxDropdownDivider>()));

		// Assert
		var hr = cut.Find("hr.dropdown-divider");
		Assert.NotNull(hr);
	}

	[Fact]
	public void HxDropdownHeader_RendersHeaderStructure()
	{
		// Act
		var cut = RenderComponent<HxDropdown>(p => p
			.AddChildContent<HxDropdownMenu>(menu => menu
				.AddChildContent<HxDropdownHeader>(header => header
					.AddChildContent("Section title"))));

		// Assert
		var h6 = cut.Find("h6.dropdown-header");
		Assert.NotNull(h6);
		Assert.Contains("Section title", h6.TextContent);
	}

	[Fact]
	public void HxDropdownButtonGroup_HasBtnGroupClass()
	{
		// Act
		var cut = RenderComponent<HxDropdownButtonGroup>(p => p
			.AddChildContent("Content"));

		// Assert
		var div = cut.Find("div");
		Assert.True(div.ClassList.Contains("btn-group"), "HxDropdownButtonGroup should have 'btn-group' CSS class.");
	}
}
