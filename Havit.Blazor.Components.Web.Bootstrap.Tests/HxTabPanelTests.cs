namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

[TestClass]
public class HxTabPanelTests : BunitTestBase
{
	[TestMethod]
	public void HxTabPanel_OnTabDeactivated_AsyncCallback_ShouldNotCauseInfiniteLoop()
	{
		// Arrange
		int callbackCount = 0;
		string activeTabId = null;

		// Act - Render component with async OnTabDeactivated callback
		var component = RenderComponent<HxTabPanel>(parameters => parameters
			.Add(p => p.ActiveTabId, activeTabId)
			.Add(p => p.ActiveTabIdChanged, newActiveTabId => activeTabId = newActiveTabId)
			.AddChildContent<HxTab>(tab => tab
				.Add(t => t.Id, "1")
				.Add(t => t.Title, "First")
				.Add(t => t.OnTabDeactivated, async () =>
				{
					callbackCount++;
					// Simulate async work that might trigger re-render
					await Task.Yield();
				})
				.Add(t => t.Content, builder => builder.AddMarkupContent(0, "This is the first tab."))
			)
			.AddChildContent<HxTab>(tab => tab
				.Add(t => t.Id, "2")
				.Add(t => t.Title, "Second")
				.Add(t => t.Content, builder => builder.AddMarkupContent(0, "This is the second tab."))
			)
		);

		// Assert - Component should render without infinite loop
		Assert.IsNotNull(component);

		// Trigger a tab change to activate the callback
		component.SetParametersAndRender(parameters => parameters
			.Add(p => p.ActiveTabId, "2")
		);

		// The callback should have been called exactly once during deactivation
		Assert.AreEqual(1, callbackCount, "OnTabDeactivated callback should be called exactly once, not in an infinite loop");
	}

	[TestMethod]
	public void HxTabPanel_ParametersSetWithoutChange_ShouldNotTriggerCallbacks()
	{
		// Arrange
		int activatedCallbackCount = 0;
		int deactivatedCallbackCount = 0;
		string activeTabId = "1";

		// Act - Render component
		var component = RenderComponent<HxTabPanel>(parameters => parameters
			.Add(p => p.ActiveTabId, activeTabId)
			.Add(p => p.ActiveTabIdChanged, newActiveTabId => activeTabId = newActiveTabId)
			.AddChildContent<HxTab>(tab => tab
				.Add(t => t.Id, "1")
				.Add(t => t.Title, "First")
				.Add(t => t.OnTabActivated, () =>
				{
					activatedCallbackCount++;
					return Task.CompletedTask;
				})
				.Add(t => t.OnTabDeactivated, () =>
				{
					deactivatedCallbackCount++;
					return Task.CompletedTask;
				})
				.Add(t => t.Content, builder => builder.AddMarkupContent(0, "First tab content"))
			)
		);

		// Set the same parameters again (no actual change to active tab)
		component.SetParametersAndRender(parameters => parameters
			.Add(p => p.ActiveTabId, activeTabId)
		);

		// Assert - No callbacks should be triggered when the active tab hasn't actually changed
		// The new implementation prevents redundant callbacks by checking _previousActiveTab
		Assert.AreEqual(0, activatedCallbackCount, "OnTabActivated should not be called when the active tab hasn't changed");
		Assert.AreEqual(0, deactivatedCallbackCount, "OnTabDeactivated should not be called when the active tab hasn't changed");
	}
}