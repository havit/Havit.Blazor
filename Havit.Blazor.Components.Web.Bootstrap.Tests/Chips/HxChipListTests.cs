namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Chips;

public class HxChipListTests : BunitTestBase
{
	[Fact]
	public void HxChipList_Render_DisplaysChipBadges()
	{
		// Arrange
		var chips = new[]
		{
			new ChipItem { ChipTemplate = b => b.AddMarkupContent(0, "State: Active") },
			new ChipItem { ChipTemplate = b => b.AddMarkupContent(0, "Name: Peter"), Removable = true },
			new ChipItem { ChipTemplate = b => b.AddMarkupContent(0, "Company: HAVIT"), Removable = true },
		};

		// Act
		var cut = RenderComponent<HxChipList>(parameters => parameters
			.Add(p => p.Chips, chips));

		// Assert – three badge elements are rendered
		Assert.Equal(3, cut.FindAll(".badge").Count());
	}

	[Fact]
	public async Task HxChipList_RemoveChip_ChipDisappears()
	{
		// Arrange
		var chips = new List<ChipItem>
		{
			new ChipItem { ChipTemplate = b => b.AddMarkupContent(0, "State: Active") },
			new ChipItem { ChipTemplate = b => b.AddMarkupContent(0, "Name: Peter"), Removable = true },
			new ChipItem { ChipTemplate = b => b.AddMarkupContent(0, "Company: HAVIT"), Removable = true },
		};
		ChipItem removedChip = null;

		var cut = RenderComponent<HxChipList>(parameters => parameters
			.Add(p => p.Chips, chips)
			.Add(p => p.OnChipRemoveClick, (ChipItem chip) => { removedChip = chip; }));

		Assert.Equal(3, cut.FindAll(".badge").Count());
		Assert.Equal(2, cut.FindAll(".hx-chip-list-remove-btn").Count());

		// Act – click the first remove button (the "Name: Peter" chip)
		await cut.InvokeAsync(() => cut.FindAll(".hx-chip-list-remove-btn")[0].Click());

		// Verify that the remove callback was invoked with the expected chip ("Name: Peter")
		Assert.NotNull(removedChip);
		Assert.Same(chips[1], removedChip);
		// Simulate the parent updating the chips list in response to the callback
		chips.Remove(removedChip);
		cut.SetParametersAndRender(p => p.Add(x => x.Chips, chips));

		// Assert – one chip badge disappeared
		Assert.Equal(2, cut.FindAll(".badge").Count());
	}

	[Fact]
	public async Task HxChipList_AfterRemoval_RemainingChipsCorrect()
	{
		// Arrange
		var chips = new List<ChipItem>
		{
			new ChipItem { ChipTemplate = b => b.AddMarkupContent(0, "State: Active") },
			new ChipItem { ChipTemplate = b => b.AddMarkupContent(0, "Name: Peter"), Removable = true },
			new ChipItem { ChipTemplate = b => b.AddMarkupContent(0, "Company: HAVIT"), Removable = true },
		};
		ChipItem removedChip = null;

		var cut = RenderComponent<HxChipList>(parameters => parameters
			.Add(p => p.Chips, chips)
			.Add(p => p.OnChipRemoveClick, (ChipItem chip) => { removedChip = chip; }));

		// Act – remove "Name: Peter" chip (first removable / first remove button)
		await cut.InvokeAsync(() => cut.FindAll(".hx-chip-list-remove-btn")[0].Click());

		// Verify that the remove callback was invoked with the expected chip ("Name: Peter")
		Assert.NotNull(removedChip);
		Assert.Same(chips[1], removedChip);

		// Simulate the parent updating the chips list in response to the callback
		chips.Remove(removedChip);
		cut.SetParametersAndRender(p => p.Add(x => x.Chips, chips));

		// Assert – remaining badges contain the expected content; "Name: Peter" is gone
		var badgeTexts = cut.FindAll(".badge")
			.Select(b => b.TextContent.Trim())
			.ToList();

		Assert.Equal(2, badgeTexts.Count());
		Assert.Contains(badgeTexts, t => t.Contains("State: Active"));
		Assert.Contains(badgeTexts, t => t.Contains("Company: HAVIT"));
		Assert.DoesNotContain(badgeTexts, t => t.Contains("Name: Peter"));
	}
}
