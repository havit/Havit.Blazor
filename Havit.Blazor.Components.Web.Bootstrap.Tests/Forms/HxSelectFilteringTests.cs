using System.Linq.Expressions;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

public class HxSelectFilteringTests : BunitTestBase
{
	private IRenderedComponent<HxSelect<string, string>> RenderSelect(List<string> items, Action<string> valueChanged = null, string value = null, Func<string, string, bool> filterPredicate = null, string nullText = null, bool? nullable = null)
	{
		Expression<Func<string>> valueExpression = () => value;

		return RenderComponent<HxSelect<string, string>>(parameters =>
		{
			parameters
				.Add(p => p.Data, items)
				.Add(p => p.Value, value)
				.Add(p => p.ValueChanged, v => valueChanged?.Invoke(v))
				.Add(p => p.ValueExpression, valueExpression)
				.Add(p => p.AllowFiltering, true)
				.Add(p => p.AutoSort, false);

			if (filterPredicate is not null)
			{
				parameters.Add(p => p.FilterPredicate, filterPredicate);
			}
			if (nullText is not null)
			{
				parameters.Add(p => p.NullText, nullText);
			}
			if (nullable is not null)
			{
				parameters.Add(p => p.Nullable, nullable);
			}
		});
	}

	[Fact]
	public void HxSelect_AllowFiltering_RendersComboboxMarkup()
	{
		// Arrange + Act
		var cut = RenderSelect(new List<string> { "Apple", "Banana", "Cherry" });

		// Assert — no native select is rendered
		Assert.Empty(cut.FindAll("select"));

		// Assert — combobox toggle (button) with the value and the caret
		var toggle = cut.Find("button.combobox-toggle");
		Assert.Contains("form-control", toggle.ClassList);
		Assert.Equal("listbox", toggle.GetAttribute("aria-haspopup"));
		Assert.Equal("false", toggle.GetAttribute("aria-expanded"));
		Assert.Equal("menu", toggle.GetAttribute("data-bs-toggle"));
		cut.Find("button.combobox-toggle .combobox-value");
		cut.Find("button.combobox-toggle svg.combobox-caret");

		// Assert — menu with the filter input and the items
		cut.Find(".menu .combobox-search input.combobox-search-input");
		var options = cut.FindAll(".menu .menu-item[role='option']");
		Assert.Equal(4, options.Count); // null item + 3 items
		Assert.Contains(options, o => o.TextContent.Contains("Apple"));
		Assert.Contains(options, o => o.TextContent.Contains("Banana"));
		Assert.Contains(options, o => o.TextContent.Contains("Cherry"));
	}

	[Fact]
	public void HxSelect_AllowFiltering_FilterNarrowsItems()
	{
		// Arrange
		var cut = RenderSelect(new List<string> { "Apple", "Banana", "Cherry" });

		// Act
		cut.Find(".combobox-search-input").Input("ban");

		// Assert — only "Banana" remains (+ the null item which is not filtered)
		var options = cut.FindAll(".menu .menu-item[role='option']");
		Assert.Equal(2, options.Count);
		Assert.Contains(options, o => o.TextContent.Contains("Banana"));
		Assert.DoesNotContain(options, o => o.TextContent.Contains("Apple"));
	}

	[Fact]
	public void HxSelect_AllowFiltering_NoFilterResults_RendersEmptyResultText()
	{
		// Arrange
		var cut = RenderSelect(new List<string> { "Apple", "Banana", "Cherry" });

		// Act
		cut.Find(".combobox-search-input").Input("xyz");

		// Assert
		cut.Find(".menu .combobox-no-results");
	}

	[Fact]
	public void HxSelect_AllowFiltering_SelectionSetsValueAndClosesMenu()
	{
		// Arrange
		string selectedValue = null;
		var cut = RenderSelect(new List<string> { "Apple", "Banana", "Cherry" }, v => selectedValue = v);

		// Act — click the "Banana" item
		var bananaItem = cut.FindAll(".menu .menu-item[role='option']").Single(o => o.TextContent.Contains("Banana"));
		bananaItem.Click();

		// Assert
		Assert.Equal("Banana", selectedValue);
		Assert.Equal("false", cut.Find("button.combobox-toggle").GetAttribute("aria-expanded"));
		Assert.DoesNotContain("show", cut.Find(".menu").ClassList);
	}

