namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public partial class HxButtonTests : BunitTestBase
{
	[Fact]
	public void HxButton_TooltipSettings_Trigger_ShouldBeConfigurableViaParameter()
	{
		// Arrange & Act
		var cut = RenderComponent<HxButton>(parameters => parameters
			.Add(p => p.Tooltip, "Test Tooltip")
			.Add(p => p.TooltipSettings, new TooltipSettings()
			{
				Trigger = TooltipTrigger.Click
			})
		);

		// Assert
		var tooltipElement = cut.Find("span[data-bs-toggle='tooltip']");
		Assert.NotNull(tooltipElement);
		var trigger = tooltipElement.GetAttribute("data-bs-trigger");
		Assert.Equal("click", trigger);
	}

	[Fact]
	public void HxButton_TooltipSettings_Trigger_ShouldBeConfigurableViaSettings()
	{
		// Arrange & Act
		var cut = RenderComponent<HxButton>(parameters => parameters
			.Add(p => p.Tooltip, "Test Tooltip")
			.Add(p => p.Settings, new ButtonSettings()
			{
				TooltipSettings = new TooltipSettings()
				{
					Trigger = TooltipTrigger.Hover | TooltipTrigger.Click
				}
			})
		);

		// Assert
		var tooltipElement = cut.Find("span[data-bs-toggle='tooltip']");
		Assert.NotNull(tooltipElement);
		var trigger = tooltipElement.GetAttribute("data-bs-trigger");
		Assert.Equal("click hover", trigger); // order does not matter
	}

	[Fact]
	public void HxButton_TooltipSettings_DefaultTrigger_ShouldNotBeSet()
	{
		// Arrange & Act
		var cut = RenderComponent<HxButton>(parameters => parameters
			.Add(p => p.Tooltip, "Test Tooltip")
		);

		// Assert
		var tooltipElement = cut.Find("span[data-bs-toggle='tooltip']");
		Assert.NotNull(tooltipElement);
		var trigger = tooltipElement.GetAttribute("data-bs-trigger");
		// The default trigger should not be set as an attribute, Bootstrap uses its own default.
		Assert.Null(trigger);
	}

	[Fact]
	public void HxButton_TooltipSettings_Placement_ShouldBeConfigurableViaParameter()
	{
		// Arrange & Act
		var cut = RenderComponent<HxButton>(parameters => parameters
			.Add(p => p.Tooltip, "Test Tooltip")
			.Add(p => p.TooltipSettings, new TooltipSettings()
			{
				Placement = TooltipPlacement.Bottom
			})
		);

		// Assert
		var tooltipElement = cut.Find("span[data-bs-toggle='tooltip']");
		Assert.NotNull(tooltipElement);
		var placement = tooltipElement.GetAttribute("data-bs-placement");
		Assert.Equal("bottom", placement);
	}

	[Fact]
	public void HxButton_TooltipSettings_Placement_ShouldBeConfigurableViaSettings()
	{
		// Arrange & Act
		var cut = RenderComponent<HxButton>(parameters => parameters
			.Add(p => p.Tooltip, "Test Tooltip")
			.Add(p => p.Settings, new ButtonSettings()
			{
				TooltipSettings = new TooltipSettings()
				{
					Placement = TooltipPlacement.Left
				}
			})
		);

		// Assert
		var tooltipElement = cut.Find("span[data-bs-toggle='tooltip']");
		Assert.NotNull(tooltipElement);
		var placement = tooltipElement.GetAttribute("data-bs-placement");
		Assert.Equal("left", placement);
	}

	[Fact]
	public void HxButton_TooltipSettings_DefaultPlacement_ShouldNotBeSet()
	{
		// Arrange & Act
		var cut = RenderComponent<HxButton>(parameters => parameters
			.Add(p => p.Tooltip, "Test Tooltip")
		);

		// Assert
		var tooltipElement = cut.Find("span[data-bs-toggle='tooltip']");
		Assert.NotNull(tooltipElement);
		var placement = tooltipElement.GetAttribute("data-bs-placement");
		// The default placement should not be set as an attribute, Bootstrap uses its own default.
		Assert.Null(placement);
	}
}
