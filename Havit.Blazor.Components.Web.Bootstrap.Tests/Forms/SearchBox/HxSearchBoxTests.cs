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

	[Fact]
	public void HxSearchBox_LabelTypeFloating_ThrowsInvalidOperationException()
	{
		// LabelType.Floating is not supported in Bootstrap 6 — the form-adorn wrapper owns the visual chrome and cannot host a floating label.

		// Arrange
		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxSearchBox<string>>(0);
			builder.AddAttribute(1, nameof(HxSearchBox<string>.Label), "Search");
			builder.AddAttribute(2, nameof(HxSearchBox<string>.LabelType), (LabelType?)LabelType.Floating);
			builder.CloseComponent();
		};

		// Act + Assert
		Assert.Throws<InvalidOperationException>(() => Render(componentRenderer));
	}
}
