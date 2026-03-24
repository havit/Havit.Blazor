using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

[TestClass]
public class HxFormValueTests : BunitTestBase
{
	[TestMethod]
	public void HxFormValue_Render_DisplaysValueText()
	{
		// Arrange & Act
		var cut = RenderComponent<HxFormValue>(parameters => parameters
			.Add(p => p.Value, "TestValue"));

		// Assert
		cut.Find("div.form-control");
		Assert.IsTrue(cut.Markup.Contains("TestValue", StringComparison.Ordinal));
	}

	[TestMethod]
	public void HxFormValue_Label_DisplaysLabelText()
	{
		// Arrange & Act
		var cut = RenderComponent<HxFormValue>(parameters => parameters
			.Add(p => p.Label, "TestLabel")
			.Add(p => p.Value, "TestValue"));

		// Assert
		cut.Find("label");
		Assert.IsTrue(cut.Markup.Contains("TestLabel", StringComparison.Ordinal));
	}

	[TestMethod]
	public void HxFormValue_ValueTemplate_OverridesValueText()
	{
		// Arrange & Act
		var cut = RenderComponent<HxFormValue>(parameters => parameters
			.Add(p => p.ValueTemplate, (RenderTreeBuilder builder) =>
			{
				builder.OpenElement(0, "span");
				builder.AddAttribute(1, "id", "custom-value");
				builder.AddContent(2, "CustomTemplateContent");
				builder.CloseElement();
			}));

		// Assert
		cut.Find("span#custom-value");
		Assert.IsTrue(cut.Markup.Contains("CustomTemplateContent", StringComparison.Ordinal));
	}

	[TestMethod]
	public void HxFormValue_Hint_DisplaysHelpText()
	{
		// Arrange & Act
		var cut = RenderComponent<HxFormValue>(parameters => parameters
			.Add(p => p.Value, "TestValue")
			.Add(p => p.Hint, "TestHint"));

		// Assert
		cut.Find("div.form-text");
		Assert.IsTrue(cut.Markup.Contains("TestHint", StringComparison.Ordinal));
	}
}
