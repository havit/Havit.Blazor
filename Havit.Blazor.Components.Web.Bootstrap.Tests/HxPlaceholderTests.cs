namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxPlaceholderTests : BunitTestBase
{
	[Fact]
	public void HxPlaceholder_Render_OutputsPlaceholderElement()
	{
		// Act
		var cut = RenderComponent<HxPlaceholder>();

		// Assert
		var element = cut.Find("span");
		Assert.True(element.ClassList.Contains("placeholder"));
	}

	[Fact]
	public void HxPlaceholderContainer_AnimationGlow_AppliesGlowClass()
	{
		// Act
		var cut = RenderComponent<HxPlaceholderContainer>(parameters => parameters
			.Add(p => p.Animation, PlaceholderAnimation.Glow)
			.AddChildContent("<span>content</span>"));

		// Assert
		var element = cut.Find("span");
		Assert.True(element.ClassList.Contains("placeholder-glow"));
	}

	[Fact]
	public void HxPlaceholderContainer_AnimationWave_AppliesWaveClass()
	{
		// Act
		var cut = RenderComponent<HxPlaceholderContainer>(parameters => parameters
			.Add(p => p.Animation, PlaceholderAnimation.Wave)
			.AddChildContent("<span>content</span>"));

		// Assert
		var element = cut.Find("span");
		Assert.True(element.ClassList.Contains("placeholder-wave"));
	}

	[Fact]
	public void HxPlaceholderButton_Render_HasDisabledAppearance()
	{
		// Act
		var cut = RenderComponent<HxPlaceholderButton>();

		// Assert
		var button = cut.Find("button");
		Assert.True(button.ClassList.Contains("placeholder"));
		Assert.True(button.HasAttribute("disabled"), "Button should have the disabled attribute.");
	}
}
