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

	[TestMethod]
	public void HxTabPanel_Render_DisplaysAllTabHeaders()
	{
		// Act
		var component = RenderComponent<HxTabPanel>(parameters => parameters
			.Add(p => p.ActiveTabId, "tab1")
			.AddChildContent<HxTab>(tab => tab
				.Add(t => t.Id, "tab1")
				.Add(t => t.Title, "Alpha")
				.Add(t => t.Content, builder => builder.AddMarkupContent(0, "Content Alpha"))
			)
			.AddChildContent<HxTab>(tab => tab
				.Add(t => t.Id, "tab2")
				.Add(t => t.Title, "Beta")
				.Add(t => t.Content, builder => builder.AddMarkupContent(0, "Content Beta"))
			)
			.AddChildContent<HxTab>(tab => tab
				.Add(t => t.Id, "tab3")
				.Add(t => t.Title, "Gamma")
				.Add(t => t.Content, builder => builder.AddMarkupContent(0, "Content Gamma"))
			)
		);

		// Assert - All three tab headers should be rendered as nav-link elements
		var navLinks = component.FindAll("a.nav-link");
		Assert.HasCount(3, navLinks);
		Assert.AreEqual("Alpha", navLinks[0].TextContent.Trim());
		Assert.AreEqual("Beta", navLinks[1].TextContent.Trim());
		Assert.AreEqual("Gamma", navLinks[2].TextContent.Trim());
	}

	[TestMethod]
	public void HxTabPanel_ClickTab_SwitchesToItsContent()
	{
		// Arrange
		string activeTabId = "tab1";

		var component = RenderComponent<HxTabPanel>(parameters => parameters
			.Add(p => p.ActiveTabId, activeTabId)
			.Add(p => p.ActiveTabIdChanged, newId => activeTabId = newId)
			.AddChildContent<HxTab>(tab => tab
				.Add(t => t.Id, "tab1")
				.Add(t => t.Title, "First")
				.Add(t => t.Content, builder => builder.AddMarkupContent(0, "<p>First content</p>"))
			)
			.AddChildContent<HxTab>(tab => tab
				.Add(t => t.Id, "tab2")
				.Add(t => t.Title, "Second")
				.Add(t => t.Content, builder => builder.AddMarkupContent(0, "<p>Second content</p>"))
			)
		);

		// Act - Click on the second tab header
		var navLinks = component.FindAll("a.nav-link");
		navLinks[1].Click();

		// Assert - The second tab's content pane should be active
		var activePane = component.Find("div.tab-pane.active");
		Assert.Contains("Second content", activePane.InnerHtml);
	}

	[TestMethod]
	public void HxTabPanel_ActiveTab_HasActiveClass()
	{
		// Arrange & Act
		var component = RenderComponent<HxTabPanel>(parameters => parameters
			.Add(p => p.ActiveTabId, "tab2")
			.AddChildContent<HxTab>(tab => tab
				.Add(t => t.Id, "tab1")
				.Add(t => t.Title, "First")
				.Add(t => t.Content, builder => builder.AddMarkupContent(0, "Content 1"))
			)
			.AddChildContent<HxTab>(tab => tab
				.Add(t => t.Id, "tab2")
				.Add(t => t.Title, "Second")
				.Add(t => t.Content, builder => builder.AddMarkupContent(0, "Content 2"))
			)
		);

		// Assert - Only the second tab header should have the active class
		var navLinks = component.FindAll("a.nav-link");
		Assert.IsFalse(navLinks[0].ClassList.Contains("active"), "First tab should not have active class");
		Assert.IsTrue(navLinks[1].ClassList.Contains("active"), "Second tab should have active class");
	}

	[TestMethod]
	public void HxTabPanel_FirstTab_ActiveByDefault()
	{
		// Act - Render without specifying ActiveTabId
		var component = RenderComponent<HxTabPanel>(parameters => parameters
			.AddChildContent<HxTab>(tab => tab
				.Add(t => t.Id, "tab1")
				.Add(t => t.Title, "First")
				.Add(t => t.Content, builder => builder.AddMarkupContent(0, "First tab content"))
			)
			.AddChildContent<HxTab>(tab => tab
				.Add(t => t.Id, "tab2")
				.Add(t => t.Title, "Second")
				.Add(t => t.Content, builder => builder.AddMarkupContent(0, "Second tab content"))
			)
		);

		// Assert - First tab should be active by default
		var navLinks = component.FindAll("a.nav-link");
		Assert.IsTrue(navLinks[0].ClassList.Contains("active"), "First tab should be active by default");
		Assert.IsFalse(navLinks[1].ClassList.Contains("active"), "Second tab should not be active by default");

		// Assert - First tab's content pane should be active
		var activePane = component.Find("div.tab-pane.active");
		Assert.Contains("First tab content", activePane.InnerHtml);
	}

	[TestMethod]
	public void HxTabPanel_AriaAttributes_ActiveTabHasCorrectRolesAndAttributes()
	{
		// Arrange & Act
		var component = RenderComponent<HxTabPanel>(parameters => parameters
			.Add(p => p.ActiveTabId, "tab1")
			.AddChildContent<HxTab>(tab => tab
				.Add(t => t.Id, "tab1")
				.Add(t => t.Title, "First")
				.Add(t => t.Content, builder => builder.AddMarkupContent(0, "Content 1"))
			)
			.AddChildContent<HxTab>(tab => tab
				.Add(t => t.Id, "tab2")
				.Add(t => t.Title, "Second")
				.Add(t => t.Content, builder => builder.AddMarkupContent(0, "Content 2"))
			)
		);

		var navLinks = component.FindAll("a.nav-link");

		// Active tab header
		Assert.AreEqual("tab", navLinks[0].GetAttribute("role"), "Active tab should have role=tab");
		Assert.AreEqual("true", navLinks[0].GetAttribute("aria-selected"), "Active tab should have aria-selected=true");
		Assert.AreEqual("0", navLinks[0].GetAttribute("tabindex"), "Active tab should have tabindex=0");
		Assert.AreEqual("tab1-header", navLinks[0].GetAttribute("id"), "Active tab header id should be tab1-header");
		Assert.AreEqual("tab1", navLinks[0].GetAttribute("aria-controls"), "Active tab should have aria-controls pointing to panel id");

		// Inactive tab header
		Assert.AreEqual("tab", navLinks[1].GetAttribute("role"), "Inactive tab should have role=tab");
		Assert.AreEqual("false", navLinks[1].GetAttribute("aria-selected"), "Inactive tab should have aria-selected=false");
		Assert.AreEqual("-1", navLinks[1].GetAttribute("tabindex"), "Inactive tab should have tabindex=-1");
	}

	[TestMethod]
	public void HxTabPanel_AriaAttributes_TabPanelHasCorrectRoleAndLabelledBy()
	{
		// Arrange & Act
		var component = RenderComponent<HxTabPanel>(parameters => parameters
			.Add(p => p.ActiveTabId, "tab1")
			.AddChildContent<HxTab>(tab => tab
				.Add(t => t.Id, "tab1")
				.Add(t => t.Title, "First")
				.Add(t => t.Content, builder => builder.AddMarkupContent(0, "Content 1"))
			)
		);

		var pane = component.Find("div.tab-pane");

		Assert.AreEqual("tabpanel", pane.GetAttribute("role"), "Tab pane should have role=tabpanel");
		Assert.AreEqual("tab1", pane.GetAttribute("id"), "Tab pane id should match tab id");
		Assert.AreEqual("tab1-header", pane.GetAttribute("aria-labelledby"), "Tab pane aria-labelledby should point to the tab header id");
		Assert.AreEqual("0", pane.GetAttribute("tabindex"), "Tab pane should have tabindex=0");
	}

	[TestMethod]
	public void HxTabPanel_AriaAttributes_TablistRoleOnNav()
	{
		// Arrange & Act
		var component = RenderComponent<HxTabPanel>(parameters => parameters
			.Add(p => p.ActiveTabId, "tab1")
			.AddChildContent<HxTab>(tab => tab
				.Add(t => t.Id, "tab1")
				.Add(t => t.Title, "First")
				.Add(t => t.Content, builder => builder.AddMarkupContent(0, "Content 1"))
			)
		);

		var nav = component.Find("nav");
		Assert.AreEqual("tablist", nav.GetAttribute("role"), "Nav container should have role=tablist");
	}

	[TestMethod]
	public void HxTabPanel_AriaAttributes_ActiveTabOnly_InactiveTabHasNoAriaControls()
	{
		// Arrange & Act
		var component = RenderComponent<HxTabPanel>(parameters => parameters
			.Add(p => p.ActiveTabId, "tab1")
			.Add(p => p.RenderMode, TabPanelRenderMode.ActiveTabOnly)
			.AddChildContent<HxTab>(tab => tab
				.Add(t => t.Id, "tab1")
				.Add(t => t.Title, "First")
				.Add(t => t.Content, builder => builder.AddMarkupContent(0, "Content 1"))
			)
			.AddChildContent<HxTab>(tab => tab
				.Add(t => t.Id, "tab2")
				.Add(t => t.Title, "Second")
				.Add(t => t.Content, builder => builder.AddMarkupContent(0, "Content 2"))
			)
		);

		var navLinks = component.FindAll("a.nav-link");

		// Active tab should still have aria-controls (its panel is rendered)
		Assert.AreEqual("tab1", navLinks[0].GetAttribute("aria-controls"), "Active tab should have aria-controls in ActiveTabOnly mode");

		// Inactive tab should NOT have aria-controls (its panel is not in the DOM)
		Assert.IsNull(navLinks[1].GetAttribute("aria-controls"), "Inactive tab should not have aria-controls in ActiveTabOnly mode");
	}
}