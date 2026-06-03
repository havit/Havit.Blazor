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
		var cut = RenderComponent<HxTreeView<TreeItem>>(parameters => parameters
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
		var cut = RenderComponent<HxTreeView<TreeItem>>(parameters => parameters
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
		var cut = RenderComponent<HxTreeView<TreeItem>>(parameters => parameters
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

		var cut = RenderComponent<HxTreeView<TreeItem>>(parameters => parameters
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
		var cut = RenderComponent<HxTreeView<TreeItem>>(parameters => parameters
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
}
