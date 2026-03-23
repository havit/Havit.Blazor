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
		// Arrange
		bool value = false;

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxSwitch>(0);
			builder.AddAttribute(1, "Value", value);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<bool>(this, (v) => { value = v; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<bool>>)(() => value));
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert
		var input = cut.Find("input");
		Assert.IsFalse(input.HasAttribute("checked"));
	}

	[TestMethod]
	public async Task HxSwitch_Click_TogglesOnAndUpdatesValue()
	{
		// Arrange
		bool value = false;

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxSwitch>(0);
			builder.AddAttribute(1, "Value", value);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<bool>(this, (v) => { value = v; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<bool>>)(() => value));
			builder.CloseComponent();
		};

		var cut = Render(componentRenderer);

		// Act
		await cut.InvokeAsync(() => cut.Find("input").Change(true));

		// Assert
		Assert.IsTrue(value);
	}

	[TestMethod]
	public async Task HxSwitch_ClickAgain_TogglesOffAndUpdatesValue()
	{
		// Arrange
		bool value = true;

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxSwitch>(0);
			builder.AddAttribute(1, "Value", value);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<bool>(this, (v) => { value = v; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<bool>>)(() => value));
			builder.CloseComponent();
		};

		var cut = Render(componentRenderer);

		// Act
		await cut.InvokeAsync(() => cut.Find("input").Change(false));

		// Assert
		Assert.IsFalse(value);
	}
}
