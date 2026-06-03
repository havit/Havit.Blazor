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

	private (IRenderedFragment cut, bool[] valueHolder) RenderSwitch(bool initialValue = false)
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

		return (Render(componentRenderer), valueHolder);
	}
}
