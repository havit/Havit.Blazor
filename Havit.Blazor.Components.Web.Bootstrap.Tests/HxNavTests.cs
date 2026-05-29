using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxNavTests : BunitTestBase
{
	[Fact]
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
		Assert.Equal(2, navLinks.Count());
		Assert.Equal("Home", navLinks[0].TextContent.Trim());
		Assert.Equal("About", navLinks[1].TextContent.Trim());
	}

	[Fact]
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
		Assert.Single(activeLinks);
		var activeLink = activeLinks[0];
		Assert.Equal("Active", activeLink.TextContent.Trim());

		var otherLinks = cut.FindAll("a.nav-link:not(.active)");
		Assert.Single(otherLinks);
	}
}
