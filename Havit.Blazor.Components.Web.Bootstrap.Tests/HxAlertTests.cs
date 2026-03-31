namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

[TestClass]
public class HxAlertTests : BunitTestBase
{
	[TestMethod]
	public void HxAlert_Color_RendersCorrectCssClass()
	{
		// Act
		var cut = RenderComponent<HxAlert>(p => p
			.Add(a => a.Color, ThemeColor.Primary)
			.AddChildContent("Test alert"));

		// Assert
		var div = cut.Find("div.alert");
		Assert.IsTrue(div.ClassList.Contains("alert-primary"));
	}

	[TestMethod]
	[DataRow(ThemeColor.Primary, "alert-primary")]
	[DataRow(ThemeColor.Secondary, "alert-secondary")]
	[DataRow(ThemeColor.Success, "alert-success")]
	[DataRow(ThemeColor.Danger, "alert-danger")]
	[DataRow(ThemeColor.Warning, "alert-warning")]
	[DataRow(ThemeColor.Info, "alert-info")]
	[DataRow(ThemeColor.Light, "alert-light")]
	[DataRow(ThemeColor.Dark, "alert-dark")]
	public void HxAlert_Color_AllThemeColors(ThemeColor color, string expectedCss)
	{
		// Act
		var cut = RenderComponent<HxAlert>(p => p
			.Add(a => a.Color, color)
			.AddChildContent("Content"));

		// Assert
		var div = cut.Find("div.alert");
		Assert.IsTrue(div.ClassList.Contains(expectedCss), $"Expected CSS class '{expectedCss}' for color {color}.");
	}

	[TestMethod]
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

	[TestMethod]
	public void HxAlert_Dismissible_AddsCloseButton()
	{
		// Act
		var cut = RenderComponent<HxAlert>(p => p
			.Add(a => a.Color, ThemeColor.Warning)
			.Add(a => a.Dismissible, true)
			.AddChildContent("Dismissible alert"));

		// Assert
		var div = cut.Find("div.alert");
		Assert.IsTrue(div.ClassList.Contains("alert-dismissible"), "Should have alert-dismissible class.");
		Assert.IsTrue(div.ClassList.Contains("fade"), "Should have fade class.");
		Assert.IsTrue(div.ClassList.Contains("show"), "Should have show class.");

		var closeButton = cut.Find("button.btn-close");
		Assert.IsNotNull(closeButton);
		Assert.AreEqual("alert", closeButton.GetAttribute("data-bs-dismiss"));
	}

	[TestMethod]
	public void HxAlert_NotDismissible_NoCloseButton()
	{
		// Act
		var cut = RenderComponent<HxAlert>(p => p
			.Add(a => a.Color, ThemeColor.Success)
			.Add(a => a.Dismissible, false)
			.AddChildContent("Non-dismissible alert"));

		// Assert
		var div = cut.Find("div.alert");
		Assert.IsFalse(div.ClassList.Contains("alert-dismissible"));
		Assert.IsEmpty(cut.FindAll("button.btn-close"));
	}

	[TestMethod]
	public void HxAlert_HasRoleAttribute()
	{
		// Act
		var cut = RenderComponent<HxAlert>(p => p
			.Add(a => a.Color, ThemeColor.Primary)
			.AddChildContent("Alert"));

		// Assert
		var div = cut.Find("div.alert");
		Assert.AreEqual("alert", div.GetAttribute("role"));
	}

	[TestMethod]
	public void HxAlert_CssClass_IsApplied()
	{
		// Act
		var cut = RenderComponent<HxAlert>(p => p
			.Add(a => a.Color, ThemeColor.Primary)
			.Add(a => a.CssClass, "my-custom-class")
			.AddChildContent("Alert"));

		// Assert
		var div = cut.Find("div.alert");
		Assert.IsTrue(div.ClassList.Contains("my-custom-class"));
	}
}
