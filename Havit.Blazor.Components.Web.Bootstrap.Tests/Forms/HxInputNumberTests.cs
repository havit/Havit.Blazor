using System.Globalization;
using Bunit;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

[TestClass]
public class HxInputNumberTests : BunitTestBase
{
	[TestMethod]
	[DataRow("en-US", "123", 123.0, "123.00")]
	[DataRow("en-US", "123,123", 123123.0, "123123.00")]
	[DataRow("cs-CZ", "123,123", 123.12, "123,12")] // Decimals = 2 (rounding)
	[DataRow("en-US", "123.123", 123.12, "123.12")]
	[DataRow("cs-CZ", "123.123", 123.12, "123,12")] // replace . with , (if possible)
	[DataRow("en-US", "15,81.549", 1581.55, "1581.55")]
	[DataRow("cs-CZ", "15 81,549", 1581.55, "1581,55")]
	[DataRow("cs-CZ", "1.234", 1.23, "1,23")] // Replace . with , (if possible)
	[DataRow("en-US", "1.237", 1.24, "1.24")] // rounding
	[DataRow("en-US", "abc", null, "abc")] // invalid input
	[DataRow("en-US", "", null, null)] // empty input
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
		Assert.AreEqual((decimal?)expectedValue, cut.Instance.Value);
		Assert.AreEqual(expectedInputText, cut.Find("input").GetAttribute("value"));
	}

	[TestMethod]
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
		Assert.AreEqual(15, myValue, "Model value should remain unchanged.");
		Assert.AreEqual("", cut.Find("input").GetAttribute("value"), "Input value should be empty.");
		Assert.IsNotNull(cut.Find($"div.{HxInputBase<object>.InvalidCssClass}"));
		Assert.AreEqual("TestParsingErrorMessage", cut.Find("div.invalid-feedback").TextContent, "ParsingValidationError should be displayed.");
	}

}
