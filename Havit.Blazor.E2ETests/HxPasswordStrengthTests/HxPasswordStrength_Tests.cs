namespace Havit.Blazor.E2ETests.HxPasswordStrengthTests;

public class HxPasswordStrength_Tests : TestAppTestBase
{
	[Fact]
	public async Task HxPasswordStrength_Typing_SetsTextColorReadByBootstrapCss()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxPasswordStrength");

		var input = Page.Locator("input[type='password']");
		var text = Page.Locator(".strength-text");

		// Act
		await input.FillAsync("abc");

		// Assert - the text is colored by --bs-strength-color (Bootstrap sets the unused --strength-color only)
		await Expect(text).ToHaveTextAsync("Weak");
		string strengthColor = await text.EvaluateAsync<string>("element => element.style.getPropertyValue('--bs-strength-color')");
		Assert.Equal("var(--bs-danger-fg)", strengthColor);
	}

	[Fact]
	public async Task HxPasswordStrength_MinLengthChanged_ReevaluatesWithNewOptions()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxPasswordStrength");

		var input = Page.Locator("input[type='password']");
		var output = Page.Locator("[data-testid='strength-output']");

		// minLength 8: minLength + lowercase = 2 points (weak)
		await input.FillAsync("abcdefgh");
		await Expect(output).ToHaveTextAsync("Weak");

		// Act - minLength 4: minLength + extraLength + lowercase = 3 points (fair)
		await Page.Locator("[data-testid='min-length-4']").ClickAsync();

		// Assert
		await Expect(output).ToHaveTextAsync("Fair");
		await Expect(Page.Locator(".strength")).ToHaveAttributeAsync("data-bs-strength", "fair");
		await Expect(Page.Locator(".strength-text")).ToHaveTextAsync("Fair");
	}

	[Fact]
	public async Task HxPasswordStrength_SwitchStaticAndDynamic_RendersConsistentMeter()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxPasswordStrength");

		var input = Page.Locator("input[type='password']");
		var meter = Page.Locator(".strength");
		var text = Page.Locator(".strength-text");

		await input.FillAsync("abc");
		await Expect(meter).ToHaveAttributeAsync("data-bs-strength", "weak");

		// Act - static display
		await Page.Locator("[data-testid='static-strong']").ClickAsync();

		// Assert
		await Expect(meter).ToHaveAttributeAsync("data-bs-strength", "strong");
		await Expect(meter.Locator(".strength-segment.active")).ToHaveCountAsync(4);
		await Expect(text).ToHaveTextAsync("Strong");

		// typing does not affect the static display (the plugin is disposed)
		await input.FillAsync("abcd");
		await Expect(meter).ToHaveAttributeAsync("data-bs-strength", "strong");

		// Act - back to the dynamic mode (the plugin is re-created and evaluates the current value)
		await Page.Locator("[data-testid='dynamic']").ClickAsync();

		// Assert
		await Expect(meter).ToHaveAttributeAsync("data-bs-strength", "weak");
		await Expect(meter.Locator(".strength-segment.active")).ToHaveCountAsync(1);
		await Expect(text).ToHaveTextAsync("Weak");
	}
}
