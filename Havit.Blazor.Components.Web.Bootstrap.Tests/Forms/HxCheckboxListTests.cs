namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

[TestClass]
public class HxCheckboxListTests : BunitTestBase
{
	[TestMethod]
	public void HxCheckboxList_Render_DisplaysItemsFromData()
	{
		// Arrange
		var data = new List<string> { "Alpha", "Beta", "Gamma" };
		List<string> selectedValues = new();

		// Act
		var cut = RenderComponent<HxCheckboxList<string, string>>(parameters => parameters
			.Add(p => p.Data, data)
			.Add(p => p.ItemTextSelector, item => item)
			.Add(p => p.ItemValueSelector, item => item)
			.Add(p => p.AutoSort, false)
			.Bind(p => p.Value, selectedValues, newValue => selectedValues = newValue)
		);

		// Assert - All three items should be rendered as checkboxes
		var checkboxInputs = cut.FindAll("input[type='checkbox']");
		Assert.HasCount(3, checkboxInputs);

		// Verify each item text is present in the rendered markup
		Assert.Contains("Alpha", cut.Markup);
		Assert.Contains("Beta", cut.Markup);
		Assert.Contains("Gamma", cut.Markup);
	}

	[TestMethod]
	public void HxCheckboxList_SelectMultiple_AllChecked()
	{
		// Arrange
		var data = new List<string> { "Alpha", "Beta", "Gamma" };
		List<string> selectedValues = new() { "Alpha", "Beta", "Gamma" };

		// Act
		var cut = RenderComponent<HxCheckboxList<string, string>>(parameters => parameters
			.Add(p => p.Data, data)
			.Add(p => p.ItemTextSelector, item => item)
			.Add(p => p.ItemValueSelector, item => item)
			.Add(p => p.AutoSort, false)
			.Bind(p => p.Value, selectedValues, newValue => selectedValues = newValue)
		);

		// Assert - All checkboxes should be checked
		var checkboxInputs = cut.FindAll("input[type='checkbox']");
		Assert.HasCount(3, checkboxInputs);
		foreach (var checkbox in checkboxInputs)
		{
			Assert.IsTrue(checkbox.HasAttribute("checked"), "All checkboxes should be checked when all values are selected.");
		}
	}

	[TestMethod]
	public void HxCheckboxList_DeselectItem_RemovesFromCollection()
	{
		// Arrange
		var data = new List<string> { "Alpha", "Beta", "Gamma" };
		List<string> selectedValues = new() { "Alpha", "Beta", "Gamma" };

		var cut = RenderComponent<HxCheckboxList<string, string>>(parameters => parameters
			.Add(p => p.Data, data)
			.Add(p => p.ItemTextSelector, item => item)
			.Add(p => p.ItemValueSelector, item => item)
			.Add(p => p.AutoSort, false)
			.Bind(p => p.Value, selectedValues, newValue => selectedValues = newValue)
		);

		// Act - Uncheck the second checkbox ("Beta")
		var checkboxInputs = cut.FindAll("input[type='checkbox']");
		checkboxInputs[1].Change(false);

		// Assert - "Beta" should be removed from the selected values
		Assert.HasCount(2, selectedValues);
		Assert.Contains("Alpha", selectedValues);
		Assert.DoesNotContain("Beta", selectedValues);
		Assert.Contains("Gamma", selectedValues);
	}

	[TestMethod]
	public void HxCheckboxList_BoundValue_MatchesSelection()
	{
		// Arrange
		var data = new List<string> { "Alpha", "Beta", "Gamma" };
		List<string> selectedValues = new();

		var cut = RenderComponent<HxCheckboxList<string, string>>(parameters => parameters
			.Add(p => p.Data, data)
			.Add(p => p.ItemTextSelector, item => item)
			.Add(p => p.ItemValueSelector, item => item)
			.Add(p => p.AutoSort, false)
			.Bind(p => p.Value, selectedValues, newValue => selectedValues = newValue)
		);

		// Act - Check the first and third checkboxes
		var checkboxInputs = cut.FindAll("input[type='checkbox']");
		checkboxInputs[0].Change(true);

		// Re-find after re-render
		checkboxInputs = cut.FindAll("input[type='checkbox']");
		checkboxInputs[2].Change(true);

		// Assert - Bound value should contain "Alpha" and "Gamma"
		Assert.HasCount(2, selectedValues);
		Assert.Contains("Alpha", selectedValues);
		Assert.Contains("Gamma", selectedValues);
		Assert.DoesNotContain("Beta", selectedValues);

		// Verify the component's Value property matches
		Assert.HasCount(2, cut.Instance.Value);
		Assert.Contains("Alpha", cut.Instance.Value);
		Assert.Contains("Gamma", cut.Instance.Value);
	}
}
