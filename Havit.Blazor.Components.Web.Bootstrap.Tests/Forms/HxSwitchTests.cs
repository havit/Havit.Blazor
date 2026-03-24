using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

[TestClass]
public class HxSwitchTests : BunitTestBase
{
	[TestMethod]
	public void HxSwitch_Render_InitiallyOff()
	{
		// Act
		var (cut, _) = RenderSwitch(initialValue: false);

		// Assert
		Assert.IsFalse(cut.Find("input").HasAttribute("checked"));
	}

	[TestMethod]
	public void HxSwitch_Change_TogglesOnAndUpdatesValue()
	{
		// Arrange
		var (cut, valueHolder) = RenderSwitch(initialValue: false);

		// Act
		cut.Find("input").Change(true);

		// Assert
		Assert.IsTrue(valueHolder[0]);
		Assert.IsTrue(cut.Find("input").HasAttribute("checked"));
	}

	[TestMethod]
	public void HxSwitch_Change_TogglesOffAndUpdatesValue()
	{
		// Arrange
		var (cut, valueHolder) = RenderSwitch(initialValue: true);

		// Act
		cut.Find("input").Change(false);

		// Assert
		Assert.IsFalse(valueHolder[0]);
		Assert.IsFalse(cut.Find("input").HasAttribute("checked"));
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
