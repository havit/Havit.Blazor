using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

[TestClass]
public class HxAutosuggestTests : BunitTestBase
{
	[TestMethod]
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
		Assert.IsNotNull(input);
	}

	[TestMethod]
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
		Assert.AreEqual("Search...", input.GetAttribute("placeholder"));
	}

	[TestMethod]
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

	[TestMethod]
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
		Assert.IsNotNull(input.GetAttribute("disabled"));
	}

	[TestMethod]
	public void HxAutosuggest_Render_HasDropdownStructure()
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

		// Assert — dropdown structure is rendered
		cut.Find(".dropdown");
	}
}
