using System.Globalization;
using System.Linq.Expressions;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

[TestClass]
public class HxInputDateTests : BunitTestBase
{
	[TestMethod]
	public void HxInputDate_EnabledShouldOverrideFormStateForNestedControls_Issue877()
	{
		// https://github.com/havit/Havit.Blazor/issues/877

		// Arrange
		var myValue = new DateTime(2020, 2, 10);

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxFormState>(0);
			builder.AddAttribute(1, nameof(HxFormState.Enabled), false);

			builder.AddAttribute(1, nameof(HxFormState.ChildContent), (RenderFragment)((builder2) =>
			{
				builder2.OpenComponent<HxInputDate<DateTime>>(0);
				builder2.AddAttribute(1, "Value", myValue);
				builder2.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<DateTime>(this, (value) => { myValue = value; }));
				builder2.AddAttribute(3, "ValueExpression", (Expression<Func<DateTime>>)(() => myValue));
				builder2.AddAttribute(4, nameof(HxInputDate<DateTime>.Enabled), true);
				builder2.CloseComponent();
			}));

			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert
		var buttons = cut.FindComponents<HxButton>();
		foreach (var button in buttons)
		{
			Assert.IsFalse(button.Find("button").HasAttribute("disabled"), $"Button {button.Instance.Text} should not be disabled.");
		}
		Assert.DoesNotContain("disabled", cut.Markup, "There should be no disabled attribute in the rendered markup.");
	}

	[TestMethod]
	public void HxInputDate_TypeValidDate_UpdatesBoundValue()
	{
		// Arrange
		DateTime? myValue = null;

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputDate<DateTime?>>(0);
			builder.AddAttribute(1, "Value", myValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<DateTime?>(this, (value) => { myValue = value; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<DateTime?>>)(() => myValue));
			builder.CloseComponent();
		};

		using (CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo("en-US")))
		{
			var cut = Render(componentRenderer);

			// Act
			cut.Find("input").Change("2/10/2020");
		}

		// Assert
		Assert.AreEqual(new DateTime(2020, 2, 10), myValue, "Bound value should be updated to the parsed date.");
	}

	[TestMethod]
	public void HxInputDate_TypeInvalidDate_ShowsValidationError()
	{
		// Arrange
		DateTime? myValue = null;

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputDate<DateTime?>>(0);
			builder.AddAttribute(1, "Value", myValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<DateTime?>(this, (value) => { myValue = value; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<DateTime?>>)(() => myValue));
			builder.AddAttribute(4, "ParsingErrorMessage", "TestParsingErrorMessage");
			builder.CloseComponent();
		};

		var cut = Render(componentRenderer);

		// Act
		cut.Find("input").Change("not-a-date");

		// Assert
		Assert.IsNull(myValue, "Bound value should remain null after invalid input.");
		Assert.IsTrue(cut.Find("input").ClassList.Contains(HxInputBase<object>.InvalidCssClass), "Input element should have the invalid CSS class.");
		Assert.AreEqual("TestParsingErrorMessage", cut.Find("div.invalid-feedback").TextContent, "Parsing error message should be displayed.");
	}

	[TestMethod]
	public void HxInputDate_ClearInput_ResetsValueToNull()
	{
		// Arrange
		DateTime? myValue = new DateTime(2020, 2, 10);

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputDate<DateTime?>>(0);
			builder.AddAttribute(1, "Value", myValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<DateTime?>(this, (value) => { myValue = value; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<DateTime?>>)(() => myValue));
			builder.CloseComponent();
		};

		var cut = Render(componentRenderer);

		// Act
		cut.Find("input").Change("");

		// Assert
		Assert.IsNull(myValue, "Bound value should be reset to null after clearing the input.");
	}

	[TestMethod]
	public void HxInputDate_NonNullable_EmptyInputShouldRaiseParsingError_Issue892()
	{
		// https://github.com/havit/Havit.Blazor/issues/892

		// Arrange
		var myValue = new DateTime(2020, 2, 10);

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputDate<DateTime>>(0);
			builder.AddAttribute(1, "Value", myValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<DateTime>(this, (value) => { myValue = value; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<DateTime>>)(() => myValue));
			builder.AddAttribute(4, "ParsingErrorMessage", "TestParsingErrorMessage");
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);
		cut.Find("input").Change("");

		// Assert
		Assert.AreEqual(new DateTime(2020, 2, 10), myValue, "Model value should remain unchanged.");
		Assert.AreEqual("", cut.Find("input").GetAttribute("value"), "Input value should be empty.");
		Assert.IsNotNull(cut.Find($"div.{HxInputBase<object>.InvalidCssClass}"));
		Assert.AreEqual("TestParsingErrorMessage", cut.Find("div.invalid-feedback").TextContent, "ParsingValidationError should be displayed.");
	}
}
