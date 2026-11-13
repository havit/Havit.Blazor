namespace Havit.Blazor.E2ETests.HxButtonTests;

/// <summary>
/// Bootstrap 6 .btn-link.active uses the global link hover color; HxButton keeps the theme-* color of a themed link.
/// </summary>
public class HxButton_ActiveLink_Tests : TestAppTestBase
{
	[Fact]
	public async Task HxButton_ActiveThemedLink_KeepsThemeColor()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxButton_ActiveLink");

		// Act
		var buttonColor = await GetColorAsync("active-danger-link");
		var referenceColor = await GetColorAsync("danger-emphasis-reference");

		// Assert
		Assert.Equal(referenceColor, buttonColor);
	}

	[Fact]
	public async Task HxButton_ActivePlainLink_UsesLinkHoverColor()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxButton_ActiveLink");

		// Act
		var buttonColor = await GetColorAsync("active-plain-link");
		var referenceColor = await GetColorAsync("link-hover-reference");

		// Assert
		Assert.Equal(referenceColor, buttonColor);
	}

	private Task<string> GetColorAsync(string testId)
	{
		return Page.Locator($"[data-testid='{testId}']").EvaluateAsync<string>("e => getComputedStyle(e).color");
	}
}
