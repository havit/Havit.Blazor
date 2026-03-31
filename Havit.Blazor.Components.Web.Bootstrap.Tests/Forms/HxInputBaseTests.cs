using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

[TestClass]
public class HxInputBaseTests
{
	[TestMethod]
	public void HxInputBase_Renders_WithoutEditContext()
	{
		// Arrange
		var ctx = new Bunit.TestContext();
		var formData = new FormData();

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxComponent>(0);
			builder.AddAttribute(1, "Value", formData.StringValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<string>(this, (value) => { formData.StringValue = value; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<string>>)(() => formData.StringValue));
			builder.CloseComponent();
		};

		// Act
		ctx.Render(componentRenderer);

		// Assert			
		// No exception thrown
	}

	private class HxComponent : HxInputBase<string>
	{
		protected override void BuildRenderInput(RenderTreeBuilder builder)
		{
			// NOOP
		}

		protected override InputSettings GetSettings() => null;

		protected override bool TryParseValueFromString(string value, [MaybeNullWhen(false)] out string result, [NotNullWhen(false)] out string validationErrorMessage)
		{
			result = value;
			validationErrorMessage = null;
			return true;
		}
	}

	[TestMethod]
	public void HxInputBase_Renders_AriaDescribedBy_WhenHintProvided()
	{
		// Arrange — regression for #1110: input must have aria-describedby referencing hint
		using var ctx = new Bunit.TestContext();
		ctx.Services.AddSingleton(TimeProvider.System);
		ctx.Services.AddLocalization();
		ctx.Services.AddLogging();
		ctx.Services.AddHxServices();
		ctx.Services.AddHxMessenger();
		ctx.Services.AddHxMessageBoxHost();
		ctx.JSInterop.Mode = JSRuntimeMode.Loose;

		var formData = new FormData();

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<EditForm>(0);
			builder.AddAttribute(1, "Model", formData);
			builder.AddAttribute(2, "ChildContent", (RenderFragment<EditContext>)(editContext => (RenderTreeBuilder b) =>
			{
				b.OpenComponent<HxInputText>(0);
				b.AddAttribute(1, "Value", formData.StringValue);
				b.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<string>(this, v => formData.StringValue = v));
				b.AddAttribute(3, "ValueExpression", (Expression<Func<string>>)(() => formData.StringValue));
				b.AddAttribute(4, "Hint", "Help text for this field");
				b.CloseComponent();
			}));
			builder.CloseComponent();
		};

		// Act
		var cut = ctx.Render(componentRenderer);

		// Assert — the input should have aria-describedby attribute
		var input = cut.Find("input");
		var ariaDescribedBy = input.GetAttribute("aria-describedby");
		Assert.IsNotNull(ariaDescribedBy, "Input should have aria-describedby when Hint is provided.");
		Assert.AreNotEqual(string.Empty, ariaDescribedBy.Trim(), "aria-describedby should not be empty.");
	}

	private class FormData
	{
		public string StringValue { get; set; }
	}
}
