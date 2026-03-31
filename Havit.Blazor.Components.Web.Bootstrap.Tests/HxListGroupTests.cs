namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

[TestClass]
public class HxListGroupTests : BunitTestBase
{
	[TestMethod]
	public void HxListGroup_Render_DisplaysAllItems()
	{
		// Act
		var cut = RenderComponent<HxListGroup>(parameters => parameters
			.AddChildContent<HxListGroupItem>(item => item
				.AddChildContent("First item"))
			.AddChildContent<HxListGroupItem>(item => item
				.AddChildContent("Second item"))
			.AddChildContent<HxListGroupItem>(item => item
				.AddChildContent("Third item"))
		);

		// Assert
		var items = cut.FindAll(".list-group-item");
		Assert.HasCount(3, items);
		Assert.AreEqual("First item", items[0].TextContent);
		Assert.AreEqual("Second item", items[1].TextContent);
		Assert.AreEqual("Third item", items[2].TextContent);
	}

	[TestMethod]
	public void HxListGroup_ClickItem_TriggersCallback()
	{
		// Arrange
		int clickCount = 0;

		var cut = RenderComponent<HxListGroup>(parameters => parameters
			.AddChildContent<HxListGroupItem>(item => item
				.Add(i => i.OnClick, () => clickCount++)
				.AddChildContent("Clickable item"))
		);

		// Act
		cut.Find(".list-group-item").Click();

		// Assert
		Assert.AreEqual(1, clickCount);
	}

	[TestMethod]
	public void HxListGroup_ActiveItem_HasActiveClass()
	{
		// Act
		var cut = RenderComponent<HxListGroup>(parameters => parameters
			.AddChildContent<HxListGroupItem>(item => item
				.Add(i => i.Active, false)
				.AddChildContent("Inactive item"))
			.AddChildContent<HxListGroupItem>(item => item
				.Add(i => i.Active, true)
				.AddChildContent("Active item"))
		);

		// Assert
		var items = cut.FindAll(".list-group-item");
		Assert.IsFalse(items[0].ClassList.Contains("active"), "First item should not have 'active' class");
		Assert.IsTrue(items[1].ClassList.Contains("active"), "Second item should have 'active' class");
	}
}
