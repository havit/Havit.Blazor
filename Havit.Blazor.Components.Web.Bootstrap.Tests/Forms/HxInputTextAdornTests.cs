using Bunit;
namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

public class HxInputTextAdornTests : BunitTestBase
{
	[Fact]
	public void HxInputText_NoAdorn_RendersPlainFormControl()
	{
		// Act
		var cut = RenderComponent<HxInputText>(parameters => parameters
			.Bind(p => p.Value, "initial", _ => { }));

		// Assert
		Assert.DoesNotContain("form-adorn", cut.Markup);
		var input = cut.Find("input");
		Assert.Contains("form-control", input.GetAttribute("class"));
		Assert.DoesNotContain("form-ghost", input.GetAttribute("class"));
	}

	[Fact]
	public void HxInputText_AdornStartText_RendersFormAdornWrapperWithGhostInput()
	{
		// Act
		var cut = RenderComponent<HxInputText>(parameters => parameters
			.Bind(p => p.Value, "initial", _ => { })
			.Add(p => p.AdornStartText, "€"));

		// Assert
		var wrapper = cut.Find("div.form-adorn");
		Assert.Contains("form-control", wrapper.GetAttribute("class"));

		var adornText = wrapper.QuerySelector(".form-adorn-text");
		Assert.NotNull(adornText);
		Assert.Equal("€", adornText.TextContent);

		var input = cut.Find("input");
		Assert.Contains("form-ghost", input.GetAttribute("class"));
		Assert.DoesNotContain("form-control", input.GetAttribute("class"));
	}

	[Fact]
	public void HxInputText_AdornEndTemplate_RendersFormAdornIconAfterInput()
	{
		// Act
		var cut = RenderComponent<HxInputText>(parameters => parameters
			.Bind(p => p.Value, "initial", _ => { })
			.Add(p => p.AdornEndTemplate, (builder) => builder.AddMarkupContent(0, "<span class=\"my-icon\"></span>")));

		// Assert
		var wrapper = cut.Find("div.form-adorn");
		var icon = wrapper.QuerySelector(".form-adorn-icon .my-icon");
		Assert.NotNull(icon);
	}

	[Fact]
	public void HxInputText_AdornWithFloatingLabel_Throws()
	{
		// Act & Assert
		Assert.Throws<InvalidOperationException>(() => RenderComponent<HxInputText>(parameters => parameters
			.Bind(p => p.Value, "initial", _ => { })
			.Add(p => p.LabelType, LabelType.Floating)
			.Add(p => p.Label, "Name")
			.Add(p => p.AdornStartText, "€")));
	}
}
