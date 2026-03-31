namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

[TestClass]
public class HxBreadcrumbTests : BunitTestBase
{
	[TestMethod]
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
		Assert.HasCount(3, items);
		Assert.Contains("Home", items[0].TextContent, "First item should contain 'Home'.");
		Assert.Contains("Library", items[1].TextContent, "Second item should contain 'Library'.");
		Assert.Contains("Data", items[2].TextContent, "Third item should contain 'Data'.");
	}

	[TestMethod]
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
		var activeItem = cut.Find("li.breadcrumb-item.active");
		Assert.AreEqual("page", activeItem.GetAttribute("aria-current"));

		var links = activeItem.QuerySelectorAll("a");
		Assert.HasCount(0, links, "Active breadcrumb item should not contain a link.");
	}

	[TestMethod]
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
		var nonActiveItems = cut.FindAll("li.breadcrumb-item:not(.active)");
		Assert.HasCount(2, nonActiveItems, "There should be two non-active items.");

		var firstAnchor = nonActiveItems[0].QuerySelector("a");
		Assert.IsNotNull(firstAnchor, "First non-active breadcrumb item should contain a link.");
		Assert.AreEqual("/home", firstAnchor.GetAttribute("href"));

		var secondAnchor = nonActiveItems[1].QuerySelector("a");
		Assert.IsNotNull(secondAnchor, "Second non-active breadcrumb item should contain a link.");
		Assert.AreEqual("/library", secondAnchor.GetAttribute("href"));
	}
}
