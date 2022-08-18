using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms
{
	public class HxInputBaseTests
	{
		[TestClass]
		public class HxInputBase_Renders_WithoutEditContext_TestCase
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

				protected override IInputsSettings GetSettings() => null;

				protected override bool TryParseValueFromString(string value, [MaybeNullWhen(false)] out string result, [NotNullWhen(false)] out string validationErrorMessage)
				{
					result = value;
					validationErrorMessage = null;
					return true;
				}
			}

			private class FormData
			{
				public string StringValue { get; set; }
			}
		}
	}
}
