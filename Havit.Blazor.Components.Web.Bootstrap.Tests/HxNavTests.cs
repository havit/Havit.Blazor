using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

[TestClass]
public class HxNavTests : BunitTestBase
{
	[TestMethod]
	public void HxNav_Render_DisplaysLinks()
	{
		// Arrange & Act
		var cut = RenderComponent<HxNav>(parameters => parameters
			.AddChildContent<HxNavLink>(link => link
				.Add(l => l.Text, "Home")
				.Add(l => l.Href, "/")
			)
			.AddChildContent<HxNavLink>(link => link
				.Add(l => l.Text, "About")
				.Add(l => l.Href, "/about")
			)
		);

		// Assert
		var navLinks = cut.FindAll("a.nav-link");
		Assert.HasCount(2, navLinks, "Expected two nav-link anchor elements to be rendered.");
		Assert.AreEqual("Home", navLinks[0].TextContent.Trim(), "Expected first nav-link to have text 'Home'.");
		Assert.AreEqual("About", navLinks[1].TextContent.Trim(), "Expected second nav-link to have text 'About'.");
	}

	[TestMethod]
	public void HxNav_ActiveRoute_LinkHasActiveClass()
	{
		// Arrange — navigate to /active-page so the matching link becomes active
		Services.GetRequiredService<Bunit.TestDoubles.FakeNavigationManager>().NavigateTo("http://localhost/active-page");

		// Act
		var cut = RenderComponent<HxNav>(parameters => parameters
			.AddChildContent<HxNavLink>(link => link
				.Add(l => l.Text, "Active")
				.Add(l => l.Href, "/active-page")
				.Add(l => l.Match, NavLinkMatch.All)
			)
			.AddChildContent<HxNavLink>(link => link
				.Add(l => l.Text, "Other")
				.Add(l => l.Href, "/other-page")
				.Add(l => l.Match, NavLinkMatch.All)
			)
		);

		// Assert
		var activeLinks = cut.FindAll("a.nav-link.active");
		Assert.HasCount(1, activeLinks, "Expected exactly one nav-link with the 'active' CSS class.");
		var activeLink = activeLinks[0];
		Assert.AreEqual("Active", activeLink.TextContent.Trim(), "Expected the 'Active' link to be the active one.");

		var otherLinks = cut.FindAll("a.nav-link:not(.active)");
		Assert.HasCount(1, otherLinks, "Expected exactly one non-active nav-link.");
	}
}
