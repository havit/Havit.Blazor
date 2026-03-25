namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

[TestClass]
public class HxButtonGroupTests : BunitTestBase
{
	[TestMethod]
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
		Assert.IsTrue(group.ClassList.Contains("btn-group"), "Button group should have 'btn-group' CSS class");
		Assert.AreEqual("group", group.GetAttribute("role"));

		// Assert - both buttons should be rendered inside the group
		var buttons = cut.FindAll("button");
		Assert.HasCount(2, buttons, "Both buttons should be rendered inside the group");
	}
}
