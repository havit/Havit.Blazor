namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

[CollectionDefinition("Input tags defaults", DisableParallelization = true)]
public class InputTagsDefaultsCollection
{
}

[Collection("Input tags defaults")]
public class HxInputTagsDefaultsTests : BunitTestBase
{
	[Fact]
	public void HxInputTags_NoColorConfigured_RendersNoThemeClass()
	{
		// Arrange
		var tags = new List<string> { "red" };

		// Act
		var cut = Render<HxInputTags>(parameters => parameters
			.Bind(p => p.Value, tags, newValue => tags = newValue));

		// Assert — the native grayscale chip appearance (ThemeColor.None)
		Assert.DoesNotContain(cut.Find(".chip.hx-tag").ClassList, cssClass => cssClass.StartsWith("theme-"));
	}

	[Fact]
	public void HxInputTags_LegacyDefaultsTagBadgeSettingsColor_IsHonored()
	{
		// Arrange — regression: Defaults.Color must not mask the (obsolete) Defaults.TagBadgeSettings.Color,
		// whose Color is still documented as honored.
		var originalDefaults = HxInputTags.Defaults;
		try
		{
#pragma warning disable CS0618 // TagBadgeSettings is obsolete
			HxInputTags.Defaults = originalDefaults with { TagBadgeSettings = new BadgeSettings { Color = ThemeColor.Primary } };
#pragma warning restore CS0618
			var tags = new List<string> { "red" };

			// Act
			var cut = Render<HxInputTags>(parameters => parameters
				.Bind(p => p.Value, tags, newValue => tags = newValue));

			// Assert
			Assert.Contains("theme-primary", cut.Find(".chip.hx-tag").ClassList);
		}
		finally
		{
			HxInputTags.Defaults = originalDefaults;
		}
	}
}
