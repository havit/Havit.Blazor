using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms.SearchBox;

[TestClass]
public class HxSearchBoxTests : BunitTestBase
{
	[TestMethod]
	public void HxSearchBox_EnabledFalse_ShouldRenderDisabledAttribute_Issue941()
	{
		// https://github.com/havit/Havit.Blazor/issues/941

		// Arrange
		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxSearchBox<string>>(0);
			builder.AddAttribute(1, "Enabled", false);
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert
		Assert.IsTrue(cut.Find("input").HasAttribute("disabled"));
	}

	[TestMethod]
	public void HxSearchBox_KeyboardShortcutEnabled_ShouldRenderKeyboardHint()
	{
		// Arrange
		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxSearchBox<string>>(0);
			builder.AddAttribute(1, "KeyboardShortcut", true);
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert
		var keyboardHint = cut.Find(".hx-search-box-keyboard-hint");
		Assert.IsNotNull(keyboardHint);
		Assert.IsNotNull(keyboardHint.QuerySelector(".hx-search-box-keyboard-hint-text"));
	}

	[TestMethod]
	public void HxSearchBox_KeyboardShortcutDisabled_ShouldNotRenderKeyboardHint()
	{
		// Arrange
		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxSearchBox<string>>(0);
			builder.AddAttribute(1, "KeyboardShortcut", false);
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert
		var keyboardHint = cut.QuerySelector(".hx-search-box-keyboard-hint");
		Assert.IsNull(keyboardHint);
	}

	[TestMethod]
	public void HxSearchBox_CustomKeyboardShortcutHint_ShouldRenderCustomText()
	{
		// Arrange
		const string customHint = "⌘K";
		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxSearchBox<string>>(0);
			builder.AddAttribute(1, "KeyboardShortcut", true);
			builder.AddAttribute(2, "KeyboardShortcutHint", customHint);
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert
		var keyboardHintText = cut.Find(".hx-search-box-keyboard-hint-text");
		Assert.AreEqual(customHint, keyboardHintText.TextContent);
	}
}
