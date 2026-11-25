using System.Globalization;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

public class HxInputRangeTests : BunitTestBase
{
	[Fact]
	public void HxInputRange_Render_HasCorrectMinMaxAttributes()
	{
		// Arrange
		int currentValue = 50;

		// Act
		var cut = Render<HxInputRange<int>>(parameters => parameters
			.Add(p => p.Min, 0)
			.Add(p => p.Max, 100)
			.Bind(p => p.Value, currentValue, newValue => currentValue = newValue));

		// Assert
		var input = cut.Find("input[type=range]");
		Assert.Equal("0", input.GetAttribute("min"));
		Assert.Equal("100", input.GetAttribute("max"));
	}

	[Fact]
	public void HxInputRange_ChangeValue_UpdatesBoundValue()
	{
		// Arrange
		int currentValue = 25;

		var cut = Render<HxInputRange<int>>(parameters => parameters
			.Add(p => p.Min, 0)
			.Add(p => p.Max, 100)
			.Bind(p => p.Value, currentValue, newValue => currentValue = newValue));

		// Act
		cut.Find("input[type=range]").Change("75");

		// Assert
		Assert.Equal(75, currentValue);
	}

	[Fact]
	public void HxInputRange_WithLabel_RendersLabelAndCorrectValueAttribute()
	{
		// Arrange
		int currentValue = 42;

		// Act
		var cut = Render<HxInputRange<int>>(parameters => parameters
			.Add(p => p.Min, 0)
			.Add(p => p.Max, 100)
			.Add(p => p.Label, "Volume")
			.Bind(p => p.Value, currentValue, newValue => currentValue = newValue));

		// Assert
		var label = cut.Find("label");
		Assert.Equal("Volume", label.TextContent);

		var input = cut.Find("input[type=range]");
		Assert.Equal("42", input.GetAttribute("value"));
	}

	[Theory]
	[InlineData("cs-CZ")]
	[InlineData("tr-TR")]
	public void HxInputRange_CommaDecimalCulture_RendersInvariantMinMaxStep(string cultureName)
	{
		// Arrange
		using var culture = CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo(cultureName));
		double currentValue = 1.2;

		// Act
		var cut = Render<HxInputRange<double>>(parameters => parameters
			.Add(p => p.Min, 0.5)
			.Add(p => p.Max, 1.5)
			.Add(p => p.Step, 0.1)
			.Bind(p => p.Value, currentValue, newValue => currentValue = newValue));

		// Assert
		var input = cut.Find("input[type=range]");
		Assert.Equal("0.5", input.GetAttribute("min"));
		Assert.Equal("1.5", input.GetAttribute("max"));
		Assert.Equal("0.1", input.GetAttribute("step"));
		Assert.Equal("1.2", input.GetAttribute("value"));
	}

	[Theory]
	[InlineData("cs-CZ")]
	[InlineData("tr-TR")]
	public void HxInputRange_CommaDecimalCulture_ParsesInvariantValueFromBrowser(string cultureName)
	{
		// Arrange
		using var culture = CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo(cultureName));
		double currentValue = 1.2;

		var cut = Render<HxInputRange<double>>(parameters => parameters
			.Add(p => p.Min, 0.5)
			.Add(p => p.Max, 1.5)
			.Add(p => p.Step, 0.1)
			.Bind(p => p.Value, currentValue, newValue => currentValue = newValue));

		// Act
		cut.Find("input[type=range]").Change("0.7");

		// Assert
		Assert.Equal(0.7, currentValue);
	}
}
