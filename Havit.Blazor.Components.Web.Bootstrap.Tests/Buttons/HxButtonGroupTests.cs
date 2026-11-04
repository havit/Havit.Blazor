namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxButtonGroupTests : BunitTestBase
{
	[Fact]
	public void HxButtonGroup_Render_GroupsButtonsHorizontally()
	{
		// Arrange & Act
		var cut = Render<HxButtonGroup>(parameters => parameters
			.Add(p => p.AriaLabel, "Test group")
			.AddChildContent<HxButton>(buttonParams => buttonParams.Add(p => p.Text, "First"))
			.AddChildContent<HxButton>(buttonParams => buttonParams.Add(p => p.Text, "Second"))
		);

		// Assert - the group should have the correct CSS class and role
		var group = cut.Find("div");
		Assert.True(group.ClassList.Contains("btn-group"), "Button group should have 'btn-group' CSS class");
		Assert.Equal("group", group.GetAttribute("role"));

		// Assert - both buttons should be rendered inside the group
		var buttons = cut.FindAll("button");
		Assert.Equal(2, buttons.Count());
	}

	[Theory]
	[InlineData(ButtonGroupSize.Regular, null)]
	[InlineData(ButtonGroupSize.ExtraSmall, "btn-group-xs")]
	[InlineData(ButtonGroupSize.Small, "btn-group-sm")]
	[InlineData(ButtonGroupSize.Large, "btn-group-lg")]
	public void HxButtonGroup_Size_RendersSizeClass(ButtonGroupSize size, string expectedCssClass)
	{
		// Arrange & Act
		var cut = Render<HxButtonGroup>(parameters => parameters
			.Add(p => p.Size, size));

		// Assert
		var group = cut.Find("div");
		Assert.Equal(expectedCssClass is null ? ["btn-group"] : ["btn-group", expectedCssClass], group.ClassList);
	}

	[Fact]
	public void HxButtonGroup_Divider_RendersDividerClass()
	{
		// Arrange & Act
		var cut = Render<HxButtonGroup>(parameters => parameters
			.Add(p => p.Divider, true));

		// Assert
		Assert.Contains("btn-group-divider", cut.Find("div").ClassList);
	}
}
