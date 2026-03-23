namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Alerts;

[TestClass]
public class HxAlertTests : BunitTestBase
{
	[TestMethod]
	public void HxAlert_Color_Primary_ShouldRenderCorrectCssClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxAlert>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Primary)
			.Add(p => p.ChildContent, builder => builder.AddMarkupContent(0, "Alert content"))
		);

		// Assert
		var alert = cut.Find("div.alert");
		Assert.IsTrue(alert.ClassList.Contains("alert-primary"));
	}

	[TestMethod]
	public void HxAlert_Color_Danger_ShouldRenderCorrectCssClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxAlert>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Danger)
			.Add(p => p.ChildContent, builder => builder.AddMarkupContent(0, "Alert content"))
		);

		// Assert
		var alert = cut.Find("div.alert");
		Assert.IsTrue(alert.ClassList.Contains("alert-danger"));
	}

	[TestMethod]
	public void HxAlert_Color_Success_ShouldRenderCorrectCssClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxAlert>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Success)
			.Add(p => p.ChildContent, builder => builder.AddMarkupContent(0, "Alert content"))
		);

		// Assert
		var alert = cut.Find("div.alert");
		Assert.IsTrue(alert.ClassList.Contains("alert-success"));
	}

	[TestMethod]
	public void HxAlert_WithoutDismissible_ShouldNotRenderCloseButton()
	{
		// Arrange & Act
		var cut = RenderComponent<HxAlert>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Primary)
			.Add(p => p.Dismissible, false)
			.Add(p => p.ChildContent, builder => builder.AddMarkupContent(0, "Alert content"))
		);

		// Assert
		Assert.IsFalse(cut.Markup.Contains("btn-close"));
	}

	[TestMethod]
	public void HxAlert_WithDismissible_ShouldRenderCloseButton()
	{
		// Arrange & Act
		var cut = RenderComponent<HxAlert>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Warning)
			.Add(p => p.Dismissible, true)
			.Add(p => p.ChildContent, builder => builder.AddMarkupContent(0, "Alert content"))
		);

		// Assert
		var closeButton = cut.Find("button.btn-close");
		Assert.IsNotNull(closeButton);
		Assert.AreEqual("alert", closeButton.GetAttribute("data-bs-dismiss"));
	}

	[TestMethod]
	public void HxAlert_WithDismissible_ShouldRenderDismissibleCssClasses()
	{
		// Arrange & Act
		var cut = RenderComponent<HxAlert>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Info)
			.Add(p => p.Dismissible, true)
			.Add(p => p.ChildContent, builder => builder.AddMarkupContent(0, "Alert content"))
		);

		// Assert
		var alert = cut.Find("div.alert");
		Assert.IsTrue(alert.ClassList.Contains("alert-dismissible"));
		Assert.IsTrue(alert.ClassList.Contains("fade"));
		Assert.IsTrue(alert.ClassList.Contains("show"));
	}

	[TestMethod]
	public void HxAlert_ShouldRenderChildContent()
	{
		// Arrange & Act
		var cut = RenderComponent<HxAlert>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Primary)
			.Add(p => p.ChildContent, builder => builder.AddMarkupContent(0, "Test alert message"))
		);

		// Assert
		Assert.IsTrue(cut.Markup.Contains("Test alert message"));
	}

	[TestMethod]
	public void HxAlert_CssClass_ShouldBeApplied()
	{
		// Arrange & Act
		var cut = RenderComponent<HxAlert>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Secondary)
			.Add(p => p.CssClass, "custom-alert-class")
			.Add(p => p.ChildContent, builder => builder.AddMarkupContent(0, "Alert content"))
		);

		// Assert
		var alert = cut.Find("div.alert");
		Assert.IsTrue(alert.ClassList.Contains("custom-alert-class"));
	}

	[TestMethod]
	public void HxAlert_ShouldRenderRoleAlert()
	{
		// Arrange & Act
		var cut = RenderComponent<HxAlert>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Primary)
			.Add(p => p.ChildContent, builder => builder.AddMarkupContent(0, "Alert content"))
		);

		// Assert
		var alert = cut.Find("div.alert");
		Assert.AreEqual("alert", alert.GetAttribute("role"));
	}
}
