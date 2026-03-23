using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

[TestClass]
public class HxInputPercentTests : BunitTestBase
{
	[TestMethod]
	public void HxInputPercent_TypePercentage_StoresDecimalValue()
	{
		// Arrange
		decimal currentValue = 0m;
		var cut = RenderComponent<HxInputPercent<decimal>>(parameters =>
			parameters.Bind(p =>
				p.Value,
				currentValue,
				newValue => currentValue = newValue));

		// Act
		cut.Find("input").Change("50");

		// Assert
		Assert.AreEqual(0.5m, currentValue, "Typing 50 should store 0.5 as decimal value.");
	}

	[TestMethod]
	public void HxInputPercent_TypeInvalid_ShowsValidationError()
	{
		// Arrange
		decimal currentValue = 0m;

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputPercent<decimal>>(0);
			builder.AddAttribute(1, "Value", currentValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<decimal>(this, (value) => { currentValue = value; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<decimal>>)(() => currentValue));
			builder.AddAttribute(4, "ParsingErrorMessage", "TestParsingErrorMessage");
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);
		cut.Find("input").Change("abc");

		// Assert
		Assert.AreEqual(0m, currentValue, "Model value should remain unchanged after invalid input.");
		Assert.IsNotNull(cut.Find($"div.{HxInputBase<object>.InvalidCssClass}"), "Invalid CSS class should be applied.");
		Assert.AreEqual("TestParsingErrorMessage", cut.Find("div.invalid-feedback").TextContent, "Parsing validation error should be displayed.");
	}

	[TestMethod]
	public void HxInputPercent_Display_ShowsPercentageCorrectly()
	{
		// Arrange
		decimal currentValue = 0.75m;
		var cut = RenderComponent<HxInputPercent<decimal>>(parameters =>
		{
			parameters.Bind(p =>
				p.Value,
				currentValue,
				newValue => currentValue = newValue);
			parameters.Add(p => p.Decimals, 0);
		});

		// Assert
		Assert.AreEqual("75", cut.Find("input").GetAttribute("value"), "Value 0.75 should be displayed as 75.");
	}
}
