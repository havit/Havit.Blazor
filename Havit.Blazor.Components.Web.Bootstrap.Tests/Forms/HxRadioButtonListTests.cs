using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

public class HxRadioButtonListTests : BunitTestBase
{
	[Fact]
	public void HxRadioButtonList_Render_DisplaysOptionsFromData()
	{
		// Arrange
		var items = new List<string> { "Option A", "Option B", "Option C" };
		string selectedValue = null;

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxRadioButtonList<string, string>>(0);
			builder.AddAttribute(1, "Data", items);
			builder.AddAttribute(2, "Value", selectedValue);
			builder.AddAttribute(3, "ValueChanged", EventCallback.Factory.Create<string>(this, v => selectedValue = v));
			builder.AddAttribute(4, "ValueExpression", (Expression<Func<string>>)(() => selectedValue));
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert
		var inputs = cut.FindAll("input[type='radio']");
		Assert.Equal(3, inputs.Count());

		var labels = cut.FindAll("label");
		Assert.Equal(3, labels.Count());
		Assert.Equal("Option A", labels[0].TextContent);
		Assert.Equal("Option B", labels[1].TextContent);
		Assert.Equal("Option C", labels[2].TextContent);
	}

	[Fact]
	public void HxRadioButtonList_SelectOption_UpdatesBoundValue()
	{
		// Arrange
		var items = new List<string> { "Apple", "Banana", "Cherry" };
		string selectedValue = null;

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxRadioButtonList<string, string>>(0);
			builder.AddAttribute(1, "Data", items);
			builder.AddAttribute(2, "Value", selectedValue);
			builder.AddAttribute(3, "ValueChanged", EventCallback.Factory.Create<string>(this, v => selectedValue = v));
			builder.AddAttribute(4, "ValueExpression", (Expression<Func<string>>)(() => selectedValue));
			builder.CloseComponent();
		};

		var cut = Render(componentRenderer);

		// Act — click the second radio (Banana)
		cut.FindAll("input[type='radio']")[1].Click();

		// Assert
		Assert.Equal("Banana", selectedValue);
	}

	[Fact]
	public void HxRadioButtonList_SingleSelection_OnlyOneChecked()
	{
		// Arrange — pre-select "Green" so exactly one item is checked from the start
		var items = new List<string> { "Red", "Green", "Blue" };
		string selectedValue = "Green";

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxRadioButtonList<string, string>>(0);
			builder.AddAttribute(1, "Data", items);
			builder.AddAttribute(2, "Value", selectedValue);
			builder.AddAttribute(3, "ValueChanged", EventCallback.Factory.Create<string>(this, v => selectedValue = v));
			builder.AddAttribute(4, "ValueExpression", (Expression<Func<string>>)(() => selectedValue));
			builder.CloseComponent();
		};

		var cut = Render(componentRenderer);

		// Assert — exactly one input should be checked, and it should be "Green"
		var checkedInputs = cut.FindAll("input[type='radio'][checked]");
		Assert.Single(checkedInputs);

		var checkedLabel = cut.Find($"label[for='{checkedInputs[0].Id}']");
		Assert.Equal("Green", checkedLabel.TextContent);
	}

	[Fact]
	public void HxRadioButtonList_ToggleButtonMode_RendersBtnCheck()
	{
		// Arrange — regression for #1181: toggle button support
		var items = new List<string> { "Option A", "Option B" };
		string selectedValue = null;

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxRadioButtonList<string, string>>(0);
			builder.AddAttribute(1, "Data", items);
			builder.AddAttribute(2, "Value", selectedValue);
			builder.AddAttribute(3, "ValueChanged", EventCallback.Factory.Create<string>(this, v => selectedValue = v));
			builder.AddAttribute(4, "ValueExpression", (Expression<Func<string>>)(() => selectedValue));
			builder.AddAttribute(5, "RenderMode", RadioButtonListRenderMode.ToggleButtons);
			builder.AddAttribute(6, "Color", ThemeColor.Primary);
			builder.CloseComponent();
		};

		var cut = Render(componentRenderer);

		// Assert — inputs should have btn-check class instead of form-check-input
		var inputs = cut.FindAll("input[type='radio']");
		Assert.Equal(2, inputs.Count());
		foreach (var input in inputs)
		{
			Assert.True(input.ClassList.Contains("btn-check"), "Radio input should have btn-check class in toggle button mode.");
		}

		// Assert — labels should have btn class with color
		var labels = cut.FindAll("label");
		Assert.Equal(2, labels.Count());
		foreach (var label in labels)
		{
			Assert.True(label.ClassList.Contains("btn"), "Label should have btn class in toggle button mode.");
		}
	}
}
