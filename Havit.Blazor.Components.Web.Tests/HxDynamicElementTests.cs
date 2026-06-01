using Bunit;
using Havit.Blazor.Components.Web.Bootstrap.Tests;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Havit.Blazor.Components.Web.Tests;

public class HxDynamicElementTests : BunitTestBase
{
	[Fact]
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
		Assert.Equal(1, cut.RenderCount);

		// Act + Assert
		var ex = Assert.Throws<MissingEventHandlerException>(() => cut.Find("span").Click());
		Assert.Contains("The element does not have an event handler for the event 'onclick'", ex.Message);
	}

	[Fact]
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
		Assert.Equal(1, cut.RenderCount);

		// Act
		cut.Find("span").Click();

		// Assert			
		Assert.Equal(1, onClickCallbackCallCounter);
		Assert.Equal(2, cut.RenderCount);
	}
}
