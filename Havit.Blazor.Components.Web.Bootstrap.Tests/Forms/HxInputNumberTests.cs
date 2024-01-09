using System.Globalization;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

[TestClass]
public class HxInputNumberTests
{
	[DataTestMethod]
	[DataRow("en-US", "123", 123.0, "123.00")]
	[DataRow("en-US", "123,123", 123123.0, "123123.00")]
	[DataRow("cs-CZ", "123,123", 123.12, "123,12")] // Decimals = 2 (rounding)
	[DataRow("en-US", "123.123", 123.12, "123.12")]
	[DataRow("cs-CZ", "123.123", 123.12, "123,12")] // replace . with , (if possible)
	[DataRow("en-US", "15,81.549", 1581.55, "1581.55")]
	[DataRow("cs-CZ", "15 81,549", 1581.55, "1581,55")]
	[DataRow("cs-CZ", "1.234", 1.23, "1,23")] // Replace . with , (if possible)
	[DataRow("en-US", "1.237", 1.24, "1.24")] // rounding
#if NET8_0_OR_GREATER
	[DataRow("en-US", "abc", null, "abc")] // invalid input
#else
	// Blazor bug - missing SetUpdatesAttributeName
	// https://github.com/havit/Havit.Blazor/issues/468
	// https://github.com/dotnet/aspnetcore/pull/46434
	[DataRow("en-US", "abc", null, null)] // invalid input
#endif
	[DataRow("en-US", "", null, null)] // empty input
	public void HxInputNumber_NullableDecimal_ValueConversions(string culture, string input, double? expectedValue, string expectedInputText)
	{
		// Arrange
		CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(culture);
		CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(culture);

		var ctx = new Bunit.TestContext();
		ctx.JSInterop.Mode = JSRuntimeMode.Loose;
		ctx.Services.AddLogging();
		ctx.Services.AddHxServices();
		ctx.Services.AddHxMessenger();
		ctx.Services.AddHxMessageBoxHost();

		decimal? currentValue = null;
		var cut = ctx.RenderComponent<HxInputNumber<decimal?>>(parameters =>
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
}
