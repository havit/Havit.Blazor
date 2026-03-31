namespace Havit.Blazor.E2ETests.HxInputTagsTests;

[TestClass]
public class HxInputTags_Tests : TestAppTestBase
{
	[TestMethod]
	public async Task HxInputTags_TypeAndEnter_AddsTag()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxInputTags");

		var input = Page.Locator("input[type='text']");
		var tagsOutput = Page.Locator("[data-testid='tags-output']");

		// Act
		await input.ClickAsync();
		await input.FillAsync("Alpha");
		await input.PressAsync("Enter");

		// Assert
		await Expect(tagsOutput.Locator("[data-testid='tag-item']")).ToHaveCountAsync(1);
		await Expect(tagsOutput.Locator("[data-testid='tag-item']").First).ToHaveTextAsync("Alpha");
	}

	[TestMethod]
	public async Task HxInputTags_ClickRemove_RemovesTag()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxInputTags");

		var input = Page.Locator("input[type='text']");
		var tagsOutput = Page.Locator("[data-testid='tags-output']");

		// Add a tag first
		await input.ClickAsync();
		await input.FillAsync("Beta");
		await input.PressAsync("Enter");
		await Expect(tagsOutput.Locator("[data-testid='tag-item']")).ToHaveCountAsync(1);

		// Act — click the remove (×) button on the tag badge
		await Page.Locator(".hx-tag-remove-button").ClickAsync();

		// Assert
		await Expect(tagsOutput.Locator("[data-testid='tag-item']")).ToHaveCountAsync(0);
	}

	[TestMethod]
	public async Task HxInputTags_AddDuplicate_IsRejected()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxInputTags");

		var input = Page.Locator("input[type='text']");
		var tagsOutput = Page.Locator("[data-testid='tags-output']");

		// Add the tag once
		await input.ClickAsync();
		await input.FillAsync("Gamma");
		await input.PressAsync("Enter");
		await Expect(tagsOutput.Locator("[data-testid='tag-item']")).ToHaveCountAsync(1);

		// Act — try to add the same tag again
		await input.FillAsync("Gamma");
		await input.PressAsync("Enter");

		// Assert — still only one tag
		await Expect(tagsOutput.Locator("[data-testid='tag-item']")).ToHaveCountAsync(1);
	}

	[TestMethod]
	public async Task HxInputTags_AddMultiple_AllDisplayed()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxInputTags");

		var input = Page.Locator("input[type='text']");
		var tagsOutput = Page.Locator("[data-testid='tags-output']");

		// Act — add three distinct tags
		await input.ClickAsync();
		await input.FillAsync("One");
		await input.PressAsync("Enter");

		await input.FillAsync("Two");
		await input.PressAsync("Enter");

		await input.FillAsync("Three");
		await input.PressAsync("Enter");

		// Assert
		await Expect(tagsOutput.Locator("[data-testid='tag-item']")).ToHaveCountAsync(3);
		await Expect(tagsOutput.Locator("[data-testid='tag-item']").Nth(0)).ToHaveTextAsync("One");
		await Expect(tagsOutput.Locator("[data-testid='tag-item']").Nth(1)).ToHaveTextAsync("Two");
		await Expect(tagsOutput.Locator("[data-testid='tag-item']").Nth(2)).ToHaveTextAsync("Three");
	}
}
