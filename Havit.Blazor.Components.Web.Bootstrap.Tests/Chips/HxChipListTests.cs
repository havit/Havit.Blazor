namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Chips;

[TestClass]
public class HxChipListTests : BunitTestBase
{
	[TestMethod]
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
		Assert.HasCount(3, cut.FindAll(".badge"), "Expected three chip badges to be rendered.");
	}

	[TestMethod]
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

		Assert.HasCount(3, cut.FindAll(".badge"), "Expected three chip badges before removal.");
		Assert.HasCount(2, cut.FindAll(".hx-chip-list-remove-btn"), "Expected two remove buttons before removal.");

		// Act – click the first remove button (the "Name: Peter" chip)
		await cut.InvokeAsync(() => cut.FindAll(".hx-chip-list-remove-btn")[0].Click());

		// Verify that the remove callback was invoked with the expected chip ("Name: Peter")
		Assert.IsNotNull(removedChip, "Expected OnChipRemoveClick to be invoked and provide a chip to remove.");
		Assert.AreSame(chips[1], removedChip, "Expected the removed chip to be the \"Name: Peter\" chip.");
		// Simulate the parent updating the chips list in response to the callback
		chips.Remove(removedChip);
		cut.SetParametersAndRender(p => p.Add(x => x.Chips, chips));

		// Assert – one chip badge disappeared
		Assert.HasCount(2, cut.FindAll(".badge"), "Expected two chip badges after removal.");
	}

	[TestMethod]
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
		Assert.IsNotNull(removedChip, "Expected OnChipRemoveClick to be invoked and provide a chip to remove.");
		Assert.AreSame(chips[1], removedChip, "Expected the removed chip to be the \"Name: Peter\" chip.");

		// Simulate the parent updating the chips list in response to the callback
		chips.Remove(removedChip);
		cut.SetParametersAndRender(p => p.Add(x => x.Chips, chips));

		// Assert – remaining badges contain the expected content; "Name: Peter" is gone
		var badgeTexts = cut.FindAll(".badge")
			.Select(b => b.TextContent.Trim())
			.ToList();

		Assert.HasCount(2, badgeTexts, "Expected two remaining chip badges.");
		Assert.Contains(t => t.Contains("State: Active"), badgeTexts, "Expected 'State: Active' chip to remain.");
		Assert.Contains(t => t.Contains("Company: HAVIT"), badgeTexts, "Expected 'Company: HAVIT' chip to remain.");
		Assert.DoesNotContain(t => t.Contains("Name: Peter"), badgeTexts, "Expected 'Name: Peter' chip to be removed.");
	}
}
