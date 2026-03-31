namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

[TestClass]
public class HxBootstrapIconTests : BunitTestBase
{
	[TestMethod]
	public void HxBootstrapIcon_Render_OutputsIconElement()
	{
		// Arrange & Act
		var cut = RenderComponent<HxIcon>(parameters => parameters
			.Add(p => p.Icon, BootstrapIcon.Alarm)
		);

		// Assert
		var element = cut.Find("i");
		Assert.IsNotNull(element);
	}

	[TestMethod]
	public void HxBootstrapIcon_Icon_AppliesCorrectCssClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxIcon>(parameters => parameters
			.Add(p => p.Icon, BootstrapIcon.Alarm)
		);

		// Assert
		var element = cut.Find("i");
		Assert.IsTrue(element.ClassList.Contains("bi-alarm"));
	}

	[TestMethod]
	public void HxBootstrapIcon_CssClass_IsApplied()
	{
		// Arrange & Act
		var cut = RenderComponent<HxIcon>(parameters => parameters
			.Add(p => p.Icon, BootstrapIcon.Alarm)
			.Add(p => p.CssClass, "my-custom-class")
		);

		// Assert
		var element = cut.Find("i");
		Assert.IsTrue(element.ClassList.Contains("my-custom-class"));
	}
}
