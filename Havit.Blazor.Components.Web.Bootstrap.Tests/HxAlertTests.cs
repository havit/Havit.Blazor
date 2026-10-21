namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxAlertTests : BunitTestBase
{
	[Fact]
	public void HxAlert_Color_RendersCorrectCssClass()
	{
		// Act
		var cut = Render<HxAlert>(p => p
			.Add(a => a.Color, ThemeColor.Primary)
			.AddChildContent("Test alert"));

		// Assert
		var div = cut.Find("div.alert");
		Assert.True(div.ClassList.Contains("theme-primary"));
	}

	[Theory]
	[InlineData(ThemeColor.Primary, "theme-primary")]
	[InlineData(ThemeColor.Secondary, "theme-secondary")]
	[InlineData(ThemeColor.Success, "theme-success")]
	[InlineData(ThemeColor.Danger, "theme-danger")]
	[InlineData(ThemeColor.Warning, "theme-warning")]
	[InlineData(ThemeColor.Info, "theme-info")]
	[InlineData(ThemeColor.Accent, "theme-accent")]
	[InlineData(ThemeColor.Inverse, "theme-inverse")]
	public void HxAlert_Color_AllThemeColors(ThemeColor color, string expectedCss)
	{
		// Act
		var cut = Render<HxAlert>(p => p
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
		var cut = Render<HxAlert>(p => p
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
		var cut = Render<HxAlert>(p => p
			.Add(a => a.Color, ThemeColor.Warning)
			.Add(a => a.Dismissible, true)
			.AddChildContent("Dismissible alert"));

		// Assert
		var div = cut.Find("div.alert");
		// Bootstrap 6 removed .alert-dismissible and fades alerts out without .fade/.show
		Assert.False(div.ClassList.Contains("alert-dismissible"));
		Assert.False(div.ClassList.Contains("fade"));
		Assert.False(div.ClassList.Contains("show"));

		var closeButton = cut.Find("button.btn-close");
		Assert.NotNull(closeButton);
		Assert.Equal("alert", closeButton.GetAttribute("data-bs-dismiss"));
	}

	[Fact]
	public void HxAlert_NotDismissible_NoCloseButton()
	{
		// Act
		var cut = Render<HxAlert>(p => p
			.Add(a => a.Color, ThemeColor.Success)
			.Add(a => a.Dismissible, false)
			.AddChildContent("Non-dismissible alert"));

		// Assert
		var div = cut.Find("div.alert");
		Assert.Empty(cut.FindAll("button.btn-close"));
	}

	[Fact]
	public void HxAlert_HasRoleAttribute()
	{
		// Act
		var cut = Render<HxAlert>(p => p
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
		var cut = Render<HxAlert>(p => p
			.Add(a => a.Color, ThemeColor.Primary)
			.Add(a => a.CssClass, "my-custom-class")
			.AddChildContent("Alert"));

		// Assert
		var div = cut.Find("div.alert");
		Assert.True(div.ClassList.Contains("my-custom-class"));
	}

	[Fact]
	public void HxAlert_NoColor_RendersNeutralAlert()
	{
		// Act
		var cut = Render<HxAlert>(p => p
			.AddChildContent("<p>Neutral alert</p>"));

		// Assert
		var div = cut.Find("div.alert");
		Assert.Equal(new[] { "alert" }, div.ClassList.ToArray());
	}

	[Fact]
	public void HxAlert_NotDismissible_DoesNotImportJsModule()
	{
		// Act
		Render<HxAlert>(p => p
			.AddChildContent("<p>Alert</p>"));

		// Assert
		Assert.DoesNotContain(JSInterop.Invocations, invocation => invocation.Identifier == "import");
	}

	[Fact]
	public void HxAlert_Dismissible_InitializesJsModule()
	{
		// Act
		Render<HxAlert>(p => p
			.Add(a => a.Dismissible, true)
			.Add(a => a.OnClosed, () => { })
			.AddChildContent("<p>Alert</p>"));

		// Assert
		var import = JSInterop.VerifyInvoke("import");
		Assert.Contains("HxAlert.js", (string)import.Arguments[0]);
		Assert.Contains(JSInterop.Invocations, invocation => invocation.Identifier == "initialize");
	}

	[Fact]
	public async Task HxAlert_HandleJsClosed_InvokesOnClosed()
	{
		// Arrange
		int closedCount = 0;
		var cut = Render<HxAlert>(p => p
			.Add(a => a.Dismissible, true)
			.Add(a => a.OnClosed, () => closedCount++)
			.AddChildContent("<p>Alert</p>"));

		// Act
		await cut.InvokeAsync(() => cut.Instance.HandleJsClosed());

		// Assert
		Assert.Equal(1, closedCount);
	}
}
