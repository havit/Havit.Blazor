namespace Havit.Blazor.E2ETests.HxInputNumberTests;

public class HxInputNumber_SmartKeyboardNegative_Tests : TestAppTestBase
{
	private const int DefaultTimeout = 5_000;

	/// <summary>
	/// Repro for https://github.com/havit/Havit.Blazor/issues/1142
	/// When the user focuses an HxInputNumber with SelectOnFocus (default), the existing value gets selected.
	/// Pressing '-' to make the number negative and then typing a replacement digit must preserve the negative sign,
	/// not produce a positive number.
	/// </summary>
	[Fact]
	public async Task HxInputNumber_PressMinusThenDigit_OnFullySelectedValue_ProducesNegativeNumber()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxInputNumber_SmartKeyboardNegative");

		var wrapper = Page.Locator("[data-testid='input-number-wrapper']");
		var input = wrapper.Locator("input");
		var currentValue = Page.Locator("[data-testid='current-value']");

		// Initial state: model is 123
		await Expect(currentValue).ToHaveTextAsync("123", new() { Timeout = DefaultTimeout });

		// Act — focus the input (SelectOnFocus selects the whole "123"), then press '-' then '5', then blur
		await input.FocusAsync();

		// Sanity-check the auto-select behavior: after focus the entire value should be selected
		var selectionLengthAfterFocus = await input.EvaluateAsync<int>("el => el.selectionEnd - el.selectionStart");
		Assert.Equal(3, selectionLengthAfterFocus);

		await input.PressAsync("-");
		await input.PressAsync("5");
		await input.BlurAsync();

		// Assert — the model should be -5 (negative), not 5 (positive)
		await Expect(currentValue).ToHaveTextAsync("-5", new() { Timeout = DefaultTimeout });
	}
}
