using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

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
	public void HxScrollspy_FirstRender_DefaultOptions_PassesNoOptions()
	{
		// Act
		Render<HxScrollspy>(parameters => parameters
			.Add(p => p.TargetId, "scrollspy-nav")
			.Add(p => p.SmoothScroll, true)
		);

		// Assert
		var options = GetJsOptions(JSInterop.Invocations.Single(i => i.Identifier == "initialize"));
		Assert.Empty(options); // topMargin, rootMargin and threshold not set => Bootstrap defaults apply; SmoothScroll is CSS only
	}

	[Fact]
	public void HxScrollspy_SmoothScroll_RendersSmoothScrollCssClass()
	{
		// HxScrollspyNavLink scrolls the container by JavaScript (scrollTo), the CSS scroll-behavior keeps that scrolling smooth.

		// Act
		var cut = Render<HxScrollspy>(parameters => parameters
			.Add(p => p.TargetId, "scrollspy-nav")
			.Add(p => p.SmoothScroll, true)
		);

		// Assert
		Assert.NotNull(cut.Find("div.hx-scrollspy.hx-scrollspy-smooth-scroll"));
	}

	[Fact]
	public void HxScrollspy_SmoothScrollNotSet_DoesNotRenderSmoothScrollCssClass()
	{
		// Act
		var cut = Render<HxScrollspy>(parameters => parameters
			.Add(p => p.TargetId, "scrollspy-nav")
		);

		// Assert
		Assert.Empty(cut.FindAll("div.hx-scrollspy-smooth-scroll"));
	}

	[Fact]
	public void HxScrollspy_FirstRender_PassesOptionsToJavaScript()
	{
		// Act
		Render<HxScrollspy>(parameters => parameters
			.Add(p => p.TargetId, "scrollspy-nav")
			.Add(p => p.TopMargin, "96px")
			.Add(p => p.RootMargin, "0px 0px -40%")
			.Add(p => p.Threshold, new[] { 0, 0.5, 1 })
			.Add(p => p.SmoothScroll, true)
		);

		// Assert
		var options = GetJsOptions(JSInterop.Invocations.Single(i => i.Identifier == "initialize"));
		Assert.Equal("96px", options["topMargin"]);
		Assert.Equal("0px 0px -40%", options["rootMargin"]);
		Assert.Equal(new[] { 0, 0.5, 1 }, (double[])options["threshold"]);
	}

	[Fact]
	public void HxScrollspy_OptionsChanged_ReinitializesJavaScript()
	{
		// Arrange
		var cut = Render<HxScrollspy>(parameters => parameters
			.Add(p => p.TargetId, "scrollspy-nav")
			.Add(p => p.Threshold, new[] { 0.0 })
		);

		// Act
		cut.Render(parameters => parameters
			.Add(p => p.RootMargin, "0px 0px -40%")
			.Add(p => p.Threshold, new[] { 0.0 }));

		// Assert
		var reinitializeInvocation = JSInterop.Invocations.Single(i => i.Identifier == "reinitialize");
		Assert.Equal("scrollspy-nav", reinitializeInvocation.Arguments[1]);
		Assert.Equal("0px 0px -40%", GetJsOptions(reinitializeInvocation)["rootMargin"]);
	}

	[Fact]
	public void HxScrollspy_OptionsChangedWhileInitializing_ReinitializesJavaScriptAfterInitialization()
	{
		// Arrange
		var module = JSInterop.SetupModule(_ => true);
		var initializeHandler = module.SetupVoid("initialize", _ => true); // pending until SetVoidResult()
		var cut = Render<HxScrollspy>(parameters => parameters
			.Add(p => p.TargetId, "scrollspy-nav")
			.Add(p => p.TopMargin, "12%")
		);

		// Act
		cut.Render(parameters => parameters.Add(p => p.TopMargin, "96px")); // rendered while initialize is pending
		cut.InvokeAsync(() => initializeHandler.SetVoidResult());

		// Assert
		cut.WaitForAssertion(() =>
		{
			var reinitializeInvocation = Assert.Single(module.Invocations, i => i.Identifier == "reinitialize");
			Assert.Equal("96px", GetJsOptions(reinitializeInvocation)["topMargin"]);
		});
	}

	[Fact]
	public void HxScrollspy_OptionsNotChanged_DoesNotReinitializeJavaScript()
	{
		// Arrange
		var cut = Render<HxScrollspy>(parameters => parameters
			.Add(p => p.TargetId, "scrollspy-nav")
			.Add(p => p.TopMargin, "96px")
			.Add(p => p.Threshold, new[] { 0, 1.0 })
		);

		// Act
		cut.Render(parameters => parameters
			.Add(p => p.TopMargin, "96px")
			.Add(p => p.Threshold, new[] { 0, 1.0 }) // new instance with the same values
			.Add(p => p.CssClass, "changed"));

		// Assert
		Assert.Single(JSInterop.Invocations, i => i.Identifier == "initialize");
		Assert.DoesNotContain(JSInterop.Invocations, i => i.Identifier == "reinitialize");
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
	public void HxScrollspyNavLink_Click_NavigatesToFragmentByJavaScriptWithoutNavigationManager()
	{
		// NavigationManager.NavigateTo() would scroll the target element into view by scrollIntoView(), which scrolls the whole page as well.

		// Arrange
		var navigationManager = Services.GetRequiredService<NavigationManager>();
		var uriBeforeClick = navigationManager.Uri;
		var cut = Render<HxScrollspyNavLink>(parameters => parameters
			.Add(l => l.Href, "#section1")
			.AddChildContent("Section 1"));

		// Act
		cut.Find("a").Click();

		// Assert
		var importInvocation = JSInterop.Invocations.Single(i => i.Identifier == "import");
		Assert.Contains("HxScrollspy.js", (string)importInvocation.Arguments[0]);
		var navigateInvocation = JSInterop.Invocations.Single(i => i.Identifier == "navigateToFragment");
		Assert.Equal("#section1", navigateInvocation.Arguments[0]);
		Assert.Equal(uriBeforeClick, navigationManager.Uri);
	}

	[Fact]
	public void HxScrollspyNavLink_Click_InvokesOnClickBeforeNavigation()
	{
		// Arrange
		var log = new List<string>();
		JSInterop.SetupVoid("navigateToFragment", _ => true).SetVoidResult();
		var cut = Render<HxScrollspyNavLink>(parameters => parameters
			.Add(l => l.Href, "#section1")
			.Add(l => l.OnClick, () => log.Add("OnClick")));

		// Act
		cut.Find("a").Click();

		// Assert
		Assert.Equal(["OnClick"], log);
		Assert.Single(JSInterop.Invocations, i => i.Identifier == "navigateToFragment");
	}

	[Fact]
	public void HxScrollspyNavLink_ClicksWhileImportPending_ImportJavaScriptOnce()
	{
		// a second import would replace the first JS object reference, which would never be released

		// Arrange
		var jsRuntime = new PendingImportJSRuntime(); // bUnit JSInterop cannot keep an import pending
		Services.AddSingleton<IJSRuntime>(jsRuntime);
		var cut = Render<HxScrollspyNavLink>(parameters => parameters
			.Add(l => l.Href, "#section1"));

		// Act
		cut.Find("a").Click();
		cut.Find("a").Click();

		// Assert
		Assert.Equal(1, jsRuntime.ImportCount);
	}

	private class FailingFirstImportJSRuntime : IJSRuntime
	{
		public int ImportCount { get; private set; }
		public int NavigateToFragmentCount { get; private set; }

		public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object[] args)
		{
			return InvokeAsync<TValue>(identifier, CancellationToken.None, args);
		}

		public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object[] args)
		{
			if (identifier == "import")
			{
				ImportCount++;
				if (ImportCount == 1)
				{
					return ValueTask.FromException<TValue>(new JSException("Import failed."));
				}
				return ValueTask.FromResult((TValue)(object)new ModuleReference(this));
			}
			return ValueTask.FromResult(default(TValue));
		}

		private class ModuleReference(FailingFirstImportJSRuntime owner) : IJSObjectReference
		{
			public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object[] args)
			{
				return InvokeAsync<TValue>(identifier, CancellationToken.None, args);
			}

			public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object[] args)
			{
				if (identifier == "navigateToFragment")
				{
					owner.NavigateToFragmentCount++;
				}
				return ValueTask.FromResult(default(TValue));
			}

			public ValueTask DisposeAsync() => ValueTask.CompletedTask;
		}
	}

	private class PendingImportJSRuntime : IJSRuntime
	{
		public int ImportCount { get; private set; }

		public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object[] args)
		{
			return InvokeAsync<TValue>(identifier, CancellationToken.None, args);
		}

		public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object[] args)
		{
			if (identifier == "import")
			{
				ImportCount++;
			}
			return new ValueTask<TValue>(new TaskCompletionSource<TValue>().Task); // never completes
		}
	}

	[Fact]
	public async Task HxScrollspyNavLink_ImportFailed_NextClickImportsAgain()
	{
		// Arrange
		var jsRuntime = new FailingFirstImportJSRuntime();
		Services.AddSingleton<IJSRuntime>(jsRuntime);
		var cut = Render<HxScrollspyNavLink>(parameters => parameters
			.Add(l => l.Href, "#section1"));

		// Act
		await Assert.ThrowsAsync<JSException>(() => cut.Find("a").ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs()));
		await cut.Find("a").ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

		// Assert
		Assert.Equal(2, jsRuntime.ImportCount);
		Assert.Equal(1, jsRuntime.NavigateToFragmentCount);
	}

	[Fact]
	public async Task HxScrollspyNavLink_ClickAfterDispose_DoesNotImportJavaScript()
	{
		// e.g. OnClick removes the link - a module imported after disposal would never be released

		// Arrange
		var cut = Render<HxScrollspyNavLink>(parameters => parameters
			.Add(l => l.Href, "#section1"));
		var link = cut.Find("a");
		await cut.Instance.DisposeAsync();

		// Act
		await link.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

		// Assert
		Assert.DoesNotContain(JSInterop.Invocations, i => i.Identifier == "import");
		Assert.DoesNotContain(JSInterop.Invocations, i => i.Identifier == "navigateToFragment");
	}

	[Theory]
	[InlineData("CssClass")]
	[InlineData("CssClassWithTab")]
	[InlineData("class")]
	[InlineData("classWithNewLine")]
	[InlineData("disabled")]
	[InlineData("aria-disabled")]
	public void HxScrollspyNavLink_DisabledClick_DoesNotNavigateNorInvokeOnClick(string disabledBy)
	{
		// e.g. keyboard activation - the .disabled class (pointer-events: none) blocks the mouse only

		// Arrange
		var onClickInvoked = false;
		var cut = Render<HxScrollspyNavLink>(parameters =>
		{
			parameters
				.Add(l => l.Href, "#section1")
				.Add(l => l.OnClick, () => onClickInvoked = true);
			_ = disabledBy switch
			{
				"CssClass" => parameters.Add(l => l.CssClass, "ms-3 disabled"),
				"CssClassWithTab" => parameters.Add(l => l.CssClass, "ms-3\tdisabled"),
				"class" => parameters.AddUnmatched("class", "list-group-item disabled"),
				"classWithNewLine" => parameters.AddUnmatched("class", "list-group-item\ndisabled"),
				"disabled" => parameters.AddUnmatched("disabled", true),
				"aria-disabled" => parameters.AddUnmatched("aria-disabled", "true"),
				_ => throw new ArgumentOutOfRangeException(nameof(disabledBy))
			};
		});

		// Act
		cut.Find("a").Click();

		// Assert
		Assert.False(onClickInvoked);
		Assert.DoesNotContain(JSInterop.Invocations, i => i.Identifier == "navigateToFragment");
	}

	[Fact]
	public void HxScrollspyNavLink_HrefWithoutHash_Throws()
	{
		// Act & Assert
		Assert.Throws<InvalidOperationException>(() => Render<HxScrollspyNavLink>(parameters => parameters
			.Add(l => l.Href, "/page")
		));
	}

	private static Dictionary<string, object> GetJsOptions(JSRuntimeInvocation invocation)
	{
		return Assert.IsType<Dictionary<string, object>>(invocation.Arguments[2]);
	}
}
