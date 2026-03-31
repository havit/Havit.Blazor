namespace Havit.Blazor.E2ETests.HxInputTagsTests;

[TestClass]
public class HxInputTags_EnterPropagation_Tests : TestAppTestBase
{
	[TestMethod]
	public async Task HxInputTags_EnterKey_AddsTagWithoutFormSubmit()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxInputTags_EnterPropagation");

		var input = Page.Locator("input[type='text']");
		var formSubmitted = Page.Locator("[data-testid='form-submitted']");
		var tagsOutput = Page.Locator("[data-testid='tags-output']");

		// Verify initial state - form not submitted
		await Expect(formSubmitted).ToHaveTextAsync("False");

		// Act — type a tag and press Enter
		await input.ClickAsync();
		await input.FillAsync("TestTag");
		await input.PressAsync("Enter");

		// Assert — the tag should be added
		await Expect(tagsOutput.Locator("[data-testid='tag-item']")).ToHaveCountAsync(1);
		await Expect(tagsOutput.Locator("[data-testid='tag-item']").First).ToHaveTextAsync("TestTag");

		// Assert — the form should NOT have been submitted
		await Expect(formSubmitted).ToHaveTextAsync("False");
	}

	[TestMethod]
	public async Task HxInputTags_MultipleEnters_NeverSubmitsForm()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxInputTags_EnterPropagation");

		var input = Page.Locator("input[type='text']");
		var formSubmitted = Page.Locator("[data-testid='form-submitted']");
		var tagsOutput = Page.Locator("[data-testid='tags-output']");

		// Act — add multiple tags via Enter key
		await input.ClickAsync();

		await input.FillAsync("Tag1");
		await input.PressAsync("Enter");

		await input.FillAsync("Tag2");
		await input.PressAsync("Enter");

		await input.FillAsync("Tag3");
		await input.PressAsync("Enter");

		// Assert — all three tags were added
		await Expect(tagsOutput.Locator("[data-testid='tag-item']")).ToHaveCountAsync(3);

		// Assert — form was never submitted
		await Expect(formSubmitted).ToHaveTextAsync("False");
	}
}
