namespace Havit.Blazor.E2ETests.HxMenuTests;

public class HxMenu_Reference_Tests : TestAppTestBase
{
	private const int MenuAnimationTimeout = 5_000;

	[Fact]
	public async Task HxMenuToggleButton_MenuReferenceParent_UsesParentReference()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxMenu_Reference");

		var toggle = Page.Locator("[data-testid='parent-toggle']");
		var menu = Page.Locator("[data-testid='parent-wrapper'] .menu");

		// Act
		await toggle.ClickAsync();
		await Expect(menu).ToBeVisibleAsync(new() { Timeout = MenuAnimationTimeout });

		// Assert - the data-bs-reference="parent" attribute must not be overridden by the JS options
		var reference = await toggle.EvaluateAsync<string>("element => String(bootstrap.Menu.getInstance(element)._config.reference)");
		Assert.Equal("parent", reference);
	}

	[Fact]
	public async Task HxMenuToggleButton_MenuReferenceDefault_UsesToggleReference()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxMenu_Reference");

		var toggle = Page.Locator("[data-testid='default-toggle']");

		// Act
		await toggle.ClickAsync();

		// Assert
		var reference = await toggle.EvaluateAsync<string>("element => String(bootstrap.Menu.getInstance(element)._config.reference)");
		Assert.Equal("toggle", reference);
	}
}
