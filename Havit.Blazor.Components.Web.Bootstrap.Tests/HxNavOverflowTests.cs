using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxNavOverflowTests : BunitTestBase
{
	[Fact]
	public void HxNavOverflow_Render_BasicStructure()
	{
		// Act
		var cut = RenderComponent<HxNavOverflow>(parameters => parameters
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
		var nav = cut.Find("nav");
		Assert.True(nav.ClassList.Contains("nav"));
		Assert.True(nav.ClassList.Contains("nav-overflow"));

		// nav copy of the items
		var navLinks = cut.FindAll("nav > a.nav-link");
		Assert.Equal(2, navLinks.Count);

		// "More" toggle (initially hidden, JavaScript decides the visibility)
		var moreItem = cut.Find("nav > .nav-overflow-item");
		Assert.Equal("true", moreItem.GetAttribute("data-bs-nav-overflow"));
		var toggle = cut.Find(".nav-overflow-toggle");
		Assert.Equal("menu", toggle.GetAttribute("data-bs-toggle"));
		Assert.Equal("bottom-end", toggle.GetAttribute("data-bs-placement"));
		Assert.Equal("More", toggle.QuerySelector(".nav-overflow-text").TextContent.Trim());

		// menu copy of the items (HxNavLink renders as menu-item)
		var menuItems = cut.FindAll(".nav-overflow-menu > a.menu-item");
		Assert.Equal(2, menuItems.Count);
		Assert.Equal("Home", menuItems[0].TextContent.Trim());
	}

	[Fact]
	public void HxNavOverflow_Variant_RendersVariantCssClass()
	{
		// Act
		var cut = RenderComponent<HxNavOverflow>(parameters => parameters
			.Add(c => c.Variant, NavVariant.Pills)
			.AddChildContent<HxNavLink>(link => link.Add(l => l.Text, "Home").Add(l => l.Href, "/"))
		);

		// Assert
		Assert.True(cut.Find("nav").ClassList.Contains("nav-pills"));
	}

	[Fact]
	public void HxNavOverflow_MoreTextAndIcon_AreRendered()
	{
		// Act
		var cut = RenderComponent<HxNavOverflow>(parameters => parameters
			.Add(c => c.MoreText, "See all")
			.Add(c => c.MoreIcon, BootstrapIcon.ThreeDotsVertical)
			.AddChildContent<HxNavLink>(link => link.Add(l => l.Text, "Home").Add(l => l.Href, "/"))
		);

		// Assert
		var toggle = cut.Find(".nav-overflow-toggle");
		Assert.Equal("See all", toggle.QuerySelector(".nav-overflow-text").TextContent.Trim());
		Assert.NotNull(toggle.QuerySelector(".nav-overflow-icon .bi-three-dots-vertical"));
	}

	[Fact]
	public void HxNavOverflow_MoreTextEmpty_RendersIconOnlyToggle()
	{
		// Act
		var cut = RenderComponent<HxNavOverflow>(parameters => parameters
			.Add(c => c.MoreText, "")
			.AddChildContent<HxNavLink>(link => link.Add(l => l.Text, "Home").Add(l => l.Href, "/"))
		);

		// Assert
		var toggle = cut.Find(".nav-overflow-toggle");
		Assert.Null(toggle.QuerySelector(".nav-overflow-text"));
		Assert.NotNull(toggle.QuerySelector(".nav-overflow-icon"));
	}

	[Fact]
	public void HxNavOverflow_IconPlacementEnd_RendersIconAfterText()
	{
		// Act
		var cut = RenderComponent<HxNavOverflow>(parameters => parameters
			.Add(c => c.IconPlacement, NavOverflowIconPlacement.End)
			.AddChildContent<HxNavLink>(link => link.Add(l => l.Text, "Home").Add(l => l.Href, "/"))
		);

		// Assert
		var toggleChildren = cut.Find(".nav-overflow-toggle").Children;
		Assert.True(toggleChildren[0].ClassList.Contains("nav-overflow-text"));
		Assert.True(toggleChildren[1].ClassList.Contains("nav-overflow-icon"));
	}

	[Fact]
	public void HxNavOverflow_MenuPlacement_RendersDataBsPlacement()
	{
		// Act
		var cut = RenderComponent<HxNavOverflow>(parameters => parameters
			.Add(c => c.MenuPlacement, MenuPlacement.TopStart)
			.AddChildContent<HxNavLink>(link => link.Add(l => l.Text, "Home").Add(l => l.Href, "/"))
		);

		// Assert
		Assert.Equal("top-start", cut.Find(".nav-overflow-toggle").GetAttribute("data-bs-placement"));
	}

	[Fact]
	public void HxNavOverflow_ActiveRoute_LinkIsActiveInBothCopies()
	{
		// Arrange
		Services.GetRequiredService<Bunit.TestDoubles.FakeNavigationManager>().NavigateTo("http://localhost/active-page");

		// Act
		var cut = RenderComponent<HxNavOverflow>(parameters => parameters
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
		Assert.Single(cut.FindAll("nav > a.nav-link.active"));
		Assert.Single(cut.FindAll(".nav-overflow-menu > a.menu-item.active"));
	}

	[Fact]
	public void HxNavOverflow_Render_InitializesJavaScript()
	{
		// Act
		var cut = RenderComponent<HxNavOverflow>(parameters => parameters
			.Add(c => c.MinimumVisibleItems, 2)
			.AddChildContent<HxNavLink>(link => link.Add(l => l.Text, "Home").Add(l => l.Href, "/"))
		);

		// Assert
		var module = JSInterop.VerifyInvoke("import");
		Assert.Contains("HxNavOverflow.js", (string)module.Arguments[0]);
		Assert.Contains(JSInterop.Invocations, invocation => invocation.Identifier == "initialize");
	}

	[Fact]
	public async Task HxNavOverflow_HandleOverflowChanged_RaisesOnOverflowChanged()
	{
		// Arrange
		NavOverflowChangedEventArgs eventArgs = null;
		var cut = RenderComponent<HxNavOverflow>(parameters => parameters
			.Add(c => c.OnOverflowChanged, (NavOverflowChangedEventArgs args) => { eventArgs = args; })
			.AddChildContent<HxNavLink>(link => link.Add(l => l.Text, "Home").Add(l => l.Href, "/"))
		);

		// Act
		await cut.InvokeAsync(() => cut.Instance.HandleOverflowChanged(overflowCount: 3, visibleCount: 5));

		// Assert
		Assert.NotNull(eventArgs);
		Assert.Equal(3, eventArgs.OverflowCount);
		Assert.Equal(5, eventArgs.VisibleCount);
	}

	[Fact]
	public async Task HxNavOverflow_UpdateAsync_InvokesJavaScriptUpdate()
	{
		// Arrange
		var cut = RenderComponent<HxNavOverflow>(parameters => parameters
			.AddChildContent<HxNavLink>(link => link.Add(l => l.Text, "Home").Add(l => l.Href, "/"))
		);

		// Act
		await cut.InvokeAsync(() => cut.Instance.UpdateAsync());

		// Assert
		Assert.Contains(JSInterop.Invocations, invocation => invocation.Identifier == "update");
	}

	[Fact]
	public void HxNavLink_OutsideNavOverflow_RendersNavLinkCssClass()
	{
		// Act
		var cut = RenderComponent<HxNavLink>(parameters => parameters
			.Add(l => l.Text, "Home")
			.Add(l => l.Href, "/")
		);

		// Assert
		Assert.NotNull(cut.Find("a.nav-link"));
	}
}
