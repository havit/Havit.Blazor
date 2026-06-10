namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxStepperTests : BunitTestBase
{
	[Fact]
	public void HxStepper_Render_RendersOrderedListWithAllItems()
	{
		// Act
		var cut = RenderComponent<HxStepper>(parameters => parameters
			.AddChildContent<HxStepperItem>(item => item
				.Add(i => i.Text, "Create account"))
			.AddChildContent<HxStepperItem>(item => item
				.Add(i => i.Text, "Confirm email"))
			.AddChildContent<HxStepperItem>(item => item
				.Add(i => i.Text, "Finish"))
		);

		// Assert
		var stepper = cut.Find("ol.stepper");
		var items = cut.FindAll("ol.stepper > li.stepper-item");
		Assert.Equal(3, items.Count);
		Assert.Equal("Create account", items[0].TextContent.Trim());
		Assert.Equal("Confirm email", items[1].TextContent.Trim());
		Assert.Equal("Finish", items[2].TextContent.Trim());
	}

	[Fact]
	public void HxStepper_ActiveItem_HasActiveClass()
	{
		// Act
		var cut = RenderComponent<HxStepper>(parameters => parameters
			.AddChildContent<HxStepperItem>(item => item
				.Add(i => i.Active, true)
				.Add(i => i.Text, "Active item"))
			.AddChildContent<HxStepperItem>(item => item
				.Add(i => i.Text, "Inactive item"))
		);

		// Assert
		var items = cut.FindAll(".stepper-item");
		Assert.True(items[0].ClassList.Contains("active"), "First item should have the 'active' class.");
		Assert.False(items[1].ClassList.Contains("active"), "Second item should not have the 'active' class.");
	}

	[Fact]
	public void HxStepper_HorizontalAlways_HasHorizontalClassAndNoWrapper()
	{
		// Act
		var cut = RenderComponent<HxStepper>(parameters => parameters
			.Add(p => p.Horizontal, StepperHorizontal.Always)
			.AddChildContent<HxStepperItem>(item => item
				.Add(i => i.Text, "Step"))
		);

		// Assert
		var stepper = cut.Find("ol");
		Assert.True(stepper.ClassList.Contains("stepper-horizontal"));
		Assert.Empty(cut.FindAll(".contains-inline"));
		Assert.Empty(cut.FindAll(".stepper-overflow"));
	}

	[Theory]
	[InlineData(StepperHorizontal.SmallUp, "sm:stepper-horizontal")]
	[InlineData(StepperHorizontal.MediumUp, "md:stepper-horizontal")]
	[InlineData(StepperHorizontal.LargeUp, "lg:stepper-horizontal")]
	[InlineData(StepperHorizontal.ExtraLargeUp, "xl:stepper-horizontal")]
	[InlineData(StepperHorizontal.XxlUp, "2xl:stepper-horizontal")]
	public void HxStepper_ResponsiveHorizontal_HasResponsiveClassAndContainsInlineWrapper(StepperHorizontal horizontal, string expectedCssClass)
	{
		// Act
		var cut = RenderComponent<HxStepper>(parameters => parameters
			.Add(p => p.Horizontal, horizontal)
			.AddChildContent<HxStepperItem>(item => item
				.Add(i => i.Text, "Step"))
		);

		// Assert
		var wrapper = cut.Find("div.contains-inline");
		var stepper = cut.Find("ol");
		Assert.Equal(wrapper, stepper.ParentElement);
		Assert.True(stepper.ClassList.Contains(expectedCssClass), $"Stepper should have the '{expectedCssClass}' class.");
	}

	[Fact]
	public void HxStepper_Overflow_RendersStepperOverflowWrapper()
	{
		// Act
		var cut = RenderComponent<HxStepper>(parameters => parameters
			.Add(p => p.Horizontal, StepperHorizontal.Always)
			.Add(p => p.Overflow, true)
			.AddChildContent<HxStepperItem>(item => item
				.Add(i => i.Text, "Step"))
		);

		// Assert
		var wrapper = cut.Find("div.stepper-overflow");
		var stepper = cut.Find("ol.stepper");
		Assert.Equal(wrapper, stepper.ParentElement);
	}

	[Fact]
	public void HxStepper_OverflowWithResponsiveHorizontal_DoesNotRenderContainsInlineWrapper()
	{
		// Act
		var cut = RenderComponent<HxStepper>(parameters => parameters
			.Add(p => p.Horizontal, StepperHorizontal.MediumUp)
			.Add(p => p.Overflow, true)
			.AddChildContent<HxStepperItem>(item => item
				.Add(i => i.Text, "Step"))
		);

		// Assert
		Assert.NotNull(cut.Find("div.stepper-overflow"));
		Assert.Empty(cut.FindAll(".contains-inline"));
	}

	[Fact]
	public void HxStepper_Color_RendersThemeClass()
	{
		// Act
		var cut = RenderComponent<HxStepper>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Success)
			.AddChildContent<HxStepperItem>(item => item
				.Add(i => i.Text, "Step"))
		);

		// Assert
		Assert.True(cut.Find("ol.stepper").ClassList.Contains("theme-success"));
	}

	[Fact]
	public void HxStepperItem_Color_RendersThemeClass()
	{
		// Act
		var cut = RenderComponent<HxStepper>(parameters => parameters
			.AddChildContent<HxStepperItem>(item => item
				.Add(i => i.Color, ThemeColor.Danger)
				.Add(i => i.Text, "Step"))
		);

		// Assert
		Assert.True(cut.Find(".stepper-item").ClassList.Contains("theme-danger"));
	}

	[Fact]
	public void HxStepperItem_Href_RendersAnchorItemsAndDivStepper()
	{
		// Act
		var cut = RenderComponent<HxStepper>(parameters => parameters
			.AddChildContent<HxStepperItem>(item => item
				.Add(i => i.Href, "/step-1")
				.Add(i => i.Active, true)
				.Add(i => i.Text, "Create account"))
			.AddChildContent<HxStepperItem>(item => item
				.Add(i => i.Href, "/step-2")
				.Add(i => i.Text, "Confirm email"))
		);

		// Assert
		var stepper = cut.Find("div.stepper");
		Assert.Empty(cut.FindAll("ol"));
		Assert.Empty(cut.FindAll("li"));
		var items = cut.FindAll("a.stepper-item");
		Assert.Equal(2, items.Count);
		Assert.Equal("/step-1", items[0].GetAttribute("href"));
		Assert.True(items[0].ClassList.Contains("active"));
		Assert.Equal("/step-2", items[1].GetAttribute("href"));
	}

	[Fact]
	public void HxStepperItem_MixedAnchorAndPlainItems_RendersPlainItemsAsDivs()
	{
		// Act
		var cut = RenderComponent<HxStepper>(parameters => parameters
			.AddChildContent<HxStepperItem>(item => item
				.Add(i => i.Href, "/step-1")
				.Add(i => i.Text, "Create account"))
			.AddChildContent<HxStepperItem>(item => item
				.Add(i => i.Text, "Confirm email"))
		);

		// Assert
		Assert.Empty(cut.FindAll("ol"));
		Assert.Empty(cut.FindAll("li"));
		Assert.NotNull(cut.Find("div.stepper > a.stepper-item"));
		Assert.NotNull(cut.Find("div.stepper > div.stepper-item"));
	}

	[Fact]
	public void HxStepper_CssClassAndAdditionalAttributes_AreRendered()
	{
		// Act
		var cut = RenderComponent<HxStepper>(parameters => parameters
			.Add(p => p.CssClass, "w-100")
			.AddUnmatched("style", "--bs-stepper-gap: 3rem")
			.AddChildContent<HxStepperItem>(item => item
				.Add(i => i.Text, "Step")
				.Add(i => i.CssClass, "align-items-start"))
		);

		// Assert
		var stepper = cut.Find("ol.stepper");
		Assert.True(stepper.ClassList.Contains("w-100"));
		Assert.Equal("--bs-stepper-gap: 3rem", stepper.GetAttribute("style"));
		Assert.True(cut.Find(".stepper-item").ClassList.Contains("align-items-start"));
	}

	[Fact]
	public void HxStepperItem_TextAndChildContent_RendersBoth()
	{
		// Act
		var cut = RenderComponent<HxStepper>(parameters => parameters
			.AddChildContent<HxStepperItem>(item => item
				.Add(i => i.Text, "Create account")
				.AddChildContent("<span class=\"badge\">Complete</span>"))
		);

		// Assert
		var item = cut.Find(".stepper-item");
		Assert.Contains("Create account", item.TextContent);
		Assert.NotNull(cut.Find(".stepper-item > span.badge"));
	}
}
