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
		Assert.AreEqual(3, items.Count);
		Assert.IsTrue(items[0].TextContent.Contains("Home"), "First item should contain 'Home'.");
		Assert.IsTrue(items[1].TextContent.Contains("Library"), "Second item should contain 'Library'.");
		Assert.IsTrue(items[2].TextContent.Contains("Data"), "Third item should contain 'Data'.");
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
		Assert.AreEqual(0, links.Length, "Active breadcrumb item should not contain a link.");
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
		Assert.AreEqual(2, nonActiveItems.Count, "There should be two non-active items.");

		var firstAnchor = nonActiveItems[0].QuerySelector("a");
		Assert.IsNotNull(firstAnchor, "First non-active breadcrumb item should contain a link.");
		Assert.AreEqual("/home", firstAnchor.GetAttribute("href"));

		var secondAnchor = nonActiveItems[1].QuerySelector("a");
		Assert.IsNotNull(secondAnchor, "Second non-active breadcrumb item should contain a link.");
		Assert.AreEqual("/library", secondAnchor.GetAttribute("href"));
	}

	[TestMethod]
	public void HxBreadcrumb_NonActiveItem_Click_Navigates()
	{
		// Arrange & Act
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

		// Get NavigationManager from the test services
		var navigationManager = Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions
			.GetRequiredService<Microsoft.AspNetCore.Components.NavigationManager>(Services);

		var initialUri = navigationManager.Uri;

		// Act — click on a non-active breadcrumb item (e.g., "Library")
		var nonActiveItems = cut.FindAll("li.breadcrumb-item:not(.active)");
		Assert.AreEqual(2, nonActiveItems.Count, "There should be two non-active items.");

		var libraryAnchor = nonActiveItems[1].QuerySelector("a");
		Assert.IsNotNull(libraryAnchor, "Second non-active breadcrumb item should contain a link.");

		libraryAnchor.Click();

		// Assert — NavigationManager URI changed to the clicked item's href
		Assert.AreNotEqual(initialUri, navigationManager.Uri, "NavigationManager URI should change after clicking a breadcrumb link.");
		StringAssert.EndsWith(navigationManager.Uri, "/library", "NavigationManager URI should end with the clicked breadcrumb href.");
	}
}
