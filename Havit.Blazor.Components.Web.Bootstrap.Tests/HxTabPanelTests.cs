namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxTabPanelTests : BunitTestBase
{
	[Fact]
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
		Assert.NotNull(component);

		// Trigger a tab change to activate the callback
		component.SetParametersAndRender(parameters => parameters
			.Add(p => p.ActiveTabId, "2")
		);

		// The callback should have been called exactly once during deactivation
		Assert.Equal(1, callbackCount);
	}

	[Fact]
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
		Assert.Equal(0, activatedCallbackCount);
		Assert.Equal(0, deactivatedCallbackCount);
	}

	[Fact]
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
		Assert.Equal(3, navLinks.Count());
		Assert.Equal("Alpha", navLinks[0].TextContent.Trim());
		Assert.Equal("Beta", navLinks[1].TextContent.Trim());
		Assert.Equal("Gamma", navLinks[2].TextContent.Trim());
	}

	[Fact]
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

	[Fact]
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
		Assert.False(navLinks[0].ClassList.Contains("active"), "First tab should not have active class");
		Assert.True(navLinks[1].ClassList.Contains("active"), "Second tab should have active class");
	}

	[Fact]
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
		Assert.True(navLinks[0].ClassList.Contains("active"), "First tab should be active by default");
		Assert.False(navLinks[1].ClassList.Contains("active"), "Second tab should not be active by default");

		// Assert - First tab's content pane should be active
		var activePane = component.Find("div.tab-pane.active");
		Assert.Contains("First tab content", activePane.InnerHtml);
	}

	[Fact]
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
		Assert.Equal("tab", navLinks[0].GetAttribute("role"));
		Assert.Equal("true", navLinks[0].GetAttribute("aria-selected"));
		Assert.Equal("0", navLinks[0].GetAttribute("tabindex"));
		Assert.Equal("tab1-header", navLinks[0].GetAttribute("id"));
		Assert.Equal("tab1", navLinks[0].GetAttribute("aria-controls"));

		// Inactive tab header
		Assert.Equal("tab", navLinks[1].GetAttribute("role"));
		Assert.Equal("false", navLinks[1].GetAttribute("aria-selected"));
		Assert.Equal("-1", navLinks[1].GetAttribute("tabindex"));
	}

	[Fact]
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

		Assert.Equal("tabpanel", pane.GetAttribute("role"));
		Assert.Equal("tab1", pane.GetAttribute("id"));
		Assert.Equal("tab1-header", pane.GetAttribute("aria-labelledby"));
		Assert.Equal("0", pane.GetAttribute("tabindex"));
	}

	[Fact]
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
		Assert.Equal("tablist", nav.GetAttribute("role"));
	}

	[Fact]
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
		Assert.Equal("tab1", navLinks[0].GetAttribute("aria-controls"));

		// Inactive tab should NOT have aria-controls (its panel is not in the DOM)
		Assert.Null(navLinks[1].GetAttribute("aria-controls"));
	}

	[Fact]
	public void HxTabPanel_VerticalOrientation_NavIsFlexColumnWithAriaOrientation()
	{
		// Arrange & Act
		var component = RenderComponent<HxTabPanel>(parameters => parameters
			.Add(p => p.Orientation, NavOrientation.Vertical)
			.Add(p => p.NavVariant, NavVariant.Pills)
			.Add(p => p.ActiveTabId, "tab1")
			.AddChildContent<HxTab>(tab => tab
				.Add(t => t.Id, "tab1")
				.Add(t => t.Title, "First")
				.Add(t => t.Content, builder => builder.AddMarkupContent(0, "Content 1"))
			)
		);

		var nav = component.Find("nav");
		Assert.Contains("flex-column", nav.ClassList);
		Assert.Equal("vertical", nav.GetAttribute("aria-orientation"));

		// The nav and the content are wrapped in a flex container so they sit side by side
		var wrapper = component.Find("div.d-flex");
		Assert.NotNull(wrapper);
	}

	[Fact]
	public void HxTabPanel_HorizontalOrientation_NavHasNoAriaOrientation()
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
		Assert.DoesNotContain("flex-column", nav.ClassList);
		Assert.Null(nav.GetAttribute("aria-orientation"));
	}

	[Fact]
	public void HxTabPanel_Fade_PanesHaveFadeAndActiveHasShow()
	{
		// Arrange & Act
		var component = RenderComponent<HxTabPanel>(parameters => parameters
			.Add(p => p.Fade, true)
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

		var panes = component.FindAll("div.tab-pane");

		// Every pane has the fade class
		Assert.All(panes, pane => Assert.Contains("fade", pane.ClassList));

		// Active pane additionally has show + active
		Assert.Contains("show", panes[0].ClassList);
		Assert.Contains("active", panes[0].ClassList);

		// Inactive pane has neither show nor active
		Assert.DoesNotContain("show", panes[1].ClassList);
		Assert.DoesNotContain("active", panes[1].ClassList);
	}

	[Fact]
	public void HxTabPanel_TabHeadersAsButtons_RendersButtonsWithAria()
	{
		// Arrange & Act
		var component = RenderComponent<HxTabPanel>(parameters => parameters
			.Add(p => p.TabHeadersAsButtons, true)
			.Add(p => p.ActiveTabId, "tab1")
			.AddChildContent<HxTab>(tab => tab
				.Add(t => t.Id, "tab1")
				.Add(t => t.Title, "First")
				.Add(t => t.Content, builder => builder.AddMarkupContent(0, "Content 1"))
			)
			.AddChildContent<HxTab>(tab => tab
				.Add(t => t.Id, "tab2")
				.Add(t => t.Title, "Second")
				.Add(t => t.Enabled, false)
				.Add(t => t.Content, builder => builder.AddMarkupContent(0, "Content 2"))
			)
		);

		// Headers are rendered as <button> elements, not anchors
		Assert.Empty(component.FindAll("a.nav-link"));
		var buttons = component.FindAll("button.nav-link");
		Assert.Equal(2, buttons.Count);

		Assert.Equal("button", buttons[0].GetAttribute("type"));
		Assert.Equal("tab", buttons[0].GetAttribute("role"));
		Assert.Equal("true", buttons[0].GetAttribute("aria-selected"));
		Assert.True(buttons[0].ClassList.Contains("active"));

		// Disabled tab renders a disabled button
		Assert.True(buttons[1].HasAttribute("disabled"));
		Assert.True(buttons[1].ClassList.Contains("disabled"));
	}

	[Fact]
	public void HxTabPanel_TabHeadersAsButtons_ClickSwitchesContent()
	{
		// Arrange
		string activeTabId = "tab1";

		var component = RenderComponent<HxTabPanel>(parameters => parameters
			.Add(p => p.TabHeadersAsButtons, true)
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

		// Act - Click on the second tab header button
		var buttons = component.FindAll("button.nav-link");
		buttons[1].Click();

		// Assert - The second tab's content pane should be active
		var activePane = component.Find("div.tab-pane.active");
		Assert.Contains("Second content", activePane.InnerHtml);
	}
}