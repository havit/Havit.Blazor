using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq.Expressions;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

public class HxInputNumberTests : BunitTestBase
{
	[Theory]
	[InlineData("en-US", "123", 123.0, "123.00")]
	[InlineData("en-US", "123,123", 123123.0, "123123.00")]
	[InlineData("cs-CZ", "123,123", 123.12, "123,12")] // Decimals = 2 (rounding)
	[InlineData("en-US", "123.123", 123.12, "123.12")]
	[InlineData("cs-CZ", "123.123", 123.12, "123,12")] // replace . with , (if possible)
	[InlineData("en-US", "15,81.549", 1581.55, "1581.55")]
	[InlineData("cs-CZ", "15 81,549", 1581.55, "1581,55")]
	[InlineData("cs-CZ", "1.234", 1.23, "1,23")] // Replace . with , (if possible)
	[InlineData("en-US", "1.237", 1.24, "1.24")] // rounding
	[InlineData("en-US", "abc", null, "abc")] // invalid input
	[InlineData("en-US", "", null, null)] // empty input
	public void HxInputNumber_NullableDecimal_ValueConversions(string culture, string input, double? expectedValue, string expectedInputText)
	{
		// Arrange
		CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(culture);
		CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(culture);

		decimal? currentValue = null;
		var cut = RenderComponent<HxInputNumber<decimal?>>(parameters =>
			parameters.Bind(p =>
				p.Value,
				currentValue,
				newValue => currentValue = newValue));

		// act
		cut.Find("input").Change(input);

		// assert
		Assert.Equal((decimal?)expectedValue, cut.Instance.Value);
		Assert.Equal(expectedInputText, cut.Find("input").GetAttribute("value"));
	}

	[Fact]
	public void HxInputNumber_TypeValidNumber_UpdatesBoundValue()
	{
		// Arrange
		int currentValue = 0;
		var cut = RenderComponent<HxInputNumber<int>>(parameters =>
			parameters.Bind(p =>
				p.Value,
				currentValue,
				newValue => currentValue = newValue));

		// Act
		cut.Find("input").Change("42");

		// Assert
		Assert.Equal(42, currentValue);
	}

	[Fact]
	public void HxInputNumber_TypeNonNumeric_ShowsValidationError()
	{
		// Arrange
		var myValue = 0;

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputNumber<int>>(0);
			builder.AddAttribute(1, "Value", myValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<int>(this, (value) => { myValue = value; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<int>>)(() => myValue));
			builder.AddAttribute(4, "ParsingErrorMessage", "TestParsingErrorMessage");
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);
		cut.Find("input").Change("abc");

		// Assert
		Assert.Equal(0, myValue);
		Assert.NotNull(cut.Find($"div.{HxInputBase<object>.InvalidCssClass}"));
		Assert.Equal("TestParsingErrorMessage", cut.Find("div.invalid-feedback").TextContent);
	}

	[Fact]
	public void HxInputNumber_ValueOutOfRange_ShowsValidationError()
	{
		// Arrange
		var model = new RangeModel { Value = 5 };

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<EditForm>(0);
			builder.AddAttribute(1, "Model", model);
			builder.AddAttribute(2, "ChildContent", (RenderFragment<EditContext>)((editContext) => (builder2) =>
			{
				builder2.OpenComponent<DataAnnotationsValidator>(0);
				builder2.CloseComponent();

				builder2.OpenComponent<HxInputNumber<int>>(1);
				builder2.AddAttribute(1, "Value", model.Value);
				builder2.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<int>(this, (value) => { model.Value = value; }));
				builder2.AddAttribute(3, "ValueExpression", (Expression<Func<int>>)(() => model.Value));
				builder2.CloseComponent();
			}));
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);
		cut.Find("input").Change("150"); // above max of 100
		cut.Find("form").Submit();

		// Assert
		Assert.NotNull(cut.Find($"div.{HxInputBase<object>.InvalidCssClass}"));
		Assert.NotEqual("", cut.Find("div.invalid-feedback").TextContent);
	}

	[Fact]
	public void HxInputNumber_NonNullable_EmptyInputShouldRaiseParsingError_Issue892()
	{
		// https://github.com/havit/Havit.Blazor/issues/892 (related for comparison)

		// Arrange
		var myValue = 15;

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputNumber<int>>(0);
			builder.AddAttribute(1, "Value", myValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<int>(this, (value) => { myValue = value; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<int>>)(() => myValue));
			builder.AddAttribute(4, "ParsingErrorMessage", "TestParsingErrorMessage");
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);
		cut.Find("input").Change("");

		// Assert
		Assert.Equal(15, myValue);
		Assert.Equal("", cut.Find("input").GetAttribute("value"));
		Assert.NotNull(cut.Find($"div.{HxInputBase<object>.InvalidCssClass}"));
		Assert.Equal("TestParsingErrorMessage", cut.Find("div.invalid-feedback").TextContent);
	}

	private class RangeModel
	{
		[Range(1, 100)]
		public int Value { get; set; }
	}
}
