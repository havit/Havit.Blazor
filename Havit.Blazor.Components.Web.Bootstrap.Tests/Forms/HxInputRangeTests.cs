using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

[TestClass]
public class HxInputRangeTests : BunitTestBase
{
	[TestMethod]
	public void HxInputRange_Render_HasCorrectMinMaxAttributes()
	{
		// Arrange
		int currentValue = 50;

		// Act
		var cut = RenderComponent<HxInputRange<int>>(parameters => parameters
			.Add(p => p.Min, 0)
			.Add(p => p.Max, 100)
			.Bind(p => p.Value, currentValue, newValue => currentValue = newValue));

		// Assert
		var input = cut.Find("input[type=range]");
		Assert.AreEqual("0", input.GetAttribute("min"));
		Assert.AreEqual("100", input.GetAttribute("max"));
	}

	[TestMethod]
	public void HxInputRange_ChangeValue_UpdatesBoundValue()
	{
		// Arrange
		int currentValue = 25;

		var cut = RenderComponent<HxInputRange<int>>(parameters => parameters
			.Add(p => p.Min, 0)
			.Add(p => p.Max, 100)
			.Bind(p => p.Value, currentValue, newValue => currentValue = newValue));

		// Act
		cut.Find("input[type=range]").Change("75");

		// Assert
		Assert.AreEqual(75, currentValue);
	}

	[TestMethod]
	public void HxInputRange_WithLabel_RendersLabelAndCorrectValueAttribute()
	{
		// Arrange
		int currentValue = 42;

		// Act
		var cut = RenderComponent<HxInputRange<int>>(parameters => parameters
			.Add(p => p.Min, 0)
			.Add(p => p.Max, 100)
			.Add(p => p.Label, "Volume")
			.Bind(p => p.Value, currentValue, newValue => currentValue = newValue));

		// Assert
		var label = cut.Find("label");
		Assert.AreEqual("Volume", label.TextContent);

		var input = cut.Find("input[type=range]");
		Assert.AreEqual("42", input.GetAttribute("value"));
	}
}
