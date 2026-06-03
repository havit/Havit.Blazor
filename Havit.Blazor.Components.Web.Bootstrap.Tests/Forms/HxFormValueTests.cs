using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

public class HxFormValueTests : BunitTestBase
{
	[Fact]
	public void HxFormValue_Render_DisplaysValueText()
	{
		// Arrange & Act
		var cut = RenderComponent<HxFormValue>(parameters => parameters
			.Add(p => p.Value, "TestValue"));

		// Assert
		var formControl = cut.Find("div.form-control");
		Assert.True(formControl.TextContent.Contains("TestValue", StringComparison.Ordinal));
	}

	[Fact]
	public void HxFormValue_Label_DisplaysLabelText()
	{
		// Arrange & Act
		var cut = RenderComponent<HxFormValue>(parameters => parameters
			.Add(p => p.Label, "TestLabel")
			.Add(p => p.Value, "TestValue"));

		// Assert
		var labels = cut.FindAll("label");
		Assert.Single(labels);
		Assert.Equal("TestLabel", labels[0].TextContent);
	}

	[Fact]
	public void HxFormValue_ValueTemplate_OverridesValueText()
	{
		// Arrange & Act
		var cut = RenderComponent<HxFormValue>(parameters => parameters
			.Add(p => p.Value, "PlainValue")
			.Add(p => p.ValueTemplate, (RenderTreeBuilder builder) =>
			{
				builder.OpenElement(0, "span");
				builder.AddAttribute(1, "id", "custom-value");
				builder.AddContent(2, "CustomTemplateContent");
				builder.CloseElement();
			}));

		// Assert
		var formControl = cut.Find("div.form-control");
		Assert.True(formControl.TextContent.Contains("CustomTemplateContent", StringComparison.Ordinal));
		Assert.False(formControl.TextContent.Contains("PlainValue", StringComparison.Ordinal));
	}

	[Fact]
	public void HxFormValue_Hint_DisplaysHelpText()
	{
		// Arrange & Act
		var cut = RenderComponent<HxFormValue>(parameters => parameters
			.Add(p => p.Value, "TestValue")
			.Add(p => p.Hint, "TestHint"));

		// Assert
		var hintElement = cut.Find("div.form-text");
		Assert.True(hintElement.TextContent.Contains("TestHint", StringComparison.Ordinal));
	}
}
