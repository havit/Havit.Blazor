namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxButtonGroupTests : BunitTestBase
{
	[Fact]
	public void HxButtonGroup_Render_GroupsButtonsHorizontally()
	{
		// Arrange & Act
		var cut = RenderComponent<HxButtonGroup>(parameters => parameters
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
}
