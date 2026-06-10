using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxAutosuggestTests : BunitTestBase
{
	[Fact]
	public void HxAutosuggest_Render_DisplaysInputField()
	{
		// Arrange
		string selectedValue = null;

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxAutosuggest<string, string>>(0);
			builder.AddAttribute(1, "Value", selectedValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<string>(this, v => selectedValue = v));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<string>>)(() => selectedValue));
			builder.AddAttribute(4, "TextSelector", (Func<string, string>)(x => x));
			builder.AddAttribute(5, "ValueSelector", (Func<string, string>)(x => x));
			builder.AddAttribute(6, "DataProvider", (AutosuggestDataProviderDelegate<string>)(async request =>
				new AutosuggestDataProviderResult<string> { Data = Enumerable.Empty<string>() }));
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert — input field is rendered
		var input = cut.Find("input[type='text']");
		Assert.NotNull(input);
	}

	[Fact]
	public void HxAutosuggest_Placeholder_RendersOnInput()
	{
		// Arrange
		string selectedValue = null;

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxAutosuggest<string, string>>(0);
			builder.AddAttribute(1, "Value", selectedValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<string>(this, v => selectedValue = v));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<string>>)(() => selectedValue));
			builder.AddAttribute(4, "TextSelector", (Func<string, string>)(x => x));
			builder.AddAttribute(5, "ValueSelector", (Func<string, string>)(x => x));
			builder.AddAttribute(6, "DataProvider", (AutosuggestDataProviderDelegate<string>)(async request =>
				new AutosuggestDataProviderResult<string> { Data = Enumerable.Empty<string>() }));
			builder.AddAttribute(7, "Placeholder", "Search...");
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert — placeholder is set on input
		var input = cut.Find("input[type='text']");
		Assert.Equal("Search...", input.GetAttribute("placeholder"));
	}

	[Fact]
	public void HxAutosuggest_Label_RendersLabel()
	{
		// Arrange
		string selectedValue = null;

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxAutosuggest<string, string>>(0);
			builder.AddAttribute(1, "Value", selectedValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<string>(this, v => selectedValue = v));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<string>>)(() => selectedValue));
			builder.AddAttribute(4, "TextSelector", (Func<string, string>)(x => x));
			builder.AddAttribute(5, "ValueSelector", (Func<string, string>)(x => x));
			builder.AddAttribute(6, "DataProvider", (AutosuggestDataProviderDelegate<string>)(async request =>
				new AutosuggestDataProviderResult<string> { Data = Enumerable.Empty<string>() }));
			builder.AddAttribute(7, "Label", "Choose item");
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert — label is rendered
		var label = cut.Find("label");
		Assert.Contains("Choose item", label.TextContent);
	}

	[Fact]
	public void HxAutosuggest_Enabled_False_DisablesInput()
	{
		// Arrange
		string selectedValue = null;

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxAutosuggest<string, string>>(0);
			builder.AddAttribute(1, "Value", selectedValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<string>(this, v => selectedValue = v));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<string>>)(() => selectedValue));
			builder.AddAttribute(4, "TextSelector", (Func<string, string>)(x => x));
			builder.AddAttribute(5, "ValueSelector", (Func<string, string>)(x => x));
			builder.AddAttribute(6, "DataProvider", (AutosuggestDataProviderDelegate<string>)(async request =>
				new AutosuggestDataProviderResult<string> { Data = Enumerable.Empty<string>() }));
			builder.AddAttribute(7, "Enabled", false);
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert — input should be disabled
		var input = cut.Find("input[type='text']");
		Assert.NotNull(input.GetAttribute("disabled"));
	}

	[Fact]
	public void HxAutosuggest_Render_HasInputStructure()
	{
		// Arrange
		string selectedValue = null;

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxAutosuggest<string, string>>(0);
			builder.AddAttribute(1, "Value", selectedValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<string>(this, v => selectedValue = v));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<string>>)(() => selectedValue));
			builder.AddAttribute(4, "TextSelector", (Func<string, string>)(x => x));
			builder.AddAttribute(5, "ValueSelector", (Func<string, string>)(x => x));
			builder.AddAttribute(6, "DataProvider", (AutosuggestDataProviderDelegate<string>)(async request =>
				new AutosuggestDataProviderResult<string> { Data = Enumerable.Empty<string>() }));
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert — the autosuggest input (the programmatic menu toggle) is rendered
		cut.Find("input.hx-autosuggest-input");
	}
}