	[Fact]
	public void HxSelect_AllowFiltering_SelectedItem_RendersSelectedStateAndToggleText()
	{
		// Arrange + Act
		var cut = RenderSelect(new List<string> { "Apple", "Banana", "Cherry" }, value: "Banana");

		// Assert — the toggle displays the selected item text (not the placeholder styling)
		var toggleValue = cut.Find("button.combobox-toggle .combobox-value");
		Assert.Equal("Banana", toggleValue.TextContent.Trim());
		Assert.DoesNotContain("combobox-placeholder", toggleValue.ClassList);

		// Assert — the selected item is marked
		var selectedItems = cut.FindAll(".menu .menu-item.selected[aria-selected='true']");
		var selectedItem = Assert.Single(selectedItems);
		Assert.Contains("Banana", selectedItem.TextContent);
		Assert.NotNull(selectedItem.QuerySelector("svg.menu-item-check"));
	}

	[Fact]
	public void HxSelect_AllowFiltering_NullItem_RenderedFirstAndSelectsNull()
	{
		// Arrange
		string selectedValue = "Banana";
		var cut = RenderSelect(new List<string> { "Apple", "Banana", "Cherry" }, v => selectedValue = v, value: "Banana", nullable: true, nullText: "-select fruit-");

		// Assert — the null item is rendered first
		var options = cut.FindAll(".menu .menu-item[role='option']");
		Assert.Equal(4, options.Count);
		Assert.Equal("-select fruit-", options[0].TextContent.Trim());

		// Act — select the null item
		options[0].Click();

		// Assert
		Assert.Null(selectedValue);
	}

	[Fact]
	public void HxSelect_AllowFiltering_NoSelection_RendersNullTextAsPlaceholder()
	{
		// Arrange + Act
		var cut = RenderSelect(new List<string> { "Apple", "Banana" }, nullable: true, nullText: "-select fruit-");

		// Assert
		var toggleValue = cut.Find("button.combobox-toggle .combobox-value");
		Assert.Equal("-select fruit-", toggleValue.TextContent.Trim());
		Assert.Contains("combobox-placeholder", toggleValue.ClassList);
	}

	[Fact]
	public void HxSelect_AllowFiltering_FilterPredicateOverride_IsApplied()
	{
		// Arrange — custom predicate filtering by the last character
		var cut = RenderSelect(
			new List<string> { "Apple", "Banana", "Cherry" },
			filterPredicate: (item, filter) => item.EndsWith(filter, StringComparison.OrdinalIgnoreCase));

		// Act
		cut.Find(".combobox-search-input").Input("y");

		// Assert — only "Cherry" matches the custom predicate
		var options = cut.FindAll(".menu .menu-item[role='option']");
		Assert.Equal(2, options.Count); // null item + Cherry
		Assert.Contains(options, o => o.TextContent.Contains("Cherry"));
		Assert.DoesNotContain(options, o => o.TextContent.Contains("Banana"));
	}

	[Fact]
	public void HxSelect_AllowFilteringNotSet_RendersNativeSelect()
	{
		// Arrange
		string selectedValue = null;
		Expression<Func<string>> valueExpression = () => selectedValue;

		// Act
		var cut = RenderComponent<HxSelect<string, string>>(parameters => parameters
			.Add(p => p.Data, new List<string> { "Apple", "Banana" })
			.Add(p => p.Value, selectedValue)
			.Add(p => p.ValueChanged, value => selectedValue = value)
			.Add(p => p.ValueExpression, valueExpression)
			.Add(p => p.Nullable, true)
		);

		// Assert — the native select is rendered, no combobox markup
		cut.Find("select");
		Assert.Empty(cut.FindAll(".combobox-toggle"));
	}
}
