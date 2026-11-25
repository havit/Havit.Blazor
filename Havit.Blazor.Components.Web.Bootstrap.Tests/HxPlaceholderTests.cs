namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxPlaceholderTests : BunitTestBase
{
	[Fact]
	public void HxPlaceholder_Render_OutputsPlaceholderElement()
	{
		// Act
		var cut = Render<HxPlaceholder>();

		// Assert
		var element = cut.Find("span");
		Assert.True(element.ClassList.Contains("placeholder"));
	}

	[Fact]
	public void HxPlaceholderContainer_AnimationPulse_AppliesPulseClass()
	{
		// Act
		var cut = Render<HxPlaceholderContainer>(parameters => parameters
			.Add(p => p.Animation, PlaceholderAnimation.Pulse)
			.AddChildContent("<span>content</span>"));

		// Assert
		var element = cut.Find("span");
		Assert.True(element.ClassList.Contains("placeholder-pulse"));
	}

	[Fact]
	public void HxPlaceholderContainer_AnimationGlow_AppliesPulseClass()
	{
		// Act
#pragma warning disable CS0618 // Type or member is obsolete
		var cut = Render<HxPlaceholderContainer>(parameters => parameters
			.Add(p => p.Animation, PlaceholderAnimation.Glow)
			.AddChildContent("<span>content</span>"));
#pragma warning restore CS0618 // Type or member is obsolete

		// Assert
		var element = cut.Find("span");
		Assert.True(element.ClassList.Contains("placeholder-pulse"));
	}

	[Fact]
	public void HxPlaceholderContainer_Active_RendersAriaHidden()
	{
		// Act
		var cut = Render<HxPlaceholderContainer>(parameters => parameters
			.AddChildContent("<span>content</span>"));

		// Assert
		Assert.Equal("true", cut.Find("span").GetAttribute("aria-hidden"));
	}

	[Fact]
	public void HxPlaceholderContainer_Inactive_DoesNotRenderAriaHidden()
	{
		// Act
		var cut = Render<HxPlaceholderContainer>(parameters => parameters
			.Add(p => p.Active, false)
			.AddChildContent("<span>content</span>"));

		// Assert
		Assert.False(cut.Find("span").HasAttribute("aria-hidden"));
	}

	[Fact]
	public void HxPlaceholderContainer_AriaHidden_CanBeOverridden()
	{
		// Act
		var cut = Render<HxPlaceholderContainer>(parameters => parameters
			.AddUnmatched("aria-hidden", "false")
			.AddChildContent("<span>content</span>"));

		// Assert
		Assert.Equal("false", cut.Find("span").GetAttribute("aria-hidden"));
	}

	[Fact]
	public void HxPlaceholderContainer_AnimationWave_AppliesWaveClass()
	{
		// Act
		var cut = Render<HxPlaceholderContainer>(parameters => parameters
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
		var cut = Render<HxPlaceholderButton>();

		// Assert
		var button = cut.Find("button");
		Assert.True(button.ClassList.Contains("placeholder"));
		Assert.True(button.HasAttribute("disabled"), "Button should have the disabled attribute.");
	}
}
