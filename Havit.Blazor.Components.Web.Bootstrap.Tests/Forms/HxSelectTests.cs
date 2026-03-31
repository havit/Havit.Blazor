using System.Linq.Expressions;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

[TestClass]
public class HxSelectTests : BunitTestBase
{
	[TestMethod]
	public void HxSelect_Render_DisplaysOptionsFromData()
	{
		// Arrange
		var items = new List<string> { "Apple", "Banana", "Cherry" };
		string selectedValue = null;
		Expression<Func<string>> valueExpression = () => selectedValue;

		// Act
		var cut = RenderComponent<HxSelect<string, string>>(parameters => parameters
			.Add(p => p.Data, items)
			.Add(p => p.Value, selectedValue)
			.Add(p => p.ValueChanged, value => selectedValue = value)
			.Add(p => p.ValueExpression, valueExpression)
			.Add(p => p.Nullable, true)
		);

		// Assert
		var options = cut.FindAll("select option");
		var optionTexts = options.Select(o => o.TextContent).ToList();
		Assert.Contains("Apple", optionTexts);
		Assert.Contains("Banana", optionTexts);
		Assert.Contains("Cherry", optionTexts);
	}

	[TestMethod]
	public void HxSelect_SelectOption_UpdatesBoundValue()
	{
		// Arrange
		var items = new List<string> { "Apple", "Banana", "Cherry" };
		string selectedValue = null;
		Expression<Func<string>> valueExpression = () => selectedValue;

		var cut = RenderComponent<HxSelect<string, string>>(parameters => parameters
			.Add(p => p.Data, items)
			.Add(p => p.Value, selectedValue)
			.Add(p => p.ValueChanged, value => selectedValue = value)
			.Add(p => p.ValueExpression, valueExpression)
			.Add(p => p.Nullable, true)
			.Add(p => p.AutoSort, false)
		);

		// The options are indexed: null option = -1, first item = 0, second = 1, ...
		var selectElement = cut.Find("select");

		// Act - select the second item (index 1 = "Banana" when AutoSort=false)
		selectElement.Change("1");

		// Assert
		Assert.AreEqual("Banana", selectedValue, "Bound value should be updated to the selected item");
	}

	[TestMethod]
	public void HxSelect_NullOption_RendersPlaceholder()
	{
		// Arrange
		var items = new List<string> { "Apple", "Banana" };
		string selectedValue = null;
		string nullText = "-- Please select --";
		Expression<Func<string>> valueExpression = () => selectedValue;

		// Act
		var cut = RenderComponent<HxSelect<string, string>>(parameters => parameters
			.Add(p => p.Data, items)
			.Add(p => p.Value, selectedValue)
			.Add(p => p.ValueChanged, value => selectedValue = value)
			.Add(p => p.ValueExpression, valueExpression)
			.Add(p => p.Nullable, true)
			.Add(p => p.NullText, nullText)
		);

		// Assert
		var options = cut.FindAll("select option");
		var nullOption = options.FirstOrDefault(o => o.GetAttribute("value") == "-1");
		Assert.IsNotNull(nullOption, "Null/placeholder option should be rendered");
		Assert.AreEqual(nullText, nullOption.TextContent, "Null option should display the configured placeholder text");
	}
}
