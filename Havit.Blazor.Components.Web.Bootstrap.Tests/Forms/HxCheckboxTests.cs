using System.Linq.Expressions;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

public class HxCheckboxTests : BunitTestBase
{
	[Fact]
	public void HxCheckbox_Render_InitiallyUnchecked()
	{
		// Arrange
		bool value = false;

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxCheckbox>(0);
			builder.AddAttribute(1, "Value", value);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<bool>(this, (v) => { value = v; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<bool>>)(() => value));
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert
		var input = cut.Find("input[type='checkbox']");
		Assert.False(input.HasAttribute("checked"));
	}

	[Fact]
	public void HxCheckbox_Click_ChecksAndUpdatesValue()
	{
		// Arrange
		bool value = false;

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxCheckbox>(0);
			builder.AddAttribute(1, "Value", value);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<bool>(this, (v) => { value = v; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<bool>>)(() => value));
			builder.CloseComponent();
		};

		var cut = Render(componentRenderer);

		// Act
		var input = cut.Find("input[type='checkbox']");
		input.Change(true);

		// Assert
		Assert.True(value);
	}

	[Fact]
	public void HxCheckbox_ClickChecked_UnchecksAndUpdatesValue()
	{
		// Arrange
		bool value = true;

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxCheckbox>(0);
			builder.AddAttribute(1, "Value", value);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<bool>(this, (v) => { value = v; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<bool>>)(() => value));
			builder.CloseComponent();
		};

		var cut = Render(componentRenderer);

		// Act
		var input = cut.Find("input[type='checkbox']");
		input.Change(false);

		// Assert
		Assert.False(value);
	}

	[Fact]
	public void HxCheckbox_Label_RendersCorrectText()
	{
		// Arrange
		bool value = false;
		string labelText = "My Checkbox Label";

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxCheckbox>(0);
			builder.AddAttribute(1, "Value", value);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<bool>(this, (v) => { value = v; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<bool>>)(() => value));
			builder.AddAttribute(4, "Text", labelText);
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert
		var label = cut.Find("div.form-field > label");
		Assert.Equal(labelText, label.TextContent);
	}
}
