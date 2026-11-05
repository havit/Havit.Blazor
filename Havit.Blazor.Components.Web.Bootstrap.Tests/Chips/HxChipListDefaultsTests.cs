namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Chips;

[CollectionDefinition("Chip list defaults", DisableParallelization = true)]
public class ChipListDefaultsCollection
{
}

[Collection("Chip list defaults")]
public class HxChipListDefaultsTests : BunitTestBase
{
	private static readonly ChipItem[] s_chips = [new ChipItem { ChipTemplate = b => b.AddMarkupContent(0, "State: Active") }];

	[Fact]
	public void HxChipList_NoColorConfigured_RendersNoThemeClass()
	{
		// Act
		var cut = Render<HxChipList>(parameters => parameters
			.Add(p => p.Chips, s_chips));

		// Assert — the native grayscale chip appearance (ThemeColor.None)
		Assert.DoesNotContain(cut.Find(".chip").ClassList, cssClass => cssClass.StartsWith("theme-"));
	}

	[Fact]
	public void HxChipList_LegacyDefaultsChipBadgeSettingsColor_IsHonored()
	{
		// Arrange — regression: Defaults.Color must not mask the (obsolete) Defaults.ChipBadgeSettings.Color,
		// whose Color is still documented as honored.
#pragma warning disable CS0618 // ChipBadgeSettings is obsolete
		var originalChipBadgeSettings = HxChipList.Defaults.ChipBadgeSettings;
		try
		{
			HxChipList.Defaults.ChipBadgeSettings = new BadgeSettings { Color = ThemeColor.Primary };

			// Act
			var cut = Render<HxChipList>(parameters => parameters
				.Add(p => p.Chips, s_chips));

			// Assert
			Assert.Contains("theme-primary", cut.Find(".chip").ClassList);
		}
		finally
		{
			HxChipList.Defaults.ChipBadgeSettings = originalChipBadgeSettings;
		}
#pragma warning restore CS0618
	}
}
