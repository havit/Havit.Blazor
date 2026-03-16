namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

[TestClass]
public class HxModalTests : BunitTestBase
{
	[TestMethod]
	public async Task HxModal_HideShow_ShouldEnqueueShowAfterHide()
	{
		// Arrange
		var moduleInterop = JSInterop.SetupModule();
		moduleInterop.SetupVoid("show", _ => true);
		moduleInterop.SetupVoid("hide", _ => true);

		var cut = RenderComponent<HxModal>(parameters => parameters
			.Add(p => p.Title, "Test Modal")
			.Add(p => p.Animated, false)
			.Add(p => p.BodyTemplate, builder => builder.AddMarkupContent(0, "<p>Body</p>"))
		);

		var modal = cut.Instance;

		// Initial show
		await cut.InvokeAsync(() => modal.ShowAsync());
		cut.Render();
		await cut.InvokeAsync(() => modal.HandleModalShown());

		// Act: call HideAsync then ShowAsync in quick succession
		await cut.InvokeAsync(async () =>
		{
			await modal.HideAsync();
			await modal.ShowAsync(); // Bug: ShowAsync was silently skipped because _opened was still true
		});

		// Assert: show should have been called twice (initial + re-show after hide)
		var showCount = moduleInterop.Invocations.Count(inv => inv.Identifier == "show");
		Assert.AreEqual(2, showCount,
			"JS show should be called twice: once for the initial show, once for the re-show after hide.");
	}

	[TestMethod]
	public async Task HxModal_HideShowHide_ShouldEnqueueAllThreeCommands()
	{
		// Arrange
		var moduleInterop = JSInterop.SetupModule();
		moduleInterop.SetupVoid("show", _ => true);
		moduleInterop.SetupVoid("hide", _ => true);

		var cut = RenderComponent<HxModal>(parameters => parameters
			.Add(p => p.Title, "Test Modal")
			.Add(p => p.Animated, false)
			.Add(p => p.BodyTemplate, builder => builder.AddMarkupContent(0, "<p>Body</p>"))
		);

		var modal = cut.Instance;

		// Initial show
		await cut.InvokeAsync(() => modal.ShowAsync());
		cut.Render();
		await cut.InvokeAsync(() => modal.HandleModalShown());

		// Act: call HideAsync, ShowAsync, HideAsync in quick succession
		await cut.InvokeAsync(async () =>
		{
			await modal.HideAsync();
			await modal.ShowAsync();
			await modal.HideAsync();
		});

		// Assert
		var showCount = moduleInterop.Invocations.Count(inv => inv.Identifier == "show");
		var hideCount = moduleInterop.Invocations.Count(inv => inv.Identifier == "hide");

		Assert.AreEqual(2, showCount,
			"JS show should be called twice: initial show + re-show between the two hides.");
		Assert.AreEqual(2, hideCount,
			"JS hide should be called twice: once for the first hide, once for the second hide.");
	}

	[TestMethod]
	public async Task HxModal_ShowHide_NormalFlow_ShouldWork()
	{
		// Arrange
		var cut = RenderComponent<HxModal>(parameters => parameters
			.Add(p => p.Title, "Test Modal")
			.Add(p => p.Animated, false)
			.Add(p => p.BodyTemplate, builder => builder.AddMarkupContent(0, "<p>Body</p>"))
		);

		var modal = cut.Instance;

		// Act - Show
		await cut.InvokeAsync(() => modal.ShowAsync());
		cut.Render();

		// Verify rendered
		Assert.IsTrue(cut.Markup.Contains("<p>Body</p>"), "Modal body should be rendered after show.");

		// Act - Hide
		await cut.InvokeAsync(() => modal.HideAsync());
		cut.Render();

		// Simulate hidden callback
		await cut.InvokeAsync(() => modal.HandleModalHidden());

		// Assert
		Assert.IsFalse(cut.Markup.Contains("<p>Body</p>"), "Modal body should NOT be rendered after hide + hidden callback.");
	}
}
