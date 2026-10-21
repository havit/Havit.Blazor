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

	[Fact]
	public async Task HxMenuToggleButton_ChangeReferenceAndStrategyWhileOpen_KeepsMenuOpenAndClosable()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxMenu_Reference");

		var toggle = Page.Locator("[data-testid='switch-toggle']");
		var menu = Page.Locator("[data-testid='switch-wrapper'] .menu");

		await toggle.ClickAsync();
		await Expect(menu).ToBeVisibleAsync(new() { Timeout = MenuAnimationTimeout });

		// Act - change MenuReference and MenuStrategy while the menu is open
		await Page.Locator("[data-testid='switch-button']").ClickAsync();

		// Assert - the re-created instance uses the new config and keeps positioning the open menu
		await Expect(menu).ToHaveCSSAsync("position", "fixed", new() { Timeout = MenuAnimationTimeout });
		var reference = await toggle.EvaluateAsync<string>("element => String(bootstrap.Menu.getInstance(element)._config.reference)");
		Assert.Equal("parent", reference);
		await Expect(menu).ToBeVisibleAsync();

		// Act - click outside
		await Page.Locator("[data-testid='outside-click-area']").ClickAsync();

		// Assert - the re-created instance still closes on outside click
		await Expect(menu).Not.ToBeVisibleAsync(new() { Timeout = MenuAnimationTimeout });
	}
}
