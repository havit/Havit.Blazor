using Bunit;
namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

public class HxInputTextAdornTests : BunitTestBase
{
	[Fact]
	public void HxInputText_NoAdorn_RendersPlainFormControl()
	{
		// Act
		var cut = Render<HxInputText>(parameters => parameters
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
		var cut = Render<HxInputText>(parameters => parameters
			.Bind(p => p.Value, "initial", _ => { })
			.Add(p => p.AdornStartText, "€"));

		// Assert — Bootstrap 6: a wrapping label focuses the input on a click on the adornment/padding,
		// the decorative adornment must not join the accessible name of the input
		var wrapper = cut.Find("label.form-adorn");
		Assert.Contains("form-control", wrapper.GetAttribute("class"));

		var adornText = wrapper.QuerySelector(".form-adorn-text");
		Assert.NotNull(adornText);
		Assert.Equal("€", adornText.TextContent);
		Assert.Equal("true", adornText.GetAttribute("aria-hidden"));

		// ...but it can be meaningful (currency, unit), so the input references it as its description
		Assert.False(String.IsNullOrEmpty(adornText.Id));
		Assert.Contains(adornText.Id, cut.Find("input").GetAttribute("aria-describedby").Split(' '));

		var input = cut.Find("input");
		Assert.Contains("form-ghost", input.GetAttribute("class"));
		Assert.DoesNotContain("form-control", input.GetAttribute("class"));
	}

	[Fact]
	public void HxInputText_AdornEndTemplate_RendersFormAdornIconAfterInput()
	{
		// Act
		var cut = Render<HxInputText>(parameters => parameters
			.Bind(p => p.Value, "initial", _ => { })
			.Add(p => p.AdornEndTemplate, (builder) => builder.AddMarkupContent(0, "<span class=\"my-icon\"></span>")));

		// Assert — a template can contain interactive content (not allowed in a label),
		// the wrapper is a div focusing the input when clicked directly
		var wrapper = cut.Find("div.form-adorn");
		var icon = wrapper.QuerySelector(".form-adorn-icon .my-icon");
		Assert.NotNull(icon);
		Assert.Contains("focus()", wrapper.GetAttribute("onclick"));
	}

	[Fact]
	public void HxInputText_AdornWithFloatingLabel_Throws()
	{
		// Act & Assert
		Assert.Throws<InvalidOperationException>(() => Render<HxInputText>(parameters => parameters
			.Bind(p => p.Value, "initial", _ => { })
			.Add(p => p.LabelType, LabelType.Floating)
			.Add(p => p.Label, "Name")
			.Add(p => p.AdornStartText, "€")));
	}

	[Fact]
	public void HxInputText_AdornTextsAndHint_AllReferencedInAriaDescribedBy()
	{
		// Act
		var cut = Render<HxInputText>(parameters => parameters
			.Bind(p => p.Value, "initial", _ => { })
			.Add(p => p.AdornStartText, "https://")
			.Add(p => p.AdornEndText, ".com")
			.Add(p => p.Hint, "Hint"));

		// Assert
		var describedBy = cut.Find("input").GetAttribute("aria-describedby").Split(' ');
		var adornTexts = cut.FindAll(".form-adorn-text");
		Assert.Equal(2, adornTexts.Count);
		Assert.Contains(adornTexts[0].Id, describedBy);
		Assert.Contains(adornTexts[1].Id, describedBy);
		Assert.Contains(cut.Find(".form-text").Id, describedBy);
		Assert.DoesNotContain("", describedBy);
	}
}
