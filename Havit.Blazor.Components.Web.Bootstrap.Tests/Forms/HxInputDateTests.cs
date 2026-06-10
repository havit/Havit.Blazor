using System.Globalization;
using System.Linq.Expressions;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

public class HxInputDateTests : BunitTestBase
{
	[Fact]
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
			Assert.False(button.Find("button").HasAttribute("disabled"), $"Button {button.Instance.Text} should not be disabled.");
		}
		Assert.DoesNotContain("disabled", cut.Markup);
	}

	[Fact]
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
		Assert.Equal(new DateTime(2020, 2, 10), myValue);
	}

	[Fact]
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
		Assert.Null(myValue);
		Assert.True(cut.Find("input").ClassList.Contains(HxInputBase<object>.InvalidCssClass), "Input element should have the invalid CSS class.");
		Assert.Equal("TestParsingErrorMessage", cut.Find("div.invalid-feedback").TextContent);
	}

	[Fact]
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
		Assert.Null(myValue);
	}

	[Fact]
	public void HxInputDate_LabelTypeFloating_ThrowsInvalidOperationException()
	{
		// LabelType.Floating is not supported in Bootstrap 6 — the form-adorn wrapper owns the visual chrome and cannot host a floating label.

		// Arrange
		DateTime? myValue = null;

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputDate<DateTime?>>(0);
			builder.AddAttribute(1, "Value", myValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<DateTime?>(this, (value) => { myValue = value; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<DateTime?>>)(() => myValue));
			builder.AddAttribute(4, nameof(HxInputDate<DateTime?>.Label), "Date");
			builder.AddAttribute(5, nameof(HxInputDate<DateTime?>.LabelType), (LabelType?)LabelType.Floating);
			builder.CloseComponent();
		};

		// Act + Assert
		Assert.Throws<InvalidOperationException>(() => Render(componentRenderer));
	}

	[Fact]
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
		Assert.Equal(new DateTime(2020, 2, 10), myValue);
		Assert.Equal("", cut.Find("input").GetAttribute("value"));
		Assert.NotNull(cut.Find($"div.{HxInputBase<object>.InvalidCssClass}"));
		Assert.Equal("TestParsingErrorMessage", cut.Find("div.invalid-feedback").TextContent);
	}
}
