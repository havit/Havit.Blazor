namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxBreadcrumbTests : BunitTestBase
{
	[Fact]
	public void HxBreadcrumb_Render_DisplaysAllItems()
	{
		// Act
		var cut = RenderComponent<HxBreadcrumb>(parameters => parameters
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
		var cut = RenderComponent<HxBreadcrumb>(parameters => parameters
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
		var cut = RenderComponent<HxBreadcrumb>(parameters => parameters
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
		var cut = RenderComponent<HxBreadcrumb>(parameters => parameters
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

		// Assert — explicit divider elements between items (Bootstrap 6), i.e. one less than the item count
		Assert.Equal(2, cut.FindAll("li.breadcrumb-divider").Count);
	}
}
