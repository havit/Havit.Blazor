using Bunit;
using Havit.Blazor.Components.Web.Bootstrap.Tests;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Havit.Blazor.Components.Web.Tests;

[TestClass]
public class HxDynamicElementTests : BunitTestBase
{
	[TestMethod]
	public void HxDynamicElement_OnClickNotSet_ShouldNotSubscribeToClickEvent()
	{
		// Arrange
		var ctx = new Bunit.TestContext();

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxDynamicElement>(0);
			builder.CloseComponent();
		};

		var cut = Render(componentRenderer);
		Assert.AreEqual(1, cut.RenderCount);

		// Act + Assert
		var ex = Assert.ThrowsExactly<MissingEventHandlerException>(() => cut.Find("span").Click());
		Assert.IsTrue(ex.Message.Contains("The element does not have an event handler for the event 'onclick'"));
	}

	[TestMethod]
	public void HxDynamicElement_OnClickIsSet_ShouldRaiseCallbackAndRerender()
	{
		// Arrange
		var ctx = new Bunit.TestContext();
		var onClickCallbackCallCounter = 0;

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxDynamicElement>(0);
			builder.AddAttribute(1, nameof(HxDynamicElement.OnClick), EventCallback.Factory.Create<MouseEventArgs>(this, () => { onClickCallbackCallCounter++; }));
			builder.CloseComponent();
		};

		var cut = Render(componentRenderer);
		Assert.AreEqual(1, cut.RenderCount);

		// Act
		cut.Find("span").Click();

		// Assert			
		Assert.AreEqual(1, onClickCallbackCallCounter);
		Assert.AreEqual(2, cut.RenderCount);
	}
}
