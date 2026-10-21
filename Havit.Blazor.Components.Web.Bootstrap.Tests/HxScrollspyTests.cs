namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxScrollspyTests : BunitTestBase
{
	[Fact]
	public void HxScrollspy_Render_RendersFocusableContainerWithContent()
	{
		// Act
		var cut = Render<HxScrollspy>(parameters => parameters
			.Add(p => p.TargetId, "scrollspy-nav")
			.Add(p => p.CssClass, "custom-class")
			.AddChildContent("<h4 id=\"section1\">Section 1</h4>")
		);

		// Assert
		var container = cut.Find("div.hx-scrollspy.custom-class");
		Assert.Equal("0", container.GetAttribute("tabindex")); // keyboard access (Bootstrap recommendation)
		Assert.NotNull(container.QuerySelector("h4#section1"));
	}

	[Fact]
	public void HxScrollspy_FirstRender_InitializesJavaScriptWithTarget()
	{
		// Act
		Render<HxScrollspy>(parameters => parameters
			.Add(p => p.TargetId, "scrollspy-nav")
		);

		// Assert
		var importInvocation = JSInterop.Invocations.Single(i => i.Identifier == "import");
		Assert.Contains("HxScrollspy.js", (string)importInvocation.Arguments[0]);
		var initializeInvocation = JSInterop.Invocations.Single(i => i.Identifier == "initialize");
		Assert.Equal("scrollspy-nav", initializeInvocation.Arguments[1]);
	}

	[Fact]
	public async Task HxScrollspy_RefreshAsync_InvokesJavaScriptRefresh()
	{
		// Arrange
		var cut = Render<HxScrollspy>(parameters => parameters
			.Add(p => p.TargetId, "scrollspy-nav")
		);

		// Act
		await cut.InvokeAsync(() => cut.Instance.RefreshAsync());

		// Assert
		Assert.Single(JSInterop.Invocations, i => i.Identifier == "refresh");
	}

	[Fact]
	public async Task HxScrollspy_DisposeAsync_DisposesJavaScript()
	{
		// Arrange
		var cut = Render<HxScrollspy>(parameters => parameters
			.Add(p => p.TargetId, "scrollspy-nav")
		);

		// Act
		await cut.Instance.DisposeAsync();

		// Assert
		Assert.Single(JSInterop.Invocations, i => i.Identifier == "dispose");
	}

	[Fact]
	public void HxScrollspyNavLink_NestedHxNav_NestedNavFollowsParentLink()
	{
		// Bootstrap ScrollSpy activates parents of an active nested link by looking for the .nav-link
		// preceding the nested .nav (SelectorEngine.prev), so the nested nav has to be a sibling right after its parent link.

		// Act
		var cut = Render<HxNav>(parameters => parameters
			.Add(p => p.Id, "scrollspy-nav")
			.AddChildContent<HxScrollspyNavLink>(link => link
				.Add(l => l.Href, "#item-1")
				.AddChildContent("Item 1"))
			.AddChildContent<HxNav>(nested => nested
				.AddChildContent<HxScrollspyNavLink>(link => link
					.Add(l => l.Href, "#item-1-1")
					.AddChildContent("Item 1-1")))
		);

		// Assert
		var nestedLink = cut.Find("nav#scrollspy-nav > nav.nav > a.nav-link[href='#item-1-1']");
		var parentLink = nestedLink.ParentElement.PreviousElementSibling;
		Assert.True(parentLink.ClassList.Contains("nav-link"));
		Assert.Equal("#item-1", parentLink.GetAttribute("href"));
	}

	[Fact]
	public void HxScrollspyNavLink_HrefWithoutHash_Throws()
	{
		// Act & Assert
		Assert.Throws<InvalidOperationException>(() => Render<HxScrollspyNavLink>(parameters => parameters
			.Add(l => l.Href, "/page")
		));
	}
}
