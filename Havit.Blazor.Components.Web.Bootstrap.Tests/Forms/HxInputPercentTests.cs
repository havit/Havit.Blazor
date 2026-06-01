using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

public class HxInputPercentTests : BunitTestBase
{
	[Fact]
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
		Assert.Equal(0.5m, currentValue);
	}

	[Fact]
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
		Assert.Equal(0m, currentValue);
		Assert.NotNull(cut.Find($"div.{HxInputBase<object>.InvalidCssClass}"));
		Assert.Equal("TestParsingErrorMessage", cut.Find("div.invalid-feedback").TextContent);
	}

	[Fact]
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
		Assert.Equal("75", cut.Find("input").GetAttribute("value"));
	}
}
