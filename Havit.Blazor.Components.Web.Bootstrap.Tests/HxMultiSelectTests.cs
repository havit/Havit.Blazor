using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxMultiSelectTests : BunitTestBase
{
	[Fact]
	public void HxMultiSelect_Render_DisplaysComboboxStructure()
	{
		// Arrange
		var items = new[] { "Apple", "Banana", "Cherry" };
		var selectedValues = new List<string>();

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxMultiSelect<string, string>>(0);
			builder.AddAttribute(1, "Data", (IEnumerable<string>)items);
			builder.AddAttribute(2, "TextSelector", (Func<string, string>)(x => x));
			builder.AddAttribute(3, "ValueSelector", (Func<string, string>)(x => x));
			builder.AddAttribute(4, "Value", selectedValues);
			builder.AddAttribute(5, "ValueChanged", EventCallback.Factory.Create<List<string>>(this, v => { }));
			builder.AddAttribute(6, "ValueExpression", (Expression<Func<List<string>>>)(() => selectedValues));
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert — renders the hx-multi-select container with the Bootstrap combobox toggle
		cut.Find(".hx-multi-select");
		cut.Find("button.combobox-toggle");

		// Assert — renders a listbox with one option per item
		var options = cut.FindAll("[role='option']");
		Assert.Equal(3, options.Count);
	}

	[Fact]
	public void HxMultiSelect_SelectedValues_OptionsAreMarkedSelected()
	{
		// Arrange
		var items = new[] { "Apple", "Banana", "Cherry" };
		var selectedValues = new List<string> { "Banana" };

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxMultiSelect<string, string>>(0);
			builder.AddAttribute(1, "Data", (IEnumerable<string>)items);
			builder.AddAttribute(2, "TextSelector", (Func<string, string>)(x => x));
			builder.AddAttribute(3, "ValueSelector", (Func<string, string>)(x => x));
			builder.AddAttribute(4, "Value", selectedValues);
			builder.AddAttribute(5, "ValueChanged", EventCallback.Factory.Create<List<string>>(this, v => { }));
			builder.AddAttribute(6, "ValueExpression", (Expression<Func<List<string>>>)(() => selectedValues));
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert — exactly one option should be marked as selected
		var selectedOptions = cut.FindAll("[role='option'][aria-selected='true']");
		Assert.Single(selectedOptions);
		Assert.Contains("Banana", selectedOptions[0].TextContent);
	}

	[Fact]
	public void HxMultiSelect_Label_RendersLabel()
	{
		// Arrange
		var items = new[] { "Apple" };
		var selectedValues = new List<string>();

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxMultiSelect<string, string>>(0);
			builder.AddAttribute(1, "Data", (IEnumerable<string>)items);
			builder.AddAttribute(2, "TextSelector", (Func<string, string>)(x => x));
			builder.AddAttribute(3, "ValueSelector", (Func<string, string>)(x => x));
			builder.AddAttribute(4, "Value", selectedValues);
			builder.AddAttribute(5, "ValueChanged", EventCallback.Factory.Create<List<string>>(this, v => { }));
			builder.AddAttribute(6, "ValueExpression", (Expression<Func<List<string>>>)(() => selectedValues));
			builder.AddAttribute(7, "Label", "Fruits");
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert — label is rendered
		var label = cut.Find("label");
		Assert.Contains("Fruits", label.TextContent);
	}

	[Fact]
	public void HxMultiSelect_NullData_RendersWithNullDataText()
	{
		// Arrange
		var selectedValues = new List<string>();

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxMultiSelect<string, string>>(0);
			builder.AddAttribute(1, "TextSelector", (Func<string, string>)(x => x));
			builder.AddAttribute(2, "ValueSelector", (Func<string, string>)(x => x));
			builder.AddAttribute(3, "Value", selectedValues);
			builder.AddAttribute(4, "ValueChanged", EventCallback.Factory.Create<List<string>>(this, v => { }));
			builder.AddAttribute(5, "ValueExpression", (Expression<Func<List<string>>>)(() => selectedValues));
			builder.AddAttribute(6, "NullDataText", "No data available");
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert — renders the null-data text in the toggle and no options
		var toggleValue = cut.Find(".combobox-value");
		Assert.Contains("No data available", toggleValue.TextContent);
		Assert.Empty(cut.FindAll("[role='option']"));
	}

	[Fact]
	public void HxMultiSelect_Enabled_False_DisablesToggle()
	{
		// Arrange
		var items = new[] { "Apple" };
		var selectedValues = new List<string>();

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxMultiSelect<string, string>>(0);
			builder.AddAttribute(1, "Data", (IEnumerable<string>)items);
			builder.AddAttribute(2, "TextSelector", (Func<string, string>)(x => x));
			builder.AddAttribute(3, "ValueSelector", (Func<string, string>)(x => x));
			builder.AddAttribute(4, "Value", selectedValues);
			builder.AddAttribute(5, "ValueChanged", EventCallback.Factory.Create<List<string>>(this, v => { }));
			builder.AddAttribute(6, "ValueExpression", (Expression<Func<List<string>>>)(() => selectedValues));
			builder.AddAttribute(7, "Enabled", false);
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert — the toggle should be disabled
		var toggle = cut.Find("button.combobox-toggle");
		Assert.NotNull(toggle.GetAttribute("disabled"));
	}

	[Fact]
	public void HxMultiSelect_Render_EmitsAccessibleListboxRoles()
	{
		// Arrange
		var items = new[] { "Apple", "Banana", "Cherry" };
		var selectedValues = new List<string>();

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxMultiSelect<string, string>>(0);
			builder.AddAttribute(1, "Data", (IEnumerable<string>)items);
			builder.AddAttribute(2, "TextSelector", (Func<string, string>)(x => x));
			builder.AddAttribute(3, "ValueSelector", (Func<string, string>)(x => x));
			builder.AddAttribute(4, "Value", selectedValues);
			builder.AddAttribute(5, "ValueChanged", EventCallback.Factory.Create<List<string>>(this, v => { }));
			builder.AddAttribute(6, "ValueExpression", (Expression<Func<List<string>>>)(() => selectedValues));
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert — the menu is an accessible multi-selectable listbox
		var listbox = cut.Find("[role='listbox']");
		Assert.Equal("true", listbox.GetAttribute("aria-multiselectable"));

		// Assert — the toggle is wired up to the listbox popup
		var toggle = cut.Find("button.combobox-toggle");
		Assert.Equal("listbox", toggle.GetAttribute("aria-haspopup"));
		Assert.Equal(listbox.Id, toggle.GetAttribute("aria-controls"));

		Assert.Equal(3, cut.FindAll("[role='option']").Count);
	}
}
