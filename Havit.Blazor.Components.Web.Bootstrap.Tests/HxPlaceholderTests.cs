namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

[TestClass]
public class HxPlaceholderTests : BunitTestBase
{
	[TestMethod]
	public void HxPlaceholder_Render_OutputsPlaceholderElement()
	{
		// Act
		var cut = RenderComponent<HxPlaceholder>();

		// Assert
		var element = cut.Find("span");
		Assert.IsTrue(element.ClassList.Contains("placeholder"));
	}

	[TestMethod]
	public void HxPlaceholderContainer_AnimationGlow_AppliesGlowClass()
	{
		// Act
		var cut = RenderComponent<HxPlaceholderContainer>(parameters => parameters
			.Add(p => p.Animation, PlaceholderAnimation.Glow)
			.AddChildContent("<span>content</span>"));

		// Assert
		var element = cut.Find("span");
		Assert.IsTrue(element.ClassList.Contains("placeholder-glow"));
	}

	[TestMethod]
	public void HxPlaceholderContainer_AnimationWave_AppliesWaveClass()
	{
		// Act
		var cut = RenderComponent<HxPlaceholderContainer>(parameters => parameters
			.Add(p => p.Animation, PlaceholderAnimation.Wave)
			.AddChildContent("<span>content</span>"));

		// Assert
		var element = cut.Find("span");
		Assert.IsTrue(element.ClassList.Contains("placeholder-wave"));
	}

	[TestMethod]
	public void HxPlaceholderButton_Render_HasDisabledAppearance()
	{
		// Act
		var cut = RenderComponent<HxPlaceholderButton>();

		// Assert
		var button = cut.Find("button");
		Assert.IsTrue(button.ClassList.Contains("placeholder"));
		Assert.IsTrue(button.HasAttribute("disabled"), "Button should have the disabled attribute.");
	}
}
