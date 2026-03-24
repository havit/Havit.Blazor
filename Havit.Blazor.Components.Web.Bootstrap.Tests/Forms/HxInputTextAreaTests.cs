using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

[TestClass]
public class HxInputTextAreaTests : BunitTestBase
{
	[TestMethod]
	public void HxInputTextArea_TypeText_UpdatesBoundValue()
	{
		// Arrange
		string currentValue = null;
		var cut = RenderComponent<HxInputTextArea>(parameters =>
			parameters.Bind(p =>
				p.Value,
				currentValue,
				newValue => currentValue = newValue));

		// Act
		cut.Find("textarea").Change("Line 1\nLine 2\nLine 3");

		// Assert
		Assert.AreEqual("Line 1\nLine 2\nLine 3", currentValue, "Bound value should be updated with multiline text.");
		Assert.AreEqual("Line 1\nLine 2\nLine 3", cut.Find("textarea").GetAttribute("value"), "Textarea value attribute should reflect the entered text.");
	}

	[TestMethod]
	public void HxInputTextArea_RequiredEmpty_ShowsValidationError()
	{
		// Arrange
		var model = new FormModel();

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<EditForm>(0);
			builder.AddAttribute(1, "Model", model);
			builder.AddAttribute(2, "ChildContent", (RenderFragment<EditContext>)((context) => (builder2) =>
			{
				builder2.OpenComponent<DataAnnotationsValidator>(0);
				builder2.CloseComponent();

				builder2.OpenComponent<HxInputTextArea>(1);
				builder2.AddAttribute(2, "Value", model.Description);
				builder2.AddAttribute(3, "ValueChanged", EventCallback.Factory.Create<string>(this, (value) => { model.Description = value; }));
				builder2.AddAttribute(4, "ValueExpression", (Expression<Func<string>>)(() => model.Description));
				builder2.CloseComponent();
			}));
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);
		cut.Find("form").Submit();

		// Assert
		Assert.IsNotNull(cut.Find($"div.{HxInputBase<object>.InvalidCssClass}"), "Component should display invalid CSS class.");
		Assert.IsTrue(cut.Find("div.invalid-feedback").TextContent.Length > 0, "Validation error message should be displayed.");
	}

	[TestMethod]
	public void HxInputTextArea_AdditionalAttributes_RenderedOnTextarea()
	{
		// Arrange
		string currentValue = null;

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputTextArea>(0);
			builder.AddAttribute(1, "Value", currentValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<string>(this, (value) => { currentValue = value; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<string>>)(() => currentValue));
			builder.AddAttribute(4, "rows", "5");
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert
		var textarea = cut.Find("textarea");
		Assert.AreEqual("5", textarea.GetAttribute("rows"), "Additional HTML attributes should be splatted onto the textarea element.");
	}

	private class FormModel
	{
		[Required]
		public string Description { get; set; }
	}
}
