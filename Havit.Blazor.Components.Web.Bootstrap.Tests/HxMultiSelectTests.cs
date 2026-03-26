using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

[TestClass]
public class HxMultiSelectTests : BunitTestBase
{
	[TestMethod]
	public void HxMultiSelect_Render_DisplaysDropdownStructure()
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

		// Assert — renders the hx-multi-select container
		cut.Find(".hx-multi-select");

		// Assert — renders dropdown menu with items as checkboxes
		var checkboxes = cut.FindAll("input[type='checkbox']");
		Assert.HasCount(3, checkboxes);
	}

	[TestMethod]
	public void HxMultiSelect_SelectedValues_CheckboxesAreChecked()
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

		// Assert — exactly one checkbox should be checked
		var checkedBoxes = cut.FindAll("input[type='checkbox'][checked]");
		Assert.HasCount(1, checkedBoxes);
	}

	[TestMethod]
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

	[TestMethod]
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

		// Assert — renders without checkboxes
		var checkboxes = cut.FindAll("input[type='checkbox']");
		Assert.IsEmpty(checkboxes);
	}

	[TestMethod]
	public void HxMultiSelect_Enabled_False_DisablesInput()
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

		// Assert — the input should be disabled
		var input = cut.Find("input[type='text']");
		Assert.IsNotNull(input.GetAttribute("disabled"));
	}
}
