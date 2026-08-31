namespace Havit.Blazor.Components.Web.Bootstrap.Smart.Tests;

public class HxSmartPasteButtonTests : BunitTestBase
{
	[Theory]
	[InlineData(ButtonIconPlacement.Start)]
	[InlineData(ButtonIconPlacement.End)]
	public void HxSmartPasteButton_IconOnly_DoesNotRenderSpacer(ButtonIconPlacement placement)
	{
		var cut = Render<HxSmartPasteButton>(parameters => parameters
			.Add(p => p.Icon, BootstrapIcon.Alarm)
			.Add(p => p.IconPlacement, placement));

		Assert.DoesNotContain('\u00A0', cut.Find("button").TextContent);
	}

	[Theory]
	[InlineData(ButtonIconPlacement.Start)]
	[InlineData(ButtonIconPlacement.End)]
	public void HxSmartPasteButton_Text_RendersSpacer(ButtonIconPlacement placement)
	{
		var cut = Render<HxSmartPasteButton>(parameters => parameters
			.Add(p => p.Icon, BootstrapIcon.Alarm)
			.Add(p => p.IconPlacement, placement)
			.Add(p => p.Text, "Paste"));

		Assert.Contains('\u00A0', cut.Find("button").TextContent);
	}

	[Theory]
	[InlineData(ButtonIconPlacement.Start)]
	[InlineData(ButtonIconPlacement.End)]
	public void HxSmartPasteButton_ChildContent_RendersSingleSpacer(
		ButtonIconPlacement placement)
	{
		var parameters = new Action<ComponentParameterCollectionBuilder<HxSmartPasteButton>>(builder => builder
			.Add(p => p.Icon, BootstrapIcon.Alarm)
			.Add(p => p.IconPlacement, placement)
			.AddChildContent("Paste"));
		var cut = Render(parameters);

		Assert.Equal(1, cut.Find("button").TextContent.Count(character => character == '\u00A0'));

		cut.Render(parameters);
		Assert.Equal(1, cut.Find("button").TextContent.Count(character => character == '\u00A0'));
	}
}
