using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components;
namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms.SearchBox;

public class HxSearchBoxTests : BunitTestBase
{
	[Fact]
	public void HxSearchBox_EnabledFalse_ShouldRenderDisabledAttribute_Issue941()
	{
		// https://github.com/havit/Havit.Blazor/issues/941

		// Arrange
		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxSearchBox<string>>(0);
			builder.AddAttribute(1, "Enabled", false);
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert
		Assert.True(cut.Find("input").HasAttribute("disabled"));
	}
}
