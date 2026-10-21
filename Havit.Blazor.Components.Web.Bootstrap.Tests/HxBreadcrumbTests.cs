namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxBreadcrumbTests : BunitTestBase
{
	[Fact]
	public void HxBreadcrumb_Render_DisplaysAllItems()
	{
		// Act
		var cut = Render<HxBreadcrumb>(parameters => parameters
			.AddChildContent<HxBreadcrumbItem>(item => item
				.Add(i => i.Href, "/home")
				.Add(i => i.Text, "Home"))
			.AddChildContent<HxBreadcrumbItem>(item => item
				.Add(i => i.Href, "/library")
				.Add(i => i.Text, "Library"))
			.AddChildContent<HxBreadcrumbItem>(item => item
				.Add(i => i.Text, "Data")
				.Add(i => i.Active, true))
		);

		// Assert
		var items = cut.FindAll("li.breadcrumb-item");
		Assert.Equal(3, items.Count());
		Assert.Contains("Home", items[0].TextContent);
		Assert.Contains("Library", items[1].TextContent);
		Assert.Contains("Data", items[2].TextContent);
	}

	[Fact]
	public void HxBreadcrumb_ActiveItem_HasNoLink()
	{
		// Act
		var cut = Render<HxBreadcrumb>(parameters => parameters
			.AddChildContent<HxBreadcrumbItem>(item => item
				.Add(i => i.Href, "/home")
				.Add(i => i.Text, "Home"))
			.AddChildContent<HxBreadcrumbItem>(item => item
				.Add(i => i.Text, "Current")
				.Add(i => i.Active, true))
		);

		// Assert
		var activeItem = cut.Find("li.breadcrumb-item[aria-current='page']");
		var activeLink = activeItem.QuerySelector(".breadcrumb-link.active");
		Assert.NotNull(activeLink);

		var links = activeItem.QuerySelectorAll("a");
		Assert.Empty(links); // no Href - renders a span, not an anchor
	}

	[Fact]
	public void HxBreadcrumb_NonActiveItems_RenderLinksWithHrefs()
	{
		// Act
		var cut = Render<HxBreadcrumb>(parameters => parameters
			.AddChildContent<HxBreadcrumbItem>(item => item
				.Add(i => i.Href, "/home")
				.Add(i => i.Text, "Home"))
			.AddChildContent<HxBreadcrumbItem>(item => item
				.Add(i => i.Href, "/library")
				.Add(i => i.Text, "Library"))
			.AddChildContent<HxBreadcrumbItem>(item => item
				.Add(i => i.Text, "Data")
				.Add(i => i.Active, true))
		);

		// Assert — non-active items render links with correct hrefs for navigation
		var nonActiveItems = cut.FindAll("li.breadcrumb-item:not([aria-current])");
		Assert.Equal(2, nonActiveItems.Count());

		var firstAnchor = nonActiveItems[0].QuerySelector("a");
		Assert.NotNull(firstAnchor);
		Assert.Equal("/home", firstAnchor.GetAttribute("href"));

		var secondAnchor = nonActiveItems[1].QuerySelector("a");
		Assert.NotNull(secondAnchor);
		Assert.Equal("/library", secondAnchor.GetAttribute("href"));
	}

	[Fact]
	public void HxBreadcrumb_RendersDividersBetweenItems()
	{
		// Act
		var cut = Render<HxBreadcrumb>(parameters => parameters
			.AddChildContent<HxBreadcrumbItem>(item => item
				.Add(i => i.Href, "/home")
				.Add(i => i.Text, "Home"))
			.AddChildContent<HxBreadcrumbItem>(item => item
				.Add(i => i.Href, "/library")
				.Add(i => i.Text, "Library"))
			.AddChildContent<HxBreadcrumbItem>(item => item
				.Add(i => i.Text, "Data")
				.Add(i => i.Active, true))
		);

		// Assert — explicit divider elements (Bootstrap 6) in front of every item, the leading one is hidden by CSS
		AssertDividerInFrontOfEveryItem(cut, 3);
	}

	[Fact]
	public void HxBreadcrumb_ItemInsertedBeforeExistingItems_DividersStayBetweenItems()
	{
		// Arrange
		var cut = Render<HxBreadcrumb>(parameters => parameters
			.AddChildContent<HxBreadcrumbItem>(item => item
				.Add(i => i.Href, "/library")
				.Add(i => i.Text, "Library"))
			.AddChildContent<HxBreadcrumbItem>(item => item
				.Add(i => i.Text, "Data")
				.Add(i => i.Active, true))
		);

		// Act — a conditionally rendered item appears in front of the existing items
		cut.Render(parameters => parameters
			.Add(p => p.ChildContent, builder =>
			{
				builder.OpenComponent<HxBreadcrumbItem>(0);
				builder.AddComponentParameter(1, nameof(HxBreadcrumbItem.Href), "/home");
				builder.AddComponentParameter(2, nameof(HxBreadcrumbItem.Text), "Home");
				builder.CloseComponent();
				builder.OpenComponent<HxBreadcrumbItem>(3);
				builder.AddComponentParameter(4, nameof(HxBreadcrumbItem.Href), "/library");
				builder.AddComponentParameter(5, nameof(HxBreadcrumbItem.Text), "Library");
				builder.CloseComponent();
				builder.OpenComponent<HxBreadcrumbItem>(6);
				builder.AddComponentParameter(7, nameof(HxBreadcrumbItem.Text), "Data");
				builder.AddComponentParameter(8, nameof(HxBreadcrumbItem.Active), true);
				builder.CloseComponent();
			}));

		// Assert
		AssertDividerInFrontOfEveryItem(cut, 3);
		Assert.Contains("Home", cut.FindAll("li.breadcrumb-item")[0].TextContent);
	}

	private static void AssertDividerInFrontOfEveryItem(IRenderedComponent<HxBreadcrumb> cut, int expectedItemCount)
	{
		var children = cut.Find("ol.breadcrumb").Children;
		Assert.Equal(expectedItemCount * 2, children.Length);
		for (int i = 0; i < children.Length; i += 2)
		{
			Assert.Contains("breadcrumb-divider", children[i].ClassList);
			Assert.Contains("breadcrumb-item", children[i + 1].ClassList);
		}
	}
}
