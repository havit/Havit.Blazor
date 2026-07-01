namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxAlertTests : BunitTestBase
{
	[Fact]
	public void HxAlert_Color_RendersCorrectCssClass()
	{
		// Act
		var cut = RenderComponent<HxAlert>(p => p
			.Add(a => a.Color, ThemeColor.Primary)
			.AddChildContent("Test alert"));

		// Assert
		var div = cut.Find("div.alert");
		Assert.True(div.ClassList.Contains("alert-primary"));
	}

	[Theory]
	[InlineData(ThemeColor.Primary, "alert-primary")]
	[InlineData(ThemeColor.Secondary, "alert-secondary")]
	[InlineData(ThemeColor.Success, "alert-success")]
	[InlineData(ThemeColor.Danger, "alert-danger")]
	[InlineData(ThemeColor.Warning, "alert-warning")]
	[InlineData(ThemeColor.Info, "alert-info")]
	[InlineData(ThemeColor.Light, "alert-light")]
	[InlineData(ThemeColor.Dark, "alert-dark")]
	public void HxAlert_Color_AllThemeColors(ThemeColor color, string expectedCss)
	{
		// Act
		var cut = RenderComponent<HxAlert>(p => p
			.Add(a => a.Color, color)
			.AddChildContent("Content"));

		// Assert
		var div = cut.Find("div.alert");
		Assert.True(div.ClassList.Contains(expectedCss), $"Expected CSS class '{expectedCss}' for color {color}.");
	}

	[Fact]
	public void HxAlert_ChildContent_IsRendered()
	{
		// Act
		var cut = RenderComponent<HxAlert>(p => p
			.Add(a => a.Color, ThemeColor.Info)
			.AddChildContent("<strong>Hello</strong> World"));

		// Assert
		var div = cut.Find("div.alert");
		Assert.Contains("<strong>Hello</strong>", div.InnerHtml);
		Assert.Contains("World", div.InnerHtml);
	}

	[Fact]
	public void HxAlert_Dismissible_AddsCloseButton()
	{
		// Act
		var cut = RenderComponent<HxAlert>(p => p
			.Add(a => a.Color, ThemeColor.Warning)
			.Add(a => a.Dismissible, true)
			.AddChildContent("Dismissible alert"));

		// Assert
		var div = cut.Find("div.alert");
		Assert.True(div.ClassList.Contains("alert-dismissible"), "Should have alert-dismissible class.");
		Assert.True(div.ClassList.Contains("fade"), "Should have fade class.");
		Assert.True(div.ClassList.Contains("show"), "Should have show class.");

		var closeButton = cut.Find("button.btn-close");
		Assert.NotNull(closeButton);
		Assert.Equal("alert", closeButton.GetAttribute("data-bs-dismiss"));
	}

	[Fact]
	public void HxAlert_NotDismissible_NoCloseButton()
	{
		// Act
		var cut = RenderComponent<HxAlert>(p => p
			.Add(a => a.Color, ThemeColor.Success)
			.Add(a => a.Dismissible, false)
			.AddChildContent("Non-dismissible alert"));

		// Assert
		var div = cut.Find("div.alert");
		Assert.False(div.ClassList.Contains("alert-dismissible"));
		Assert.Empty(cut.FindAll("button.btn-close"));
	}

	[Fact]
	public void HxAlert_HasRoleAttribute()
	{
		// Act
		var cut = RenderComponent<HxAlert>(p => p
			.Add(a => a.Color, ThemeColor.Primary)
			.AddChildContent("Alert"));

		// Assert
		var div = cut.Find("div.alert");
		Assert.Equal("alert", div.GetAttribute("role"));
	}

	[Fact]
	public void HxAlert_CssClass_IsApplied()
	{
		// Act
		var cut = RenderComponent<HxAlert>(p => p
			.Add(a => a.Color, ThemeColor.Primary)
			.Add(a => a.CssClass, "my-custom-class")
			.AddChildContent("Alert"));

		// Assert
		var div = cut.Find("div.alert");
		Assert.True(div.ClassList.Contains("my-custom-class"));
	}
}
