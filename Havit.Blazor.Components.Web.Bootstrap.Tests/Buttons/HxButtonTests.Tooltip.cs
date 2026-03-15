namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

[TestClass]
public partial class HxButtonTests : BunitTestBase
{
	[TestMethod]
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
		Assert.IsNotNull(tooltipElement);
		var trigger = tooltipElement.GetAttribute("data-bs-trigger");
		Assert.AreEqual("click", trigger);
	}

	[TestMethod]
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
		Assert.IsNotNull(tooltipElement);
		var trigger = tooltipElement.GetAttribute("data-bs-trigger");
		Assert.AreEqual("click hover", trigger); // order does not matter
	}

	[TestMethod]
	public void HxButton_TooltipSettings_DefaultTrigger_ShouldNotBeSet()
	{
		// Arrange & Act
		var cut = RenderComponent<HxButton>(parameters => parameters
			.Add(p => p.Tooltip, "Test Tooltip")
		);

		// Assert
		var tooltipElement = cut.Find("span[data-bs-toggle='tooltip']");
		Assert.IsNotNull(tooltipElement);
		var trigger = tooltipElement.GetAttribute("data-bs-trigger");
		// The default trigger should not be set as an attribute, Bootstrap uses its own default.
		Assert.IsNull(trigger);
	}

	[TestMethod]
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
		Assert.IsNotNull(tooltipElement);
		var placement = tooltipElement.GetAttribute("data-bs-placement");
		Assert.AreEqual("bottom", placement);
	}

	[TestMethod]
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
		Assert.IsNotNull(tooltipElement);
		var placement = tooltipElement.GetAttribute("data-bs-placement");
		Assert.AreEqual("left", placement);
	}

	[TestMethod]
	public void HxButton_TooltipSettings_DefaultPlacement_ShouldNotBeSet()
	{
		// Arrange & Act
		var cut = RenderComponent<HxButton>(parameters => parameters
			.Add(p => p.Tooltip, "Test Tooltip")
		);

		// Assert
		var tooltipElement = cut.Find("span[data-bs-toggle='tooltip']");
		Assert.IsNotNull(tooltipElement);
		var placement = tooltipElement.GetAttribute("data-bs-placement");
		// The default placement should not be set as an attribute, Bootstrap uses its own default.
		Assert.IsNull(placement);
	}
}
