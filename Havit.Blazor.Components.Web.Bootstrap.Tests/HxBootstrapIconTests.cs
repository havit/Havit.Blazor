namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxBootstrapIconTests : BunitTestBase
{
	[Fact]
	public void HxBootstrapIcon_Render_OutputsIconElement()
	{
		// Arrange & Act
		var cut = RenderComponent<HxIcon>(parameters => parameters
			.Add(p => p.Icon, BootstrapIcon.Alarm)
		);

		// Assert
		var element = cut.Find("i");
		Assert.NotNull(element);
	}

	[Fact]
	public void HxBootstrapIcon_Icon_AppliesCorrectCssClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxIcon>(parameters => parameters
			.Add(p => p.Icon, BootstrapIcon.Alarm)
		);

		// Assert
		var element = cut.Find("i");
		Assert.True(element.ClassList.Contains("bi-alarm"));
	}

	[Fact]
	public void HxBootstrapIcon_CssClass_IsApplied()
	{
		// Arrange & Act
		var cut = RenderComponent<HxIcon>(parameters => parameters
			.Add(p => p.Icon, BootstrapIcon.Alarm)
			.Add(p => p.CssClass, "my-custom-class")
		);

		// Assert
		var element = cut.Find("i");
		Assert.True(element.ClassList.Contains("my-custom-class"));
	}
}
