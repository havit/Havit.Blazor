using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

public class HxSwitchTests : BunitTestBase
{
	[Fact]
	public void HxSwitch_Render_InitiallyOff()
	{
		// Act
		var (cut, _) = RenderSwitch(initialValue: false);

		// Assert
		Assert.False(cut.Find("input").HasAttribute("checked"));
	}

	[Fact]
	public void HxSwitch_Change_TogglesOnAndUpdatesValue()
	{
		// Arrange
		var (cut, valueHolder) = RenderSwitch(initialValue: false);

		// Act
		cut.Find("input").Change(true);

		// Assert
		Assert.True(valueHolder[0]);
		Assert.True(cut.Find("input").HasAttribute("checked"));
	}

	[Fact]
	public void HxSwitch_Change_TogglesOffAndUpdatesValue()
	{
		// Arrange
		var (cut, valueHolder) = RenderSwitch(initialValue: true);

		// Act
		cut.Find("input").Change(false);

		// Assert
		Assert.False(valueHolder[0]);
		Assert.False(cut.Find("input").HasAttribute("checked"));
	}

	[Fact]
	public void HxSwitch_Render_HasSwitchAttribute()
	{
		// Act — Bootstrap 6 renders the switch attribute on every switch (native haptic feedback in mobile Safari)
		var (cut, _) = RenderSwitch();

		// Assert
		var input = cut.Find(".switch > input");
		Assert.True(input.HasAttribute("switch"));
		Assert.Equal("switch", input.GetAttribute("role"));
	}

	[Theory]
	[InlineData(InputSize.Small, "switch-sm")]
	[InlineData(InputSize.Large, "switch-lg")]
	public void HxSwitch_InputSize_RendersSwitchSizeClassOnWrapper(InputSize inputSize, string expectedCssClass)
	{
		// Arrange
		bool value = false;

		// Act
		var cut = Render<HxSwitch>(parameters => parameters
			.Bind(p => p.Value, value, v => value = v)
			.Add(p => p.Text, "Text")
			.Add(p => p.InputSize, inputSize));

		// Assert — Bootstrap 6 sizes the .switch wrapper (the input inside is unstyled)
		Assert.True(cut.Find("div.switch").ClassList.Contains(expectedCssClass));
		Assert.False(cut.Find("div.switch > input").ClassList.Contains(expectedCssClass));
	}

	private (IRenderedComponent<HxSwitch> cut, bool[] valueHolder) RenderSwitch(bool initialValue = false)
	{
		var valueHolder = new bool[] { initialValue };

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxSwitch>(0);
			builder.AddAttribute(1, "Value", valueHolder[0]);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<bool>(this, v => valueHolder[0] = v));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<bool>>)(() => valueHolder[0]));
			builder.CloseComponent();
		};

		return (Render<HxSwitch>(componentRenderer), valueHolder);
	}
}
