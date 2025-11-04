namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

[TestClass]
public class HxButtonTests : BunitTestBase
{
	[TestMethod]
	public void HxButton_TooltipSettings_Trigger_ShouldBeConfigurableViaDefaults()
	{
		// Arrange
		var originalDefaults = HxButton.Defaults;
		try
		{
			HxButton.Defaults = new ButtonSettings()
			{
				Size = ButtonSize.Regular,
				IconPlacement = ButtonIconPlacement.Start,
				Color = ThemeColor.None,
				Outline = false,
				TooltipSettings = new TooltipSettings()
				{
					Trigger = TooltipTrigger.Hover
				}
			};

			// Act
			var cut = RenderComponent<HxButton>(parameters => parameters
				.Add(p => p.Tooltip, "Test Tooltip")
			);

			// Assert
			var tooltipElement = cut.Find("span[data-bs-toggle='tooltip']");
			Assert.IsNotNull(tooltipElement);
			var trigger = tooltipElement.GetAttribute("data-bs-trigger");
			Assert.AreEqual("hover", trigger);
		}
		finally
		{
			HxButton.Defaults = originalDefaults;
		}
	}

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
		Assert.IsTrue(trigger.Contains("hover") && trigger.Contains("click"));
	}

	[TestMethod]
	public void HxButton_TooltipSettings_DefaultTrigger_ShouldBeHoverAndFocus()
	{
		// Arrange & Act
		var cut = RenderComponent<HxButton>(parameters => parameters
			.Add(p => p.Tooltip, "Test Tooltip")
		);

		// Assert
		var tooltipElement = cut.Find("span[data-bs-toggle='tooltip']");
		Assert.IsNotNull(tooltipElement);
		var trigger = tooltipElement.GetAttribute("data-bs-trigger");
		// The default trigger should be "hover focus" (note the space separation)
		Assert.AreEqual("hover focus", trigger);
	}
}
