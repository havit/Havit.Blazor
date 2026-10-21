namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxTreeViewTests : BunitTestBase
{
	private class TreeItem
	{
		public string Title { get; set; }
		public List<TreeItem> Children { get; set; } = new();
	}

	private List<TreeItem> CreateTestData()
	{
		return new List<TreeItem>
		{
			new TreeItem
			{
				Title = "Root1",
				Children = new List<TreeItem>
				{
					new TreeItem { Title = "Child1A" },
					new TreeItem
					{
						Title = "Child1B",
						Children = new List<TreeItem>
						{
							new TreeItem { Title = "GrandChild1B1" }
						}
					}
				}
			},
			new TreeItem { Title = "Root2" }
		};
	}

	[Fact]
	public void HxTreeView_Render_DisplaysRootNodes()
	{
		// Arrange & Act
		var cut = Render<HxTreeView<TreeItem>>(parameters => parameters
			.Add(p => p.Items, CreateTestData())
			.Add(p => p.ItemTitleSelector, item => item.Title)
			.Add(p => p.ItemChildrenSelector, item => item.Children)
		);

		// Assert - select only root-level items (direct children of the tree view container)
		var treeView = cut.Find(".hx-tree-view");

		var rootItems = treeView.Children.Where(e => e.ClassList.Contains("hx-tree-view-item"));
		var titleTexts = rootItems
			.Select(item => item.QuerySelector(".hx-tree-view-item-title")?.TextContent)
			.Where(text => text is not null)
			.ToList();
		Assert.Equal(2, titleTexts.Count());
		Assert.Contains("Root1", titleTexts);
		Assert.Contains("Root2", titleTexts);
	}

	[Fact]
	public void HxTreeView_ExpandNode_ShowsChildren()
	{
		// Arrange & Act - Use ItemInitialExpandedSelector to expand Root1
		var cut = Render<HxTreeView<TreeItem>>(parameters => parameters
			.Add(p => p.Items, CreateTestData())
			.Add(p => p.ItemTitleSelector, item => item.Title)
			.Add(p => p.ItemChildrenSelector, item => item.Children)
			.Add(p => p.ItemInitialExpandedSelector, item => item.Title == "Root1")
		);

		// Assert - The collapse container for Root1's children should have "show" class
		var treeView = cut.Find(".hx-tree-view");
		var expandedCollapses = treeView.QuerySelectorAll(".collapse.show");
		Assert.Single(expandedCollapses);

		// Verify children are within the expanded section
		var childTitleTexts = expandedCollapses[0].QuerySelectorAll(".hx-tree-view-item-title").Select(t => t.TextContent).ToList();
		Assert.Contains("Child1A", childTitleTexts);
		Assert.Contains("Child1B", childTitleTexts);
	}

	[Fact]
	public void HxTreeView_DefaultState_NodesAreCollapsed()
	{
		// Arrange & Act - Default state: all nodes collapsed
		var cut = Render<HxTreeView<TreeItem>>(parameters => parameters
			.Add(p => p.Items, CreateTestData())
			.Add(p => p.ItemTitleSelector, item => item.Title)
			.Add(p => p.ItemChildrenSelector, item => item.Children)
		);

		// Assert - No collapse should have "show" class
		var treeView = cut.Find(".hx-tree-view");
		var expandedCollapses = treeView.QuerySelectorAll(".collapse.show");
		Assert.Empty(expandedCollapses);

		// Verify collapse elements exist (items with children have collapse containers)
		var allCollapses = treeView.QuerySelectorAll(".collapse");
		Assert.NotEmpty(allCollapses);
	}

	[Fact]
	public void HxTreeView_SelectNode_HighlightsIt()
	{
		// Arrange
		var testData = CreateTestData();
		TreeItem selectedItem = null;

		var cut = Render<HxTreeView<TreeItem>>(parameters => parameters
			.Add(p => p.Items, testData)
			.Add(p => p.ItemTitleSelector, item => item.Title)
			.Add(p => p.ItemChildrenSelector, item => item.Children)
			.Add(p => p.SelectedItem, selectedItem)
			.Add(p => p.SelectedItemChanged, (TreeItem item) => { selectedItem = item; })
		);

		// Act - Click on Root1
		var root1Item = cut.FindAll(".hx-tree-view-item")
			.First(e => e.QuerySelector(".hx-tree-view-item-title")?.TextContent == "Root1");
		root1Item.Click();

		// Assert - Callback should have been invoked
		Assert.NotNull(selectedItem);
		Assert.Equal("Root1", selectedItem.Title);

		// Assert - The clicked item should have "selected" class
		var selectedItems = cut.FindAll(".hx-tree-view-item.selected");
		Assert.Single(selectedItems);
		var selectedTitle = selectedItems[0].QuerySelector(".hx-tree-view-item-title");
		Assert.Equal("Root1", selectedTitle.TextContent);
	}

	[Fact]
	public void HxTreeView_NestedExpansion_WorksCorrectly()
	{
		// Arrange & Act - Expand all levels
		var cut = Render<HxTreeView<TreeItem>>(parameters => parameters
			.Add(p => p.Items, CreateTestData())
			.Add(p => p.ItemTitleSelector, item => item.Title)
			.Add(p => p.ItemChildrenSelector, item => item.Children)
			.Add(p => p.ItemInitialExpandedSelector, _ => true)
		);

		// Assert - All collapse elements should have "show" class (fully expanded)
		var treeView = cut.Find(".hx-tree-view");
		var allCollapses = treeView.QuerySelectorAll(".collapse");
		Assert.NotEmpty(allCollapses);
		foreach (var collapse in allCollapses)
		{
			Assert.True(collapse.ClassList.Contains("show"), "All collapse elements should have 'show' class when fully expanded.");
		}

		// Verify all titles at every level are present
		var allTitleTexts = cut.FindAll(".hx-tree-view-item-title").Select(e => e.TextContent).ToList();
		Assert.Contains("Root1", allTitleTexts);
		Assert.Contains("Root2", allTitleTexts);
		Assert.Contains("Child1A", allTitleTexts);
		Assert.Contains("Child1B", allTitleTexts);
		Assert.Contains("GrandChild1B1", allTitleTexts);
	}

	[Fact]
	public void HxTreeView_ExpandOnSelection_ExpandsNestedItem()
	{
		// Arrange
		var cut = Render<HxTreeView<TreeItem>>(parameters => parameters
			.Add(p => p.Items, CreateTestData())
			.Add(p => p.ItemTitleSelector, item => item.Title)
			.Add(p => p.ItemChildrenSelector, item => item.Children)
			.Add(p => p.ItemInitialExpandedSelector, _ => false)
			.Add(p => p.ExpandOnSelection, true)
		);

		// Act - select the nested Child1B item.
		var child1BTitle = cut.FindAll(".hx-tree-view-item-title")
			.Single(e => e.TextContent == "Child1B");

		child1BTitle.ParentElement!.Click();

		// Assert - its child is expanded.
		var collapseElement = cut.FindAll(".collapse")
			.Single(e => e.TextContent == "GrandChild1B1");

		var invocation = JSInterop.Invocations.Single(i => i.Identifier.EndsWith("show"));
		invocation.Arguments[0].ShouldBeElementReferenceTo(collapseElement);
	}

	[Fact]
	public void HxTreeView_ExpandOnSelection_KeyboardOnSelectedItem_ExpandsItem()
	{
		// Arrange - Child1B is selected (and expanded) by a click.
		var cut = Render<HxTreeView<TreeItem>>(parameters => parameters
			.Add(p => p.Items, CreateTestData())
			.Add(p => p.ItemTitleSelector, item => item.Title)
			.Add(p => p.ItemChildrenSelector, item => item.Children)
			.Add(p => p.ExpandOnSelection, true)
		);

		cut.FindAll(".hx-tree-view-item-title")
			.Single(e => e.TextContent == "Child1B")
			.ParentElement!.Click();

		// Act - activate the already selected item with the keyboard (e.g. after the user collapsed it).
		cut.FindAll(".hx-tree-view-item-title")
			.Single(e => e.TextContent == "Child1B")
			.ParentElement!.KeyDown(Key.Enter);

		// Assert - the keyboard activation expands it again, the same as a click does.
		var collapseElement = cut.FindAll(".collapse")
			.Single(e => e.TextContent == "GrandChild1B1");

		var invocations = JSInterop.Invocations.Where(i => i.Identifier.EndsWith("show")).ToList();
		Assert.Equal(2, invocations.Count);
		invocations[1].Arguments[0].ShouldBeElementReferenceTo(collapseElement);
	}

	[Fact]
	public void HxTreeView_ExpandOnSelection_InitialSelection_ExpandsSelectedItem()
	{
		// Arrange
		var testData = CreateTestData();
		var child1B = testData[0].Children[1];

		// Act - render with the nested Child1B item initially selected.
		var cut = Render<HxTreeView<TreeItem>>(parameters => parameters
			.Add(p => p.Items, testData)
			.Add(p => p.ItemTitleSelector, item => item.Title)
			.Add(p => p.ItemChildrenSelector, item => item.Children)
			.Add(p => p.ExpandOnSelection, true)
			.Add(p => p.SelectedItem, child1B)
		);

		// Assert - Child1B renders expanded.
		var expandedCollapse = Assert.Single(cut.FindAll(".collapse.show"));
		Assert.Contains("GrandChild1B1", expandedCollapse.TextContent);
	}

	[Fact]
	public void HxTreeView_ExpandOnSelection_ProgrammaticSelection_ExpandsSelectedItem()
	{
		// Arrange
		var testData = CreateTestData();
		var child1B = testData[0].Children[1];

		var cut = Render<HxTreeView<TreeItem>>(parameters => parameters
			.Add(p => p.Items, testData)
			.Add(p => p.ItemTitleSelector, item => item.Title)
			.Add(p => p.ItemChildrenSelector, item => item.Children)
			.Add(p => p.ExpandOnSelection, true)
		);

		// Act - select the nested Child1B item through a parameter update.
		cut.Render(parameters => parameters.Add(p => p.SelectedItem, child1B));

		// Assert - its child is expanded.
		var collapseElement = cut.FindAll(".collapse")
			.Single(e => e.TextContent == "GrandChild1B1");

		var invocation = JSInterop.Invocations.Single(i => i.Identifier.EndsWith("show"));
		invocation.Arguments[0].ShouldBeElementReferenceTo(collapseElement);
	}

	[Fact]
	public void HxTreeView_ExpandOnSelection_ChildrenRemoved_SelectionDoesNotExpand()
	{
		// Arrange
		var testData = CreateTestData();
		var root1 = testData[0];

		var cut = Render<HxTreeView<TreeItem>>(parameters => parameters
			.Add(p => p.Items, testData)
			.Add(p => p.ItemTitleSelector, item => item.Title)
			.Add(p => p.ItemChildrenSelector, item => item.Children)
			.Add(p => p.ExpandOnSelection, true)
		);

		// Act - Root1 becomes a leaf (its HxCollapse gets removed), then it is selected by click.
		root1.Children.Clear();
		cut.Render(parameters => parameters.Add(p => p.Items, testData));

		var root1Item = cut.FindAll(".hx-tree-view-item")
			.First(e => e.QuerySelector(".hx-tree-view-item-title")?.TextContent == "Root1");
		root1Item.Click();

		// Assert - selection works, no expansion is attempted on the removed collapse.
		var selectedTitle = Assert.Single(cut.FindAll(".hx-tree-view-item.selected")).QuerySelector(".hx-tree-view-item-title");
		Assert.Equal("Root1", selectedTitle.TextContent);
		Assert.DoesNotContain(JSInterop.Invocations, i => i.Identifier.EndsWith("show"));
	}

	[Fact]
	public void HxTreeView_Render_HasTreeAndGroupRolesAndExpanderState()
	{
		// Arrange & Act
		var cut = Render<HxTreeView<TreeItem>>(parameters => parameters
			.Add(p => p.Items, CreateTestData())
			.Add(p => p.ItemTitleSelector, item => item.Title)
			.Add(p => p.ItemChildrenSelector, item => item.Children)
			.Add(p => p.ItemInitialExpandedSelector, item => item.Title == "Root1")
		);

		// Assert - every treeitem is owned by a tree or a group
		var treeView = cut.Find(".hx-tree-view");
		Assert.Equal("tree", treeView.GetAttribute("role"));
		var treeItems = cut.FindAll("[role=treeitem]");
		Assert.Equal(5, treeItems.Count);
		Assert.All(treeItems, treeItem => Assert.Contains(treeItem.ParentElement.GetAttribute("role"), new[] { "tree", "group" }));
		Assert.All(cut.FindAll(".collapse"), collapse => Assert.Equal("group", collapse.GetAttribute("role")));

		// Assert - expander state is driven by aria-expanded (the chevron CSS keys on it), no collapsed class
		var expanders = cut.FindAll(".hx-tree-view-item-expander");
		Assert.Equal(2, expanders.Count); // Root1, Child1B
		Assert.Equal("true", expanders[0].GetAttribute("aria-expanded"));
		Assert.Equal("Collapse", expanders[0].GetAttribute("aria-label"));
		Assert.Equal("false", expanders[1].GetAttribute("aria-expanded"));
		Assert.Equal("Expand", expanders[1].GetAttribute("aria-label"));
		Assert.All(expanders, expander => Assert.False(expander.ClassList.Contains("collapsed")));
	}
}
